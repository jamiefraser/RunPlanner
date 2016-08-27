using MvvmCross.Core.ViewModels;
using System.Collections.ObjectModel;

namespace RunPlanner.Core.ViewModels
{
    public class FirstViewModel 
        : MvxViewModel
    {
        public FirstViewModel()
        {
            
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
        public ObservableCollection<RunPoint>Pins
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
    }
}
