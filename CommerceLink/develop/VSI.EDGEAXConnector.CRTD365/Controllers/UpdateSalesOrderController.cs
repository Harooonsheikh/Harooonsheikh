using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using NewRelic.Api.Agent;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class UpdateSalesOrderController : BaseController, IUpdateSalesOrderController
    {
        public UpdateSalesOrderController(string storeKey) : base(storeKey)
        {

        }
        #region "Public"

        public ErpCancelContractResponse CancelContract(string salesOrderId, string salesLineRecId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpSalesOrder> salesOrders = new List<ErpSalesOrder>();
            ErpCancelContractResponse erpResponse = new ErpCancelContractResponse(false, "", "");
            try
            {
                var rsResponse = ECL_TV_CancelContract(salesOrderId, salesLineRecId);

                if ((bool)rsResponse.Status)
                {
                    erpResponse = new ErpCancelContractResponse(true, rsResponse.Message, rsResponse.Result);
                }
                else
                {
                    erpResponse = new ErpCancelContractResponse(false, rsResponse.Message, rsResponse.Result);
                }
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        public ErpTerminateContractResponse TerminateContract(string salesOrderId, string channelReferenceId, string salesLineRecId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpSalesOrder> salesOrders = new List<ErpSalesOrder>();
            ErpTerminateContractResponse erpResponse = new ErpTerminateContractResponse(false, "", "");
            try
            {
                var rsResponse = ECL_TV_TerminateContract(salesOrderId, channelReferenceId, salesLineRecId);

                if ((bool)rsResponse.Status)
                {
                    erpResponse = new ErpTerminateContractResponse(true, rsResponse.Message, rsResponse.Result);
                }
                else
                {
                    erpResponse = new ErpTerminateContractResponse(false, rsResponse.Message, rsResponse.Result);
                }
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        public ErpUpdateContractResponse UpdateContract(ErpTMVCrosssellType action, string salesOrderId, string salesLineRecId, ErpAdditionalSalesLine newSalesLine, ErpCustomerOrderInfo customerOrderInfo)
        {
            throw new NotImplementedException();
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            //ErpUpdateContractResponse erpResponse = new ErpUpdateContractResponse(false, "", "");
            //try
            //{
            //    customerOrderInfo.ChannelRecordId = baseChannelId.ToString();

            //    CustomerOrderInfo customerOrderUpdatedInfo = new CustomerOrderInfo();
            //    customerOrderUpdatedInfo = _mapper.Map<ErpCustomerOrderInfo, CustomerOrderInfo>(customerOrderInfo);
            //    string customerOrderInfoJsonString = JsonConvert.SerializeObject(customerOrderUpdatedInfo);



            //    //Ecommerce will handle Time Qty implementation
            //    //Caculating line amount with respect to client specific custom dll
            //    //SalesOrderController salesOrderController = new SalesOrderController();
            //    //double timeQuantity = salesOrderController.TMV_CalculateTimeQty();
            //    //salesLine.RelativeAmount = (decimal)timeQuantity * salesLine.NetAmount;                
            //    var rsResponse = ECL_TV_UpdateContract(action, salesOrderId, salesLineRecId, newSalesLine, customerOrderInfoJsonString);

            //    if (rsResponse.Success)
            //    {
            //        erpResponse = new ErpUpdateContractResponse(true, rsResponse.Message, rsResponse.Result);
            //    }
            //    else
            //    {
            //        erpResponse = new ErpUpdateContractResponse(false, rsResponse.Message, rsResponse.Result);
            //    }

            //}
            //catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            //{
            //    string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
            //    AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
            //    throw exp;
            //}
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            //return erpResponse;
        }
        public ErpAddContractRelationResponse AddContractRelation(ErpTMVContractRelationType action, string orgSalesOrderId, string orgSalesLineRecIds, string newSalesOrderId, string newSalesLineRecIds)
        {
            throw new NotImplementedException();
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            //ErpAddContractRelationResponse erpResponse = new ErpAddContractRelationResponse(false, "", "");
            //try
            //{
            //    var rsResponse = ECL_TV_AddContractRelation(action, orgSalesOrderId, orgSalesLineRecIds, newSalesOrderId, newSalesLineRecIds);

            //    if (rsResponse.Success)
            //    {
            //        erpResponse = new ErpAddContractRelationResponse(true, rsResponse.Message, rsResponse.Result);
            //    }
            //    else
            //    {
            //        erpResponse = new ErpAddContractRelationResponse(false, rsResponse.Message, rsResponse.Result);
            //    }
            //}
            //catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            //{
            //    string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
            //    AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
            //    throw exp;
            //}
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            //return erpResponse;
        }

        #endregion

        #region "Private"

        #endregion

        #region RetailServer API
        [Trace]
        private TerminateContractResponse ECL_TV_TerminateContract(string salesOrderId, string channelReferenceId, string salesLineRecId)
        {
            var salesOrderManager = RPFactory.GetManager<ISalesOrderManager>();
            var rsResponse = Task.Run(async () => await salesOrderManager.ECL_TV_TerminateContract(salesOrderId, channelReferenceId, salesLineRecId, baseCompany)).Result;
            return rsResponse;
        }
        [Trace]
        private CancelContractResponse ECL_TV_CancelContract(string salesOrderId, string salesLineRecId)
        {
            var salesOrderManager = RPFactory.GetManager<ISalesOrderManager>();
            var rsResponse = Task.Run(async () => await salesOrderManager.ECL_TV_CancelContract(salesOrderId, salesLineRecId, baseCompany)).Result;
            return rsResponse;
        }
        //[Trace]
        //private UpdateContractResponse ECL_TV_UpdateContract(ErpTMVCrosssellType action, string salesOrderId,
        //    string salesLineRecId, ErpAdditionalSalesLine newSalesLine, string customerOrderInfoJsonString)
        //{
        //    throw new NotImplementedException();
        //    //var salesOrderManager = RPFactory.GetManager<ISalesOrderManager>();
        //    //var rsResponse = Task.Run(async () => await salesOrderManager.ECL_TV_UpdateContract(salesOrderId, salesLineRecId,
        //    //    newSalesLine.ItemId, newSalesLine.InventDimensionId, newSalesLine.Quantity, action.ToString(),
        //    //    customerOrderInfoJsonString, baseCompany)).Result;
        //    //return rsResponse;
        //}
        //[Trace]
        //private AddContractRelationResponse ECL_TV_AddContractRelation(ErpTMVContractRelationType action,
        //    string orgSalesOrderId, string orgSalesLineRecIds, string newSalesOrderId, string newSalesLineRecIds)
        //{
        //    throw new NotImplementedException();
        //    //var salesOrderManager = RPFactory.GetManager<ISalesOrderManager>();

        //    //var rsResponse = Task.Run(async () => await salesOrderManager.ECL_TV_AddContractRelation(orgSalesOrderId,
        //    //    orgSalesLineRecIds, newSalesOrderId, newSalesLineRecIds, action.ToString(), baseCompany)).Result;
        //    //return rsResponse;
        //}
        #endregion
    }
}