using System.Collections.Generic;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using System;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Enums.Enums;
using System.Reflection;
using Newtonsoft.Json;
using VSI.CommerceLink.EcomDataModel.Request;
using VSI.CommerceLink.EcomDataModel.Response;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums.Enums.TMV;
using VSI.EDGEAXConnector.Web.ActionFilters;
using VSI.EDGEAXConnector.Data.DTO;
using Microsoft.Dynamics.Retail.PaymentSDK.Portable;

namespace VSI.EDGEAXConnector.Web
{

    #region OldCode

    /*   /// <summary>
       /// PaymentController defines properties and methods for API controller for payment.
       /// </summary>

       [Authorize(Roles = "eCommerce")]
       public class PaymentController : ApiBaseController
       {
           public static readonly string PaymentManagerService = "PaymentManager";

           #region API Methods

           //TODO: Test Code 
           //https://localhost:44302/api/Payment/ProcessPayment

           /// <summary>
           /// ProcessPayment processes payment.
           /// </summary>
           /// <param name="card"></param>
           /// <returns></returns>
           [HttpPost]
           public IHttpActionResult ProcessPayment(PaymentParameter parameter)
           {
               try
               {
                   bool isCapture = parameter.isCapture;
                   List<CartTenderLine> cartTenderLineList = new List<CartTenderLine> { parameter.CartTenderLine };
                   //CartTenderLine temp = new CartTenderLine();
                   //temp.TokenizedPaymentCard = new TokenizedPaymentCard();
                   //temp.TokenizedPaymentCard.CardTokenInfo = new CardTokenInfo();
                   //temp.PaymentCard = new PaymentCard();
                   //var json = temp.SerializeToJson(1);

                   List<TenderLine> tenderLines = new List<TenderLine>();


                   tenderLines = ProcessPendingOrderPayments(CommerceRuntimeHelper.RequestContext, cartTenderLineList, isCapture); 

                   //VSI.EDGEAXConnector.AXAdapter.Controllers.PaymentController paymentController = new VSI.EDGEAXConnector.AXAdapter.Controllers.PaymentController();
                   //paymentController.ProcessPayment(cartTenderLineList);

                   //var data = new { succeeded = true };
                   var data = new { tenderLines };
                   return Ok(data);

               }
               catch (System.Exception ex)
               {
                   var data = new { succeeded = false, errorMessage = ex.Message };
                   return Ok(data);

               }


           }

           public static List<TenderLine> ProcessPendingOrderPayments(RequestContext context, IEnumerable<CartTenderLine> cartTenderLines, bool isCapture)
           {
               ThrowIf.Null(context, "context");
               //ThrowIf.Null(context.GetSalesTransaction(), "context.GetSalesTransaction()");
               ThrowIf.Null(cartTenderLines, "cartTenderLines");

               // Assign id to each cart tender lines
               foreach (CartTenderLine cartTenderLine in cartTenderLines)
               {
                   cartTenderLine.TenderLineId = Guid.NewGuid().ToString();
               }

               //decimal totalTenderedAmount = GetPaymentsSum(cartTenderLines);

               // If the total of tender line amounts do not match cart total we cannot create order
               //if (context.GetSalesTransaction().TotalAmount != totalTenderedAmount)
               //{
               //    var exception = new DataValidationException(
               //        DataValidationErrors.AmountDueMustBePaidBeforeCheckout, "Tender line totals do not match cart total. Transaction = {0}, Tender Total = {1}, Cart Total = {2}", context.GetSalesTransaction().Id, totalTenderedAmount, context.GetSalesTransaction().TotalAmount);

               //    throw exception;
               //}

               //NetTracer.Information("OrderWorkflowHelper.AuthorizePayments(): Transaction = {0}, CustomerId = {1}", context.GetSalesTransaction().Id, context.GetSalesTransaction().CustomerId);

               List<TenderLine> tenderLines = new List<TenderLine>();

               ChannelDataManager channelDataManager = new ChannelDataManager(context);
               channelDataManager.GetChannelConfiguration();

               try
               {
                   foreach (CartTenderLine cartTenderLine in cartTenderLines)
                   {
                       ReadOnlyCollection<TenderType> channelTenderTypes =
                       new ChannelDataManager(context).GetChannelTenderTypes(context.GetPrincipal().ChannelId, new QueryResultSettings(PagingInfo.AllRecords));
                       TenderType type = channelTenderTypes.SingleOrDefault(channelTenderType => string.Equals(channelTenderType.TenderTypeId, cartTenderLine.TenderTypeId, StringComparison.OrdinalIgnoreCase));
                       TenderLine tenderLine = new TenderLine();
                       if (!isCapture)//(type.OperationId == (int)RetailOperation.PayCard)
                       {
                           tenderLine = GenerateCardTokenAndGetAuthorization(context, cartTenderLine);
                       }
                       else
                       {

                           tenderLine = AuthorizeAndCapturePayment(context, cartTenderLine, skipLimitValidation: false);
                       }

                       tenderLines.Add(tenderLine);
                   }
               }
               catch (Exception)
               {
                   try
                   {
                       // Cancel the payment authorizations
                       if (tenderLines.Any())
                       {
                           CancelPayments(context, tenderLines, cartTenderLines);
                       }
                   }
                   catch (Exception)
                   {
                       //NetTracer.Error("Voiding payments failed with: {0}, {1}, {2}", cancelPaymentsEx, cancelPaymentsEx.ToString(), ex);
                       throw;
                   }

                   throw;
               }

               // Setting tender lines on the salesTransaction here so they can be used for saving order in the database.
               //context.GetSalesTransaction().TenderLines.Clear();
               //context.GetSalesTransaction().TenderLines.AddRange(tenderLines);

               return tenderLines;
           }

           public static decimal GetPaymentsSum(IEnumerable<TenderLineBase> tenderLines)
           {
               if (tenderLines == null)
               {
                   throw new ArgumentNullException("tenderLines");
               }

               var notVoidedTenderLines = tenderLines.Where(t => t.Status != TenderLineStatus.Voided && t.Status != TenderLineStatus.Historical);
               decimal paymentsSum = notVoidedTenderLines.Sum(t => t.Amount);

               return paymentsSum;
           }
           private static TenderLine AuthorizeAndCapturePayment(RequestContext context, CartTenderLine cartTenderLine, bool skipLimitValidation)
           {
               ThrowIf.Null(cartTenderLine, "cartTenderLine");
               TenderLine tenderLine = new TenderLine { TenderLineId = cartTenderLine.TenderLineId };
               tenderLine.CopyPropertiesFrom(cartTenderLine);
               var authorizedTenderLine = AuthorizePayment(context, tenderLine, cartTenderLine.PaymentCard, skipLimitValidation);
               return CapturePayments(context, authorizedTenderLine, cartTenderLine.PaymentCard);
           }

           private static TenderLine AuthorizePayment(RequestContext context, TenderLine tenderLine, PaymentCard paymentCard, bool skipLimitValidation)
           {
               AuthorizePaymentServiceRequest authorizeRequest = new AuthorizePaymentServiceRequest(tenderLine, paymentCard, skipLimitValidation);

               IRequestHandler paymentManagerHandler = context.Runtime.GetRequestHandler(authorizeRequest.GetType(), PaymentManagerService);

               var authorizeResponse = context.Execute<AuthorizePaymentServiceResponse>(authorizeRequest);
               // var authorizeResponse = AuthorizePayment(authorizeRequest);
               if (authorizeResponse.TenderLine == null)
               {
                   throw new PaymentException(PaymentErrors.Microsoft_Dynamics_Commerce_Runtime_UnableToAuthorizePayment, "Payment service did not return tender line.");
               }

               return authorizeResponse.TenderLine;
           }

           private static TenderLine CapturePayments(RequestContext context, TenderLine tenderLine, PaymentCard pay)
           {
               switch (tenderLine.Status)
               {
                   case TenderLineStatus.PendingCommit:
                       {
                           CapturePaymentServiceRequest captureRequest = new CapturePaymentServiceRequest(tenderLine, pay);



                           IRequestHandler paymentManagerHandler = context.Runtime.GetRequestHandler(captureRequest.GetType(), ServiceTypes.PaymentManagerService);
                           CapturePaymentServiceResponse captureResponse = context.Execute<CapturePaymentServiceResponse>(captureRequest, paymentManagerHandler);

                           // CapturePaymentServiceResponse captureResponse = CapturePayment(captureRequest, paymentManagerHandler);
                           return captureResponse.TenderLine;
                       }

                   case TenderLineStatus.Committed:
                       {
                           return tenderLine;
                       }

                   default:
                       {
                           throw new InvalidOperationException(string.Format("Payment authorization returned unexpected tender line status: {0}", tenderLine.Status));
                       }
               }
           }

           private static TenderLine GenerateCardTokenAndGetAuthorization(RequestContext context, CartTenderLine cartTenderLine)
           {
               ThrowIf.Null(cartTenderLine, "tenderLine");
               TenderLine tenderLine = new TenderLine { TenderLineId = cartTenderLine.TenderLineId };

               tenderLine.CopyPropertiesFrom(cartTenderLine);

               TokenizedPaymentCard tokenizedPaymentCard;
               string tokenResponsePaymentProperties = string.Empty;

               if (cartTenderLine.PaymentCard != null)
               {
                   GenerateCardTokenPaymentServiceRequest tokenRequest = new GenerateCardTokenPaymentServiceRequest(tenderLine, cartTenderLine.PaymentCard);

                   // IRequestHandler PaymentHandler = context.Runtime.GetRequestHandler(tokenRequest.GetType());


                   GenerateCardTokenPaymentServiceResponse tokenResponse = context.Execute<GenerateCardTokenPaymentServiceResponse>(tokenRequest);


                   //GenerateCardTokenPaymentServiceResponse tokenResponse = GenerateCardToken(tokenRequest);

                   tenderLine = tokenResponse.TenderLine;
                   tokenizedPaymentCard = tokenResponse.TokenizedPaymentCard;
                   tokenResponsePaymentProperties = tokenResponse.TokenResponsePaymentProperties;
               }
               // Dont know how these fields( CardToken ,MaskedCardNumber ,UniqueCardId ) generating externally
               else if (cartTenderLine.TokenizedPaymentCard != null)
               {
                   //Token response properties blob will remain empty since this token was generated externally.
                   tokenizedPaymentCard = cartTenderLine.TokenizedPaymentCard;
               }
               else
               {
                   throw new DataValidationException(PaymentErrors.Microsoft_Dynamics_Commerce_Runtime_UnableToAuthorizePayment, "Either one of PaymentCard or TokenizedPaymentCard must be set on cartTenderLine.");
               }

               //AuthorizeTokenizedCardPaymentServiceRequest authorizeRequest = new AuthorizeTokenizedCardPaymentServiceRequest(
               //    tenderLine,
               //    tokenizedPaymentCard,
               //    tokenResponsePaymentProperties);

               AuthorizeTokenizedCardPaymentServiceRequest authorizeRequest = new AuthorizeTokenizedCardPaymentServiceRequest(
       tenderLine,
       tokenizedPaymentCard);

               IRequestHandler cardPaymentHandler = context.Runtime.GetRequestHandler(authorizeRequest.GetType(), (int)RetailOperation.PayCard);
               AuthorizePaymentServiceResponse authorizeResponse = context.Execute<AuthorizePaymentServiceResponse>(authorizeRequest);



               //AuthorizePaymentServiceResponse authorizeResponse = AuthorizeTokenizedCardPayment(authorizeRequest);

               tenderLine = authorizeResponse.TenderLine;

               return tenderLine;
           }



           /// <summary>
           /// Cancels the card authorized payments.
           /// </summary>
           /// <param name="context">The request context.</param>
           /// <param name="tenderLines">The tender lines containing authorization responses.</param>
           /// <param name="cartTenderLines">The cart tender lines containing authorization.</param>
           public static void CancelPayments(RequestContext context, IEnumerable<TenderLine> tenderLines, IEnumerable<CartTenderLine> cartTenderLines)
           {
               if (tenderLines == null || !tenderLines.Any())
               {
                   // Nothing to do as there are no authorization responses.
                   return;
               }

               foreach (TenderLine tenderLine in tenderLines)
               {
                   // For each authorization response, cancel/void the payment.
                   if (tenderLine.Status == TenderLineStatus.Committed || tenderLine.Status == TenderLineStatus.PendingCommit)
                   {
                       try
                       {
                           ReadOnlyCollection<TenderType> channelTenderTypes =
                                           new ChannelDataManager(context).GetChannelTenderTypes(context.GetPrincipal().ChannelId, new QueryResultSettings(PagingInfo.AllRecords));

                           TenderType tenderType = channelTenderTypes.SingleOrDefault(channelTenderType => string.Equals(channelTenderType.TenderTypeId, tenderLine.TenderTypeId, StringComparison.OrdinalIgnoreCase));
                           if (tenderType.OperationId == (int)RetailOperation.PayCard)
                           {
                               VoidCardAuthorization(context, tenderLine);
                           }
                           else
                           {
                               CartTenderLine refundCartTenderLine = cartTenderLines.SingleOrDefault(t => t.TenderLineId == tenderLine.TenderLineId);

                               if (refundCartTenderLine == null)
                               {
                                   var message = string.Format(CultureInfo.InvariantCulture, "Voiding payment failed due to not being able find original tender line that represents credit card authorization. Line id: {0}.", tenderLine.TenderLineId);
                                   throw new PaymentException(PaymentErrors.Microsoft_Dynamics_Commerce_Runtime_UnableToCancelPayment, message);
                               }

                               RefundPayment(context, refundCartTenderLine);
                           }
                       }
                       catch (PaymentException)
                       {
                           throw;
                       }
                       catch (Exception ex)
                       {
                           var message = string.Format(CultureInfo.InvariantCulture, "Payment cancellation failed for tender line ({0}) with tender type id ({1}).", tenderLine.TenderLineId, tenderLine.TenderTypeId);
                           throw new PaymentException(PaymentErrors.Microsoft_Dynamics_Commerce_Runtime_UnableToCancelPayment, ex, message);
                       }
                   }
               }
           }

           /// <summary>
           /// Cancels the authorized payments for only authorized card type.
           /// </summary>
           /// <param name="context">The request context.</param>
           /// <param name="tenderLine">The tender lines containing authorization responses.</param>

           public static void VoidCardAuthorization(RequestContext context, TenderLine tenderLine)
           {
               try
               {
                   VoidPaymentServiceRequest voidRequest = new VoidPaymentServiceRequest(tenderLine);
                   IRequestHandler cardPaymentHandler = context.Runtime.GetRequestHandler(voidRequest.GetType(), (int)RetailOperation.PayCard);
                   context.Execute<VoidPaymentServiceResponse>(voidRequest, cardPaymentHandler);
               }
               catch (PaymentException)
               {
                   throw;
               }
               catch (Exception ex)
               {
                   var message = string.Format(CultureInfo.InvariantCulture, "Payment cancellation failed for tender line ({0}) with tender type id ({1}).", tenderLine.TenderLineId, tenderLine.TenderTypeId);
                   throw new PaymentException(PaymentErrors.Microsoft_Dynamics_Commerce_Runtime_UnableToCancelPayment, ex, message);
               }
           }

           /// <summary>
           /// Refunds payment for captured card type.
           /// </summary>
           /// <param name="context">The request context.</param>
           /// <param name="refundTenderLine">The tender lines containing authorization responses.</param>
           /// <returns>The Tender line created after processing payment for return.</returns>
           public static TenderLine RefundPayment(RequestContext context, CartTenderLine refundTenderLine)
           {
               refundTenderLine.Amount = decimal.Negate(refundTenderLine.Amount);
               return AuthorizeAndCapturePayment(context, refundTenderLine, skipLimitValidation: true);
           }

           #endregion


       }*/

