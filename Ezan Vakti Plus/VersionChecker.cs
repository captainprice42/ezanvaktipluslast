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
        private static readonly Regex VER_RX = new(@"v(?<ver>\d+\.\d+)", RegexOptions.IgnoreCase);
        private static readonly Regex ZIP_RX = new(@"href\s*=\s*[""'](?<url>https?://[^""']+\.zip)[""']", RegexOptions.IgnoreCase);

        private const string BOOTSTRAPPER_EXE = "bootstrapper.exe";

        public static async Task CheckAsync(Window owner)
{
    using HttpClient client = new();
    string html = await client.GetStringAsync(PAGE_URL);

    string latestStr = VER_RX.Match(html).Groups["ver"].Value;
    string currentStr = Assembly.GetExecutingAssembly()
                                .GetName().Version!.ToString(2);

    if (!Version.TryParse(latestStr, out var latest) ||
        !Version.TryParse(currentStr, out var current) ||
        latest <= current)
        return;

    string zipUrl = ZIP_RX.Match(html).Groups["url"].Value;
    if (string.IsNullOrEmpty(zipUrl))
    {
        MessageBox.Show("Yeni sürüm var ama .zip linki bulunamadı.", "Güncelleme", MessageBoxButton.OK, MessageBoxImage.Warning);
        return;
    }

    var res = MessageBox.Show($"Yeni sürüm bulundu: v{latest}\n" +
                              $"Şu anki sürümün: v{current}\n\n" +
                              "Otomatik indirip kurayım mı?",
                              "Güncelleme", MessageBoxButton.YesNo, MessageBoxImage.Question);

    if (res != MessageBoxResult.Yes) return;

    string appDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    string bootstrapperPath = Path.Combine(appDir, BOOTSTRAPPER_EXE);

    if (!File.Exists(bootstrapperPath))
    {
        MessageBox.Show($"Güncelleme programı ({BOOTSTRAPPER_EXE}) bulunamadı!", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
        return;
    }

    try
    {
        ProcessStartInfo psi = new ProcessStartInfo(bootstrapperPath)
        {
            Arguments = $"\"{zipUrl}\" \"{appDir}\"",
            UseShellExecute = true,
            WindowStyle = ProcessWindowStyle.Minimized  // Konsol küçük ve gizli başlasın
        };
        Process.Start(psi);

        Application.Current.Shutdown();
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Güncelleme başlatılamadı: {ex.Message}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}

    }
}
