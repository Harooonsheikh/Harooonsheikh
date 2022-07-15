using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
    /// DiscountWithAffiliationManager class used to upload Discount CSV files to SFTP.
    /// </summary>
    public class DiscountWithAffiliationManager : IJobManager
    {
        public static readonly string IDENTIFIER = "UploadDiscountWithAffiliationSync";
        public static readonly string GROUP = "Synchronization";
        public StoreDto store = null;
        SFTPManager fTPManager = null;
        public FileHelper fileHelper = null;

        #region Data Members

        /// <summary>
        /// StartTime used for tracing time.
        /// </summary>
        DateTime StartTime = DateTime.MinValue;

        /// <summary>
        /// Used for SessionId.
        /// </summary>
        Guid sessionId = Guid.NewGuid();

        ConfigurationHelper configurationHelper;

        #endregion

        #region Public Methods

        public DiscountWithAffiliationManager()
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
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.UploadDiscountWithAffiliationSync);
            return job;
        }

        public bool Sync()
        {
            try
            {
                StartTime = DateTime.UtcNow;

                #region Upload Discount files to FTP

                string sourceDirUpload = this.configurationHelper.GetDirectory(
                    configurationHelper.GetSetting(DISCOUNTWITHAFFILIATION.Local_Output_Path));
                string localDirUpload = configurationHelper.GetSetting(DISCOUNTWITHAFFILIATION.Remote_Path);

                string[] Uploadfiles = Directory.GetFiles(sourceDirUpload);
                List<string> failedFiles = new List<string>();
                fTPManager.UploadFiles(Uploadfiles, localDirUpload, out failedFiles);

                foreach (var f in Uploadfiles)
                {
                    if (!failedFiles.Contains(f))
                    {
                        fileHelper.MoveFileToLocalFolder(f, "Processed", this.configurationHelper.GetDirectory(
                            configurationHelper.GetSetting(DISCOUNTWITHAFFILIATION.Local_Output_Path)));
                    }
                }

                #endregion

                // logger.Log(LogLevel.Info, traceInfo.ToString());

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncDiscountWithAffiliation upload Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.UploadDiscountWithAffiliationSync, this.store.StoreId);
        }
        public void InitializeParameter()
        {
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
            // this.emailSender = new EmailSender(store.StoreKey);
            fTPManager = new SFTPManager(store.StoreKey);
            fileHelper = new FileHelper(store.StoreKey);
        }

        #endregion

    }
}
