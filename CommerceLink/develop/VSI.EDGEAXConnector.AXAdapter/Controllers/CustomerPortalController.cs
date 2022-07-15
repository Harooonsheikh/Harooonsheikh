using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NewRelic.Api.Agent;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.AosClasses;
using VSI.EDGEAXConnector.ERPDataModels.CalculateContract;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class CustomerPortalController : BaseController, ICustomerPortalController
    {
        CustomerDAL objDAL = null;
        private const string TVRemoteAccess = "TVRA";

        public CustomerPortalController(string storeKey) : base(storeKey)
        {
            objDAL = new CustomerDAL(storeKey);
        }

        public ErpCreateContractNewPaymentMethodResponse CreateNewContractPaymentMethod(ErpCreateContractNewPaymentMethod request, string requestId)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.PAYPAL_EXPRESS.ToString()))
            {
                request.TenderLine.MaskedCardNumber = request.TenderLine.Email;
            }

            //Generating Blob for new Credit Card
            if (!request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.SEPA.ToString()) &&
                !request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.ADYEN_HPP.ToString())
               )
            {

                if (request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.ALLPAGO_CC.ToString()))
                {
                    request.TenderLine.UniqueCardId = request.TenderLine.CardToken;
                }
                else if (string.IsNullOrEmpty(request.TenderLine.UniqueCardId))
                {
                    request.TenderLine.UniqueCardId = request.TenderLine.CustomAttributes.FirstOrDefault(x => x.Key.ToLower().Equals("transaction-id")).Value;
                }
            }

            ErpSalesOrder salesOrder = new ErpSalesOrder();
            salesOrder.CustomerId = request.Customer.CustomerId;
            salesOrder.CurrencyCode = request.CurrencyCode;
            salesOrder.BillingAddress = request.Customer.BillingAddress;

            SalesOrderHelper soHelper = new SalesOrderHelper(currentStore.StoreKey);
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, "SetupPaymentMethod", DateTime.UtcNow);

            soHelper.SetupPaymentMethod(request.TenderLine, salesOrder, requestId);

            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, requestId, "SetupPaymentMethod", DateTime.UtcNow);

            var creditCardCRTManager = new CustomerPortalCRTManager();
            ErpCreateContractNewPaymentMethodResponse erpCreditCardResponse = creditCardCRTManager.CreateContractNewPaymentMethod(request, currentStore.StoreKey, requestId);

            if (erpCreditCardResponse.CreditCard != null)
            {
                PaymentConnectorDAL paymentConnector = new PaymentConnectorDAL(currentStore.StoreKey);

                PaymentConnector paymentConnectorValue = paymentConnector.GetPaymentConnectorUsingErpPaymentConnector(erpCreditCardResponse.CreditCard.CreditCardProcessors);

                if (paymentConnectorValue != null)
                {
                    erpCreditCardResponse.CreditCard.ProcessorId = paymentConnectorValue.EComCreditCardProcessorName;
                }
            }

            return erpCreditCardResponse;
        }

        public UpdateBillingAddressResponse UpdateBillingAddress(UpdateBillingAddressRequest updateBillingAddressRequest, string requestId)
        {
            var customerPortalCrtManager = new CustomerPortalCRTManager();
            return customerPortalCrtManager.UpdateBillingAddress(updateBillingAddressRequest, requestId, currentStore.StoreKey);
        }

        public UserSessionInfo TestCall(string requestId)
        {
            var customerPortalCrtManager = new CustomerPortalCRTManager();
            return customerPortalCrtManager.TestCall(requestId, currentStore.StoreKey);
        }
        [Trace]
        public ErpUpdateSubscriptionContractResponse UpdateSubscriptionContract(ErpUpdateSubscriptionContract request, string requestId)
        {
            var customerPortalCrtManager = new CustomerPortalCRTManager();

            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

            foreach (ErpContractLine contractLine in request.ContractLines)
            {
                var key = integrationManager.GetErpKey(Entities.Product, contractLine.ProductId);

                if (key != null)
                {
                    string productId = contractLine.ProductId;
                    string[] itemVariantId = productId.Split('_');
                    contractLine.ItemId = itemVariantId[0];
                    contractLine.VariantId = itemVariantId[1];
                }
                else
                {
                    string errorMessage = string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL401200), contractLine.ProductId);
                    return new ErpUpdateSubscriptionContractResponse(false, errorMessage, "");
                }
            }
            
            ErpUpdateSubscriptionContractResponse response =  customerPortalCrtManager.UpdateSubscriptionContract(request, currentStore.StoreKey, requestId);

            return response;
        }

        public ProcessContractTerminateResponse ProcessContractTerminate(ErpProcessContractTerminateRequest processContractTerminateRequest, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var customerPortalCrtManager = new CustomerPortalCRTManager();
            return customerPortalCrtManager.ProcessContractTerminate(processContractTerminateRequest, requestId, currentStore.StoreKey);
        }

        public ProcessContractReactivateResponse ProcessContractReactivate(ErpProcessContractReactivateRequest processContractReactivateRequest, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var customerPortalCrtManager = new CustomerPortalCRTManager();
            return customerPortalCrtManager.ProcessContractReactivate(processContractReactivateRequest, requestId, currentStore.StoreKey);
        }
        public UnblockContractResponse UnblockContract(UnblockContractRequest unblockContractRequest, string requestId)
        {
            var customerPortalCrtManager = new CustomerPortalCRTManager();
            return customerPortalCrtManager.UnblockContract(unblockContractRequest, requestId, currentStore.StoreKey);
        }

        public PromiseToPayResponse PromiseToPay(PromiseToPayRequest request, string requestId)
        {
            var customerPortalCrtManager = new CustomerPortalCRTManager();
            return customerPortalCrtManager.PromiseToPay(request, requestId, currentStore.StoreKey);
        }

        public AssignCustomerPortalAccessResponse AssignCustomerPortalAccess(AssignCustomerPortalAccessRequest assignCustomerPortalAccessRequest, string requestId)
        {

            var customerPortalCrtManager = new CustomerPortalCRTManager();
            return customerPortalCrtManager.AssignCustomerPortalAccess(assignCustomerPortalAccessRequest, requestId, currentStore.StoreKey);
        }

        public ContractCalculationResponse CalculateSubscriptionChange(bool useOldContractDates, DateTimeOffset contractStartDate, DateTimeOffset contractEndDate, CLSubscriptionOfferType subWeight, DateTimeOffset requestDate, long affiliationId, List<CLContractCartLine> cartLines, CLDeliverySpecification deliverySpecification, List<string> couponCodes, string requestId)
        {
            string cartId = string.Concat("TMV_CP_" + Guid.NewGuid());

            ProcessContractCartLine(cartLines);

            if (deliverySpecification != null && string.IsNullOrWhiteSpace(deliverySpecification.DeliveryModeId))
            {
                deliverySpecification.DeliveryModeId = configurationHelper.GetSetting(SALESORDER.AX_Default_Delivery_Mode);
            }

            List<CLContractCartLine> calcualteAbleCartLines = new List<CLContractCartLine>();
            foreach (CLContractCartLine cLine in cartLines)
            {
                calcualteAbleCartLines.Add(cLine);
            }

            //Remove Lines from cart request which are not required to calculate in case of Switch operation
            if (calcualteAbleCartLines.Any(cl => cl.CLSalesLineAction == CLContractOperation.Switch))
            {
                calcualteAbleCartLines.RemoveAll(c => c.CLSalesLineAction == CLContractOperation.Existing);
            }

            var customerPortalCrtManager = new CustomerPortalCRTManager();
            ContractCalculationResponse cartResponse = customerPortalCrtManager.CalculateSubscriptionChange(cartId, affiliationId, calcualteAbleCartLines, deliverySpecification, couponCodes, currentStore.StoreKey, requestId);

            //Recalculate contract here
            if (cartResponse.Cart != null)
            {
                CopyContractCartLinesIntoCartResponse(cartResponse.Cart, cartLines);

                CalculateTimeQuantity(contractStartDate, contractEndDate, subWeight, requestDate, cartResponse.Cart, useOldContractDates);

                CalculateCLAmounts(cartResponse.Cart, cartLines, useOldContractDates);

                //Remove Lines from cart response which are not required to show on ecom
                List<CLCartLine> removeableItems = cartResponse.Cart.CartLines.Where(cl => cl.CLSalesLineAction == CLContractOperation.Existing.ToString()).ToList();
                foreach(CLCartLine rLine in removeableItems)
                {
                    cartResponse.Cart.CartLines.Remove(rLine);
                }

                ReCalculateContractCart(cartResponse.Cart);
            }

            return cartResponse;
        }

        public ContractActivationLogResponse ContractActivationLog(ContractActivationLogRequest request, string requestId)
        {

            var customerPortalCrtManager = new CustomerPortalCRTManager();
            return customerPortalCrtManager.ContractActivationLog(request, requestId, currentStore.StoreKey);
        }

        #region Supporting Methods

        private void ProcessContractCartLine(List<CLContractCartLine> cartLines)
        {
            if (cartLines != null && cartLines.Count() > 0)
            {
                foreach (CLContractCartLine cartLine in cartLines)
                {
                    GetContractCartLineItemIdProductIdFromIntegrationDB(cartLine);

                    if (string.IsNullOrWhiteSpace(cartLine.UnitOfMeasureSymbol))
                    {
                        cartLine.UnitOfMeasureSymbol = configurationHelper.GetSetting(SALESORDER.AX_Default_UnitofMeasure);
                    }
                }
            }
        }

        private void GetContractCartLineItemIdProductIdFromIntegrationDB(CLContractCartLine line)
        {
            bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            var key = integrationManager.GetErpKey(Entities.Product, line.ItemId);
            if (key == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40503, currentStore, line.ItemId);
                throw new CommerceLinkError(message);
            }
            string itemId = string.Empty;
            string variantId = string.Empty;
            if (!isFlatProductHierarchy) //Standard Product Hierarchy
            {
                try
                {
                    var temp = key.Description.Split(':');

                    if (temp != null && temp.Any())
                    {
                        itemId = temp[0];
                        if (temp.Length > 1)
                        {
                            variantId = temp[1];
                        }
                    }
                }
                catch (Exception)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40504, currentStore);
                    throw new CommerceLinkError(message);
                }
                //Store input item id as EcomItemId
                line.EcomItemId = line.ItemId;
                line.ProductId = Convert.ToInt64(key.ErpKey);
                line.ItemId = itemId;
            }
            else
            {
                line.ProductId = Convert.ToInt64(key.ErpKey);
                line.ItemId = key.Description;
            }
        }

        private void CopyContractCartLinesIntoCartResponse(CLCart cart, List<CLContractCartLine> contractCartLines)
        {
            foreach (CLCartLine cartLine in cart.CartLines)
            {
                //Copy input values to response
                CLContractCartLine contractInputLine = contractCartLines.Where(ccl => ccl.LineId == cartLine.LineId).FirstOrDefault();

                if (contractInputLine != null)
                {
                    cartLine.TMVContractCalculateFrom = contractInputLine.TMVContractCalculateFrom;
                    cartLine.TMVContractCalculateTo = contractInputLine.TMVContractCalculateTo;
                    cartLine.TMVOldLineInvoiceAmountAndAdjustment = contractInputLine.TMVOldLineInvoiceAmountAndAdjustment;
                    cartLine.CLSalesLineAction = contractInputLine.CLSalesLineAction.ToString();
                    cartLine.CLSwitchFromLineId = contractInputLine.CLSwitchFromLineId;
                    cartLine.CLParentLineNumber = contractInputLine.CLParentLineNumber;

                    //No need to do check from integration DB get it from input
                    cartLine.ItemId = contractInputLine.EcomItemId;
                }
                //Handling Mix & Max discounted line which are extra line in cart generated by system
                else
                {
                    if (cartLine.RelatedDiscountedLineIds != null && cartLine.RelatedDiscountedLineIds.Count > 0)
                    {
                        string relatedDiscountedLineIds = cartLine.RelatedDiscountedLineIds.FirstOrDefault();
                        contractInputLine = contractCartLines.Where(ccl => ccl.LineId == relatedDiscountedLineIds).FirstOrDefault();

                        cartLine.TMVContractCalculateFrom = contractInputLine.TMVContractCalculateFrom;
                        cartLine.TMVContractCalculateTo = contractInputLine.TMVContractCalculateTo;
                        cartLine.TMVOldLineInvoiceAmountAndAdjustment = 0;
                        cartLine.CLSalesLineAction = CLContractOperation.New.ToString();
                        cartLine.CLParentLineNumber = contractInputLine.CLParentLineNumber;

                        //No need to do check from integration DB get it from input
                        cartLine.ItemId = contractInputLine.EcomItemId;
                    }
                }

                //Ecom require virtual(ComKey) in ItemId for Cart API response
                /*
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                var key = integrationManager.GetComKey(Entities.Product, cartLine.ProductId.ToString());
                if (key != null)
                {
                    cartLine.ItemId = key.ComKey;
                }
                */
            }
        }

        private void CalculateTimeQuantity(DateTimeOffset contractStartDate, DateTimeOffset contractEndDate, CLSubscriptionOfferType subWeight, DateTimeOffset requestDate, CLCart cart, bool useOldContractDates)
        {
            int subscriptionWeight = (int)subWeight;

            foreach(CLCartLine cartLine in cart.CartLines)
            {
                if (cartLine.TMVContractCalculateTo != null && requestDate != null && subscriptionWeight > 0)
                {
                    TimeSpan spanA = cartLine.TMVContractCalculateTo.Date - requestDate.Date.AddDays(-1);
                    TimeSpan spanB = new TimeSpan();

                    if (subscriptionWeight > 1)
                    {
                        spanB = cartLine.TMVContractCalculateTo.Date - cartLine.TMVContractCalculateTo.Date.AddMonths(-1 * subscriptionWeight);
                    }
                    else
                    {
                        spanB = requestDate.Date.AddMonths(subscriptionWeight).AddDays(-1) - requestDate.Date.AddDays(-1);
                    }

                    //cartLine.CLTimeQantity = Math.Round((decimal)spanA.Days / spanB.Days, 6);
                    cartLine.CLTimeQantity = (decimal)spanA.Days / spanB.Days;
                }
                else
                {
                    cartLine.CLTimeQantity = 1;
                }
            }
        }

        private void CalculateCLAmounts(CLCart cart, List<CLContractCartLine> contractCartLines, bool useOldContractDates)
        {
            foreach (CLCartLine cartLine in cart.CartLines)
            {
                decimal oldLineInvoiceAmount = 0;

                CLContractOperation operation;
                Enum.TryParse(cartLine.CLSalesLineAction, out operation);

                //(CurrentPrice - OldLineInvoiceAmount) * TimeQuantity

                //In New Line operation no adjustment
                //In QuantityUpgrade Line operation adjust amount of old quantity
                //In Switch Line operation adjust amount of old line
                //In Existing Line operation no adjustment
                switch (operation)
                {
                    
                    case CLContractOperation.New:
                            //Do noting
                        break;
                    case CLContractOperation.QuantityUpgrade:
                            oldLineInvoiceAmount = cartLine.TMVOldLineInvoiceAmountAndAdjustment;
                        break;
                    case CLContractOperation.Switch:
                        if (!string.IsNullOrEmpty(cartLine.CLSwitchFromLineId))
                        {
                            //TODO: in case of TV Core to Tensor switch need to understand logic from AX and write here
                            //Switch with other product
                            if (cartLine.CLSwitchFromLineId != cartLine.LineId)
                            {
                                CLContractCartLine switchFromCartLine = contractCartLines.Where(cl => cl.LineId == cartLine.CLSwitchFromLineId).FirstOrDefault();
                                if (switchFromCartLine != null)
                                {
                                    string productName = switchFromCartLine.ItemId.Substring(0, 4);
                                    //Switch form TV Core to Core
                                    if (productName != TVRemoteAccess)
                                    {
                                        oldLineInvoiceAmount = switchFromCartLine.TMVOldLineInvoiceAmountAndAdjustment;
                                    }
                                    //Switch from TV Remote Access to TV Core  
                                    else
                                    {
                                        decimal totalDiscount = switchFromCartLine.TMVOldLineInvoiceAmountAndAdjustment;

                                        List<CLContractCartLine> remoteAccessAddOns = new List<CLContractCartLine>();
                                        remoteAccessAddOns = contractCartLines.Where(cl => cl.CLParentLineNumber == cartLine.LineId && cl.ItemId.Contains(TVRemoteAccess)).ToList();
                                        foreach(CLContractCartLine cLine in remoteAccessAddOns)
                                        {
                                            totalDiscount += cLine.TMVOldLineInvoiceAmountAndAdjustment;
                                            //TV RA Add-Ons cannot be switch into new contract so changing their CLSalesLineAction = Existing and before ReCalculation it will be removed
                                            CLCartLine cLCartLine = cart.CartLines.Where(cl => cl.LineId == cLine.LineId).FirstOrDefault();
                                            if(cLCartLine != null)
                                            {
                                                cLCartLine.CLSalesLineAction = CLContractOperation.Existing.ToString();
                                            }
                                        }
                                        oldLineInvoiceAmount = totalDiscount;
                                    }
                                }
                            }
                            //Self Switch or Transfer
                            else
                            {
                                oldLineInvoiceAmount = cartLine.TMVOldLineInvoiceAmountAndAdjustment;
                            }
                        }
                        break;                    
                    case CLContractOperation.Existing:
                            //Do noting
                        break;
                }

                int roundingValue = 2;
                int salesOrderPriceRounding = -1;
                salesOrderPriceRounding = Convert.ToInt32(configurationHelper.GetSetting(APPLICATION.ERP_AX_SalesOrderPriceRounding));
                if (salesOrderPriceRounding > -1)
                {
                    roundingValue = salesOrderPriceRounding;
                }

                if (!useOldContractDates)
                {
                    oldLineInvoiceAmount = Math.Round((decimal)(oldLineInvoiceAmount) * cartLine.CLTimeQantity, roundingValue);

                    cartLine.CLTimeQantity = 1;

                }

                //NewLine, QuantityUpgrade and Switch from line having old line invoice amount 
                if (cartLine.NetAmountWithoutTax > 0)
                {
                    cartLine.CLNetAmountWithoutTax = Math.Round((decimal)(cartLine.NetAmountWithoutTax - oldLineInvoiceAmount) * cartLine.CLTimeQantity, roundingValue);                    
                }
                else
                {
                    cartLine.CLNetAmountWithoutTax = Math.Round((decimal)cartLine.NetAmountWithoutTax, roundingValue);
                }

                //do CL net amount 0 if its negative value
                if (cartLine.CLNetAmountWithoutTax < 0)
                {
                    cartLine.CLNetAmountWithoutTax = 0;
                }

                cartLine.CLAdjustmentAmount = Math.Round((decimal)cartLine.NetAmountWithoutTax - cartLine.CLNetAmountWithoutTax, roundingValue);
                cartLine.CLTaxAmount = Math.Round((decimal)(cartLine.CLNetAmountWithoutTax * cartLine.TaxRatePercent / 100), roundingValue);
                cartLine.CLTotalAmount = Math.Round((decimal)(cartLine.CLNetAmountWithoutTax + cartLine.CLTaxAmount), roundingValue);
            }
        }

        private void ReCalculateContractCart(CLCart cart)
        {
            if (cart.CartLines != null && cart.CartLines.Count() > 0)
            {
                int roundingValue = 2;
                int salesOrderPriceRounding = -1;
                salesOrderPriceRounding = Convert.ToInt32(configurationHelper.GetSetting(APPLICATION.ERP_AX_SalesOrderPriceRounding));
                if (salesOrderPriceRounding > -1)
                {
                    roundingValue = salesOrderPriceRounding;
                }

                //Process lines
                foreach (CLCartLine cartLine in cart.CartLines)
                {
                    //Rounding each discount on 2 decimal places
                    foreach (ErpDiscountLine discountLine in cartLine.DiscountLines)
                    {
                        int discountPriceRounding = -1;
                        discountPriceRounding = Convert.ToInt32(configurationHelper.GetSetting(APPLICATION.ERP_AX_DiscountPriceRounding));

                        if (discountPriceRounding > -1)
                        {
                            discountLine.EffectiveAmount = Math.Round(discountLine.EffectiveAmount, discountPriceRounding);
                        }
                        else
                        {
                            discountLine.EffectiveAmount = Math.Round(discountLine.EffectiveAmount, 2);
                        }
                    }
                    //Update total discount amount of cart line
                    cartLine.DiscountAmount = cartLine.DiscountLines.Sum(d => d.EffectiveAmount);
                    cartLine.CLDiscountAmount = cartLine.DiscountLines.Sum(d => d.EffectiveAmount);
                    
                    cartLine.Price = Math.Round(Convert.ToDecimal(cartLine.Price), roundingValue);

                    //Update NetAmountWithoutTax
                    cartLine.NetAmountWithoutTax = (cartLine.Price * cartLine.Quantity) - cartLine.DiscountAmount;

                    cartLine.TaxRatePercent = Math.Round((decimal)cartLine.TaxRatePercent, roundingValue);

                    //Re-calculate tax
                    if (cartLine.TaxRatePercent > 0)
                    {
                        cartLine.TaxAmount = Math.Round((decimal)(cartLine.NetAmountWithoutTax * cartLine.TaxRatePercent / 100), roundingValue);
                    }

                    //Update line totals
                    cartLine.TotalAmount = Math.Round((decimal)(cartLine.NetAmountWithoutTax + cartLine.TaxAmount), roundingValue);                    
                }

                //Update cart discounts
                cart.DiscountAmount = cart.CartLines.Sum(d => d.DiscountAmount);
                cart.CLDiscountAmount = cart.CartLines.Sum(d => d.CLDiscountAmount);
                
                //Update cart Taxs
                cart.TaxAmount = cart.CartLines.Sum(c => c.TaxAmount);
                cart.CLTaxAmount = cart.CartLines.Sum(c => c.CLTaxAmount);
                
                //Update cart Totals
                cart.SubtotalAmount = cart.CartLines.Sum(st => st.NetAmountWithoutTax);
                cart.SubtotalAmountWithoutTax = cart.CartLines.Sum(st => st.NetAmountWithoutTax);
                cart.TotalAmount = cart.CartLines.Sum(cl => cl.TotalAmount);
                cart.CLSubtotalAmount = cart.CartLines.Sum(st => st.CLNetAmountWithoutTax);
                cart.CLSubtotalAmountWithoutTax = cart.CartLines.Sum(st => st.CLNetAmountWithoutTax);
                cart.CLTotalAmount = cart.CartLines.Sum(cl => cl.CLTotalAmount);
            }
        }

        #endregion

        
    }
}
