using System.Windows;
using System.Windows.Input;

using OBS_Overlay_Manager.Models;

using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace OBS_Overlay_Manager.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel(JSONConfig json, PNGComposite comp)
        {
            _json = json;
            _comp = comp;

            Profile? lastLeft = _json.AllProfiles.FirstOrDefault(p => p.ProfileName.Equals(json.LastSelectedProfileLeft), new());
            Profile? lastRight = _json.AllProfiles.FirstOrDefault(p => p.ProfileName.Equals(json.LastSelectedProfileRight), new());

            if (lastLeft is not null)
                _profileLeft = new Profile(lastLeft.ProfileName, lastLeft.Color, lastLeft.TeamName, lastLeft.Score);
            else
                _profileLeft = new();

            if (lastRight is not null)
                _profileRight = new Profile(lastRight.ProfileName, lastRight.Color, lastRight.TeamName, lastRight.Score);
            else
                _profileRight = new();

            CmdIncrementScoreLeft = new RelayCommand(IncrementScoreLeft);
            CmdIncrementScoreRight = new RelayCommand(IncrementScoreRight);
            CmdDecrementScoreLeft = new RelayCommand(DecrementScoreLeft);
            CmdDecrementScoreRight = new RelayCommand(DecrementScoreRight);
            CmdSaveProfileLeft = new RelayCommand(SaveNewProfileLeft);
            CmdSaveProfileRight = new RelayCommand(SaveNewProfileRight);
            CmdDeleteProfileLeft = new RelayCommand(DeleteProfileLeft);
            CmdDeleteProfileRight = new RelayCommand(DeleteProfileRight);
            CmdResetScores = new RelayCommand(ResetScores);
            CmdClearAll = new RelayCommand(ClearAll);
        }

        #region Properties
        private readonly JSONConfig _json;
        private readonly PNGComposite _comp;

        private Profile _profileLeft;
        private Profile _profileRight;

        public ObservableCollection<Profile> AllProfiles
        {
            get { return _json.AllProfiles; }
        }

        public int ColorLeft
        {
            get { return _profileLeft.Color; }

            set
            {
                _profileLeft.Color = value;
                NotifyPropertyChangedAndUpdateComp(nameof(ColorLeft));
            }
        }

        public int ScoreLeft
        {
            get { return _profileLeft.Score; }
            set
            {
                _profileLeft.Score = value;
                NotifyPropertyChangedAndUpdateComp(nameof(ScoreLeft));
            }
        }

        public string TeamNameLeft
        {
            get { return _profileLeft.TeamName; }
            set
            {
                _profileLeft.TeamName = value;
                NotifyPropertyChangedAndUpdateComp(nameof(TeamNameLeft));
            }
        }

        public string ProfileNameLeft
        {
            get { return _profileLeft.ProfileName; }
            set
            {
                _profileLeft.ProfileName = value;
                NotifyPropertyChanged(nameof(ProfileNameLeft));
            }
        }

        private int _profileIndexLeft = -1;

        public int ProfileIndexLeft
        {
            get { return _profileIndexLeft; }
            set
            {
                _profileIndexLeft = value;
                _json.LastSelectedProfileLeft = ProfileNameLeft;
                try
                {
                    _profileLeft = new(_json.AllProfiles[value]);
                }
                catch { }
                NotifyAllLeft();
                SaveJSON();
            }
        }

        public int ColorRight
        {
            get { return _profileRight.Color; }

            set
            {
                _profileRight.Color = value;
                NotifyPropertyChangedAndUpdateComp(nameof(ColorRight));
            }
        }

        public int ScoreRight
        {
            get { return _profileRight.Score; }
            set
            {
                _profileRight.Score = value;
                NotifyPropertyChangedAndUpdateComp(nameof(ScoreRight));
            }
        }

        public string TeamNameRight
        {
            get { return _profileRight.TeamName; }
            set
            {
                _profileRight.TeamName = value;
                NotifyPropertyChangedAndUpdateComp(nameof(TeamNameRight));
            }
        }

        public string ProfileNameRight
        {
            get { return _profileRight.ProfileName; }
            set
            {
                _profileRight.ProfileName = value;
                NotifyPropertyChanged(nameof(ProfileNameRight));
            }
        }

        private int _profileIndexRight = -1;

        public int ProfileIndexRight
        {
            get { return _profileIndexRight; }
            set
            {
                _profileIndexRight = value;
                _json.LastSelectedProfileRight = ProfileNameRight;
                try
                {
                    _profileRight = new(_json.AllProfiles[value]);
                }
                catch { }
                NotifyAllRight();
                SaveJSON();
            }
        }

        private bool _showOverlays = false;

        public bool ShowOverlays
        {
            get { return _showOverlays; }
            set
            {
                _showOverlays = value;
                NotifyPropertyChangedAndUpdateComp(nameof(ShowOverlays));
            }
        }
        #endregion

        #region Commands
        public ICommand CmdClearAll { get; }
        public ICommand CmdDecrementScoreLeft { get; }
        public ICommand CmdDecrementScoreRight { get; }
        public ICommand CmdDeleteProfileLeft { get; }
        public ICommand CmdDeleteProfileRight { get; }
        public ICommand CmdIncrementScoreLeft { get; }
        public ICommand CmdIncrementScoreRight { get; }
        public ICommand CmdResetScores { get; }
        public ICommand CmdSaveProfileLeft { get; }
        public ICommand CmdSaveProfileRight { get; }
        #endregion

        #region Methods
        private void ClearAll()
        {
            _profileLeft = new();
            _profileRight = new();
            NotifyAllLeft();
            NotifyAllRight();
            UpdateJSON(true, true);
        }

        private void DecrementScoreLeft()
        {
            if (ScoreLeft > 0)
                --ScoreLeft;
        }

        private void DecrementScoreRight()
        {
            if (ScoreRight > 0)
                --ScoreRight;
        }

        private void DeleteProfile(bool isLeft)
        {
            try
            {
                _json.AllProfiles.Remove(AllProfiles.First(p => p.ProfileName.Equals(GetProfileName(isLeft))));

                if (isLeft)
                {
                    _profileLeft = new();
                    NotifyAllLeft();
                }
                else
                {
                    _profileRight = new();
                    NotifyAllRight();
                }
                SaveJSON();
                NotifyPropertyChanged(nameof(AllProfiles));
            }
            catch { }
        }

        private void DeleteProfileLeft()
        {
            DeleteProfile(true);
        }

        private void DeleteProfileRight()
        {
            DeleteProfile(false);
        }

        private int GetColor(bool isLeft)
        {
            return isLeft ? ColorLeft : ColorRight;
        }

        private string GetProfileName(bool isLeft)
        {
            return isLeft ? ProfileNameLeft : ProfileNameRight;
        }

        private int GetScore(bool isLeft)
        {
            return isLeft ? ScoreLeft : ScoreRight;
        }

        private string GetTeamName(bool isLeft)
        {
            return isLeft ? TeamNameLeft : TeamNameRight;
        }

        private void IncrementScoreLeft()
        {
            ++ScoreLeft;
        }

        private void IncrementScoreRight()
        {
            ++ScoreRight;
        }

        private void NotifyAllLeft()
        {
            NotifyPropertyChanged(nameof(ProfileIndexLeft));
            NotifyPropertyChanged(nameof(ProfileNameLeft));
            NotifyPropertyChanged(nameof(ColorLeft));
            NotifyPropertyChanged(nameof(TeamNameLeft));
            NotifyPropertyChangedAndUpdateComp(nameof(ScoreLeft));
        }

        private void NotifyAllRight()
        {
            NotifyPropertyChanged(nameof(ProfileIndexRight));
            NotifyPropertyChanged(nameof(ProfileNameRight));
            NotifyPropertyChanged(nameof(ColorRight));
            NotifyPropertyChanged(nameof(TeamNameRight));
            NotifyPropertyChangedAndUpdateComp(nameof(ScoreRight));
        }

        private void NotifyPropertyChangedAndUpdateComp(string propertyName)
        {
            NotifyPropertyChanged(propertyName);
            _comp.Update(_profileLeft, _profileRight, ShowOverlays);
        }

        private void ResetScores()
        {
            ScoreLeft = ScoreRight = 0;
            UpdateJSON(true, true);
        }


        internal void SaveJSON()
        {
            _json.LastSelectedProfileLeft = ProfileNameLeft;
            _json.LastSelectedProfileRight = ProfileNameRight;

            Profile.SaveProfiles(_json);
        }

        private void SaveNewProfile(bool isLeft)
        {
            if (GetProfileName(isLeft).Equals(string.Empty))
                return;

            // if profile exists, update it instead of adding new
            if (AllProfiles.Count > 0 && _json.AllProfiles.FirstOrDefault(p => p is not null && p.ProfileName.Equals(GetProfileName(isLeft)), null) is not null)
            {
                UpdateJSON(isLeft, !isLeft);
                return;
            }

            try
            {
                _json.AllProfiles.Add(new Profile(GetProfileName(isLeft), GetColor(isLeft), GetTeamName(isLeft), GetScore(isLeft)));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.Source, MessageBoxButton.OK, MessageBoxImage.Warning);
            }

            SaveJSON();
            NotifyPropertyChanged(nameof(AllProfiles));
        }

        internal void SaveNewProfileLeft()
        {
            SaveNewProfile(true);
        }

        internal void SaveNewProfileRight()
        {
            SaveNewProfile(false);
        }

        private void UpdateJSON(bool isLeft)
        {
            try
            {
                Profile p = _json.AllProfiles.First(p => p.ProfileName.Equals(GetProfileName(isLeft)));
                p.ProfileName = GetProfileName(isLeft);
                p.Color = GetColor(isLeft);
                p.TeamName = GetTeamName(isLeft);
                p.Score = GetScore(isLeft);
            }
            catch { }

            SaveJSON();
        }

        private void UpdateJSON(bool doLeft, bool doRight)
        {
            if (doLeft)
                UpdateJSON(true);

            if (doRight)
                UpdateJSON(false);
        }
        #endregion
    }
}
