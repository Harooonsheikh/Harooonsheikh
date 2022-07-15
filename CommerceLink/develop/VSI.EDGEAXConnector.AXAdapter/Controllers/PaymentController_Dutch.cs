using Microsoft.Dynamics.Commerce.Runtime.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EdgeCommerceConnector.adptAX2012R3;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using Microsoft.Dynamics.Commerce.Runtime.Services.Messages;
using Microsoft.Dynamics.Commerce.Runtime;
using Microsoft.Dynamics.Retail.PaymentSDK.Portable.Constants;
using Microsoft.Dynamics.Retail.PaymentSDK.Portable;
using System.Collections.ObjectModel;
using Microsoft.Dynamics.Commerce.Runtime.DataServices.Messages;
using Microsoft.Dynamics.Commerce.Runtime.Data;
using Microsoft.Dynamics.Retail.SDKManager.Portable;
using System.Globalization;
using Microsoft.Dynamics.Commerce.Runtime.Workflow;
using Newtonsoft.Json;
using VSI.EDGEAXConnector.AXCommon;
using VSI.EDGEAXConnector.Logging;
using Microsoft.Dynamics.Commerce.Runtime.Services;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class PaymentController_Dutch : BaseController, IPaymentController
    {
           
	    
          //Payment Connector Code
        private const string CardNumberMask = "xxxxxxxxxxxx";
        private static string locale;
        private static IEnumerable<IPaymentProcessor> Connectors { get; set; }
        public static readonly string PaymentManagerService = "PaymentManager";

        DateTime StartTime = DateTime.MinValue;

        List<TenderLine> tenderLines = new List<TenderLine>();

        /// <summary>
        /// it will call using web service call
        /// </summary>
        /// <param name="CardTenderLine"></param>
        public void ProcessPayment(List<CartTenderLine> CardTenderLine)
        {
            try
            {

                StartTime = DateTime.Now;
                StringBuilder traceInfo = new StringBuilder();
                traceInfo.Append(string.Format("Controller=>ProcessPayment () Completed at [{0}] with token [{1}] ", StartTime, tenderLines.ToString()) + Environment.NewLine);

            //    tenderLines = Microsoft.Dynamics.Commerce.Runtime.Workflow.OrderWorkflowHelper.ProcessPendingOrderPayments(CommerceRuntimeHelper.RequestContext, CardTenderLine);

                traceInfo.Append(string.Format("Controller=>ProcessPayment () Completed at [{0}] with token [{1}] ", StartTime, tenderLines.ToString()) + Environment.NewLine);

                CustomLogger.LogTraceInfo(traceInfo.ToString());

            }
            catch (Exception ex)
            {

                CustomLogger.LogException("ProcessPayment Exception" + Environment.NewLine + Common.CommonUtility.GetExceptionInfo(ex));
                throw;
            }
            

        }


        /// <summary>
        /// it will call from Program.cs (Manual call)
        /// </summary>

        public void ProcessPayment()
        {
            // Above object will be generated automatically in case of paymentCard & in case of tokenized Payment card it will be generated externally.
            // Find the first connector that supports the currency and cardtype.that connector will be use for processing also service id will be extracted from this connector
            List<CartTenderLine> CardTenderLineList = new List<CartTenderLine>();

            PaymentCard payCard = new PaymentCard();
            payCard.CardNumber = "4351670872651130";
            payCard.CardTypes = "VISA";
            payCard.CCID = "123";
            payCard.City = "New York";
            payCard.Address1 = "4 Cedarbrook Drive,New York, New York, 10009, US";
            payCard.Address2 = "4 Cedarbrook Drive,New York, New York, 10009, US";
            payCard.Country = "US";
            payCard.ExpirationMonth = 1;
            payCard.ExpirationYear = 2019;
            payCard.NameOnCard = "systems30";
            payCard.Phone = "123456789";
            payCard.State = "New York";
            payCard.Zip = "10009";
            payCard.IsSwipe = false;


            //payCard.UseShippingAddress = true; // bool optional
            // payCard.VoiceAuthorizationCode = //string optional
            //payCard.EncryptedPin = String.Empty; string optional
            //payCard.EntityName = // string optional


            CardTokenInfo cti = new CardTokenInfo();

            cti.CardToken = "NDM1MTY3MDg3MjY1MTEzMA==";
            cti.MaskedCardNumber = "xxxxxxxxxxxx1130";
            cti.ServiceAccountId = "35e907fe-3256-42ec-ac9b-4789d220f6ff";
            cti.UniqueCardId = "7debe00d-7b0c-487e-9d1f-afcc08230277";



            TokenizedPaymentCard tpc = new TokenizedPaymentCard();

            tpc.CardTokenInfo = cti;
            tpc.CardTypes = "VISA";
            tpc.Country = "USA";
            tpc.City = "New York";
            tpc.Address1 = "4 Cedarbrook Drive,New York, New York, 10009, US";
            tpc.Address2 = "4 Cedarbrook Drive,New York, New York, 10009, US";
            tpc.Country = "USA";
            tpc.ExpirationMonth = 1;
            tpc.ExpirationYear = 2019;
            tpc.NameOnCard = "systems30";
            tpc.Phone = "123456789";
            tpc.State = "NY";
            tpc.Zip = "10009";
            tpc.NameOnCard = "systems30";
            tpc.IsSwipe = false;

         

            CartTenderLine ctl = new CartTenderLine();
           // ctl.PaymentCard = payCard;
            ctl.TenderTypeId = "2";
            ctl.TokenizedPaymentCard = tpc;
            ctl.Amount = Convert.ToDecimal(204.92);
            ctl.Currency = "USD";

            CardTenderLineList.Add(ctl);
           
            // start of zubair code
          //  var context = VSI.EDGEAXConnector.AXCommon.CommerceRuntimeHelper.RequestContext;

          //  TenderLine tenderLine = new TenderLine { TenderLineId = ctl.TenderLineId };
          //  tenderLine.CopyPropertiesFrom(ctl);

          //  GenerateCardTokenPaymentServiceRequest tokenRequest = new GenerateCardTokenPaymentServiceRequest(tenderLine, payCard);
          //  GenerateCardTokenPaymentServiceResponse tokenResponse = context.Execute<GenerateCardTokenPaymentServiceResponse>(tokenRequest);
          //  tenderLine = tokenResponse.TenderLine;

          //  TokenizedPaymentCard tokenizedPaymentCard;
          //  string tokenResponsePaymentProperties = string.Empty;
          //  tokenizedPaymentCard = tokenResponse.TokenizedPaymentCard;
          //  tokenResponsePaymentProperties = tokenResponse.TokenResponsePaymentProperties;

          //  AuthorizeTokenizedCardPaymentServiceRequest authorizeRequest = new AuthorizeTokenizedCardPaymentServiceRequest(
          //   tenderLine,
          //   tokenizedPaymentCard,
          //   tokenResponsePaymentProperties);  

          ////  ThrowIf.Null(cartTenderLine, "cartTenderLine");
          //  TenderLine tl = new TenderLine { TenderLineId = ctl.TenderLineId };
          //  tenderLine.CopyPropertiesFrom(ctl);
          // // var authorizedTenderLine = AuthorizePayment(context, tenderLine, ctl.PaymentCard, false);
          // // var temp = CapturePayment(context, authorizedTenderLine);
       

          //  IRequestHandler cardPaymentHandler = context.Runtime.GetRequestHandler(authorizeRequest.GetType(), (int)RetailOperation.PayCard);
          //  AuthorizePaymentServiceResponse authorizeResponse = context.Execute<AuthorizePaymentServiceResponse>(authorizeRequest, cardPaymentHandler);

          //  tenderLine = authorizeResponse.TenderLine;
          //  TenderLine tl2 =AuthorizePayment(context, tenderLine, payCard,false);
      
          //  var temp = CapturePayment(context, tl2, payCard);

            //tenderLine = authorizeResponse.TenderLine;
           //end if zubair code

            tenderLines = ProcessPendingOrderPayments(CommerceRuntimeHelper.RequestContext, CardTenderLineList); 

          //  tenderLines = Microsoft.Dynamics.Commerce.Runtime.Workflow.OrderWorkflowHelper.ProcessPendingOrderPayments(CommerceRuntimeHelper.RequestContext, CardTenderLineList);


        }
       

        //private static TenderLine AuthorizePayment(RequestContext context, TenderLine tenderLine, PaymentCard paymentCard, bool skipLimitValidation)
        //{
        //    AuthorizePaymentServiceRequest authorizeRequest = new AuthorizePaymentServiceRequest(tenderLine, paymentCard, skipLimitValidation);

        //    IRequestHandler paymentManagerHandler = context.Runtime.GetRequestHandler(authorizeRequest.GetType(), ServiceTypes.PaymentManagerService);

        //    var authorizeResponse = context.Execute<AuthorizePaymentServiceResponse>(authorizeRequest);
        //    if (authorizeResponse.TenderLine == null)
        //    {
        //        throw new PaymentException(PaymentErrors.UnableToAuthorizePayment, "Payment service did not return tender line.");
        //    }

        //    return authorizeResponse.TenderLine;
        //}

        public TenderLine CapturePayment(RequestContext context, TenderLine tenderLine,PaymentCard pay)
        {
            switch (tenderLine.Status)
            {
                case TenderLineStatus.PendingCommit:
                    {
                        CapturePaymentServiceRequest captureRequest = new CapturePaymentServiceRequest(tenderLine,pay);
                        IRequestHandler paymentManagerHandler = context.Runtime.GetRequestHandler(captureRequest.GetType(), ServiceTypes.PaymentManagerService);
                        CapturePaymentServiceResponse captureResponse = context.Execute<CapturePaymentServiceResponse>(captureRequest, paymentManagerHandler);
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

        #region "Payment Connector Code"

        public static List<TenderLine> ProcessPendingOrderPayments(RequestContext context, IEnumerable<CartTenderLine> cartTenderLines)
        {
            ThrowIf.Null(context, "context");
            //ThrowIf.Null(context.GetSalesTransaction(), "context.GetSalesTransaction()");
            ThrowIf.Null(cartTenderLines, "cartTenderLines");

            // Assign id to each cart tender lines
            foreach (CartTenderLine cartTenderLine in cartTenderLines)
            {
                cartTenderLine.TenderLineId = Guid.NewGuid().ToString();
            }

            decimal totalTenderedAmount = GetPaymentsSum(cartTenderLines);

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
                    new ChannelDataManager(context).GetChannelTenderTypes(context.GetPrincipal().ChannelId, new QueryResultSettings());
                    TenderType type = channelTenderTypes.SingleOrDefault(channelTenderType => string.Equals(channelTenderType.TenderTypeId, cartTenderLine.TenderTypeId, StringComparison.OrdinalIgnoreCase));
                    TenderLine tenderLine = new TenderLine();
                    if (type.OperationId == (int)RetailOperation.PayCard)
                    {
                        tenderLine = GenerateCardTokenAndGetAuthorization(context, cartTenderLine);
                    }
                   // else
                   // {
                        tenderLine = AuthorizeAndCapturePayment(context, cartTenderLine, skipLimitValidation: false);
                   // }

                    tenderLines.Add(tenderLine);
                }
            }
            catch (Exception ex)
            {
                try
                {
                    // Cancel the payment authorizations
                    if (tenderLines.Any())
                    {
                        CancelPayments(context, tenderLines, cartTenderLines);
                    }
                }
                catch (Exception cancelPaymentsEx)
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


                //GenerateCardTokenPaymentServiceResponse tokenResponse = context.Execute<GenerateCardTokenPaymentServiceResponse>(tokenRequest);


                GenerateCardTokenPaymentServiceResponse tokenResponse = GenerateCardToken(tokenRequest);

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
                throw new DataValidationException(PaymentErrors.UnableToAuthorizePayment, "Either one of PaymentCard or TokenizedPaymentCard must be set on cartTenderLine.");
            }

            AuthorizeTokenizedCardPaymentServiceRequest authorizeRequest = new AuthorizeTokenizedCardPaymentServiceRequest(
                tenderLine,
                tokenizedPaymentCard,
                tokenResponsePaymentProperties);

            IRequestHandler cardPaymentHandler = context.Runtime.GetRequestHandler(authorizeRequest.GetType(), (int)RetailOperation.PayCard);
            //AuthorizePaymentServiceResponse authorizeResponse = context.Execute<AuthorizePaymentServiceResponse>(authorizeRequest);



            AuthorizePaymentServiceResponse authorizeResponse = AuthorizeTokenizedCardPayment(authorizeRequest);

            tenderLine = authorizeResponse.TenderLine;

            return tenderLine;
        }

        /// <summary>
        /// Authorizes and captures the payment.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <param name="cartTenderLine">The authorization tender line.</param>
        /// <param name="skipLimitValidation">If set to 'true' limits validation (over tender, under tender etc.) will be skipped.</param>
        /// <returns>The tender line created after payment processing.</returns>
        private static TenderLine AuthorizeAndCapturePayment(RequestContext context, CartTenderLine cartTenderLine, bool skipLimitValidation)
        {
            ThrowIf.Null(cartTenderLine, "cartTenderLine");
            TenderLine tenderLine = new TenderLine { TenderLineId = cartTenderLine.TenderLineId };
            tenderLine.CopyPropertiesFrom(cartTenderLine);
            var authorizedTenderLine = AuthorizePayment(context, tenderLine, cartTenderLine.PaymentCard, skipLimitValidation);
            return CapturePayments(context, authorizedTenderLine, cartTenderLine.PaymentCard);
        }

        #region "Request,Reponse Functions"

        #region GenerateCardToken

        private static GenerateCardTokenPaymentServiceResponse GenerateCardToken(GenerateCardTokenPaymentServiceRequest request)
        {
            //NetTracer.Information("Calling Payment.GenerateCardToken");

            if (request.TokenizedPaymentCard != null)
            {
                request.TenderLine.Authorization = request.TokenizedPaymentCard.CardTokenInfo.CardToken;
                TenderLine localEncryptedTenderLine = ReEncryptPaymentAuthorizationProperties(request.RequestContext, request.TenderLine, request.Platform);
                request.TokenizedPaymentCard.CardTokenInfo.CardToken = localEncryptedTenderLine.Authorization;
                return new GenerateCardTokenPaymentServiceResponse(localEncryptedTenderLine, request.TokenizedPaymentCard, localEncryptedTenderLine.Authorization);
            }

            //Request paymentRequest = new Request { Locale = locale ?? GetLocale(request.RequestContext) };   

            Request paymentRequest = new Request { Locale = locale ?? GetLocale(CommerceRuntimeHelper.RequestContext) };

            string cardTypeId = request.TenderLine.CardTypeId ?? request.PaymentCard.CardTypes;
            CardTypeInfo cardTypeConfiguration = GetCardTypeConfiguration(CommerceRuntimeHelper.RequestContext.GetPrincipal().ChannelId, cardTypeId, CommerceRuntimeHelper.RequestContext);

            IList<PaymentProperty> paymentProperties = GetPaymentPropertiesForAuthorizationAndRefund(request.TenderLine, request.PaymentCard, cardTypeConfiguration, isCardTokenRequired: true, allowPartialAuthorization: false); // Since this is a tokenization request, the value of AllowPartialAuthorization flag does not matter. 
            if (paymentProperties == null || !paymentProperties.Any())
            {
                throw new DataValidationException(DataValidationErrors.RequiredValueNotFound, "Invalid request.");
            }

            string connectorName;
            IEnumerable<PaymentProperty> connectorProperties = GetConnectorProperties(CommerceRuntimeHelper.RequestContext, paymentProperties, out connectorName);

            paymentProperties.AddRange(connectorProperties);
            paymentRequest.Properties = paymentProperties.ToArray();

            // Get payment processor
            IPaymentProcessor processor = GetPaymentProcessor(connectorName);

            // Run GenerateCardToken
            Response response = processor.GenerateCardToken(paymentRequest, null);
            VerifyResponseErrors("GenerateCardToken", response, PaymentErrors.UnableToGenerateToken);

            CardTokenInfo cardTokenInfo = new CardTokenInfo();

            Microsoft.Dynamics.Retail.PaymentSDK.Portable.Hashtable hashTable = PaymentProperty.ConvertToHashtable(response.Properties);

            PaymentProperty property = PaymentProperty.GetPropertyFromHashtable(hashTable, GenericNamespace.PaymentCard, PaymentCardProperties.CardToken);
            cardTokenInfo.CardToken = (property != null) ? property.StringValue : string.Empty;

            property = PaymentProperty.GetPropertyFromHashtable(hashTable, GenericNamespace.PaymentCard, PaymentCardProperties.UniqueCardId);
            cardTokenInfo.UniqueCardId = (property != null) ? property.StringValue : string.Empty;

            property = PaymentProperty.GetPropertyFromHashtable(hashTable, GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId);
            cardTokenInfo.ServiceAccountId = (property != null) ? property.StringValue : string.Empty;

            property = PaymentProperty.GetPropertyFromHashtable(hashTable, GenericNamespace.PaymentCard, PaymentCardProperties.Last4Digits);
            cardTokenInfo.MaskedCardNumber = (property != null) ? (CardNumberMask + property.StringValue) : string.Empty;

            string reponsePropertiesBlob = PaymentProperty.ConvertPropertyArrayToXML(response.Properties);

            TokenizedPaymentCard tokenizedPaymentCard = GetTokenizedPaymentCard(request.PaymentCard, cardTokenInfo);

            //NetTracer.Information("Completed Payment.GenerateCardToken");
            return new GenerateCardTokenPaymentServiceResponse(GetTenderLineForGeneratedCardToken(request, hashTable), tokenizedPaymentCard, reponsePropertiesBlob);
        }
        private static List<PaymentProperty> GetPaymentPropertiesForAuthorizationAndRefund(TenderLine tenderLine, PaymentCard paymentCard, CardTypeInfo cardTypeConfiguration, bool isCardTokenRequired, bool allowPartialAuthorization)
        {
            if (tenderLine == null)
            {
                throw new ArgumentNullException("tenderLine");
            }

            if (paymentCard == null)
            {
                throw new ArgumentNullException("tenderLine", "Payment card info cannot be null inside tender line.");
            }

            List<PaymentProperty> paymentProperties = new List<PaymentProperty>();

            // Industry type.
            PaymentProperty paymentProperty = new PaymentProperty(
                GenericNamespace.TransactionData,
                TransactionDataProperties.IndustryType,
                IndustryType.Retail.ToString(),
                SecurityLevel.None);
            paymentProperties.Add(paymentProperty);

            // Amount.
            paymentProperty = new PaymentProperty(
                GenericNamespace.TransactionData,
                TransactionDataProperties.Amount,
                Math.Abs(tenderLine.Amount), // for refunds request amount must be positive
                SecurityLevel.None);
            paymentProperties.Add(paymentProperty);

            // Currency code.
            paymentProperty = new PaymentProperty(
                GenericNamespace.TransactionData,
                TransactionDataProperties.CurrencyCode,
                tenderLine.Currency,
                SecurityLevel.None);
            paymentProperties.Add(paymentProperty);

            // Is cart tokenization supported?
            paymentProperty = new PaymentProperty(
                GenericNamespace.TransactionData,
                TransactionDataProperties.SupportCardTokenization,
                isCardTokenRequired.ToString(),
                SecurityLevel.None);
            paymentProperties.Add(paymentProperty);

            // Is partial authorization allowed?
            paymentProperty = new PaymentProperty(
                GenericNamespace.TransactionData,
                TransactionDataProperties.AllowPartialAuthorization,
                allowPartialAuthorization.ToString(),
                SecurityLevel.None);
            paymentProperties.Add(paymentProperty);

            // Card number.
            paymentProperty = new PaymentProperty(
                GenericNamespace.PaymentCard,
                PaymentCardProperties.CardNumber,
                paymentCard.CardNumber,
                SecurityLevel.None);
            paymentProperties.Add(paymentProperty);

            // Card type.
            Microsoft.Dynamics.Retail.PaymentSDK.Portable.CardType paymentSdkCardType = GetPaymentSdkCardType(cardTypeConfiguration, tenderLine.CardTypeId ?? paymentCard.CardTypes);
            paymentProperty = new PaymentProperty(
                GenericNamespace.PaymentCard,
                PaymentCardProperties.CardType,
                paymentSdkCardType.ToString(),
                SecurityLevel.None);
            paymentProperties.Add(paymentProperty);

            // Voice authorization code.
            if (!string.IsNullOrWhiteSpace(paymentCard.VoiceAuthorizationCode))
            {
                paymentProperty = new PaymentProperty(
                    GenericNamespace.PaymentCard,
                    PaymentCardProperties.VoiceAuthorizationCode,
                    paymentCard.VoiceAuthorizationCode,
                    SecurityLevel.None);
                paymentProperties.Add(paymentProperty);
            }

            //if (paymentSdkCardType == PaymentSDK.CardType.Debit)
            //{
            //    if (tenderLine.CashBackAmount > 0)
            //    {
            //        // Cashback amount.
            //        paymentProperty = new PaymentProperty(
            //            GenericNamespace.PaymentCard,
            //            PaymentCardProperties.CashBackAmount,
            //            tenderLine.CashBackAmount,
            //            SecurityLevel.None);
            //        paymentProperties.Add(paymentProperty);
            //    }

            //    // Encrypted pin.
            //    paymentProperty = new PaymentProperty(
            //        GenericNamespace.PaymentCard,
            //        PaymentCardProperties.EncryptedPin,
            //        paymentCard.EncryptedPin,
            //        SecurityLevel.PCI);
            //    paymentProperties.Add(paymentProperty);

            //    // Additional security data.
            //    paymentProperty = new PaymentProperty(
            //        GenericNamespace.PaymentCard,
            //        PaymentCardProperties.AdditionalSecurityData,
            //        paymentCard.AdditionalSecurityData,
            //        SecurityLevel.PCI);
            //    paymentProperties.Add(paymentProperty);
            //}

            if (!paymentCard.IsSwipe)
            {
                // Card verification value.
                paymentProperty = new PaymentProperty(
                    GenericNamespace.PaymentCard,
                    PaymentCardProperties.CardVerificationValue,
                    paymentCard.CCID,
                    SecurityLevel.None);
                paymentProperties.Add(paymentProperty);

                // Card expiration year.
                paymentProperty = new PaymentProperty(
                    GenericNamespace.PaymentCard,
                    PaymentCardProperties.ExpirationYear,
                    paymentCard.ExpirationYear,
                    SecurityLevel.None);
                paymentProperties.Add(paymentProperty);

                // Card expiration month.
                paymentProperty = new PaymentProperty(
                    GenericNamespace.PaymentCard,
                    PaymentCardProperties.ExpirationMonth,
                    paymentCard.ExpirationMonth,
                    SecurityLevel.None);
                paymentProperties.Add(paymentProperty);

                // Card holder name.
                paymentProperty = new PaymentProperty(
                    GenericNamespace.PaymentCard,
                    PaymentCardProperties.Name,
                    paymentCard.NameOnCard,
                    SecurityLevel.None);
                paymentProperties.Add(paymentProperty);

                // Card street address.
                paymentProperty = new PaymentProperty(
                    GenericNamespace.PaymentCard,
                    PaymentCardProperties.StreetAddress,
                    paymentCard.Address1,
                    SecurityLevel.None);
                paymentProperties.Add(paymentProperty);

                // Card postal code.
                paymentProperty = new PaymentProperty(
                    GenericNamespace.PaymentCard,
                    PaymentCardProperties.PostalCode,
                    paymentCard.Zip,
                    SecurityLevel.None);
                paymentProperties.Add(paymentProperty);

                // Card country.
                paymentProperty = GetCountryProperty(paymentCard.Country);
                paymentProperties.Add(paymentProperty);
            }
            else
            {
                // Track1
                paymentProperty = new PaymentProperty(
                    GenericNamespace.PaymentCard,
                    PaymentCardProperties.Track1,
                    paymentCard.Track1,
                    SecurityLevel.None);
                paymentProperties.Add(paymentProperty);

                // Track2
                paymentProperty = new PaymentProperty(
                    GenericNamespace.PaymentCard,
                    PaymentCardProperties.Track2,
                    paymentCard.Track2,
                    SecurityLevel.None);
                paymentProperties.Add(paymentProperty);

                // Track3
                paymentProperty = new PaymentProperty(
                    GenericNamespace.PaymentCard,
                    PaymentCardProperties.Track3,
                    paymentCard.Track3,
                    SecurityLevel.None);
                paymentProperties.Add(paymentProperty);
            }

            return paymentProperties;
        }
        // chnaged to get specific service ID
        private static IEnumerable<PaymentProperty> GetConnectorProperties(RequestContext context, IEnumerable<PaymentProperty> paymentProperties, out string connectorName)
        {
            List<PaymentProperty> enhancedAuthorizationProperties = new List<PaymentProperty>();

            var paymentConnectorConfigs = GetMerchantConnectorInformation(context);
            if (paymentConnectorConfigs == null || paymentConnectorConfigs.Count == 0)
            {
                // NetTracer.Error("No payment connectors loaded.");
                throw new ConfigurationException(ConfigurationErrors.PaymentConnectorNotFound, "No payment connectors loaded.");
            }

            // Get currency for request
            string requestCurrency;
            Microsoft.Dynamics.Retail.PaymentSDK.Portable.Hashtable requestHashtable = PaymentProperty.ConvertToHashtable(paymentProperties.ToArray());
            if (PaymentProperty.GetPropertyValue(requestHashtable, GenericNamespace.TransactionData, TransactionDataProperties.CurrencyCode, out requestCurrency)
                && !string.IsNullOrWhiteSpace(requestCurrency))
            {
                requestCurrency = requestCurrency.ToUpper();
            }
            else
            {
                // NetTracer.Error("Invalid request for Payment.Authorize currency missing");
                throw new DataValidationException(DataValidationErrors.RequiredValueNotFound, "Invalid request for Payment.Authorize currency missing");
            }

            // Get cardtype from request
            string requestCardType;
            if (PaymentProperty.GetPropertyValue(requestHashtable, GenericNamespace.PaymentCard, PaymentCardProperties.CardType, out requestCardType)
                && !string.IsNullOrWhiteSpace(requestCardType))
            {
                requestCardType = requestCardType.ToUpper();
            }
            else
            {
                /// NetTracer.Error("Invalid request for Payment.Authorize CardType missing");
                throw new DataValidationException(DataValidationErrors.RequiredValueNotFound, "Invalid request for Payment.Authorize CardType missing");
            }

            // Find the first connector that supports the currency and cardtype
            ////////////////////////////////////////////////////
            connectorName = string.Empty;

            foreach (PaymentConnectorConfiguration paymentConnectorConfig in paymentConnectorConfigs)
            {
                PaymentProperty[] properties = PaymentProperty.ConvertXMLToPropertyArray(paymentConnectorConfig.ConnectorProperties);
                Microsoft.Dynamics.Retail.PaymentSDK.Portable.Hashtable merchantConnectorInformation = PaymentProperty.ConvertToHashtable(properties);

                string foundServiceId;

                PaymentProperty.GetPropertyValue(merchantConnectorInformation, GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, out foundServiceId);

                if (foundServiceId == "35e907fe-3256-42ec-ac9b-4789d220f6ff")
                {
                    connectorName = paymentConnectorConfig.Name;
                    enhancedAuthorizationProperties.AddRange(properties);
                    PaymentProperty testModeProperty = new PaymentProperty(
                        GenericNamespace.TransactionData,
                        TransactionDataProperties.IsTestMode,
                        paymentConnectorConfig.IsTestMode.ToString(),
                        SecurityLevel.None);

                    enhancedAuthorizationProperties.Add(testModeProperty);
                    break;
                }

                else
                {
                    //NetTracer.Warning("Connector {0} missing supported currencies property", paymentConnectorConfig.Name);
                }
            }

            if (string.IsNullOrEmpty(connectorName))
            {
                // NetTracer.Error("Connector to service request not found!");
                throw new DataValidationException(DataValidationErrors.RequiredValueNotFound, "Connector to service request not found!");
            }

            return enhancedAuthorizationProperties;
        }

        // fetch payment connector which first matches currency and card type
        //private static IEnumerable<PaymentProperty> GetConnectorProperties(RequestContext context, IEnumerable<PaymentProperty> paymentProperties, out string connectorName)
        //{
        //    List<PaymentProperty> enhancedAuthorizationProperties = new List<PaymentProperty>();

        //    var paymentConnectorConfigs = GetMerchantConnectorInformation(context);
        //    if (paymentConnectorConfigs == null || paymentConnectorConfigs.Count == 0)
        //    {
        //        // NetTracer.Error("No payment connectors loaded.");
        //        throw new ConfigurationException(ConfigurationErrors.PaymentConnectorNotFound, "No payment connectors loaded.");
        //    }

        //    // Get currency for request
        //    string requestCurrency;
        //    Microsoft.Dynamics.Retail.PaymentSDK.Portable.Hashtable requestHashtable = PaymentProperty.ConvertToHashtable(paymentProperties.ToArray());
        //    if (PaymentProperty.GetPropertyValue(requestHashtable, GenericNamespace.TransactionData, TransactionDataProperties.CurrencyCode, out requestCurrency)
        //        && !string.IsNullOrWhiteSpace(requestCurrency))
        //    {
        //        requestCurrency = requestCurrency.ToUpper();
        //    }
        //    else
        //    {
        //        // NetTracer.Error("Invalid request for Payment.Authorize currency missing");
        //        throw new DataValidationException(DataValidationErrors.RequiredValueNotFound, "Invalid request for Payment.Authorize currency missing");
        //    }

        //    // Get cardtype from request
        //    string requestCardType;
        //    if (PaymentProperty.GetPropertyValue(requestHashtable, GenericNamespace.PaymentCard, PaymentCardProperties.CardType, out requestCardType)
        //        && !string.IsNullOrWhiteSpace(requestCardType))
        //    {
        //        requestCardType = requestCardType.ToUpper();
        //    }
        //    else
        //    {
        //        // NetTracer.Error("Invalid request for Payment.Authorize CardType missing");
        //        throw new DataValidationException(DataValidationErrors.RequiredValueNotFound, "Invalid request for Payment.Authorize CardType missing");
        //    }

        //    // Find the first connector that supports the currency and cardtype
        //    connectorName = string.Empty;

        //    foreach (PaymentConnectorConfiguration paymentConnectorConfig in paymentConnectorConfigs)
        //    {
        //        PaymentProperty[] properties = PaymentProperty.ConvertXMLToPropertyArray(paymentConnectorConfig.ConnectorProperties);
        //        Microsoft.Dynamics.Retail.PaymentSDK.Portable.Hashtable merchantConnectorInformation = PaymentProperty.ConvertToHashtable(properties);

        //        string currencies;
        //        if (PaymentProperty.GetPropertyValue(merchantConnectorInformation, GenericNamespace.MerchantAccount, MerchantAccountProperties.SupportedCurrencies, out currencies)
        //            && !string.IsNullOrWhiteSpace(currencies))
        //        {
        //            if (currencies.ToUpper().Contains(requestCurrency))
        //            {
        //                string cardTypes;
        //                if (PaymentProperty.GetPropertyValue(merchantConnectorInformation, GenericNamespace.MerchantAccount, MerchantAccountProperties.SupportedTenderTypes, out cardTypes)
        //                    && !string.IsNullOrWhiteSpace(cardTypes))
        //                {
        //                    if (cardTypes.ToUpper().Contains(requestCardType))
        //                    {
        //                        connectorName = paymentConnectorConfig.Name;
        //                        enhancedAuthorizationProperties.AddRange(properties);
        //                        PaymentProperty testModeProperty = new PaymentProperty(
        //                            GenericNamespace.TransactionData,
        //                            TransactionDataProperties.IsTestMode,
        //                            paymentConnectorConfig.IsTestMode.ToString(),
        //                            SecurityLevel.None);

        //                        enhancedAuthorizationProperties.Add(testModeProperty);
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //NetTracer.Warning("Connector {0} missing supported currencies property", paymentConnectorConfig.Name);
        //        }
        //    }

        //    if (string.IsNullOrEmpty(connectorName))
        //    {
        //        //NetTracer.Error("Connector to service request not found!");
        //        throw new DataValidationException(DataValidationErrors.RequiredValueNotFound, "Connector to service request not found!");
        //    }

        //    return enhancedAuthorizationProperties;
        //}

        private static TokenizedPaymentCard GetTokenizedPaymentCard(PaymentCard paymentCard, CardTokenInfo cardTokenInfo)
        {
            TokenizedPaymentCard tokenizedPaymentCard = new TokenizedPaymentCard()
            {
                CardTokenInfo = cardTokenInfo,
                CardTypes = paymentCard.CardTypes,
                NameOnCard = paymentCard.NameOnCard,
                ExpirationYear = paymentCard.ExpirationYear,
                ExpirationMonth = paymentCard.ExpirationMonth,
                Address1 = paymentCard.Address1,
                Country = paymentCard.Country,
                Zip = paymentCard.Zip,
            };

            return tokenizedPaymentCard;
        }

        private static TenderLine GetTenderLineForGeneratedCardToken(GenerateCardTokenPaymentServiceRequest request, Microsoft.Dynamics.Retail.PaymentSDK.Portable.Hashtable hashTable)
        {
            TenderLine tenderLine = request.TenderLine;

            PaymentProperty property = PaymentProperty.GetPropertyFromHashtable(hashTable, GenericNamespace.PaymentCard, PaymentCardProperties.Last4Digits);
            tenderLine.MaskedCardNumber = (property != null) ? (CardNumberMask + property.StringValue) : string.Empty;

            // Hardware station needs to return the token blob for future use            
            tenderLine.Authorization = PaymentProperty.ConvertPropertyArrayToXML(PaymentProperty.ConvertHashToProperties(hashTable));

            return tenderLine;
        }

        private static TenderLine ReEncryptPaymentAuthorizationProperties(RequestContext context, TenderLine tenderline, string platformString)
        {
            // Convert tenderline authorizations to local payment SDK settings.
            GetCardPaymentPropertiesServiceRequest propertyRequest = new GetCardPaymentPropertiesServiceRequest(tenderline.Authorization, false, platformString);
            GetCardPaymentPropertiesServiceResponse propertyResponse = context.Execute<GetCardPaymentPropertiesServiceResponse>(propertyRequest);
            tenderline.Authorization = propertyResponse.PortablePaymentPropertyXmlBlob;
            return tenderline;
        }


        private static Microsoft.Dynamics.Retail.PaymentSDK.Portable.CardType GetPaymentSdkCardType(CardTypeInfo cardTypeConfiguration, string cardType)
        {
            if (string.IsNullOrWhiteSpace(cardType))
            {
                throw new ArgumentException("cardType argument cannot be epmty", "cardType");
            }

            //if (cardTypeConfiguration != null && cardTypeConfiguration.CardType == DataModel.CardType.InternationalDebitCard)
            //{
            //    return Microsoft.Dynamics.Retail.PaymentSDK.Portable.CardType.Debit;
            //}

            Microsoft.Dynamics.Retail.PaymentSDK.Portable.CardType paymentSdkType;
            if (!Enum.TryParse(cardType, ignoreCase: true, result: out paymentSdkType))
            {
                return Microsoft.Dynamics.Retail.PaymentSDK.Portable.CardType.Unknown;
            }

            return paymentSdkType;
        }

        #endregion

        #region "AuthorizedTokenizedCardPayment"

        private static IPaymentProcessor GetPaymentProcessor(string paymentConnectorName)
        {
            IPaymentProcessor processor = null;
            try
            {
                processor = PaymentProcessorManager.GetPaymentProcessor(paymentConnectorName);

                if (processor == null)
                {
                    var message = string.Format("The specified payment connector {0} could not be loaded.", paymentConnectorName);
                    ConfigurationException configurationException = new ConfigurationException(ConfigurationErrors.PaymentConnectorNotFound, message);
                    throw configurationException;
                }
            }
            catch (InvalidOperationException exception)
            {
                var message = string.Format("The specified payment connector {0} could not be loaded.", paymentConnectorName);
                ConfigurationException configurationException = new ConfigurationException(ConfigurationErrors.PaymentConnectorNotFound, message, exception);
                throw configurationException;
            }

            return processor;
        }

        private static AuthorizePaymentServiceResponse AuthorizeTokenizedCardPayment(AuthorizeTokenizedCardPaymentServiceRequest authorizationRequest)
        {
            // NetTracer.Information("Calling Payment.TokenenizedCardPaymentAuthorize");
            string cardTypeId = authorizationRequest.TenderLine.CardTypeId ?? authorizationRequest.TokenizedPaymentCard.CardTypes;
            CardTypeInfo typeConfiguration = GetCardTypeConfiguration(RequestContextExtensions.GetPrincipal(CommerceRuntimeHelper.RequestContext).ChannelId, cardTypeId, CommerceRuntimeHelper.RequestContext);
            if (!authorizationRequest.SkipLimitValidation)
                ValidateCardEntry((PaymentCardBase)authorizationRequest.TokenizedPaymentCard, typeConfiguration);
            ValidateTokenInfo(authorizationRequest.TokenizedPaymentCard.CardTokenInfo);
            PaymentProperty[] authorizationWithToken = GetPaymentPropertiesForAuthorizationWithToken(authorizationRequest.TenderLine, authorizationRequest.TokenizedPaymentCard, authorizationRequest.AllowPartialAuthorization);
            string connectorName;
            PaymentProperty[] paymentRequestProperties = AddConnectorPropertiesByServiceAccountId(CommerceRuntimeHelper.RequestContext, authorizationWithToken, out connectorName);
            PaymentProperty[] responseProperties = GetTokenResponseProperties(authorizationRequest.TokenResponsePaymentProperties, authorizationRequest.TokenizedPaymentCard);
            return AuthorizePayment(CommerceRuntimeHelper.RequestContext, authorizationRequest.TenderLine, connectorName, paymentRequestProperties, responseProperties);
        }

        private static void ValidateCardEntry(PaymentCardBase paymentCardBase, CardTypeInfo cardTypeInfo)
        {
            if (!cardTypeInfo.AllowManualInput && !paymentCardBase.IsSwipe)
            {
                throw new PaymentException(PaymentErrors.ManualCardNumberNotAllowed, "The card number for card type '{0}' cannot be manually entered.", cardTypeInfo.TypeId);
            }
        }
        private static void ValidateTokenInfo(CardTokenInfo cardTokenInfo)
        {
            string message = string.Empty;
            bool flag = false;
            if (string.IsNullOrEmpty(cardTokenInfo.ServiceAccountId))
            {
                message += "A non-empty payment service account identifier, that was used to create the credit card token, must be specified for payment processing to proceed.";
                flag = true;
            }
            if (string.IsNullOrEmpty(cardTokenInfo.CardToken))
            {
                message += "The CardToken field must not be empty.";
                flag = true;
            }
            if (string.IsNullOrEmpty(cardTokenInfo.UniqueCardId))
            {
                message += "The UniqueCardId field must not be empty.";
                flag = true;
            }
            if (string.IsNullOrEmpty(cardTokenInfo.MaskedCardNumber))
            {
                message += "The MaskedCardNumber field must not be empty.";
                flag = true;
            }
            if (flag)
                throw new DataValidationException(PaymentErrors.UnableToAuthorizePayment, message, new object[0]);
        }

        private static PaymentProperty[] GetPaymentPropertiesForAuthorizationWithToken(TenderLine tenderLine, TokenizedPaymentCard tokenizedPaymentCard, bool allowPartialAuthorization)
        {
            if (tenderLine == null)
                throw new ArgumentNullException("tenderLine");
            if (tokenizedPaymentCard == null)
                throw new ArgumentNullException("tokenizedPaymentCard");
            List<PaymentProperty> list = new List<PaymentProperty>();
            PaymentProperty paymentProperty1 = new PaymentProperty(GenericNamespace.TransactionData, TransactionDataProperties.Amount, Math.Abs(tenderLine.Amount), SecurityLevel.None);
            list.Add(paymentProperty1);
            PaymentProperty paymentProperty2 = new PaymentProperty(GenericNamespace.TransactionData, TransactionDataProperties.CurrencyCode, tenderLine.Currency, SecurityLevel.None);
            list.Add(paymentProperty2);
            PaymentProperty paymentProperty3 = new PaymentProperty(GenericNamespace.TransactionData, TransactionDataProperties.SupportCardTokenization, true.ToString(), SecurityLevel.None);
            list.Add(paymentProperty3);
            PaymentProperty paymentProperty4 = new PaymentProperty(GenericNamespace.TransactionData, TransactionDataProperties.AllowPartialAuthorization, allowPartialAuthorization.ToString(), SecurityLevel.None);
            list.Add(paymentProperty4);
            PaymentProperty paymentProperty6 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Name, tokenizedPaymentCard.NameOnCard, SecurityLevel.None);
            list.Add(paymentProperty6);
            PaymentProperty paymentProperty5 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardType, tokenizedPaymentCard.CardTypes, SecurityLevel.None);
            list.Add(paymentProperty5);

            PaymentProperty paymentProperty7 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.ExpirationYear, (Decimal)tokenizedPaymentCard.ExpirationYear, SecurityLevel.None);
            list.Add(paymentProperty7);
            PaymentProperty paymentProperty8 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.ExpirationMonth, (Decimal)tokenizedPaymentCard.ExpirationMonth, SecurityLevel.None);
            list.Add(paymentProperty8);
            PaymentProperty paymentProperty9 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.StreetAddress, tokenizedPaymentCard.Address1, SecurityLevel.None);
            list.Add(paymentProperty9);
            PaymentProperty paymentProperty10 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.PostalCode, tokenizedPaymentCard.Zip, SecurityLevel.None);
            list.Add(paymentProperty10);
            PaymentProperty countryProperty = GetCountryProperty(tokenizedPaymentCard.Country);
            list.Add(countryProperty);
            PaymentProperty paymentProperty11 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.UniqueCardId, tokenizedPaymentCard.CardTokenInfo.UniqueCardId, SecurityLevel.None);
            list.Add(paymentProperty11);
            PaymentProperty paymentProperty12 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardToken, tokenizedPaymentCard.CardTokenInfo.CardToken, SecurityLevel.None);
            list.Add(paymentProperty12);
            PaymentProperty paymentProperty13 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Last4Digits, tokenizedPaymentCard.CardTokenInfo.MaskedCardNumber, SecurityLevel.None);
            list.Add(paymentProperty13);
            PaymentProperty paymentProperty14 = new PaymentProperty(GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, tokenizedPaymentCard.CardTokenInfo.ServiceAccountId, SecurityLevel.None);
            list.Add(paymentProperty14);
            return list.ToArray();
        }

        private static PaymentProperty GetCountryProperty(string countryRegionCode)
        {
            if (string.IsNullOrWhiteSpace(countryRegionCode))
                throw new ArgumentException("countryRegionCode is empty.", "countryRegionCode");
            string str = "USA";
            if (countryRegionCode.Length == 2)
            {
                str = countryRegionCode;
            }
            else
            {
                if (countryRegionCode.Length != 3)
                    throw new DataValidationException(DataValidationErrors.InvalidFormat, "Invalid country/region code format.", new object[0]);
                // str = CardPaymentService.countryRegionMapper.ConvertToTwoLetterCountryCode(countryRegionCode);
            }
            return new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Country, str, SecurityLevel.None);
        }

        //commented

        //private static PaymentProperty[] AddConnectorPropertiesByServiceAccountId(RequestContext context, PaymentProperty[] paymentRequestProperties, out string connectorName)
        //{
        //    connectorName = string.Empty;
        //    string str1;
        //    if (!PaymentProperty.GetPropertyValue(PaymentProperty.ConvertToHashtable(paymentRequestProperties), GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, out str1) || string.IsNullOrWhiteSpace(str1))
        //    {
        //        //NetTracer.Error("Invalid request missing ServiceAccountId");
        //        throw new DataValidationException(DataValidationErrors.RequiredValueNotFound, "ServiceAccountId not found.", new object[0]);
        //    }
        //    List<PaymentProperty> list = Enumerable.ToList<PaymentProperty>((IEnumerable<PaymentProperty>)paymentRequestProperties);
        //    Collection<PaymentConnectorConfiguration> connectorInformation = GetMerchantConnectorInformation(context);
        //    bool flag = false;
        //    foreach (PaymentConnectorConfiguration connectorConfiguration in connectorInformation)
        //    {
        //        PaymentProperty[] properties = PaymentProperty.ConvertXMLToPropertyArray(connectorConfiguration.ConnectorProperties);
        //        string str2;
        //        Hashtable ht = PaymentProperty.ConvertToHashtable(properties);
        //        bool flg = PaymentProperty.GetPropertyValue(ht, GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, out str2);
        //        //if (flg && !string.IsNullOrWhiteSpace(str2) && str2 == str1)
        //        //{
        //        connectorName = connectorConfiguration.Name;
        //        flag = true;
        //        foreach (PaymentProperty paymentProperty in properties)
        //        {
        //            if (!list.Contains(paymentProperty))
        //                list.Add(paymentProperty);
        //        }
        //        break;
        //        //}
        //    }
        //    if (!flag)
        //    {
        //        string message = string.Format("No payment connector configurations were found that have the requested ServiceAccountId : '{0}'.", (object)str1);
        //        throw new ConfigurationException(ConfigurationErrors.PaymentConnectorNotFound, message, new object[0]);
        //    }
        //    PaymentProperty connectorNamePaymentProperty = new PaymentProperty(GenericNamespace.Connector, ConnectorProperties.ConnectorName, connectorName, SecurityLevel.None);
        //    PaymentProperty paymentProperty1 = Enumerable.SingleOrDefault<PaymentProperty>((IEnumerable<PaymentProperty>)list, (Func<PaymentProperty, bool>)(i =>
        //    {
        //        if (i.Namespace.Equals(connectorNamePaymentProperty.Namespace))
        //            return i.Name.Equals(connectorNamePaymentProperty.Name);
        //        return false;
        //    }));
        //    if (paymentProperty1 == null)
        //        list.Add(connectorNamePaymentProperty);
        //    else
        //        paymentProperty1.StringValue = connectorName;
        //    return list.ToArray();
        //}


        private static PaymentProperty[] AddConnectorPropertiesByServiceAccountId(RequestContext context, PaymentProperty[] paymentRequestProperties, out string connectorName)
        {
            connectorName = string.Empty;

            Hashtable requestHashtable = PaymentProperty.ConvertToHashtable(paymentRequestProperties);

            string requestedServiceAccountId;
            if (!PaymentProperty.GetPropertyValue(requestHashtable, GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, out requestedServiceAccountId)
                || string.IsNullOrWhiteSpace(requestedServiceAccountId))
            {
                //NetTracer.Error("Invalid request missing ServiceAccountId");
                throw new DataValidationException(DataValidationErrors.RequiredValueNotFound, "ServiceAccountId not found.");
            }

            // Need to load correct merchant account details
            List<PaymentProperty> updatedPaymentRequestProperties = paymentRequestProperties.ToList();

            var paymentConnectorConfigs = GetMerchantConnectorInformation(context);
            bool wasRequestedServiceAccountIdFound = false;
            foreach (PaymentConnectorConfiguration connectorConfig in paymentConnectorConfigs)
            {
                PaymentProperty[] connectorProperties = PaymentProperty.ConvertXMLToPropertyArray(connectorConfig.ConnectorProperties);
                Hashtable connectorPropertiesHashTable = PaymentProperty.ConvertToHashtable(connectorProperties);
                string foundServiceId;

                if (PaymentProperty.GetPropertyValue(connectorPropertiesHashTable, GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, out foundServiceId)
                   && !string.IsNullOrWhiteSpace(foundServiceId))
                {
                    if (foundServiceId == requestedServiceAccountId)
                    {
                        connectorName = connectorConfig.Name;
                        wasRequestedServiceAccountIdFound = true;
                        foreach (PaymentProperty connectorProperty in connectorProperties)
                        {
                            if (!updatedPaymentRequestProperties.Contains(connectorProperty))
                            {
                                updatedPaymentRequestProperties.Add(connectorProperty);
                            }
                        }

                        break;
                    }
                }
            }

            //if (!wasRequestedServiceAccountIdFound)
            //{
            //    string message = string.Format("No payment connector configurations were found that have the requested ServiceAccountId : '{0}'.", requestedServiceAccountId);
            //    throw new ConfigurationException(ConfigurationErrors.PaymentConnectorNotFound, message);
            //}

            PaymentProperty connectorNamePaymentProperty = new PaymentProperty(GenericNamespace.Connector, ConnectorProperties.ConnectorName, connectorName, SecurityLevel.None);

            var previousConnectorNameProperty = updatedPaymentRequestProperties.SingleOrDefault(i => (i.Namespace.Equals(connectorNamePaymentProperty.Namespace) && i.Name.Equals(connectorNamePaymentProperty.Name)));
            if (previousConnectorNameProperty == null)
            {
                updatedPaymentRequestProperties.Add(connectorNamePaymentProperty);
            }
            else
            {
                previousConnectorNameProperty.StringValue = connectorName;
            }

            return updatedPaymentRequestProperties.ToArray();
        }

        private static Collection<PaymentConnectorConfiguration> GetMerchantConnectorInformation(RequestContext context)
        {
            var merchantConnectorInformationList = new Collection<PaymentConnectorConfiguration>();

            var connectorConfigs = new Collection<PaymentConnectorConfiguration>();
            if (IsRequestFromTerminal(context))
            {
                if (context.GetTerminal() != null)
                {
                    GetPaymentConnectorDataRequest dataRequest = new GetPaymentConnectorDataRequest(context.GetTerminal().TerminalId);
                    connectorConfigs.Add(context.Execute<SingleEntityDataServiceResponse<PaymentConnectorConfiguration>>(dataRequest).Entity);
                }
            }
            else
            {
                long currentChannelId = context.GetPrincipal().ChannelId;

                // Get all setup merchant payment connector settings for a channel
                GetPaymentConnectorConfigurationDataRequest configurationRequest = new GetPaymentConnectorConfigurationDataRequest(currentChannelId);
                IEnumerable<PaymentConnectorConfiguration> connectors = context.Execute<EntityDataServiceResponse<PaymentConnectorConfiguration>>(configurationRequest).EntityCollection;

                // IEnumerable<PaymentConnectorConfiguration> connectors2 = GetPaymentConnectorConfiguration(configurationRequest).EntityCollection;
                connectorConfigs.AddRange(connectors);
            }

            if (!connectorConfigs.Any())
            {
                throw new ConfigurationException(ConfigurationErrors.PaymentConnectorNotFound, "No payment connectors found.");
            }

            foreach (PaymentConnectorConfiguration item in connectorConfigs)
            {
                if (item == null || string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.ConnectorProperties))
                {
                    continue;
                }

                //NetTracer.Information("Adding merchant information {0}", item.Name);

                // Save merchant data
                merchantConnectorInformationList.Add(item);
            }

            LoadAllSetupConnectors(merchantConnectorInformationList.ToArray());

            return merchantConnectorInformationList;
        }


        private static EntityDataServiceResponse<PaymentConnectorConfiguration> GetPaymentConnectorConfiguration(GetPaymentConnectorConfigurationDataRequest request)
        {
            PaymentConnectorDataManager connectorDataManager = new PaymentConnectorDataManager(request.RequestContext);
            ChannelDataManager channelDataManager = new ChannelDataManager(request.RequestContext);
            long currentChannelId = channelDataManager.GetCurrentChannelId();
            ReadOnlyCollection<PaymentConnectorConfiguration> connectors = connectorDataManager.GetPaymentConnectors(currentChannelId, new QueryResultSettings());
            return new EntityDataServiceResponse<PaymentConnectorConfiguration>(connectors);
        }

        private static bool IsRequestFromTerminal(RequestContext context)
        {
            return context != null && context.GetPrincipal() != null && context.GetPrincipal().TerminalId > 0;

        }
        private static void LoadAllSetupConnectors(params PaymentConnectorConfiguration[] paymentConfigs)
        {
            List<string> list = new List<string>();
            foreach (PaymentConnectorConfiguration connectorConfiguration in paymentConfigs)
            {
                Hashtable hashtable = PaymentProperty.ConvertToHashtable(PaymentProperty.ConvertXMLToPropertyArray(connectorConfiguration.ConnectorProperties));
                string str;
                if (!PaymentProperty.GetPropertyValue(hashtable, GenericNamespace.MerchantAccount, MerchantAccountProperties.PortableAssemblyName, out str))
                    PaymentProperty.GetPropertyValue(hashtable, GenericNamespace.MerchantAccount, MerchantAccountProperties.AssemblyName, out str);
                if (!string.IsNullOrEmpty(str) && !list.Contains(str))
                {
                    //NetTracer.Information("Adding assemblies to load {0}", (object)str);
                    list.Add(str);
                }
            }
            if (!Enumerable.Any<string>((IEnumerable<string>)list))
                return;
            try
            {
                // NetTracer.Information("Loading connectors");
                PaymentProcessorManager.Create(list.ToArray());
            }
            catch (Exception ex)
            {

            }
        }
        private static PaymentProperty[] GetTokenResponseProperties(string serializedTokenResponseProperties, TokenizedPaymentCard tokenizedPaymentCard)
        {
            return string.IsNullOrWhiteSpace(serializedTokenResponseProperties) ? GetSimulatedTokenResponseProperties(tokenizedPaymentCard) : PaymentProperty.ConvertXMLToPropertyArray(serializedTokenResponseProperties);
        }
        private static PaymentProperty[] GetSimulatedTokenResponseProperties(TokenizedPaymentCard tokenizedPaymentCard)
        {
            List<PaymentProperty> list = new List<PaymentProperty>();
            PaymentProperty paymentProperty1 = new PaymentProperty(GenericNamespace.TransactionData, TransactionDataProperties.SupportCardTokenization, true.ToString(), SecurityLevel.None);
            list.Add(paymentProperty1);
            PaymentProperty paymentProperty2 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardType, tokenizedPaymentCard.CardTypes, SecurityLevel.None);
            list.Add(paymentProperty2);
            PaymentProperty paymentProperty3 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Name, tokenizedPaymentCard.NameOnCard, SecurityLevel.None);
            list.Add(paymentProperty3);
            PaymentProperty paymentProperty4 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.ExpirationYear, (Decimal)tokenizedPaymentCard.ExpirationYear, SecurityLevel.None);
            list.Add(paymentProperty4);
            PaymentProperty paymentProperty5 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.ExpirationMonth, (Decimal)tokenizedPaymentCard.ExpirationMonth, SecurityLevel.None);
            list.Add(paymentProperty5);
            PaymentProperty paymentProperty6 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.StreetAddress, tokenizedPaymentCard.Address1, SecurityLevel.None);
            list.Add(paymentProperty6);
            PaymentProperty paymentProperty7 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.PostalCode, tokenizedPaymentCard.Zip, SecurityLevel.None);
            list.Add(paymentProperty7);
            PaymentProperty paymentProperty8 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Country, tokenizedPaymentCard.Country, SecurityLevel.None);
            list.Add(paymentProperty8);
            PaymentProperty paymentProperty9 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.UniqueCardId, tokenizedPaymentCard.CardTokenInfo.UniqueCardId, SecurityLevel.None);
            list.Add(paymentProperty9);
            PaymentProperty paymentProperty10 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardToken, tokenizedPaymentCard.CardTokenInfo.CardToken, SecurityLevel.None);
            list.Add(paymentProperty10);
            PaymentProperty paymentProperty11 = new PaymentProperty(GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, tokenizedPaymentCard.CardTokenInfo.ServiceAccountId, SecurityLevel.None);
            list.Add(paymentProperty11);
            PaymentProperty paymentProperty12 = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Last4Digits, tokenizedPaymentCard.CardTokenInfo.MaskedCardNumber, SecurityLevel.None);
            list.Add(paymentProperty12);
            return list.ToArray();
        }
        private static AuthorizePaymentServiceResponse AuthorizePayment(RequestContext context, TenderLine tenderLine, string connectorName, PaymentProperty[] paymentRequestProperties, PaymentProperty[] cardTokenResponsePaymentProperties)
        {
            Microsoft.Dynamics.Retail.PaymentSDK.Portable.Request request = new Microsoft.Dynamics.Retail.PaymentSDK.Portable.Request()
            {
                Locale = GetLocale(context)
            };
            request.Properties = paymentRequestProperties;
            IPaymentProcessor paymentProcessor = GetPaymentProcessor(connectorName);
            if (tenderLine.Amount >= new Decimal(0))
            {
                Microsoft.Dynamics.Retail.PaymentSDK.Portable.Response response = paymentProcessor.Authorize(request, (PaymentProperty[])null);
                VerifyResponseResult("Authorization", response, GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.AuthorizationResult, new List<string>()
           {
             AuthorizationResult.Success.ToString(),
             AuthorizationResult.PartialAuthorization.ToString()
           }, PaymentErrors.UnableToAuthorizePayment);
                if (cardTokenResponsePaymentProperties != null && Enumerable.Any<PaymentProperty>((IEnumerable<PaymentProperty>)cardTokenResponsePaymentProperties))
                {
                    PaymentProperty paymentProperty = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Properties, cardTokenResponsePaymentProperties);
                    response.Properties = CombinePaymentProperties((IEnumerable<PaymentProperty>)response.Properties, paymentProperty);
                }
                //NetTracer.Information("Completed Payment.Authorize");
                return new AuthorizePaymentServiceResponse(GetTenderLineFromAuthorizationResponse(tenderLine, response.Properties));
            }
            Microsoft.Dynamics.Retail.PaymentSDK.Portable.Response response1 = paymentProcessor.Refund(request, (PaymentProperty[])null);
            VerifyResponseResult("Refund", response1, GenericNamespace.RefundResponse, RefundResponseProperties.RefundResult, new List<string>()
         {
           RefundResult.Success.ToString()
         }, PaymentErrors.UnableToRefundPayment);
            // NetTracer.Information("Completed Payment.Refund");
            return new AuthorizePaymentServiceResponse(GetTenderLineFromRefundResponse(tenderLine, response1.Properties));
        }

        private static TenderLine GetTenderLineFromRefundResponse(TenderLine tenderLine, PaymentProperty[] refundProperties)
        {
            tenderLine.Status = TenderLineStatus.Committed; // refund doesn't require capture
            tenderLine.IsVoidable = false; // refund cannot be voided.

            Microsoft.Dynamics.Retail.PaymentSDK.Portable.Hashtable hashTable = PaymentProperty.ConvertToHashtable(refundProperties);

            PaymentProperty property = PaymentProperty.GetPropertyFromHashtable(hashTable, GenericNamespace.RefundResponse, RefundResponseProperties.Last4Digits);
            tenderLine.MaskedCardNumber = (property != null) ? (CardNumberMask + property.StringValue) : string.Empty;

            return tenderLine;
        }

        private static TenderLine GetTenderLineFromAuthorizationResponse(TenderLine tenderLine, PaymentProperty[] authorizationProperties)
        {
            tenderLine.Status = TenderLineStatus.PendingCommit;
            tenderLine.IsVoidable = true;

            Microsoft.Dynamics.Retail.PaymentSDK.Portable.Hashtable hashTable = PaymentProperty.ConvertToHashtable(authorizationProperties);

            PaymentProperty property = PaymentProperty.GetPropertyFromHashtable(hashTable, GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ApprovedAmount);
            tenderLine.Amount = (property != null) ? property.DecimalValue : 0.0m;

            property = PaymentProperty.GetPropertyFromHashtable(hashTable, GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.CashBackAmount);
            tenderLine.CashBackAmount = (property != null) ? property.DecimalValue : 0.0m;

            property = PaymentProperty.GetPropertyFromHashtable(hashTable, GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.Last4Digits);
            tenderLine.MaskedCardNumber = (property != null) ? (CardNumberMask + property.StringValue) : string.Empty;

            // Some payment connectors may capture in one payment operation.
            property = PaymentProperty.GetPropertyFromHashtable(hashTable, GenericNamespace.CaptureResponse, CaptureResponseProperties.CaptureResult);
            if (property != null && property.StringValue.Equals(CaptureResult.Success.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                // Yes it has been captured, set the tender line status to committed.
                tenderLine.Status = TenderLineStatus.Committed;
            }

            tenderLine.Authorization = PaymentProperty.ConvertPropertyArrayToXML(authorizationProperties);

            return tenderLine;
        }

        private static PaymentProperty[] CombinePaymentProperties(IEnumerable<PaymentProperty> paymentProperties1, PaymentProperty paymentProperty)
        {
            return CombinePaymentProperties(paymentProperties1, new[] { paymentProperty });
        }

        private static PaymentProperty[] CombinePaymentProperties(IEnumerable<PaymentProperty> paymentProperties1, IEnumerable<PaymentProperty> paymentProperties2)
        {
            // This logic can be replaced by a hashset to increase performance. 
            // Using HashSet will require changes to PaymentProperty.
            var combinedPaymentProperties = new List<PaymentProperty>(paymentProperties1);

            foreach (var paymentProperty in paymentProperties2)
            {
                if (!combinedPaymentProperties.Contains(paymentProperty))
                {
                    combinedPaymentProperties.Add(paymentProperty);
                }
            }

            return combinedPaymentProperties.ToArray();
        }
        private static string GetLocale(RequestContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            GetDefaultLanguageIdDataRequest languageIdRequest = new GetDefaultLanguageIdDataRequest();
            return context.Execute<SingleEntityDataServiceResponse<string>>(languageIdRequest).Entity;
        }

        private static void VerifyResponseResult(string operationName, Response response, string responseNamespace, string resultProperty, List<string> expectedValues, string errorResourceId)
        {
            PaymentProperty property = null;

            // Payment connector might of returned no properties
            if (response.Properties != null)
            {
                Microsoft.Dynamics.Retail.PaymentSDK.Portable.Hashtable hashTable = PaymentProperty.ConvertToHashtable(response.Properties);
                property = PaymentProperty.GetPropertyFromHashtable(
                    hashTable,
                    responseNamespace,
                    resultProperty);
            }

            if (property == null || !expectedValues.Where(s => s.Equals(property.StringValue, StringComparison.OrdinalIgnoreCase)).Any())
            {
                VerifyResponseErrors(operationName, response, errorResourceId);

                // throw generic exception if operation failed but no errors returned.
                throw new PaymentException(errorResourceId, "{0} failed.", operationName);
            }
        }

        private static void VerifyResponseErrors(string operationName, Response response, string errorResourceId)
        {
            StringBuilder errorsInfo = new StringBuilder("Error returned from the payment connector. See error details below:");

            if (response.Errors == null)
            {
                return;
            }

            // The format is used by clientError() in source\frameworks\retail\channels\apps\framework\core\notificationhandler.ts
            // to parse the error message.  If any format change is made here, update notificationhandler.ts accordingly.
            foreach (Microsoft.Dynamics.Retail.PaymentSDK.Portable.PaymentError error in response.Errors)
            {
                errorsInfo.AppendFormat("\nErrorCode: {0}, ErrorMessage: {1}", error.Code, error.Message);
            }

            string exceptionMessage = string.Format(
                CultureInfo.InvariantCulture,
                "{0} failed. Error(s): {1}",
                operationName,
                errorsInfo);
            var exception = new PaymentException(errorResourceId, exceptionMessage);

            foreach (Microsoft.Dynamics.Retail.PaymentSDK.Portable.PaymentError error in response.Errors)
            {
                exception.Data.Add(error.Code.ToString(), error.Message);
            }

            throw exception;
        }

        private static CardTypeInfo GetCardTypeConfiguration(long channelId, string cardTypeId, RequestContext context)
        {
            if (string.IsNullOrWhiteSpace(cardTypeId))
                throw new ArgumentNullException("cardTypeId");
            if (context == null)
                throw new ArgumentNullException("context");
            PagingInfo paging = new PagingInfo()
            {
                Skip = 0,
                Top = 1
            };
            IEnumerable<CardTypeInfo> source;
            if (string.IsNullOrWhiteSpace(cardTypeId))
            {
                GetCardTypeDataRequest cardTypeDataRequest = new GetCardTypeDataRequest(channelId);
                source = (IEnumerable<CardTypeInfo>)RequestContextExtensions.Execute<EntityDataServiceResponse<CardTypeInfo>>(context, (Microsoft.Dynamics.Commerce.Runtime.Messages.Request)cardTypeDataRequest).EntityCollection;
            }
            else
            {
                
                GetCardTypeDataRequest cardTypeDataRequest = new GetCardTypeDataRequest(channelId, cardTypeId);
                cardTypeDataRequest.QueryResultSettings = new QueryResultSettings(paging);
                source = (IEnumerable<CardTypeInfo>)IEnumerableExtensions.AsReadOnly<CardTypeInfo>((IEnumerable<CardTypeInfo>)RequestContextExtensions.Execute<EntityDataServiceResponse<CardTypeInfo>>(context, (Microsoft.Dynamics.Commerce.Runtime.Messages.Request)cardTypeDataRequest).EntityCollection);
            }

            return Enumerable.SingleOrDefault<CardTypeInfo>(source);
        }

        #endregion

        #endregion

        #region ExceptionsPayment

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
                                        new ChannelDataManager(context).GetChannelTenderTypes(context.GetPrincipal().ChannelId, new QueryResultSettings());

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
                                throw new PaymentException(PaymentErrors.UnableToCancelPayment, message);
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
                        throw new PaymentException(PaymentErrors.UnableToCancelPayment, message, ex);
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
                throw new PaymentException(PaymentErrors.UnableToCancelPayment, message, ex);
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



        /// <summary>
        /// Authorizes the payment.
        /// </summary>
        /// <param name="context">Request context.</param>
        /// <param name="tenderLine">Tender line to authorize.</param>
        /// <param name="paymentCard">Payment card information (optional).</param>
        /// <param name="skipLimitValidation">If set to 'true' limits validation (over-tender, under-tender etc.) will be skipped.</param>
        /// <returns>Tender line that represents authorized payment.</returns>
        private static TenderLine AuthorizePayment(RequestContext context, TenderLine tenderLine, PaymentCard paymentCard, bool skipLimitValidation)
        {
            AuthorizePaymentServiceRequest authorizeRequest = new AuthorizePaymentServiceRequest(tenderLine, paymentCard, skipLimitValidation);

            IRequestHandler paymentManagerHandler = context.Runtime.GetRequestHandler(authorizeRequest.GetType(), PaymentManagerService);

            var authorizeResponse = context.Execute<AuthorizePaymentServiceResponse>(authorizeRequest);
           // var authorizeResponse = AuthorizePayment(authorizeRequest);
            if (authorizeResponse.TenderLine == null)
            {
                throw new PaymentException(PaymentErrors.UnableToAuthorizePayment, "Payment service did not return tender line.");
            }

            return authorizeResponse.TenderLine;
        }

        private static AuthorizePaymentServiceResponse AuthorizePayment(AuthorizePaymentServiceRequest authorizationRequest)
        {
           // NetTracer.Information("Calling Payment.Authorize");

            if (authorizationRequest.TenderLine.IsPreProcessed)
            {
                return new AuthorizePaymentServiceResponse(ReEncryptPaymentAuthorizationProperties(authorizationRequest.RequestContext, authorizationRequest.TenderLine, authorizationRequest.Platform));
            }

            string connectorName;

            string cardTypeId = authorizationRequest.TenderLine.CardTypeId ?? authorizationRequest.PaymentCard.CardTypes;
            CardTypeInfo cardTypeConfiguration = GetCardTypeConfiguration(CommerceRuntimeHelper.RequestContext.GetPrincipal().ChannelId, cardTypeId, CommerceRuntimeHelper.RequestContext);
            if (!authorizationRequest.SkipLimitValidation)
            {
                ValidateCardEntry(authorizationRequest.PaymentCard, cardTypeConfiguration);
            }

            IList<PaymentProperty> paymentProperties = GetPaymentPropertiesForAuthorizationAndRefund(authorizationRequest.TenderLine, authorizationRequest.PaymentCard, cardTypeConfiguration, authorizationRequest.IsCardTokenRequired, authorizationRequest.AllowPartialAuthorization);

            IEnumerable<PaymentProperty> connectorProperties = GetConnectorProperties(CommerceRuntimeHelper.RequestContext, paymentProperties, out connectorName);
            paymentProperties.AddRange(connectorProperties);
            PaymentProperty[] paymentRequestProperties = paymentProperties.ToArray();

            AuthorizePaymentServiceResponse authorizeResponse = AuthorizePayment(
                CommerceRuntimeHelper.RequestContext,
                authorizationRequest.TenderLine,
                connectorName,
                paymentRequestProperties,
                cardTokenResponsePaymentProperties: null);

            return authorizeResponse;
        }


        /// <summary>
        /// Captures the payment.
        /// </summary>
        /// <param name="context">Request context.</param>
        /// <param name="tenderLine">Tender line to capture.</param>
        /// <returns>Tender line that represents captured payment.</returns>
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


        //private static CapturePaymentServiceResponse CapturePayment(CapturePaymentServiceRequest request)
        //{
        //    //NetTracer.Information("Calling Payment.CapturePayment");


        //    if (request.TenderLine.IsPreProcessed)
        //    {
        //        request.TenderLine.Status = TenderLineStatus.Committed;
        //        return new CapturePaymentServiceResponse(ReEncryptPaymentAuthorizationProperties(request.RequestContext, request.TenderLine, request.Platform));
        //    }

        //    string Locale = locale ?? GetLocale(CommerceRuntimeHelper.RequestContext); 


        //    List<PaymentProperty> Properties = GetPaymentPropertiesForCapture(request.TenderLine, request.PaymentCard.CardNumber);//.ToArray();

        //    Request paymentRequest = new Request();
        //    paymentRequest.Locale=Locale;
        //    paymentRequest.Properties=Properties.ToArray();



        //    if (paymentRequest.Properties == null
        //        || !paymentRequest.Properties.Any())
        //    {
        //        //NetTracer.Error("Invalid request");
        //        throw new DataValidationException(DataValidationErrors.RequiredValueNotFound, "Invalid request.");
        //    }

        //    PaymentProperty[] updatedPaymentRequestProperties = paymentRequest.Properties;

        //    string connectorName;
        //    updatedPaymentRequestProperties = AddConnectorPropertiesByServiceAccountId(CommerceRuntimeHelper.RequestContext, updatedPaymentRequestProperties, out connectorName);

        //    paymentRequest.Properties = updatedPaymentRequestProperties;

        //    // Get payment processor
        //    IPaymentProcessor processor = GetPaymentProcessor(connectorName);

        //    // Run Capture
        //    Response response = processor.Capture(paymentRequest);
        //    VerifyResponseResult("Capture", response, GenericNamespace.CaptureResponse, CaptureResponseProperties.CaptureResult, new List<string>() { CaptureResult.Success.ToString() }, PaymentErrors.UnableToCapturePayment);

        //   // NetTracer.Information("Completed Payment.Capture");
        //    return new CapturePaymentServiceResponse(GetTenderLineFromCaptureResponse(request));
        //}

        private static TenderLine GetTenderLineFromCaptureResponse(CapturePaymentServiceRequest request)
        {
            TenderLine tenderLine = request.TenderLine;
            tenderLine.Status = TenderLineStatus.Committed;
            tenderLine.IsVoidable = false; // payment cannot be voided once captured. 
            return tenderLine;
        }

        private static List<PaymentProperty> GetPaymentPropertiesForCapture(TenderLine tenderLine, string cardNumber)
        {
            List<PaymentProperty> properties = new List<PaymentProperty>();
            PaymentProperty property;

            if (!string.IsNullOrEmpty(cardNumber))
            {
                // Card number.
                property = new PaymentProperty(
                    GenericNamespace.PaymentCard,
                    PaymentCardProperties.CardNumber,
                    cardNumber,
                    SecurityLevel.PCI);
                properties.Add(property);
            }

            // Amount.
            property = new PaymentProperty(
                GenericNamespace.TransactionData,
                TransactionDataProperties.Amount,
                Math.Abs(tenderLine.Amount), // for refunds request amount must be positive
                SecurityLevel.None);
            properties.Add(property);

            // Currency
            property = new PaymentProperty(
                GenericNamespace.TransactionData,
                TransactionDataProperties.CurrencyCode,
                tenderLine.Currency,
                SecurityLevel.None);
            properties.Add(property);

            // Extract payment properties from TenderLine.Authorization.
            PaymentProperty[] storedAuthorizationResponseProperties = PaymentProperty.ConvertXMLToPropertyArray(tenderLine.Authorization);
            var paymentProperties = CombinePaymentProperties(storedAuthorizationResponseProperties, properties);

            return new List<PaymentProperty>(paymentProperties);
        }

        #endregion

        #endregion
		
	
      

    }
}
