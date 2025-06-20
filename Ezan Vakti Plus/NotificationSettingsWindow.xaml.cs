using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;

namespace Ezan_Vakti_Plus
{
    public partial class NotificationSettingsWindow : Window
    {
        private readonly AppSettings settings;
        private readonly Action saveSettings;
        private bool allowClose;

        public NotificationSettingsWindow(AppSettings settings, Action saveSettings)
        {
            InitializeComponent();
            this.settings     = settings ?? new AppSettings();
            this.saveSettings = saveSettings;
            LoadValues();
        }

        private void LoadValues()
        {
            ImsakTextBox.Text  = settings.ImsakNotificationMinutes.ToString();
            GunesTextBox.Text  = settings.GunesNotificationMinutes.ToString();
            OgleTextBox.Text   = settings.OgleNotificationMinutes.ToString();
            IkindiTextBox.Text = settings.IkindiNotificationMinutes.ToString();
            AksamTextBox.Text  = settings.AksamNotificationMinutes.ToString();
            YatsiTextBox.Text  = settings.YatsiNotificationMinutes.ToString();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ((Storyboard)FindResource("FadeInStoryboard")).Begin(RootGrid);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateAndSave()) return;

            // Fade‑out + kapat
            var sb = (Storyboard)FindResource("FadeOutStoryboard");
            sb.Completed += (_, __) => { allowClose = true; Close(); };
            sb.Begin(RootGrid);
        }

        // ‑‑‑ Yardımcılar ‑‑‑
        private bool ValidateAndSave()
        {
            if (!TrySet(ImsakTextBox,  v => settings.ImsakNotificationMinutes  = v)) return false;
            if (!TrySet(GunesTextBox,  v => settings.GunesNotificationMinutes  = v)) return false;
            if (!TrySet(OgleTextBox,   v => settings.OgleNotificationMinutes   = v)) return false;
            if (!TrySet(IkindiTextBox, v => settings.IkindiNotificationMinutes = v)) return false;
            if (!TrySet(AksamTextBox,  v => settings.AksamNotificationMinutes  = v)) return false;
            if (!TrySet(YatsiTextBox,  v => settings.YatsiNotificationMinutes  = v)) return false;

            saveSettings?.Invoke();   // settings.json güncelle
            return true;
        }

        private static bool TrySet(System.Windows.Controls.TextBox box, Action<int> setter)
        {
            if (int.TryParse(box.Text, out int tmp) && tmp >= 0)
            {
                setter(tmp);
                return true;
            }

            MessageBox.Show($"{box.Name.Replace("TextBox","")} için geçerli bir sayı girin.",
                            "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (allowClose) return;          // Fade‑out’tan geliyorsak gerçek kapatma

            e.Cancel = true;                // Aksi halde kapatmayı iptal et ve animasyon başlat
            var sb = (Storyboard)FindResource("FadeOutStoryboard");
            sb.Completed += (_, __) => { allowClose = true; Close(); };
            sb.Begin(RootGrid);
        }
    }
}
