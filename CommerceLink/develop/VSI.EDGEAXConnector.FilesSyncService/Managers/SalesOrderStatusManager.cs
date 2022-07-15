using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.FilesSyncService.Managers;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.SFTPlib;

namespace VSI.EDGEAXConnector.FilesSyncService
{
    /// <summary>
    /// SalesOrderStatusManager class used to upload SalesOrderStatus CSV files to SFTP.
    /// </summary>
    public class SalesOrderStatusManager : IJobManager
    {
        public static readonly string IDENTIFIER = "UploadSalesOrderStatusSync";
        public static readonly string GROUP = "Synchronization";
        public StoreDto store = null;
        public FileHelper fileHelper = null;
        SFTPManager fTPManager = null;
        #region Data Members

        /// <summary>
        /// StartTime used for tracing time.
        /// </summary>
        DateTime StartTime = DateTime.MinValue;

        /// <summary>
        /// Get instance of Logger class.
        /// </summary>
       // Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Used for SessionId.
        /// </summary>
        Guid sessionId = Guid.NewGuid();

        /// <summary>
        /// traceInfo used to append tracing.
        /// </summary>
        //StringBuilder traceInfo = new StringBuilder();

        ConfigurationHelper configurationHelper;

        #endregion

        #region Public Methods

        public SalesOrderStatusManager()
        {
           
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
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.UploadSalesOrderStatusSync);
            return job;
        }

        public bool Sync()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                // traceInfo.Append(string.Format("Session [{0}]: SalesOrderStatus() Started at [{1}]", sessionId, StartTime) + Environment.NewLine);

                #region Upload SalesOrderStatus files to FTP

                string sourceDirUpload = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(SALESORDER.Status_local_Path));

                string localDirUpload = configurationHelper.GetSetting(SALESORDER.Status_Remote_Path);

                // traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Uploaded Started at [{3}]", sessionId, sourceDirUpload, localDirUpload, DateTime.UtcNow));

                //upload files
            
                string[] Uploadfiles = Directory.GetFiles(sourceDirUpload);
                List<string> failedFiles = new List<string>();
                fTPManager.UploadFiles(Uploadfiles, localDirUpload, out failedFiles);

                foreach (var f in Uploadfiles)
                {
                    if (!failedFiles.Contains(f))
                    {
                        fileHelper.MoveFileToLocalFolder(f, "Processed", configurationHelper.GetSetting(SALESORDER.Status_local_Path));
                    }
                }

                #endregion

                // logger.Log(LogLevel.Info, traceInfo.ToString());

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncSalesOrderStatus upload Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
        }

        /// <summary>
        /// Upload Discount CSV files from local directory path to SFTP path.
        /// </summary>
        /// <returns></returns>
        public bool UploadSalesOrderStatusSync()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                // traceInfo.Append(string.Format("Session [{0}]: SalesOrderStatus() Started at [{1}]", sessionId, StartTime) + Environment.NewLine);

                #region Upload SalesOrderStatus files to FTP

                string sourceDirUpload = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(SALESORDER.Status_local_Path));

                string localDirUpload = configurationHelper.GetSetting(SALESORDER.Status_Remote_Path);

                // traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Uploaded Started at [{3}]", sessionId, sourceDirUpload, localDirUpload, DateTime.UtcNow));

                //upload files
                string[] Uploadfiles = Directory.GetFiles(sourceDirUpload);
                List<string> failedFiles = new List<string>();
                fTPManager.UploadFiles(Uploadfiles, localDirUpload, out failedFiles);

                foreach (var f in Uploadfiles)
                {
                    if (!failedFiles.Contains(f))
                    {
                        fileHelper.MoveFileToLocalFolder(f, "Processed", configurationHelper.GetSetting(SALESORDER.Status_local_Path));
                    }
                }

                #endregion

                // logger.Log(LogLevel.Info, traceInfo.ToString());

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncSalesOrderStatus upload Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
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

        public void SetStore(StoreDto store)
        {
            this.store = store;
        }

        public JobSchedule GetSchedule()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.UploadSalesOrderStatusSync, this.store.StoreId);
        }
        public void InitializeParameter()
        {
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
            fTPManager = new SFTPManager(store.StoreKey);
            fileHelper = new FileHelper(store.StoreKey);
        }
   
        #endregion

    }
}
