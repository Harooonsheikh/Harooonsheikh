using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.FilesSyncService.Managers;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.SFTPlib;

namespace VSI.EDGEAXConnector.FilesSyncService
{

    /// <summary>
    /// SalesOrderManager class used to download Sales Order CSV files from SFTP.
    /// </summary>
    public class SalesOrderManager : IJobManager
    {
        public static readonly string IDENTIFIER = "DownloadSalesOrderSync";
        public static readonly string GROUP = "Synchronization";
        #region Data Members

        public StoreDto store = null;
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
        // StringBuilder traceInfo = new StringBuilder();
        ConfigurationHelper configurationHelper;

        FileHelper fileHelper = null;

        SFTPManager fTPManager = null;
        #endregion

        #region Public Methods

        public SalesOrderManager()
        {

        }

        /// <summary>
        /// Download Sales Order CSV files from SFTP path to local directory path.
        /// </summary>
        /// <returns></returns>
        public bool DownloadSalesOrderSync()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                // traceInfo.Append(string.Format("Session [{0}]: SyncSalesOrder() Started at [{1}]", sessionId, StartTime) + Environment.NewLine);

                #region Download Sales Order CSV Files from SFTP

                string sourceDir = configurationHelper.GetSetting(SALESORDER.Remote_Input_Path);
                string localDir = Convert.ToBoolean(configurationHelper.GetSetting(SALESORDER.Multiple_To_Single_File))
                    ? this.configurationHelper.GetDirectory(configurationHelper.GetSetting(SALESORDER.Multiplefile_Input_Path))
                    : this.configurationHelper.GetDirectory(configurationHelper.GetSetting(SALESORDER.Singlefile_Input_Path));

