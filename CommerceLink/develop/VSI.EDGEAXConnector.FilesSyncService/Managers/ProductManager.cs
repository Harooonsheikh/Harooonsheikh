using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.FilesSyncService.Managers;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.SFTPlib;
//using VSI.EDGEAXConnector.ECommerceDataModels;

namespace VSI.EDGEAXConnector.FilesSyncService
{

    /// <summary>
    /// ProductManager class used to upload Product and Image CSV files to FTP.
    /// </summary>
    public class ProductManager : IJobManager
    {
        public static readonly string IDENTIFIER = "UploadProductSync";
        public static readonly string GROUP = "Synchronization";
        public StoreDto store = null;
        public EmailSender emailSender = null;
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

        public ProductManager()
        {
          
        }

        /// <summary>
        /// Upload Product CSV files from local directory path to FTP path.
        /// </summary>
        /// <returns></returns>
        public bool UploadProductsSync()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                // traceInfo.Append(string.Format("Session [{0}]: SyncProducts() Started at [{1}]", sessionId, StartTime) + Environment.NewLine);

                #region Upload Product files to FTP

                string sourceDirUpload = this.configurationHelper.GetDirectory(
                    configurationHelper.GetSetting(PRODUCT.Local_Output_Path));
                string localDirUpload = configurationHelper.GetSetting(PRODUCT.Remote_Path);

                // traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Uploaded Started at [{3}]", sessionId, sourceDirUpload, localDirUpload, DateTime.UtcNow));

                //upload files
                string[] Uploadfiles = Directory.GetFiles(sourceDirUpload);
                List<string> failedFiles = new List<string>();
                fTPManager.UploadFiles(Uploadfiles, localDirUpload, out failedFiles, true);

                foreach (var f in Uploadfiles)
                {
                    if (!failedFiles.Contains(f))
                    {
                        fileHelper.MoveFileToLocalFolder(f, "Processed", configurationHelper.GetSetting(PRODUCT.Local_Output_Path));
                    }
                }

                #endregion

                //logger.Log(LogLevel.Info, traceInfo.ToString());

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncProducts upload Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
        }

        /// <summary>
        /// Upload Image CSV files from local directory path to FTP path.
        /// </summary>
        /// <returns></returns>
        public bool UploadImagesSync()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                CustomLogger.LogTraceInfo(string.Format("Session [{0}]: SyncImages() Started at [{1}]", sessionId, StartTime) + Environment.NewLine, store.StoreId, store.CreatedBy);

                #region Upload Image files to FTP

                string sourceDirUpload = this.configurationHelper.GetDirectory(
                    configurationHelper.GetSetting(PRODUCT.Image_Local_Output_Path));
                string localDirUpload = configurationHelper.GetSetting(PRODUCT.Image_Remote_Path);

                CustomLogger.LogTraceInfo(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Uploaded Started at [{3}]", sessionId, sourceDirUpload, localDirUpload, DateTime.UtcNow), store.StoreId, store.CreatedBy);

                //upload files
                string[] Uploadfiles = Directory.GetFiles(sourceDirUpload);
                List<string> failedFiles = new List<string>();
                fTPManager.UploadFiles(Uploadfiles, localDirUpload, out failedFiles);

                foreach (var f in Uploadfiles)
                {
                    if (!failedFiles.Contains(f))
                    {
                        fileHelper.MoveFileToLocalFolder(f, "Processed", configurationHelper.GetSetting(PRODUCT.Image_Local_Output_Path));
                    }
                }

                #endregion

                //logger.Log(LogLevel.Info, traceInfo.ToString());

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncImages upload Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
        }

        public bool Sync()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                // traceInfo.Append(string.Format("Session [{0}]: SyncProducts() Started at [{1}]", sessionId, StartTime) + Environment.NewLine);

                #region Upload Product files to FTP

                string sourceDirUpload = this.configurationHelper.GetDirectory(
                    configurationHelper.GetSetting(PRODUCT.Local_Output_Path));
                string localDirUpload = configurationHelper.GetSetting(PRODUCT.Remote_Path);

                // traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Uploaded Started at [{3}]", sessionId, sourceDirUpload, localDirUpload, DateTime.UtcNow));

                //upload files
                string[] Uploadfiles = Directory.GetFiles(sourceDirUpload);
                List<string> failedFiles = new List<string>();
                fTPManager.UploadFiles(Uploadfiles, localDirUpload, out failedFiles, true);

                foreach (var f in Uploadfiles)
                {
                    if (!failedFiles.Contains(f))
                    {
                        fileHelper.MoveFileToLocalFolder(f, "Processed", configurationHelper.GetSetting(PRODUCT.Local_Output_Path));
                    }
                }

                #endregion

                //logger.Log(LogLevel.Info, traceInfo.ToString());

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncProducts upload Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
        }

        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.UploadProductSync);
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.UploadProductSync, this.store.StoreId);
        }
        public void InitializeParameter()
        {
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
            this.emailSender = new EmailSender(store.StoreKey);
            fileHelper = new FileHelper(store.StoreKey);
            fTPManager = new SFTPManager(store.StoreKey);
        }
        #endregion

    }
}
