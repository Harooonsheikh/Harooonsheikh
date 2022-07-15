using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.MongoData.Helpers;

namespace VSI.EDGEAXConnector.WindowService.Managers
{
    public class DiscountManager : IJobManager
    {
        public  readonly string IDENTIFIER = "DiscountSync";
        public  readonly string GROUP = "Synchronization";
        private readonly IEComAdapterFactory _eComAdapterFactory;
        private readonly IErpAdapterFactory _erpAdapterFactory;
        public EmailSender emailSender = null;
        ConfigurationHelper configurationHelper;
        public StoreDto store; 

        public DiscountManager()
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
                string fileName = string.Empty;
                try
                {
                    fileName = configurationHelper.GetSetting(DISCOUNT.Filename_Prefix) + store.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff");
                    CustomLogger.LogDebugInfo(string.Format("Discount filename successfully generated"), store.StoreId, store.CreatedBy, fileName);
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(string.Format("File Name is not generated"), store.StoreId, store.CreatedBy);
                    throw;
                }
                String content = String.Empty;

                CustomLogger.LogDebugInfo(string.Format("@@@@@@@@@@@@@ Enter in SyncDiscounts() @@@@@@@@@@@@@"), store.StoreId, store.CreatedBy);
                CustomLogger.LogDebugInfo(string.Format("Initializing ERP Adapter & Discount Controller"), store.StoreId, store.CreatedBy);
                var erpDiscountController = _erpAdapterFactory.CreateDiscountController(store.StoreKey);
                CustomLogger.LogDebugInfo(string.Format("Calling ERP Adapter's GetDiscounts() Method "), store.StoreId, store.CreatedBy);
                List<ErpProductDiscount> erpDiscount = erpDiscountController.GetDiscounts();
                CustomLogger.LogDebugInfo(string.Format("GetDiscounts() Method call finished"), store.StoreId, store.CreatedBy);
                // traceInfo.Append(string.Format("Session [{0}]: GetDiscounts() Method call finished at [{1}] ", sessionId, DateTime.UtcNow) + Environment.NewLine);
                // VW Discount Code
                ErpDiscount discounts = new ErpDiscount();
                discounts.Currency = "USD";
                discounts.Online = true;
                discounts.Name = "USD Sale Prices";
                discounts.OfferId = "FinalPrice";
                discounts.Discounts = erpDiscount;
                discounts.ValidFrom = Convert.ToString(DateTime.UtcNow.Date);
                discounts.ValidTo = Convert.ToString(new DateTime(2154, 12, 31));

                traceInfo.Append(string.Format("ErpProductDiscount to ErpDiscount conversion completed at [{0}]. Now starting to generate discount xml ", DateTime.UtcNow) + Environment.NewLine);
                using (var ecomDiscountController = _eComAdapterFactory.CreateDiscountController(store.StoreKey))
                {
                    content = ecomDiscountController.PushAllProductDiscounts(discounts, fileName);
                    CustomLogger.LogDebugInfo(string.Format("Discounts xml generation completed"), store.StoreId, store.CreatedBy);
                }
                //if (content != null && configurationHelper.GetSetting(ECOM.Discount_Output_Type).ToLower() == "csv")
                //{
                //    var discountFileList = ReadDiscountFile(fileName + "." + configurationHelper.GetSetting(ECOM.Discount_Output_Type).ToLower());

                //    int chunkSize = Convert.ToInt32(configurationHelper.GetSetting(APPLICATION.Mongo_ChunkSize).ToString());
                //    DiscountMongoHelper discountMongoHelper = new DiscountMongoHelper(configurationHelper.GetSetting(APPLICATION.Mongo_Connection), configurationHelper.GetSetting(APPLICATION.Mongo_DBName));
                //    discountMongoHelper.SaveDiscount(fileName, discountFileList, chunkSize, store.Name);
                //}
                CustomLogger.LogTraceInfo(traceInfo.ToString(), store.StoreId, store.CreatedBy);
                CustomLogger.LogDebugInfo(string.Format(" @@@@@@@@@@@@@ Completed SyncDiscounts() @@@@@@@@@@@@@"), store.StoreId, store.CreatedBy);
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                emailSender.NotifyThroughEmail("", ex.ToString(), "", (int)Common.Enums.EmailTemplateId.Discount);
                throw;
            }
        }
        List<DiscountEntity> ReadDiscountFile(string fileName)
        {
            List<DiscountEntity> listOfDiscount = new List<DiscountEntity>();
            string path = @configurationHelper.GetSetting(DISCOUNT.Local_Output_Path) + "\\" + fileName;

            using (var reader = new StreamReader(path))
            {
                using (var csv = new CsvReader(reader))
                {
                    var records = csv.GetRecords<dynamic>();
                    DiscountEntity discountEntity = new DiscountEntity();

                    foreach (var discountRecord in records)
                    {
                        discountEntity = new DiscountEntity { sku = discountRecord.sku, special_price = discountRecord.special_price, special_price_from_date = discountRecord.special_price_from_date, special_price_to_date = discountRecord.special_price_to_date };
                        listOfDiscount.Add(discountEntity);
                    }
                }
            }

            return listOfDiscount;
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.DiscountSync, this.store.StoreId);
        }
        public void InitializeParameter()
        {
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
            this.emailSender = new EmailSender(store.StoreKey);
        }
    }
}
