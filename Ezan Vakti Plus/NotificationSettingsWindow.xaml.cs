using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using System.IO;

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
        
        private void BrowseSound(string namazAdi)
        {
            var dlg = new OpenFileDialog();
            dlg.Filter = "WAV Dosyaları (*.wav)|*.wav";
            if (dlg.ShowDialog() == true)
            {
                string sourceFile = dlg.FileName;

                string appDir = AppDomain.CurrentDomain.BaseDirectory;
                string sesKlasoru = Path.Combine(appDir, "Ses");

                if (!Directory.Exists(sesKlasoru))
                    Directory.CreateDirectory(sesKlasoru);

                string targetFile = Path.Combine(sesKlasoru, $"{namazAdi.ToLower()}.wav");

                // Orijinal dosyayı silmeden kopyala (üzerine yaz)
                File.Copy(sourceFile, targetFile, true);

                // Ayarlara kaydet (yol değil sadece isim - cünkü sabit klasörde)
                switch (namazAdi)
                {
                    case "İmsak":
                        settings.ImsakNotificationSoundFile = targetFile;
                        break;
                    case "Güneş":
                        settings.GunesNotificationSoundFile = targetFile;
                        break;
                    case "Öğle":
                        settings.OgleNotificationSoundFile = targetFile;
                        break;
                    case "İkindi":
                        settings.IkindiNotificationSoundFile = targetFile;
                        break;
                    case "Akşam":
                        settings.AksamNotificationSoundFile = targetFile;
                        break;
                    case "Yatsı":
                        settings.YatsiNotificationSoundFile = targetFile;
                        break;
                }
                saveSettings?.Invoke();

                MessageBox.Show($"{namazAdi} için ses dosyası başarıyla yüklendi.", "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        
        
        
        
        
        private void BrowseImsak_Click(object sender, RoutedEventArgs e) => BrowseSound("İmsak");
        private void BrowseGunes_Click(object sender, RoutedEventArgs e) => BrowseSound("Güneş");
        private void BrowseOgle_Click(object sender, RoutedEventArgs e) => BrowseSound("Öğle");
        private void BrowseIkindi_Click(object sender, RoutedEventArgs e) => BrowseSound("İkindi");
        private void BrowseAksam_Click(object sender, RoutedEventArgs e) => BrowseSound("Akşam");
        private void BrowseYatsi_Click(object sender, RoutedEventArgs e) => BrowseSound("Yatsı");

        
        

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