                CustomLogger.LogDebugInfo(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Download Started at [{3}]", sessionId, sourceDir, localDir, DateTime.UtcNow) + Environment.NewLine, store.StoreId, store.CreatedBy);


                if (Debugger.IsAttached)
                {
                    var Files = System.IO.Directory.GetFiles(sourceDir);

                    foreach (string fr in Files)
                    {
                        if (System.IO.File.Exists(localDir + @"\" + System.IO.Path.GetFileName(fr)))
                            System.IO.File.Delete(localDir + @"\" + System.IO.Path.GetFileName(fr));

                        System.IO.File.Move(fr, localDir + @"\" + System.IO.Path.GetFileName(fr));
                    }
                }
                else
                {
                    //Download all files from SFTP customer files folder  to local folder
                    fTPManager.DownloadDirectory(sourceDir, localDir);
                }

                if (Convert.ToBoolean(configurationHelper.GetSetting(SALESORDER.Multiple_To_Single_File)))
                {
                    ProcessMultiSalesOrderFileToSingleFile(localDir);
                }

                string[] files = Directory.GetFiles(localDir);

                if (files.Length <= 0)
                {
                    CustomLogger.LogDebugInfo(string.Format("Session [{0}]: No SalesOrder files downloaded [{1}] ", sessionId, DateTime.UtcNow), store.StoreId, store.CreatedBy);
                }
                else
                {
                    CustomLogger.LogDebugInfo(string.Format("Session [{0}]: FTP Download Completed at [{1}] -:- [{2}] files downloaded", sessionId, DateTime.UtcNow, files.Length) + Environment.NewLine, store.StoreId, store.CreatedBy);
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncSalesOrder download Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
        }

        private void ProcessMultiSalesOrderFileToSingleFile(string localDir)
        {
            try
            {
                //Getting path where to place new generated files
                string localSalesOrderDir = this.configurationHelper.GetDirectory(
                    configurationHelper.GetSetting(SALESORDER.Singlefile_Input_Path));

                string[] files = Directory.GetFiles(localDir);

                foreach (var file in files)
                {
                    if (File.Exists(file) && fileHelper.CheckFileAvailability(file))
                    {
                        try
                        {
                            XmlDocument salesOrdersDoc = new XmlDocument();
                            salesOrdersDoc.Load(file);
                            salesOrdersDoc.InnerXml = RemoveAllXmlNamespace(salesOrdersDoc.InnerXml);

                            XmlNodeList ordersList = salesOrdersDoc.SelectNodes(@"//orders/order");

                            foreach (XmlNode order in ordersList)
                            {
                                XmlDocument orderDoc = new XmlDocument();
                                XmlNode docNode = orderDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                                orderDoc.AppendChild(docNode);
                                //Creating new element
                                XmlElement ordersElement = orderDoc.CreateElement("orders");
                                ordersElement.InnerXml = order.OuterXml;

                                //Adding file name as reference
                                string[] fileName = file.Split('\\');
                                ordersElement.SetAttribute("fileDetails", fileName[fileName.Length - 1]);

                                orderDoc.AppendChild(ordersElement);

                                XmlAttribute orderNo = order.Attributes["order-no"];
                                string strFileName = "tempOrder-" + orderNo.Value;
                                //Save file with temp name which is ignorable by SyncSalesOrder job 

                                orderDoc.Save(localSalesOrderDir + '\\' + strFileName);
                                //After save successfully renaming file to actual name
                                File.Move(localSalesOrderDir + '\\' + strFileName, localSalesOrderDir + '\\' + strFileName.Replace("tempOrder-", ""));
                            }

                        }
                        catch (Exception ex)
                        {
                            CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                        }
                    }
                    else
                    {
                        CustomLogger.LogTraceInfo("DW Sales orders file has already been broken into single file  : " + file, store.StoreId, store.CreatedBy);
                    }
                    //Moving DW actual file into processed folder
                    fileHelper.MoveFileToLocalFolder(file, "Processed", localDir);
                }

            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
            }
        }

        public static string RemoveAllXmlNamespace(string xmlData)
        {
            string xmlnsPattern = "\\s+xmlns\\s*(:\\w)?\\s*=\\s*\\\"(?<url>[^\\\"]*)\\\"";
            MatchCollection matchCol = Regex.Matches(xmlData, xmlnsPattern);

            foreach (Match m in matchCol)
            {
                xmlData = xmlData.Replace(m.ToString(), "");
            }
            return xmlData;
        }

        public bool Sync()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                // traceInfo.Append(string.Format("Session [{0}]: SyncSalesOrder() Started at [{1}]", sessionId, StartTime) + Environment.NewLine);

                #region Download Sales Order CSV Files from SFTP

                string sourceDir = configurationHelper.GetSetting(SALESORDER.Remote_Input_Path);
                string localDir = Convert.ToBoolean(configurationHelper.GetSetting(SALESORDER.Multiple_To_Single_File))
                    ? this.configurationHelper.GetDirectory(configurationHelper.GetSetting(SALESORDER.Multiplefile_Input_Path))
                    : this.configurationHelper.GetDirectory(configurationHelper.GetSetting(SALESORDER.Singlefile_Input_Path));

                CustomLogger.LogDebugInfo(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Download Started at [{3}]", sessionId, sourceDir, localDir, DateTime.UtcNow) + Environment.NewLine, store.StoreId, store.CreatedBy);


                if (Debugger.IsAttached)
                {
                    var Files = System.IO.Directory.GetFiles(sourceDir);

                    foreach (string fr in Files)
                    {
                        if (System.IO.File.Exists(localDir + @"\" + System.IO.Path.GetFileName(fr)))
                            System.IO.File.Delete(localDir + @"\" + System.IO.Path.GetFileName(fr));

                        System.IO.File.Move(fr, localDir + @"\" + System.IO.Path.GetFileName(fr));
                    }
                }
                else
                {
                    //Download all files from SFTP customer files folder  to local folder
                    fTPManager.DownloadDirectory(sourceDir, localDir);
                }

                if (Convert.ToBoolean(configurationHelper.GetSetting(SALESORDER.Multiple_To_Single_File)))
                {
                    ProcessMultiSalesOrderFileToSingleFile(localDir);
                }

                string[] files = Directory.GetFiles(localDir);

                if (files.Length <= 0)
                {
                    CustomLogger.LogDebugInfo(string.Format("Session [{0}]: No SalesOrder files downloaded [{1}] ", sessionId, DateTime.UtcNow), store.StoreId, store.CreatedBy);
                }
                else
                {
                    CustomLogger.LogDebugInfo(string.Format("Session [{0}]: FTP Download Completed at [{1}] -:- [{2}] files downloaded", sessionId, DateTime.UtcNow, files.Length) + Environment.NewLine, store.StoreId, store.CreatedBy);
                }

                #endregion

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncSalesOrder download Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
        }

        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.DownloadSalesOrderSync);
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.DownloadSalesOrderSync, this.store.StoreId);
        }

        public void InitializeParameter()
        {
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
            fileHelper = new FileHelper(store.StoreKey);
            fTPManager = new SFTPManager(store.StoreKey);
        }
        #endregion

    }
}