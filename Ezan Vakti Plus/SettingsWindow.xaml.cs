using System;
using System.ComponentModel;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.Json;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media;
using Microsoft.Win32;

namespace Ezan_Vakti_Plus
{
    public partial class SettingsWindow : Window
    {
        private readonly AppSettings settings;
        private readonly Action saveSettings;

        private bool allowClose;

        private const string SettingsFolder = "settings";

        public SettingsWindow(AppSettings settings, Action saveSettings)
        {
            InitializeComponent();
            this.settings = settings ?? new AppSettings();
            this.saveSettings = saveSettings;

            StartupCheckBox.IsChecked = this.settings.StartWithWindows;
            MinimizeStartupCheckBox.IsChecked = this.settings.StartMinimized;

            UpdateMinimizeCheckBoxVisibility(this.settings.StartWithWindows);

            // İnternet durumu kontrolü
            UpdateInternetStatus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (FindResource("FadeInStoryboard") as Storyboard)?.Begin(MainGrid);
        }

        private void StartupCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            settings.StartWithWindows = true;
            StartupHelper.SetStartup(true);
            saveSettings?.Invoke();

            FadeInMinimizeCheckBox();
        }

        private void StartupCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            settings.StartWithWindows = false;
            StartupHelper.SetStartup(false);
            saveSettings?.Invoke();

            FadeOutMinimizeCheckBox();
        }

        private void MinimizeStartupCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (StartupCheckBox.IsChecked != true)
            {
                // Minimize seçeneği yalnızca Başlat tikliyse aktif
                MinimizeStartupCheckBox.IsChecked = false;
                return;
            }

            settings.StartMinimized = true;
            saveSettings?.Invoke();
        }

        private void MinimizeStartupCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            settings.StartMinimized = false;
            saveSettings?.Invoke();
        }

        private void FadeInMinimizeCheckBox()
        {
            MinimizeStartupCheckBox.Visibility = Visibility.Visible;
            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            MinimizeStartupCheckBox.BeginAnimation(OpacityProperty, fadeIn);
        }

        private void FadeOutMinimizeCheckBox()
        {
            var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(300));
            fadeOut.Completed += (s, e) =>
            {
                MinimizeStartupCheckBox.Visibility = Visibility.Collapsed;
                MinimizeStartupCheckBox.IsChecked = false; // Otomatik kaldır
                settings.StartMinimized = false;
                saveSettings?.Invoke();
            };
            MinimizeStartupCheckBox.BeginAnimation(OpacityProperty, fadeOut);
        }

        private void UpdateMinimizeCheckBoxVisibility(bool isVisible)
        {
            if (isVisible)
            {
                MinimizeStartupCheckBox.Visibility = Visibility.Visible;
                MinimizeStartupCheckBox.Opacity = 1;
            }
            else
            {
                MinimizeStartupCheckBox.Visibility = Visibility.Collapsed;
                MinimizeStartupCheckBox.Opacity = 0;
            }
        }

        private void UpdateInternetStatus()
        {
            bool isOnline = CheckInternetConnection();

            if (isOnline)
            {
                InternetStatusIcon.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/online.ico"));
                InternetStatusText.Text = "Çevrimiçi";
                InternetStatusText.Foreground = Brushes.LimeGreen;
            }
            else
            {
                InternetStatusIcon.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("pack://application:,,,/Resources/error.ico"));
                InternetStatusText.Text = "Çevrimdışı";
                InternetStatusText.Foreground = Brushes.Red;
            }
        }

        private bool CheckInternetConnection()
        {
            try
            {
                return NetworkInterface.GetIsNetworkAvailable();
            }
            catch
            {
                return false;
            }
        }

        private void NotificationSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            new NotificationSettingsWindow(settings, saveSettings) { Owner = this }.ShowDialog();
        }

        private void SaveSettingsDifferentButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SaveFileDialog
            {
                Title = "Ayarları Farklı Kaydet",
                Filter = "JSON Dosyaları (*.json)|*.json",
                InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SettingsFolder),
                FileName = "ayarlar.json"
            };

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    if (!Directory.Exists(SettingsFolder))
                        Directory.CreateDirectory(SettingsFolder);

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string json = JsonSerializer.Serialize(settings, options);

                    File.WriteAllText(dlg.FileName, json);
                    MessageBox.Show($"Ayarlar başarıyla '{dlg.FileName}' dosyasına kaydedildi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ayarları farklı kaydederken hata oluştu:\n" + ex.Message, "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

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

                        // Yeni yüklenen ayarlara göre UI güncelle
                        StartupCheckBox.IsChecked = settings.StartWithWindows;
                        MinimizeStartupCheckBox.IsChecked = settings.StartMinimized;
                        UpdateMinimizeCheckBoxVisibility(settings.StartWithWindows);

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

        private void CopySettings(AppSettings source, AppSettings target)
        {
            target.IsDarkMode = source.IsDarkMode;
            target.StartWithWindows = source.StartWithWindows;
            target.StartMinimized = source.StartMinimized;
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

        private void OKButton_Click(object sender, RoutedEventArgs e) => BeginFadeOutAndClose();

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (allowClose) return;
            e.Cancel = true;
            BeginFadeOutAndClose();
        }

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
