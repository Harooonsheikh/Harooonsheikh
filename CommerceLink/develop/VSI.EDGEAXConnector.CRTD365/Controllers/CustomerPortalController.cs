using EdgeAXCommerceLink.RetailProxy.Extensions;
using Microsoft.Dynamics.Commerce.RetailProxy;
using NewRelic.Api.Agent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels.CalculateContract;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;
using AssignCustomerPortalAccessResponse = VSI.EDGEAXConnector.ERPDataModels.Custom.AssignCustomerPortalAccessResponse;
using UnblockContractResponse = VSI.EDGEAXConnector.ERPDataModels.Custom.UnblockContractResponse;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class CustomerPortalController : BaseController, ICustomerPortalController
    {
        public CustomerPortalController(string storeKey) : base(storeKey)
        {

        }

        public ErpCreateContractNewPaymentMethodResponse CreateContractNewPaymentMethod(ErpCreateContractNewPaymentMethod request, string requestId)
        {
            bool isExternalSystemTimeLogged = false;
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpCreateContractNewPaymentMethodResponse erpResponse = new ErpCreateContractNewPaymentMethodResponse(false, "", null, null, string.Empty);
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, "External", DateTime.UtcNow);
            try
            {
                timer = Stopwatch.StartNew();
                CreateContractNewPaymentMethodResponse rsResponse = ECL_CreateContractNewPaymentMethod(request);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateContractNewPaymentMethod", GetElapsedTime());

                if ((bool)rsResponse.Status)
                {
                    string recId = rsResponse.RecId;
                    string mandateRecId = rsResponse.MandateRecId;

                    ErpBankAccount erpBankAccount = null;

                    if (string.IsNullOrEmpty(request.TenderLine.IBAN))
                    {
                        // Credit card was created
                        request.TenderLine.RecId = Convert.ToInt64(recId);
                        erpResponse = new ErpCreateContractNewPaymentMethodResponse(true, rsResponse.Message, request.TenderLine, erpBankAccount, rsResponse.ErrorCode);
                    }
                    else
                    {
                        // Bank account was created
                        erpBankAccount = new ErpBankAccount(Convert.ToInt64(recId), Convert.ToInt64(mandateRecId));
                        erpResponse = new ErpCreateContractNewPaymentMethodResponse(true, rsResponse.Message, null, erpBankAccount, rsResponse.ErrorCode);
                    }
                }
                else
                {
                    erpResponse = new ErpCreateContractNewPaymentMethodResponse(false, rsResponse.Message, null, null, rsResponse.ErrorCode);
                }

            }
            catch (Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, requestId, "External", DateTime.UtcNow);
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "CreateContractNewPaymentMethod", GetElapsedTime());
                }
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpResponse = new ErpCreateContractNewPaymentMethodResponse(false, exp.Message, null, null, string.Empty);
            }
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, requestId, "External", DateTime.UtcNow);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, requestId, MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            return erpResponse;
        }
        public UpdateBillingAddressResponse UpdateBillingAddress(UpdateBillingAddressRequest updateBillingAddressRequest, string requestId)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            try
            {
                bool isExternalSystemTimeLogged = false;
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, "External", DateTime.UtcNow);

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
                UpdateBillingAddressResponse erpResponse = new UpdateBillingAddressResponse(false, string.Empty);
                try
                {
                    timer = Stopwatch.StartNew();
                    var rsResponse = ECL_TV_CreateContactAndUpdateBillingAddress(updateBillingAddressRequest);
                    isExternalSystemTimeLogged = true;
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, requestId, "External", DateTime.UtcNow);

                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "UpdateBillingAddress", GetElapsedTime());

                    erpResponse.Status = (bool)rsResponse.Status;
                    erpResponse.Message = rsResponse.Message;
                    if (erpResponse.Status)
                    {
                        erpResponse.Message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL700000);
                    }

                    return erpResponse;

                }
                catch (Exception exp)
                {
                    if (!isExternalSystemTimeLogged)
                    {
                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, requestId, "External", DateTime.UtcNow);

                        CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "UpdateBillingAddress", GetElapsedTime());
                    }
                    CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                    erpResponse = new UpdateBillingAddressResponse(false, exp.Message);
                }
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
                return erpResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public ContractCalculationResponse CalculateSubscriptionChange(string cartId, long affiliationId, List<CLContractCartLine> cartLines, CLDeliverySpecification deliverySpecification, List<string> couponCodes, string requestId)
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, MethodBase.GetCurrentMethod().Name, DateTime.UtcNow);

            bool isExternalSystemTimeLogged = false;
            ContractCalculationResponse clCartResponse = new ContractCalculationResponse(false, "", null);
            try
            {
                CLCart clCart = new CLCart();
                CartResponse cartResponse = new CartResponse();
                List<CartLine> cLines = new List<CartLine>();

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, "Mapping", DateTime.UtcNow);

                cLines = _mapper.Map<List<CLContractCartLine>, List<CartLine>>(cartLines);
                var deliverySpec = _mapper.Map<CLDeliverySpecification, DeliverySpecification>(deliverySpecification);

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, requestId, "Mapping", DateTime.UtcNow);

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, "External", DateTime.UtcNow);

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_CreateCLCartforContractCalculation", DateTime.UtcNow);
                timer = Stopwatch.StartNew();
                cartResponse = ECL_CreateCLCartforContractCalculation(cartId, affiliationId, couponCodes, cLines, deliverySpec);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CalculateSubscriptionChange", GetElapsedTime());
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, requestId, "External", DateTime.UtcNow);

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_CreateCLCartforContractCalculation", DateTime.UtcNow);

                if ((bool)cartResponse.Status && couponCodes.Count() > 0)
                {
                    return AddCouponsToCart(cartId, couponCodes, false, requestId);
                }

                isExternalSystemTimeLogged = true;
                if (cartResponse.Result != null)
                {
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, requestId, "Deserializing_Mapping", DateTime.UtcNow);

                    Cart cartRes = JsonConvert.DeserializeObject<Cart>(cartResponse.Result);
                    CLCart clCartWithItems = _mapper.Map<Cart, CLCart>(cartRes);

                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, requestId, "Deserializing_Mapping", DateTime.UtcNow);

                    clCartResponse = new ContractCalculationResponse((bool)cartResponse.Status, cartResponse.Message, clCartWithItems);
                }
                else
                {
                    clCartResponse = new ContractCalculationResponse(false, cartResponse.Message, null);
                }
            }
            catch (RetailProxyException rpe)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, requestId, "External", DateTime.UtcNow);
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "CalculateSubscriptionChange", GetElapsedTime());
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_CreateCLCartforContractCalculation", DateTime.UtcNow);
                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                clCartResponse = new ContractCalculationResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "CalculateSubscriptionChange", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, requestId, "External", DateTime.UtcNow);
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_CreateCLCartforContractCalculation", DateTime.UtcNow);
                }
               
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }

            return clCartResponse;
        }
        public UnblockContractResponse UnblockContract(UnblockContractRequest unblockContractRequest, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            UnblockContractResponse erpResponse = new UnblockContractResponse(false, string.Empty);
            try
            {

                var rsResponse = ECL_TV_UnblockContract(unblockContractRequest);
                erpResponse.Status = (bool)rsResponse.Status;
                erpResponse.Message = rsResponse.Message;

                return erpResponse;
            }
            catch (Exception exp)
            {
                string message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                erpResponse = new UnblockContractResponse(false, message);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }

        public ERPDataModels.Custom.PromiseToPayResponse PromiseToPay(PromiseToPayRequest request, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var erpResponse = new ERPDataModels.Custom.PromiseToPayResponse(false, string.Empty);
            try
            {

                var rsResponse = ECL_TV_PromiseToPay(request);
                erpResponse.Status = (bool)rsResponse.Status;
                erpResponse.Message = rsResponse.Message;

                return erpResponse;
            }
            catch (Exception exp)
            {
                string message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                erpResponse = new ERPDataModels.Custom.PromiseToPayResponse(false, message);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }

        public AssignCustomerPortalAccessResponse AssignCustomerPortalAccess(AssignCustomerPortalAccessRequest assignCustomerPortalAccessRequest, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            AssignCustomerPortalAccessResponse erpResponse = new AssignCustomerPortalAccessResponse(false, string.Empty);
            try
            {
                var rsResponse = ECL_TV_AssignCustomerPortalAccess(assignCustomerPortalAccessRequest);
                erpResponse.Status = (bool)rsResponse.Status;
                erpResponse.Message = rsResponse.Message;
                erpResponse.StatusCode = rsResponse.Result;
                return erpResponse;

            }
            catch (Exception exp)
            {
                string message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                erpResponse = new AssignCustomerPortalAccessResponse(false, message, "EXCPTION");
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        public ERPDataModels.Custom.ContractActivationLogResponse ContractActivationLog(ContractActivationLogRequest request, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var erpResponse = new ERPDataModels.Custom.ContractActivationLogResponse(false, string.Empty, string.Empty);

            try
            {
                var rsResponse = ECL_TV_ContractActivationLog(request);
                erpResponse.Status = (bool)rsResponse.Status;
                erpResponse.Message = rsResponse.Message;

                return erpResponse;
            }
            catch (Exception exp)
            {
                string message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId,
                    CommerceLinkLogger.GetInnerMostErrorMessage(exp));
                erpResponse = new ERPDataModels.Custom.ContractActivationLogResponse(false, message, string.Empty);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }

        public ContractCalculationResponse AddCouponsToCart(string cartId, IEnumerable<string> couponCodes, bool isLegacyDiscountCode, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var response = new ContractCalculationResponse(false, "", null);
            try
            {
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "External ECL_AddCouponsToCart Starts", DateTime.UtcNow);
                timer = Stopwatch.StartNew();

                var cartResponse = ECL_ValidateCouponsToCart(cartId, couponCodes, isLegacyDiscountCode);
                if (!(bool)cartResponse.Status)
                {
                    return new ContractCalculationResponse((bool)cartResponse.Status, cartResponse.Message, null);
                }

                var cart = ECL_AddCouponsToCart(cartId, couponCodes, isLegacyDiscountCode);

                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "AddCouponsToCart", GetElapsedTime());

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "External ECL_AddCouponsToCart Ends", DateTime.UtcNow);

                if (cart != null)
                {

                    var erpCart = _mapper.Map<Cart, CLCart>(cart);
                    response = new ContractCalculationResponse(true, "VSIRSTV20000 | Success! A discount has been applied to your purchase.", erpCart);
                }
                else
                {
                    response = new ContractCalculationResponse(false, "Empty cart", null);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);

                response = new ContractCalculationResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                response = new ContractCalculationResponse(false, exp.Message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return response;
        }

        #region RetailServer API Calls
        [Trace]
        private CreateContractNewPaymentMethodResponse ECL_CreateContractNewPaymentMethod(ErpCreateContractNewPaymentMethod request)
        {
            var contractPaymentMethodManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await contractPaymentMethodManager.ECL_CreateContractNewPaymentMethod(request.SalesId,
                                request.Customer.CustomerId,
                                request.TenderLine.UniqueCardId,
                                request.TenderLine.CardToken,
                                request.TenderLine.Authorization,
                                request.TenderLine.CardOrAccount,
                                request.TenderLine.IBAN,
                                request.TenderLine.SwiftCode,
                                request.TenderLine.BankName,
                                request.TenderLine.TenderTypeId,
                                baseCompany)).Result;
        }

        [Trace]
        private CreateContactAndUpdateBillingAddressResponse ECL_TV_CreateContactAndUpdateBillingAddress(UpdateBillingAddressRequest updateBillingAddressRequest)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_TV_CreateContactAndUpdateBillingAddress(
                                        updateBillingAddressRequest.SalesOrderId, updateBillingAddressRequest.FirstName,
                                        updateBillingAddressRequest.LastName, updateBillingAddressRequest.CustomerEmail,
                                        updateBillingAddressRequest.VATNumber, updateBillingAddressRequest.Phone,
                                        updateBillingAddressRequest.BillingAddress.Name, updateBillingAddressRequest.BillingAddress.Street,
                                        updateBillingAddressRequest.BillingAddress.City, updateBillingAddressRequest.BillingAddress.State,
                                        updateBillingAddressRequest.BillingAddress.ZipCode, updateBillingAddressRequest.BillingAddress.ThreeLetterISORegionName, 
                                        updateBillingAddressRequest.LocalTaxId, baseCompany
                                    )).Result;
        }
        [Trace]
        private CartResponse ECL_CreateCLCartforContractCalculation(string cartId, long affiliationId, List<string> couponCodes, List<CartLine> cLines, DeliverySpecification deliverySpec)
        {
            var cartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            return Task.Run(async () => await cartManager.ECL_CreateCLCartforContractCalculation(cartId, affiliationId, cLines, deliverySpec)).Result;
        }
        
        [Trace]
        private EdgeAXCommerceLink.RetailProxy.Extensions.UnblockContractResponse ECL_TV_UnblockContract(UnblockContractRequest unblockContractRequest)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_TV_UnblockContract(unblockContractRequest.SalesOrderId, unblockContractRequest.InvoiceId, baseCompany)).Result;
        }

        [Trace]
        private EdgeAXCommerceLink.RetailProxy.Extensions.PromiseToPayResponse ECL_TV_PromiseToPay(PromiseToPayRequest request)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_TV_PromiseToPay(request.InvoiceId, baseCompany)).Result;
        }

        [Trace]
        private EdgeAXCommerceLink.RetailProxy.Extensions.AssignCustomerPortalAccessResponse ECL_TV_AssignCustomerPortalAccess(AssignCustomerPortalAccessRequest assignCustomerPortalAccessRequest)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_AssignCustomerPortalAccess(assignCustomerPortalAccessRequest.PACLicense, assignCustomerPortalAccessRequest.InvoiceId, baseCompany)).Result;
        }
        [Trace]
        private EdgeAXCommerceLink.RetailProxy.Extensions.ContractActivationLogResponse ECL_TV_ContractActivationLog(ContractActivationLogRequest request)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_ContractActivationLog(request.SalesId, request.Email, (DateTime)request.RequestedDate, baseCompany)).Result;
        }

        [Trace]
        private CartResponse ECL_ValidateCouponsToCart(string cartId, IEnumerable<string> couponCodes, bool isLegacyDiscountCode)
        {
            var clCartManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICartManager>();
            return Task.Run(async () => await clCartManager.ECL_ValidateCouponsToCart(cartId, couponCodes, isLegacyDiscountCode)).Result;
        }

        [Trace]
        private Cart ECL_AddCouponsToCart(string cartId, IEnumerable<string> couponCodes, bool isLegacyDiscountCode)
        {
            var cartManager = RPFactory.GetManager<Microsoft.Dynamics.Commerce.RetailProxy.ICartManager>();
            return Task.Run(async () => await cartManager.AddCoupons(cartId, couponCodes, isLegacyDiscountCode)).Result;
        }

        #endregion
    }
}
