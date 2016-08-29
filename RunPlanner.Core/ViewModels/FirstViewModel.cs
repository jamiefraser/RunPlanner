using MvvmCross.Core.ViewModels;
using System.Collections.ObjectModel;
using MvvmCross.Plugins.Location;
using MvvmCross.Platform.Platform;
using RunPlanner.Core.Entities;
using System.Threading.Tasks;

namespace RunPlanner.Core.ViewModels
{
    public class FirstViewModel 
        : MvxViewModel
    {
        #region Members
        private IMvxLocationWatcher _location;
        #endregion
        #region CIRS
        public FirstViewModel(IMvxLocationWatcher location)
        {
            _location = location;
            MvxLocationOptions options = new MvxLocationOptions();
            options.Accuracy = MvxLocationAccuracy.Fine;
            options.MovementThresholdInM = 5;
            options.TimeBetweenUpdates = System.TimeSpan.FromSeconds(1);
            options.TrackingMode = MvxLocationTrackingMode.Background;
            _location.Start(options, (loc) =>
            {
                CurrentLat = loc.Coordinates.Latitude;
                CurrentLong = loc.Coordinates.Longitude;
                CurrentLocation = loc.Coordinates;
            },
                (fail) => { MvxTrace.Trace(fail.Code.ToString());
            });
            Pins = new ObservableCollection<RunPoint>();
            Pins.CollectionChanged += Pins_CollectionChanged;
        }
        #endregion

        #region Hacks!
        private void Pins_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            RaisePropertyChanged(() => Pins);
            SaveCommand.RaiseCanExecuteChanged();
        }
        #endregion

        #region Properties
        #region ProposedRunLengthInKilometers
        private double _proposedrunlengthinkilometers;
        public double ProposedRunLengthInKilometers
        {
            get { return _proposedrunlengthinkilometers; }
            set { _proposedrunlengthinkilometers = value; RaisePropertyChanged(() => ProposedRunLengthInKilometers); }
        }
        #endregion
        private MvxCoordinates _currentlocation;
        public MvxCoordinates CurrentLocation
        {
            get { return _currentlocation; }
            set
            {
                if (_currentlocation != value)
                {
                    _currentlocation = value;
                    RaisePropertyChanged(() => CurrentLocation);
                }
            }
        }

        private double _currentlat;
        public double CurrentLat
        {
            get { return _currentlat; }
            set { _currentlat = value; RaisePropertyChanged(() => CurrentLat); }
        }
        private double _currentlong;
        public double CurrentLong
        {
            get { return _currentlong; }
            set { _currentlong = value; RaisePropertyChanged(() => CurrentLong); }
        }

        private string _hello = "Hello MvvmCross";
        public string Hello
        {
            get
            {
                return RunPlanner.Core.Properties.Resources.lblHello;
            }
        }
        private ObservableCollection<RunPoint> _pins;
        public ObservableCollection<RunPoint> Pins
        {
            get
            {
                return _pins;
            }
            set
            {
                _pins = value;
                RaisePropertyChanged(() => Pins);
            }
        }
        #endregion

        #region Commands
        #region AddPointCommand
        private MvxAsyncCommand<RunPoint> _addpointcommand;
        public MvxAsyncCommand<RunPoint>AddPointCommand
        {
            get
            {
                if(_addpointcommand==null)
                {
                    _addpointcommand = new MvxAsyncCommand<RunPoint>(async(point) =>
                    {
                        Pins.Add(point);
                    });
                }
                return _addpointcommand;
            }
        }
        #endregion

        #region RemovePointCommand
        private MvxAsyncCommand<RunPoint> _removepointcommand;
        public MvxAsyncCommand<RunPoint>RemovePointCommand
        {
            get
            {
                if(_removepointcommand==null)
                {
                    _removepointcommand = new MvxAsyncCommand<RunPoint>(async(point) =>
                    {
                        Pins.Remove(point);
                    });
                }
                return _removepointcommand;
            }
        }
        #endregion
        #region SaveCommand
        private MvxCommand _savecommand;

        public MvxCommand SaveCommand
        {
            get
            {
                _savecommand = _savecommand ?? new MvxCommand(async () => await DoSave(), () => this.Pins.Count > 1);
                return _savecommand;
            }
        }

        private async Task DoSave()
        {
            System.Diagnostics.Debug.WriteLine("Saving!!");
        }
        #endregion
        #endregion
    }
}
