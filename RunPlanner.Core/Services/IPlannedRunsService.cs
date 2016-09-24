using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunPlanner.Core.Services
{
    public interface IPlannedRunsService
    {
        MobileServiceClient MobileService { get; set; }
        Task Initialize();
        Task<IEnumerable<Entities.PlannedRun>> GetPlannedRuns();
        Task AddPlannedRun();
        Task SyncPlannedRuns();
    }
}
