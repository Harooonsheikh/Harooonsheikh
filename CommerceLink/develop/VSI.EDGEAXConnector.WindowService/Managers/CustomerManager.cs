using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper.TransformationAdapter;
using VSI.EDGEAXConnector.SFTPlib;
using VSI.EDGEAXConnector.WindowService.Managers;

namespace VSI.EDGEAXConnector.WindowService
{
    public class CustomerManager : IJobManager
    {
        public static readonly string IDENTIFIER = "CustomerSync";
        public static readonly string GROUP = "Synchronization";
        private readonly IErpAdapterFactory _erpAdapterFactory;
        private readonly IEComAdapterFactory _eComAdapterFactory;
        DateTime StartTime = DateTime.MinValue;
        DateTime EndTime = DateTime.MinValue;
        ConfigurationHelper configurationHelper;
        public EmailSender emailSender = null;
        public StoreDto store = null;
        public FileHelper fileHelper = null;
        SFTPManager fTPManager = null;
        public CustomerManager()
        {
            _erpAdapterFactory = new ErpAdapterFactory();
            _eComAdapterFactory = new EComAdapterFactory();

        }
        public bool SyncCustomer()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                Guid sessionId = Guid.NewGuid();
                StringBuilder traceInfo = new StringBuilder();
                traceInfo.Append(string.Format("Session [{0}]: SyncCustomer() Started at [{1}]", sessionId, StartTime) + Environment.NewLine);
                #region New Code SFTP
                string sourceDir = configurationHelper.GetSetting(CUSTOMER.Remote_File_Path);
                string localDir = configurationHelper.GetSetting(CUSTOMER.Local_Input_Path);
                traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Download Started at [{3}]", sessionId, sourceDir, localDir, DateTime.UtcNow) + Environment.NewLine);
                //Download all files from SFTP customer files folder  to local folder
                //SFTPlib.SFTPManager.DownloadDirectory(sourceDir, localDir);
                string[] files = Directory.GetFiles(localDir);
                traceInfo.Append(string.Format("Session [{0}]: FTP Download Completed at [{1}] -:- [{2}] files downloaded", sessionId, DateTime.UtcNow, files.Length) + Environment.NewLine);

