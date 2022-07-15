using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data.ViewModels;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class LoggingDAL : BaseClass
    {
        public LoggingDAL(string storeKey) : base(storeKey)
        {

        }
        public LoggingDAL()
        {

        }
        public void LogRequest(RequestResponse requestResponse)
        {
            try
            {
                using (IntegrationDBEntities db = new IntegrationDBEntities())
                {
                    db.RequestResponse.Add(requestResponse);
                    db.SaveChanges();
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public void LogResponse(string id, bool? status, string response)
        {
            try
            {
                using (IntegrationDBEntities db = new IntegrationDBEntities())
                {
                    var requestResponseLog = db.RequestResponse.FirstOrDefault(log => log.RequestResponseId.ToString() == id);
                    if (requestResponseLog != null)
                    {
                        requestResponseLog.OutputPacket = response;
                        requestResponseLog.OutputSentAt = DateTime.Now;
                        var timespan = (requestResponseLog.OutputSentAt - requestResponseLog.CreatedOn);
                        requestResponseLog.TotalProcessingDuration = timespan.HasValue ? Convert.ToDecimal(timespan.Value.TotalMilliseconds) : 0;
                        requestResponseLog.IsSuccess = status;
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public List<LogMapper> GetAllLogMappers()
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                List<LogMapper> logMapperList = db.LogMapper.ToList();
                return logMapperList;
            }
        }

        public void LogRequestResponse(RequestResponse requestResponseLog)
        {
            try
            {
                CustomLogger.LogRequestResponse(requestResponseLog.MethodName, DataDirectionType.EcomRequestToCL, requestResponseLog.DataPacket,
                    requestResponseLog.CreatedOn,StoreId, requestResponseLog.CreatedBy, requestResponseLog.Description, requestResponseLog.EcomTransactionId, requestResponseLog.RequestInitiatedIP, requestResponseLog.OutputPacket,
                    (DateTime)requestResponseLog.OutputSentAt, requestResponseLog.IdentifierKey, requestResponseLog.IdentifierValue, requestResponseLog.IsSuccess == true ? 1 : 0, requestResponseLog.TotalProcessingDuration
                    );
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public RequestDetailsVM GetErrorDetails(string requestID)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var requestDetailsVm = new RequestDetailsVM()
                {
                    RequestID = requestID,
                    Log = db.Log.FirstOrDefault(a => a.IdentityId == requestID),
                    RequestResponseDetails = db.RequestResponse.FirstOrDefault(a=>a.Description == requestID)
                };
                return requestDetailsVm;
            }
        }
    }
}
