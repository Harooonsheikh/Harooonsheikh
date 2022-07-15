using EdgeAXCommerceLink.RetailProxy.Extensions;
using Microsoft.Dynamics.Commerce.RetailProxy;
using NewRelic.Api.Agent;
using System.Reflection;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class InAppPurchaseController : BaseController, IInAppPurchaseController
    {

        #region "Public"

        public InAppPurchaseController(string storeKey) : base(storeKey)
        {

        }
        public ERPAppAutoRenewContractResponse AutoRenewContract(AppAutoRenewContractRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var erpResponse = new ERPAppAutoRenewContractResponse(false, "");

            try
            {
                var rsResponse = ECL_TV_AppleAutoRenewContract(request);
                erpResponse = new ERPAppAutoRenewContractResponse((bool)rsResponse.Status, rsResponse.Message.ToString());
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        public ERPAppCancelContractResponse CancelContract(AppCancelContractRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var erpResponse = new ERPAppCancelContractResponse(false, "");

            try
            {
                var rsResponse = ECL_TV_AppleCancelContract(request);
                erpResponse = new ERPAppCancelContractResponse((bool)rsResponse.Status, rsResponse.Message.ToString());
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        public ERPAppReactivateContractResponse ReactivateContract(AppReactivateContractRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var erpResponse = new ERPAppReactivateContractResponse(false, "");

            try
            {
                var rsResponse = ECL_TV_AppleReactivateContract(request);
                erpResponse = new ERPAppReactivateContractResponse((bool)rsResponse.Status, rsResponse.Message);
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        public ERPAppTransferContractResponse TransferContract(AppTransferContractRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var erpResponse = new ERPAppTransferContractResponse(false, "");

            try
            {
                var rsResponse = ECL_TV_AppleTransferContract(request);
                erpResponse = new ERPAppTransferContractResponse((bool)rsResponse.Status, rsResponse.Message);
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }

        public ERPAppRebuyContractResponse RebuyContract(AppRebuyContractRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var erpResponse = new ERPAppRebuyContractResponse(false, "","");

            try
            {
                var rsResponse = ECL_TV_AppleRebuyContract(request);
                erpResponse = new ERPAppRebuyContractResponse((bool)rsResponse.Status, rsResponse.Message,rsResponse.Result);
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        #endregion

        #region RetailServer API
        [Trace]
        private AppleAutoRenewContractResponse ECL_TV_AppleAutoRenewContract(AppAutoRenewContractRequest request)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_AppleAutoRenewContract(request.ChannelReferenceId, (bool)request.AutoRenew,
                                                request.SalesOrderId, baseCompany, request.PONumber, request.IsUpdateRenewalPrice)).Result;
        }

        [Trace]
        private AppleCancelContractResponse ECL_TV_AppleCancelContract(AppCancelContractRequest request)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_AppleCancelContract(request.ChannelReferenceId, baseCompany)).Result;
        }

        [Trace]
        private AppleReactivateContractResponse ECL_TV_AppleReactivateContract(AppReactivateContractRequest request)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_AppleReactivateContract(request.ChannelReferenceId, request.SubscriptionExpiryDate, baseCompany)).Result;
        }

        [Trace]
        private AppleTransferContractResponse ECL_TV_AppleTransferContract(AppTransferContractRequest request)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_AppleTransferContract(request.ChannelReferenceId, request.CustomerEmail, baseCompany)).Result;
        }

        [Trace]
        private AppleRebuyContractResponse ECL_TV_AppleRebuyContract(AppRebuyContractRequest request)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_AppleRebuyContract(request.CustomerRef, baseCompany)).Result;
        }
        #endregion

    }
}
