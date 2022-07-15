using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Emailing;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums.TMV;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.WindowService.Managers;

namespace VSI.EDGEAXConnector.WindowService
{
    public class SalesOrderStatusManager : IJobManager
    {
        public static readonly string IDENTIFIER = "SalesOrderStatusSync";
        public static readonly string GROUP = "Synchronization";
        private readonly IEComAdapterFactory _eComAdapterFactory;
        private readonly IErpAdapterFactory _erpAdapterFactory;
        private StoreDto _store;
        private ConfigurationHelper _configurationHelper;
        private EmailSender _emailSender;
        private FileHelper _fileHelper;

        private int salesOrderProcessingThreadCount = 0;

        public SalesOrderStatusManager()
        {
            this._erpAdapterFactory = new ErpAdapterFactory();
            this._eComAdapterFactory = new EComAdapterFactory();
        }

        private bool SyncOrderStatus()
        {
            try
            {
                CustomLogger.LogDebugInfo("Syn Order Status Started", _store.StoreId, _store.CreatedBy);

                var erpSalesOrderController = _erpAdapterFactory.CreateSalesOrderController(_store.StoreKey);

                CustomLogger.LogDebugInfo("erpSalesOrderController Instance Created", _store.StoreId, _store.CreatedBy);

                bool useCrt = !(System.Configuration.ConfigurationManager.AppSettings["IsLoadSalesOrderStatusUsingCRT"] != null && System.Configuration.ConfigurationManager.AppSettings["IsLoadSalesOrderStatusUsingCRT"] == "0");

                CustomLogger.LogDebugInfo(string.Format("Value of useCRT is {0}", useCrt.ToString()), _store.StoreId, _store.CreatedBy);

                // Get orders to Sync with Ingram after processed in D365.
                List<ErpSalesOrderStatus> orderUpdates = erpSalesOrderController.GetSalesOrderStatusUpdate(useCrt);

                // Get orders to Sync with Ingram without processing in D365.
                orderUpdates.AddRange(GetOrderToSynchWithThirdParty());

                CustomLogger.LogDebugInfo("Successfully return form function erpSalesOrderController.GetSalesOrderStatusUpdate(useCRT).", _store.StoreId, _store.CreatedBy);

                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                        DataDirectionType.CLRequestToThirdParty, // Data direction
                        "", // Data packed
                        DateTime.UtcNow, // Created on
                        _store.StoreId,
                        "System", // Created by
                        "List of orders that will be synced with Ingram", // Description
                        "", // eCom Transaction Id
                        "", // Request Initialed IP
                        Newtonsoft.Json.JsonConvert.SerializeObject(orderUpdates), // Output packed
                        DateTime.UtcNow, // Output sent at
                        "", // Identifier Key
                        "", // Identifier Value
                        1, // Success
                        0 // TotalProcessingDuration
                        );

                if (orderUpdates.Count > 0)
                {
                    var ecomUpdates = (orderUpdates.Where(o => o.Notify).ToList());

                    CustomLogger.LogDebugInfo(ecomUpdates, _store.StoreId, _store.CreatedBy, "ecomUpdates to Json");

                    using (var eComSalesOrderController = _eComAdapterFactory.CreateSalesOrderStatusController(_store.StoreKey))
                    {
                        CustomLogger.LogDebugInfo("eComSalesOrderController Instance Created", _store.StoreId, _store.CreatedBy);

                        eComSalesOrderController.UpdateOrderStatus(ecomUpdates);
                    }
                }
                else
                {
                    CustomLogger.LogDebugInfo("There are no sales orders to be updated", _store.StoreId, _store.CreatedBy);
                    CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                        DataDirectionType.CLRequestToThirdParty, // Data direction
                        "", // Data packed
                        DateTime.UtcNow, // Created on
                        _store.StoreId,
                        "System", // Created by
                        "There are no sales orders to be updated", // Description
                        "", // eCom Transaction Id
                        "", // Request Initialed IP
                        "", // Output packed
                        DateTime.UtcNow, // Output sent at
                        "", // Identifier Key
                        "", // Identifier Value
                        1, // Success
                        0 // TotalProcessingDuration
                        );
                }
                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, _store.StoreId, _store.CreatedBy);
                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                    DataDirectionType.CLRequestToThirdParty, // Data direction
                    "", // Data packed
                    DateTime.UtcNow, // Created on
                    _store.StoreId,
                    "System", // Created by
                    "exception when syncing back ingram order statuses", // Description
                    "", // eCom Transaction Id
                    "", // Request Initialed IP
                    Newtonsoft.Json.JsonConvert.SerializeObject(ex), // Output packed
                    DateTime.UtcNow, // Output sent at
                    "", // Identifier Key
                    "", // Identifier Value
                    0, // Success
                    0 // TotalProcessingDuration
                    );
                throw;
            }
        }

        #region Test Code

        public bool Sync()
        {
            try
            {
                return SyncOrderStatus();
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, _store.StoreId, _store.CreatedBy);
                _emailSender.NotifyThroughEmail("", ex.ToString(), "", (int)Common.Enums.EmailTemplateId.SalesOrderStatus);
                CustomLogger.LogRequestResponse(MethodBase.GetCurrentMethod().Name,
                    DataDirectionType.ThirdPartyResponseToCL, // Data direction
                    "", // Data packed
                    DateTime.UtcNow, // Created on
                    _store.StoreId,
                    "System", // Created by
                    "Sales Order Status Job Failed", // Description
                    "", // eCom Transaction Id
                    "", // Request Initialed IP
                    Newtonsoft.Json.JsonConvert.SerializeObject(ex), // Output packed
                    DateTime.UtcNow, // Output sent at
                    "", // Identifier Key
                    "", // Identifier Value
                    0, // Success
                    0 // TotalProcessingDuration
                    );
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
            return jobRepo.GetJobSchedule((int)Common.Enums.SyncJobs.SalesOrderStatusSync, this._store.StoreId);
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

        private List<ErpSalesOrderStatus> GetOrderToSynchWithThirdParty()
        {
            var messageDal = new ThirdPartyMessageDAL(_store.StoreKey);

            var orders = new List<ThirdPartyMessage>();
            orders.AddRange(messageDal.GetSalesOrdersList(TransactionStatus.TransferIngramRequest_ValidationFailed, salesOrderProcessingThreadCount));
            orders.AddRange(messageDal.GetSalesOrdersList(TransactionStatus.MissingParameter_EndCustomerAdminEmail, salesOrderProcessingThreadCount));

            return orders.Select(o => new ErpSalesOrderStatus
            {
                ChannelRefId = o.ThirdPartyId,
                Notify = true,
                Status = o.TransactionStatus == (int)TransactionStatus.MissingParameter_EndCustomerAdminEmail ? ApplicationConstant.IngramMissingParameterStatus : ""
            }).ToList();
        }


        #endregion
    }

}
