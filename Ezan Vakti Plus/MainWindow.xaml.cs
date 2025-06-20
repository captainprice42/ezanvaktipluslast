using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Media;

namespace Ezan_Vakti_Plus
{
    public partial class MainWindow : Window
    {
        
        
        private const string ProfileFilePath = "profiles/profile.json";

        // Burada senin önceden verdiğin Karatay ve Meram vakitleri:
        private static readonly Dictionary<int, string[]> KaratayVakitleri = new Dictionary<int, string[]>
        {
            // Örnek: 1 Ocak için vakitler
            { 1, new string[] { "03:37", "05:22", "12:52", "16:43", "20:11", "21:47" } },
            { 2, new string[] { "03:35", "05:21", "12:53", "16:43", "20:12", "21:48" } },
            { 3, new string[] { "03:35", "05:21", "12:53", "16:43", "20:13", "21:49" } },
            { 4, new string[] { "03:34", "05:20", "12:53", "16:43", "20:13", "21:50" } },
            { 5, new string[] { "03:34", "05:20", "12:53", "16:44", "20:14", "21:51" } },
            { 6, new string[] { "03:33", "05:20", "12:53", "16:44", "20:14", "21:51" } },
            { 7, new string[] { "03:33", "05:20", "12:53", "16:44", "20:14", "21:52" } },
            { 8, new string[] { "03:33", "05:20", "12:53", "16:44", "20:15", "21:52" } },
            { 9, new string[] { "03:33", "05:21", "12:54", "16:47", "20:18", "21:57" } },
            { 10, new string[] { "03:33", "05:20", "12:54", "16:47", "20:19", "21:58" } },
            { 11, new string[] { "03:33", "05:20", "12:55", "16:48", "20:19", "21:59" } },
            { 12, new string[] { "03:33", "05:20", "12:55", "16:48", "20:19", "21:59" } },
            { 13, new string[] { "03:32", "05:20", "12:55", "16:48", "20:20", "22:00" } },
            { 14, new string[] { "03:32", "05:20", "12:55", "16:48", "20:20", "22:00" } },
            { 15, new string[] { "03:32", "05:20", "12:55", "16:49", "20:21", "22:01" } },
            { 16, new string[] { "03:32", "05:20", "12:56", "16:49", "20:21", "22:01" } },
            { 17, new string[] { "03:32", "05:20", "12:56", "16:49", "20:21", "22:02" } },
            { 18, new string[] { "03:32", "05:21", "12:56", "16:49", "20:22", "22:02" } },
            { 19, new string[] { "03:32", "05:21", "12:56", "16:49", "20:22", "22:03" } },
            { 20, new string[] { "03:32", "05:21", "12:57", "16:50", "20:22", "22:03" } },
            { 21, new string[] { "03:32", "05:21", "12:57", "16:50", "20:23", "22:03" } },
            { 22, new string[] { "03:32", "05:21", "12:57", "16:50", "20:23", "22:03" } },
            { 23, new string[] { "03:32", "05:22", "12:57", "16:50", "20:23", "22:03" } },
            { 24, new string[] { "03:32", "05:22", "12:57", "16:51", "20:23", "22:04" } },
            { 25, new string[] { "03:33", "05:22", "12:58", "16:51", "20:23", "22:04" } },
            { 26, new string[] { "03:33", "05:22", "12:58", "16:51", "20:23", "22:04" } },
            { 27, new string[] { "03:33", "05:23", "12:58", "16:51", "20:23", "22:04" } },
            { 28, new string[] { "03:33", "05:23", "12:58", "16:51", "20:23", "22:04" } },
            { 29, new string[] { "03:34", "05:23", "12:58", "16:51", "20:23", "22:04" } },
            { 30, new string[] { "03:34", "05:23", "12:58", "16:51", "20:23", "22:04" } }
        };

