using Windows.ApplicationModel.Activation;
using System.Runtime.InteropServices;
using CommunityToolkit.WinUI.Notifications;
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
        private readonly string MainSettingsFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "main.json");
        
        

        // Burada senin önceden verdiğin Karatay ve Meram vakitleri:
        private static readonly Dictionary<int, string[]> MeramVakitleri = new Dictionary<int, string[]>
{
    {1, new[]{ "05:18", "06:39", "12:45", "16:04", "18:41", "19:56" }},
    {2, new[]{ "05:19", "06:39", "12:45", "16:03", "18:40", "19:55" }},
    {3, new[]{ "05:20", "06:40", "12:44", "16:02", "18:38", "19:53" }},
    {4, new[]{ "05:21", "06:41", "12:44", "16:01", "18:37", "19:52" }},
    {5, new[]{ "05:22", "06:42", "12:44", "16:00", "18:35", "19:50" }},
    {6, new[]{ "05:23", "06:43", "12:43", "15:59", "18:34", "19:49" }},
    {7, new[]{ "05:24", "06:44", "12:43", "15:58", "18:32", "19:47" }},
    {8, new[]{ "05:25", "06:45", "12:43", "15:57", "18:31", "19:46" }},
    {9, new[]{ "05:26", "06:46", "12:42", "15:56", "18:29", "19:44" }},
    {10, new[]{ "05:27", "06:47", "12:42", "15:54", "18:28", "19:43" }},
    {11, new[]{ "05:27", "06:48", "12:42", "15:53", "18:26", "19:41" }},
    {12, new[]{ "05:28", "06:49", "12:42", "15:52", "18:25", "19:40" }},
    {13, new[]{ "05:29", "06:50", "12:41", "15:51", "18:23", "19:38" }},
    {14, new[]{ "05:30", "06:51", "12:41", "15:50", "18:22", "19:37" }},
    {15, new[]{ "05:31", "06:51", "12:41", "15:49", "18:20", "19:36" }},
    {16, new[]{ "05:32", "06:52", "12:41", "15:48", "18:19", "19:34" }},
    {17, new[]{ "05:33", "06:53", "12:40", "15:47", "18:18", "19:33" }},
    {18, new[]{ "05:34", "06:54", "12:40", "15:46", "18:16", "19:32" }},
    {19, new[]{ "05:35", "06:55", "12:40", "15:45", "18:15", "19:30" }},
    {20, new[]{ "05:36", "06:56", "12:40", "15:44", "18:14", "19:29" }},
    {21, new[]{ "05:37", "06:57", "12:40", "15:43", "18:12", "19:28" }},
    {22, new[]{ "05:37", "06:58", "12:40", "15:42", "18:11", "19:27" }},
    {23, new[]{ "05:38", "06:59", "12:39", "15:41", "18:10", "19:25" }},
    {24, new[]{ "05:39", "07:00", "12:39", "15:40", "18:08", "19:24" }},
    {25, new[]{ "05:40", "07:01", "12:39", "15:39", "18:07", "19:23" }},
    {26, new[]{ "05:41", "07:02", "12:39", "15:38", "18:06", "19:22" }},
    {27, new[]{ "05:42", "07:03", "12:39", "15:37", "18:05", "19:21" }},
    {28, new[]{ "05:43", "07:04", "12:39", "15:36", "18:03", "19:20" }},
    {29, new[]{ "05:44", "07:05", "12:39", "15:35", "18:02", "19:19" }},
    {30, new[]{ "05:45", "07:06", "12:39", "15:34", "18:01", "19:18" }},
    {31, new[]{ "05:46", "07:07", "12:39", "15:33", "18:00", "19:17" }}
};

