using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.ObjectModel;

namespace RunPlanner.Core.Entities
{
    public class PlannedRun
    {
        [Newtonsoft.Json.JsonProperty("userId")]
        public string Id { get; set; }

        [Microsoft.WindowsAzure.MobileServices.Version]
        public string AzureVersion { get; set; }

        [Newtonsoft.Json.JsonProperty("Points")]
        ObservableCollection<RunPoint> Points { get; set; }
    }
}