        private Dictionary<int, string[]> MeramVakitleri = new Dictionary<int, string[]>
        {
            { 1, new string[] { "03:40", "05:25", "12:55", "16:45", "20:14", "21:49" } },
            { 2, new string[] { "03:39", "05:24", "12:56", "16:46", "20:15", "21:50" } },
            { 3, new string[] { "03:38", "05:24", "12:56", "16:46", "20:16", "21:52" } },
            { 4, new string[] { "03:38", "05:24", "12:56", "16:46", "20:16", "21:52" } },
            { 5, new string[] { "03:37", "05:23", "12:56", "16:46", "20:17", "21:53" } },
            { 6, new string[] { "03:37", "05:23", "12:56", "16:47", "20:17", "21:54" } },
            { 7, new string[] { "03:36", "05:23", "12:56", "16:47", "20:18", "21:55" } },
            { 8, new string[] { "03:35", "05:23", "12:57", "16:47", "20:19", "21:56" } },
            { 9, new string[] { "03:35", "05:22", "12:57", "16:47", "20:19", "21:56" } },
            { 10, new string[] { "03:35", "05:22", "12:57", "16:47", "20:20", "21:58" } },
            { 11, new string[] { "03:34", "05:22", "12:57", "16:48", "20:20", "21:58" } },
            { 12, new string[] { "03:34", "05:22", "12:57", "16:48", "20:20", "21:59" } },
            { 13, new string[] { "03:34", "05:22", "12:58", "16:48", "20:21", "21:59" } },
            { 14, new string[] { "03:34", "05:22", "12:58", "16:48", "20:22", "21:59" } },
            { 15, new string[] { "03:34", "05:22", "12:58", "16:49", "20:22", "22:01" } },
            { 16, new string[] { "03:34", "05:22", "12:58", "16:49", "20:22", "22:01" } },
            { 17, new string[] { "03:34", "05:22", "12:58", "16:49", "20:23", "22:01" } },
            { 18, new string[] { "03:34", "05:22", "12:58", "16:49", "20:23", "22:02" } },
            { 19, new string[] { "03:34", "05:22", "12:58", "16:49", "20:23", "22:02" } },
            { 20, new string[] { "03:34", "05:22", "12:59", "16:50", "20:23", "22:02" } },
            { 21, new string[] { "03:34", "05:22", "12:59", "16:50", "20:24", "22:03" } },
            { 22, new string[] { "03:34", "05:22", "12:59", "16:50", "20:24", "22:03" } },
            { 23, new string[] { "03:34", "05:22", "12:59", "16:50", "20:24", "22:03" } },
            { 24, new string[] { "03:35", "05:22", "12:59", "16:51", "20:24", "22:04" } },
            { 25, new string[] { "03:35", "05:22", "12:59", "16:51", "20:24", "22:04" } },
            { 26, new string[] { "03:35", "05:22", "12:59", "16:51", "20:24", "22:04" } },
            { 27, new string[] { "03:35", "05:23", "12:59", "16:51", "20:24", "22:04" } },
            { 28, new string[] { "03:35", "05:23", "12:59", "16:51", "20:24", "22:04" } },
            { 29, new string[] { "03:35", "05:23", "12:59", "16:51", "20:24", "22:04" } },
            { 30, new string[] { "03:35", "05:23", "12:59", "16:51", "20:24", "22:04" } }
        };

        private int lastNotifiedIndex = -1;
        private readonly Brush activeBackground = new SolidColorBrush(Color.FromRgb(221, 111, 0));
        private readonly Brush activeForeground = Brushes.White;
        private readonly Brush inactiveBackground = Brushes.Transparent;
        private readonly Brush inactiveForeground = Brushes.White;
        private readonly Brush lightInactiveBackground = new SolidColorBrush(Color.FromRgb(224, 224, 224));

        private Dictionary<int, string[]> currentVakitler;
        private Button currentActiveButton;

        private DispatcherTimer countdownTimer;
        private DispatcherTimer marqueeTimer;
        private double marqueeX;

        private SoundPlayer notificationSound;

        private AppSettings settings;

