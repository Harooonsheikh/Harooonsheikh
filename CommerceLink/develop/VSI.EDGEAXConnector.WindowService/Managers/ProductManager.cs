using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.MongoData.Helpers;
using VSI.EDGEAXConnector.WindowService.Managers;
using SyncJobs = VSI.EDGEAXConnector.Common.Enums.SyncJobs;

namespace VSI.EDGEAXConnector.WindowService
{
    /// <summary>
    /// ProductManager
    /// </summary>
    public class ProductManager : IJobManager
    {
        #region Properties & Variables
        public static readonly string IDENTIFIER = "ProductSync";
        public static readonly string GROUP = "Synchronization";
        public StoreDto store = null;
 
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
        public ProductManager()
        {
            this._erpAdapterFactory = new ErpAdapterFactory();
            this._eComAdapterFactory = new EComAdapterFactory();
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
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.ProductSync);
            return job;
        }
        public void SetStore(StoreDto store)
        {
            this.store = store;
        }
        
        public bool Sync()
        {

            bool useDelta = true;
            string instanceId = string.Empty;
            string strFileName = string.Empty;
            Stopwatch timer = Stopwatch.StartNew();
            timer.Start();
            CommerceLinkLogger.LogSyncTrace($"Entered Method [{MethodBase.GetCurrentMethod().Name}]" +
                                                          $"Catalog Sync Job Started");
            // Just for Logging Purpose
            List<ErpProduct> erpProductsLog = new List<ErpProduct>();
            try
            {
                ErpCatalog catalog = new ErpCatalog();
                try
                {
                    strFileName = configurationHelper.GetSetting(PRODUCT.Filename_Prefix) + store.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff");
                    CustomLogger.LogDebugInfo(string.Format("Catalog filename successfully generated"), store.StoreId, store.CreatedBy, strFileName);
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(string.Format("File Name is not generated"), store.StoreId, store.CreatedBy);
                    throw;
                }
                String content = String.Empty;
                //instanceId = WorkflowManger.Start(fileName, (int)SyncJobs.ProductSync, Entities.Product, WorkFlowType.Merchandise);
                //AF:Start
                //VW -Customizations , VW product struture and processing is for flat products
                bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();
                CustomLogger.LogDebugInfo(string.Format("Enter in @@@@@@@@@@@@@ SyncProducts() @@@@@@@@@@@@@ and Value of isFlatProductHierarchy is {0}", isFlatProductHierarchy), store.StoreId, store.CreatedBy);

                //Getting Categories
                var erpCategoryController = _erpAdapterFactory.CreateCategoryController(store.StoreKey);
                catalog.Categories = erpCategoryController.GetAllCategories();
                //AF:End
                //Getting Products                
                var erpProductsController = _erpAdapterFactory.CreateProductController(store.StoreKey);

                //TODO: Temp test code to enable all products 
                try
                {
                    useDelta = !Convert.ToBoolean(configurationHelper.GetSetting(PRODUCT.Delta_Disable));
                }
                catch
                {
                    throw;
                }
                CustomLogger.LogDebugInfo(string.Format("Enter in SyncProducts() and Value of isFlatProductHierarchy is {0}, Now going to getAllProducts", isFlatProductHierarchy), store.StoreId, store.CreatedBy);

                CommerceLinkLogger.LogSyncTrace($"Enter Method [erpProductsController.GetAllProducts]" +
                                                              $"CRT Calls Started");

                var erpProducts = erpProductsController.GetAllProducts(useDelta, catalog.Categories, true);

                CommerceLinkLogger.LogSyncTrace($"Exit Method [erpProductsController.GetAllProducts]" +
                                                              $"CRT Calls Completed");
                var listJson = erpProducts.SerializeToJson();
                erpProductsLog = JsonConvert.DeserializeObject<List<ErpProduct>>(listJson);
                //AF:Start
                if (!isFlatProductHierarchy)
                {
                    List<ErpProduct> erpCatalogMasterProducts = new List<ErpProduct>();
                    erpCatalogMasterProducts = erpProducts.FindAll(x => x.IsMasterProduct == true);
                    catalog.CatalogMasterProducts = erpCatalogMasterProducts;
                }
                if (erpProducts.Count > 0 && erpProducts != null)
                {
                    catalog.Products = erpProducts;

                    var erpConfigurationController = _erpAdapterFactory.CreateChannelConfigurationController(store.StoreKey);
                    ErpConfiguration erpConfiguration = new ErpConfiguration();
                    erpConfiguration.Channel = erpConfigurationController.GetChannelInformation();

                    // KAR - Code to fetch all master products assigned to catalog
                    using (var ecomProductController = _eComAdapterFactory.CreateProductController(store.StoreKey))
                    {
                        content= ecomProductController.PushProducts(catalog, erpConfiguration.Channel, strFileName);
                        CustomLogger.LogDebugInfo(string.Format("Exit from PushProducts()"), store.StoreId, store.CreatedBy);
                    }
                    //if (content != null)
                    //{
                    //    XmlDocument xmlDocument = JobHelper.convertStringToXML(content);
                    //    int chunkSize = Convert.ToInt32(configurationHelper.GetSetting(APPLICATION.Mongo_ChunkSize).ToString());
                    //    CatalogMongoHelper catalogMongoHelper = new CatalogMongoHelper(configurationHelper.GetSetting(APPLICATION.Mongo_Connection), configurationHelper.GetSetting(APPLICATION.Mongo_DBName));
                    //    catalogMongoHelper.SaveCatalog(strFileName, xmlDocument, chunkSize, store.Name);
                    //}
                }
                else
                {
                    CustomLogger.LogWarn(string.Format("No Products generated,Please check Logs"), store.StoreId, store.CreatedBy);
                }

                CommerceLinkLogger.LogSyncTrace($"Exit Method [{MethodBase.GetCurrentMethod().Name}]" +
                                                              $"Catalog Sync Job Completed. Total time taken {timer.Elapsed.TotalSeconds} seconds");
                timer.Stop();
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                emailSender.NotifyThroughEmail("", ex.ToString(), "", (int)Common.Enums.EmailTemplateId.Product);
                throw;
            }
            finally
            {
                LogCatalogData(erpProductsLog);
            }
        }

