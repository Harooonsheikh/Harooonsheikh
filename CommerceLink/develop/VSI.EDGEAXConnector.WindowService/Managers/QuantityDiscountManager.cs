using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Data.DTO;

namespace VSI.EDGEAXConnector.WindowService.Managers
{
    public class QuantityDiscountManager : IJobManager
    {
        public readonly string IDENTIFIER = "QuantityDiscountSync";
        public readonly string GROUP = "Synchronization";
        private readonly IEComAdapterFactory _eComAdapterFactory;
        private readonly IErpAdapterFactory _erpAdapterFactory;
        public EmailSender emailSender = null;

        public StoreDto store;

        public QuantityDiscountManager()
        {
            _erpAdapterFactory = new ErpAdapterFactory();
            _eComAdapterFactory = new EComAdapterFactory();
        }
        public void UpdateJobStatus(JobSchedule jobSchedule, Common.Enums.SynchJobStatus status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.UpdateJobStatus(jobSchedule.JobId, (int)status, this.store.StoreId);
        }
        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.DiscountSync);
            return job;
        }
        public string GetIdentifier()
        {
            return IDENTIFIER;
        }
        public string GetGroup()
        {
            return GROUP;
        }
        public bool Sync()
        {
            StringBuilder traceInfo = new StringBuilder();
            try
            {
                Dictionary<long, List<ErpProduct>> categoryErpProductDictionary = new Dictionary<long, List<ErpProduct>>();

                ErpCatalog catalog = new ErpCatalog();
                CustomLogger.LogDebugInfo(string.Format("Enter in @@@@@@@@@@@@@ Sync() for QuantityDiscount @@@@@@@@@@@@@"), store.StoreId, store.CreatedBy);

                //Getting Categories
                var erpCategoryController = _erpAdapterFactory.CreateCategoryController(store.StoreKey);
                catalog.Categories = erpCategoryController.GetAllCategories();

                //Getting Products                
                var erpProductsController = _erpAdapterFactory.CreateProductController(store.StoreKey);

                CustomLogger.LogDebugInfo(string.Format("Entered in Sync() for QuantityDiscount now going to call GetAllProducts"), store.StoreId, store.CreatedBy);
                var erpProducts = erpProductsController.GetAllProducts(false, catalog.Categories, false);

                CustomLogger.LogDebugInfo(string.Format("Initializing ERP Adapter & Discount Controller"), store.StoreId, store.CreatedBy);
                var erpDiscountController = _erpAdapterFactory.CreateQuantityDiscountController(store.StoreKey);
                CustomLogger.LogDebugInfo(string.Format("Calling ERP Adapter's GetQuantityDiscount() Method "), store.StoreId, store.CreatedBy);
                List <ErpProductQuantityDiscount> erpProductQuantityDiscount = erpDiscountController.GetQuantityDiscount();
                CustomLogger.LogDebugInfo(string.Format("GetDiscounts() Method call finished"), store.StoreId, store.CreatedBy);
                // traceInfo.Append(string.Format("Session [{0}]: GetDiscounts() Method call finished at [{1}] ", sessionId, DateTime.UtcNow) + Environment.NewLine);

                ErpQuantityDiscount erpQuantityDiscount = new ErpQuantityDiscount();
                erpQuantityDiscount.QuantityDiscounts = erpProductQuantityDiscount;

                traceInfo.Append(string.Format("ErpProductQuantityDiscount to ErpQuantityDiscount conversion completed at [{0}]. Now starting to generate discount xml ", DateTime.UtcNow) + Environment.NewLine);
                using (var ecomQuantityDiscountController = _eComAdapterFactory.CreateIQuantityDiscountController(store.StoreKey))
                {
                    ecomQuantityDiscountController.PushAllQuantityDiscounts(erpQuantityDiscount, erpProducts);
                    CustomLogger.LogDebugInfo(string.Format("Quantity Discounts xml generation completed"), store.StoreId, store.CreatedBy);
                }
                CustomLogger.LogTraceInfo(traceInfo.ToString(), store.StoreId, store.CreatedBy);
                CustomLogger.LogDebugInfo(string.Format(" @@@@@@@@@@@@@ Completed Quantity Discount Sync() @@@@@@@@@@@@@"), store.StoreId, store.CreatedBy);
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                emailSender.NotifyThroughEmail("", ex.ToString(), "", (int)Common.Enums.EmailTemplateId.QuantityDiscount);
                throw;
            }
        }
        public void SetStore(StoreDto store)
        {
            this.store = store;
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.QuantityDiscountSync, this.store.StoreId);
        }
        public void InitializeParameter()
        {
            this.emailSender = new EmailSender(store.StoreKey);
        }
    }
}
