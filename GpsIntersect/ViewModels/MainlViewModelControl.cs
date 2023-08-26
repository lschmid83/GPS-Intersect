using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GpsIntersect.Properties;
using System.Windows.Input;

namespace GpsIntersect.ViewModels
{
    /// <summary>
    /// Sets the visibility of Window controls from View menu options and store values in Settings.
    /// </summary>
    public class MainlViewModelControl : ViewModelBase
    {
        private bool gpsFilesVisibility;
        public bool GpsFilesVisibility
        {
            get { return gpsFilesVisibility; }
            set
            {
                gpsFilesVisibility = value;
                RaisePropertyChanged("GpsFilesVisibility");
                GpsFilesSeparatorVisibility = value;
                Settings.Default.GpsFilesVisibility = value;
                Settings.Default.Save();
            }
        }

        private bool gpsFilesSeparatorVisibility;
        public bool GpsFilesSeparatorVisibility
        {
            get { return gpsFilesSeparatorVisibility; }
            set
            {
                gpsFilesSeparatorVisibility = value;
                RaisePropertyChanged("GpsFilesSeparatorVisibility");
            }
        }

        private bool intersectionsVisibility;
        public bool IntersectionsVisibility
        {
            get { return intersectionsVisibility; }
            set
            {
                intersectionsVisibility = value;
                RaisePropertyChanged("IntersectionsVisibility");            
                IntersectionsSeparatorVisibility = value;

                Settings.Default.IntersectionsVisibility = value;
                Settings.Default.Save();
            }
        }

        private bool intersectionsSeparatorVisibility;
        public bool IntersectionsSeparatorVisibility
        {
            get { return intersectionsSeparatorVisibility; }
            set
            {
                intersectionsSeparatorVisibility = value;
                RaisePropertyChanged("IntersectionsSeparatorVisibility");
            }
        }

        private bool trackPointsVisibility;
        public bool TrackPointsVisibility
        {
            get { return trackPointsVisibility; }
            set
            {
                trackPointsVisibility = value;
                RaisePropertyChanged("TrackPointsVisibility");
                IntersectionsSeparatorVisibility = value;

                Settings.Default.TrackPointsVisibility = value;
                Settings.Default.Save();
            }
        }

        private bool mapLabelsVisibility;
        public bool MapLabelsVisibility
        {
            get { return mapLabelsVisibility; }
            set
            {
                mapLabelsVisibility = value;
                RaisePropertyChanged("MapLabelsVisibility");

                Settings.Default.MapLabelsVisibility = value;
                Settings.Default.Save();
            }
        }

        private bool statusBarVisibility;
        public bool StatusBarVisibility
        {
            get { return statusBarVisibility; }
            set
            {
                statusBarVisibility = value;
                RaisePropertyChanged("StatusBarVisibility");

                Settings.Default.StatusBarVisibility = value;
                Settings.Default.Save();
            }
        }

        public ICommand ToggleGpsFilesVisibility
        {
            get
            {
                return new RelayCommand(() =>
                {
                    GpsFilesVisibility = !GpsFilesVisibility;
                });
            }
        }

        public ICommand ToggleIntersectionsVisibility
        {
            get
            {
                return new RelayCommand(() =>
                {
                    IntersectionsVisibility = !IntersectionsVisibility;
                });
            }
        }

        public ICommand ToggleTrackPointsVisibility
        {
            get
            {
                return new RelayCommand(() =>
                {
                    TrackPointsVisibility = !TrackPointsVisibility;
                });
            }
        }

        public ICommand ToggleMapLabelsVisibility
        {
            get
            {
                return new RelayCommand(() =>
                {
                    MapLabelsVisibility = !MapLabelsVisibility;
                });
            }
        }

        public ICommand ToggleStatusBarVisibility
        {
            get
            {
                return new RelayCommand(() =>
                {
                    StatusBarVisibility = !StatusBarVisibility;
                });
            }
        }
    }
}