    #endregion

    /// <summary>
    /// PaymentController defines properties and methods for API controller for payment.
    /// </summary>
    [RoutePrefix("api/v1")]
    public class PaymentController : ApiBaseController
    {
        /// <summary>
        /// Payment Controller 
        /// </summary>
        public PaymentController()
        {
            ControllerName = "PaymentController";
        }

        public static readonly string PaymentManagerService = "PaymentManager";

        #region API Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clSynchronizeServiceAccountIdRequest"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("Payment/SynchronizeServiceAccountIdsWithCL")]
        public CLSynchronizeServiceAccountIdResponse SynchronizeServiceAccountIdsWithCL([FromBody] CLSynchronizeServiceAccountIdRequest clSynchronizeServiceAccountIdRequest)
        {
            CLSynchronizeServiceAccountIdResponse response;
            var relevantStoresList = new List<StoreDto>();
            List<PaymentConnector> clDbPymentConnectorList = new List<PaymentConnector>();
            string currentStoreId = "";

            try
            {

                response = this.ValidateCLSynchronizeServiceAccountIdRequest(clSynchronizeServiceAccountIdRequest, out relevantStoresList);

                if (response != null)
                {
                    return response;
                }

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "CLSynchronizeServiceAccountIdRequest: {0}", JsonConvert.SerializeObject(clSynchronizeServiceAccountIdRequest));

                PaymentConnectorDAL paymentConnectorDAL = new PaymentConnectorDAL(currentStore.StoreKey);
                clDbPymentConnectorList = paymentConnectorDAL.GetAllPaymentConnectors();

                foreach (StoreDto store in relevantStoresList)
                {
                    currentStore = store;
                    currentStoreId = store.StoreId.ToString();

                    var paymentController = erpAdapterFactory.CreatePaymentController(currentStore.StoreKey);

                    var paymentConnectors = paymentController.GetPaymentConnectorInfo(GetRequestGUID(Request));

                    foreach (var paymentConnector in paymentConnectors)
                    {
                        PaymentProperty[] paymentProperties = PaymentProperty.ConvertXMLToPropertyArray(paymentConnector.ConnectorProperties);
                        Hashtable paymentHash = PaymentProperty.ConvertToHashtable(paymentProperties);

                        string serviceAccountId = string.Empty;
                        var isServiceAccountId = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.ServiceAccountId.ToString(), out serviceAccountId);

                        PaymentMethodDAL paymentMethodDAL = new PaymentMethodDAL(currentStore.StoreKey);

                        foreach (PaymentConnector clDbPaymentConnector in clDbPymentConnectorList)
                        {
                            if (paymentConnector.Name == clDbPaymentConnector.PaymentConnectorName)
                            {
                                if (isServiceAccountId)
                                {
                                    paymentMethodDAL.UpdateServiceAccountIdForPaymentMethod(currentStore.StoreId, serviceAccountId, clDbPaymentConnector.PaymentConnectorId);
                                }
                            }
                        }
                    }
                }

