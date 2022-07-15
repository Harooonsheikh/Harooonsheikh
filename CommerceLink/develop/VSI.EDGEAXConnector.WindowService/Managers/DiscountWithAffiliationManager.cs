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
using System.Linq;
using VSI.EDGEAXConnector.Data.DTO;

namespace VSI.EDGEAXConnector.WindowService.Managers
{
    public class DiscountWithAffiliationManager : IJobManager
    {
        public readonly string IDENTIFIER = "DiscountWithAffiliationSync";
        public readonly string GROUP = "Synchronization";
        private readonly IEComAdapterFactory _eComAdapterFactory;
        private readonly IErpAdapterFactory _erpAdapterFactory;
        public EmailSender emailSender = null;

        public StoreDto store;

        public DiscountWithAffiliationManager()
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
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.DiscountWithAffiliationSync);
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


                CustomLogger.LogDebugInfo(string.Format("@@@@@@@@@@@@@ Enter in SyncDiscountWithAffiliations() @@@@@@@@@@@@@"), store.StoreId, store.CreatedBy);
                CustomLogger.LogDebugInfo(string.Format("Initializing ERP Adapter & Discount With Affiliation Controller"), store.StoreId, store.CreatedBy);
                var erpDiscountController = _erpAdapterFactory.CreateDiscountController(store.StoreKey);
                var erpDiscountWithAffiliationController = _erpAdapterFactory.CreateDiscountWithAffiliationController(store.StoreKey);
                CustomLogger.LogDebugInfo(string.Format("Calling ERP Adapter's GetDiscounts() Method "), store.StoreId, store.CreatedBy);
                List<ErpProductDiscountWithAffiliation> erpDiscount = erpDiscountWithAffiliationController.GetDiscountsWithAffiliation();

                erpDiscount = erpDiscount.GroupBy(disc => new
                {
                    disc.SKU,
                    disc.AffiliationId,
                    disc.AffiliationName,
                    disc.OfferPrice,
                    disc.ValidationFrom,
                    disc.ValidationTo,
                    disc.OfferId,
                    disc.OfferName,
                    disc.PeriodicDiscountType,
                    disc.DiscountType,
                    disc.DiscountMethod,
                    disc.DiscPct,
                    disc.DiscAmount,
                    disc.DiscPrice,
                    disc.CurrencyCode
                }
                ).Select(group => group.First()).ToList();

                CustomLogger.LogDebugInfo(string.Format("GetDiscounts() Method call finished"), store.StoreId, store.CreatedBy);
                // traceInfo.Append(string.Format("Session [{0}]: GetDiscounts() Method call finished at [{1}] ", sessionId, DateTime.UtcNow) + Environment.NewLine);
                // VW Discount Code
                ErpDiscountWithAffiliation discountWithAffiliations = new ErpDiscountWithAffiliation();
                discountWithAffiliations.Currency = "USD";
                discountWithAffiliations.Online = true;
                discountWithAffiliations.Name = "USD Sale Prices";
                discountWithAffiliations.OfferId = "FinalPrice";
                discountWithAffiliations.Discounts = erpDiscount;
                discountWithAffiliations.ValidFrom = Convert.ToString(DateTime.UtcNow.Date);
                discountWithAffiliations.ValidTo = Convert.ToString(new DateTime(2154, 12, 31));

                traceInfo.Append(string.Format("ErpProductDiscount to ErpDiscountWithAffiliation conversion completed at [{0}]. Now starting to generate discount xml ", DateTime.UtcNow) + Environment.NewLine);
                using (var ecomDiscountWithAffilationController = _eComAdapterFactory.CreateDiscountWithAffiliationController(store.StoreKey))
                {
                    ecomDiscountWithAffilationController.PushAllProductDiscountWithAffiliations(discountWithAffiliations);
                    CustomLogger.LogDebugInfo(string.Format("Discounts xml generation completed"), store.StoreId, store.CreatedBy);
                }
                CustomLogger.LogTraceInfo(traceInfo.ToString(), store.StoreId, store.CreatedBy);
                CustomLogger.LogDebugInfo(string.Format(" @@@@@@@@@@@@@ Completed SyncDiscountWithAffiliations() @@@@@@@@@@@@@"), store.StoreId, store.CreatedBy);
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                emailSender.NotifyThroughEmail("", ex.ToString(), "", (int)Common.Enums.EmailTemplateId.Discount);
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.DiscountWithAffiliationSync, this.store.StoreId);
        }
        public void InitializeParameter()
        {
            this.emailSender = new EmailSender(store.StoreKey);
        }
    }
}
