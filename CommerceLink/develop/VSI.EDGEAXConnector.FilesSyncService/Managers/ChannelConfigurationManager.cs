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
//using VSI.EDGEAXConnector.ECommerceDataModels;

namespace VSI.EDGEAXConnector.FilesSyncService
{

    /// <summary>
    /// ChannelConfigurationManager class used to upload configuration files to SFTP.
    /// </summary>
    public class ChannelConfigurationManager : IJobManager
    {

        public static readonly string IDENTIFIER = "UploadConfigurationSync";
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
        /// Get instance of Logger class.
        /// </summary>
        //Logger logger = LogManager.GetCurrentClassLogger();

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

        public ChannelConfigurationManager()
        {
            
        }

    
        /// <summary>
        /// Upload Configuration files from local directory path to FTP path.
        /// </summary>
        /// <returns></returns>
        public bool Sync()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                #region Upload Configuration files to FTP
                string sourceDirUpload = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(CHANNELCONFIGURATION.Local_Output_Path));
                string localDirUpload = configurationHelper.GetSetting(CHANNELCONFIGURATION.Remote_Path);
                //upload files
                string[] Uploadfiles = Directory.GetFiles(sourceDirUpload);
                List<string> failedFiles = new List<string>();
                fTPManager.UploadFiles(Uploadfiles, localDirUpload, out failedFiles);

                foreach (var f in Uploadfiles)
                {
                    if (!failedFiles.Contains(f))
                    {
                        fileHelper.MoveFileToLocalFolder(f, "Processed", this.configurationHelper.GetDirectory(
                            configurationHelper.GetSetting(CHANNELCONFIGURATION.Local_Output_Path)));
                    }
                }
                #endregion
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncConfiguration upload Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
        }

        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.UploadConfigurationSync);
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.UploadConfigurationSync, this.store.StoreId);
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

