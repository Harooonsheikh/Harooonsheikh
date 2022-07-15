using EdgeAXCommerceLink.RetailProxy.Extensions;
using Microsoft.Dynamics.Commerce.Runtime.Services.CustomerOrder;
using NewRelic.Api.Agent;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class QuotationController : BaseController, IQuotationController
    {
        public QuotationController(string storeKey) : base(storeKey)
        {

        }
        public ErpCreateCustomerQuotationResponse CreateCustomerQuotation(ErpCustomerOrderInfo customerQuotation, string requestId = "")
        {
            ErpCreateCustomerQuotationResponse erpResponse = new ErpCreateCustomerQuotationResponse(false, "", "");

            try
            {
                CustomerOrderInfo customerQuotInfo = new CustomerOrderInfo();
                // Map returned ErpCustomerOrderInfo object to CustomerOrderInfo object
                customerQuotInfo = _mapper.Map<ErpCustomerOrderInfo, CustomerOrderInfo>(customerQuotation);

                // customerQuotInfo.ChannelRecordId = customerQuotInfo.StoreId = baseChannelId.ToString();
                string customerOrderInfoJsonString = JsonConvert.SerializeObject(customerQuotInfo);

                timer = Stopwatch.StartNew();
                var returnQuotation = ECL_CreateCustomerQuotation(customerOrderInfoJsonString);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateCustomerQuotation", GetElapsedTime());

                if ((bool)returnQuotation.Status)
                {
                    erpResponse = new ErpCreateCustomerQuotationResponse(true, returnQuotation.Message, returnQuotation.Result);
                }
                else
                {
                    erpResponse = new ErpCreateCustomerQuotationResponse(false, returnQuotation.Message, "");
                }
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }
            return erpResponse;
        }

        private bool IsTrue(object value)
        {
            if (value.ToString().ToUpper().Equals("1") || value.ToString().ToUpper().Equals("TRUE") || value.ToString().ToUpper().Equals("YES"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ErpGetCustomerQuotationResponse GetCustomerQuotation(string custAccount, string status, string offerType, string quotationId, string requestId)
        {
            bool isExternalSystemTimeLogged = false;
            ErpGetCustomerQuotationResponse erpResponse = new ErpGetCustomerQuotationResponse(false, "", null, null, null);

            try
            {

                timer = Stopwatch.StartNew();
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_TV_GetCustomerQuotation", DateTime.UtcNow);
                var returnQuotation = ECL_TV_GetCustomerQuotation(custAccount, status, offerType, quotationId);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "GetCustomerQuotation", GetElapsedTime());
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_TV_GetCustomerQuotation", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;
                if ((bool)returnQuotation.Status)
                {
                    List<ErpCustomerOrderInfo> erpCustomerOrders = new List<ErpCustomerOrderInfo>();

                    List<CustomerOrder> customerOrders = JsonConvert.DeserializeObject<List<CustomerOrder>>(returnQuotation.Quotation);

                    foreach (var order in customerOrders)
                    {
                        ErpCustomerOrderInfo erpCustomerOrderInfo = new ErpCustomerOrderInfo();
                        erpCustomerOrderInfo.CustomerAccount = order.CustomerAccount;
                        erpCustomerOrderInfo.CustomerRecordId = order.CustomerRecordId;
                        erpCustomerOrderInfo.Id = order.Id;
                        erpCustomerOrderInfo.AddressRecordId = order.AddressRecord;
                        erpCustomerOrderInfo.InventSiteId = order.InventSiteId;
                        erpCustomerOrderInfo.Comment = order.Comment;
                        erpCustomerOrderInfo.Status = Convert.ToInt16(order.Status);
                        erpCustomerOrderInfo.Email = order.Email;
                        erpCustomerOrderInfo.CreationDateString = order.CreationDate;
                        erpCustomerOrderInfo.TotalManualDiscountAmount = string.IsNullOrEmpty(order.TotalManualDiscountAmount) ? 0 : Convert.ToDecimal(order.TotalManualDiscountAmount);
                        erpCustomerOrderInfo.TotalManualDiscountPercentage = string.IsNullOrEmpty(order.TotalManualDiscountPercentage) ? 0 : Convert.ToDecimal(order.TotalManualDiscountPercentage);
                        erpCustomerOrderInfo.ChannelReferenceId = order.ChannelReferenceId;
                        erpCustomerOrderInfo.CommissionSalesGroup = order.CommissionSalesGroup;
                        erpCustomerOrderInfo.SalespersonName = order.SalespersonName;
                        erpCustomerOrderInfo.SalespersonStaffId = order.SalespersonStaffId;
                        erpCustomerOrderInfo.StoreId = order.StoreId;
                        erpCustomerOrderInfo.IsTaxIncludedInPrice = order.IsTaxIncludedInPrice;
                        erpCustomerOrderInfo.CurrencyCode = order.CurrencyCode;
                        erpCustomerOrderInfo.DeliveryMode = order.DeliveryMode;
                        erpCustomerOrderInfo.RequestedDeliveryDateString = order.RequestedDeliveryDate;
                        erpCustomerOrderInfo.ExpiryDateString = order.QuotationExpiryDate;
                        erpCustomerOrderInfo.HasLoyaltyPayment = string.IsNullOrEmpty(order.HasLoyaltyPayment) ? false : Convert.ToBoolean(order.HasLoyaltyPayment);
                        erpCustomerOrderInfo.TMVMAINOFFERTYPE = order.TMVMainOfferType;
                        erpCustomerOrderInfo.TMVPRODUCTFAMILY = order.TMVProductFamily;
                        erpCustomerOrderInfo.TMVSALESORDERSUBTYPE = order.TMVSalesOrderSubType;
                        erpCustomerOrderInfo.TMVOFFERTYPE = order.TMVOfferType;
                        erpCustomerOrderInfo.TMVBILLINGINTERVAL = order.TMVBillingInterval;
                        erpCustomerOrderInfo.TMVCONTRACTENDDATE = order.TMVContractEndDate;
                        erpCustomerOrderInfo.SHIPPINGDATEREQUESTED = order.ShippingDateRequested;
                        erpCustomerOrderInfo.CONTACTPERSONID = order.ContactPersonId;
                        erpCustomerOrderInfo.PURCHORDERFORMNUM = order.PurchOrderFormNum;
                        erpCustomerOrderInfo.SourceId = order.SourceId;
                        erpCustomerOrderInfo.QuotationId = order.Id;
                        erpCustomerOrderInfo.ReturnReasonCodeId = order.ReasonId;
                        erpCustomerOrderInfo.SALESID = order.SalesId;
                        erpCustomerOrderInfo.SubtotalAmount = order.SubtotalAmount;
                        erpCustomerOrderInfo.TotalLineDiscount = order.TotalLineDiscount;
                        erpCustomerOrderInfo.TotalAmount = order.TotalAmount;
                        erpCustomerOrderInfo.TotalSalesTax = order.TotalSalesTax;
                        erpCustomerOrderInfo.Language = order.Language;
                        erpCustomerOrderInfo.RetailChannel = order.RetailChannel;
                        erpCustomerOrderInfo.ThreeLetterISORegionName = order.ThreeLetterISORegionName;

                        // VSTS 33525 Begin
                        erpCustomerOrderInfo.TMVResellerAccount = order.TMVResellerAccount;
                        erpCustomerOrderInfo.TMVDistributorAccount = order.TMVDistributorAccount;
                        erpCustomerOrderInfo.TMVIndirectCustomer = order.TMVIndirectCustomer;
                        erpCustomerOrderInfo.TMVCommentForQuote = order.Comment;
                        erpCustomerOrderInfo.TMVCustomerReference = order.TMVCustomerReference;
                        // VSTS 33525 End

                        // VSTS 55353 Begin
                        erpCustomerOrderInfo.TMVLoginEmail = order.TMVLoginEmail;
                        // VSTS 55353 End

                        erpCustomerOrderInfo.Addresses = new List<ErpAddress>();

                        if (order.Addresses != null)
                        {
                            foreach (var address in order.Addresses.PostalAddress)
                            {
                                ErpAddress erpAddress = new ErpAddress();

                                erpAddress.RecordId = Convert.ToInt64(address.AddressRecId);
                                erpAddress.FullAddress = address.Address;
                                erpAddress.Street = address.Street;
                                erpAddress.Street2 = address.Street2;
                                erpAddress.City = address.City;
                                erpAddress.State = address.State;
                                erpAddress.ZipCode = address.ZipCode;
                                erpAddress.ThreeLetterISORegionName = address.CountryRegion;
                                /*
                                erpAddress.RecordId = Convert.ToInt64(address.LogisticsPostalAddress.RecId);
                                // erpAddress.Location = address.LogisticsPostalAddress.Location;
                                erpAddress.FullAddress = address.LogisticsPostalAddress.Address;
                                erpAddress.ZipCode = address.LogisticsPostalAddress.ZipCode;
                                erpAddress.State = address.LogisticsPostalAddress.State;
                                erpAddress.ThreeLetterISORegionName = address.LogisticsPostalAddress.CountryRegionId;
                                erpAddress.County = address.LogisticsPostalAddress.County;
                                erpAddress.City = address.LogisticsPostalAddress.City;
                                erpAddress.Street = address.LogisticsPostalAddress.Street;
                                // erpAddress.TimeZone = address.LogisticsPostalAddress.TimeZone;
                                erpAddress.StreetNumber = address.LogisticsPostalAddress.StreetNumber;
                                erpAddress.BuildingCompliment = address.LogisticsPostalAddress.BuildingCompliment;
                                erpAddress.DistrictName = address.LogisticsPostalAddress.DistrictName;
                                // erpAddress.ValidFrom = address.LogisticsPostalAddress.ValidFrom;
                                // erpAddress.ValidTo = address.LogisticsPostalAddress.ValidTo;
                                erpAddress.TwoLetterISORegionName = address.LogisticsPostalAddress.ISOcode;

                                //DirPartyLocation
                                erpAddress.DirectoryPartyLocationRecordId = Convert.ToInt64(address.DirPartyLocation.RecId);

                                erpAddress.IsPrimary = Convert.ToBoolean(IsTrue(address.DirPartyLocation.IsPrimary));
                                erpAddress.IsPrivate = Convert.ToBoolean(IsTrue(address.DirPartyLocation.IsPrivate));

                                // Logistics Location
                                erpAddress.LogisticsLocationRecordId = Convert.ToInt64(address.LogisticsLocation.RecId);
                                erpAddress.Name = address.LogisticsLocation.Description;
                                erpAddress.LogisticsLocationId = address.LogisticsLocation.LocationId;


                                erpAddress.AddressType = (ErpAddressType)Convert.ToInt32(address.LogisticsLocationRole.Type);
                                erpAddress.AddressTypeValue = Convert.ToInt32(address.LogisticsLocationRole.Type);
                                erpAddress.AddressTypeStrValue = Enum.GetName(typeof(ErpAddressType), (ErpAddressType)Convert.ToInt32(address.LogisticsLocationRole.Type));
                                
                                */
                                erpCustomerOrderInfo.Addresses.Add(erpAddress);

                            }
                        }

                        erpCustomerOrderInfo.Items = new ObservableCollection<ErpItemInfo>();
                        if (order.Items != null)
                        {
                            foreach (var item in order.Items.Item)
                            {
                                ErpItemInfo erpItemInfo = new ErpItemInfo();
                                //erpCustomerOrderInfo.Items = new ObservableCollection<ErpItemInfo>();

                                erpItemInfo.RecId = string.IsNullOrEmpty(item.RecId) ? 0 : Convert.ToInt64(item.RecId);
                                erpItemInfo.ITEMID = item.ItemId;
                                erpItemInfo.NAME = item.Name;
                                erpItemInfo.LineNumber = string.IsNullOrEmpty(item.LineNumber) ? 0 : Convert.ToDecimal(item.LineNumber);
                                erpItemInfo.INVENTDIMID = item.InventDimId;
                                erpItemInfo.TMVTimeQuantity = item.TMVTimeQuantity;
                                erpItemInfo.Price = string.IsNullOrEmpty(item.SalesPrice) ? 0 : Convert.ToDecimal(item.SalesPrice);
                                erpItemInfo.Quantity = string.IsNullOrEmpty(item.Quantity) ? 0 : Convert.ToDecimal(item.Quantity);
                                erpItemInfo.QuantityPicked = string.IsNullOrEmpty(item.QuantityPicked) ? 0 : Convert.ToDecimal(item.QuantityPicked);
                                erpItemInfo.Status = string.IsNullOrEmpty(item.Status) ? 0 : Convert.ToInt32(item.Status);
                                erpItemInfo.SALESUNIT = item.SalesUnit;
                                erpItemInfo.Discount = string.IsNullOrEmpty(item.Discount) ? 0 : Convert.ToDecimal(item.Discount);
                                erpItemInfo.DiscountPercent = string.IsNullOrEmpty(item.DiscountPercent) ? 0 : Convert.ToDecimal(item.DiscountPercent);
                                erpItemInfo.NetAmount = string.IsNullOrEmpty(item.LineAmount) ? 0 : Convert.ToDecimal(item.LineAmount);
                                erpItemInfo.SalesTaxGroup = item.TaxGroup;
                                erpItemInfo.ItemTaxGroup = item.TaxItemGroup;
                                erpItemInfo.SalesMarkup = string.IsNullOrEmpty(item.SalesMarkup) ? 0 : Convert.ToDecimal(item.SalesMarkup);
                                erpItemInfo.DeliveryMode = item.DeliveryMode;
                                erpItemInfo.AddressRecordId = item.AddressRecord;
                                erpItemInfo.RequestedDeliveryDateString = item.RequestedDeliveryDate;
                                erpItemInfo.VariantId = item.VariantId;
                                erpItemInfo.ColorId = item.InventColorId;
                                erpItemInfo.StyleId = item.InventStyleId;
                                erpItemInfo.Comment = item.Comment;
                                erpItemInfo.CommissionSalesGroup = item.CommissionSalesGroup;
                                erpItemInfo.TMVCONTRACTVALIDFROM = item.TMVContractValidFrom;
                                erpItemInfo.TMVCONTRACTCALCULATEFROM = item.TMVContractCalculateFrom;
                                erpItemInfo.TMVCONTRACTVALIDTO = item.TMVContractValidTo;
                                erpItemInfo.TMVAUTOPROLONGATION = item.TMVAutoProlongation;
                                erpItemInfo.TMVORIGINALLINEAMOUNT = item.TMVOriginalLineAmount;
                                erpItemInfo.TMVPIT = item.TMVPit;
                                erpItemInfo.SerialId = item.SerialId;
                                erpItemInfo.BatchId = item.BatchId;
                                erpItemInfo.SourceId = item.SourceId;
                                erpItemInfo.TMVPARENT = item.TMVParent;
                                erpItemInfo.TotalPrice = item.TotalPrice;
                                erpItemInfo.TMVTargetAmount = item.TMVTargetAmount;

                                SetUpItemDiscount(erpItemInfo, item);
                                SetUpItemCharges(erpItemInfo, item);
                                SetUpItemAttributeValues(erpItemInfo, item);

                                // item.Taxes = new Taxes();
                                if (item.Taxes != null)
                                {
                                    foreach (var taxItem in item.Taxes.Tax)
                                    {
                                        if (taxItem != null)
                                        {
                                            ErpTaxInfo erpTaxInfo = new ErpTaxInfo
                                            {
                                                TaxCode = taxItem.TaxCode,
                                                Amount = string.IsNullOrEmpty(taxItem.Amount) ? 0 : Convert.ToDecimal(taxItem.Amount),
                                                TaxPercentage = string.IsNullOrEmpty(taxItem.TaxPercentage) ? 0 : Convert.ToDecimal(taxItem.TaxPercentage)
                                            };

                                            erpItemInfo.Taxes.Add(erpTaxInfo);
                                        }
                                    }
                                }

                                erpCustomerOrderInfo.Items.Add(erpItemInfo);

                            }
                        }

                        erpCustomerOrderInfo.Taxes = new ObservableCollection<ErpTaxInfo>();
                        if (order.Taxes != null)
                        {
                            foreach (var item in order.Taxes.Tax)
                            {
                                ErpTaxInfo erpItemInfo = new ErpTaxInfo
                                {
                                    TaxCode = item.TaxCode,
                                    Amount = string.IsNullOrEmpty(item.Amount) ? 0 : Convert.ToDecimal(item.Amount)
                                };

                                erpCustomerOrderInfo.Taxes.Add(erpItemInfo);
                            }
                        }

                        erpCustomerOrders.Add(erpCustomerOrderInfo);
                    }

                    erpResponse = new ErpGetCustomerQuotationResponse(true, "", JsonConvert.DeserializeObject<ErpCustomer>(returnQuotation.Customer), JsonConvert.DeserializeObject<ErpContactPerson>(returnQuotation.ContactPerson), erpCustomerOrders);
                }
                else
                {
                    erpResponse = new ErpGetCustomerQuotationResponse((bool)returnQuotation.Status, returnQuotation.Message, null, null, null);
                }
            }
            catch (Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "GetCustomerQuotation", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_GetCustomerQuotation", DateTime.UtcNow);
                }
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }
            return erpResponse;
        }
        public ERPQuotationReasonGroupsResponse GetQuotationReasonGroups()
        {
            ERPQuotationReasonGroupsResponse erpResponse = new ERPQuotationReasonGroupsResponse(false, "", null);

            //try
            //{
            //    List<ERPQuotationReasonGroup> erpQuotationReasonGroupList = new List<ERPQuotationReasonGroup>();
            //    var rsResponse = ECL_TV_GetQuotationReasonGroup();

            //    if (rsResponse.Success)
            //    {
            //        ERPQuotationReasonGroup eRPQuotationReasonGroup = new ERPQuotationReasonGroup();

            //        //++ Deserialize Objects 
            //        List<QuotationReasonGroup> rsQuotationReasonGroup = JsonConvert.DeserializeObject<List<QuotationReasonGroup>>(rsResponse.Result);

            //        // Map returned CustomerOrderInfo object to ErpCustomerOrderInfo object
            //        foreach (var item in rsQuotationReasonGroup)
            //        {
            //            ERPQuotationReasonGroup eRPQuotationReason = new ERPQuotationReasonGroup();
            //            eRPQuotationReason = _mapper.Map<QuotationReasonGroup, ERPQuotationReasonGroup>(item);
            //            erpQuotationReasonGroupList.Add(eRPQuotationReason);
            //        }
            //        erpResponse = new ERPQuotationReasonGroupsResponse(true, "", erpQuotationReasonGroupList);
            //    }
            //    else
            //    {
            //        erpResponse = new ERPQuotationReasonGroupsResponse(erpResponse.Success, erpResponse.Message, null);
            //    }
            //}
            //catch (Exception exp)
            //{
            //    string message = CommerceLinkLogger.LogFatal(Enums.Enums.CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(exp));
            //    erpResponse = new ERPQuotationReasonGroupsResponse(false, message, null);
            //}
            return erpResponse;
        }
        public ErpConfirmCustomerQuotationResponse ConfirmCustomerQuotation(string quotationId, string requestId)
        {
            ErpConfirmCustomerQuotationResponse erpResponse = new ErpConfirmCustomerQuotationResponse(false, "", null);
            //try
            //{
            //    timer = Stopwatch.StartNew();
            //    var rsResponse = ECL_TV_ConfirmCustomerQuotation(quotationId);
            //    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "ConfirmCustomerQuotation", GetElapsedTime());
            //    if (rsResponse.Success)
            //    {
            //        erpResponse = new ErpConfirmCustomerQuotationResponse(true, rsResponse.Result, quotationId);
            //    }
            //    else
            //    {
            //        erpResponse = new ErpConfirmCustomerQuotationResponse(rsResponse.Success, rsResponse.Result, null);
            //    }
            //}
            //catch (Exception exp)
            //{
            //    var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
            //    throw new CommerceLinkError(message);
            //}
            return erpResponse;
        }
        public ErpRejectCustomerQuotationResponse RejectCustomerQuotation(string quotationId, string reasonCode)
        {
            ErpRejectCustomerQuotationResponse erpResponse = new ErpRejectCustomerQuotationResponse(false, "", null);

            try
            {
                var rsResponse = ECL_TV_RejectCustomerQuotation(quotationId, reasonCode);
                if ((bool)rsResponse.Status)
                {

                    erpResponse = new ErpRejectCustomerQuotationResponse(true, rsResponse.Result, quotationId);
                }
                else
                {
                    erpResponse = new ErpRejectCustomerQuotationResponse(false, rsResponse.Message, null);
                }
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, Guid.NewGuid().ToString());
                throw new CommerceLinkError(message);
            }
            return erpResponse;
        }
        /// <summary>
        /// CreateCustomerQuotations creates Quotations for specified list of Quotation objects.
        /// </summary>
        /// <param name="erpCustomerQuotations"></param>
        /// <returns></returns>
        public List<ErpCreateCustomerQuotationResponse> CreateCustomerQuotations(List<ErpCustomerOrderInfo> erpCustomerQuotations)
        {
            List<ErpCreateCustomerQuotationResponse> erpResponse = new List<ErpCreateCustomerQuotationResponse>();

            try
            {
                foreach (var customerQuotInfo in erpCustomerQuotations)
                {
                    customerQuotInfo.ChannelRecordId = customerQuotInfo.StoreId = baseChannelId.ToString();
                    customerQuotInfo.DataAreaId = baseCompany;

                    var returnQuotationResponse = this.CreateCustomerQuotation(customerQuotInfo);

                    if (returnQuotationResponse.Success)
                    {
                        IntegrationManager integrationManager = new IntegrationManager(configurationHelper.currentStore.StoreKey);
                        integrationManager.MarkAbandonedCartProcessed(customerQuotInfo.TransactionId);
                    }
                    else
                    {
                        IntegrationManager integrationManager = new IntegrationManager(configurationHelper.currentStore.StoreKey);
                        integrationManager.MarkAbandonedCartFailedToCreate(customerQuotInfo.TransactionId);
                    }

                    erpResponse.Add(returnQuotationResponse);
                }
            }
            catch (Exception exp)
            {
                string message = CommerceLinkLogger.LogFatal(Enums.Enums.CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(exp));
            }
            return erpResponse;
        }

        #region Private Methods 

        private void SetUpItemDiscount(ErpItemInfo erpItemInfo, Item item)
        {
            if (item.Discounts != null)
            {
                erpItemInfo.Discounts = new ObservableCollection<ErpDiscountInfo>();
                foreach (var discount in item.Discounts.Discount)
                {
                    DateTime? validFrom = null;
                    DateTime? validTo = null;
                    DateTime? contractValidFrom = null;
                    DateTime? contractValidTo = null;
                    if (!string.IsNullOrEmpty(discount.ValidFrom))
                    {
                        validFrom = DateTime.Parse(discount.ValidFrom, CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(discount.ValidTo))
                    {
                        validTo = DateTime.Parse(discount.ValidTo, CultureInfo.InvariantCulture);
                    }
                    if (!string.IsNullOrEmpty(discount.ContractValidFrom))
                    {
                        contractValidFrom = DateTime.Parse(discount.ContractValidFrom, CultureInfo.InvariantCulture);  // discount.ContractValidFrom;
                    }
                    if (!string.IsNullOrEmpty(discount.ContractValidTo))
                    {
                        contractValidTo = DateTime.Parse(discount.ContractValidTo, CultureInfo.InvariantCulture);
                    }
                    erpItemInfo.Discounts.Add(item.Discounts == null ? new ErpDiscountInfo() : new
                        ErpDiscountInfo
                    {
                        Amount = string.IsNullOrEmpty(discount.Amount) ? 0 : Convert.ToDecimal(discount.Amount),
                        DiscountOriginType = string.IsNullOrEmpty(discount.DiscountOriginType) ? 0 : Convert.ToInt16(discount.DiscountOriginType),
                        CustomerDiscountType = string.IsNullOrEmpty(discount.CustomerDiscountType) ? 0 : Convert.ToInt16(discount.CustomerDiscountType),
                        DiscountCode = discount.DiscountCode,
                        PeriodicDiscountOfferId = discount.PeriodicDiscountOfferId,
                        PeriodicDiscountType = string.IsNullOrEmpty(discount.PeriodicDiscountType) ? 0 : Convert.ToInt16(discount.PeriodicDiscountType),
                        DiscountAmount = string.IsNullOrEmpty(discount.Amount) ? 0 : Convert.ToDecimal(discount.DiscountAmount),
                        DealPrice = string.IsNullOrEmpty(discount.DealPrice) ? 0 : Convert.ToDecimal(discount.DealPrice),
                        TMVTargetAmount = string.IsNullOrEmpty(discount.TMVTargetAmount) ? 0 : Convert.ToDecimal(discount.TMVTargetAmount),
                        OfferName = discount.OfferName,
                        ManualDiscountType = string.IsNullOrEmpty(discount.ManualDiscountType) ? 0 : Convert.ToInt16(discount.ManualDiscountType),
                        Percentage = string.IsNullOrEmpty(discount.Percentage) ? 0 : Convert.ToDecimal(discount.Percentage),
                        ValidFrom = validFrom,
                        ValidTo = validTo,
                        ContractValidFrom = contractValidFrom,
                        ContractValidTo = contractValidTo,
                        DiscountMethod = discount.DiscountMethod,
                        TMVPriceOverrideReasonCode = discount.TMVPriceOverrideReasonCode
                    });
                }
            }
        }

        private void SetUpItemCharges(ErpItemInfo erpItemInfo, Item item)
        {
            if (item.Charges != null)
            {
                erpItemInfo.Charges = new ObservableCollection<ErpChargeInfo>();

                foreach (var charge in item.Charges.Charge)
                {
                    erpItemInfo.Charges.Add(item.Charges == null ? new ErpChargeInfo() : new ErpChargeInfo
                    {
                        Amount = string.IsNullOrEmpty(charge.Amount) ? 0 : Convert.ToDecimal(charge.Amount),
                        Code = charge.Code,
                        TaxGroup = charge.TaxGroup,
                        SalesTaxGroup = charge.TaxItemGroup
                    });
                }
            }
        }

        private void SetUpItemAttributeValues(ErpItemInfo erpItemInfo, Item item)
        {
            if (item.AttributeValues != null)
            {
                erpItemInfo.ExtensionProperties = new ObservableCollection<ErpCommerceProperty>();

                foreach (var attr in item.AttributeValues.AttributeValue)
                {
                    erpItemInfo.ExtensionProperties.Add(new ErpCommerceProperty { Key = attr.Name, Value = new ErpCommercePropertyValue { StringValue = attr.TextValue } });
                }
            }
        }

        private string GetPropertyValue(ErpCommercePropertyValue Value)
        {
            var propertyValue = string.Empty;
            if (Value.LongValue != null)
            {
                propertyValue = Value.LongValue.ToString();
            }
            else if (Value.IntegerValue != null)
            {
                propertyValue = Value.IntegerValue.ToString();
            }
            else if (Value.DateTimeOffsetValue != null)
            {
                propertyValue = Value.DateTimeOffsetValue.ToString();
            }
            else if (Value.DecimalValue != null)
            {
                propertyValue = Value.DecimalValue.ToString();
            }
            else if (Value.ByteValue != null)
            {
                propertyValue = Value.ByteValue.ToString();
            }
            else if (Value.BooleanValue != null)
            {
                propertyValue = Value.BooleanValue.ToString();
            }
            else if (Value.StringValue != null)
            {
                propertyValue = Value.StringValue;
            }

            return propertyValue;
        }

        //public Task<QuotationReasonGroup> Create(QuotationReasonGroup entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<QuotationReasonGroup> Read(long rECID, ICollection<string> expandProperties = null)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<Microsoft.Dynamics.Commerce.RetailProxy.PagedResult<QuotationReasonGroup>> ReadAll(Microsoft.Dynamics.Commerce.RetailProxy.QueryResultSettings queryResultSettings, ICollection<string> expandProperties = null)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<QuotationReasonGroup> Update(QuotationReasonGroup entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task Delete(QuotationReasonGroup entity)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<QuotationReasonGroupsResponse> ECL_TV_GetQuotationReasonGroup(string company)
        //{
        //    throw new NotImplementedException();
        //}

        public ErpConfirmQuotationResponse ConfirmQuotation(ErpConfirmQuotationRequest request, string requestId)
        {
            bool isExternalSystemTimeLogged = false;
            ErpConfirmQuotationResponse erpResponse = new ErpConfirmQuotationResponse(false, "", string.Empty); ;

            try
            {
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_TV_ConfirmQuotation", DateTime.UtcNow);
                timer = Stopwatch.StartNew();
                var rsResponse = ECL_TV_ConfirmQuotation(request);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "ConfirmQuotation", GetElapsedTime());
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_TV_ConfirmQuotation", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;

                erpResponse = new ErpConfirmQuotationResponse((bool)rsResponse.Status, rsResponse.Message, rsResponse.ErrorCode);
            }
            catch (Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ConfirmQuotation", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_ConfirmQuotation", DateTime.UtcNow);
                }

                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(exp));
                erpResponse = new ErpConfirmQuotationResponse(false, message, string.Empty);
            }

            return erpResponse;
        }

        public ErpQuoteOpportunityUpdateResponse QuoteOpportunityUpdate(ErpQuoteOpportunityUpdateRequest erpRequest)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpQuoteOpportunityUpdateResponse erpResponse = new ErpQuoteOpportunityUpdateResponse(false, "");
            try
            {
                var rsResponse = ECL_TV_QuoteOpportunityUpdate(erpRequest);
                erpResponse = new ErpQuoteOpportunityUpdateResponse((bool)rsResponse.Status, rsResponse.Message);
            }
            catch (Exception exp)
            {
                string message = CommerceLinkLogger.LogFatal(Enums.Enums.CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(exp));
                erpResponse = new ErpQuoteOpportunityUpdateResponse(false, message);
            }
            return erpResponse;
        }

        #region CustmerOrder

        public class Charge
        {
            public string Amount { get; set; }
            public string Code { get; set; }
            public string TaxGroup { get; set; }
            public string TaxItemGroup { get; set; }
        }

        public class Charges
        {
            public List<Charge> Charge { get; set; }
        }

        public class Discount
        {
            public string Amount { get; set; }
            public string DiscountOriginType { get; set; }
            public string CustomerDiscountType { get; set; }
            public string DiscountCode { get; set; }
            public string ManualDiscountType { get; set; }
            public string PeriodicDiscountOfferId { get; set; }
            public string PeriodicDiscountType { get; set; }
            public string OfferName { get; set; }
            public string DealPrice { get; set; }
            public string TMVTargetAmount { get; set; }
            public string DiscountAmount { get; set; }
            public string Percentage { get; set; }
            public string BundleId { get; set; }
            public string ValidFrom { get; set; }
            public string ValidTo { get; set; }
            public string ContractValidFrom { get; set; }
            public string ContractValidTo { get; set; }
            public string DiscountMethod { get; set; }
            public string TMVPriceOverrideReasonCode { get; set; }
        }

        public class Discounts
        {
            public List<Discount> Discount { get; set; }
        }

        public class AttributeValue
        {
            public string Name { get; set; }
            public string LineNum { get; set; }
            public string TextValue { get; set; }
        }

        public class AttributeValues
        {
            public List<AttributeValue> AttributeValue { get; set; }
        }

        public class Item
        {
            public Charges Charges { get; set; }
            public Discounts Discounts { get; set; }
            public AttributeValues AttributeValues { get; set; }
            public string RecId { get; set; }
            public string ItemId { get; set; }
            public string Name { get; set; }
            public string LineNumber { get; set; }
            public string InventDimId { get; set; }
            public string TMVTimeQuantity { get; set; }
            public string SalesPrice { get; set; }
            public string Quantity { get; set; }
            public string QuantityPicked { get; set; }
            public string Status { get; set; }
            public string SalesUnit { get; set; }
            public string Discount { get; set; }
            public string DiscountPercent { get; set; }
            public string LineAmount { get; set; }
            public string TaxGroup { get; set; }
            public string TaxItemGroup { get; set; }
            public string SalesMarkup { get; set; }
            public string InventLocationId { get; set; }
            public string InventColorId { get; set; }
            public string InventStyleId { get; set; }
            public string DeliveryMode { get; set; }
            public string AddressRecord { get; set; }
            public string RequestedDeliveryDate { get; set; }
            public string VariantId { get; set; }
            public string Comment { get; set; }
            public string CommissionSalesGroup { get; set; }
            public string TMVContractValidFrom { get; set; }
            public string TMVContractCalculateFrom { get; set; }
            public string TMVContractValidTo { get; set; }
            public string TMVContractPossTermDate { get; set; }
            public string TMVContractCancelDate { get; set; }
            public string TMVContractPossCancelDate { get; set; }
            public string TMVContractTermDate { get; set; }
            public string TMVContractTermDateEffective { get; set; }
            public string TMVAutoProlongation { get; set; }
            public string TMVCustomerRef { get; set; }
            public string TMVContractStatusLine { get; set; }
            public string TMVEULAVersion { get; set; }
            public string TMVBillingPeriod { get; set; }
            public string TMVOriginalLineAmount { get; set; }
            public string TMVLineModified { get; set; }
            public string TMVReversedLine { get; set; }
            public string TMVPit { get; set; }
            public string TMVParent { get; set; }
            public string SourceId { get; set; }
            public string TotalPrice { get; set; }
            public string SerialId { get; set; }
            public string BatchId { get; set; }
            public Taxes Taxes { get; set; }
            public string TMVTargetAmount { get; set; }
        }

        public class Items
        {
            public List<Item> Item { get; set; }
        }

        public class CustomerOrder
        {
            public string ReasonId { get; set; }
            public string CustomerAccount { get; set; }
            public string CustomerRecordId { get; set; }
            public string Id { get; set; }
            public string AddressRecord { get; set; }
            public Addresses Addresses { get; set; }
            public string InventLocationId { get; set; }
            public string InventSiteId { get; set; }
            public string Comment { get; set; }
            public string Status { get; set; }
            public string Email { get; set; }
            public string CreationDate { get; set; }
            public string Coupons { get; set; }
            public string TotalManualDiscountAmount { get; set; }
            public string TotalManualDiscountPercentage { get; set; }
            public string ChannelReferenceId { get; set; }
            public string CommissionSalesGroup { get; set; }
            public string SalespersonName { get; set; }
            public string SalespersonStaffId { get; set; }
            public string StoreId { get; set; }
            public string IsTaxIncludedInPrice { get; set; }
            public string CurrencyCode { get; set; }
            public string DeliveryMode { get; set; }
            public string RequestedDeliveryDate { get; set; }
            public string QuotationExpiryDate { get; set; }
            public string DataAreaId { get; set; }
            public string HasLoyaltyPayment { get; set; }
            public string AttributeValues { get; set; }
            public string TMVResellerAccount { get; set; }
            public string TMVDistributorAccount { get; set; }
            public string TMVMainOfferType { get; set; }
            public string TMVProductFamily { get; set; }
            public string TMVSalesOrderSubType { get; set; }
            public string TMVIndirectCustomer { get; set; }
            public string TMVOfferType { get; set; }
            public string TMVBillingInterval { get; set; }
            public string TMVContractEndDate { get; set; }
            public string ShippingDateRequested { get; set; }
            public string ContactPersonId { get; set; }
            public string PurchOrderFormNum { get; set; }
            public string SourceId { get; set; }
            public Items Items { get; set; }
            public string SalesId { get; set; }

            public Taxes Taxes { get; set; }

            public string SubtotalAmount { get; set; }

            public string TotalLineDiscount { get; set; }
            public string TotalAmount { get; set; }
            public string TotalSalesTax { get; set; }
            public string Language { get; set; }
            public string RetailChannel { get; set; }
            public string ThreeLetterISORegionName { get; set; }
            public string TMVCustomerReference { get; set; }
            public string TMVLoginEmail { get; set; }
        }

        public class Tax
        {
            public string RecId { get; set; }

            public string TaxCode { get; set; }

            public string Amount { get; set; }

            public string TaxPercentage { get; set; }
        }

        public class Taxes
        {
            public List<Tax> Tax { get; set; }
        }

        public class DirPartyLocation
        {
            public string RecId { get; set; }
            public string Location { get; set; }
            public string Party { get; set; }
            public string IsPostalAddress { get; set; }
            public string IsPrimary { get; set; }
            public string IsPrivate { get; set; }
            public string IsLocationOwner { get; set; }
            public string ValidFrom { get; set; }
            public string ValidTo { get; set; }
        }

        public class LogisticsLocation
        {
            public string RecId { get; set; }
            public string LocationId { get; set; }
            public string Description { get; set; }
            public string IsPostalAddress { get; set; }
            public string ParentLocation { get; set; }
        }

        public class LogisticsLocationRole
        {
            public string RecId { get; set; }
            public string Name { get; set; }
            public string Type { get; set; }
            public string IsPostalAddress { get; set; }
            public string IsContactInfo { get; set; }
        }

        public class LogisticsPostalAddress
        {
            public string RecId { get; set; }
            public string Location { get; set; }
            public string Address { get; set; }
            public string ZipCode { get; set; }
            public string State { get; set; }
            public string CountryRegionId { get; set; }
            public string County { get; set; }
            public string City { get; set; }
            public string Street { get; set; }
            public string TimeZone { get; set; }
            public string StreetNumber { get; set; }
            public string BuildingCompliment { get; set; }
            public string DistrictName { get; set; }
            public string ValidFrom { get; set; }
            public string ValidTo { get; set; }
            public string ISOcode { get; set; }
        }

        public class PostalAddress
        {
            public DirPartyLocation DirPartyLocation { get; set; }
            public string DirPartyLocationRole { get; set; }
            public LogisticsLocation LogisticsLocation { get; set; }
            public LogisticsLocationRole LogisticsLocationRole { get; set; }
            public LogisticsPostalAddress LogisticsPostalAddress { get; set; }
            public string AddressRecId { get; set; }
            public string Address { get; set; }
            public string Street { get; set; }
            public string Street2 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string ZipCode { get; set; }
            public string CountryRegion { get; set; }
        }

        public class Addresses
        {
            public List<PostalAddress> PostalAddress { get; set; }
        }

        #endregion

        #endregion

        #region RetailServer API
        
        [Trace]
        private QuoteOpportunityUpdateResponse ECL_TV_QuoteOpportunityUpdate(ErpQuoteOpportunityUpdateRequest erpRequest)
        {
            var quotationrManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await quotationrManager.ECL_TV_QuoteOpportunityUpdate(erpRequest.QuoteId, erpRequest.OpportunityId, erpRequest.OpportunityGuid, baseCompany)).Result;
        }
        
        [Trace]
        private CreateCustomerQuotationResponse ECL_CreateCustomerQuotation(string customerOrderInfoJsonString)
        {

            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            return Task.Run(async () => await customerManager.ECL_CreateCustomerQuotation(customerOrderInfoJsonString, baseCompany)).Result;
        }
        
        [Trace]
        private GetCustomerQuotationResponse ECL_TV_GetCustomerQuotation(string custAccount, string status, string offerType,
            string quotationId)
        {
            var customerManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICustomerManager>();
            var returnQuotation = Task.Run(async () =>
                    await customerManager.ECL_TV_GetCustomerQuotation(quotationId, custAccount, status, offerType, baseCompany)).Result;
            return returnQuotation;
        }
        //[Trace]
        //private QuotationReasonGroupsResponse ECL_TV_GetQuotationReasonGroup()
        //{
        //    throw new NotImplementedException();
        //    //var qManager = RPFactory.GetManager<IQuotationReasonGroupManager>();
        //    //var rsResponse = Task.Run(async () => await qManager.ECL_TV_GetQuotationReasonGroup(baseCompany)).Result;
        //    //return rsResponse;
        //}
        //[Trace]
        //private ConfirmCustomerQuotationResponse ECL_TV_ConfirmCustomerQuotation(string quotationId)
        //{
        //    throw new NotImplementedException();
        //    //var quottionManager = RPFactory.GetManager<IQuotationReasonGroupManager>();
        //    //var rsResponse = Task.Run(async () => await quottionManager.ECL_TV_ConfirmCustomerQuotation(quotationId, baseCompany)).Result;
        //    //return rsResponse;
        //}
        
        [Trace]
        private RejectCustomerQuotationResponse ECL_TV_RejectCustomerQuotation(string quotationId, string reasonCode)
        {
            var quottionManager = RPFactory.GetManager<ICustomerManager>();
            return Task.Run(async () => await quottionManager.ECL_TV_RejectCustomerQuotation(quotationId, reasonCode, baseCompany)).Result;
        }
        
        [Trace]
        private ConfirmQuotationResponse ECL_TV_ConfirmQuotation(ErpConfirmQuotationRequest request)
        {

            var _authCode = (request.TenderLine.CardTypeId?.ToUpper() == Enums.CardType.ALIPAY.ToString() || request.TenderLine.CardTypeId?.ToUpper() == Enums.CardType.ALIPAY_HK.ToString())
                            ? request.TenderLine.PspReference
                            : "";

            var salesOrderManager = RPFactory.GetManager<ICustomerManager>();
            var rsResponse = Task.Run(async () => await salesOrderManager.ECL_TV_ConfirmQuotation(request.QuotationId,
                baseCompany, request.TenderLine.Amount, request.TMVSalesOrigin, request.TMVFraudReviewStatus,
                request.TMVKountScore, request.TenderLine.CardTypeId,
                request.TenderLine.TenderTypeId, request.TenderLine.MaskedCardNumber, request.TransactionId, _authCode,
                request.TransactionDate, DateTime.Now, request.TenderLine.CardToken, request.TenderLine.Authorization,
                request.TenderLine.CardOrAccount, request.TenderLine.IBAN, request.TenderLine.SwiftCode,
                request.TenderLine.BankName, request.TenderLine.BoletoXml, request.TenderLine.PspReference, request.TenderLine.Alipay?.BuyerId,
                request.TenderLine.Alipay?.BuyerEmail, request.TenderLine.Alipay?.OutTradeNo, request.TenderLine.Alipay?.TradeNo, request.ChannelReferenceId
                )).Result;
            return rsResponse;
        }
        #endregion
    }
}
