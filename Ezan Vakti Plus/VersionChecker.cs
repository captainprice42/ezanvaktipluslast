using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Ezan_Vakti_Plus
{
    public static class VersionChecker
    {
        private const string PAGE_URL = "https://nv9g7uwlp01ko3qzyxt.blogspot.com/p/version.html";

        //  ➜  “v1.7” gibi kalıbı yakalar
        private static readonly Regex VER_RX = new(@"v(?<ver>\d+\.\d+)", RegexOptions.IgnoreCase);

        //  ➜  “...zip” linkini yakalar
        private static readonly Regex ZIP_RX = new(@"href\s*=\s*[""'](?<url>https?://[^""']+\.zip)[""']",
                                                   RegexOptions.IgnoreCase);

        private const string BOOTSTRAPPER_EXE = "bootstrapper.exe";

        /// <summary>
        ///  Yeni sürüm kontrolü yapar; varsa UpdateDialog gösterir
        /// </summary>
        public static async Task CheckAsync(Window owner)
        {
            // 1) Versiyon ve zip bilgilerini sayfadan çek
            using HttpClient client = new();
            string html = await client.GetStringAsync(PAGE_URL);

            string latestStr  = VER_RX.Match(html).Groups["ver"].Value;   // örn “1.7”
            string currentStr = Assembly.GetExecutingAssembly()
                                        .GetName().Version!.ToString(2); // örn “1.6”

            if (!Version.TryParse(latestStr,  out var latest)  ||
                !Version.TryParse(currentStr, out var current) ||
                 latest <= current)                             // güncel ya da daha yeni
                return;

            string zipUrl = ZIP_RX.Match(html).Groups["url"].Value;
            if (string.IsNullOrEmpty(zipUrl))
            {
                MessageBox.Show(owner,
                                "Yeni sürüm var ama .zip bağlantısı bulunamadı.",
                                "Güncelleme", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // 2) Modern EVET / HAYIR penceresi göster
            var dlg = new UpdateDialog(currentStr, latestStr) { Owner = owner };
            dlg.ShowDialog();
            if (!dlg.IsAccepted)          // HAYIR’a basıldı
                return;

            // 3) Bootstrapper yolu
            string appDir           = AppDomain.CurrentDomain.BaseDirectory
                                      .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string bootstrapperPath = Path.Combine(appDir, BOOTSTRAPPER_EXE);

            if (!File.Exists(bootstrapperPath))
            {
                MessageBox.Show(owner,
                                $"Güncelleme programı bulunamadı: {BOOTSTRAPPER_EXE}",
                                "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // 4) Bootstrapper’ı arka planda çalıştır
            try
            {
                var psi = new ProcessStartInfo(bootstrapperPath)
                {
                    Arguments      = $"\"{zipUrl}\" \"{appDir}\"",
                    UseShellExecute = true,
                    WindowStyle     = ProcessWindowStyle.Minimized,
                    Verb            = "runas"
                };
                Process.Start(psi);

                // Mevcut pencereyi kapat – güncelleme arka planda devam ediyor
                owner.Dispatcher.Invoke(owner.Close);
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner,
                                $"Güncelleme başlatılamadı:\n{ex.Message}",
                                "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
