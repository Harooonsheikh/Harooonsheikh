using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.FilesSyncService;
using VSI.EDGEAXConnector.FilesSyncService.Jobs;
using VSI.EDGEAXConnector.FilesSyncService.Managers;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.SFTPlib;

namespace VSI.EDGEAXConnector.FileSyncService
{

    /// <summary>
    /// EDGEAXConnectorFilesSyncService used to scheduled jobs.
    /// </summary>
    public partial class EDGEAXConnectorFileSyncService : ServiceBase
    {
        public const string EventLogName = "CommerceLink";
        public const string ServiceDisplayName = "CommerceLink File Service";
        private List<long> jobScheduleIds = new List<long>();
        private long DelayInSeconds = 0;

        #region Data Members

        /// <summary>
        /// Used to get scheduler.
        /// </summary>
        IScheduler _schedule;
        #endregion

        #region Constructor

        /// <summary>
        /// Used to intialize new instance of class.
        /// </summary>
        public EDGEAXConnectorFileSyncService()
        {
            InitializeComponent();
            //Setup event logging
            ((ISupportInitialize)this.EventLog).BeginInit();
            if (CheckSourceExists(this.ServiceName))
            {
                this.EventLog.Source = this.ServiceName;
                this.EventLog.Log = EventLogName;
                this.AutoLog = false;
            }
            else
            {
                this.AutoLog = true;
            }
            ((ISupportInitialize)this.EventLog).EndInit();

        }

        #endregion

        // For Manually Starting in Debug Mode
        public void Start()
        {
            OnStart(new List<string>().ToArray());
        }
        #region Start Stop Events

        /// <summary>
        /// OnStart method used to start File Sync Service.
        /// </summary>
        /// <param name="args"></param>
        /// 

        protected override void OnStart(string[] args)
        {
            //System.Diagnostics.Debugger.Launch();
            WriteEvent(this.ServiceName + " starting scheduler", EventLogEntryType.Information);
            CustomLogger.LogSyncTrace(0, 0, this.ServiceName, "Scheduler Attempt to start", $"");

            try
            {
                NameValueCollection properties = new NameValueCollection();
                properties["quartz.threadPool.threadCount"] = ConfigurationManager.AppSettings["ThreadCountOfJobs"];
                long.TryParse(ConfigurationManager.AppSettings["MissFireThresholdInSeconds"], out long missFireThresholdInSeconds);
                if (missFireThresholdInSeconds != 0)
                {
                    properties["quartz.jobStore.misfireThreshold"] = TimeSpan.FromSeconds(missFireThresholdInSeconds).TotalMilliseconds.ToString();
                }

                CustomLogger.LogSyncTrace(0, 0, this.ServiceName, "Thread Count Setting", "ThreadCount: " + properties["quartz.threadPool.threadCount"]);
                // Schedule jobs
                ISchedulerFactory schedFact = new StdSchedulerFactory(properties);
                _schedule = schedFact.GetScheduler();
                CustomLogger.LogSyncTrace(0, 0, this.ServiceName, "Thread Pool Size", "ThreadCount: " + _schedule.GetMetaData().ThreadPoolSize);
                _schedule.ListenerManager.AddTriggerListener(new MisfireLogger());
                _schedule.Start();
                WriteEvent(this.ServiceName + " started scheduler", EventLogEntryType.Information);
                CustomLogger.LogSyncTrace(0, 0, this.ServiceName, "Scheduler Started Successfully", $"");
                CreateSFTPDirStructure();
                CustomLogger.LogSyncTrace(0, 0, this.ServiceName, "CreateSFTPDirStructure Successfully", $"");
                AddJobs();
                CustomLogger.LogSyncTrace(0, 0, this.ServiceName, "AddJobs Successfully", $"");

            }
            catch (Exception e)
            {
                WriteEvent(this.ServiceName + " failed to start with exception " + e.Message, EventLogEntryType.Error);
                CustomLogger.LogSyncTrace(0, 0, this.ServiceName, "failed to start with exception", $"");
                // EmailSender.NotifyThroughEmail("(FileSyncService) Unexpectedly.", e.ToString(), (int)EmailTemplateId.SimpleNotification);TODO
                ExitCode = 1064;
                throw;
            }

        }

