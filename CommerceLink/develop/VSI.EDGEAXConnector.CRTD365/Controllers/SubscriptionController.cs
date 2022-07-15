using System;
using System.Reflection;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.AosClasses;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class SubscriptionController : AosBaseController, ISubscriptionController
    {
        public SubscriptionController(string storeKey) : base(storeKey)
        {

        }
        public UserSessionInfo TestCall(string requestId)
        {
            this.RequestId = requestId;

            var userSessionResponse = Task.Run(async () =>
                await SendPostRequestAsync<UserSessionInfo>(AosMethod.AifUserSessionService_GetUserSessionInfo)).Result;

            return userSessionResponse;
        }
        public ErpUpdateSubscriptionContractResponse UpdateSubscriptionContract(ErpUpdateSubscriptionContract request, string requestId)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, CurrentStore, requestId, MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            ErpUpdateSubscriptionContractResponse response = new ErpUpdateSubscriptionContractResponse(false, "", "");

            this.RequestId = requestId;

            AosJsonServiceRequest<ErpUpdateSubscriptionContract> newRequest = new AosJsonServiceRequest<ErpUpdateSubscriptionContract>(request);

            response = Task.Run(async () => await SendPostRequestAsync<ErpUpdateSubscriptionContractResponse>(AosMethod.UpdateSubscriptionContract, newRequest)).Result;

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, CurrentStore, requestId, MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            return response;
        }
        public ProcessContractTerminateResponse ProcessContractTerminate(
            ErpProcessContractTerminateRequest processContractTerminateRequest, string requestId)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, CurrentStore, requestId, MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            try
            {
                this.RequestId = requestId;
                var  aosProcessContractTerminateReq = new AosJsonServiceRequest<ErpProcessContractTerminateRequest>(processContractTerminateRequest);

                var processContractTerminateResponse = Task.Run(async () => await
                    SendPostRequestAsync<ProcessContractTerminateResponse>
                    (AosMethod.ProcessContractTerminate,
                        aosProcessContractTerminateReq
                    )).Result;

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, CurrentStore, requestId, MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                return processContractTerminateResponse;
            }
            catch (Exception ex)
            {
                throw ex.InnerException ?? ex;
            }
        }

        public ProcessContractReactivateResponse ProcessContractReactivate(
            ErpProcessContractReactivateRequest processContractReactivateRequest, string requestId)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, CurrentStore, requestId, MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            try
            {
                this.RequestId = requestId;
                var aosProcessContractReactivateReq = new AosJsonServiceRequest<ErpProcessContractReactivateRequest>(processContractReactivateRequest);

                var processContractReactivateResponse = Task.Run(async () => await
                    SendPostRequestAsync<ProcessContractReactivateResponse>
                    (AosMethod.ProcessContractReactivate,
                        aosProcessContractReactivateReq
                    )).Result;

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, CurrentStore, requestId, MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

                return processContractReactivateResponse;
            }
            catch (Exception ex)
            {
                throw ex.InnerException ?? ex;
            }
        }
    }


}