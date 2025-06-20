using Microsoft.Win32;
using System.Diagnostics;
using System.IO;

public static class StartupHelper
{
    private const string REG_KEY = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
    private const string VALUE_NAME = "EzanVaktiPlus";

    public static void SetStartup(bool enable)
    {
        string exePath = Process.GetCurrentProcess().MainModule!.FileName;

        // Yol boşsa ya da exe bulunamazsa çık
        if (string.IsNullOrWhiteSpace(exePath) || !File.Exists(exePath))
            return;

        // Boşluk problemlerine karşı yolun tamamını çift tırnakla sar
        exePath = $"\"{exePath}\"";

        try
        {
            using RegistryKey rk = Registry.CurrentUser.OpenSubKey(REG_KEY, writable: true)
                                   ?? Registry.CurrentUser.CreateSubKey(REG_KEY)!;

            if (enable)
                rk.SetValue(VALUE_NAME, exePath, RegistryValueKind.String);
            else
                rk.DeleteValue(VALUE_NAME, throwOnMissingValue: false);
        }
        catch (Exception ex)
        {
            // Gerekirse logla ya da MessageBox göster
            Debug.WriteLine($"SetStartup failed: {ex.Message}");
        }
    }
}