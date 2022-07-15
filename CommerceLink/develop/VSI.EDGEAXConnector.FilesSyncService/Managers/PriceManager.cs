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
    /// PriceManager class upload Price CSV files to FTP.
    /// </summary>
    public class PriceManager : IJobManager
    {
        public static readonly string IDENTIFIER = "UploadPriceSync";
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
        // StringBuilder traceInfo = new StringBuilder();
         ConfigurationHelper configurationHelper;
        #endregion

        #region Public Methods

        public PriceManager()
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
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.UploadPriceSync);
            return job;
        }

    


        public bool Sync()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                // traceInfo.Append(string.Format("Session [{0}]: SyncPrice() Started at [{1}]", sessionId, StartTime) + Environment.NewLine);

                #region Upload Price files to FTP

                string sourceDirUpload = this.configurationHelper.GetDirectory(
                    configurationHelper.GetSetting(PRICE.local_Output_Path));//ConfigurationHelper.ProductPriceOutputPath;
                string localDirUpload = configurationHelper.GetSetting(PRICE.Remote_Path);       //ConfigurationHelper.ProductPriceFTPPath;


                // traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Uploaded Started at [{3}]", sessionId, sourceDirUpload, localDirUpload, DateTime.UtcNow));

                //upload files
                string[] Uploadfiles = Directory.GetFiles(sourceDirUpload);
                List<string> failedFiles = new List<string>();
                fTPManager.UploadFiles(Uploadfiles, localDirUpload, out failedFiles);

                foreach (var f in Uploadfiles)
                {
                    if (!failedFiles.Contains(f))
                    {
                        fileHelper.MoveFileToLocalFolder(f, "Processed", configurationHelper.GetSetting(PRICE.local_Output_Path));   // ProductPriceOutputPath);
                    }
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncPrice Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
        }

      


        /// <summary>
        /// Upload Price CSV files from local directory path to FTP path.
        /// </summary>
        /// <returns></returns>
        public bool UploadPricesSync()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                // traceInfo.Append(string.Format("Session [{0}]: SyncPrice() Started at [{1}]", sessionId, StartTime) + Environment.NewLine);

                #region Upload Price files to FTP

                string sourceDirUpload = this.configurationHelper.GetDirectory(
                    configurationHelper.GetSetting(PRICE.local_Output_Path));//ConfigurationHelper.ProductPriceOutputPath;
                string localDirUpload = configurationHelper.GetSetting(PRICE.Remote_Path);       //ConfigurationHelper.ProductPriceFTPPath;


                // traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Uploaded Started at [{3}]", sessionId, sourceDirUpload, localDirUpload, DateTime.UtcNow));

                //upload files
                string[] Uploadfiles = Directory.GetFiles(sourceDirUpload);
                List<string> failedFiles = new List<string>();
                fTPManager.UploadFiles(Uploadfiles, localDirUpload, out failedFiles);

                foreach (var f in Uploadfiles)
                {
                    if (!failedFiles.Contains(f))
                    {
                        fileHelper.MoveFileToLocalFolder(f, "Processed", configurationHelper.GetSetting(PRICE.local_Output_Path));   // ProductPriceOutputPath);
                    }
                }
              
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncPrice Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.UploadPriceSync, this.store.StoreId);
        }
   
        public void InitializeParameter()
        {
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
            // this.emailSender = new EmailSender(store.StoreKey);

            fileHelper = new FileHelper(store.StoreKey);
            fTPManager = new SFTPManager(store.StoreKey);
        }
        #endregion

    }
}
