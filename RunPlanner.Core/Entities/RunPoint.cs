using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunPlanner.Core.Entities
{
    public class RunPoint : MvxCoordinates
    {
        private string _name;
        public string Name
        {
            get { return _name;  }
            set { _name = value; }
        }

    }
}