                if (files.Length > 0)
                {
                    // Moved controllers here to improve performance
                    var customerTranformer = new TransformationAdapter<List<EcomcustomerCustomerEntity>, List<ErpCustomer>>();
                    var erpCtroller = _erpAdapterFactory.CreateCustomerController(store.StoreKey);
                    traceInfo.Append(string.Format("Session [{0}]: ERP Controller Initialized at [{1}]", sessionId, DateTime.UtcNow) + Environment.NewLine);
                    using (var ecomCtrlor = _eComAdapterFactory.CreateCustomerController(store.StoreKey))
                    {
                        traceInfo.Append(string.Format("Session [{0}]: Ecommerce Controller Initialized at [{1}]", sessionId, DateTime.UtcNow) + Environment.NewLine);

                        CustomLogger.LogTraceInfo(traceInfo.ToString(), store.StoreId, store.CreatedBy);
                        bool exception = false;
                        foreach (var file in files)
                        {
                            if (File.Exists(file) && fileHelper.CheckFileAvailability(file))
                            {
                                StringBuilder fileTraceInfo = new StringBuilder();
                                try
                                {
                                    fileTraceInfo.Clear();
                                    fileTraceInfo.Append(string.Format("Session [{0}]: Processing file [{1}] at [{2}]", sessionId, file, DateTime.UtcNow, files.Length) + Environment.NewLine);
                                    List<EcomcustomerCustomerEntity> ecomCusts = ecomCtrlor.GetCustomer(file);
                                    fileTraceInfo.Append(string.Format("Session [{0}]: [{1}] customers read from file", sessionId, ecomCusts.Count) + Environment.NewLine);
                                    var erpCustomers = customerTranformer.Transform(ecomCusts);
                                    fileTraceInfo.Append(string.Format("Session [{0}]: [{1}] EcomcustomerCustomerEntity transformed to ErpCustomer", sessionId, erpCustomers.Count) + Environment.NewLine);
                                    erpCtroller.CreateCustomer(erpCustomers);
                                    fileTraceInfo.Append(string.Format("Session [{0}]: Processing file [{1}] Completed at [{2}]", sessionId, file, DateTime.UtcNow, files.Length) + Environment.NewLine);
                                    fileHelper.MoveFileToLocalFolder(file, "Processed");
                                    fileTraceInfo.Append(string.Format("Sesion [{0}]: File [{1}] moved to Processed folder at [{2}]", sessionId, file, DateTime.UtcNow, files.Length) + Environment.NewLine);
                                    CustomLogger.LogDebugInfo(fileTraceInfo.ToString(), store.StoreId, store.CreatedBy);
                                }
                                catch (Exception exx) // Need to improve the exception handling here
                                {
                                    string msg = string.Empty;
                                    // exception = true;
                                    if (exx.Message.Contains("IntegrationManager.GetComKey"))
                                    {
                                        msg = string.Format("Session [{0}]: EntityCommandExecutionException in Customer Manager.cs [File:{1}]. File not moved to Failed folder", sessionId, file) + Environment.NewLine;
                                        CustomLogger.LogWarn(msg + Common.CommonUtility.GetExceptionInfo(exx), store.StoreId, store.CreatedBy);
                                        // We will not move file to "FAILED" folder as this exception is due to SQL server not responding at this time. We will give this file an other try.
                                    }
                                    else if (exx.Message.Contains("Microsoft.Dynamics.Commerce.Runtime.CommunicationException"))
                                    {
                                        msg = string.Format("Session [{0}]: CommunicationException in Customer Manager.cs [File:{1}]. File not moved to Failed folder", sessionId, file) + Environment.NewLine;
                                        //exception = true;
                                        CustomLogger.LogWarn(msg + Common.CommonUtility.GetExceptionInfo(exx), store.StoreId, store.CreatedBy);
                                        // We will not move file to "FAILED" folder as this exception is due to SQL server not responding at this time. We will give this file an other try.
                                    }
                                    else if (exx.Message.Contains("Microsoft.Dynamics.Commerce.Runtime.HeadquarterTransactionServiceException"))
                                    {
                                        msg = string.Format("Session [{0}]: HeadquarterTransactionServiceException in Customer Manager.cs [File:{1}]. File not moved to Failed folder", sessionId, file) + Environment.NewLine;
                                        //exception = true;
                                        CustomLogger.LogWarn(msg + Common.CommonUtility.GetExceptionInfo(exx), store.StoreId, store.CreatedBy);
                                        // We will not move file to "FAILED" folder as this exception is due to SQL server not responding at this time. We will give this file an other try.
                                    }
                                    else
                                    {
                                        msg = string.Format("Session [{0}]: Exception in Customer Manager.cs [File:{1}]", sessionId, file) + Environment.NewLine;
                                        CustomLogger.LogException(msg + Common.CommonUtility.GetExceptionInfo(exx), store.StoreId, store.CreatedBy);
                                        string failedFile = fileHelper.MoveFileToLocalFolder(file, "Failed");
                                        if (File.Exists(failedFile) && fileHelper.CheckFileAvailability(failedFile))
                                        {
                                            emailSender.NotifyThroughEmail(Path.GetFileNameWithoutExtension(failedFile), exx.ToString(), failedFile, (int)Common.Enums.EmailTemplateId.Customer);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                CustomLogger.LogTraceInfo("Customer file has already been processed : " + file, store.StoreId, store.CreatedBy);

                            }
                        }

                        if (exception)
                        {
                            throw new Exception(string.Format("Session [{0}]: Exception ocurred in customer files processing. Please reveiw previous errors in Log for more information", sessionId));
                        }
                        else
                        {
                            CustomLogger.LogDebugInfo(string.Format("Session [{0}]: All files processing DONE at [{1}] ", sessionId, DateTime.UtcNow), store.StoreId, store.CreatedBy);
                        }
                    }
                }
                else
                {
                    CustomLogger.LogDebugInfo(string.Format("Session [{0}]: No files downloaded [{1}] ", sessionId, DateTime.UtcNow), store.StoreId, store.CreatedBy);
                }

                #endregion
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncCustomer Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
        }
        public bool SynchUpdatedCustomersAndAddresses()
        {
            DateTime StartDateTime;
            Guid sessionId = Guid.NewGuid();
            StringBuilder traceInfo = new StringBuilder();
            traceInfo.Append(string.Format("Sesion [{0}]: for SynchUpdatedCustomersAndAddresses() Started at [{1}]", sessionId, DateTime.UtcNow) + Environment.NewLine);
            int CustomerFunctionId = int.Parse(configurationHelper.GetSetting(CUSTOMER.Job_id));//"40";
            JobRepository objJobLog = new JobRepository(store.StoreKey);
            try
            {
                var tranformer = new TransformationAdapter<List<ErpCustomer>, List<EcomcustomerCustomerEntity>>();
                var tranformer1 = new TransformationAdapter<List<ErpAddress>, List<EcomcustomerAddressEntityItem>>();
                var erpCustomerCtroller = _erpAdapterFactory.CreateCustomerController(store.StoreKey);


                StartDateTime = objJobLog.GetLastJobCallTimeStamp(CustomerFunctionId); //Convert.ToDateTime("7/05/2015"); 
                StartDateTime = StartDateTime.AddHours(double.Parse(configurationHelper.GetSetting(APPLICATION.TimeStamp_Difference))); //TODO - Either remove this or make this value 0 in Config
                traceInfo.Append(string.Format("Session [{0}]: Searching customers updated after [{1}] ", sessionId, StartDateTime) + Environment.NewLine);
                List<ErpCustomer> erpCustomer = erpCustomerCtroller.GetUpdatedCustomersAndAddresses(StartDateTime, configurationHelper.GetSetting(APPLICATION.ERP_Default_Customer_Group));
                traceInfo.Append(string.Format("Session [{0}]: Searching addresses updated after [{1}] ", sessionId, StartDateTime) + Environment.NewLine);
                List<ErpAddress> erpAddress = erpCustomerCtroller.GetUpdatedAddresses(StartDateTime, configurationHelper.GetSetting(APPLICATION.ERP_Default_Customer_Group));
                if ((erpCustomer != null && erpCustomer.Count > 0) || (erpAddress != null && erpAddress.Count > 0))
                {
                    if (erpCustomer != null && erpCustomer.Count > 0)
                    {
                        //This part of code just to test xml template generation
                        //erpCustomer.ForEach(c => {
                        //    c.Addresses = erpAddress.FindAll(a => a.EcomCustomerId == c.EcomCustomerId);
                        //});
                        StringBuilder clog = new StringBuilder();
                        clog.Append(string.Format("Session [{0}]: Total [{1}] customer found", sessionId, erpCustomer.Count) + Environment.NewLine);
                        var ecomCustomer = tranformer.Transform(erpCustomer);
                        using (var ecomCustomerController = _eComAdapterFactory.CreateCustomerController(store.StoreKey))
                        {
                            ecomCustomerController.UpdateCustomer(ecomCustomer);
                            //ecomCustomerController.UpdateCustomerViaXML(ecomCustomer);
                        }
                        clog.Append(string.Format("Session [{0}]: Total [{1}] customer's CSV file generated.", sessionId, erpCustomer.Count) + Environment.NewLine);
                        CustomLogger.LogDebugInfo(clog.ToString(), store.StoreId, store.CreatedBy);
                    }
                    if (erpAddress != null && erpAddress.Count > 0)
                    {
                        StringBuilder sLog = new StringBuilder();
                        #region CSV Generation
                        sLog.Append(string.Format("Session [{0}]: Total [{1}] updated addresses found", sessionId, erpAddress.Count) + Environment.NewLine);
                        var ecomAddress = tranformer1.Transform(erpAddress);
                        using (var ecomCustomerController = _eComAdapterFactory.CreateCustomerController(store.StoreKey))
                        {
                            ecomCustomerController.UpdateAddress(ecomAddress);
                        }
                        sLog.Append(string.Format("Session [{0}]: Total [{1}] Address's CSV file generated", sessionId, erpCustomer.Count) + Environment.NewLine);
                        #endregion

                        #region Live Call for Address Update

                        //using (var ecomAddressController = _eComAdapterFactory.CreateAddressController())
                        //{
                        //    var addresses = ConvertToEcomAddresses(erpAddress);
                        //    ecomAddressController.SyncAddress(addresses);
                        //}
                        //sLog.Append(string.Format("Session [{0}]: Total [{1}] Address's updated on Ecommerce store", sessionId, erpCustomer.Count) + Environment.NewLine);
                        #endregion
                        CustomLogger.LogDebugInfo(sLog.ToString(), store.StoreId, store.CreatedBy);

                    }
                }
                else
                {
                    traceInfo.Append(string.Format("Session [{0}]: No updated Customers OR Addresses found", sessionId) + Environment.NewLine);
                }
                traceInfo.Append(string.Format("Sesion [{0}]: for SynchUpdatedCustomersAndAddresses() Completed at [{1}]", sessionId, DateTime.UtcNow) + Environment.NewLine);
                CustomLogger.LogDebugInfo(traceInfo.ToString(), store.StoreId, store.CreatedBy);
                return true;
            }
            catch (Exception ex)
            {
                // CustomLogger.LogDebugInfo(traceInfo.ToString());
                CustomLogger.LogException("SynchUpdatedCustomersAndAddresses Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                emailSender.NotifyThroughEmail("", ex.ToString(), "", (int)Common.Enums.EmailTemplateId.Customer);
                return false;
            }
        }

        #region Test Code
        public void GetCustomer(string accountNo)
        {
            try
            {
                var erpCtroller = _erpAdapterFactory.CreateCustomerController(store.StoreKey);
                erpCtroller.GetCustomer(accountNo);

            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, store.StoreId, store.CreatedBy);
                Trace.WriteLine(ex.Message);
            }
        }

        private List<EcomcustomerAddressEntityItem> ConvertToEcomAddresses(List<ErpAddress> Addresses)
        {
            List<EcomcustomerAddressEntityItem> list = new List<EcomcustomerAddressEntityItem>();
            foreach (var src in Addresses)
            {
                EcomcustomerAddressEntityItem dest = new EcomcustomerAddressEntityItem();

                dest.customer_id = Convert.ToInt32(string.IsNullOrEmpty(src.EcomCustomerId) ? "0" : src.EcomCustomerId);
                dest.customer_address_id = Convert.ToInt32(string.IsNullOrEmpty(src.EcomAddressId) ? "0" : src.EcomAddressId);
                string[] names = src.Name.Split(' ');
                dest.firstname = names[0];
                dest.lastname = "--";
                if (names.Length > 2)
                {
                    dest.middlename = names[1];
                    dest.lastname = names[2];
                }
                else if (names.Length > 1)
                    dest.lastname = names[1];
                else
                    dest.lastname = "-"; // TO avoid the address creation failure in Magento
                if (src.ThreeLetterISORegionName.ToLower() == "usa")
                    dest.country_id = "US";
                else
                    dest.country_id = src.ThreeLetterISORegionName;
                dest.postcode = src.ZipCode;
                dest.city = src.City;
                dest.region = src.State;
                dest.street = src.Street;
                dest.telephone = string.IsNullOrEmpty(src.Phone) ? "-" : src.Phone;// TO avoid the address creation failure in Magento
                dest.fax = src.Fax;
                //dest.is_default_billing = src.IsPrimary;
                dest.is_default_shipping = src.IsPrimary;
                dest.is_default_shippingSpecified = src.IsPrimary;
                //dest.is_default_shipping = src.AddressType == ErpAddressType.Delivery ? true : false;
                dest.edgeaxintegrationkey = src.RecordId;
                list.Add(dest);
            }

            return list;
        }
        #endregion
  

        public bool Sync()
        {
            try
            {
                StartTime = DateTime.UtcNow;
                Guid sessionId = Guid.NewGuid();
                StringBuilder traceInfo = new StringBuilder();
                traceInfo.Append(string.Format("Session [{0}]: SyncCustomer() Started at [{1}]", sessionId, StartTime) + Environment.NewLine);
                #region New Code SFTP
                string sourceDir = configurationHelper.GetSetting(CUSTOMER.Remote_File_Path);
                string localDir = configurationHelper.GetSetting(CUSTOMER.Local_Input_Path);
                traceInfo.Append(string.Format("Session [{0}]: Source Dir [{1}], Local Dir [{2}] - FTP Download Started at [{3}]", sessionId, sourceDir, localDir, DateTime.UtcNow) + Environment.NewLine);
                //Download all files from SFTP customer files folder  to local folder
                //SFTPlib.SFTPManager.DownloadDirectory(sourceDir, localDir);
                string[] files = Directory.GetFiles(localDir);
                traceInfo.Append(string.Format("Session [{0}]: FTP Download Completed at [{1}] -:- [{2}] files downloaded", sessionId, DateTime.UtcNow, files.Length) + Environment.NewLine);

                if (files.Length > 0)
                {
                    // Moved controllers here to improve performance
                    var customerTranformer = new TransformationAdapter<List<EcomcustomerCustomerEntity>, List<ErpCustomer>>();
                    var erpCtroller = _erpAdapterFactory.CreateCustomerController(store.StoreKey);
                    traceInfo.Append(string.Format("Session [{0}]: ERP Controller Initialized at [{1}]", sessionId, DateTime.UtcNow) + Environment.NewLine);
                    using (var ecomCtrlor = _eComAdapterFactory.CreateCustomerController(store.StoreKey))
                    {
                        traceInfo.Append(string.Format("Session [{0}]: Ecommerce Controller Initialized at [{1}]", sessionId, DateTime.UtcNow) + Environment.NewLine);

                        CustomLogger.LogTraceInfo(traceInfo.ToString(), store.StoreId, store.CreatedBy);
                        bool exception = false;
                        foreach (var file in files)
                        {
                            if (File.Exists(file) && fileHelper.CheckFileAvailability(file))
                            {
                                StringBuilder fileTraceInfo = new StringBuilder();
                                try
                                {
                                    fileTraceInfo.Clear();
                                    fileTraceInfo.Append(string.Format("Session [{0}]: Processing file [{1}] at [{2}]", sessionId, file, DateTime.UtcNow, files.Length) + Environment.NewLine);
                                    List<EcomcustomerCustomerEntity> ecomCusts = ecomCtrlor.GetCustomer(file);
                                    fileTraceInfo.Append(string.Format("Session [{0}]: [{1}] customers read from file", sessionId, ecomCusts.Count) + Environment.NewLine);
                                    var erpCustomers = customerTranformer.Transform(ecomCusts);
                                    fileTraceInfo.Append(string.Format("Session [{0}]: [{1}] EcomcustomerCustomerEntity transformed to ErpCustomer", sessionId, erpCustomers.Count) + Environment.NewLine);
                                    erpCtroller.CreateCustomer(erpCustomers);
                                    fileTraceInfo.Append(string.Format("Session [{0}]: Processing file [{1}] Completed at [{2}]", sessionId, file, DateTime.UtcNow, files.Length) + Environment.NewLine);
                                    fileHelper.MoveFileToLocalFolder(file, "Processed");
                                    fileTraceInfo.Append(string.Format("Sesion [{0}]: File [{1}] moved to Processed folder at [{2}]", sessionId, file, DateTime.UtcNow, files.Length) + Environment.NewLine);
                                    CustomLogger.LogDebugInfo(fileTraceInfo.ToString(), store.StoreId, store.CreatedBy);
                                }
                                catch (Exception exx) // Need to improve the exception handling here
                                {
                                    string msg = string.Empty;
                                    // exception = true;
                                    if (exx.Message.Contains("IntegrationManager.GetComKey"))
                                    {
                                        msg = string.Format("Session [{0}]: EntityCommandExecutionException in Customer Manager.cs [File:{1}]. File not moved to Failed folder", sessionId, file) + Environment.NewLine;
                                        CustomLogger.LogWarn(msg + Common.CommonUtility.GetExceptionInfo(exx), store.StoreId, store.CreatedBy);
                                        // We will not move file to "FAILED" folder as this exception is due to SQL server not responding at this time. We will give this file an other try.
                                    }
                                    else if (exx.Message.Contains("Microsoft.Dynamics.Commerce.Runtime.CommunicationException"))
                                    {
                                        msg = string.Format("Session [{0}]: CommunicationException in Customer Manager.cs [File:{1}]. File not moved to Failed folder", sessionId, file) + Environment.NewLine;
                                        //exception = true;
                                        CustomLogger.LogWarn(msg + Common.CommonUtility.GetExceptionInfo(exx), store.StoreId, store.CreatedBy);
                                        // We will not move file to "FAILED" folder as this exception is due to SQL server not responding at this time. We will give this file an other try.
                                    }
                                    else if (exx.Message.Contains("Microsoft.Dynamics.Commerce.Runtime.HeadquarterTransactionServiceException"))
                                    {
                                        msg = string.Format("Session [{0}]: HeadquarterTransactionServiceException in Customer Manager.cs [File:{1}]. File not moved to Failed folder", sessionId, file) + Environment.NewLine;
                                        //exception = true;
                                        CustomLogger.LogWarn(msg + Common.CommonUtility.GetExceptionInfo(exx), store.StoreId, store.CreatedBy);
                                        // We will not move file to "FAILED" folder as this exception is due to SQL server not responding at this time. We will give this file an other try.
                                    }
                                    else
                                    {
                                        msg = string.Format("Session [{0}]: Exception in Customer Manager.cs [File:{1}]", sessionId, file) + Environment.NewLine;
                                        CustomLogger.LogException(msg + Common.CommonUtility.GetExceptionInfo(exx), store.StoreId, store.CreatedBy);
                                        string failedFile = fileHelper.MoveFileToLocalFolder(file, "Failed");
                                        if (File.Exists(failedFile) && fileHelper.CheckFileAvailability(failedFile))
                                        {
                                            emailSender.NotifyThroughEmail(Path.GetFileNameWithoutExtension(failedFile), exx.ToString(), failedFile, (int)Common.Enums.EmailTemplateId.Customer);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                CustomLogger.LogTraceInfo("Customer file has already been processed : " + file, store.StoreId, store.CreatedBy);

                            }
                        }

                        if (exception)
                        {
                            throw new Exception(string.Format("Session [{0}]: Exception ocurred in customer files processing. Please reveiw previous errors in Log for more information", sessionId));
                        }
                        else
                        {
                            CustomLogger.LogDebugInfo(string.Format("Session [{0}]: All files processing DONE at [{1}] ", sessionId, DateTime.UtcNow), store.StoreId, store.CreatedBy);
                        }
                    }
                }
                else
                {
                    CustomLogger.LogDebugInfo(string.Format("Session [{0}]: No files downloaded [{1}] ", sessionId, DateTime.UtcNow), store.StoreId, store.CreatedBy);
                }

                #endregion
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("SyncCustomer Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex), store.StoreId, store.CreatedBy);
                throw;
            }
        }

        public Job GetJob()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            Job job = jobRepo.GetJob((int)Common.Enums.SyncJobs.CustomerSync);
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

        public void SetStore(StoreDto store)
        {
            this.store = store;
        }

        public void UpdateJobStatus(JobSchedule jobSchedule, Common.Enums.SynchJobStatus status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.UpdateJobStatus(jobSchedule.JobId, (int)status, this.store.StoreId);
        }

        public void JobLog(JobSchedule jobLog, int status)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            jobRepo.JobLog(jobLog.JobId, status);
        }
        public bool IsJobCompletedTodayInJobLog(JobSchedule jobSchedule, int jobStatus)
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.IsJobCompletedTodayInJobLog(jobSchedule.JobId, jobStatus);
        }

        public JobSchedule GetSchedule()
        {
            JobRepository jobRepo = new JobRepository(this.store.StoreKey);
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.CustomerSync, this.store.StoreId);
        }

        public void InitializeParameter()
        {
            this.configurationHelper = new ConfigurationHelper(store.StoreKey);
            this.emailSender = new EmailSender(store.StoreKey);
            fileHelper = new FileHelper(store.StoreKey);
            fTPManager = new SFTPManager(store.StoreKey);
        }
    }
}


