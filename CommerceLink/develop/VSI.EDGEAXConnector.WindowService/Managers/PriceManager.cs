using System;
using System.Collections.Generic;
using System.IO;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.MongoData.Helpers;
using VSI.EDGEAXConnector.WindowService.Managers;
using CsvHelper;
using VSI.EDGEAXConnector.Data.DTO;

namespace VSI.EDGEAXConnector.WindowService
{
    /// <summary>
    /// PriceManager
    /// </summary>
    public class PriceManager : IJobManager
    {
        public static readonly string IDENTIFIER = "PriceSync";
        public static readonly string GROUP = "Synchronization";
        public StoreDto store = null;
        #region Properties
        private readonly IErpAdapterFactory _erpAdapterFactory;
        private readonly IEComAdapterFactory _eComAdapterFactory;
        ConfigurationHelper configurationHelper;
        public EmailSender emailSender = null;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="erpAdapterFactory"></param>
        /// <param name="eComAdapterFactory"></param>
        public PriceManager()
        {
            this._erpAdapterFactory = new ErpAdapterFactory();
            this._eComAdapterFactory = new EComAdapterFactory();
        }
        public string GetIdentifier()
        {
            return IDENTIFIER;
        }

        public string GetGroup()
        {
            return GROUP;
        }
        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.PriceSync);
            return job;
        }
        public bool Sync()
        {
            try
            {
                string fileName = string.Empty;
                try
                {
                    fileName = configurationHelper.GetSetting(PRICE.Filename_Prefix) + store.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff");
                    CustomLogger.LogDebugInfo(string.Format("Price filename successfully generated"), store.StoreId, store.CreatedBy, fileName);
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(string.Format("File Name is not generated"), store.StoreId, store.CreatedBy);
                    throw;
                }
                String content = String.Empty;
                //Configure mappings 
                //AutoMapper.Mapper.Configuration.AddProfile<VSI.EDGEAXConnector.Mapper.MappingConfig.Configuration>();
                CustomLogger.LogDebugInfo(string.Format("Enter in @@@@@@@@@@@@@ SyncPrices() @@@@@@@@@@@@@"), store.StoreId, store.CreatedBy);
                var erpPriceController = _erpAdapterFactory.CreatePriceController(store.StoreKey);
                CustomLogger.LogDebugInfo(string.Format("Created Price Controllers"), store.StoreId, store.CreatedBy);
                var erpProductPrices = erpPriceController.GetDefaultProductPrice(); // New method by KAR
                CustomLogger.LogDebugInfo(string.Format("Exit from GetDefaultProductPrice()"), store.StoreId, store.CreatedBy);
                ErpPrice price = new ErpPrice();
                price.Currency = configurationHelper.GetSetting(CUSTOMER.Default_CurrencyCode); //TODO: Map with channel currency
                price.Online = true;
                price.Prices = erpProductPrices;
                using (var ecomProductController = _eComAdapterFactory.CreatePriceController(store.StoreKey))
                {
                    content= ecomProductController.PushAllProductPrice(price, fileName);
                    CustomLogger.LogDebugInfo(string.Format("Exit from PushAllProductPrice()"), store.StoreId, store.CreatedBy);
                }
                //if (content != null && configurationHelper.GetSetting(ECOM.Price_Output_Type).ToLower() == "csv")
                //{
                //    var PriceFileList = ReadPriceFile(fileName + "." + configurationHelper.GetSetting(ECOM.Price_Output_Type).ToLower());

                //    int chunkSize = Convert.ToInt32(configurationHelper.GetSetting(APPLICATION.Mongo_ChunkSize).ToString());
                //    PriceMongoHelper priceMongoHelper = new PriceMongoHelper(configurationHelper.GetSetting(APPLICATION.Mongo_Connection), configurationHelper.GetSetting(APPLICATION.Mongo_DBName));
                //    priceMongoHelper.SavePrice(fileName, PriceFileList, chunkSize, store.Name);
                //}
                CustomLogger.LogDebugInfo(string.Format("@@@@@@@@@@@@@ End SyncPrices() @@@@@@@@@@@@@"), store.StoreId, store.CreatedBy);
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                emailSender.NotifyThroughEmail("", ex.ToString(), "", (int)Common.Enums.EmailTemplateId.Price);
                throw;
            }
        }
        List<PriceEntity> ReadPriceFile(string fileName)
        {
            var listOfPrice = new List<KeyValuePair<string, string>>();
            List<PriceEntity> priceList = new List<PriceEntity>();

            string path = @configurationHelper.GetSetting(PRICE.local_Output_Path) + "\\" + fileName;

            using (var reader = new StreamReader(path))
            {
                using (var csv = new CsvReader(reader))
                {
                    var records = csv.GetRecords<dynamic>();
                    PriceEntity priceEntity = new PriceEntity();

                    foreach (var priceRecord in records)
                    {
                        priceEntity = new PriceEntity { sku = priceRecord.sku, price = priceRecord.price, attribute_set_code = priceRecord.attribute_set_code, product_type = priceRecord.product_type };
                        priceList.Add(priceEntity);
                    }
                }
            }

            return priceList;
        }
        #endregion

        #region Public Methods
        public void SetStore(StoreDto store)
        {
            this.store = store;
        }
        public void UpdateJobStatus(JobSchedule jobSchedule, Common.Enums.SynchJobStatus status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.UpdateJobStatus(jobSchedule.JobId, (int)status, this.store.StoreId);
        }
        public void JobLog(JobSchedule jobSchedule, int status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.JobLog(jobSchedule.JobId, status);
        }
        public bool IsJobCompletedTodayInJobLog(JobSchedule jobSchedule, int jobStatus)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.IsJobCompletedTodayInJobLog(jobSchedule.JobId, jobStatus);
        }
        public JobSchedule GetSchedule()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.PriceSync, this.store.StoreId);
        }
        public void InitializeParameter()
        {
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
            this.emailSender = new EmailSender(store.StoreKey);
        }
        #endregion

    }
}
