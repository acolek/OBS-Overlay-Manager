using System.IO;
using System.Windows;

using Newtonsoft.Json;

namespace OBS_Overlay_Manager.Models
{
    internal class Profile(string profileName, int color, string teamName, int score)
    {
        private static readonly string _profilesPath = $"{Environment.CurrentDirectory}\\Profiles.json";

        public string ProfileName { get; set; } = profileName;
        public int Color { get; set; } = color;
        public int Score { get; set; } = score;
        public string TeamName { get; set; } = teamName;

        public Profile() : this("", -1, "", 0) { }

        public Profile(Profile p) : this(p.ProfileName, p.Color, p.TeamName, p.Score) { }

        public static void SaveProfiles(JSONConfig json)
        {
            try
            {
                File.WriteAllText(_profilesPath, JsonConvert.SerializeObject(json, Formatting.Indented));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public static JSONConfig LoadProfiles()
        {
            if (File.Exists(_profilesPath))
            {
                return JsonConvert.DeserializeObject<JSONConfig>(File.ReadAllText(_profilesPath)) ?? new JSONConfig();
            }
            else
                return new JSONConfig();
        }
    }

}
