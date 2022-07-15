//using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class UpdateSalesOrderController : BaseController, IUpdateSalesOrderController
    {
        #region Public Methods

        public UpdateSalesOrderController(string storeKey) : base(storeKey)
        {
        }

        public ErpAddContractRelationResponse AddContractRelation(ErpTMVContractRelationType action, string orgSalesOrderId, string orgSalesLineRecIds, string newSalesOrderId, string newSalesLineRecIds)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var updateSalesOrderManager = new UpdateSalesOrderCRTManager();
            return updateSalesOrderManager.AddContractRelation(action, orgSalesOrderId, orgSalesLineRecIds, newSalesOrderId, newSalesLineRecIds, currentStore.StoreKey);
        }

        public ErpCancelContractResponse CancelContract(string salesOrderId, string salesLineRecId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var updateSalesOrderManager = new UpdateSalesOrderCRTManager();
            return updateSalesOrderManager.CancelContract(salesOrderId, salesLineRecId, currentStore.StoreKey);
        }

        public ErpTerminateContractResponse TerminateContract(string salesOrderId, string ChannelReferenceId, string salesLineRecId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var updateSalesOrderManager = new UpdateSalesOrderCRTManager();
            return updateSalesOrderManager.TerminateContract(salesOrderId, ChannelReferenceId, salesLineRecId, currentStore.StoreKey);
        }

        public ErpUpdateContractResponse UpdateContract(ErpTMVCrosssellType action, string salesOrderId, string salesLineRecId, ErpAdditionalSalesLine newSalesLine, ErpTenderLine payment, List<ErpAdditionalSalesLine> addOns, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCustomerOrderInfo customerOrderInfo = new ErpCustomerOrderInfo();

            if (payment != null)
            {
                //Generating Blob for new payment
                if (!string.IsNullOrEmpty(payment.MaskedCardNumber))
                {
                    ErpSalesOrder salesOrder = new ErpSalesOrder();

                    salesOrder.CurrencyCode = configurationHelper.GetSetting(APPLICATION.Default_Currency_Code);
                    salesOrder.BillingAddress = payment.BillingAddress;
                    salesOrder.CustomerId = payment.CustomerId;

                    SalesOrderHelper soHelper = new SalesOrderHelper(currentStore.StoreKey);
                    soHelper.SetupPaymentMethod(payment, salesOrder, requestId);
                }
                else
                {
                    payment.IsDeposit = false;
                }                
                
                customerOrderInfo.Payments = new System.Collections.ObjectModel.ObservableCollection<ErpPaymentInfo>();

                ErpPaymentInfo paymentInfo = new ErpPaymentInfo();

                paymentInfo.Amount = (decimal)payment.Amount;
                paymentInfo.Currency = payment.Currency;
                paymentInfo.PaymentType = payment.TenderTypeId;
                paymentInfo.Prepayment = (bool)payment.IsDeposit;
                paymentInfo.CardType = payment.CardTypeId;
                paymentInfo.PaymentCaptured = false;
                paymentInfo.CreditCardAuthorization = payment.Authorization;
                paymentInfo.CreditCardToken = payment.CardToken;

                customerOrderInfo.Payments.Add(paymentInfo);
            }
            customerOrderInfo.OrderType = ErpCustomerOrderType.SalesOrder.ToString();
            customerOrderInfo.Items = new System.Collections.ObjectModel.ObservableCollection<ErpItemInfo>();

            foreach (ErpAdditionalSalesLine addOn in addOns)
            {
                ErpItemInfo itemInfo = new ErpItemInfo();

                itemInfo.ItemId = addOn.ItemId;
                itemInfo.InventDimensionId = addOn.InventDimensionId;
                itemInfo.Quantity = addOn.Quantity;

                customerOrderInfo.Items.Add(itemInfo);
            }

            //ErpSalesLine newSalesLine = new ErpSalesLine() { ItemId = newSalesLineItemId };
            ////getting item ids from integration database 
            //SalesOrderHelper soHelper = new SalesOrderHelper(currentStore.StoreKey);
            //soHelper.GetLineItemIdProductIdFromIntegrationDB(newSalesLine, newSalesLine);

            var updateSalesOrderManager = new UpdateSalesOrderCRTManager();
            return updateSalesOrderManager.UpdateContract(action, salesOrderId, salesLineRecId, newSalesLine, customerOrderInfo, currentStore.StoreKey);
        }

        #endregion

    }
}
