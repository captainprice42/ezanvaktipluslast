using System;
using System.IO;
using System.Text.Json;

namespace Ezan_Vakti_Plus
{
    public class AppSettings
    {
        public bool IsDarkMode { get; set; } = true;
        public bool StartWithWindows { get; set; } = false;
        public int ImsakNotificationMinutes { get; set; } = 15;
        public int GunesNotificationMinutes { get; set; } = 15;
        public int OgleNotificationMinutes { get; set; } = 15;
        public int IkindiNotificationMinutes { get; set; } = 15;
        public int AksamNotificationMinutes { get; set; } = 15;
        public int YatsiNotificationMinutes { get; set; } = 15;

        public string ImsakNotificationSoundFile { get; set; } = "";
        public string GunesNotificationSoundFile { get; set; } = "";
        public string OgleNotificationSoundFile { get; set; } = "";
        public string IkindiNotificationSoundFile { get; set; } = "";
        public string AksamNotificationSoundFile { get; set; } = "";
        public string YatsiNotificationSoundFile { get; set; } = "";
    }

    public static class SettingsLoader
    {
        private static readonly string SettingsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings");
        private static readonly string DefaultSettingsPath = Path.Combine(SettingsFolder, "settings.json");

        public static AppSettings LoadSettings(string path = null)
        {
            string loadPath = path ?? DefaultSettingsPath;

            if (!File.Exists(loadPath))
                return new AppSettings();

            try
            {
                string json = File.ReadAllText(loadPath);
                var options = new JsonSerializerOptions
                {
                    AllowTrailingCommas = true,
                    ReadCommentHandling = JsonCommentHandling.Skip,
                    PropertyNameCaseInsensitive = true
                };

                var loadedSettings = JsonSerializer.Deserialize<AppSettings>(json, options);

                return loadedSettings ?? new AppSettings();
            }
            catch
            {
                return new AppSettings();
            }
        }

        public static void SaveSettings(AppSettings settings, string path = null)
        {
            string savePath = path ?? DefaultSettingsPath;

            try
            {
                if (!Directory.Exists(SettingsFolder))
                    Directory.CreateDirectory(SettingsFolder);

                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(settings, options);
                File.WriteAllText(savePath, json);
            }
            catch
            {
                // Hata yönetimi ekleyebilirsin.
            }
        }
    }
}
