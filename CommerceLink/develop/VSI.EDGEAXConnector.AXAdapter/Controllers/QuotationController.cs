//using Autofac;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.BoletoPayment;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class QuotationController : BaseController, IQuotationController
    {
        public QuotationController(string storeKey) : base(storeKey)
        {

        }
        public ErpGetCustomerQuotationResponse GetCustomerQuotation(string custAccount, string status, string offerType, string quotationId, string storeKey, string requestId)
        {
            var quotationManager = new QuotationCRTManager();
            ErpGetCustomerQuotationResponse quotationResponse = quotationManager.GetCustomerQuotation(custAccount, status, offerType, quotationId, storeKey, requestId);

            /// As per discussion with Wajhe and Khurrum, only current discounts will be shown 
            /// on quotation as they were displayed earlier i.e. after previous deployment
            // Remove all invalid discount lines from all quotations
            // SelectActiveDiscounts(quotationResponse); // Un-Comment this line to enable discount filter

            LanguageCodesDAL languageCodes = null;
            StoreCodesDAL storeCodesDAL = null;
            string storeKeyByCountryCode = string.Empty;
            string erpLanguage = string.Empty;
            string ecomLanguage = string.Empty;
            string ecomSiteCode = string.Empty;
            string threeLetterISORegion = string.Empty;

            if (quotationResponse.Quotations != null && quotationResponse.Quotations.Count > 0)
            {
                foreach (var quotation in quotationResponse.Quotations)
                {
                    threeLetterISORegion = quotation.ThreeLetterISORegionName;
                    storeKeyByCountryCode = string.IsNullOrWhiteSpace(threeLetterISORegion) ? storeKey : GetStoreKeyByCountryCode(quotation.ThreeLetterISORegionName);
                    IntegrationManager integrationManager = new IntegrationManager(storeKeyByCountryCode);

                    if (quotation.Items != null && quotation.Items.Count > 0)
                    {
                        foreach (var item in quotation.Items)
                        {
                            var result = integrationManager.GetKeyByDescription(Entities.Product, item.ITEMID + ":" + item.VariantId);

                            if (result != null)
                            {
                                item.ITEMID = result.ComKey;
                            }
                        }
                    }

                    languageCodes = new LanguageCodesDAL(storeKeyByCountryCode);
                    storeCodesDAL = new StoreCodesDAL(storeKeyByCountryCode);

                    erpLanguage = quotation.Language;
                    ecomLanguage = languageCodes.GetEcomLanguageCode(erpLanguage);
                    ecomSiteCode = storeCodesDAL.GetEcomStoreCode(erpLanguage);
                    quotation.Language = string.IsNullOrWhiteSpace(ecomLanguage) ? erpLanguage : ecomLanguage;
                    quotation.SiteCode = string.IsNullOrWhiteSpace(ecomSiteCode) ? erpLanguage : ecomSiteCode;
                }
            }

            //swap language for customer
            threeLetterISORegion = string.Empty;

            if (quotationResponse.Customer != null &&
                quotationResponse.Customer.Addresses != null &&
                quotationResponse.Customer.Addresses.Count > 0)
            {
                threeLetterISORegion = quotationResponse.Customer.Addresses.Where(s => s.IsPrimary = true).FirstOrDefault().ThreeLetterISORegionName;
                storeKeyByCountryCode = string.IsNullOrWhiteSpace(threeLetterISORegion) ? storeKey : GetStoreKeyByCountryCode(threeLetterISORegion);
                languageCodes = new LanguageCodesDAL(storeKeyByCountryCode);
                erpLanguage = quotationResponse.Customer.Language;
                ecomLanguage = languageCodes.GetEcomLanguageCode(erpLanguage);
                quotationResponse.Customer.Language = string.IsNullOrWhiteSpace(ecomLanguage) ? erpLanguage : ecomLanguage;
            }

            //swap language for contact person
            threeLetterISORegion = string.Empty;
            if (quotationResponse.ContactPerson != null &&
                quotationResponse.ContactPerson.Addresses != null)
            {
                threeLetterISORegion = quotationResponse.ContactPerson.Addresses[0].ThreeLetterISORegionName;
                storeKeyByCountryCode = string.IsNullOrWhiteSpace(threeLetterISORegion) ? storeKey : GetStoreKeyByCountryCode(threeLetterISORegion);
                languageCodes = new LanguageCodesDAL(storeKeyByCountryCode);
                erpLanguage = quotationResponse.ContactPerson.Language;
                ecomLanguage = languageCodes.GetEcomLanguageCode(erpLanguage);
                quotationResponse.ContactPerson.Language = string.IsNullOrWhiteSpace(ecomLanguage) ? erpLanguage : ecomLanguage;
            }

            return quotationResponse;
        }

        private ErpGetCustomerQuotationResponse UpdateQuotationAddressType(ErpGetCustomerQuotationResponse erpGetCustomerQuotationResponse)
        {
            if (erpGetCustomerQuotationResponse.Quotations != null && erpGetCustomerQuotationResponse.Quotations.Count > 0)
            {
                foreach (ErpCustomerOrderInfo quotation in erpGetCustomerQuotationResponse.Quotations)
                {
                    if (quotation.Addresses != null && quotation.Addresses.Count > 0)
                    {
                        foreach (ErpAddress address in quotation.Addresses)
                        {
                            if (string.IsNullOrEmpty(address.AddressType.ToString()))
                            {
                                if (address.IsPrimary)
                                {
                                    address.AddressType = (ErpAddressType)configurationHelper.GetSetting(SALESORDER.AX_Invoice_Address_Type).IntValue();
                                    address.AddressTypeValue = configurationHelper.GetSetting(SALESORDER.AX_Invoice_Address_Type).IntValue();
                                }
                                else
                                {
                                    address.AddressType = (ErpAddressType)configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue();
                                    address.AddressTypeValue = configurationHelper.GetSetting(SALESORDER.AX_Delivery_Address_Type).IntValue();
                                }
                            }
                        } 
                    }
                }
            }

            return erpGetCustomerQuotationResponse;
        }


        public ErpCreateCustomerQuotationResponse CreateCustomerQuotation(ErpCustomerOrderInfo customerQuotation, string storeKey,string requestId)
        {
            var quotationManager = new QuotationCRTManager();
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

            foreach (var item in customerQuotation.Items)
            {
                var key = integrationManager.GetErpKey(Entities.Product, item.ItemId);

                if (key == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL30203, currentStore, item.ItemId);
                    message += item.ItemId;
                    throw new CommerceLinkError(message);
                }
                try
                {
                    var temp = key.Description.Split(':');

                    if (temp != null && temp.Any())
                    {
                        item.ItemId = temp[0];
                        
                        if (temp.Length > 1)
                        {
                            item.VariantId = temp[1];
                        }
                    }
                }
                catch (Exception)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40504, currentStore);
                    throw new CommerceLinkError(message);
                }

                item.DeliveryMode = configurationHelper.GetSetting(SALESORDER.AX_Default_Delivery_Mode);
            }
            return quotationManager.CreateCustomerQuotation(customerQuotation,storeKey, requestId);
        }

        public ERPQuotationReasonGroupsResponse GetQuotationReasonGroups(string storeKey)
        {
            var quotationManager = new QuotationCRTManager();
            return quotationManager.GetQuotationReasonGroups( storeKey);
        }

        public ErpConfirmCustomerQuotationResponse ConfirmCustomerQuotation(string quotationId, string storeKey, string requestId)
        {
            var quotationManager = new QuotationCRTManager();
            return quotationManager.ConfirmCustomerQuotation(quotationId, storeKey,requestId);
        }

        public ErpRejectCustomerQuotationResponse RejectCustomerQuotation(string quotationId, string reasonCode, string storeKey)
        {
            var quotationManager = new QuotationCRTManager();
            return quotationManager.RejectCustomerQuotation(quotationId, reasonCode, storeKey);
        }

        public List<ErpCreateCustomerQuotationResponse> CreateCustomerQuotations(List<ErpCustomerOrderInfo> customerQuotations, string storeKey)
        {
            var quotationManager = new QuotationCRTManager();
            return quotationManager.CreateCustomerQuotations(customerQuotations, storeKey);
        }

        private string GetStoreKeyByCountryCode(string threeLetterISORegionName)
        {
            var appSettingsDAL = new ApplicationSettingsDAL(currentStore.StoreKey);
            var storeId = appSettingsDAL.GetStoreId(threeLetterISORegionName);
            var store = StoreService.GetStoreById(storeId);

            if (store == null)
                throw new CommerceLinkError("Store not found with store Id '" + store.StoreId + "'");

            return store.StoreKey;
        }

        private void SelectActiveDiscounts(ErpGetCustomerQuotationResponse quotations)
        {

            foreach (ErpCustomerOrderInfo quotation in quotations.Quotations)
            {
                foreach (ErpItemInfo quotationLine in quotation.Items)
                {
                    List<ErpDiscountInfo> activeDiscountList = null;

                    foreach (ErpDiscountInfo discount in quotationLine.Discounts)
                    {
                        if (
                            (discount.ContractValidTo >= DateTime.Now.Date || discount.ContractValidTo == null) ||
                            (discount.ValidTo >= DateTime.Now.Date || discount.ValidTo == null)
                           )
                        {
                            if (activeDiscountList == null)
                            {
                                activeDiscountList = new List<ErpDiscountInfo>();
                            }
                            activeDiscountList.Add(discount);
                        }
                    }

                    quotationLine.Discounts.Clear();
                    if (activeDiscountList != null)
                    {
                        activeDiscountList.ForEach(d => quotationLine.Discounts.Add(d));
                    }
                    
                }
            }

            /*
            foreach (ErpSalesOrder salesOrder in salesOrders)
            {
                foreach (ErpSalesLine salesLine in salesOrder.SalesLines)
                {
                    List<ErpDiscountLine> activeDiscountList = null;

                    foreach (ErpDiscountLine discountLine in salesLine.DiscountLines)
                    {
                        if (discountLine.ContractValidTo >= Convert.ToDateTime(salesLine.TMVContractValidTo) ||
                                discountLine.ContractValidTo == null
                            )
                        {
                            if (activeDiscountList == null)
                            {
                                activeDiscountList = new List<ErpDiscountLine>();
                            }
                            activeDiscountList.Add(discountLine);
                        }
                    }

                    salesLine.DiscountLines = activeDiscountList;
                }
            }
            */
        }

        public ErpConfirmQuotationResponse ConfirmQuotation(ErpConfirmQuotationRequest request, string requestId)
        {
            if (request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.BOLETO.ToString()))
            {
                SetupBoletoProperties(request);
                var salesOrderManager = new QuotationCRTManager();
                return salesOrderManager.ConfirmQuotation(request, currentStore.StoreKey, requestId);
            }
            //Generating Blob for new payment
            else if (!string.IsNullOrEmpty(request.TenderLine.MaskedCardNumber) || 
                     request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.PURCHASEORDER.ToString()) || 
                     request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.SEPA.ToString()) ||
                     request.TenderLine.TenderTypeId.ToUpper().Equals(PaymentCon.ADYEN_HPP.ToString())
                    )
            {
                SalesOrderHelper soHelper = new SalesOrderHelper(currentStore.StoreKey);
                soHelper.SetupPaymentMethod(request.TenderLine, request.SalesOrder, requestId);

                var salesOrderManager = new QuotationCRTManager();
                return salesOrderManager.ConfirmQuotation(request, currentStore.StoreKey, requestId);
            }
            else
            {
                return new ErpConfirmQuotationResponse(false, "MaskedCardNumber is Missing in tenderLine object", string.Empty);
            }
        }

        public ErpQuoteOpportunityUpdateResponse QuoteOpportunityUpdate(ErpQuoteOpportunityUpdateRequest erpRequest)
        {
            var quotationManager = new QuotationCRTManager();
            return quotationManager.QuoteOpportunityUpdate(erpRequest, currentStore.StoreKey);
        }

        private void SetupBoletoProperties(ErpConfirmQuotationRequest request)
        {
            request.TenderLine.BoletoXml = ConvertBoletoToXmlString(request.TenderLine.Boleto);

            PaymentMethodDAL paymentMethodDAL = new PaymentMethodDAL(currentStore.StoreKey);
            PaymentMethod paymentMethod = paymentMethodDAL.GetPaymentMothodByEcommerceKey(request.TenderLine.TenderTypeId);

            if(paymentMethod == null)
            {
                throw new CommerceLinkError("Payment method not found.");
            }

            request.TenderLine.TenderTypeId = paymentMethod.ErpValue;

        }

        private string ConvertBoletoToXmlString(Boleto boleto)
        {
            //33415 -Start - Convert Customer due date format DD/MM/YYYY to YYYY-MM-DD
            if (!string.IsNullOrEmpty(boleto.CustomParameters.Custom_due_date))
            {
                if (DateTime.TryParseExact(boleto.CustomParameters.Custom_due_date,
                                            "d/M/yyyy",
                                            CultureInfo.InvariantCulture,
                                            DateTimeStyles.None,
                    out DateTime customerDueDate))
                {
                    boleto.CustomParameters.Custom_due_date = customerDueDate.ToString("yyyy-MM-dd");
                }
                else
                {
                    throw new CommerceLinkError(string.Format("Invalid Boleto Custom due date {0}", boleto.CustomParameters.Custom_due_date));
                }
            }
            //33415 -End - Convert Customer due date format DD/MM/YYYY to YYYY-MM-DD

            var boletoXml = string.Empty;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Boleto));

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, boleto);
                boletoXml = textWriter.ToString();
            }
            return boletoXml;
        }
    }
}
