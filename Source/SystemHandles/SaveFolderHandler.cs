using System.Diagnostics;

namespace OpenVikings.SystemHandles
{
    internal class SaveFolderHandler
    {
        private readonly string savesFolderPath = Path.Combine(Environment.ProcessPath!, ConstantsHandler.SAVES_FOLDER_NAME);
        internal static readonly char[] separator = [' '];

        private bool SetSaveFolder()
        {
            try
            {
                if (!Directory.Exists(savesFolderPath))
                    Directory.CreateDirectory(savesFolderPath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to create directory: {savesFolderPath}. Exception: {ex.Message}");
                return false;
            }

            return true;
        }

        private bool SetOptionsGameSettingsINIFile()
        {
            string filePathWithININame = Path.Combine(savesFolderPath, ConstantsHandler.OPT_GAME_INI_NAME);

            if (!File.Exists(filePathWithININame))
            {
                try
                {
                    File.Create(filePathWithININame).Dispose();

                    Dictionary<string, string> defaultConfigurations = GetDefaultConfigurations();

                    using StreamWriter writer = new(filePathWithININame);
                    foreach (KeyValuePair<string, string> config in defaultConfigurations)
                    {
                        writer.WriteLine($"{config.Key} {config.Value}");
                    }
                }
                catch (Exception)
                {
                    Debug.WriteLine("Failed to create opt_game.ini");
                    return false;
                }
            }

            return true;
        }

        private bool SetDefaultConfigurations()
        {
            if (!SetSaveFolder())
                return false;

            if (!SetOptionsGameSettingsINIFile())
                return false;

            return true;
        }

        private static Dictionary<string, string> GetDefaultConfigurations()
        {
            Dictionary<string, string> defaultConfigurations = new()
            {
            { "engine_lod", "0" },
            { "music_mode", "2" },
            { "fx_volume", "100" },
            { "fx_quality", "2" },
            { "fx_jingles_off", "1" },
            { "dm_volume", "70" },
            { "gui_scroll_speed", "2" },
            { "gui_main_mode", "1" },
            { "gui_expert_flag", "0" },
            { "gui_tooltipsoff_flag", "0" },
            { "gui_scroll_on_third_button", "1" },
            { "gui_scroll_on_border", "1" },
            { "gui_mouse_software", "0" }
        };

            return defaultConfigurations;
        }

        internal (string Key, string Value) GetConfiguration(string dateiPfad, string key)
        {
            Dictionary<string, string> optionSetting = ParseINIFile(dateiPfad);

            if (optionSetting.TryGetValue(key, out string value))
            {
                return (key, value);
            }
            else
            {
                Debug.WriteLine($"The key '{key}' was not found in the configuration file.");
            }
        }

        private static Dictionary<string, string> ParseINIFile(string dateiPfad)
        {
            Dictionary<string, string> optionSetting = [];

            foreach (string line in File.ReadLines(dateiPfad))
            {
                string cleanedLine = line.Trim();
                if (string.IsNullOrWhiteSpace(cleanedLine))
                    continue;

                string[] teile = cleanedLine.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                if (teile.Length == 2)
                {
                    string key = teile[0];
                    string value = teile[1];

                    optionSetting[key] = value;
                }
            }

            return optionSetting;
        }

        internal void InitializeConfigurations()
        {
            if (SetDefaultConfigurations())
            {
                Debug.WriteLine("Default configurations are set successfully.");
            }
            else
            {
                Debug.WriteLine("Failed to set default configurations.");
            }
        }
    }
}