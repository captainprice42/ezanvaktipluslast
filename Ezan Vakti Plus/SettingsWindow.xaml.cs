using System;
using System.ComponentModel;
using System.IO;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Animation;
using Microsoft.Win32;

namespace Ezan_Vakti_Plus
{
    public partial class SettingsWindow : Window
    {
        private readonly AppSettings settings;
        private readonly Action<bool> setStartup; // Senin orijinalde vardı, kullanmadığın yerde tutuyorum
        private readonly Action saveSettings;

        private bool allowClose;

        private const string SettingsFolder = "settings";

        public SettingsWindow(AppSettings settings, Action saveSettings)
        {
            InitializeComponent();
            this.settings = settings ?? new AppSettings();
            this.saveSettings = saveSettings;

            // CheckBox ilk durum
            StartupCheckBox.IsChecked = this.settings.StartWithWindows;
        }

        /* ---------- Events ---------- */

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (FindResource("FadeInStoryboard") as Storyboard)?.Begin(MainGrid);
        }

        private void StartupCheckBox_Checked(object s, RoutedEventArgs e)
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

        // ---------- Yeni Eklendi: Ayarları Farklı Kaydet ----------
        private void SaveSettingsDifferentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Directory.Exists(SettingsFolder))
                    Directory.CreateDirectory(SettingsFolder);

                int index = 1;
                string filePath;

                do
                {
                    filePath = Path.Combine(SettingsFolder, $"{index}.json");
                    index++;
                } while (File.Exists(filePath));

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, options);

                File.WriteAllText(filePath, json);
                MessageBox.Show($"Ayarlar başarıyla '{filePath}' dosyasına kaydedildi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ayarları farklı kaydederken hata oluştu:\n" + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ---------- Yeni Eklendi: Ayarları Yükle ----------
        private void LoadSettingsDifferentButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                Title = "Ayar Dosyası Seç",
                Filter = "JSON Dosyaları (*.json)|*.json",
                InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFolder)
            };

            if (dlg.ShowDialog() == true)
            {
                try
                {
                    string json = File.ReadAllText(dlg.FileName);

                    var options = new JsonSerializerOptions
                    {
                        AllowTrailingCommas = true,
                        ReadCommentHandling = JsonCommentHandling.Skip,
                        PropertyNameCaseInsensitive = true
                    };

                    var loadedSettings = JsonSerializer.Deserialize<AppSettings>(json, options);

                    if (loadedSettings != null)
                    {
                        CopySettings(loadedSettings, settings);
                        saveSettings?.Invoke();

                        MessageBox.Show($"Ayarlar '{Path.GetFileName(dlg.FileName)}' dosyasından yüklendi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Seçilen dosya geçerli bir ayar dosyası değil.", "Hata", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ayarları yüklerken hata oluştu:\n" + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Ayar nesnelerini kopyalıyor, referans değil değer değiştiriyor
        private void CopySettings(AppSettings source, AppSettings target)
        {
            target.IsDarkMode = source.IsDarkMode;
            target.StartWithWindows = source.StartWithWindows;
            target.ImsakNotificationMinutes = source.ImsakNotificationMinutes;
            target.GunesNotificationMinutes = source.GunesNotificationMinutes;
            target.OgleNotificationMinutes = source.OgleNotificationMinutes;
            target.IkindiNotificationMinutes = source.IkindiNotificationMinutes;
            target.AksamNotificationMinutes = source.AksamNotificationMinutes;
            target.YatsiNotificationMinutes = source.YatsiNotificationMinutes;

            target.ImsakNotificationSoundFile = source.ImsakNotificationSoundFile;
            target.GunesNotificationSoundFile = source.GunesNotificationSoundFile;
            target.OgleNotificationSoundFile = source.OgleNotificationSoundFile;
            target.IkindiNotificationSoundFile = source.IkindiNotificationSoundFile;
            target.AksamNotificationSoundFile = source.AksamNotificationSoundFile;
            target.YatsiNotificationSoundFile = source.YatsiNotificationSoundFile;
        }
    }
}