        #endregion

        #region Public Methods
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.ProductSync, this.store.StoreId);
        }
        public void InitializeParameter()
        {
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
            this.emailSender = new EmailSender(store.StoreKey);
        }

        public void LogCatalogData(List<ErpProduct> erpProducts)
        {
            try
            {
                if (configurationHelper.GetSetting(PRODUCT.Log_Data).ToLower() == "true")
                {
                    List<CatalogLogs> catalogLogs = new List<CatalogLogs>();
                    DateTime dateTime = DateTime.Now;
                    CatalogService catalogService = new CatalogService();

                    if (erpProducts != null && erpProducts.Count > 0)
                    {
                        foreach (var erpProduct in erpProducts)
                        {
                            string sku = erpProduct.SKU;
                            if (string.IsNullOrWhiteSpace(erpProduct.SKU))
                            {
                                sku = erpProduct.MasterProductNumber + "_" + erpProduct.ItemId;
                            }
                            catalogLogs.Add(new CatalogLogs() { Key = sku, Value = erpProduct.SerializeToJson() == null ? string.Empty : erpProduct.SerializeToJson(), CreatedOn = dateTime, StoreId = store.StoreId });

                        }
                        catalogService.LogCatalog(catalogLogs);
                    }
                    else
                    {
                        List<CatalogLogs> NoCatalogLogs = new List<CatalogLogs>();
                        NoCatalogLogs.Add(new CatalogLogs() { Key = "No Products Received from ERP", Value = string.Empty, CreatedOn = dateTime, StoreId = store.StoreId });
                        catalogService.LogCatalog(NoCatalogLogs);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

    }
}
