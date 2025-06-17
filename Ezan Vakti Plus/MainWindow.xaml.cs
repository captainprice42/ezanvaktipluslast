using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Windows.Input;
using System.Media;  // Ses için

namespace Ezan_Vakti_Plus
{
    public partial class MainWindow : Window
    {
private Dictionary<int, string[]> KaratayVakitleri = new Dictionary<int, string[]>
{
    {1, new string[] {"03:37", "05:22", "12:52", "16:43", "20:11", "21:47"} },
    {2, new string[] {"03:35", "05:21", "12:53", "16:43", "20:12", "21:48"} },
    {3, new string[] {"03:35", "05:21", "12:53", "16:43", "20:13", "21:49"} },
    {4, new string[] {"03:34", "05:20", "12:53", "16:43", "20:13", "21:50"} },
    {5, new string[] {"03:34", "05:20", "12:53", "16:44", "20:14", "21:51"} },
    {6, new string[] {"03:33", "05:20", "12:53", "16:44", "20:14", "21:51"} },
    {7, new string[] {"03:33", "05:20", "12:53", "16:44", "20:14", "21:52"} },
    {8, new string[] {"03:33", "05:20", "12:53", "16:44", "20:15", "21:52"} },
    {9, new string[] {"03:33", "05:21", "12:54", "16:47", "20:18", "21:57"} },
    {10, new string[] {"03:33", "05:20", "12:54", "16:47", "20:19", "21:58"} },
    {11, new string[] {"03:33", "05:20", "12:55", "16:48", "20:19", "21:59"} },
    {12, new string[] {"03:33", "05:20", "12:55", "16:48", "20:19", "21:59"} },
    {13, new string[] {"03:32", "05:20", "12:55", "16:48", "20:20", "22:00"} },
    {14, new string[] {"03:32", "05:20", "12:55", "16:48", "20:20", "22:00"} },
    {15, new string[] {"03:32", "05:20", "12:55", "16:49", "20:21", "22:01"} },
    {16, new string[] {"03:32", "05:20", "12:56", "16:49", "20:21", "22:01"} },
    {17, new string[] {"03:32", "05:20", "12:56", "16:49", "20:21", "22:02"} },
    {18, new string[] {"03:32", "05:21", "12:56", "16:49", "20:22", "22:02"} },
    {19, new string[] {"03:32", "05:21", "12:56", "16:49", "20:22", "22:03"} },
    {20, new string[] {"03:32", "05:21", "12:57", "16:50", "20:22", "22:03"} },
    {21, new string[] {"03:32", "05:21", "12:57", "16:50", "20:23", "22:03"} },
    {22, new string[] {"03:32", "05:21", "12:57", "16:50", "20:23", "22:03"} },
    {23, new string[] {"03:32", "05:22", "12:57", "16:50", "20:23", "22:03"} },
    {24, new string[] {"03:32", "05:22", "12:57", "16:51", "20:23", "22:04"} },
    {25, new string[] {"03:33", "05:22", "12:58", "16:51", "20:23", "22:04"} },
    {26, new string[] {"03:33", "05:22", "12:58", "16:51", "20:23", "22:04"} },
    {27, new string[] {"03:33", "05:23", "12:58", "16:51", "20:23", "22:04"} },
    {28, new string[] {"03:33", "05:23", "12:58", "16:51", "20:23", "22:04"} },
    {29, new string[] {"03:34", "05:23", "12:58", "16:51", "20:23", "22:04"} },
    {30, new string[] {"03:34", "05:23", "12:58", "16:51", "20:23", "22:04"} }
};

private Dictionary<int, string[]> MeramVakitleri = new Dictionary<int, string[]>
{
    {1, new string[] {"03:40", "05:25", "12:55", "16:45", "20:14", "21:49"} },
    {2, new string[] {"03:39", "05:24", "12:56", "16:46", "20:15", "21:50"} },
    {3, new string[] {"03:38", "05:24", "12:56", "16:46", "20:16", "21:52"} },
    {4, new string[] {"03:38", "05:24", "12:56", "16:46", "20:16", "21:52"} },
    {5, new string[] {"03:37", "05:23", "12:56", "16:46", "20:17", "21:53"} },
    {6, new string[] {"03:37", "05:23", "12:56", "16:47", "20:17", "21:54"} },
    {7, new string[] {"03:36", "05:23", "12:56", "16:47", "20:18", "21:55"} },
    {8, new string[] {"03:35", "05:23", "12:57", "16:47", "20:19", "21:56"} },
    {9, new string[] {"03:35", "05:22", "12:57", "16:47", "20:19", "21:56"} },
    {10, new string[] {"03:35", "05:22", "12:57", "16:47", "20:20", "21:58"} },
    {11, new string[] {"03:34", "05:22", "12:57", "16:48", "20:20", "21:58"} },
    {12, new string[] {"03:34", "05:22", "12:57", "16:48", "20:20", "21:59"} },
    {13, new string[] {"03:34", "05:22", "12:58", "16:48", "20:21", "21:59"} },
    {14, new string[] {"03:34", "05:22", "12:58", "16:48", "20:22", "21:59"} },
    {15, new string[] {"03:34", "05:22", "12:58", "16:49", "20:22", "22:01"} },
    {16, new string[] {"03:34", "05:22", "12:58", "16:49", "20:22", "22:01"} },
    {17, new string[] {"03:34", "05:22", "12:58", "16:49", "20:23", "22:01"} },
    {18, new string[] {"03:34", "05:22", "12:58", "16:49", "20:23", "22:02"} },
    {19, new string[] {"03:34", "05:22", "12:58", "16:49", "20:23", "22:02"} },
    {20, new string[] {"03:34", "05:22", "12:59", "16:50", "20:23", "22:02"} },
    {21, new string[] {"03:34", "05:22", "12:59", "16:50", "20:24", "22:03"} },
    {22, new string[] {"03:34", "05:22", "12:59", "16:50", "20:24", "22:03"} },
    {23, new string[] {"03:34", "05:22", "12:59", "16:50", "20:24", "22:03"} },
    {24, new string[] {"03:35", "05:22", "12:59", "16:51", "20:24", "22:04"} },
    {25, new string[] {"03:35", "05:22", "12:59", "16:51", "20:24", "22:04"} },
    {26, new string[] {"03:35", "05:22", "12:59", "16:51", "20:24", "22:04"} },
    {27, new string[] {"03:35", "05:23", "12:59", "16:51", "20:24", "22:04"} },
    {28, new string[] {"03:35", "05:23", "12:59", "16:51", "20:24", "22:04"} },
    {29, new string[] {"03:35", "05:23", "12:59", "16:51", "20:24", "22:04"} },
    {30, new string[] {"03:35", "05:23", "12:59", "16:51", "20:24", "22:04"} }
};


