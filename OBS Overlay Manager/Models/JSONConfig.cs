using System.Collections.ObjectModel;

namespace OBS_Overlay_Manager.Models
{
    internal class JSONConfig(string lastSelectedProfileLeft, string lastSelectedProfileRight, ObservableCollection<Profile> allProfiles)
    {
        public string LastSelectedProfileLeft = lastSelectedProfileLeft;
        public string LastSelectedProfileRight = lastSelectedProfileRight;

        public ObservableCollection<Profile> AllProfiles = allProfiles;

        public JSONConfig() : this("", "", []) { }
    }
}