private static readonly Dictionary<int, string[]> KaratayVakitleri = new Dictionary<int, string[]>
{
    {1, new[]{ "05:18", "06:39", "12:45", "16:04", "18:41", "19:56" }},
    {2, new[]{ "05:19", "06:39", "12:44", "16:03", "18:39", "19:55" }},
    {3, new[]{ "05:20", "06:40", "12:44", "16:02", "18:38", "19:53" }},
    {4, new[]{ "05:21", "06:41", "12:44", "16:01", "18:36", "19:52" }},
    {5, new[]{ "05:22", "06:42", "12:44", "16:00", "18:35", "19:50" }},
    {6, new[]{ "05:23", "06:43", "12:43", "15:59", "18:33", "19:49" }},
    {7, new[]{ "05:24", "06:44", "12:43", "15:58", "18:32", "19:47" }},
    {8, new[]{ "05:25", "06:45", "12:43", "15:57", "18:30", "19:46" }},
    {9, new[]{ "05:26", "06:46", "12:42", "15:56", "18:29", "19:44" }},
    {10, new[]{ "05:26", "06:47", "12:42", "15:54", "18:28", "19:43" }},
    {11, new[]{ "05:27", "06:48", "12:42", "15:53", "18:26", "19:41" }},
    {12, new[]{ "05:28", "06:49", "12:42", "15:52", "18:25", "19:40" }},
    {13, new[]{ "05:29", "06:49", "12:41", "15:51", "18:23", "19:38" }},
    {14, new[]{ "05:30", "06:50", "12:41", "15:50", "18:22", "19:37" }},
    {15, new[]{ "05:31", "06:51", "12:41", "15:49", "18:20", "19:36" }},
    {16, new[]{ "05:32", "06:52", "12:41", "15:48", "18:19", "19:34" }},
    {17, new[]{ "05:33", "06:53", "12:40", "15:47", "18:18", "19:33" }},
    {18, new[]{ "05:34", "06:54", "12:40", "15:46", "18:16", "19:32" }},
    {19, new[]{ "05:35", "06:55", "12:40", "15:45", "18:15", "19:30" }},
    {20, new[]{ "05:36", "06:56", "12:40", "15:44", "18:14", "19:29" }},
    {21, new[]{ "05:36", "06:57", "12:40", "15:43", "18:12", "19:28" }},
    {22, new[]{ "05:37", "06:58", "12:40", "15:42", "18:11", "19:27" }},
    {23, new[]{ "05:38", "06:59", "12:39", "15:41", "18:10", "19:25" }},
    {24, new[]{ "05:39", "07:00", "12:39", "15:40", "18:08", "19:24" }},
    {25, new[]{ "05:40", "07:01", "12:39", "15:39", "18:07", "19:23" }},
    {26, new[]{ "05:41", "07:02", "12:39", "15:38", "18:06", "19:22" }},
    {27, new[]{ "05:42", "07:03", "12:39", "15:37", "18:05", "19:21" }},
    {28, new[]{ "05:43", "07:04", "12:39", "15:36", "18:03", "19:20" }},
    {29, new[]{ "05:44", "07:05", "12:39", "15:35", "18:02", "19:19" }},
    {30, new[]{ "05:45", "07:06", "12:39", "15:34", "18:01", "19:18" }},
    {31, new[]{ "05:46", "07:07", "12:39", "15:33", "18:00", "19:17" }}
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
            
            UpdateMonthlyVerse();

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
        
        

        
        private void LoadLastSelectedRegion()
        {
            switch (settings.LastSelectedRegionIndex)
            {
                case 0:
                    btnKaratay_Click(btnKaratay, null);
                    break;
                case 1:
                    btnMeram_Click(btnMeram, null);
                    break;
                // Ek bölge varsa onları da buraya ekle
                default:
                    btnKaratay_Click(btnKaratay, null);
                    break;
            }
        }


        private void LoadSettings()
        {
            try
            {
                if (File.Exists(MainSettingsFilePath))
                {
                    string json = File.ReadAllText(MainSettingsFilePath);
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
                var dir = Path.GetDirectoryName(MainSettingsFilePath);
                if (!Directory.Exists(dir) && dir != null)
                    Directory.CreateDirectory(dir);

                string json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(MainSettingsFilePath, json);
                Logger.Log("Ayarlar başarıyla kaydedildi.");
            }
            catch
            {
                // Sessiz devam et
            }
        }

// Dışarıdan başka bir ayar dosyası yüklenirse:
        private void LoadProfileAndSetAsMain(string profilePath)
        {
            try
            {
                if (!File.Exists(profilePath))
                    throw new FileNotFoundException("Profil dosyası bulunamadı.", profilePath);

                string json = File.ReadAllText(profilePath);
                var loadedSettings = JsonSerializer.Deserialize<AppSettings>(json);
                if (loadedSettings == null)
                    throw new Exception("Geçersiz ayar dosyası.");

                settings = loadedSettings;

                // Ana ayarlar dosyasına kaydet
                SaveSettings();

                MessageBox.Show($"Profil '{Path.GetFileName(profilePath)}' yüklendi ve main.json olarak kaydedildi.", 
                    "Başarılı", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Profil yüklenirken hata oluştu:\n{ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
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
        
        public static class Logger
        {
            private static readonly string logDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

            public static void Log(string message)
            {
                try
                {
                    if (!Directory.Exists(logDir))
                        Directory.CreateDirectory(logDir);

                    string logFile = Path.Combine(logDir, $"log_{DateTime.Now:yyyyMMdd}.txt");
                    string logLine = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
                    File.AppendAllLines(logFile, new[] { logLine });
                }
                catch
                {
                    // Hata yansıtma yapma, sessiz devam et
                }
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
            // Arkaplan daima şeffaf
            btnKaratay.Background = Brushes.Transparent;
            btnMeram.Background   = Brushes.Transparent;

            // Sadece yazı rengi değişiyor
            btnKaratay.Foreground =
                currentActiveButton == btnKaratay ? Brushes.Black : Brushes.White;

            btnMeram.Foreground =
                currentActiveButton == btnMeram ? Brushes.Black : Brushes.White;
        }



        private void btnKaratay_Click(object sender, RoutedEventArgs e)
        {
            if (currentActiveButton == btnKaratay) return;
            currentActiveButton = btnKaratay;

            settings.LastSelectedRegionIndex = 0; // Karatay index'i
            SaveSettings(); // AYARI BURADA KAYDET

            AnimateSelectionHighlight(0);
            UpdateButtonStyles();
            AnimatePrayerTimeChange(() => UpdatePrayerTimes(DateTime.Now.Day));
        }

        private void btnMeram_Click(object sender, RoutedEventArgs e)
        {
            if (currentActiveButton == btnMeram) return;
            currentActiveButton = btnMeram;

            settings.LastSelectedRegionIndex = 1; // Meram index'i
            SaveSettings(); // AYARI BURADA KAYDET

            AnimateSelectionHighlight(1);
            UpdateButtonStyles();
            AnimatePrayerTimeChange(() => UpdatePrayerTimes(DateTime.Now.Day));
        }


        
        
        
        
        private readonly Dictionary<int, string> AylarVeAyetler = new Dictionary<int, string>()
        {
            { 1, "Allah, kendisiyle birlikte başka ilah olmayan, diridir, kayyumdur. (Bakara, 2:255)" },
            { 2, "Her kim bir iyilik yaparsa, kendisi için yapmış olur. (Bakara, 2:272)" },
            { 3, "Peygamberimiz (sav) dedi ki: 'Kolaylaştırınız, zorlaştırmayınız; müjdeleyiniz, nefret ettirmeyiniz.' (Buhari)" },
            { 4, "Şüphesiz, güçlükle beraber bir kolaylık vardır. (İnşirah, 94:6)" },
            { 5, "Ey iman edenler! Sabır ve namazla Allah’tan yardım isteyin. (Bakara, 2:153)" },
            { 6, "İnsanlar için en hayırlı olan, Allah’ın kendilerine vahyettiği kitaba uyan ve salih amel işleyenlerdir. (Bakara, 2:112)" },
            { 7, "Peygamberimiz (sav) buyurdu: 'Müminin hali, yürüyen bir ağaç gibidir.' (Tirmizi)" },
            { 8, "İman edenler ve kalpleri Allah’ı anmakla huzur bulanlar... (Ra’d, 13:28)" },
            { 9, "En hayırlınız, insanlara en faydalı olandır. (Taberani)" },
            { 10, "Allah’ın rahmeti, iman eden ve salih amel işleyenlere yakındır. (A’raf, 7:56)" },
            { 11, "Peygamberimiz (sav): 'İlim öğrenmek her Müslüman erkek ve kadın üzerine farzdır.' (İbn Mace)" },
            { 12, "Allah’ın rahmeti geniştir. O’na tevekkül edenler asla hüsrana uğramazlar. (Hud, 11:56)" }
        };

        

        private void UpdateMonthlyVerse()
        {
            int currentMonth = DateTime.Now.Month;
            if (AylarVeAyetler.TryGetValue(currentMonth, out string ayet))
            {
                marqueeText.Text = ayet;  // monthlyVerseTextBlock UI'da ayet gösteren TextBlock
            }
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
        
        private readonly DoubleAnimation slideAnim = new()
        {
            Duration = TimeSpan.FromMilliseconds(180),   // hızlı ama sarsmıyor
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        
        private void MoveHighlight(Button target)
        {
            double targetX = target == btnKaratay ? 0 : 140; // buton genişliği
            slideAnim.To = targetX;
            highlightTT.BeginAnimation(TranslateTransform.XProperty, slideAnim);
        }



        private void SetPrayerLabels(string[] vakitler)
{
    // Namaz isimleri sıralı
    string[] namazlar = { "İmsak", "Güneş", "Öğle", "İkindi", "Akşam", "Yatsı" };

    // Etiketleri bir listeye alalım ki dinamik ayarlayalım
    var labels = new TextBlock[] { imsakLabel, gunesLabel, ogleLabel, ikindiLabel, aksamLabel, yatsiLabel };

    DateTime now = DateTime.Now;
    int nextPrayerIndex = -1;

    // Hangi vaktin gelecekte olduğunu bul
    for (int i = 0; i < vakitler.Length; i++)
    {
        if (!TimeSpan.TryParse(vakitler[i], out var time))
            continue;

        DateTime vakitDateTime = now.Date + time;

        if (vakitDateTime > now)
        {
            nextPrayerIndex = i;
            break;
        }
    }

    for (int i = 0; i < vakitler.Length; i++)
    {
        string text = $"{namazlar[i]}: {vakitler[i]}";
        labels[i].Text = text;

        if (i == nextPrayerIndex)
        {
            // Bir sonraki vakit: sarı (örneğin #FFA500)
            labels[i].Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFA500"));
        }
        else if (i < nextPrayerIndex || nextPrayerIndex == -1)
        {
            // Geçmiş vakitler beyaz (veya ayarladığın normal renk)
            labels[i].Foreground = Brushes.White;
        }
        else
        {
            // Gelecek vakitler normal tema renginde (örneğin gri veya siyah)
            labels[i].Foreground = settings.IsDarkMode ? Brushes.Gray : new SolidColorBrush(Color.FromRgb(51, 51, 51));
        }
    }

    UpdateProgressBar(vakitler, nextPrayerIndex);
}

private void UpdateProgressBar(string[] vakitler, int nextPrayerIndex)
{
    DateTime now = DateTime.Now;
    DateTime nextPrayerTime;
    DateTime previousPrayerTime;

    if (nextPrayerIndex == -1)
    {
        // Eğer sonraki vakit yoksa, gece yarısından sonra ilk vakte geçiş
        nextPrayerTime = now.Date.AddDays(1) + TimeSpan.Parse(vakitler[0]);
        previousPrayerTime = now.Date + TimeSpan.Parse(vakitler[vakitler.Length - 1]);
    }
    else
    {
        nextPrayerTime = now.Date + TimeSpan.Parse(vakitler[nextPrayerIndex]);
        previousPrayerTime = nextPrayerIndex > 0
            ? now.Date + TimeSpan.Parse(vakitler[nextPrayerIndex - 1])
            : now.Date;
    }

    var totalInterval = nextPrayerTime - previousPrayerTime;
    var elapsed = now - previousPrayerTime;

    double progressPercentage = 0;

    if (totalInterval.TotalSeconds > 0)
        progressPercentage = Math.Max(0, Math.Min(1, elapsed.TotalSeconds / totalInterval.TotalSeconds));

    progressBar.Value = progressPercentage * 100;
}

private void AnimateSelectionHighlight(int targetIndex)
{
    double targetX = targetIndex * 140; // 140: buton genişliği + margin, sen kendi grid yapına göre ayarla
    
    var anim = new DoubleAnimation
    {
        To = targetX,
        Duration = TimeSpan.FromMilliseconds(400),
        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
    };

    highlightTT.BeginAnimation(TranslateTransform.XProperty, anim);
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

            countdownLabel.Text = "Allah kabul etsin.";
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