        private Dictionary<int, string[]> currentVakitler;
        private Button currentActiveButton;

        private DispatcherTimer countdownTimer;
        private DispatcherTimer marqueeTimer;
        private double marqueeX;
        private bool marqueeRotatedRight = false;

        private SoundPlayer notificationSound;

        private bool notificationPlayedForIkindi = false;

        public MainWindow()
        {
            InitializeComponent();

            currentVakitler = KaratayVakitleri;
            currentActiveButton = btnKaratay;

            UpdateButtonStyles();

            UpdatePrayerTimes(DateTime.Now.Day);

            StartCountdownTimer();
            StartMarquee();

            // Ses dosyasını yükle (proje dizininde "Ses/bildirim.wav" olmalı)
            try
            {
                notificationSound = new SoundPlayer("Ses/bildirim.wav");
                notificationSound.Load();
            }
            catch
            {
                // Dosya bulunamazsa sessizce devam et
                notificationSound = null;
            }
        }

        private void UpdateButtonStyles()
        {
            btnKaratay.Style = currentActiveButton == btnKaratay
                ? (Style)FindResource("ActiveButtonStyle")
                : (Style)FindResource("InactiveButtonStyle");

            btnMeram.Style = currentActiveButton == btnMeram
                ? (Style)FindResource("ActiveButtonStyle")
                : (Style)FindResource("InactiveButtonStyle");
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

            var vakitler = currentVakitler.ContainsKey(day) ? currentVakitler[day] : null;

            if (vakitler == null)
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
            countdownTimer = new DispatcherTimer();
            countdownTimer.Interval = TimeSpan.FromSeconds(1);
            countdownTimer.Tick += CountdownTimer_Tick;
            countdownTimer.Start();
        }

        private void CountdownTimer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var vakitler = currentVakitler.ContainsKey(now.Day) ? currentVakitler[now.Day] : null;

            if (vakitler == null)
            {
                countdownLabel.Text = "Namaz vakitleri bulunamadı.";
                return;
            }

            // İkindi vakti zamanı
            if (TimeSpan.TryParse(vakitler[3], out TimeSpan ikindiTime))
            {
                var ikindiDateTime = now.Date + ikindiTime;
                var diff = ikindiDateTime - now;

                if (diff.TotalSeconds > 0)
                {
                    countdownLabel.Text = $"İkindi'ye kalan süre: {diff.Hours:D2}:{diff.Minutes:D2}:{diff.Seconds:D2}";

                    if (diff.TotalMinutes <= 15 && !notificationPlayedForIkindi)
                    {
                        notificationPlayedForIkindi = true;
                        PlayNotificationSound();
                    }
                }
                else
                {
                    countdownLabel.Text = "İkindi vakti geçti.";
                    notificationPlayedForIkindi = false; // Ertesi gün için reset
                }
            }
        }

        private void PlayNotificationSound()
        {
            if (notificationSound != null)
            {
                try
                {
                    notificationSound.Play();
                }
                catch
                {
                    // Hata varsa geç
                }
            }
        }

        private void StartMarquee()
        {
            marqueeX = marqueeCanvas.ActualWidth;
            Canvas.SetLeft(marqueeText, marqueeX);

            marqueeTimer = new DispatcherTimer();
            marqueeTimer.Interval = TimeSpan.FromMilliseconds(20);
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
            left -= 2; // hız

            if (left + marqueeText.ActualWidth < 0)
                left = marqueeCanvas.ActualWidth;

            Canvas.SetLeft(marqueeText, left);
        }

        private void MarqueeText_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var rotateAnim = new DoubleAnimation
            {
                Duration = TimeSpan.FromMilliseconds(400),
                FillBehavior = FillBehavior.HoldEnd
            };

            if (!marqueeRotatedRight)
            {
                rotateAnim.To = 0;
                rotateAnim.Completed += (s, ev) =>
                {
                    marqueeText.Text = "Ezan Vakti Plus İyi Günler Diler";
                    marqueeRotatedRight = false;
                    marqueeTimer.Start();
                };
            }
            else
            {
                rotateAnim.To = 0;
                rotateAnim.Completed += (s, ev) =>
                {
                    marqueeText.Text = "Ezan Vakti Plus İyi Günler Diler";
                    marqueeRotatedRight = false;
                    marqueeTimer.Start();
                };
            }

            marqueeRotate.BeginAnimation(System.Windows.Media.RotateTransform.AngleProperty, rotateAnim);
        }
    }
}
