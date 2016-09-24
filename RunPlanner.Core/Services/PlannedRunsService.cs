using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Humanizer;
using MvvmCross.Platform.Platform;

namespace RunPlanner.Core.Services
{
    public class PlannedRunsService : IPlannedRunsService
    {
        IMobileServiceSyncTable<Entities.PlannedRun> plannedRunsTable;
        public MobileServiceClient MobileService
        {
            get;set;
        }
        private bool _isinitialized = false;

        #region Initialize
        public async Task Initialize()
        {
            if (_isinitialized) return;
            MobileService = new MobileServiceClient("https://runplanner.azurewebsites.net");
            const string path = "syncstore.db";
            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<Entities.PlannedRun>();
            await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());
            plannedRunsTable = MobileService.GetSyncTable<Entities.PlannedRun>();
            _isinitialized = true;
        }
        #endregion

        #region Sync
        public async Task SyncPlannedRuns()
        {
            try
            {
                await plannedRunsTable.PullAsync("plannedRuns", plannedRunsTable.CreateQuery());
                await MobileService.SyncContext.PushAsync();
            }
            catch(Exception ex)
            {
                MvxTrace.Trace("Unable to sync planned runs at the moment:\n" + ex.Message);
            }
        }
        #endregion
        public async Task AddPlannedRun()
        {
            await Initialize();

        }

        public async Task<IEnumerable<Entities.PlannedRun>> GetPlannedRuns()
        {
            await Initialize();
            await SyncPlannedRuns();
            return await plannedRunsTable.OrderBy(pr => pr.AzureVersion).ToEnumerableAsync();
        }
    }
}
