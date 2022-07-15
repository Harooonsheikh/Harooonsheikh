using System;
using System.Reflection;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.WindowService.Managers;

namespace VSI.EDGEAXConnector.WindowService
{
    //As per our current Architecture Only Scync method is executed of Manager. 
    //when Architecture is change to support multiple method of Manager to execute, than Sync Method code will be shift to "SalesOrderManager".
    public class SalesOrderThirdPartyDownloadManager : IJobManager
    {
        public static readonly string IDENTIFIER = "SalesOrderThirdPartyDownload";
        public static readonly string GROUP = "Synchronization";
        private readonly IEComAdapterFactory _eComAdapterFactory;
        StoreDto _store;
        ConfigurationHelper _configurationHelper;
        EmailSender _emailSender;
        FileHelper _fileHelper;

        public SalesOrderThirdPartyDownloadManager(IEComAdapterFactory eComAdapterFactory)
        {
            _eComAdapterFactory = eComAdapterFactory;
        }

        public SalesOrderThirdPartyDownloadManager()
        {
            this._eComAdapterFactory = new EComAdapterFactory();
        }

        public bool Sync()
        {
            try
            {
                CustomLogger.LogDebugInfo("erpSalesOrderController Instance Created", _store.StoreId, _store.CreatedBy);
                var eComSalesOrderController = _eComAdapterFactory.CreateSalesOrderController(_store.StoreKey);
                eComSalesOrderController.SaveThirdPartySalesOrder();

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, _store.StoreId, _store.CreatedBy);
                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                    DataDirectionType.ThirdPartyResponseToCL, // Data direction
                    "", // Data packed
                    DateTime.UtcNow, // Created on
                    _store.StoreId,
                    "System", // Created by
                    "Sales Order Third Party Download Failed", // Description
                    "", // eCom Transaction Id
                    "", // Request Initialed IP
                    Newtonsoft.Json.JsonConvert.SerializeObject(ex), // Output packed
                    DateTime.UtcNow, // Output sent at
                    "", // Identifier Key
                    "", // Identifier Value
                    0, // Success
                    0 // TotalProcessingDuration
                    );
                _emailSender.NotifyThroughEmail("", ex.ToString(), "", (int)Common.Enums.EmailTemplateId.ThirdPartySalesOrder);
                throw;
            }
        }

        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this._store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.SalesOrderSync);
            return job;
        }

        public void JobLog(Job jb, int status)
        {
            JobRepository jobRepo = new JobRepository(this._store.StoreKey);
            jobRepo.JobLog(jb.JobID, status);
        }

        public string GetIdentifier()
        {
            return IDENTIFIER;
        }

        public string GetGroup()
        {
            return GROUP;
        }

        public void SetStore(StoreDto store)
        {
            this._store = store;

        }

        public void UpdateJobStatus(JobSchedule jobSchedule, Common.Enums.SynchJobStatus status)
        {
            JobRepository jobRepo = new JobRepository(this._store.StoreKey);
            jobRepo.UpdateJobStatus(jobSchedule.JobId, (int)status, this._store.StoreId);
        }

        public void JobLog(JobSchedule jobSchedule, int status)
        {
            JobRepository jobRepo = new JobRepository(this._store.StoreKey);
            jobRepo.JobLog(jobSchedule.JobId, status);
        }

        public JobSchedule GetSchedule()
        {
            JobRepository jobRepo = new JobRepository(this._store.StoreKey);
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.DownloadThirdPartySalesOrderSync, this._store.StoreId);
        }

        public void InitializeParameter()
        {
            this._configurationHelper = new ConfigurationHelper(_store.StoreKey);
            this._emailSender = new EmailSender(_store.StoreKey);
            _fileHelper = new FileHelper(_store.StoreKey);
        }

        public bool IsJobCompletedTodayInJobLog(JobSchedule jobSchedule, int jobStatus)
        {
            JobRepository jobRepo = new JobRepository(this._store.StoreKey);
            return jobRepo.IsJobCompletedTodayInJobLog(jobSchedule.JobId, jobStatus);
        }
    }

}
