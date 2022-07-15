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
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.WindowService.Jobs;
using VSI.EDGEAXConnector.WindowService.Managers;

namespace VSI.EDGEAXConnector.WindowService
{

    public partial class EDGEAXConnectorWindowsService : ServiceBase
    {
        IScheduler _schedule;
        public const string EventLogName = "CommerceLink";
        public const string ServiceDisplayName = "CommerceLink Sync Service";
        private List<long> jobScheduleIds = new List<long>();
        private long DelayInSeconds = 0;

        public EDGEAXConnectorWindowsService()
        {
            //InitializeComponent();
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

        // For Manually Starting in Debug Mode
        public void Start()
        {
            OnStart(new List<string>().ToArray());
        }

        #region Start Stop Events

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
                CustomLogger.LogSyncTrace(0, 0, this.ServiceName, "Scheduler Started Successfully", $"");
                WriteEvent(this.ServiceName + " started scheduler", EventLogEntryType.Information);
                AddJobs();
                CustomLogger.LogSyncTrace(0, 0, this.ServiceName, "AddJobs Successfully", $"");

            }
            catch (Exception e)
            {
                WriteEvent(this.ServiceName + " failed to start with exception " + e.Message, EventLogEntryType.Error);
                CustomLogger.LogSyncTrace(0, 0, this.ServiceName, "failed to start with exception", $"");
                ExitCode = 1064;
                throw;
            }
        }
        protected override void OnStop()
        {
            try
            {
                StoreService.ResetJobsStatus(jobScheduleIds, 1);
                _schedule.Shutdown();
                WriteEvent(this.ServiceName + " stopped successfully", EventLogEntryType.Information);
                //  EmailSender.NotifyThroughEmail("Manually.", "No Exception", (int)EmailTemplateId.SimpleNotification);
                base.OnStop();
            }
            catch (Exception e)
            {
                WriteEvent(this.ServiceName + " failed to stop with exception " + e.Message, EventLogEntryType.Error);
                _schedule.Shutdown();
                base.OnStop();
                throw e;
            }

        }
        #endregion
        private void AddJobs()
        {
            //++ setting Up Stores 
            var storeKeys = ConfigurationManager.AppSettings["STORESKEY"].ToString();
            //++ Need to load if all stores if storeKeys are empty. 

            var ignoreStoreKeys = ConfigurationManager.AppSettings["IgnoreStoreKeys"].ToString();

            var stores = StoreService.GetStoreByKeys(storeKeys, ignoreStoreKeys);
            //Fetch Data Sync jobs only
            List<JobAndScheduleModel> winSyncjobs = StoreService.GetAllActiveJobsOfType(stores, false);
            jobScheduleIds = winSyncjobs.Select(x => x.JobSchduleId).ToList();
            var manager = new FactoryManager();
            //var container = manager.Configure();

            foreach (JobAndScheduleModel activeJob in winSyncjobs)
            {
                CustomLogger.LogSyncTrace(activeJob.storeId, activeJob.JobID, activeJob.JobName, "Adding Job - JobScheduleLog", activeJob.JobSchduleId.ToString());

                if (activeJob.JobName == "InventorySyncJob")
                {
                    var inventoryManager = new InventoryManager();
                    inventoryManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, inventoryManager);
                }
                else if (activeJob.JobName == "ProductSyncJob")
                {
                    var productManager = new ProductManager();
                    productManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, productManager);
                }
                else if (activeJob.JobName == "StoreSyncJob")
                {
                    var storeManager = new StoreManager();
                    storeManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, storeManager);
                }
                else if (activeJob.JobName == "PriceSyncJob")
                {
                    var priceManager = new PriceManager();
                    priceManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, priceManager);

                }
                else if (activeJob.JobName == "SyncCustomerJob")
                {
                    var customerManager = new CustomerManager();
                    customerManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, customerManager);
                }
                else if (activeJob.JobName == "SalesOrderSyncJob")
                {
                    var salesOrderManager = new SalesOrderManager();
                    salesOrderManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, salesOrderManager);
                }
                //else if (activeJob.JobName == "SyncCustomerInMagento")
                //{
                //    var man = new CustomerMagentoManager();
                //    AddSyncJob(activeJob, iStoreId, man);
                //}
                if (activeJob.JobName == "SyncDiscount")
                {
                    var discountManager = new DiscountManager();
                    discountManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, discountManager);
                }
                else if (activeJob.JobName == "DiscountWithAffiliationSync")
                {
                    var discountWithAffiliationManager = new DiscountWithAffiliationManager();
                    discountWithAffiliationManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, discountWithAffiliationManager);
                }
                else if (activeJob.JobName == "QuantityDiscountSync")
                {
                    var quantityDiscountManager = new QuantityDiscountManager();
                    quantityDiscountManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, quantityDiscountManager);
                }
                else if (activeJob.JobName == "QuantityDiscountWithAffiliationSync")
                {
                    var quantityDiscountWithAffiliationManager = new QuantityDiscountWithAffiliationManager();
                    quantityDiscountWithAffiliationManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, quantityDiscountWithAffiliationManager);
                }
                else if (activeJob.JobName == "SyncSalesOrderStatus")
                {
                    var salesOrderStatusManager = new SalesOrderStatusManager();
                    salesOrderStatusManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, salesOrderStatusManager);
                }
                //else if (activeJob.JobName == "SyncCustomerDeletedAddressesJob")
                //{
                //    var man = new CustomerDeletedAddressesManager();
                //    AddSyncJob(activeJob, iStoreId, man);
                //}
                else if (activeJob.JobName == "ChannelPublishingSync")
                {
                    var channelPublishingManager = new ChannelPublishingManager();
                    channelPublishingManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, channelPublishingManager);
                }
                else if (activeJob.JobName == "QuotationReasonGroupSync")
                {
                    var quotationReasonGroupManager = new QuotationReasonGroupManager();
                    quotationReasonGroupManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, quotationReasonGroupManager);
                }
                else if (activeJob.JobName == "ConfigurationSync")
                {
                    var channelConfigurationManager = new ChannelConfigurationManager();
                    channelConfigurationManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, channelConfigurationManager);
                }
                else if (activeJob.JobName == "CartSync")
                {
                    var cartManager = new CartManager();
                    cartManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, cartManager);
                }
                else if (activeJob.JobName == "DownloadThirdPartySalesOrderSync")
                {

                    var thirdPartyManager = new SalesOrderThirdPartyDownloadManager();
                    thirdPartyManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, thirdPartyManager);
                }
                else if (activeJob.JobName == "DataDeleteSync")
                {
                    var dataDeleteManager = new DataDeleteManager();
                    dataDeleteManager.SetStore(stores.FirstOrDefault(x => x.StoreId == activeJob.storeId));
                    AddSyncJob(activeJob, activeJob.storeId, dataDeleteManager);
                }
            }
        }

        #region Add Jobs Code
        private void AddSyncJob(JobAndScheduleModel job, int storeId, IJobManager jobManager)
        {
            JobDataMap map = new JobDataMap();
            map["manager"] = jobManager;

            IJobDetail jobSynch = JobBuilder.Create<DataSyncJob>()
                .WithIdentity(jobManager.GetIdentifier() + storeId, jobManager.GetGroup())
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
        public  void WriteEvent(String text, EventLogEntryType type)
        {
            if (CheckSourceExists(ServiceDisplayName))
            {
                EventLog.WriteEntry(ServiceDisplayName, text, type);
            }
        }
        private  bool CheckSourceExists(string source)
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

    }
}

