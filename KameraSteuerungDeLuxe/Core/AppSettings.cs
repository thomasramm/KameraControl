using IWshRuntimeLibrary;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace KameraSteuerungDeLuxe
{
    public class AppSettings
    {
        public bool OpenOnStart { get; set; } = true;

        public bool HideWindowOnClick { get; set; } = false;

        public bool ShowManualControlWindow { get; set; } = true;

        public double OpenPositionX { get; set; } = 0;

        public double OpenPositionY { get; set; } = 0;

        public double OpenManualWindowPositionLeft { get; set; } = 0;

        public double OpenManualWindowPositionTop { get; set; } = 0;

        public string CameraIP { get; set; } = "10.0.1.41";

        public int CameraPort { get; set; } = 5678;
        public ObservableCollection<DisplayButton> DisplayButtons { get; set; } = new();

        public string PresetCameraOn { get; set; } = "1";

        public string PresetCameraOff { get; set; } = "9";
    }

    public static class AppSettingsManager
    {
        private static readonly string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "KameraSteuerungDeLuxe",
            "Settings.xml");

        private static readonly string AppName = "KameraSteuerungDeLuxe";
        private static readonly string AutostartRegistryPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private static readonly string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), AppName + ".lnk");

        public static void Save(AppSettings settings, bool? autostartEnabled = null)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);

                var serializer = new XmlSerializer(typeof(AppSettings));
                using var writer = new StreamWriter(FilePath);
                serializer.Serialize(writer, settings);

                if (autostartEnabled.HasValue)
                    SetAutostartEnabled(autostartEnabled);
            }
            catch (Exception ex)
            {
                // optional: Logging oder Fehlermeldung
                Console.WriteLine($"Fehler beim Speichern der Einstellungen: {ex.Message}");
            }
        }

        public static AppSettings Load()
        {
            AppSettings? settings = null;

            try
            {
                if (System.IO.File.Exists(FilePath))
                {
                    var serializer = new XmlSerializer(typeof(AppSettings));
                    using var reader = new StreamReader(FilePath);
                    settings = (AppSettings?)serializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                // optional: Logging oder Fehlermeldung
                Console.WriteLine($"Fehler beim Laden der Einstellungen: {ex.Message}");
            }
            if (settings is null)
            {
                settings ??= new AppSettings();
                LoadDefaultConfig(settings);
            }

            return settings;
        }

        private static void LoadDefaultConfig(AppSettings settings)
        {
            //Programm default Settings
            settings.DisplayButtons.Add(new DisplayButton { Position = 1, Name = "Redner", Icon = "Redner 01", Preset = "1" });
            settings.DisplayButtons.Add(new DisplayButton { Position = 2, Name = "Redner", Icon = "Redner 02", Preset = "2" });
            settings.DisplayButtons.Add(new DisplayButton { Position = 3, Name = "Gespräch", Icon = "2 Personen 04", Preset = "3" });
            settings.DisplayButtons.Add(new DisplayButton { Position = 4, Name = "Redner", Icon = "Redner 03", Preset = "4" });
            settings.DisplayButtons.Add(new DisplayButton { Position = 5, Name = "Redner", Icon = "Redner 04", Preset = "5" });
            settings.DisplayButtons.Add(new DisplayButton { Position = 6, Name = "Aus", Icon = "Kamera Aus", Preset = "6" });
            settings.DisplayButtons.Add(new DisplayButton { Position = 7, Name = "Leser", Icon = "2 Personen 01", Preset = "7" });
            settings.DisplayButtons.Add(new DisplayButton { Position = 8, Name = "Tisch", Icon = "Tisch 02", Preset = "8" });
            settings.DisplayButtons.Add(new DisplayButton { Position = 9, Name = "Bühne", Icon = "Bühne 01", Preset = "9" });
            settings.DisplayButtons.Add(new DisplayButton { Position = 10, Name = "Bühne", Icon = "Bühne 02", Preset = "0" });
        }

        public static void SetAutostartEnabled(bool? enabled)
        {
            SetAutostartEnabledWithAppShortcut(enabled);
        }

        private static void SetAutostartEnabledWithAppShortcut(bool? enabled)
        {
            if (enabled == null) return;

            if (enabled == true)
            {
                WshShell shell = new WshShell();
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.Description = $"Startet {AppName} beim Windows-Start";
                shortcut.TargetPath = Environment.ProcessPath;
                shortcut.WorkingDirectory = Path.GetDirectoryName(shortcut.TargetPath);
                shortcut.Save();
            }
            else
            {
                System.IO.File.Delete(shortcutPath);
            }
        }

        private static void SetAutostartEnabledWithRegistry(bool? enabled)
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(AutostartRegistryPath, true);

            if (enabled == true)
            {
                key?.SetValue(AppName, $"\"{Environment.ProcessPath}\"");
            }
            else
            {
                if (key?.GetValue(AppName) != null)
                {
                    key.DeleteValue("KameraSteuerungDeLuxe");
                }
            }
        }

        public static bool IsAutostartEnabled()
        {
            return IsAutostartEnabledWithAppShortcut();
        }

        private static bool IsAutostartEnabledWithAppShortcut()
        {
            return System.IO.File.Exists(shortcutPath);
        }

        private static bool IsAutostartEnabledWithRegistry()
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey(AutostartRegistryPath, false);
            return key?.GetValue(AppName) != null;
        }
    }
}