        public MainWindow()
        {
            
            Loaded += async (_, __) => await VersionChecker.CheckAsync(this);
            

            
            InitializeComponent();

            currentVakitler = KaratayVakitleri;
            currentActiveButton = btnKaratay;

            LoadSettings();
            ApplyTheme();
            UpdateButtonStyles();
            UpdatePrayerTimes(DateTime.Now.Day);

            StartCountdownTimer();
            StartMarquee();

            try
            {
                notificationSound = new SoundPlayer("Ses/bildirim.wav");
                notificationSound.Load();
            }
            catch
            {
                notificationSound = null;
            }


        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(ProfileFilePath))
                {
                    string json = File.ReadAllText(ProfileFilePath);
                    settings = JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
                }
                else
                {
                    settings = new AppSettings();
                    SaveSettings();
                }
            }
            catch
            {
                settings = new AppSettings();
            }
        }

        private void SaveSettings()
        {
            try
            {
                var dir = Path.GetDirectoryName(ProfileFilePath);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ProfileFilePath, json);
            }
            catch
            {
                // Sessizce devam et
            }
        }

        private void ApplyTheme()
        {
            if (settings.IsDarkMode)
            {
                Background = new SolidColorBrush(Color.FromRgb(18, 18, 18));
                timesPanel.Children.OfType<TextBlock>().ToList().ForEach(tb => tb.Foreground = Brushes.White);
                marqueeText.Foreground = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                countdownLabel.Foreground = new SolidColorBrush(Color.FromRgb(255, 165, 0));
                PrayerTimesBorder.Background = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                btnKaratay.Background = activeBackground;
                btnMeram.Background = inactiveBackground;
                btnMeram.Foreground = inactiveForeground;
            }
            else
            {
                Background = Brushes.White;
                timesPanel.Children.OfType<TextBlock>().ToList()
                    .ForEach(tb => tb.Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51)));
                marqueeText.Foreground = new SolidColorBrush(Color.FromRgb(221, 111, 0));
                countdownLabel.Foreground = new SolidColorBrush(Color.FromRgb(221, 111, 0));
                PrayerTimesBorder.Background = new SolidColorBrush(Color.FromRgb(240, 240, 240));
                btnKaratay.Background = activeBackground;
                btnMeram.Background = lightInactiveBackground;
                btnMeram.Foreground = new SolidColorBrush(Color.FromRgb(51, 51, 51));
            }
        }

        private void SetStartup(bool enable)
        {
            try
            {
                using var rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (enable)
                    rk.SetValue("EzanVaktiPlus", System.Reflection.Assembly.GetExecutingAssembly().Location);
                else
                    rk.DeleteValue("EzanVaktiPlus", false);
            }
            catch
            {
                // Sessizce devam et
            }
        }

        private void UpdateButtonStyles()
        {
            if (currentActiveButton == btnKaratay)
            {
                btnKaratay.Background = activeBackground;
                btnKaratay.Foreground = activeForeground;
                btnMeram.Background = settings.IsDarkMode ? inactiveBackground : lightInactiveBackground;
                btnMeram.Foreground = settings.IsDarkMode ? inactiveForeground : new SolidColorBrush(Color.FromRgb(51, 51, 51));
            }
            else
            {
                btnMeram.Background = activeBackground;
                btnMeram.Foreground = activeForeground;
                btnKaratay.Background = settings.IsDarkMode ? inactiveBackground : lightInactiveBackground;
                btnKaratay.Foreground = settings.IsDarkMode ? inactiveForeground : new SolidColorBrush(Color.FromRgb(51, 51, 51));
            }
        }

        private void btnKaratay_Click(object sender, RoutedEventArgs e)
        {
            if (currentActiveButton == btnKaratay) return;

            currentActiveButton = btnKaratay;
            UpdateButtonStyles();
            AnimatePrayerTimeChange(() => UpdatePrayerTimes(DateTime.Now.Day));
        }

        private void btnMeram_Click(object sender, RoutedEventArgs e)
        {
            if (currentActiveButton == btnMeram) return;

            currentActiveButton = btnMeram;
            UpdateButtonStyles();
            AnimatePrayerTimeChange(() => UpdatePrayerTimes(DateTime.Now.Day));
        }

        private void UpdatePrayerTimes(int day)
        {
            currentVakitler = currentActiveButton == btnKaratay ? KaratayVakitleri : MeramVakitleri;

            if (!currentVakitler.TryGetValue(day, out var vakitler))
            {
                SetPrayerLabels("--:--");
                return;
            }

            SetPrayerLabels(vakitler);
        }

        private void SetPrayerLabels(string[] vakitler)
        {
            imsakLabel.Text = $"İmsak: {vakitler[0]}";
            gunesLabel.Text = $"Güneş: {vakitler[1]}";
            ogleLabel.Text = $"Öğle: {vakitler[2]}";
            ikindiLabel.Text = $"İkindi: {vakitler[3]}";
            aksamLabel.Text = $"Akşam: {vakitler[4]}";
            yatsiLabel.Text = $"Yatsı: {vakitler[5]}";
        }

        private void SetPrayerLabels(string defaultText)
        {
            imsakLabel.Text = $"İmsak: {defaultText}";
            gunesLabel.Text = $"Güneş: {defaultText}";
            ogleLabel.Text = $"Öğle: {defaultText}";
            ikindiLabel.Text = $"İkindi: {defaultText}";
            aksamLabel.Text = $"Akşam: {defaultText}";
            yatsiLabel.Text = $"Yatsı: {defaultText}";
        }

        private void AnimatePrayerTimeChange(Action updateAction)
        {
            var fadeOut = (Storyboard)FindResource("FadeOutStoryboard");
            var fadeIn = (Storyboard)FindResource("FadeInStoryboard");

            fadeOut.Completed += (s, e) =>
            {
                updateAction();
                fadeIn.Begin(timesPanel);
            };

            fadeOut.Begin(timesPanel);
        }

        private void StartCountdownTimer()
        {
            countdownTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            countdownTimer.Tick += CountdownTimer_Tick;
            countdownTimer.Start();
        }
        
        
        string GetYonelmeEki(string vakit)
        {
            switch (vakit)
            {
                case "İmsak":
                case "Akşam":
                    return "'a";
                case "Güneş":
                    return "'e";
                case "Öğle":
                case "İkindi":
                    return "'ye";
                case "Yatsı":
                    return "'ya";
                default:
                    return "'a"; // Genel varsayılan
            }
        }


        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;

            if (!currentVakitler.TryGetValue(now.Day, out var vakitler))
            {
                countdownLabel.Text = "Namaz vakitleri bulunamadı.";
                return;
            }

            string[] namazlar = { "İmsak", "Güneş", "Öğle", "İkindi", "Akşam", "Yatsı" };
            int[] notificationMinutes =
            {
                settings.ImsakNotificationMinutes,
                settings.GunesNotificationMinutes,
                settings.OgleNotificationMinutes,
                settings.IkindiNotificationMinutes,
                settings.AksamNotificationMinutes,
                settings.YatsiNotificationMinutes
            };

            for (int i = 0; i < vakitler.Length; i++)
            {
                if (!TimeSpan.TryParse(vakitler[i], out var namazTime))
                    continue;

                var namazDateTime = now.Date + namazTime;
                var diff = namazDateTime - now;

                if (diff.TotalSeconds > 0)
                {
                    string yonelmeEki = GetYonelmeEki(namazlar[i]);
                    countdownLabel.Text = $"{namazlar[i]}{yonelmeEki} kalan süre: {diff.Hours:D2}:{diff.Minutes:D2}:{diff.Seconds:D2}";

                    if (diff.TotalMinutes <= notificationMinutes[i] && lastNotifiedIndex != i)
                    {
                        PlayNotificationSound(i);
                        lastNotifiedIndex = i;
                    }

                    return;
                }
            }

            countdownLabel.Text = "Bugünkü namaz vakitleri tamamlandı.";
            lastNotifiedIndex = -1;
        }


        private void PlayNotificationSound(int vakitIndex)
        {
            string soundFile = vakitIndex switch
            {
                0 => settings.ImsakNotificationSoundFile,
                1 => settings.GunesNotificationSoundFile,
                2 => settings.OgleNotificationSoundFile,
                3 => settings.IkindiNotificationSoundFile,
                4 => settings.AksamNotificationSoundFile,
                5 => settings.YatsiNotificationSoundFile,
                _ => null
            };

            if (!string.IsNullOrEmpty(soundFile) && File.Exists(soundFile))
            {
                try
                {
                    var player = new System.Media.SoundPlayer(soundFile);
                    player.Play();
                }
                catch
                {
                    // Hata varsa yut
                }
            }
        }


        private void StartMarquee()
        {
            marqueeX = marqueeCanvas.ActualWidth;
            Canvas.SetLeft(marqueeText, marqueeX);

            marqueeTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(20)
            };
            marqueeTimer.Tick += MarqueeTimer_Tick;
            marqueeTimer.Start();

            marqueeCanvas.SizeChanged += (s, e) =>
            {
                marqueeX = marqueeCanvas.ActualWidth;
                Canvas.SetLeft(marqueeText, marqueeX);
            };
        }

        private void MarqueeTimer_Tick(object sender, EventArgs e)
        {
            double left = Canvas.GetLeft(marqueeText);
            left -= 2;

            if (left + marqueeText.ActualWidth < 0)
                left = marqueeCanvas.ActualWidth;

            Canvas.SetLeft(marqueeText, left);
        }

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(settings, SaveSettings);
            settingsWindow.Owner = this;
            settingsWindow.ShowDialog();
        }
    }
}