                response = new CLSynchronizeServiceAccountIdResponse(true, "Service account ids updated successfully.");
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                response = new CLSynchronizeServiceAccountIdResponse(false, "Current Store Id = " + currentStoreId + " " + message);
            }
            finally
            {
            }

            return response;
        }

        /// <summary>
        /// ProcessPayment processes payment.
        /// </summary>
        /// <param name="processPaymentRequest"></param>
        /// <returns>ProcessPaymentResponse</returns>
        [HttpPost]
        [Route("Payment/ProcessPayment")]
        public ProcessPaymentResponse ProcessPayment([FromBody] ProcessPaymentRequest processPaymentRequest)
        {
            return null;
        }

        /// <summary>
        /// ProcessPendingOrderPayments processes pending order payment.
        /// </summary>
        /// <param name="processPendingOrderPaymentRequest"></param>
        /// <returns>ProcessPendingOrderPaymentResponse</returns>
        [HttpPost]
        [Route("Payment/ProcessPendingOrderPayments")]
        public ProcessPendingOrderPaymentResponse ProcessPendingOrderPayments([FromBody] ProcessPendingOrderPaymentRequest processPendingOrderPaymentRequest)
        {
            return null;
        }


        /// <summary>
        /// CancelPayments Cancels the card authorized payments.
        /// </summary>
        /// <param name="cancelPaymentRequest">The request context.</param>
        /// <returns>CancelPaymentResponse</returns>
        [HttpGet]
        [Route("Payment/CancelPayments")]
        public CancelPaymentResponse CancelPayments([FromBody] CancelPaymentRequest cancelPaymentRequest)
        {
            return null;
        }

        /// <summary>
        /// VoidCardAuthorization Cancels the authorized payments for only authorized card type.
        /// </summary>
        /// <param name="voidCardAuthorizationRequest">The request context.</param>
        /// <returns>CardAuthorizationResponse</returns>
        [HttpGet]
        [Route("Payment/VoidCardAuthorization")]
        public VoidCardAuthorizationResponse VoidCardAuthorization([FromBody] VoidCardAuthorizationRequest voidCardAuthorizationRequest)
        {
            return null;
        }

        /// <summary>
        /// RefundPayment refunds payment for captured card type.
        /// </summary>
        /// <param name="refundPaymentRequest">The request context.</param>
        /// <returns>RefundPaymentResponse</returns>
        [HttpGet]
        [Route("Payment/RefundPayment")]
        public RefundPaymentResponse RefundPayment([FromBody] RefundPaymentRequest refundPaymentRequest)
        {
            return null;
        }

        #endregion

        #region private methods
        private CLSynchronizeServiceAccountIdResponse ValidateCLSynchronizeServiceAccountIdRequest(CLSynchronizeServiceAccountIdRequest clSynchronizeServiceAccountIdRequest, out List<StoreDto> relevantStoresList)
        {
            relevantStoresList = new List<StoreDto>();

            if (clSynchronizeServiceAccountIdRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CLSynchronizeServiceAccountIdResponse(false, message);
            }

            if (clSynchronizeServiceAccountIdRequest.SynchronizeAll == false)
            {
                if (clSynchronizeServiceAccountIdRequest.StoreIds == null || clSynchronizeServiceAccountIdRequest.StoreIds.Count == 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "CLSynchronizeServiceAccountIdRequest.StoreIds");
                    return new CLSynchronizeServiceAccountIdResponse(false, message);
                }

                StoreDAL storeDAL = new StoreDAL(currentStore.StoreKey, 0);
                var storeList = StoreService.GetAllStores();

                for (int i = 0; i < clSynchronizeServiceAccountIdRequest.StoreIds.Count; i++)
                {
                    bool bStoreIdFound = false;

                    foreach (var store in storeList)
                    {
                        if (store.StoreId == clSynchronizeServiceAccountIdRequest.StoreIds[i])
                        {
                            bStoreIdFound = true;
                            relevantStoresList.Add(store);
                        }
                    }

                    if (bStoreIdFound == false)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40007, currentStore, clSynchronizeServiceAccountIdRequest.StoreIds[i]);
                        return new CLSynchronizeServiceAccountIdResponse(false, message);
                    }
                }
            }
            else
            {
                if (clSynchronizeServiceAccountIdRequest.StoreIds != null && clSynchronizeServiceAccountIdRequest.StoreIds.Count > 0)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40008, currentStore);
                    return new CLSynchronizeServiceAccountIdResponse(false, message);
                }

                relevantStoresList = StoreService.GetAllStores();
            }

            return null;
        }
        #endregion

        #region Payment request, response classes

        public class TokenizeCardRequest
        {
            public ErpPaymentCard erpPaymentCard { get; set; }
        }

        public class ProcessPaymentRequest
        {

        }

        public class ProcessPendingOrderPaymentRequest
        {

        }

        public class CancelPaymentRequest
        {

        }

        public class VoidCardAuthorizationRequest
        {

        }

        public class RefundPaymentRequest
        {

        }

        public class ProcessPaymentResponse
        {
            public ProcessPaymentResponse(string status, ErpPaymentCard card, string message)
            {
                this.status = status;
                this.message = message;
                this.ErpPaymentCard = card;
            }

            /// <summary>
            /// status of sales order trans
            /// </summary>
            public string status { get; set; }

            public ErpPaymentCard ErpPaymentCard { get; set; }

            /// <summary>
            /// message of sales order trans
            /// </summary>
            public string message { get; set; }

        }

        public class ProcessPendingOrderPaymentResponse
        {

        }

        public class CancelPaymentResponse
        {

        }

        public class VoidCardAuthorizationResponse
        {

        }

        public class RefundPaymentResponse
        {

        }

        #endregion

    }

}
