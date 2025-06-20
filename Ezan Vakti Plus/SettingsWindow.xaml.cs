using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace Ezan_Vakti_Plus
{
    public partial class SettingsWindow : Window
    {
        private readonly AppSettings settings;
        private readonly Action<bool> setStartup;
        private readonly Action saveSettings;

        private bool allowClose;

        public SettingsWindow(AppSettings settings, Action saveSettings)
        {
            InitializeComponent();
            this.settings     = settings ?? new AppSettings();
            this.saveSettings = saveSettings;

            // CheckBox ilk durum
            StartupCheckBox.IsChecked = this.settings.StartWithWindows;
        }

        /* ---------- Events ---------- */

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (FindResource("FadeInStoryboard") as Storyboard)?.Begin(MainGrid);
        }

        private void StartupCheckBox_Checked  (object s, RoutedEventArgs e)
        {
            settings.StartWithWindows = true;
            StartupHelper.SetStartup(true);
            saveSettings?.Invoke();
        }

        private void StartupCheckBox_Unchecked(object s, RoutedEventArgs e)
        {
            settings.StartWithWindows = false;
            StartupHelper.SetStartup(false);
            saveSettings?.Invoke();
        }

        private void NotificationSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            new NotificationSettingsWindow(settings, saveSettings) { Owner = this }.ShowDialog();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e) => BeginFadeOutAndClose();

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (allowClose) return;
            e.Cancel = true;
            BeginFadeOutAndClose();
        }

        /* ---------- Helpers ---------- */

        private void BeginFadeOutAndClose()
        {
            var fade = FindResource("FadeOutStoryboard") as Storyboard;
            if (fade == null) { allowClose = true; Close(); return; }

            fade.Completed += (_, __) =>
            {
                allowClose = true;
                Close();
            };
            fade.Begin(MainGrid);
        }
    }
}
