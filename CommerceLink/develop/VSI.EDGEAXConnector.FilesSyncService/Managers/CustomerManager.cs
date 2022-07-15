
using System;
using System.Collections.Generic;
using System.IO;
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
    /// CustomerManager class used to download Customer, Deleted Address CSV files from SFTP and upload
    /// Address and Customer CSV files to FTP.
    /// </summary>
    public class CustomerManager : IJobManager
    {
        public static readonly string IDENTIFIER = "DownloadCustomerSync";
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

        public CustomerManager()
        {
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
        }

        /// <summary>
        /// Download Customer CSV files from SFTP path to local directory path.
        /// </summary>
        /// <returns></returns>
        public bool DownloadCustomerSync()
        {

            StartTime = DateTime.UtcNow;
            //traceInfo.Append(string.Format("Session [{0}]: SyncCustomer() Started at [{1}]", sessionId, StartTime) + Environment.NewLine);
            #region Download Customer CSV Files from SFTP
            try
            {
                string sourceDir = configurationHelper.GetSetting(CUSTOMER.Remote_File_Path);
                string localDir = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(CUSTOMER.Local_Input_Path));

                // traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Download Started at [{3}]", sessionId, sourceDir, localDir, DateTime.UtcNow) + Environment.NewLine);

                //Download all files from SFTP customer files folder  to local folder
                fTPManager.DownloadDirectory(sourceDir, localDir);
                string[] files = Directory.GetFiles(localDir);
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncCustomer File Download Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
            #endregion
        }

        /// <summary>
        /// Upload Customer and Address CSV files from local directory path to FTP path.
        /// </summary>
        /// <returns></returns>
        public bool UploadUpdatedCustomersAndAddressesSync()
        {

            string sourceDirUpload = string.Empty;
            string localDirUpload = string.Empty;
            #region Upload updated Customer files to FTP
            try
            {
                sourceDirUpload = this.configurationHelper.GetDirectory(
                    configurationHelper.GetSetting(CUSTOMER.Local_Output_Path));
                localDirUpload = configurationHelper.GetSetting(CUSTOMER.Remote_Upload_SFTP_Path);
                // traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Uploaded Started at [{3}]", sessionId, sourceDirUpload, localDirUpload, DateTime.UtcNow));
                //upload files
                string[] Uploadfiles = Directory.GetFiles(sourceDirUpload);
                List<string> failedFiles = new List<string>();
                fTPManager.UploadFiles(Uploadfiles, localDirUpload, out failedFiles);
                foreach (var f in Uploadfiles)
                {
                    if (!failedFiles.Contains(f))
                    {
                        fileHelper.MoveFileToLocalFolder(f, "Uploaded", configurationHelper.GetSetting(CUSTOMER.Local_Output_Path));
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncCustomer File Upload Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }

            #endregion

            #region Upload updated addresses to FTP

            sourceDirUpload = string.Empty;
            localDirUpload = string.Empty;

            sourceDirUpload = this.configurationHelper.GetDirectory(
                configurationHelper.GetSetting(CUSTOMER.Address_Local_Output_Path));
            localDirUpload = configurationHelper.GetSetting(CUSTOMER.Address_Upload_SFTP_Path);


            //traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Uploaded Started at [{3}]", sessionId, sourceDirUpload, localDirUpload, DateTime.UtcNow));

            //upload files
            string[] Uploadfiles2 = Directory.GetFiles(sourceDirUpload);
            List<string> failedFiles2 = new List<string>();
            fTPManager.UploadFiles(Uploadfiles2, localDirUpload, out failedFiles2);

            foreach (var f in Uploadfiles2)
            {
                if (!failedFiles2.Contains(f))
                {
                    fileHelper.MoveFileToLocalFolder(f, "Uploaded", configurationHelper.GetSetting(CUSTOMER.Address_Local_Output_Path));	//CustomerAddressOutputPathLocal);
                }
            }

            //if (Uploadfiles2.Length <= 0)
            //{
            //    logger.Log(LogLevel.Info, string.Format("Session [{0}]: No files Uploaded [{1}] ", sessionId, DateTime.UtcNow));
            //}
            //else
            //{
            //    traceInfo.Append(string.Format("Session [{0}]: FTP Uploaded Completed at [{1}] -:- [{2}] files Uploaded", sessionId, DateTime.UtcNow, Uploadfiles2.Length) + Environment.NewLine);
            //}

            #endregion

           // logger.Log(LogLevel.Info, traceInfo.ToString());

            return true;
        }

        /// <summary>
        /// Download Deleted Address CSV files from SFTP path to local directory path.
        /// </summary>
        /// <returns></returns>
        public bool DownloadDeletedAddressesSync()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                // traceInfo.Append(string.Format("Session [{0}]: SyncDeletedAddresses() Started at [{1}]", sessionId, StartTime) + Environment.NewLine);

                #region Download Deleted Address CSV Files from SFTP

                string sourceDir = configurationHelper.GetSetting(ADDRESS.Remote_Path_Deleted);
                string localDir = this.configurationHelper.GetDirectory(
                    configurationHelper.GetSetting(ADDRESS.Local_Path_Deleted));

                // traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Download Started at [{3}]", sessionId, sourceDir, localDir, DateTime.UtcNow) + Environment.NewLine);
                //Download all files from SFTP customer files folder  to local folder
               fTPManager.DownloadDirectory(sourceDir, localDir);
                string[] files = Directory.GetFiles(localDir);

                //if (files.Length <= 0)
                //{
                //    logger.Log(LogLevel.Info, string.Format("Session [{0}]: No deleted address files downloaded [{1}] ", sessionId, DateTime.UtcNow));
                //}
                //else
                //{
                //    traceInfo.Append(string.Format("Session [{0}]: FTP Download Completed at [{1}] -:- [{2}] files downloaded", sessionId, DateTime.UtcNow, files.Length) + Environment.NewLine);
                //}

                #endregion

               // logger.Log(LogLevel.Info, traceInfo.ToString());

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncDeletedAddresses Download Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
        }

        public bool Sync()
        {

            string sourceDirUpload = string.Empty;
            string localDirUpload = string.Empty;

            #region Upload updated Customer files to FTP

            try
            {
                sourceDirUpload = this.configurationHelper.GetDirectory(
                    configurationHelper.GetSetting(CUSTOMER.Local_Output_Path));
                localDirUpload = configurationHelper.GetSetting(CUSTOMER.Remote_Upload_SFTP_Path);

                // traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Uploaded Started at [{3}]", sessionId, sourceDirUpload, localDirUpload, DateTime.UtcNow));

                //upload files
                string[] Uploadfiles = Directory.GetFiles(sourceDirUpload);
                List<string> failedFiles = new List<string>();
                fTPManager.UploadFiles(Uploadfiles, localDirUpload, out failedFiles);

                foreach (var f in Uploadfiles)
                {
                    if (!failedFiles.Contains(f))
                    {
                        fileHelper.MoveFileToLocalFolder(f, "Uploaded", configurationHelper.GetSetting(CUSTOMER.Local_Output_Path));
                    }
                }

                //if (Uploadfiles.Length <= 0)
                //{
                //    logger.Log(LogLevel.Info, string.Format("Session [{0}]: No files Uploaded [{1}] ", sessionId, DateTime.UtcNow));
                //}
                //else
                //{
                //    traceInfo.Append(string.Format("Session [{0}]: FTP Uploaded Completed at [{1}] -:- [{2}] files Uploaded", sessionId, DateTime.UtcNow, Uploadfiles.Length) + Environment.NewLine);
                //}
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncCustomer File Upload Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }

            #endregion

            #region Upload updated addresses to FTP

            sourceDirUpload = string.Empty;
            localDirUpload = string.Empty;

            sourceDirUpload = this.configurationHelper.GetDirectory(
                configurationHelper.GetSetting(CUSTOMER.Address_Local_Output_Path));
            localDirUpload = configurationHelper.GetSetting(CUSTOMER.Address_Upload_SFTP_Path);


            //traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Uploaded Started at [{3}]", sessionId, sourceDirUpload, localDirUpload, DateTime.UtcNow));

            //upload files
            string[] Uploadfiles2 = Directory.GetFiles(sourceDirUpload);
            List<string> failedFiles2 = new List<string>();
            fTPManager.UploadFiles(Uploadfiles2, localDirUpload, out failedFiles2);

            foreach (var f in Uploadfiles2)
            {
                if (!failedFiles2.Contains(f))
                {
                    fileHelper.MoveFileToLocalFolder(f, "Uploaded", configurationHelper.GetSetting(CUSTOMER.Address_Local_Output_Path));	//CustomerAddressOutputPathLocal);
                }
            }

            //if (Uploadfiles2.Length <= 0)
            //{
            //    logger.Log(LogLevel.Info, string.Format("Session [{0}]: No files Uploaded [{1}] ", sessionId, DateTime.UtcNow));
            //}
            //else
            //{
            //    traceInfo.Append(string.Format("Session [{0}]: FTP Uploaded Completed at [{1}] -:- [{2}] files Uploaded", sessionId, DateTime.UtcNow, Uploadfiles2.Length) + Environment.NewLine);
            //}

            #endregion

            // logger.Log(LogLevel.Info, traceInfo.ToString());

            return true;
        }

        public Job GetJob()
        {

            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.DownloadCustomerSync);
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.DownloadCustomerSync, this.store.StoreId);
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


