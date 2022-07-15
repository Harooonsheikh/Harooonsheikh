using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.CRTD365.Helper;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using VSI.EDGEAXConnector.Enums.Enums.TMV;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using Microsoft.Dynamics.Commerce.RetailProxy;
using NewRelic.Api.Agent;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;
using Microsoft.Dynamics.Retail.PaymentSDK.Portable.Constants;
using Microsoft.Dynamics.Retail.PaymentSDK.Portable;
using PaymentMethod = VSI.EDGEAXConnector.Data.PaymentMethod;
using Hashtable = Microsoft.Dynamics.Retail.PaymentSDK.Portable.Hashtable;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class PaymentController : BaseController, IPaymentController
    {
        public string defaultLocale = string.Empty;
        public PaymentController(string storeKey) : base(storeKey)
        {
            defaultLocale = configurationHelper.GetSetting(PAYMENTCONNECTOR.Default_Locale);
        }

        #region Working Methods
        public string GenerateCardBlob(ErpPaymentCard cardProperties, string authTransactionId, PaymentMethod paymentMethod, string requestId, PaymentConnectorData paymentConnectorData)
        {
            if (cardProperties.ECommerceValue == PaymentCon.ALLPAGO_CC.ToString())
            {
                return GenerateCardAllPagoBlob(cardProperties, authTransactionId, paymentMethod, requestId, paymentConnectorData);
            }
            else if (cardProperties.ECommerceValue == PaymentCon.ADYEN_CC.ToString())
            {
                return GenerateCardAdyenBlob(cardProperties, authTransactionId, paymentMethod, requestId, paymentConnectorData);
            }
            else if (cardProperties.ECommerceValue == PaymentCon.ADYEN_HPP.ToString())
            {
                return GenerateCardAlipayBlob(cardProperties, authTransactionId, paymentMethod, requestId, paymentConnectorData);
            }

            //++ Getting Payment Connector informaion 
            var paymentConnectors = GetPaymentConnectorInfo(requestId);

            Hashtable paymentHash;
            PaymentProperty[] paymentProperties;
            string cardTokenBlob = string.Empty;
            string paymentConnectorLog = string.Empty;

            if (paymentConnectors != null)
            {
                bool isServiceAccountMatch = false;

                foreach (var connector in paymentConnectors)
                {
                    paymentProperties = PaymentProperty.ConvertXMLToPropertyArray(connector.ConnectorProperties);
                    paymentHash = PaymentProperty.ConvertToHashtable(paymentProperties);

                    string supportedTenderTypes = "";
                    var issupportedTenderTypes = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.SupportedTenderTypes.ToString(), out supportedTenderTypes);
                    if (issupportedTenderTypes)
                        paymentConnectorData.SupportedTenderTypes = supportedTenderTypes;

                    string serviceAccountId = "";
                    var isServiceAccountId = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.ServiceAccountId.ToString(), out serviceAccountId);
                    if (isServiceAccountId)
                        paymentConnectorData.ServiceAccountId = serviceAccountId;

                    paymentConnectorLog += "serviceAccountId: " + serviceAccountId +
                                           " || supportedTenderTypes: " + supportedTenderTypes + "; ";

                    var supportedTenderTypeList = paymentConnectorData.SupportedTenderTypes.ToUpper().Split(';').ToList();

                    if (serviceAccountId == paymentMethod.ServiceAccountId && supportedTenderTypeList.Contains(cardProperties.CardTypes.ToUpper()))
                    {
                        isServiceAccountMatch = true;

                        paymentConnectorData.PaymentService = connector;
                        if (paymentMethod.ECommerceValue == PaymentConnectorProperties.TESTCONNECTOR.ToString())
                        {
                            string assemblyName = "";
                            var isAssemblyName = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.AssemblyName.ToString(), out assemblyName);
                            if (isAssemblyName)
                                paymentConnectorData.ConnectorAssembly = assemblyName;
                        }
                        else
                        {
                            string assemblyName = "";
                            var isAssemblyName = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), "PortableAssemblyName", out assemblyName);
                            if (isAssemblyName)
                                paymentConnectorData.ConnectorAssembly = assemblyName;
                        }
                        break;
                    }
                }

                if (!isServiceAccountMatch)
                {
                    var clLogData = "CL Payment Method: " + paymentMethod.ECommerceValue +
                                  " || Service Account Id: " + paymentMethod.ServiceAccountId +
                                  " || Cardtype: " + cardProperties.CardTypes;

                    var log = clLogData + " , does not match with D365 connector information: " + paymentConnectorLog;

                    CommerceLinkLogger.LogException(currentStore, new Exception(log), MethodBase.GetCurrentMethod().Name, requestId);

                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40518, currentStore);
                    throw new CommerceLinkError(message);
                }

                List<PaymentProperty> requestProperties = new List<PaymentProperty>();
                PaymentProperty property;

                //TODO: get it from CRT
                property = new PaymentProperty(GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, paymentConnectorData.ServiceAccountId, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.Connector, ConnectorProperties.ConnectorName, paymentConnectorData.PaymentService.Name, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardType, cardProperties.CardTypes, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Last4Digits, cardProperties.CardNumber, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.ExpirationMonth, cardProperties.ExpirationMonth.Value, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.ExpirationYear, cardProperties.ExpirationYear.Value, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardToken, cardProperties.CardToken, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.UniqueCardId, cardProperties.UniqueCardId, SecurityLevel.None);
                requestProperties.Add(property);
                if (!string.IsNullOrWhiteSpace(cardProperties.CCID))
                {
                    property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.AdditionalSecurityData, cardProperties.CCID, SecurityLevel.None);
                    requestProperties.Add(property);
                }
                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Name, cardProperties.NameOnCard, SecurityLevel.None);
                requestProperties.Add(property);


                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.StreetAddress, cardProperties.Address1, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.City, cardProperties.City, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.State, cardProperties.State, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.PostalCode, cardProperties.Zip, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Country, cardProperties.Country, SecurityLevel.None);
                requestProperties.Add(property);

                if (configurationHelper.GetSetting(PAYMENTCONNECTOR.Supported_Card_Types).ToUpper().Contains(cardProperties.CardTypes.ToUpper()))
                {
                    property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.ProviderTransactionType.ToString(), "authorizationonly", SecurityLevel.None);
                    requestProperties.Add(property);
                }

                //++For PayPal.
                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.ParentTransactionId.ToString(), cardProperties.ParentTransactionId, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.PayerId.ToString(), cardProperties.PayerId, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.Email.ToString(), cardProperties.Email, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.Note.ToString(), cardProperties.Note, SecurityLevel.None);
                requestProperties.Add(property);

                if (!string.IsNullOrEmpty(cardProperties.ThreeDSecure))
                {
                    property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.ThreeDSecure.ToString(), cardProperties.ThreeDSecure, SecurityLevel.None);
                    requestProperties.Add(property);
                }

                cardTokenBlob = PaymentProperty.ConvertPropertyArrayToXML(requestProperties.ToArray());
            }
            else
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40517, currentStore);
                throw new CommerceLinkError(message);
            }
            return cardTokenBlob;
        }
        public string GenerateAuthBlob(ErpPaymentCard cardProperties, string authTransactionId, string approvalCode, decimal approvalAmount, DateTime transactionDate, string currencyCode, PaymentConnectorData paymentConnectorData)
        {
            if (cardProperties.ECommerceValue == PaymentCon.ALLPAGO_CC.ToString())
            {
                return GenerateAuthBlobAllPago(cardProperties, authTransactionId, approvalCode, approvalAmount, transactionDate, currencyCode, paymentConnectorData);
            }
            else if (cardProperties.ECommerceValue == PaymentCon.ADYEN_CC.ToString())
            {
                return GenerateAuthBlobAdyen(cardProperties, authTransactionId, approvalCode, approvalAmount, transactionDate, currencyCode, paymentConnectorData);
            }
            else if (cardProperties.ECommerceValue == PaymentCon.ADYEN_HPP.ToString())
            {
                return GenerateAuthBlobAlipay(cardProperties, authTransactionId, approvalCode, approvalAmount, transactionDate, currencyCode, paymentConnectorData);
            }
            List<PaymentProperty> requestProperties = new List<PaymentProperty>();
            PaymentProperty property;

            //TODO: get it from CRT
            property = new PaymentProperty(GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, paymentConnectorData.ServiceAccountId, SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.Connector, ConnectorProperties.ConnectorName, paymentConnectorData.PaymentService.Name, SecurityLevel.None);
            requestProperties.Add(property);

            var authResposneProperties = new List<PaymentProperty>();

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.CardType, cardProperties.CardTypes, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.IsSwiped, "false", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.Last4Digits, cardProperties.CardNumber, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.CardToken, cardProperties.IsPayPal ? authTransactionId : cardProperties.CardToken, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.UniqueCardId, cardProperties.IsPayPal ? authTransactionId : cardProperties.UniqueCardId, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ProviderTransactionId, authTransactionId, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ApprovalCode, authTransactionId, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ApprovedAmount, approvalAmount, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.CurrencyCode, currencyCode, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.AuthorizationResult, "Success", SecurityLevel.None); authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ProviderMessage, "Success", SecurityLevel.None);
            authResposneProperties.Add(property);

            //property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.AVSResult, "Returned", SecurityLevel.None);

            authResposneProperties.Add(property); property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.AVSDetail, "BillingAddress", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.CVV2Result, "Success", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.AvailableBalance, 0, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.TransactionType, "Authorize", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.TransactionDateTime, transactionDate, SecurityLevel.None);
            authResposneProperties.Add(property);

            //++ New Fields
            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ResponseCode, authTransactionId, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentCardProperties.ExpirationYear, cardProperties.ExpirationYear.Value, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentCardProperties.ExpirationMonth, cardProperties.ExpirationMonth.Value, SecurityLevel.None);
            authResposneProperties.Add(property);

            //++ Wirecard specific 
            //if (cardProperties.CardTypes.ToUpper() == "VISA" || cardProperties.CardTypes.ToUpper() == "AMEX" || cardProperties.CardTypes.ToUpper() == "MASTERCARD" || cardProperties.CardTypes.ToUpper() == "DISCOVER" || cardProperties.CardTypes.ToUpper() == "JCB" || cardProperties.CardTypes.ToUpper() == "DINERSCLUB")
            if (configurationHelper.GetSetting(PAYMENTCONNECTOR.Supported_Card_Types).ToUpper().Contains(cardProperties.CardTypes.ToUpper()))
            {
                property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.ProviderTransactionType.ToString(), "authorization", SecurityLevel.None);
                authResposneProperties.Add(property);
            }

            //++Paypal
            //Not required
            //property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.ParentTransactionId.ToString(), cardProperties.ParentTransactionId, SecurityLevel.None);
            //requestProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.PayerId.ToString(), cardProperties.PayerId, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.Email.ToString(), cardProperties.Email, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.Note.ToString(), cardProperties.Note, SecurityLevel.None);
            authResposneProperties.Add(property);

            if (!string.IsNullOrEmpty(cardProperties.ThreeDSecure))
            {
                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.ThreeDSecure.ToString(), cardProperties.ThreeDSecure, SecurityLevel.None);
                requestProperties.Add(property);
            }

            List<PaymentProperty> level2Properties = new List<PaymentProperty>();
            var level2Property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.Properties, propertyList: authResposneProperties.ToArray());
            level2Properties.Add(level2Property);

            //++TransactionData 
            property = new PaymentProperty(GenericNamespace.TransactionData, TransactionDataProperties.IndustryType, transactionDate, SecurityLevel.None);
            authResposneProperties.Add(property);

            // property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.TransactionDateTime, transactionDate, SecurityLevel.None);

            Response tokenizeResponse = new Response { Properties = requestProperties.ToArray(), Locale = "en-US" };
            Response authResponse = new Response { Properties = level2Properties.ToArray(), Locale = "en-US" };

            Response combinedResponse = this.CombineResponses(tokenizeResponse, authResponse, null, null);

            string authBlob = PaymentProperty.ConvertPropertyArrayToXML(combinedResponse.Properties);
            return authBlob;
        }

        #region Adyen Blobs
        public string GenerateCardAdyenBlob(ErpPaymentCard cardProperties, string authTransactionId, PaymentMethod paymentMethod, string requestId, PaymentConnectorData paymentConnectorData)
        {
            var paymentConnectors = GetPaymentConnectorInfo(requestId);

            Hashtable paymentHash;
            PaymentProperty[] paymentProperties;
            string cardTokenBlob = string.Empty;
            string paymentConnectorLog = string.Empty;

            if (paymentConnectors != null)
            {
                bool isServiceAccountMatch = false;

                foreach (var connector in paymentConnectors)
                {
                    paymentProperties = PaymentProperty.ConvertXMLToPropertyArray(connector.ConnectorProperties);
                    paymentHash = PaymentProperty.ConvertToHashtable(paymentProperties);

                    string supportedTenderTypes = "";
                    var issupportedTenderTypes = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.SupportedTenderTypes.ToString(), out supportedTenderTypes);
                    if (issupportedTenderTypes)
                        paymentConnectorData.SupportedTenderTypes = supportedTenderTypes;

                    string serviceAccountId = "";
                    var isServiceAccountId = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.ServiceAccountId.ToString(), out serviceAccountId);
                    if (isServiceAccountId)
                        paymentConnectorData.ServiceAccountId = serviceAccountId;

                    paymentConnectorLog += "serviceAccountId: " + serviceAccountId +
                                           " || supportedTenderTypes: " + supportedTenderTypes + "; ";

                    var supportedTenderTypeList = paymentConnectorData.SupportedTenderTypes.ToUpper().Split(';').ToList();

                    if (serviceAccountId == paymentMethod.ServiceAccountId && supportedTenderTypeList.Contains(cardProperties.CardTypes.ToUpper()))
                    {
                        isServiceAccountMatch = true;

                        paymentConnectorData.PaymentService = connector;
                        if (paymentMethod.ECommerceValue == PaymentConnectorProperties.TESTCONNECTOR.ToString())
                        {
                            string assemblyName = "";
                            var isAssemblyName = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.AssemblyName.ToString(), out assemblyName);
                            if (isAssemblyName)
                                paymentConnectorData.ConnectorAssembly = assemblyName;
                        }
                        else
                        {
                            string assemblyName = "";
                            var isAssemblyName = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), "PortableAssemblyName", out assemblyName);
                            if (isAssemblyName)
                                paymentConnectorData.ConnectorAssembly = assemblyName;
                        }
                        break;
                    }
                }

                if (!isServiceAccountMatch)
                {
                    var clLogData = "CL Payment Method: " + paymentMethod.ECommerceValue +
                                  " || Service Account Id: " + paymentMethod.ServiceAccountId +
                                  " || Cardtype: " + cardProperties.CardTypes;

                    var log = clLogData + " , does not match with D365 connector information: " + paymentConnectorLog;

                    CommerceLinkLogger.LogException(currentStore, new Exception(log), MethodBase.GetCurrentMethod().Name, requestId);

                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40518, currentStore);
                    throw new CommerceLinkError(message);
                }

                List<PaymentProperty> requestProperties = new List<PaymentProperty>();
                PaymentProperty property;

                //TODO: get it from CRT
                property = new PaymentProperty(GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, paymentConnectorData.ServiceAccountId, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.Connector, ConnectorProperties.ConnectorName, paymentConnectorData.PaymentService.Name, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.FraudResult.ToString(), "GREEN", SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardType, cardProperties.CardTypes, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardToken, cardProperties.CardToken, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Last4Digits, cardProperties.CardNumber, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.UniqueCardId, authTransactionId, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.ExpirationYear, cardProperties.ExpirationYear.Value, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.ExpirationMonth, cardProperties.ExpirationMonth.Value, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Name, cardProperties.NameOnCard, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.BankIdentificationNumberStart, cardProperties.BankIdentificationNumberStart, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardVerificationValue, "Success", SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.House, cardProperties.Address1, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.StreetAddress, cardProperties.Address1, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.City, cardProperties.City, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.State, cardProperties.State, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.PostalCode, cardProperties.Zip, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Country, cardProperties.IssuerCountry, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.Note.ToString(), "Web", SecurityLevel.None);
                requestProperties.Add(property);

                if (!string.IsNullOrEmpty(cardProperties.ThreeDSecure))
                {
                    property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.ThreeDSecure.ToString(), cardProperties.ThreeDSecure, SecurityLevel.None);
                    requestProperties.Add(property);
                }

                cardTokenBlob = PaymentProperty.ConvertPropertyArrayToXML(requestProperties.ToArray());
            }
            else
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40517, currentStore);
                throw new CommerceLinkError(message);
            }
            return cardTokenBlob;
        }
        public string GenerateAuthBlobAdyen(ErpPaymentCard cardProperties, string authTransactionId, string approvalCode, decimal approvalAmount, DateTime transactionDate, string currencyCode, PaymentConnectorData paymentConnectorData)
        {
            List<PaymentProperty> requestProperties = new List<PaymentProperty>();
            PaymentProperty property;

            //TODO: get it from CRT
            property = new PaymentProperty(GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, paymentConnectorData.ServiceAccountId, SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.Connector, ConnectorProperties.ConnectorName, paymentConnectorData.PaymentService.Name, SecurityLevel.None);
            requestProperties.Add(property);

            var authResposneProperties = new List<PaymentProperty>();
            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.AuthorizationResult, "Success", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ApprovedAmount, approvalAmount, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.UniqueCardId, authTransactionId, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.Last4Digits, cardProperties.CardNumber, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.CardType, cardProperties.CardTypes, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.CVV2Result, "Success", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ApprovalCode, approvalCode, SecurityLevel.None);
            //property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ApprovalCode, "33011", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ProviderTransactionId, authTransactionId, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.TransactionId.ToString(), cardProperties.shopperReference, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.OriginalPSP.ToString(), cardProperties.shopperReference, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.TransactionType, "Authorize", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.TransactionDateTime, transactionDate, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.AVSResult, "VerificationNotSupported", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.AVSDetail, "None", SecurityLevel.None);
            authResposneProperties.Add(property);

            if (!string.IsNullOrEmpty(cardProperties.ThreeDSecure))
            {
                property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.ThreeDSecure.ToString(), cardProperties.ThreeDSecure, SecurityLevel.None);
                requestProperties.Add(property);
            }

            List<PaymentProperty> level2Properties = new List<PaymentProperty>();
            var level2Property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.Properties, propertyList: authResposneProperties.ToArray());
            level2Properties.Add(level2Property);

            //++TransactionData 
            property = new PaymentProperty(GenericNamespace.TransactionData, TransactionDataProperties.IndustryType, transactionDate, SecurityLevel.None);
            authResposneProperties.Add(property);

            // property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.TransactionDateTime, transactionDate, SecurityLevel.None);

            Response tokenizeResponse = new Response { Properties = requestProperties.ToArray(), Locale = "en-US" };
            Response authResponse = new Response { Properties = level2Properties.ToArray(), Locale = "en-US" };

            Response combinedResponse = this.CombineResponses(tokenizeResponse, authResponse, null, null);

            string authBlob = PaymentProperty.ConvertPropertyArrayToXML(combinedResponse.Properties);
            return authBlob;
        }
        #endregion

        #region Alipay
        public string GenerateCardAlipayBlob(ErpPaymentCard cardProperties, string authTransactionId, PaymentMethod paymentMethod, string requestId, PaymentConnectorData paymentConnectorData)
        {
            var paymentConnectors = GetPaymentConnectorInfo(requestId);

            Hashtable paymentHash;
            PaymentProperty[] paymentProperties;
            string cardTokenBlob = string.Empty;
            string paymentConnectorLog = string.Empty;

            if (paymentConnectors != null)
            {
                bool isServiceAccountMatch = false;

                foreach (var connector in paymentConnectors)
                {
                    paymentProperties = PaymentProperty.ConvertXMLToPropertyArray(connector.ConnectorProperties);
                    paymentHash = PaymentProperty.ConvertToHashtable(paymentProperties);

                    string supportedTenderTypes = "";
                    var issupportedTenderTypes = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.SupportedTenderTypes.ToString(), out supportedTenderTypes);
                    if (issupportedTenderTypes)
                        paymentConnectorData.SupportedTenderTypes = supportedTenderTypes;

                    string serviceAccountId = "";
                    var isServiceAccountId = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.ServiceAccountId.ToString(), out serviceAccountId);
                    if (isServiceAccountId)
                        paymentConnectorData.ServiceAccountId = serviceAccountId;

                    paymentConnectorLog += "serviceAccountId: " + serviceAccountId +
                                           " || supportedTenderTypes: " + supportedTenderTypes + "; ";

                    var supportedTenderTypeList = paymentConnectorData.SupportedTenderTypes.ToUpper().Split(';').ToList();

                    if (serviceAccountId == paymentMethod.ServiceAccountId && supportedTenderTypeList.Contains(cardProperties.CardTypes.ToUpper()))
                    {
                        isServiceAccountMatch = true;

                        paymentConnectorData.PaymentService = connector;
                        if (paymentMethod.ECommerceValue == PaymentConnectorProperties.TESTCONNECTOR.ToString())
                        {
                            string assemblyName = "";
                            var isAssemblyName = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.AssemblyName.ToString(), out assemblyName);
                            if (isAssemblyName)
                                paymentConnectorData.ConnectorAssembly = assemblyName;
                        }
                        else
                        {
                            string assemblyName = "";
                            var isAssemblyName = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), "PortableAssemblyName", out assemblyName);
                            if (isAssemblyName)
                                paymentConnectorData.ConnectorAssembly = assemblyName;
                        }
                        break;
                    }
                }

                if (!isServiceAccountMatch)
                {
                    var clLogData = "CL Payment Method: " + paymentMethod.ECommerceValue +
                                  " || Service Account Id: " + paymentMethod.ServiceAccountId +
                                  " || Cardtype: " + cardProperties.CardTypes;

                    var log = clLogData + " , does not match with D365 connector information: " + paymentConnectorLog;

                    CommerceLinkLogger.LogException(currentStore, new Exception(log), MethodBase.GetCurrentMethod().Name, requestId);

                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40518, currentStore);
                    throw new CommerceLinkError(message);
                }

                List<PaymentProperty> requestProperties = new List<PaymentProperty>();
                PaymentProperty property;

                property = new PaymentProperty(GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, paymentConnectorData.ServiceAccountId, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.Connector, ConnectorProperties.ConnectorName, paymentConnectorData.PaymentService.Name, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardType, cardProperties.CardTypes, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardToken, cardProperties.CardToken, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Last4Digits, cardProperties.CardNumber, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.UniqueCardId, authTransactionId, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.ExpirationYear, cardProperties.ExpirationYear.Value, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.ExpirationMonth, cardProperties.ExpirationMonth.Value, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Name, cardProperties.NameOnCard, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.House, cardProperties.Address1, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.StreetAddress, cardProperties.Address1, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.City, cardProperties.City, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.State, cardProperties.State, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.PostalCode, cardProperties.Zip, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Country, cardProperties.IssuerCountry, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.Note.ToString(), "Web", SecurityLevel.None);
                requestProperties.Add(property);

                cardTokenBlob = PaymentProperty.ConvertPropertyArrayToXML(requestProperties.ToArray());
            }
            else
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40517, currentStore);
                throw new CommerceLinkError(message);
            }
            return cardTokenBlob;
        }
        public string GenerateAuthBlobAlipay(ErpPaymentCard cardProperties, string authTransactionId, string approvalCode, decimal approvalAmount, DateTime transactionDate, string currencyCode, PaymentConnectorData paymentConnectorData)
        {
            List<PaymentProperty> requestProperties = new List<PaymentProperty>();
            PaymentProperty property;

            property = new PaymentProperty(GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, paymentConnectorData.ServiceAccountId, SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.Connector, ConnectorProperties.ConnectorName, paymentConnectorData.PaymentService.Name, SecurityLevel.None);
            requestProperties.Add(property);

            var authResposneProperties = new List<PaymentProperty>();
            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.AuthorizationResult, "Success", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ApprovedAmount, approvalAmount, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.UniqueCardId, authTransactionId, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.Last4Digits, cardProperties.CardNumber, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.CardType, cardProperties.CardTypes, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ProviderTransactionId, authTransactionId, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.TransactionId.ToString(), cardProperties.shopperReference, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.TransactionType, "Authorize", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.TransactionDateTime, transactionDate, SecurityLevel.None);
            authResposneProperties.Add(property);

            List<PaymentProperty> level2Properties = new List<PaymentProperty>();
            var level2Property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.Properties, propertyList: authResposneProperties.ToArray());
            level2Properties.Add(level2Property);

            property = new PaymentProperty(GenericNamespace.TransactionData, TransactionDataProperties.IndustryType, transactionDate, SecurityLevel.None);
            authResposneProperties.Add(property);

            Response tokenizeResponse = new Response { Properties = requestProperties.ToArray(), Locale = "en-US" };
            Response authResponse = new Response { Properties = level2Properties.ToArray(), Locale = "en-US" };

            Response combinedResponse = this.CombineResponses(tokenizeResponse, authResponse, null, null);

            string authBlob = PaymentProperty.ConvertPropertyArrayToXML(combinedResponse.Properties);
            return authBlob;
        }
        #endregion

        #region ALLPAGO
        public string GenerateCardAllPagoBlob(ErpPaymentCard cardProperties, string authTransactionId, PaymentMethod paymentMethod, string requestId, PaymentConnectorData paymentConnectorData)
        {
            var paymentConnectors = this.GetPaymentConnectorInfo(requestId);

            Hashtable paymentHash;
            PaymentProperty[] paymentProperties;
            string cardTokenBlob = "";
            string paymentConnectorLog = string.Empty;

            if (paymentConnectors != null)
            {
                bool isServiceAccountMatch = false;

                foreach (var connector in paymentConnectors)
                {
                    paymentProperties = PaymentProperty.ConvertXMLToPropertyArray(connector.ConnectorProperties);
                    paymentHash = PaymentProperty.ConvertToHashtable(paymentProperties);

                    string supportedTenderTypes = "";
                    var issupportedTenderTypes = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.SupportedTenderTypes.ToString(), out supportedTenderTypes);
                    if (issupportedTenderTypes)
                        paymentConnectorData.SupportedTenderTypes = supportedTenderTypes;

                    string serviceAccountId = "";
                    var isServiceAccountId = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.ServiceAccountId.ToString(), out serviceAccountId);
                    if (isServiceAccountId)
                        paymentConnectorData.ServiceAccountId = serviceAccountId;

                    paymentConnectorLog += "serviceAccountId: " + serviceAccountId +
                   " || supportedTenderTypes: " + supportedTenderTypes + "; ";

                    var supportedTenderTypeList = paymentConnectorData.SupportedTenderTypes.ToUpper().Split(';').ToList();

                    if (serviceAccountId == paymentMethod.ServiceAccountId && supportedTenderTypeList.Contains(cardProperties.CardTypes.ToUpper()))
                    {
                        isServiceAccountMatch = true;

                        paymentConnectorData.PaymentService = connector;
                        if (paymentMethod.ECommerceValue == PaymentConnectorProperties.TESTCONNECTOR.ToString())
                        {
                            string assemblyName = "";
                            var isAssemblyName = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), PaymentConfigurations.AssemblyName.ToString(), out assemblyName);
                            if (isAssemblyName)
                                paymentConnectorData.ConnectorAssembly = assemblyName;
                        }
                        else
                        {
                            string assemblyName = "";
                            var isAssemblyName = PaymentProperty.GetPropertyValue(paymentHash, PaymentConfigurations.MerchantAccount.ToString(), "PortableAssemblyName", out assemblyName);
                            if (isAssemblyName)
                                paymentConnectorData.ConnectorAssembly = assemblyName;
                        }
                        break;
                    }
                }

                //Throw exception if service account id is not matched
                if (!isServiceAccountMatch)
                {
                    var clLogData = "CL Payment Method: " + paymentMethod.ECommerceValue +
                                    " || Service Account Id: " + paymentMethod.ServiceAccountId +
                                    " || Cardtype: " + cardProperties.CardTypes;

                    var log = clLogData +
                              " , does not match with D365 connector information: " +
                              paymentConnectorLog;

                    CommerceLinkLogger.LogException(currentStore, new Exception(log), MethodBase.GetCurrentMethod().Name, requestId);

                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40518, currentStore);
                    throw new CommerceLinkError(message);
                }

                List<PaymentProperty> requestProperties = new List<PaymentProperty>();
                PaymentProperty property;

                property = new PaymentProperty(GenericNamespace.Connector, ConnectorProperties.ConnectorName, paymentConnectorData.PaymentService.Name, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, paymentConnectorData.ServiceAccountId, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardType, cardProperties.CardTypes.ToUpper(), SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.CardToken, authTransactionId, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Last4Digits, cardProperties.CardNumber, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.ExpirationYear, cardProperties.ExpirationYear.Value, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.ExpirationMonth, cardProperties.ExpirationMonth.Value, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.UniqueCardId, cardProperties.CardToken, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.ProviderTransactionType.ToString(), "PA", SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.registrationId.ToString(), cardProperties.CardToken, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.numberOfInstallments.ToString(), "" + cardProperties.NumberOfInstallments, SecurityLevel.None);
                requestProperties.Add(property);

                if (!string.IsNullOrWhiteSpace(cardProperties.CCID))
                {
                    property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.AdditionalSecurityData, cardProperties.CCID, SecurityLevel.None);
                    requestProperties.Add(property);
                }
                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Name, cardProperties.NameOnCard, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.StreetAddress, cardProperties.Address1 ?? string.Empty, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.City, cardProperties.City ?? string.Empty, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.State, cardProperties.State ?? string.Empty, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.PostalCode, cardProperties.Zip ?? string.Empty, SecurityLevel.None);
                requestProperties.Add(property);

                var country = string.Empty;
                if (cardProperties.Country == "BRA" || configurationHelper.GetSetting(CUSTOMER.Default_ThreeLetterISORegionName) == "BRA")
                {
                    country = "BR";
                }
                else if (cardProperties.Country == "MEX" || configurationHelper.GetSetting(CUSTOMER.Default_ThreeLetterISORegionName) == "MEX")
                {
                    country = "MX";
                }

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentCardProperties.Country, country, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.Email.ToString(), cardProperties.Email, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.IP.ToString(), cardProperties.IP, SecurityLevel.None);
                requestProperties.Add(property);

                property = new PaymentProperty(GenericNamespace.PaymentCard, PaymentConfigurations.Note.ToString(), cardProperties.Note, SecurityLevel.None);
                requestProperties.Add(property);

                cardTokenBlob = PaymentProperty.ConvertPropertyArrayToXML(requestProperties.ToArray());
            }
            else
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40517, currentStore);
                throw new CommerceLinkError(message);
            }
            return cardTokenBlob;
        }
        public string GenerateAuthBlobAllPago(ErpPaymentCard cardProperties, string authTransactionId, string approvalCode, decimal approvalAmount, DateTime transactionDate, string currencyCode, PaymentConnectorData paymentConnectorData)
        {
            List<PaymentProperty> requestProperties = new List<PaymentProperty>();
            PaymentProperty property;

            //TODO: get it from CRT

            property = new PaymentProperty(GenericNamespace.Connector, ConnectorProperties.ConnectorName, paymentConnectorData.PaymentService.Name, SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.MerchantAccount, MerchantAccountProperties.ServiceAccountId, paymentConnectorData.ServiceAccountId, SecurityLevel.None);
            requestProperties.Add(property);

            var authResposneProperties = new List<PaymentProperty>();

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.CardType, cardProperties.CardTypes, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.CardToken, authTransactionId, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.Last4Digits, cardProperties.CardNumber, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentCardProperties.ExpirationYear, cardProperties.ExpirationYear.Value, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentCardProperties.ExpirationMonth, cardProperties.ExpirationMonth.Value, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.UniqueCardId, cardProperties.CardToken, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.ProviderTransactionType.ToString(), "PA", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.numberOfInstallments.ToString(), cardProperties.NumberOfInstallments.ToString(), SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ProviderTransactionId, authTransactionId, SecurityLevel.None);
            authResposneProperties.Add(property);
            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ResponseCode, approvalCode, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ProviderMessage, "Request successfully processed", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.TransactionType, "Authorize", SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.TransactionDateTime, transactionDate, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.MerchantTransactionId.ToString(), cardProperties.TransactionId, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.MerchantCustomerId.ToString(), cardProperties.LocalTaxId, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.IsRecurringTransaction.ToString(), "false", SecurityLevel.None);
            authResposneProperties.Add(property);

            //property = new PaymentProperty(GenericNamespace.AuthorizationResponse, PaymentConfigurations.OriginalProviderTransactionId.ToString(), authTransactionId, SecurityLevel.None);
            //authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ApprovalCode, approvalCode, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ApprovedAmount, approvalAmount, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.CurrencyCode, currencyCode, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.ApprovalCode, cardProperties.ApprovalCode, SecurityLevel.None);
            authResposneProperties.Add(property);

            property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.AuthorizationResult, "Success", SecurityLevel.None);
            authResposneProperties.Add(property);


            List<PaymentProperty> level2Properties = new List<PaymentProperty>();
            var level2Property = new PaymentProperty(GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.Properties, propertyList: authResposneProperties.ToArray());
            level2Properties.Add(level2Property);


            Response tokenizeResponse = new Response { Properties = requestProperties.ToArray(), Locale = "en-US" };
            Response authResponse = new Response { Properties = level2Properties.ToArray(), Locale = "en-US" };

            Response combinedResponse = this.CombineResponses(tokenizeResponse, authResponse, null, null);

            string authBlob = PaymentProperty.ConvertPropertyArrayToXML(combinedResponse.Properties);
            return authBlob;
        }
        #endregion
        private Response CombineResponses(Response tokenizeResponse, Response authorizeResponse, Response captureResponse, Response voidResponse)
        {
            Response paymentResponse = new Response();
            var properties = new List<PaymentProperty>();
            var errors = new List<Microsoft.Dynamics.Retail.PaymentSDK.Portable.PaymentError>();

            if (tokenizeResponse != null)
            {
                // Start with tokenize response
                paymentResponse.Locale = tokenizeResponse.Locale;

                if (tokenizeResponse.Properties != null)
                {
                    properties.AddRange(tokenizeResponse.Properties);
                }

                if (tokenizeResponse.Errors != null)
                {
                    errors.AddRange(tokenizeResponse.Errors);
                }

                // Merge with authorize response
                if (authorizeResponse != null)
                {
                    if (authorizeResponse.Properties != null)
                    {
                        var authorizeResponseProperties = PaymentProperty.ConvertToHashtable(authorizeResponse.Properties);
                        PaymentProperty innerAuthorizeResponseProperty = PaymentProperty.GetPropertyFromHashtable(
                            authorizeResponseProperties,
                            GenericNamespace.AuthorizationResponse,
                            AuthorizationResponseProperties.Properties);
                        properties.Add(innerAuthorizeResponseProperty);
                    }

                    if (authorizeResponse.Errors != null)
                    {
                        errors.AddRange(authorizeResponse.Errors);
                    }
                }
            }
            else if (authorizeResponse != null)
            {
                // Start with Authorize response
                paymentResponse.Locale = authorizeResponse.Locale;

                if (authorizeResponse.Properties != null)
                {
                    properties.AddRange(authorizeResponse.Properties);
                }

                if (authorizeResponse.Errors != null)
                {
                    errors.AddRange(authorizeResponse.Errors);
                }
            }

            // Merge with authorize response
            if (captureResponse != null)
            {
                if (captureResponse.Properties != null)
                {
                    var captureResponseProperties = PaymentProperty.ConvertToHashtable(captureResponse.Properties);
                    PaymentProperty innerCaptureResponseProperty = PaymentProperty.GetPropertyFromHashtable(
                        captureResponseProperties,
                        GenericNamespace.CaptureResponse,
                        CaptureResponseProperties.Properties);
                    properties.Add(innerCaptureResponseProperty);
                }

                if (captureResponse.Errors != null)
                {
                    errors.AddRange(captureResponse.Errors);
                }
            }

            // Merge with void response
            if (voidResponse != null)
            {
                if (voidResponse.Properties != null)
                {
                    var voidResponseProperties = PaymentProperty.ConvertToHashtable(voidResponse.Properties);
                    PaymentProperty innerVoidResponseProperty = PaymentProperty.GetPropertyFromHashtable(
                        voidResponseProperties,
                        GenericNamespace.VoidResponse,
                        VoidResponseProperties.Properties);
                    properties.Add(innerVoidResponseProperty);
                }

                if (voidResponse.Errors != null)
                {
                    errors.AddRange(voidResponse.Errors);
                }
            }

            if (properties.Count > 0)
            {
                paymentResponse.Properties = properties.ToArray();
            }

            if (errors.Count > 0)
            {
                paymentResponse.Errors = errors.ToArray();
            }

            return paymentResponse;
        }
        public List<ERPPaymentConnectorResponse> GetPaymentConnectorInfo(string requestId)
        {
            try
            {
                var rsResponse = ECL_TV_GetMerchantConnectorInformation();
                List<ERPPaymentConnectorResponse> eprPaymentConnector = new List<ERPPaymentConnectorResponse>();
                if ((bool)rsResponse.Status)
                {
                    eprPaymentConnector = JsonConvert.DeserializeObject<List<ERPPaymentConnectorResponse>>(rsResponse.Result);
                }
                else
                {
                    throw new CommerceLinkError(rsResponse.Message);
                }
                return eprPaymentConnector;
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch (Exception ex)
            {
                var message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }
        }
        #endregion

        #region PoC Methods
        private List<PaymentProperty> GetCommonPaymentRequestProperties(ErpPaymentCard cardProperties)
        {
            var requestProperties = new List<PaymentProperty>();

            PaymentProperty property;

            property = new PaymentProperty(
    GenericNamespace.PaymentCard,
                PaymentCardProperties.CardEntryType,
                CardEntryTypes.ManuallyEntered.ToString(), SecurityLevel.None);
            requestProperties.Add(property);


            property = new PaymentProperty(
    GenericNamespace.PaymentCard,
                PaymentCardProperties.CardType,
                cardProperties.CardTypes, SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(
      GenericNamespace.PaymentCard,
                    PaymentCardProperties.CardNumber,
                    cardProperties.CardNumber, SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(
    GenericNamespace.PaymentCard,
                PaymentCardProperties.ExpirationMonth,
                cardProperties.ExpirationMonth.ToString(), SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(
    GenericNamespace.PaymentCard,
                PaymentCardProperties.ExpirationYear,
                cardProperties.ExpirationYear.ToString(), SecurityLevel.None);
            requestProperties.Add(property);

            if (!string.IsNullOrWhiteSpace(cardProperties.CCID))
            {
                property = new PaymentProperty(
      GenericNamespace.PaymentCard,
                    PaymentCardProperties.AdditionalSecurityData,
                    cardProperties.CCID, SecurityLevel.None);
                requestProperties.Add(property);
            }

            property = new PaymentProperty(
    GenericNamespace.PaymentCard,
                PaymentCardProperties.Name,
                cardProperties.NameOnCard, SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(
    GenericNamespace.PaymentCard,
                PaymentCardProperties.StreetAddress,
                cardProperties.Address1, SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(
    GenericNamespace.PaymentCard,
                PaymentCardProperties.City,
                cardProperties.City, SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(
    GenericNamespace.PaymentCard,
                PaymentCardProperties.State,
                cardProperties.State, SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(
    GenericNamespace.PaymentCard,
                PaymentCardProperties.PostalCode,
                cardProperties.Zip, SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(
    GenericNamespace.PaymentCard,
                PaymentCardProperties.Country,
                cardProperties.Country, SecurityLevel.None);
            requestProperties.Add(property);

            return requestProperties;
        }
        private void Authorize(IPaymentProcessor processor, List<PaymentProperty> requestProperties, ErpCardAuthorizeRequest erpCardAuthorizeRequest)
        {

            // Authorize and Capture if requested
            // Do not authorize if tokenization failed.
            Response authorizeResponse = null;

            PaymentProperty property = null;

            property = new PaymentProperty(
                GenericNamespace.TransactionData,
                TransactionDataProperties.Amount,
               erpCardAuthorizeRequest.Amount, SecurityLevel.None);
            requestProperties.Add(property);

            property = new PaymentProperty(
              GenericNamespace.TransactionData,
              TransactionDataProperties.CurrencyCode,
             "USD", SecurityLevel.None);
            requestProperties.Add(property);

            // Authorize payment
            var paymentRequest = new Request();
            paymentRequest.Locale = defaultLocale;
            paymentRequest.Properties = requestProperties.ToArray();

            authorizeResponse = processor.Authorize(paymentRequest, null);

            if (authorizeResponse.Errors != null && authorizeResponse.Errors.Any())
            {
                // Authorization failure, Throw an exception and stop the payment.
                throw new CardPaymentException("Authorization failure.");
            }

            // Check authorization result
            bool isAuthorizeFailed = false;
            PaymentProperty innerAuthorizeResponseProperty = null;
            Hashtable innerAuthorizeResponseProperties = null;
            if (authorizeResponse == null
                || authorizeResponse.Properties == null
                || (authorizeResponse.Errors != null && authorizeResponse.Errors.Length > 0))
            {
                isAuthorizeFailed = true;
            }
            else
            {
                var authorizeResponseProperties = PaymentProperty.ConvertToHashtable(authorizeResponse.Properties);
                innerAuthorizeResponseProperty = PaymentProperty.GetPropertyFromHashtable(authorizeResponseProperties, GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.Properties);

                innerAuthorizeResponseProperties = PaymentProperty.ConvertToHashtable(innerAuthorizeResponseProperty.PropertyList);

                string authorizationResult = null;
                PaymentProperty.GetPropertyValue(innerAuthorizeResponseProperties, GenericNamespace.AuthorizationResponse, AuthorizationResponseProperties.AuthorizationResult, out authorizationResult);

                // TO DO: In this sample, we only check the authorization results. CVV2 result and AVS result are ignored. 
                if (!AuthorizationResult.Success.ToString().Equals(authorizationResult, StringComparison.OrdinalIgnoreCase)
                    && !AuthorizationResult.PartialAuthorization.ToString().Equals(authorizationResult, StringComparison.OrdinalIgnoreCase))
                {
                    isAuthorizeFailed = true;
                }
            }

            if (!isAuthorizeFailed)
            {
                // Authorize success or partial authorization success...
                // Get authorized amount
                decimal authorizedAmount = 0m;
                PaymentProperty.GetPropertyValue(
                    innerAuthorizeResponseProperties,
                    GenericNamespace.AuthorizationResponse,
                    AuthorizationResponseProperties.ApprovedAmount,
                    out authorizedAmount);

            }
            else
            {
                // Authorization failure, Throw an exception and stop the payment.
                var errors = new List<Microsoft.Dynamics.Retail.PaymentSDK.Portable.PaymentError>();
                errors.Add(new Microsoft.Dynamics.Retail.PaymentSDK.Portable.PaymentError(ErrorCode.AuthorizationFailure, "Authorization failure."));
                throw new CardPaymentException("Authorization failure.");
            }
        }
        private Response Tokenize(IPaymentProcessor processor, List<PaymentProperty> requestProperties)
        {
            var paymentRequest = new Request();
            paymentRequest.Locale = defaultLocale;
            paymentRequest.Properties = requestProperties.ToArray();
            Response tokenizeResponse = processor.GenerateCardToken(paymentRequest, null);
            //if (tokenizeResponse.Errors != null && tokenizeResponse.Errors.Any())
            //{
            //    // Tokenization failure, Throw an exception and stop the payment.
            //    throw new CardPaymentException("Tokenization failure.", tokenizeResponse.Errors);
            //}

            return tokenizeResponse;
        }
        /// <summary>
        /// Combines multiple arrays of payment properties.
        /// </summary>
        /// <param name="propertyArray1">The first array.</param>
        /// <param name="propertyArray2">The second array.</param>
        /// <param name="morePropertyArrays">More arrays.</param>
        /// <returns>The combined array.</returns>
        public PaymentProperty[] CombineProperties(PaymentProperty[] propertyArray1, PaymentProperty[] propertyArray2, params PaymentProperty[][] morePropertyArrays)
        {
            var properties = new List<PaymentProperty>();

            if (propertyArray1 != null)
            {
                properties.AddRange(propertyArray1);
            }

            if (propertyArray2 != null)
            {
                properties.AddRange(propertyArray2);
            }

            if (morePropertyArrays != null)
            {
                foreach (var propertyArray in morePropertyArrays)
                {
                    if (propertyArray != null)
                    {
                        properties.AddRange(propertyArray);
                    }
                }
            }

            if (properties.Count == 0)
            {
                return null;
            }
            else
            {
                return properties.ToArray();
            }
        }
        public void GetCardPaymentAcceptPoint()
        {
            string hostPageOrigin = "https://localhost/";
            EdgeAXCommerceLink.RetailProxy.Extensions.ICardPaymentManager paymentManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICardPaymentManager>();
            var res = Task.Run(async () => await paymentManager.ECL_GetCardPaymentAcceptPoint(0, new CardPaymentAcceptSettings
            {
                CardTokenizationEnabled = true,
                CardPaymentEnabled = false,
                HostPageOrigin = hostPageOrigin,
                AdaptorPath = hostPageOrigin + "/Connectors/"
            })).Result;

        }
        #endregion

        #region RetailServer API
        [Trace]
        private GetMerchantConnectorInformationResponse ECL_TV_GetMerchantConnectorInformation()
        {
            EdgeAXCommerceLink.RetailProxy.Extensions.ICardPaymentManager productManager =
                RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICardPaymentManager>();
            var rsResponse = Task.Run(async () => await productManager.ECL_TV_GetMerchantConnectorInformation()).Result;
            return rsResponse;
        }
        
        #endregion
    }
}
