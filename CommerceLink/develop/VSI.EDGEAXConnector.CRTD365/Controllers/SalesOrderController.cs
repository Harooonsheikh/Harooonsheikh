using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels;
using TMVRetailCommerceOperations;
using Newtonsoft.Json;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.ERPDataModels.Enums;
using System.Xml.Serialization;
using System.Xml;
using VSI.EDGEAXConnector.Data;
using Microsoft.Dynamics.Commerce.RetailProxy;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using System.Globalization;
using System.Text;
using VSI.EDGEAXConnector.Logging;
using System.IO;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;
using NewRelic.Api.Agent;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class SalesOrderController : BaseController, ISalesOrderController
    {
        #region "Public"
        public SalesOrderController(string storeKey) : base(storeKey)
        {

        }
        public ErpSalesOrder UploadOrder(ErpSalesOrder salesOrder, string requestId)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            bool isExternalSystemTimeLogged = false;
            SalesOrder objRSSalesOrder;
            string response = "";
            string d365Error = "";
            try
            {
                if (salesOrder.AffiliationLoyaltyTierLines != null)
                {
                    foreach (var affiliation in salesOrder.AffiliationLoyaltyTierLines)
                    {
                        affiliation.ChannelId = baseChannelId;
                    }
                }

                //Required to push sales order via RS because it accept JSON objects
                salesOrder = SalesOrderMappingHelper.SetNullValues(salesOrder);
                // US
                //String strSalesOrder = Newtonsoft.Json.JsonConvert.SerializeObject(salesOrder);
                objRSSalesOrder = _mapper.Map<SalesOrder>(salesOrder);
                objRSSalesOrder = SalesOrderMappingHelper.MapMissingProperties(salesOrder, objRSSalesOrder);
                List<CommerceProperty> variantIds = new List<CommerceProperty>();

                var variantRecIds = new List<long>();

                foreach (var salesOrderSalesLine in salesOrder.SalesLines)
                {
                    variantRecIds.Add(salesOrderSalesLine.ProductId);

                    var com = new CommerceProperty
                    {
                        Key = salesOrderSalesLine.ProductId.ToString(),
                        Value = new CommercePropertyValue { StringValue = salesOrderSalesLine.Variant.VariantId }
                    };
                    variantIds.Add(com);
                }

                StringBuilder disablePacLicenseOfSalesLines = new StringBuilder("");
                foreach (var salesLine in salesOrder.SalesLines)
                {
                    var tmvPACLicense = salesLine.CustomAttributes.FirstOrDefault(x => x.Key == "TMVDisablePACLicense").Value;
                    if (tmvPACLicense == "1")
                    {
                        disablePacLicenseOfSalesLines.Append(salesLine.CustomAttributes.FirstOrDefault(x => x.Key == "TMVOLDSALESLINENUMBER").Value + ",");
                    }
                }
                //NS: Close old contract for contract management
                if (!string.IsNullOrEmpty(salesOrder.TMVOldSalesOrderNumber))
                {
                    ErpCloseExistingOrderResponse closeContractResponse = this.CloseExistingOrder(salesOrder.TMVOldSalesOrderNumber, "", disablePacLicenseOfSalesLines.ToString());

                    if (closeContractResponse.Success)
                    {
                        response = "";
                    }
                    else
                    {
                        d365Error = closeContractResponse.Message;
                        CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40525, currentStore, salesOrder.TMVOldSalesOrderNumber, d365Error);
                        response = "-1";
                    }
                }

                if (response == "")
                {
                    timer = Stopwatch.StartNew();
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "AsyncUploadSalesOrder", DateTime.UtcNow);
                    response = AsyncUploadSalesOrder(objRSSalesOrder, variantIds).Result;
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateSalesOrderTransaction", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "AsyncUploadSalesOrder", DateTime.UtcNow);
                    isExternalSystemTimeLogged = true;
                }

                if (response == "-1")
                {
                    string message = string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40525), salesOrder.TMVOldSalesOrderNumber, d365Error);
                    Exception exp = new Exception(message);
                    throw exp;
                }
                else if (response == "0")
                {
                    //Sales order has been uploaded without extensions
                    //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL40519, currentStore, salesOrder.Id);
                    string message = string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40519), salesOrder.Id);
                    Exception exp = new Exception(message);
                    throw exp;
                }
                else if (response == "1")
                {
                    //Sales order has been uploaded with extensions
                    //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL40520, currentStore, salesOrder.Id);
                }

                //NS: Contract Migration SO Customization
                if (!string.IsNullOrEmpty(salesOrder.TMVMigratedOrderNumber))
                {
                    UpdateSalesOrderController updateSalesOrder = new UpdateSalesOrderController(currentStore.StoreKey);

                    List<Int64> orgSaleLinesIds = new List<Int64>();
                    List<Int64> newSaleLinesIds = new List<Int64>();

                    foreach (var line in salesOrder.SalesLines)
                    {
                        if (line.TMVMigratedSalesLineNumber > 0)
                        {
                            orgSaleLinesIds.Add(line.TMVMigratedSalesLineNumber);
                            newSaleLinesIds.Add(0);
                        }
                    }

                    var addContractRelationResponse = updateSalesOrder.AddContractRelation(ErpTMVContractRelationType.Migration, salesOrder.TMVMigratedOrderNumber, String.Join(",", orgSaleLinesIds), string.Empty, String.Join(",", newSaleLinesIds));

                    if (addContractRelationResponse.Success)
                    {
                        //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL40521, currentStore);
                    }
                    else
                    {
                        //TODO: How to handle if migration info failed?
                        //In D365 will update new sales order info to this record if will not exist then will insert complete record.
                        CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40522, currentStore, salesOrder.Id, salesOrder.TMVMigratedOrderNumber);
                    }
                }

                //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

                // Return the Erp SalesOrder object, no need to reverse mapping
                return salesOrder;
            }
            catch (RetailProxyException rpe)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateSalesOrderTransaction", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "AsyncUploadSalesOrder", DateTime.UtcNow);
                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch (Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateSalesOrderTransaction", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "AsyncUploadSalesOrder", DateTime.UtcNow);
                }
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }
        }
        public List<ErpSalesOrderStatus> GetSalesOrderStatus(string channelReferenceIds, string salesIds, DateTimeOffset fromDateOff, DateTimeOffset toDateOff)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                QueryResultSettings queryResultSettings = new QueryResultSettings { Paging = new PagingInfo() };

                var fromDate = fromDateOff.DateTime.ToString("yyyy-MM-dd");
                var toDate = toDateOff.DateTime.ToString("yyyy-MM-dd");

                var salesOrderReturn = AsyncSearchSalesOrderStatuses(channelReferenceIds, salesIds, fromDate, toDate, baseChannelId, queryResultSettings);

                List<ErpSalesOrderStatus> erpSalesOrderStatuses = new List<ErpSalesOrderStatus>();

                if ((bool)salesOrderReturn.Status)
                {
                    foreach (var orderData in salesOrderReturn.SalesOrderStatuses)
                    {
                        string[] orderStatusData = orderData.Split(',');

                        if (orderStatusData.Length > 0)
                        {
                            ErpSalesOrderStatus erpSalesOrderStatus = new ErpSalesOrderStatus
                            {
                                ChannelRefId = orderStatusData[0],
                                Status = orderStatusData[1],
                                CustomerAcc = orderStatusData[2],
                                SalesId = orderStatusData[3],
                                Notify = false
                            };

                            erpSalesOrderStatuses.Add(erpSalesOrderStatus);
                        }
                    }
                }

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

                // Return the ErpSalesOrderStatus
                return erpSalesOrderStatuses;
            }

            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
        }
        public List<ErpSalesOrderStatus> GetSalesOrderRenewalStatus(string salesIds)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                var salesOrderReturn = AsyncSearchSalesOrderRenewalStatuses(salesIds, baseChannelId);

                List<ErpSalesOrderStatus> erpSalesOrderStatuses = new List<ErpSalesOrderStatus>();
                if ((bool)salesOrderReturn.Status && salesOrderReturn.SalesOrderStatuses != null)
                {
                    foreach (var orderData in salesOrderReturn.SalesOrderStatuses)
                    {
                        string[] orderStatusData = orderData.Split(',');

                        if (orderStatusData.Length > 0)
                        {
                            ErpSalesOrderStatus erpSalesOrderStatus = new ErpSalesOrderStatus
                            {
                                Status = orderStatusData[0],
                                SalesId = orderStatusData[1],
                                Notify = true
                            };

                            erpSalesOrderStatuses.Add(erpSalesOrderStatus);
                        }
                    }
                }

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

                return erpSalesOrderStatuses;
            }

            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
        }
        public ERPContractSalesorderResponse GetContractSalesOrder(ContractSalesorderRequest request, string requestId)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            bool isExtenalSystemTimeLogged = false;
            ERPContractSalesorderResponse erpResponse = new ERPContractSalesorderResponse(false, null);
            bool isSuccess = true;
            string message = string.Empty;

            try
            {
                IEnumerable<string> requestStatuses = request.Status;

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_TV_GetContractSalesorder", DateTime.UtcNow);

                timer = Stopwatch.StartNew();
                var rsResponse = ECL_TV_GetContractSalesorder(request, requestStatuses);

                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "GetContractSalesorder", GetElapsedTime());

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_TV_GetContractSalesorder", DateTime.UtcNow);
                isExtenalSystemTimeLogged = true;

                if ((bool)rsResponse.Status)
                {
                    foreach (var contract in rsResponse.Contracts)
                    {
                        List<ErpSalesOrder> erpSalesOrders = new List<ErpSalesOrder>();

                        ////++ Deserialize Objects
                        CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL600003, requestId, "Mapper.Map<ErpCustomer>", DateTime.UtcNow);

                        ErpCustomer erpCustomer = _mapper.Map<ErpCustomer>(JsonConvert.DeserializeObject<Customer>(contract.Customer));
                        ErpContactPerson erpContactPerson = _mapper.Map<ErpContactPerson>(JsonConvert.DeserializeObject<ContactPerson>(contract.ContactPerson));
                        List<SalesOrder> salesOrders = JsonConvert.DeserializeObject<List<SalesOrder>>(contract.SalesOrders);

                        if (salesOrders.Count > 0)
                        {
                            foreach (var order in salesOrders)
                            {
                                ErpSalesOrder erpSalesOrder = new ErpSalesOrder();
                                erpSalesOrder = _mapper.Map<ErpSalesOrder>(order);
                                //++ Setting up extention properties 
                                SetupSalesOrderExtentionProperties(erpSalesOrder, request.SmallResponse);
                                SetupSalesLineExtentionProperties(erpSalesOrder);
                                erpSalesOrders.Add(erpSalesOrder);
                            }

                            // Do not add Duplicate Customer,ContactPerson Data.
                            var existingContract = erpResponse.Contracts.FirstOrDefault(c => c.Customer.AccountNumber == erpCustomer.AccountNumber);
                            if (existingContract != null)
                            {
                                existingContract.SalesOrders.AddRange(erpSalesOrders);
                            }
                            else
                            {
                                var contracts = new Contract(erpCustomer, erpContactPerson, erpSalesOrders);
                                erpResponse.Contracts.Add(contracts);
                            }
                            isSuccess = isSuccess ? true : false;
                        }
                        else
                        {
                            var contracts = new Contract(erpCustomer, erpContactPerson, erpSalesOrders);
                            erpResponse.Contracts.Add(contracts);
                            isSuccess = false;
                        }

                        message = isSuccess ? "Success" : CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40501);
                        erpResponse.Success = true;
                        erpResponse.Message = message;
                    }
                }
                else if (!(bool)rsResponse.Status)
                {
                    erpResponse = new ERPContractSalesorderResponse(false, rsResponse.Message);
                }
            }
            catch (RetailProxyException rpe)
            {
                if (!isExtenalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "GetContractSalesorder", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_TV_GetContractSalesorder", DateTime.UtcNow);
                }
                message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch (Exception exp)
            {
                if (!isExtenalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "GetContractSalesorder", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_TV_GetContractSalesorder", DateTime.UtcNow);
                }
                throw exp;
            }
            return erpResponse;
        }

        /// <summary>
        /// Mehtod to get the invoices against the provided SalesOrderId
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ErpContractInvoicesResponse GetContractInvoices(ContractInvoicesRequest request)
        {
            throw new NotImplementedException();
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            //List<ErpCustInvoiceJour> erpCustInvoiceJourList = new List<ErpCustInvoiceJour>();
            //ErpContractInvoicesResponse erpResponse = new ErpContractInvoicesResponse(false, "", null);
            //try
            //{
            //    var rsResponse = ECL_GetContractInvoices(request);

            //    if (rsResponse.Success)
            //    {
            //        var invoicesList = JsonConvert.DeserializeObject<CustInvoiceJours>(rsResponse.Result);

            //        if (invoicesList != null && invoicesList.CustInvoiceJour != null)
            //        {
            //            for (int i = 0; i < invoicesList.CustInvoiceJour.Count; i++)
            //            {
            //                CustInvoiceJour item = invoicesList.CustInvoiceJour[i];

            //                if (String.IsNullOrEmpty(request.InvoiceId) == false)
            //                {
            //                    if (item.InvoiceId == request.InvoiceId)
            //                    {
            //                        ErpCustInvoiceJour erpCustInvoiceJour = getErpCustInvoiceJour(item);

            //                        erpCustInvoiceJourList.Add(erpCustInvoiceJour);
            //                    }
            //                    else
            //                    {
            //                        continue;
            //                    }
            //                }
            //                else
            //                {
            //                    ErpCustInvoiceJour erpCustInvoiceJour = getErpCustInvoiceJour(item);

            //                    erpCustInvoiceJourList.Add(erpCustInvoiceJour);
            //                }
            //            }

            //            erpResponse = new ErpContractInvoicesResponse(true, "Success", erpCustInvoiceJourList);
            //        }
            //        else
            //        {
            //            erpResponse = new ErpContractInvoicesResponse(true, CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40501), erpCustInvoiceJourList);
            //        }
            //    }
            //    else if (!rsResponse.Success)
            //    {
            //        erpResponse = new ErpContractInvoicesResponse(false, rsResponse.Message.Replace("\"", ""), null);
            //    }
            //}
            //catch (RetailProxyException rpe)
            //{
            //    string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
            //    AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
            //    throw exp;
            //}
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            //return erpResponse;
        }
        public ErpValidateVATNumberResponse ValidateVATNumber(ErpValidateVATNumberRequest request, string requestId)
        {
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            bool isExternalSystemTimeLogged = false;
            ErpValidateVATNumberResponse erpValidateVATResponse;

            try
            {
                //ContactPerson conPerson = _mapper.Map<ErpContactPerson, ContactPerson>(contactPerson);


                timer = Stopwatch.StartNew();
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_TV_ValidateVATNumber", DateTime.UtcNow);
                var response = ECL_TV_ValidateVATNumber(request);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "ValidateVATNumber", GetElapsedTime());
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_TV_ValidateVATNumber", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;
                erpValidateVATResponse = new ErpValidateVATNumberResponse((bool)response.Status, response.Message, response.Result);
            }
            catch (Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "ValidateVATNumber", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_ValidateVATNumber", DateTime.UtcNow);
                }
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpValidateVATResponse = new ErpValidateVATNumberResponse(false, exp.Message, string.Empty);
            }

            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpValidateVATResponse;
        }
        /// <summary>
        /// Method to get Retail Affilications from AX
        /// </summary>
        /// <returns></returns>
        public List<ErpAffiliation> GetRetailAffiliations(string storeKey)
        {

            PagedResult<Affiliation> affiliataionsReturn = AsyncGetRetailAffiliations();

            List<Affiliation> affiliataions = affiliataionsReturn.ToList();

            List<ErpAffiliation> erpAffilications =
                _mapper.Map<List<ErpAffiliation>>(affiliataions) ?? new List<ErpAffiliation>();

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpAffilications;
        }

        public ErpAddPaymentLinkForInvoiceResponse AddPaymentLinkForInvoice(ErpAddPaymentLinkForInvoiceRequest request, string requestId)
        {
            bool isExternalSystemTimeLogged = false;
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpAddPaymentLinkForInvoiceResponse erpResponse = new ErpAddPaymentLinkForInvoiceResponse(false, string.Empty, string.Empty);
            try
            {

                timer = Stopwatch.StartNew();
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_TV_AddPaymentLinkForInvoice", DateTime.UtcNow);
                var rsResponse = ECL_TV_AddPaymentLinkForInvoice(request);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "AddPaymentLinkForInvoice", GetElapsedTime());
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_TV_AddPaymentLinkForInvoice", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;
                if ((bool)rsResponse.Status)
                {
                    erpResponse = new ErpAddPaymentLinkForInvoiceResponse(true, rsResponse.Message, rsResponse.Result);
                }
                else
                {
                    erpResponse = new ErpAddPaymentLinkForInvoiceResponse(false, rsResponse.Message, rsResponse.Result);
                }

            }
            catch (RetailProxyException rpe)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "AddPaymentLinkForInvoice", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_AddPaymentLinkForInvoice", DateTime.UtcNow);
                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch (Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "AddPaymentLinkForInvoice", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_AddPaymentLinkForInvoice", DateTime.UtcNow);
                }
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        public ErpAddPaymentLinkForInvoiceResponse AddPaymentLinkForInvoiceBoleto(ErpAddPaymentLinkForInvoiceBoletoRequest request, string requestId)
        {
            bool isExternalSystemTimeLogged = false;
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            ErpAddPaymentLinkForInvoiceResponse erpResponse = new ErpAddPaymentLinkForInvoiceResponse(false, string.Empty, string.Empty);
            try
            {

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_TV_AddPaymentLinkForInvoiceBoleto", DateTime.UtcNow);
                var rsResponse = ECL_TV_AddPaymentLinkForInvoiceBoleto(request);
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_TV_AddPaymentLinkForInvoiceBoleto", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;
                if ((bool)rsResponse.Status)
                {
                    erpResponse = new ErpAddPaymentLinkForInvoiceResponse(true, rsResponse.Message, rsResponse.Result);
                }
                else
                {
                    erpResponse = new ErpAddPaymentLinkForInvoiceResponse(false, rsResponse.Message, rsResponse.Result);
                }

            }
            catch (RetailProxyException rpe)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_AddPaymentLinkForInvoiceBoleto", DateTime.UtcNow);
                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch (Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_AddPaymentLinkForInvoiceBoleto", DateTime.UtcNow);
                }
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        public ErpCreateLicenseResponse CreateProductLicense(List<ErpCreateActionLinkRequest> erpCreateActionLinkRequests, string requestId)
        {
            bool isExternalSystemTimeLogged = false;
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            List<CreateLicenseRequest> createLicenseRequest = new List<CreateLicenseRequest>();
            ErpCreateLicenseResponse erpResponse = new ErpCreateLicenseResponse(false, "", null);
            CreateProductLicenseResponse createLicenseResponse = new CreateProductLicenseResponse();
            List<ErpProductLicenseResponse> erpProductLicenseResponse = new List<ErpProductLicenseResponse>();
            try
            {
                createLicenseRequest = MapClObjectToErp(erpCreateActionLinkRequests);

                timer = Stopwatch.StartNew();
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500002, currentStore, requestId, "ECL_TV_CreateProductLicense", DateTime.UtcNow);
                createLicenseResponse = ECL_TV_CreateProductLicense(createLicenseRequest);
                CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateProductLicense", GetElapsedTime());
                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500003, currentStore, requestId, "ECL_TV_CreateProductLicense", DateTime.UtcNow);
                isExternalSystemTimeLogged = true;

                if ((bool)createLicenseResponse.Status)
                {
                    erpProductLicenseResponse = MapErpObjectToCL(createLicenseResponse.ProductLicenseResponses.ToList());
                    erpResponse = new ErpCreateLicenseResponse(true, createLicenseResponse.Message, erpProductLicenseResponse);
                }
                else
                {
                    erpResponse = new ErpCreateLicenseResponse(false, createLicenseResponse.Message, null);
                }

            }
            catch (RetailProxyException rpe)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateProductLicense", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_CreateProductLicense", DateTime.UtcNow);
                }
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch (Exception exp)
            {
                if (!isExternalSystemTimeLogged)
                {
                    CommerceLinkLogger.LogTimeTraceCSV(CommerceLinkLoggerMessages.VSICL500008, currentStore, requestId, "CreateProductLicense", GetElapsedTime());
                    CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL500005, currentStore, requestId, "ECL_TV_CreateProductLicense", DateTime.UtcNow);
                }
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpResponse;
        }
        /*
        public List<ErpSalesOrderStatus> GetSalesOrderStatus(DateTimeOffset fromDateOff, DateTimeOffset toDateOff)
        {
            try
            {
                 Query Result Settings
                QueryResultSettings queryResultSettings = new QueryResultSettings();
                queryResultSettings.Paging = new PagingInfo();
                queryResultSettings.Paging.Skip = 0;
                queryResultSettings.Paging.Top = (long?)Convert.ToInt64(ConfigurationHelper.GetSetting(SALESORDER.Retail_Server_Paging));

                PagedResult<SalesOrder> salesOrderReturn;
                List<ErpSalesOrderStatus> erpSalesOrderStatuses = new List<ErpSalesOrderStatus>();

                 Start without skipping
                queryResultSettings.Paging.Skip = 0;

                 AX Call
                do
                {
                    salesOrderReturn = AsyncSearchSalesOrder(fromDateOff, toDateOff, queryResultSettings).Result;

                    queryResultSettings.Paging.Skip += queryResultSettings.Paging.Top;

                     Map each SalesOrder to EroSalesOrderStatus and add to return object
                    foreach (SalesOrder order in salesOrderReturn)
                    {
                        ErpSalesOrderStatus erpSalesOrderStatus = new ErpSalesOrderStatus();

                        erpSalesOrderStatus.ChannelRefId = order.ChannelReferenceId;
                        erpSalesOrderStatus.CustomerAcc = order.CustomerId;
                        ErpSalesStatus statusOfSalesOrder = (ErpSalesStatus)order.StatusValue;
                        erpSalesOrderStatus.Status = statusOfSalesOrder.ToString();
                        erpSalesOrderStatus.SalesId = order.SalesId;
                        erpSalesOrderStatus.Notify = false;

                        erpSalesOrderStatuses.Add(erpSalesOrderStatus);
                    }

                }
                while (salesOrderReturn.Count() == queryResultSettings.Paging.Top);

                 Return the ErpSalesOrderStatus
                return erpSalesOrderStatuses;
            }

            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);
                throw exp;
            }
        }
        */

        /// <summary>
        /// Close existing order
        /// </summary>
        /// <param name="salesId"></param>
        /// <param name="tmvContractLineActionType"></param>
        /// <param name="pacLicense"></param>
        /// <returns></returns>
        public ErpCloseExistingOrderResponse CloseExistingOrder(string salesId, string pacLicense, string disablePacLicenseOfSalesLines = "")
        {
            throw new NotImplementedException();
            //ErpCloseExistingOrderResponse erpResponse;

            //try
            //{
            //    var response = ECL_CloseExistingOrder(salesId, pacLicense, disablePacLicenseOfSalesLines);

            //    if (response.Success)
            //    {
            //        erpResponse = new ErpCloseExistingOrderResponse(true, response.Message, true);
            //    }
            //    else
            //    {
            //        erpResponse = new ErpCloseExistingOrderResponse(false, response.Message, false);
            //    }
            //}
            //catch (RetailProxyException rpe)
            //{
            //    string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
            //    AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
            //    throw exp;
            //}
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            //return erpResponse;
        }
        /// <summary>
        /// Change Contract Payment Method
        /// </summary>
        /// <param name="salesId"></param>
        /// <param name="newPaymentMethodRecId"></param>
        /// <returns></returns>
        public ErpChangeContractPaymentMethodResponse ChangeContractPaymentMethod(string salesId, long newPaymentMethodRecId, string tenderTypeId, long bankAccountRecId)
        {
            throw new NotImplementedException();
            //ErpChangeContractPaymentMethodResponse erpResponse;

            //try
            //{
            //    var response = ECL_RTS_ChangeContractPaymentMethod(salesId, newPaymentMethodRecId, tenderTypeId, bankAccountRecId);

            //    if (response.Success)
            //    {
            //        erpResponse = new ErpChangeContractPaymentMethodResponse(true, response.Message, true);
            //    }
            //    else
            //    {
            //        erpResponse = new ErpChangeContractPaymentMethodResponse(false, response.Message, false);
            //    }
            //}
            //catch (RetailProxyException rpe)
            //{
            //    string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
            //    AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
            //    throw exp;
            //}
            //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            //return erpResponse;
        }
        public ProcessContractOperationResponse ProcessContractOperation(string processContractOperationRequest)
        {
            ProcessContractOperationResponse erpResponse = new ProcessContractOperationResponse(false, "", null);

            try
            {
                var rsResponse = ECL_TV_ContractSwitchMigrate(processContractOperationRequest);

                if ((bool)rsResponse.Status)
                {
                    erpResponse = new ProcessContractOperationResponse(true, rsResponse.Message, rsResponse.Result);
                }
                else
                {
                    if (CommonUtility.IsValidXML(rsResponse.Result))
                    {
                        ProductInformation productInformation = new ProductInformation();
                        if (!string.IsNullOrWhiteSpace(rsResponse.Result))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(ProductInformation));
                            productInformation = (ProductInformation)serializer.Deserialize(new StringReader(rsResponse.Result));
                        }
                        erpResponse = new ProcessContractOperationResponse((bool)rsResponse.Status, rsResponse.Message, productInformation);
                    }
                    else
                    {
                        erpResponse = new ProcessContractOperationResponse((bool)rsResponse.Status, rsResponse.Message, rsResponse.Result);
                    }
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                erpResponse = new ProcessContractOperationResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, string.Empty);
                throw new CommerceLinkError(message);
            }
            return erpResponse;
        }
        public ProcessContractOperationResponse CheckoutProcessContractOperation(string request)
        {
            var erpResponse = new ProcessContractOperationResponse(false, "", null);

            try
            {
                var rsResponse = ECL_TV_CheckoutProcessContractOperation(request);
                erpResponse = new ProcessContractOperationResponse((bool)rsResponse.Status, rsResponse.Message, rsResponse.Result);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, string.Empty);
                throw new CommerceLinkError(message);
            }

            return erpResponse;
        }

        public PriceResponse GetOrValidatePriceInformation(PriceRequest priceOperationRequest)
        {
            string priceOperationXMLRequest = string.Empty;

            PriceResponse erpResponse = new PriceResponse(false, "", null);
            LoggingDAL loggingDAL = new LoggingDAL(currentStore.StoreKey);
            RequestResponse requestResponse = new RequestResponse()
            {
                ApplicationName = string.Empty,
                CreatedOn = DateTime.UtcNow,
                DataDirectionId = 1,
                DataPacket = priceOperationRequest.SerializeToJson(),
                IsSuccess = true,
                IdentifierKey = "IndirectCustomerAccount",
                IdentifierValue = priceOperationRequest.IndirectCustomerAccount,
                MethodName = "GetOrValidatePriceInformation",
            };

            try
            {
                priceOperationRequest.ChannelRecId = baseChannelId;
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(PriceRequest));

                using (StringWriter textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, priceOperationRequest);
                    priceOperationXMLRequest = textWriter.ToString();
                }
                var rsResponse = ECL_TV_GetOrValidatePriceInformation(priceOperationXMLRequest);

                if ((bool)rsResponse.Status && priceOperationRequest.IsValidateRequest)
                {
                    requestResponse.IsSuccess = true;
                    erpResponse = new PriceResponse(true, rsResponse.Message, null);
                }
                else
                {
                    requestResponse.IsSuccess = false;
                    ProductInformation productInformation = new ProductInformation();
                    if (!string.IsNullOrWhiteSpace(rsResponse.Result))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(ProductInformation));
                        productInformation = (ProductInformation)serializer.Deserialize(new StringReader(rsResponse.Result));
                    }
                    erpResponse = new PriceResponse((bool)rsResponse.Status, rsResponse.Message, productInformation);
                }

            }
            catch (Exception exp)
            {
                requestResponse.IsSuccess = false;
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, string.Empty);
                //CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
                erpResponse = new PriceResponse(false, exp.Message + " | " + message, null);
            }
            requestResponse.OutputSentAt = DateTime.UtcNow;
            requestResponse.OutputPacket = erpResponse.SerializeToJson();
            loggingDAL.LogRequestResponse(requestResponse);
            return erpResponse;
        }
        public ProcessContractOperationResponse CreateNewContractLines(string createNewContractLinesRequest)
        {
            ProcessContractOperationResponse erpResponse = new ProcessContractOperationResponse(false, "", null);

            try
            {
                var rsResponse = ECL_TV_CreateNewContractLines(createNewContractLinesRequest);

                if ((bool)rsResponse.Status)
                {
                    erpResponse = new ProcessContractOperationResponse(true, rsResponse.Message, rsResponse.Result);
                }
                else
                {
                    if (CommonUtility.IsValidXML(rsResponse.Result))
                    {
                        ProductInformation productInformation = new ProductInformation();
                        if (!string.IsNullOrWhiteSpace(rsResponse.Result))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(ProductInformation));
                            productInformation = (ProductInformation)serializer.Deserialize(new StringReader(rsResponse.Result));
                        }
                        erpResponse = new ProcessContractOperationResponse((bool)rsResponse.Status, rsResponse.Message, productInformation);
                    }
                    else
                    {
                        erpResponse = new ProcessContractOperationResponse((bool)rsResponse.Status, rsResponse.Message, rsResponse.Result);
                    }
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                erpResponse = new ProcessContractOperationResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, string.Empty);
                throw new CommerceLinkError(message);
            }
            
            return erpResponse;
        }
        public ErpCreatePaymentJournalResponse CreatePaymentJournal(ErpCreatePaymentJournalRequest request, string requestId)
        {
            throw new NotImplementedException();
            //string xmlRequest = string.Empty;
            //var erpResponse = new ErpCreatePaymentJournalResponse(false, "");

            //xmlRequest = CommonUtility.ConvertToXmlString(request);

            //var rsResponse = ECL_TV_CreatePaymentJournal(xmlRequest);

            //erpResponse = new ErpCreatePaymentJournalResponse(rsResponse.Success, rsResponse.Message);

            //return erpResponse;
        }
        public ErpTransferPartnerContractResponse TransferPartnerContract(TransferPartnerContractRequest request, string requestId)
        {
            try
            {
                string xmlRequest = CommonUtility.ConvertToXmlString(request);

                var rsResponse = ECL_TV_TransferPartnerContract(xmlRequest);

                return new ErpTransferPartnerContractResponse((bool)rsResponse.Status, rsResponse.Message, rsResponse.MessageCode, rsResponse.SalesOrderId);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }
        }

        public ErpContractRenewalResponse ContractRenewal(ContractRenewalRequest request, string requestId)
        {
            try
            {
                string xmlRequest = CommonUtility.ConvertToXmlString(request);
                var rsResponse = ECL_TV_ContractRenewal(xmlRequest);
                return new ErpContractRenewalResponse((bool)rsResponse.Status, rsResponse.Message, rsResponse.Result);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }
        }

        public ErpGetBoletoUrlResponse GetBoletoUrl(ErpGetBoletoUrlRequest request, string requestId)
        {
            ErpGetBoletoUrlResponse erpResponse;
            
            try
            {
                var rsResponse = ECL_TV_GetBoletoUrl(request);
                erpResponse = new ErpGetBoletoUrlResponse((bool)rsResponse.Status, rsResponse.Message, rsResponse.Result);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, requestId);
                throw new CommerceLinkError(message);
            }

            return erpResponse;
        }
        public ErpUpdateCustomerPortalLinkResponse UpdateCustomerPortalLink(UpdateCustomerPortalLinkRequest request)
        {
            ErpUpdateCustomerPortalLinkResponse erpResponse;

            try
            {
                string xmlRequest = CommonUtility.ConvertToXmlString(request);
                var rsResponse = ECL_TV_UpdateCustomerPortalLink(xmlRequest);
                erpResponse = new ErpUpdateCustomerPortalLinkResponse((bool)rsResponse.Status, rsResponse.Message,rsResponse.Result);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, string.Empty);
                throw new CommerceLinkError(message);
            }

            return erpResponse;
        }
        #endregion

        #region "Private"

        private List<CreateLicenseRequest> MapClObjectToErp(List<ErpCreateActionLinkRequest> erpCreateActionLinkRequests)
        {
            List<CreateLicenseRequest> createLicenseRequests = new List<CreateLicenseRequest>();
            foreach (var erpRequest in erpCreateActionLinkRequests)
            {
                CreateLicenseRequest createLicenseRequest = new CreateLicenseRequest();
                createLicenseRequest.GUID = erpRequest.GUID;
                createLicenseRequest.ItemId = erpRequest.ItemId;
                createLicenseRequest.ProductId = erpRequest.ProductId;
                createLicenseRequest.VariantId = erpRequest.VariantId;
                createLicenseRequest.Quantity = erpRequest.Quantity;
                createLicenseRequests.Add(createLicenseRequest);
            }
            return createLicenseRequests;
        }

        private List<ErpProductLicenseResponse> MapErpObjectToCL(List<ProductLicenseResponse> productLicenseResponses)
        {
            List<ErpProductLicenseResponse> erpProductLicenseResponses = new List<ErpProductLicenseResponse>();
            foreach (var erpResponse in productLicenseResponses)
            {
                ErpProductLicenseResponse erpProductLicenseResponse = new ErpProductLicenseResponse("", "", "", 0, 0, "");
                erpProductLicenseResponse.GUID = erpResponse.GUID;
                erpProductLicenseResponse.ItemId = erpResponse.ItemId;
                erpProductLicenseResponse.ProductId = erpResponse.ProductId;
                erpProductLicenseResponse.VariantId = erpResponse.VariantId;
                erpProductLicenseResponse.Quantity = erpResponse.Quantity;
                erpProductLicenseResponse.ActionLink = erpResponse.ActionLink;
                erpProductLicenseResponses.Add(erpProductLicenseResponse);
            }
            return erpProductLicenseResponses;
        }
        /// <summary>
        /// Asynchronous sales order upload
        /// </summary>
        /// <param name="salesOrder"></param>
        /// <returns></returns>
        private async Task<string> AsyncUploadSalesOrder(SalesOrder salesOrder, List<CommerceProperty> variantIds)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            // Saving object to Json before Uploading
            var orderJson = salesOrder.SerializeToJson(1);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10502, currentStore, salesOrder.ChannelReferenceId, orderJson);
            // Create Sales Order Manager object
            var res = ECL_UploadSalesOrder(salesOrder, variantIds);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return res;
        }
        /// <summary>
        /// Asynchronous sales order search
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="queryResultSettings">Query Result Settings for asynchronous call</param>
        /// <param name="channelReferenceIds"></param>
        /// <param name="salesIds"></param>
        /// <param name="fromDateOff"></param>
        /// <param name="toDateOff"></param>
        /// <returns></returns>
        private GetSaleOrderStatusesResponse AsyncSearchSalesOrderStatuses(string channelReferenceIds, string salesIds, string fromDateOff, string toDateOff, long channelId, QueryResultSettings queryResultSettings)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var salesOrderReturn = ECL_RTS_GetSaleOrderStatuses(channelReferenceIds, salesIds, fromDateOff, toDateOff, channelId);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            // Return the SalesOrder object
            return salesOrderReturn;
        }
        /// <summary>
        /// Asynchronous sales order search
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="queryResultSettings">Query Result Settings for asynchronous call</param>
        /// <param name="channelReferenceIds"></param>
        /// <param name="salesIds"></param>
        /// <param name="fromDateOff"></param>
        /// <param name="toDateOff"></param>
        /// <returns></returns>
        private GetSaleOrderRenewalStatusesResponse AsyncSearchSalesOrderRenewalStatuses(string salesIds, long channelId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            // Create sales order manager object
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            var salesOrderReturn = salesOrderManager.ECL_RTS_GetSaleOrderRenewalStatuses(salesIds, baseCompany).Result;
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            // Return the SalesOrder object
            return salesOrderReturn;
        }

        /// <summary>
        /// Asynchronous sales order search
        /// </summary>
        /// <param name="orderNumber">Order Number</param>
        /// <param name="queryResultSettings">Query Result Settings for asynchronous call</param>
        /// <returns></returns>
        private async Task<PagedResult<SalesOrder>> AsyncSearchSalesOrder(DateTimeOffset fromDateOff, DateTimeOffset toDateOff, QueryResultSettings queryResultSettings)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            // Sales order search criteria
            SalesOrderSearchCriteria salesOrderSearchCriteria = new SalesOrderSearchCriteria();

            salesOrderSearchCriteria.StartDateTime = fromDateOff;
            salesOrderSearchCriteria.EndDateTime = toDateOff;
            salesOrderSearchCriteria.SearchTypeValue = (int)OrderSearchType.SalesOrder;
            salesOrderSearchCriteria.SalesTransactionTypeValues =
                new System.Collections.ObjectModel.ObservableCollection<int> { (int)SalesTransactionType.PendingSalesOrder };

            // Search the sales orders in SalesOrder object

            var salesOrderReturn = await ECL_SearchSalesOrders(queryResultSettings, salesOrderSearchCriteria);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            // Return the SalesOrder object
            return salesOrderReturn;
        }
        public object SearchOrders(string orderNumber)
        {

            throw new NotImplementedException();
        }

        #region TimeQuantity
        public double CalculateLineAmount(DateTime _calculateDateFrom, DateTime _validTo, int _billingPeriod, int _salesQty, double _salesPrice,
            double _discAmount, double _discPct, bool _isSubscription = false, bool _tmvAutoProlongation = false)
        {
            TMVSalesCommerceOperations tMVSales = new TMVSalesCommerceOperations();
            return tMVSales.calculateLineAmount(_calculateDateFrom, _validTo, _billingPeriod, _salesQty, _salesPrice, _discAmount, _discPct, _isSubscription, _tmvAutoProlongation);
        }

        public double CalculateTimeQty(DateTime _calculateDateFrom, DateTime _validTo, int _billingPeriod, bool _isSubscription = false,
                    bool _tmvAutoProlongation = false)
        {
            TMVSalesCommerceOperations tMVSales = new TMVSalesCommerceOperations();
            return tMVSales.calculateTimeQty(_calculateDateFrom, _validTo, _billingPeriod, _isSubscription, _tmvAutoProlongation);
        }

        #endregion

        private string CheckAndGetPropertyValue(ErpSalesOrder salesOrder, string key)
        {
            string returnValue = "";
            if (salesOrder.ExtensionProperties.Where(x => x.Key == key).Count() > 0)
            {
                returnValue = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == key).Value);
            }
            return returnValue;
        }

        private void SetupSalesOrderPaymentInformation(ErpSalesOrder salesOrder, bool smallResponse = false)
        {
            ErpCreditCardCust erpCreditCardCust = new ErpCreditCardCust();
            erpCreditCardCust.BankIBAN = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodBankIBAN").Value);
            if (string.IsNullOrWhiteSpace(erpCreditCardCust.BankIBAN))
            {

                if (!smallResponse)
                {
                    erpCreditCardCust.Notes = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodNotes").Value);
                    erpCreditCardCust.CustAccount = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodCustAccount").Value);
                }

                erpCreditCardCust.PayerId = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodPayerId").Value);
                erpCreditCardCust.ParentTransactionId = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodParenttransactionId").Value);
                erpCreditCardCust.EmailAddress = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodEmailAddress").Value);
                erpCreditCardCust.Authorization = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodAuthorization").Value);
                erpCreditCardCust.CardToken = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodCardToken").Value);
                erpCreditCardCust.CardNumber = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodCardNumber").Value);
                erpCreditCardCust.CreditCardProcessors = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodCreditCardProcessorId").Value);
                erpCreditCardCust.CreditCardTypeName = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodCreditCardTypeName").Value);
                erpCreditCardCust.ExpiryDate = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodExpiryDate").Value);
                erpCreditCardCust.RecId = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodRecId").Value);
                erpCreditCardCust.TMVCreditCardTokenIdOld = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodTMVCreditCardTokenIdOld").Value);
                erpCreditCardCust.ProcessorId = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodCreditCardProcessor").Value);
                erpCreditCardCust.IssuerCountry = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodIssuerCountry").Value);
            }
            else
            {
                // SEPA
                erpCreditCardCust.CustAccount = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodCustAccount").Value);
                erpCreditCardCust.RecId = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodRecId").Value);
                erpCreditCardCust.AccountId = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodAccountId").Value);
                erpCreditCardCust.Name = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodName").Value);
                erpCreditCardCust.ContactPerson = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodContactPerson").Value);
                erpCreditCardCust.SwiftCode = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodSwiftCode").Value);
                erpCreditCardCust.MandateRecId = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentMethodMandateRecId").Value);
            }

            salesOrder.PaymentInfo = erpCreditCardCust;
        }

        private void SetupSalesOrderExtentionProperties(ErpSalesOrder salesOrder, bool smallResponse = false)
        {
            if (!smallResponse)
            {
                salesOrder.TMVResellerAccount = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVResellerAccount").Value);
                salesOrder.TMVProductFamily = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVProductFamily").Value);
                salesOrder.TMVMainOfferType = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVMainOfferType").Value);
                salesOrder.TMVDistributorAccount = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVDistributorAccount").Value);
                salesOrder.TMVSalesOrderSubType = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVSalesOrderSubType").Value);
                salesOrder.TMVInvoiceScheduleComplete = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVInvoiceScheduleComplete").Value);
                salesOrder.TMVIndirectCustomer = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVIndirectCustomer").Value);
                salesOrder.PaymMode = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymMode").Value);
                salesOrder.RetailChannel = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "RetailChannel").Value);
            }
            salesOrder.TMVInvoicePosted = Convert.ToBoolean(GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVInvoicePosted").Value));
            salesOrder.TMVSubscriptionName = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVSubscriptionName").Value);
            salesOrder.TMVSubscriptionWeight = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVSubscriptionWeight").Value);

            salesOrder.PaymentRecID = Convert.ToInt64(GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "PaymentRecID").Value));
            salesOrder.TMVContractStartDate = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractStartDate").Value);
            salesOrder.TMVContractEndDate = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractEndDate").Value);
            salesOrder.Language = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "Language").Value);
            salesOrder.ThreeLetterISORegionName = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "ThreeLetterISORegionName").Value);
            salesOrder.TMVMainOfferType = GetPropertyValue(salesOrder.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVMainOfferType").Value);

            SetupSalesOrderPaymentInformation(salesOrder, smallResponse);

            salesOrder.ExtensionProperties = null;
        }
        private void SetupSalesLineExtentionProperties(ErpSalesOrder salesOrder)
        {
            foreach (var lineItem in salesOrder.SalesLines)
            {
                lineItem.VariantId = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "VariantId").Value);

                lineItem.TMVAutoProlongation = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVAutoProlongation").Value);
                lineItem.TMVBillingPeriod = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVBillingPeriod").Value);
                lineItem.TMVContractCalculateFrom = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractCalculateFrom").Value);
                lineItem.TMVContractCalculateTo = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractCalculateTo").Value);
                lineItem.TMVContractCancelDate = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractCancelDate").Value);
                lineItem.TMVContractPossCancelDate = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractPossCancelDate").Value);
                lineItem.TMVContractStatusLine = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractStatusLine").Value);
                lineItem.TMVContractTermDate = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractTermDate").Value);
                lineItem.TMVContractTermDateEffective = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractTermDateEffective").Value);
                lineItem.TMVContractValidFrom = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractValidFrom").Value);
                lineItem.TMVContractValidTo = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractValidTo").Value);
                lineItem.TMVCustomerRef = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVCustomerRef").Value);
                lineItem.TMVEULAVersion = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVEULAVersion").Value);
                lineItem.TMVLineModified = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVLineModified").Value);
                lineItem.TMVOriginalLineAmount = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVOriginalLineAmount").Value);
                lineItem.TMVPurchOrderFormNum = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVPurchOrderFormNum").Value);
                lineItem.TMVReversedLine = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVReversedLine").Value);
                lineItem.TMVContractPossTermDate = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVContractPossTermDate").Value);
                lineItem.PACLicense = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "PACLicense").Value);

                lineItem.ProductName = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ProductName").Value);
                lineItem.ProductDescription = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ProductDescription").Value);
                lineItem.TMVIsMigrated = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVIsMigrated").Value);
                lineItem.TMVIsSwitch = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVIsSwitch").Value);
                lineItem.TMVParent = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVParent").Value);
                lineItem.TMVTimeQuantity = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVTimeQuantity").Value);
                lineItem.TMVCalculateLineAmount = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVCalculateLineAmount").Value);
                lineItem.LineAmount = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "LineAmount").Value);
                lineItem.TMVCustomerLineNum = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "CustomerLineNum").Value);

                lineItem.TMVAdjustmentAmount = GetPropertyValue(lineItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVAdjustmentAmount").Value);
                lineItem.ExtensionProperties = null;

                SetupSalesLineDiscountsExtentionProperties(lineItem);
            }
        }
        private void SetupSalesLineDiscountsExtentionProperties(ErpSalesLine salesLine)
        {
            foreach (var discountItem in salesLine.DiscountLines)
            {
                discountItem.TMVPriceOverrideReasonCode =
                    GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVPriceOverrideReasonCode").Value);

                if (!string.IsNullOrEmpty(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVTargetAmount").Value)))
                {
                    discountItem.TMVTargetAmount =
                        Convert.ToDecimal(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "TMVTargetAmount").Value));
                }

                if (!string.IsNullOrEmpty(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ValidFrom").Value)))
                {
                    discountItem.ValidFrom =
                        // Convert.ToDateTime(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ValidFrom").Value));
                        DateTime.Parse(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ValidFrom").Value), CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ValidTo").Value)))
                {
                    discountItem.ValidTo =
                        // Convert.ToDateTime(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ValidTo").Value));
                        DateTime.Parse(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ValidTo").Value), CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ContractValidFrom").Value)))
                {
                    discountItem.ContractValidFrom =
                        // Convert.ToDateTime(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ContractValidFrom").Value));
                        DateTime.Parse(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ContractValidFrom").Value), CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ContractValidTo").Value)))
                {
                    discountItem.ContractValidTo =
                        // Convert.ToDateTime(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ContractValidTo").Value));
                        DateTime.Parse(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ContractValidTo").Value), CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ContractCalculateFrom").Value)))
                {
                    discountItem.ContractCalculateFrom =
                        // Convert.ToDateTime(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ContractCalculateFrom").Value));
                        DateTime.Parse(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ContractCalculateFrom").Value), CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ContractCalculateTo").Value)))
                {
                    discountItem.ContractCalculateTo =
                        // Convert.ToDateTime(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ContractCalculateTo").Value));
                        DateTime.Parse(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "ContractCalculateTo").Value), CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "DiscountMethod").Value)))
                {
                    discountItem.DiscountMethod =
                        Convert.ToInt32(GetPropertyValue(discountItem.ExtensionProperties.FirstOrDefault(x => x.Key == "DiscountMethod").Value));
                }


                discountItem.ExtensionProperties = null;
            }
        }
        public static string GetPropertyValue(ErpCommercePropertyValue Value)
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
        private ErpCustInvoiceJour getErpCustInvoiceJour(CustInvoiceJour custInvoiceJour)
        {
            ErpCustInvoiceJour erpCustInvoiceJour = new ErpCustInvoiceJour();
            erpCustInvoiceJour.ContractInvoiceEndDate = custInvoiceJour.ContractInvoiceEndDate;
            erpCustInvoiceJour.ContractInvoiceStartDate = custInvoiceJour.ContractInvoiceStartDate;
            erpCustInvoiceJour.CurrencyCode = custInvoiceJour.CurrencyCode;
            erpCustInvoiceJour.CustomerRef = custInvoiceJour.CustomerRef;
            erpCustInvoiceJour.InvoiceAccount = custInvoiceJour.InvoiceAccount;
            erpCustInvoiceJour.InvoiceAmount = custInvoiceJour.InvoiceAmount;
            erpCustInvoiceJour.InvoiceDate = custInvoiceJour.InvoiceDate;
            erpCustInvoiceJour.InvoiceId = custInvoiceJour.InvoiceId;
            erpCustInvoiceJour.InvoicingName = custInvoiceJour.InvoicingName;
            erpCustInvoiceJour.LicenseId = custInvoiceJour.LicenseId;
            erpCustInvoiceJour.RecId = custInvoiceJour.RecId;
            erpCustInvoiceJour.SalesId = custInvoiceJour.SalesId;
            erpCustInvoiceJour.SalesType = custInvoiceJour.SalesType;
            erpCustInvoiceJour.TMVAutoProlongation = custInvoiceJour.TMVAutoProlongation;
            erpCustInvoiceJour.TMVDistributorAccount = custInvoiceJour.TMVDistributorAccount;
            erpCustInvoiceJour.TMVIndirectCustomer = custInvoiceJour.TMVIndirectCustomer;
            erpCustInvoiceJour.TMVPurchOrderFormNum = custInvoiceJour.TMVPurchOrderFormNum;
            erpCustInvoiceJour.TMVResellerAccount = custInvoiceJour.TMVResellerAccount;

            return erpCustInvoiceJour;
        }
        private PagedResult<Affiliation> AsyncGetRetailAffiliations()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            // Query Result Settings
            QueryResultSettings queryResultSettings = new QueryResultSettings();
            queryResultSettings.Paging = new PagingInfo();
            queryResultSettings.Paging.Top = 1000;
            queryResultSettings.Paging.Skip = 0;

            var affiliataionsReturn = ECL_GetRetailAffiliations(queryResultSettings);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            // Return the SalesOrder object
            return affiliataionsReturn;
        }
        public ErpReactivateContract ReactivateContract(string pacLicenseList, string subscriptionStartDate)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpReactivateContract erpReactivateContract = new ErpReactivateContract(false, string.Empty, string.Empty);

            var reactivateContractResponse = ECL_TV_ReactivateContract(pacLicenseList, subscriptionStartDate);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            if (reactivateContractResponse != null)
            {
                erpReactivateContract.Success = (bool)reactivateContractResponse.Status;
                erpReactivateContract.Message = reactivateContractResponse.Message;
                erpReactivateContract.Result = reactivateContractResponse.Result;
            }
            return erpReactivateContract;
        }
        public ErpCancelIngramOrderResponse CancelIngramOrder(string prNumber, string salesId, DateTimeOffset orderDate)
        {
            ErpCancelIngramOrderResponse erpResponse = new ErpCancelIngramOrderResponse(false, IngramCancelTerminationCodes.Other, string.Empty, string.Empty);
            try
            {
                var response = ECL_TV_CancelIngramOrder(prNumber, salesId, orderDate);

                var cancelTerminationCode = IngramCancelTerminationCodes.Other;
                if (!string.IsNullOrEmpty(response.Code) && Enum.IsDefined(typeof(IngramCancelTerminationCodes), response.Code))
                    cancelTerminationCode = (IngramCancelTerminationCodes)Enum.Parse(typeof(IngramCancelTerminationCodes), response.Code);

                erpResponse = new ErpCancelIngramOrderResponse((bool)response.Status, cancelTerminationCode, response.Message, response.SalesId);
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                erpResponse = new ErpCancelIngramOrderResponse(false, IngramCancelTerminationCodes.Other, exp.Message, null);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, Guid.NewGuid().ToString());
                erpResponse = new ErpCancelIngramOrderResponse(false, IngramCancelTerminationCodes.Other, message, null);
            }
            return erpResponse;
        }

        public ErpChangeIngramOrderResponse ChangeIngramOrder(string salesOrderXML)
        {
            ErpChangeIngramOrderResponse erpResponse = new ErpChangeIngramOrderResponse(false, string.Empty, string.Empty);
            try
            {
                var response = ECL_TV_ChangeIngramOrder(salesOrderXML);
                erpResponse = new ErpChangeIngramOrderResponse((bool)response.Status, response.Message, response.Result);
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                erpResponse = new ErpChangeIngramOrderResponse(false, exp.Message, null);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, Guid.NewGuid().ToString());
                erpResponse = new ErpChangeIngramOrderResponse(false, message, null);
            }
            return erpResponse;
        }

        public ErpTransferIngramOrderResponse TransferIngramOrder(string salesOrderXML)
        {
            ErpTransferIngramOrderResponse erpResponse = new ErpTransferIngramOrderResponse(false, IngramTransferCodes.Other, string.Empty, string.Empty, null);
            try
            {
                var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();

                var response = Task.Run(async () => await salesOrderManager.ECL_TV_TransferIngramOrder(salesOrderXML, baseCompany)).Result;

                IngramTransferCodes ingramTransferCode = IngramTransferCodes.None;
                if (!string.IsNullOrEmpty(response.Code) && Enum.IsDefined(typeof(IngramTransferCodes), response.Code))
                    ingramTransferCode = (IngramTransferCodes)Enum.Parse(typeof(IngramTransferCodes), response.Code);

                erpResponse = new ErpTransferIngramOrderResponse((bool)response.Status, ingramTransferCode, response.Message, response.Result, response.RenewalDate);
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                erpResponse = new ErpTransferIngramOrderResponse(false, IngramTransferCodes.Other, exp.Message, null, null);
            }
            catch (Exception exp)
            {
                var message = CommerceLinkLogger.LogException(currentStore, exp, MethodBase.GetCurrentMethod().Name, Guid.NewGuid().ToString());
                erpResponse = new ErpTransferIngramOrderResponse(false, IngramTransferCodes.Other, message, null, null);
            }
            return erpResponse;
        }

        #endregion

        #region Inner Classes
        class CustInvoiceJour
        {
            public string RecId { get; set; }
            public string InvoiceId { get; set; }
            public string SalesId { get; set; }
            public string SalesType { get; set; }
            public string InvoiceDate { get; set; }
            public string CurrencyCode { get; set; }
            public string InvoiceAmount { get; set; }
            public string InvoiceAccount { get; set; }
            public string InvoicingName { get; set; }
            public string TMVResellerAccount { get; set; }
            public string TMVDistributorAccount { get; set; }
            public string TMVIndirectCustomer { get; set; }
            public string ContractInvoiceStartDate { get; set; }
            public string ContractInvoiceEndDate { get; set; }
            public string TMVAutoProlongation { get; set; }
            public string TMVPurchOrderFormNum { get; set; }
            public string CustomerRef { get; set; }
            public string LicenseId { get; set; }
        }

        class CustInvoiceJours
        {
            public List<CustInvoiceJour> CustInvoiceJour { get; set; }
        }

        #endregion

        #region RetailServer API

        [Trace]
        private ChangeIngramOrderResponse ECL_TV_ChangeIngramOrder(string salesOrderXML)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_ChangeIngramOrder(salesOrderXML, baseCompany)).Result;
        }

        [Trace]
        private CancelIngramOrderResponse ECL_TV_CancelIngramOrder(string prNumber, string salesId, DateTimeOffset orderDate)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_CancelIngramOrder(prNumber, salesId, orderDate, baseCompany)).Result;
        }

        [Trace]
        private ReactivateContractResponse ECL_TV_ReactivateContract(string pacLicenseList, string subscriptionStartDate)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();

            ReactivateContractResponse reactivateContractResponse = Task.Run(async () =>
                await salesOrderManager.ECL_TV_ReactivateContract(pacLicenseList, subscriptionStartDate, baseCompany)).Result;
            return reactivateContractResponse;
        }
        [Trace]
        private PagedResult<Affiliation> ECL_GetRetailAffiliations(QueryResultSettings queryResultSettings)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            PagedResult<Affiliation> affiliataionsReturn =
                Task.Run(async () => await salesOrderManager.ECL_GetRetailAffiliations(queryResultSettings)).Result;
            return affiliataionsReturn;
        }
        [Trace]
        private async Task<PagedResult<SalesOrder>> ECL_SearchSalesOrders(QueryResultSettings queryResultSettings,
            SalesOrderSearchCriteria salesOrderSearchCriteria)
        {
            throw new NotImplementedException();
            //var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            ////++RSCall
            //PagedResult<SalesOrder> salesOrderReturn =
            //    await salesOrderManager.ECL_SearchSalesOrders(salesOrderSearchCriteria, queryResultSettings);
            //return salesOrderReturn;
        }
        [Trace]
        private GetSaleOrderStatusesResponse ECL_RTS_GetSaleOrderStatuses(string channelReferenceIds, string salesIds, string fromDateOff, string toDateOff, long channelId)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return salesOrderManager.ECL_RTS_GetSaleOrderStatuses(channelReferenceIds, salesIds, fromDateOff, toDateOff, channelId, baseCompany).Result;
        }

        [Trace]
        private string ECL_UploadSalesOrder(SalesOrder salesOrder, List<CommerceProperty> variantIds)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            //++RSCall
            var res = Task.Run(async () => await salesOrderManager.ECL_UploadSalesOrder(salesOrder, variantIds, baseChannelId)).Result.Result;
            return res;
        }

        [Trace]
        private TransferPartnerContractResponse ECL_TV_TransferPartnerContract(string xmlRequest)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            var rsResponse = Task.Run(async () => await salesOrderManager.ECL_TV_TransferPartnerContract(xmlRequest, baseCompany)).Result;
            return rsResponse;
        }
        //[Trace]
        //private CreatePaymentJournalResponse ECL_TV_CreatePaymentJournal(string xmlRequest)
        //{
        //    //var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
        //    //var rsResponse = Task.Run(async () => await salesOrderManager.ECL_TV_CreatePaymentJournal(xmlRequest, baseCompany))
        //    //    .Result;
        //    //return rsResponse;
        //}
        [Trace]
        private CreateNewContractLinesResponse ECL_TV_CreateNewContractLines(string createNewContractLinesRequest)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();

            var rsResponse = Task.Run(async () =>
                await salesOrderManager.ECL_TV_CreateNewContractLines(createNewContractLinesRequest, baseCompany)).Result;
            return rsResponse;
        }
        [Trace]
        private GetOrValidatePriceInformationResponse ECL_TV_GetOrValidatePriceInformation(string priceOperationXMLRequest)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            var rsResponse = Task.Run(async () =>
                await salesOrderManager.ECL_TV_GetOrValidatePriceInformation(priceOperationXMLRequest, baseCompany)).Result;
            return rsResponse;
        }
        [Trace]
        private ContractSwitchMigrateResponse ECL_TV_ContractSwitchMigrate(string processContractOperationRequest)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();

            var rsResponse = Task.Run(async () =>
                await salesOrderManager.ECL_TV_ContractSwitchMigrate(processContractOperationRequest, baseCompany)).Result;
            return rsResponse;
        }
        [Trace]
        private GetContractSalesorderResponse ECL_TV_GetContractSalesorder(ContractSalesorderRequest request,
            IEnumerable<string> requestStatuses)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            GetContractSalesorderResponse rsResponse = Task.Run(async () => await salesOrderManager.ECL_TV_GetContractSalesorder(
                request.SalesOrderId
                , request.OfferType, request.CustomerAccount, baseCompany, request.isActive, requestStatuses,
                request.LicenseNumber
                , baseChannelId, request.SmallResponse, request.ChannelReferenceId)).Result;
            return rsResponse;
        }
        //[Trace]
        //private GetContractInvoicesResponse ECL_GetContractInvoices(ContractInvoicesRequest request)
        //{
        //    //var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();

        //    //EdgeAXCommerceLink.RetailProxy.Extensions.GetContractInvoicesResponse rsResponse = Task.Run(async () =>
        //    //    await salesOrderManager.ECL_GetContractInvoices(request.SalesOrderId, baseCompany, request.InvoiceId,
        //    //        request.CustomerAccount)).Result;
        //    //return rsResponse;
        //}
        [Trace]
        private ValidateVATNumberResponse ECL_TV_ValidateVATNumber(ErpValidateVATNumberRequest request)
        {
            var saleOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            var response = Task.Run(async () =>
                await saleOrderManager.ECL_TV_ValidateVATNumber(request.VATNumber, request.CountryId, baseCompany)).Result;
            return response;
        }
        [Trace]
        private AddPaymentLinkForInvoiceResponse ECL_TV_AddPaymentLinkForInvoice(ErpAddPaymentLinkForInvoiceRequest request)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            var rsResponse = Task.Run(async () => await salesOrderManager.ECL_TV_AddPaymentLinkForInvoice(
                request.Customer.CustomerNo, request.TenderLine.Amount, request.TenderLine.CardTypeId,
                request.TenderLine.MaskedCardNumber, request.TenderLine.Currency, "", request.TransactionId,
                request.TenderLine.TenderTypeId, "", request.TransactionDate, DateTime.Now, request.TenderLine.CardToken,
                request.TenderLine.Authorization, request.InvoiceId, request.SalesId, baseCompany,
                request.TenderLine.CardOrAccount, request.TenderLine.IBAN, request.TenderLine.SwiftCode, request.TenderLine.BankName,
                request.TenderLine.PspReference, request.TenderLine.Alipay.BuyerId, request.TenderLine.Alipay.BuyerEmail,
                request.TenderLine.Alipay.OutTradeNo, request.TenderLine.Alipay.TradeNo, request.ChannelReferenceId
                )).Result;
            return rsResponse;
        }
        [Trace]
        private AddPaymentLinkForInvoiceResponse ECL_TV_AddPaymentLinkForInvoiceBoleto(
            ErpAddPaymentLinkForInvoiceBoletoRequest request)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            var rsResponse = Task.Run(async () =>
                await salesOrderManager.ECL_TV_AddPaymentLinkForInvoiceBoleto(request.InvoiceId, request.SalesId,
                    request.Payment.BoletoXml, baseCompany,request.ChannelReferenceId)).Result;
            return rsResponse;
        }
        [Trace]
        private CreateProductLicenseResponse ECL_TV_CreateProductLicense(List<CreateLicenseRequest> createLicenseRequest)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            var createLicenseResponse = Task.Run(async () =>
                await salesOrderManager.ECL_TV_CreateProductLicense(createLicenseRequest, baseCompany)).Result;
            return createLicenseResponse;
        }
        //[Trace]
        //private CloseExistingOrderResponse ECL_CloseExistingOrder(string salesId, string pacLicense,
        //    string disablePacLicenseOfSalesLines)
        //{
        //    //var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
        //    //CloseExistingOrderResponse response = Task.Run(async () =>
        //    //        await salesOrderManager.ECL_CloseExistingOrder(salesId, pacLicense, baseCompany,
        //    //            disablePacLicenseOfSalesLines))
        //    //    .Result;
        //    //return response;
        //}
        //[Trace]
        //private ChangeContractPaymentMethodResponse ECL_RTS_ChangeContractPaymentMethod(string salesId,
        //    long newPaymentMethodRecId, string tenderTypeId, long bankAccountRecId)
        //{
        //    //var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
        //    //ChangeContractPaymentMethodResponse response = Task.Run(async () =>
        //    //    await salesOrderManager.ECL_RTS_ChangeContractPaymentMethod(salesId, newPaymentMethodRecId, tenderTypeId,
        //    //        bankAccountRecId, baseCompany)).Result;
        //    //return response;
        //}

        [Trace]
        private CheckoutProcessContractOperationResponse ECL_TV_CheckoutProcessContractOperation(string requestXml)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_CheckoutProcessContractOperation(requestXml, baseCompany)).Result;
        }

        [Trace]
        private ContractRenewalResponse ECL_TV_ContractRenewal(string xmlRequest)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_ContractRenewal(xmlRequest, baseCompany)).Result;
        }

        [Trace]
        private GetBoletoUrlResponse ECL_TV_GetBoletoUrl(ErpGetBoletoUrlRequest request)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_GetBoletoUrl(request.InvoiceId, baseCompany)).Result;
        }

        [Trace]
        private UpdateCustomerPortalLinkResponse ECL_TV_UpdateCustomerPortalLink(string xmlRequest)
        {
            var salesOrderManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ISalesOrderManager>();
            return Task.Run(async () => await salesOrderManager.ECL_TV_UpdateCustomerPortalLink(xmlRequest, baseCompany)).Result;
        }
        #endregion
    }
}

