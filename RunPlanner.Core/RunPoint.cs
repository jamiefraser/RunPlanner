using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunPlanner.Core
{
    public class RunPoint : MvxNotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; RaisePropertyChanged(() => Name); }
        }

        private long _lat;
        public long Lat
        {
            get { return _lat; }
            set { _lat = value; RaisePropertyChanged(() => Lat); }
        }

        private long _long;
        public long Long
        {
            get { return _long; }
            set { _long = value; RaisePropertyChanged(() => Long); }
        }

    }
}
