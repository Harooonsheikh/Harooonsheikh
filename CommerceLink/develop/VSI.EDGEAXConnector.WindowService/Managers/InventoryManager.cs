using System;
using System.Collections.Generic;
using System.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Common;
using System.Xml;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.WindowService.Managers;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Data.DTO;

namespace VSI.EDGEAXConnector.WindowService
{
    public class InventoryManager : IJobManager
    {
        #region Properies 
        public static readonly string IDENTIFIER = "InventorySync";
        public static readonly string GROUP = "Synchronization";
        private readonly IEComAdapterFactory _eComAdapterFactory;
        private readonly IErpAdapterFactory _erpAdapterFactory;
        public StoreDto store = null;
        public EmailSender emailSender = null;

        #endregion

        #region Constructor 
        public InventoryManager()
        {
            _erpAdapterFactory = new ErpAdapterFactory();
            _eComAdapterFactory = new EComAdapterFactory();
        }
        #endregion

        #region Public Functions
        public void SetStore(StoreDto store)
        {
            this.store = store;
        }
        public string GetGroup()
        {
            return GROUP;
        }

        public string GetIdentifier()
        {
            return IDENTIFIER;
        }

        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.InventorySynch);
            return job;
        }

        public bool Sync()
        {
            CustomLogger.LogDebugInfo(string.Format(" @@@@@@@@@@@@@ Enter in SyncInventory() @@@@@@@@@@@@@"), store.StoreId, store.CreatedBy);
            try
            {
                CustomLogger.LogDebugInfo(string.Format("Initializing ERP Adapter and Create Inventory Controller"), store.StoreId, store.CreatedBy);
                var erpInventoryController = _erpAdapterFactory.CreateInventoryController(store.StoreKey);
                CustomLogger.LogDebugInfo(string.Format(" Calling ERP Adapter's GetInventory() Method  Started"), store.StoreId, store.CreatedBy);
                List<ErpProduct> erpInventory = erpInventoryController.GetInventory();
                CustomLogger.LogDebugInfo(string.Format("GetInventory() Method call finished"), store.StoreId, store.CreatedBy);
                ErpInventoryProducts inventoryProd = new ErpInventoryProducts();

                inventoryProd.Description = "Inventory";
                inventoryProd.DefaultInstock = false;
                inventoryProd.UseBundleInventory = true;
                if (erpInventory.Count > 0 && erpInventory != null)
                    inventoryProd.Products = erpInventory;

                using (var ecomInventoryController = _eComAdapterFactory.CreateInventoryController(store.StoreKey))
                {
                    ecomInventoryController.PushAllProductInventory(inventoryProd);
                    CustomLogger.LogDebugInfo(string.Format("Inventory xml generation completed"), store.StoreId, store.CreatedBy);
                }
                CustomLogger.LogDebugInfo(string.Format("SyncInventory() Completed"), store.StoreId, store.CreatedBy);
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                emailSender.NotifyThroughEmail("", ex.ToString(), "", (int)Common.Enums.EmailTemplateId.Inventory);
                throw;
            }
        }


        public void UpdateJobStatus(JobSchedule jobSchedule, Common.Enums.SynchJobStatus status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.UpdateJobStatus(jobSchedule.JobId, (int)status, this.store.StoreId);
        }

        public void JobLog(JobSchedule jobLog, int status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.JobLog(jobLog.JobId, status);
        }
        public bool IsJobCompletedTodayInJobLog(JobSchedule jobSchedule, int jobStatus)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.IsJobCompletedTodayInJobLog(jobSchedule.JobId, jobStatus);
        }

        public JobSchedule GetSchedule()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.InventorySynch, this.store.StoreId);
        }

        public void InitializeParameter()
        {
            this.emailSender = new EmailSender(store.StoreKey);
        }
        #endregion
    }
}