        /// <summary>
        /// OnStop method used to stop service.
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                StoreService.ResetJobsStatus(jobScheduleIds, 1);
                _schedule.Shutdown();
                WriteEvent(this.ServiceName + " stopped successfully", EventLogEntryType.Information);
                //EmailSender.NotifyThroughEmail("(FileSyncService) Manually.", "No Exception", (int)EmailTemplateId.SimpleNotification);TODO
                base.OnStop();
            }
            catch (Exception e)
            {
                WriteEvent(this.ServiceName + " failed to stop with exception " + e.Message, EventLogEntryType.Error);
                throw e;
            }
        }

        #endregion

        #region Add Jobs Code

        /// <summary>
        /// AddJobs used to call all jobs.
        /// </summary>
        public void AddJobs()
        {
            //System.Diagnostics.Debugger.Launch();
            //++ setting Up Stores 
            var storeKeys = ConfigurationManager.AppSettings["STORESKEY"].ToString();

            /*****************************************************************
             * NOTE:
             * 1. PLEASE UPDATE COLUMN "IsSFTPDirTreeCreated" IN "Store" TABLE
             * WITH VALUE "0" FOR AUTOMATIC CREATION OF FOLDER ON SFTP
             * 2. ALSO ADD THE SFTP PATH IN METHOD GetStoreRemotePathList()
             * IN CASE A NEW JOB IS CREATED
             * ***************************************************************/

            //++ Need to load if all stores if storeKeys are empty. 

            var ignoreStoreKeys = ConfigurationManager.AppSettings["IgnoreStoreKeys"].ToString();

            var stores = StoreService.GetStoreByKeys(storeKeys, ignoreStoreKeys);

            //Fetch File Sync jobs only
            List<JobAndScheduleModel> fileSyncjobs = StoreService.GetAllActiveJobsOfType(stores, true);
            jobScheduleIds = fileSyncjobs.Select(x => x.JobSchduleId).ToList();
            foreach (JobAndScheduleModel activeJob in fileSyncjobs)
            {
                //customLogger.LogSyncTrace($"{this.ServiceName} :: Adding {activeJob.JobName}", activeJob.JobID.ToString(), activeJob.storeId.ToString(), DateTime.UtcNow);
                CustomLogger.LogSyncTrace(activeJob.storeId, activeJob.JobID,activeJob.JobName, "Adding Job - JobScheduleLog", activeJob.JobSchduleId.ToString());

                if (activeJob.JobName == "UploadProductSync")
                {
                    ProductManager productManager = new ProductManager();
                    productManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, productManager);
                }
                else if (activeJob.JobName == "UploadPriceSync")
                {
                    PriceManager priceManager = new PriceManager();
                    priceManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, priceManager);
                }
                else if (activeJob.JobName == "DownloadCustomerSync")
                {
                    CustomerManager customerManager = new CustomerManager();
                    customerManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, customerManager);
                }
                else if (activeJob.JobName == "DownloadSalesOrderSync")
                {
                    SalesOrderManager salesOrderManager = new SalesOrderManager();
                    salesOrderManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, salesOrderManager);
                }
               else if (activeJob.JobName == "UploadInventorySync")
                {
                    InventoryManager inventoryManager = new InventoryManager();
                    inventoryManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, inventoryManager);
                }
                else if (activeJob.JobName == "UploadDiscountSync")
                {
                    DiscountManager discountManager = new DiscountManager();
                    discountManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, discountManager);
                }
                else if (activeJob.JobName == "UploadDiscountWithAffiliationSync")
                {
                    DiscountWithAffiliationManager discountWithAffiliationManager = new DiscountWithAffiliationManager();
                    discountWithAffiliationManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, discountWithAffiliationManager);
                }
                else if (activeJob.JobName == "UploadQuantityDiscountSync")
                {
                    QuantityDiscountManager quantityDiscountManager = new QuantityDiscountManager();
                    quantityDiscountManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, quantityDiscountManager);
                }
                else if (activeJob.JobName == "UploadQuantityDiscountWithAffiliationSync")
                {
                    QuantityDiscountWithAffiliationManager quantityDiscountWithAffiliationManager = new QuantityDiscountWithAffiliationManager();
                    quantityDiscountWithAffiliationManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, quantityDiscountWithAffiliationManager);
                }
                else if (activeJob.JobName == "UploadSalesOrderStatusSync")
                {
                    SalesOrderStatusManager salesOrderStatusManager = new SalesOrderStatusManager();
                    salesOrderStatusManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, salesOrderStatusManager);
                }
                else if (activeJob.JobName == "UploadQuotationReasonGroupSync")
                {
                    QuotationReasonGroupManager quotationReasonGroupManager = new QuotationReasonGroupManager();
                    quotationReasonGroupManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, quotationReasonGroupManager);
                }
                else if (activeJob.JobName == "UploadConfigurationSync")
                {
                    ChannelConfigurationManager channelConfigurationManager = new ChannelConfigurationManager();
                    channelConfigurationManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, channelConfigurationManager);
                }
            }
        }
        private void AddSyncJob(JobAndScheduleModel job, int storeId, IJobManager jobManager)
        {
            JobDataMap map = new JobDataMap();
            map["manager"] = jobManager;

            IJobDetail jobSynch = JobBuilder.Create<DataSyncJob>()
                .WithIdentity((jobManager.GetIdentifier() + storeId), jobManager.GetGroup())
                .UsingJobData(map)
                .Build();
            CustomLogger.LogSyncTrace(job.storeId, job.JobID, job.JobSchduleId.ToString(), "AddSyncJob Method", "JobSync Build Success");

            if (job.JobInterval > 0)
            {
                TriggerBuilder triggerBuilder = TriggerBuilder.Create()
                  .WithIdentity((jobManager.GetIdentifier() + storeId), jobManager.GetGroup());
                long.TryParse(ConfigurationManager.AppSettings["JobsDelayInSeconds"], out long jobsDelay);
                DelayInSeconds += jobsDelay;
                if (jobsDelay > 0)
                {
                    triggerBuilder = triggerBuilder.StartAt(DateTime.UtcNow.AddSeconds(DelayInSeconds));
                }
                else
                {
                    triggerBuilder = triggerBuilder.StartNow();
                }

                ITrigger trigger = triggerBuilder.WithSimpleSchedule(x => x
                      .WithIntervalInMinutes((int)job.JobInterval)
                      .RepeatForever()
                      //.WithMisfireHandlingInstructionIgnoreMisfires() //Use this to Ignore Missfires and run all jobs
                      )
                  .Build();
                _schedule.ScheduleJob(jobSynch, trigger);
                CustomLogger.LogSyncTrace(job.storeId, job.JobID, job.JobSchduleId.ToString(), "AddSyncJob Method", $"Job Scheduled Interval:{job.JobInterval}");
            }
            else
            {
                TimeSpan timeSpan = (TimeSpan)job.StartTime;
                ITrigger trigger = TriggerBuilder.Create()
              .WithIdentity(jobManager.GetIdentifier() + storeId, jobManager.GetGroup())
              .WithDailyTimeIntervalSchedule(s => s.OnEveryDay().StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(timeSpan.Hours, timeSpan.Minutes))
              .EndingDailyAfterCount(1)).Build();
                _schedule.ScheduleJob(jobSynch, trigger);
                CustomLogger.LogSyncTrace(job.storeId, job.JobID, job.JobSchduleId.ToString(), "AddSyncJob Method", $"Job Scheduled JobTime:{timeSpan.Hours}:{timeSpan.Minutes}");
            }
        }
        #endregion

        #region private method
        private void CreateSFTPDirStructure()
        {
            try
            {
                List<Store> Storelst = StoreService.GetStoresForSFTPDirStructCreation();

                foreach (var store in Storelst)
                {
                    SFTPManager manager = new SFTPManager(store.StoreKey);

                    List<String> remotePathlst = GetStoreRemotePathList(store.StoreKey);
                    manager.CreateServerDirectoryIfItDoesntExist(remotePathlst);

                    StoreService.UpdateIsSFTPDirCreatedFlag(store.StoreId, true);
                }
            }
            catch (Exception ex)
            {
                WriteEvent(this.ServiceName + " failed to create SFTP directory structure " + ex.Message + Environment.NewLine + ex.InnerException + Environment.NewLine + ex.StackTrace, EventLogEntryType.Error);
            }
        }

        private List<string> GetStoreRemotePathList(string storeKey)
        {
            List<String> remotePathlst = new List<string>();

            try
            {
                ConfigurationHelper configurationHelper = new ConfigurationHelper(storeKey);

                remotePathlst.Add(configurationHelper.GetSetting(ADDRESS.Remote_Path_Deleted));
                remotePathlst.Add(configurationHelper.GetSetting(CUSTOMER.Remote_File_Path));
                remotePathlst.Add(configurationHelper.GetSetting(CUSTOMER.Remote_Upload_SFTP_Path));
                remotePathlst.Add(configurationHelper.GetSetting(PRODUCT.Image_Remote_Path));
                remotePathlst.Add(configurationHelper.GetSetting(PRODUCT.Remote_Path));
                remotePathlst.Add(configurationHelper.GetSetting(CHANNELCONFIGURATION.Remote_Path));
                remotePathlst.Add(configurationHelper.GetSetting(DISCOUNT.Remote_Path));
                remotePathlst.Add(configurationHelper.GetSetting(INVENTORY.Remote_Path));
                remotePathlst.Add(configurationHelper.GetSetting(SALESORDER.Remote_Input_Path));
                remotePathlst.Add(configurationHelper.GetSetting(SALESORDER.Status_Remote_Path));
                remotePathlst.Add(configurationHelper.GetSetting(PRICE.Remote_Path));
                remotePathlst.Add(configurationHelper.GetSetting(QUANTITYDISCOUNT.Remote_Path));
                remotePathlst.Add(configurationHelper.GetSetting(QUANTITYDISCOUNTWITHAFFILIATION.Remote_Path));
                remotePathlst.Add(configurationHelper.GetSetting(QUOTATIONREASONGROUP.Remote_Path));
                remotePathlst.Add(configurationHelper.GetSetting(DISCOUNTWITHAFFILIATION.Remote_Path));
            }
            catch (Exception)
            {

                throw;
            }

            return remotePathlst;
        }
        #endregion

        #region Public Methods

        public static void WriteEvent(String text, EventLogEntryType type)
        {
            if (CheckSourceExists(ServiceDisplayName))
            {
                EventLog.WriteEntry(ServiceDisplayName, text, type);
            }
        }
        
        private static bool CheckSourceExists(string source)
        {
            try
            {
                if (EventLog.SourceExists(source))
                {
                    EventLog evLog = new EventLog { Source = source };
                    if (evLog.Log != EventLogName)
                    {
                        EventLog.DeleteEventSource(source);
                    }
                }
                if (!EventLog.SourceExists(source))
                {
                    EventLog.CreateEventSource(source, EventLogName);
                }
                return EventLog.SourceExists(source);
            }
            catch (Exception)
            {
                // Probably run without admin privileges
                return false;
            }
        }
        #endregion
    }
}

