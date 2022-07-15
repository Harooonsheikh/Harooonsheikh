using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using AutoMapper;
using Microsoft.Dynamics.Commerce.Runtime;
using Microsoft.Dynamics.Commerce.Runtime.Client;
using Microsoft.Dynamics.Commerce.Runtime.DataModel;
using Microsoft.Dynamics.Commerce.Runtime.TransactionService;
using VSI.EdgeCommerceConnector.adptAX2012R3;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using System.Collections.ObjectModel;
using VSI.Commerce.Runtime;
using VSI.Commerce.Runtime.Entities;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.AXCommon;
using System.Threading;
using Newtonsoft.Json;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{/* Please see the dutch code for reference
    public class SaleOrderController_Dutch : BaseController, ISaleOrderController
    {
        public SaleOrderController_Dutch()
        {
        }

        private long AssignAXAddress(ErpAddress address, string customerAccount)
        {
            if (!string.IsNullOrEmpty(address.EcomAddressId))
            {
                IntegrationKey erpKey = null;
                erpKey = IntegrationManager.GetErpKey(Entities.CustomerAddress, address.EcomAddressId);
                if (erpKey != null)
                {
                    return Convert.ToInt64(erpKey.ErpKey);
                }
                else
                {
                    var customerController = new CustomerController();
                    customerController.SaveAddresses(customerAccount, new List<ErpAddress> { address });
                    erpKey = IntegrationManager.GetErpKey(Entities.CustomerAddress, address.EcomAddressId);
                    if (erpKey != null)
                    {
                        return Convert.ToInt64(erpKey.ErpKey);
                    }
                }
            }
            else
            {
                throw new NullReferenceException("Magento delivery address id not found. Sales order cannot be processed");
            }
            return 0;
        }
        private string AssignCustomer(ErpCustomer customer)
        {
            //  return  "004221"; // hard coded because of customer changes.
            if (!string.IsNullOrEmpty(customer.EcomCustomerId))
            {
                IntegrationKey customerKey = null;
                customerKey = IntegrationManager.GetErpKey(Entities.Customer, customer.EcomCustomerId);
                if (customerKey == null)
                {
                    if(customer.Addresses.Any() && string.IsNullOrEmpty(customer.Addresses[0].EcomAddressId))
                    {
                        customer.Addresses.Clear();
                    }

                    var customerController = new CustomerController();
                    customerController.CreateCustomer(new List<ErpCustomer> { customer });
                    customerKey = IntegrationManager.GetErpKey(Entities.Customer, customer.EcomCustomerId);
                    return customerKey.Description;
                    //string message = String.Format("Customer with Magento customerid : {0} is not found. Cannot Sync Sales Order", customerId);
                    //CustomLogger.LogException(new Exception(message));
                    //throw new NullReferenceException(message);
                }
                else
                {
                    return customerKey.Description;
                }
            }
            string message = String.Format("Customer with Magento customerid : {0} is not found. Cannot Sync Sales Order", customer.EcomCustomerId);
            CustomLogger.LogException(new Exception(message));
            throw new NullReferenceException(message);
        }
        public List<ErpSalesOrderStatus> GetSalesOrderStatusUpdate()
        {

            try
            {
                //process to get sales order updates
                // check integration key for sales order other than complete.
                // get one by one and check if previous sales order status is different
                // add in the list of updated sales order 
                // send back to Magento
                // var inCompleteOrder = IntegrationManager.GetComKey


                TransactionServiceClient tsClient = new TransactionServiceClient(CommerceRuntimeHelper.RequestContext);

                var salesOrderStatuses = new List<ErpSalesOrderStatus>();

                int saleOrderStausUpdateTimeFrameInDays = Convert.ToInt32(ConfigurationHelper.SaleOrderStausUpdateTimeFrameInDays);

                //get sales order in date range
                string fromDate = DateTime.Now.Date.AddDays(-1 * saleOrderStausUpdateTimeFrameInDays).ToString("yyyy-MM-dd HH:mm:ss"); //new DateTime(2015, 07, 10).ToString("yyyy-MM-dd h:mm:ss"); 
                string toDate = DateTime.Now.Date.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss"); //new DateTime(2015, 07, 28).ToString("yyyy-MM-dd h:mm:ss");
                var containerArray = tsClient.InvokeExtensionMethod("VSIGetSalesOrderStatus", fromDate, toDate, CommerceRuntimeHelper.ChannelId);
                CustomLogger.LogTraceInfo(string.Format("{0}-{1}-{2}", fromDate, toDate, CommerceRuntimeHelper.ChannelId));
                var lengthOfContainer = containerArray.Count;
                for (var index = 0; index < lengthOfContainer; index++)
                {
                    string item = containerArray[index].ToString();
                    try
                    {
                        CustomLogger.LogTraceInfo(string.Format("Order Status {0}", item));
                        var values = item.Split(',');
                        var data = new ErpSalesOrderStatus
                        {
                            ChannelRefId = values[0],
                            Status = values[1].Trim(),
                            CustomerAcc = values[2],
                            SalesId = values[3]
                        };
                        index = index++;
                        salesOrderStatuses.Add(data);
                    }
                    catch (Exception ex)
                    {
                        // absorb exception and continue for other
                        CustomLogger.LogException(new Exception("error in parsing data for " + containerArray[index] + " " + ex.Message));                        
                    }

                }

                salesOrderStatuses.ForEach(so =>
                {
                    var key = IntegrationManager.GetErpKey(Entities.SalesOrderStatus, so.ChannelRefId);
                    so.Notify = true;

                    if (key != null && key.ErpKey == so.Status && (key.Description == "complete" || key.Description == "canceled"))
                    {
                        so.Notify = false;
                    }

                });


                List<ErpShipmentItem> shipmentItems = new List<ErpShipmentItem>();

                //get order shipments
                salesOrderStatuses.Where(so => so.Notify).ToList().ForEach(o =>
                {
                    try
                    {
                        shipmentItems.Clear();
                        if (!string.IsNullOrEmpty(o.SalesId))
                        {
                            CustomLogger.LogTraceInfo(string.Format("calling tracking for order {0}, Magento Order {1}  ", o.SalesId, o.ChannelRefId));
                            var shipmentContainer = tsClient.InvokeExtensionMethod("VSIGetSalesOrderShipmentTracking", o.SalesId);
                            if (shipmentContainer.Any())
                            {
                                o.Shipments = new List<ErpShipment>();
                                shipmentContainer.ToList().ForEach(s =>
                                {
                                    CustomLogger.LogTraceInfo(string.Format("Shipment of order {0}, Magento Order{1}, ShipmentInfo {2}  ", o.SalesId, o.ChannelRefId, s));

                                    var values = s.ToString().Split(',');
                                    decimal qty = 0;

                                    decimal.TryParse(values[3], out qty);
                                    shipmentItems.Add(new ErpShipmentItem
                                    {
                                        ShipmentId = values[0],
                                        TrackingNo = values[1],
                                        SKU = values[2],
                                        Qty = Convert.ToInt32(qty)
                                    });
                                });
                                var shipments = shipmentItems.GroupBy(gr => gr.ShipmentId).ToList();
                                shipments.ToList().ForEach(gr =>
                                {
                                    o.Shipments.Add(new ErpShipment
                                    {
                                        ShipmentId = gr.Key,
                                        Containers = gr.ToList()
                                    });
                                });
                            }

                        }
                    }
                    catch (CommunicationException)
                    {
                        // if service contains no data, it throws exception "service does not contain data. So we are not logging it"
                    }
                    catch (Exception ex)
                    {
                        // absorb exception and continue for other orders
                        CustomLogger.LogException(new Exception("error in parsing data for" + o.SalesId + " " + ex.Message));
                    }
                });

                //Adding Shipment = null check to avoid "Cannot do shipment error"
                var salesOrdersToNotify = salesOrderStatuses.Where(o => o.Notify && o.Shipments != null).ToList();

                return salesOrdersToNotify;
            }
            catch (CommunicationException)
            {
                return new List<ErpSalesOrderStatus>();
                // if service contains no data, it throws exception "service does not contain data. So we are not logging it"         
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void UpdateStatusIntegration(Dictionary<string, string> updatedOrders)
        {
            try
            {
                updatedOrders.ToList()
                    .ForEach(o => { IntegrationManager.UpdateIntegrationKey(Entities.SaleOrder, "", o.Key, o.Value); });
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
            }
        }
        public bool CreateSaleOrders(ErpSalesOrder salesOrderParam)
        {
            if (salesOrderParam == null) throw new Exception("No sales orders found");

            // if order already processed and file still exists in directory, dont process it again.
            if (IntegrationManager.GetErpKey(Entities.SaleOrder, salesOrderParam.Key.ComKey) != null)
                return true;

            salesOrderParam.Id = string.Concat("MG_" + Guid.NewGuid());
            int lineNo = 1;
            salesOrderParam.SalesLines.ToList().ForEach(line => line.LineNumber = lineNo++);


            //If custome code is empty, assign channel default customer
            if (string.IsNullOrEmpty(salesOrderParam.CustomerId))
            {
                salesOrderParam.CustomerId = currentChannelState.OnlineChannelInstance.DefaultCustomerAccount;
            }
            //if customer has valid ecom id, it should be synced with AX. If not, we will create it here
            else
            {
                for (int i = 0; i < 3; i++)
                {


                    try
                    {
                        string vatNum = salesOrderParam.isTaxExemptCustomer == "1" ? Configuration.ConfigurationHelper.TaxExemptNumber : string.Empty;
                        salesOrderParam.CustomerId = this.AssignCustomer(new ErpCustomer
                               {
                                   EcomCustomerId = salesOrderParam.CustomerId,
                                   FirstName = salesOrderParam.CustomerName,
                                   Email = salesOrderParam.CustomerEmail,
                                   TaxGroup = ConfigurationHelper.OnlineCustomerTaxGroup,
                                   CustomerGroup = ConfigurationHelper.AXCustomerGroup,
                                   CurrencyCode = ConfigurationHelper.CustomerDefaultCurrencyCode,
                                   Addresses = new List<ErpAddress> { salesOrderParam.ShippingAddress },
                                   VatNumber = vatNum
                               });
                        break;
                    }
                    catch (Exception)
                    {                                              
                        if (i >= 2)
                            throw;
                        Thread.Sleep(2000);
                    }
                }
            }


            // Fix for Ticket 23867 - Address No Longer Effective

            //Get AX address based on magento key. Address should be there in Integration Key. If not RTS creates it and save in Integration k

            //if (salesOrderParam.ShippingAddress != null && salesOrderParam.CustomerId != currentChannelState.OnlineChannelInstance.DefaultCustomerAccount)
            //{
            //    if (!string.IsNullOrEmpty(salesOrderParam.ShippingAddress.EcomAddressId))
            //    {
            //        try
            //        {
            //            salesOrderParam.ShippingAddress.RecordId = this.AssignAXAddress(salesOrderParam.ShippingAddress, salesOrderParam.CustomerId);
            //        }
            //        catch (Exception ex)
            //        {
            //            // If customer fails to add address in AX, Sales order should not fail. So we absorb exception here
            //            CustomLogger.LogTraceInfo(ex.Message);
            //        }
            //    }
            //}

            Mapper.AssertConfigurationIsValid<ErpMappingConfiguration>();
            //Mapper.CreateMap<ErpSalesOrder, SalesOrder>()
            //    .ConstructUsing((Func<ErpSalesOrder, SalesOrder>)(x => new SalesOrder()))
            //    .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore())
            //    .ForMember(dest => dest.ShippingAddress, opt => opt.Ignore());
            //Mapper.CreateMap<ErpSalesLine, SalesLine>()
            //    .ForMember(dest => dest.ExtensionProperties, opt => opt.Ignore());
            //Mapper.CreateMap<ErpTenderLine, TenderLine>()
            //    .ForMember(x => x.ExtensionProperties, opt => opt.Ignore());

            //var mappingDictionary = Mapper.GetAllTypeMaps();

            var salesOrder = Mapper.Map<ErpSalesOrder, SalesOrder>(salesOrderParam);

            if (salesOrderParam.ShippingAddress != null)
            {
                salesOrder.ShippingAddress = CreateAddressFromErpAddress(salesOrderParam.ShippingAddress);
                salesOrder.ContactInformationCollection = new Collection<ContactInformation>();
                //adding phone no for address
                if (!string.IsNullOrEmpty(salesOrder.ShippingAddress.Phone))
                {
                    salesOrder.ContactInformationCollection.Add(new ContactInformation
                    {
                        ContactInformationType = ContactInformationType.Phone,
                        ContactInformationTypeValue = (int)ContactInformationType.Phone,
                        Value = salesOrder.ShippingAddress.Phone
                    });
                    //salesOrder.ShippingAddress.Email = salesOrder.ReceiptEmail;
                }
            }

            var helper = new SalesOrderHelper
            {
                InventoryLocation = currentChannelState.OnlineChannelInstance.InventoryLocationId,

            };
            if (salesOrderParam.PaymentGiftCards != null && salesOrderParam.PaymentGiftCards.Any())
            {
                salesOrder.TenderLines.Clear();
                salesOrderParam.PaymentGiftCards.ForEach(g =>
                {
                    salesOrder.TenderLines.Add(new TenderLine
                    {
                        Amount = g.Amount,
                        TenderTypeId = VSI.EDGEAXConnector.Data.PaymentModesDAL.GetPaymentMothodByEcommerceKey("GiftCard").ErpValue,
                        GiftCardId = g.Card
                    });
                });
            }

            salesOrder = helper.PopulateSalesOrder(salesOrder, salesOrderParam);

            salesOrder.ChannelId = CommerceRuntimeHelper.ChannelId;
            salesOrder.InventoryLocationId = currentChannelState.OnlineChannelInstance.InventoryLocationId;
            salesOrder.ChannelCurrencyExchangeRate = 1.00M;

            // Gift card unlocking should be last operation before creating sales transction 
            // CU9 has issue with Validate
            // salesOrder.ValidateSalesOrder(Utility._RequestContext);


            //++ Saving object to Json before Uploading

            var orderJson = salesOrder.SerializeToJson(1);
            CustomLogger.LogTraceInfo(string.Format("Json Order :{0}", orderJson), salesOrder.ChannelReferenceId);

            //Mapping Dictonary
            /*
            var mappingDictionary = AutoMapper.Mapper.GetAllTypeMaps();

            var mappingJson = JsonConvert.SerializeObject(mappingDictionary, Newtonsoft.Json.Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                //ReferenceLoopHandling = ReferenceLoopHandling.Serialize
                                PreserveReferencesHandling = PreserveReferencesHandling.Objects
                            });

            CustomLogger.LogTraceInfo(string.Format("Json Mapping :{0}", mappingJson), salesOrder.ChannelReferenceId);
            */


    /*****************
     * 
     * Uncomment this code for user
     * ****************/

    /*
            var orderManager = OrderManager.Create(CommerceRuntimeHelper.CommerceRuntime);
            try
            {
                if (salesOrderParam.PaymentGiftCards != null && salesOrderParam.PaymentGiftCards.Any())
                {
                    string giftCardCurrency = string.Empty;
                    decimal balance = 0;
                    TransactionServiceClient tsClient = new TransactionServiceClient(CommerceRuntimeHelper.RequestContext);
                    salesOrderParam.PaymentGiftCards.ForEach(g =>
                    {
                        tsClient.UnlockGiftCard(g.Card);                        
                        tsClient.PayGiftCard(g.Card, g.Amount, ConfigurationHelper.DefaultAXTenderLineCurrencyCode, VSI.EDGEAXConnector.AXCommon.CommerceRuntimeHelper.ChannelId, "", "", salesOrder.Id, "", out giftCardCurrency, out balance);                      
                    });
                }

                orderManager.UploadOrder(salesOrder);
                IntegrationManager.CreateIntegrationKey(Entities.SaleOrder, salesOrder.Id, salesOrderParam._integrationKey.ComKey,
                    salesOrder.Status.ToString());
            }
            catch (Exception ex)
            {
                //if sales order has unlocked any gift card and order creation failed. Lock all cards again
                if (salesOrderParam.PaymentGiftCards != null && salesOrderParam.PaymentGiftCards.Any())
                {
                    LockSalesOrderGiftCards(salesOrderParam.PaymentGiftCards);
                }
                throw ex;
            }

            try
            {
                SalesOrderExtension SOExt = new SalesOrderExtension();
                SOExt.TRANSACTIONID = salesOrder.Id;
                SOExt.WebsiteOrderFrom = salesOrderParam.WebsiteSource;
                SOExt.MagentoSO = salesOrder.ChannelReferenceId;
                SOExt.TransDate = salesOrder.OrderPlacedDate.Date;
                SOExt.TransTime = Convert.ToInt32(salesOrder.OrderPlacedDate.TimeOfDay.TotalSeconds);


                List<VSI.Commerce.Runtime.Entities.SalesLineExtension> SOLine = new List<VSI.Commerce.Runtime.Entities.SalesLineExtension>() { };
                foreach (ErpSalesLine erpSaleLine in salesOrderParam.ActiveSalesLines)
                {
                    VSI.Commerce.Runtime.Entities.SalesLineExtension SLExt = new Commerce.Runtime.Entities.SalesLineExtension();
                    SLExt.Font = erpSaleLine.MonogramFont;
                    SLExt.LINENUM = erpSaleLine.LineNumber;
                    SLExt.Thread = erpSaleLine.MonogramThread;
                    SLExt.Initials = erpSaleLine.MonogramInitials;
                    SLExt.IsFinalSales = erpSaleLine.IsFinalSales;
                    SOLine.Add(SLExt);
                }

                ProcessSalesOrderLineExtension(SOExt, SOLine);

                SalesOrderAdderssExtension addressEX = new SalesOrderAdderssExtension();
                addressEX.IsResidential = salesOrderParam.ShippingAddress.Residential;
                addressEX.TransactionId = salesOrder.Id;
                addressEX.ShopRunner = salesOrderParam.ShopRunner;
                addressEX.IsInternational = salesOrderParam.isInernationalOrder;
                addressEX.IsBorderFree = salesOrderParam.isBorderFree;
                
                ProcessOrderAddressExtension(addressEX);

                //IntegrationManager.CreateIntegrationKey(Entities.SaleOrder, salesOrder.Id, salesOrderParam.Key.ComKey,
                //    salesOrder.Status.ToString());

            }

            catch (Exception ex)
            {
                //absorbing excetion as main flow worked but customized values couldn't updated
                CustomLogger.LogException(new Exception( string.Format("Order {0} processed but customization didnt work", salesOrder.ChannelReferenceId))+ex.ToString());
            }

            return true;
        }
        private static void LockSalesOrderGiftCards(List<ErpGiftCard> giftCards)
        {
            long channelId = CommerceRuntimeHelper.ChannelId;
            string terminalId = string.Empty;
            string outCardCurrency = string.Empty;
            decimal outBalance = 0;
            TransactionServiceClient tsClient = new TransactionServiceClient(CommerceRuntimeHelper.RequestContext);

            giftCards.ForEach(g =>
              {
                  tsClient.LockGiftCard(g.Card, channelId, terminalId, out outCardCurrency, out outBalance);
              });
        }
        public Address CreateAddressFromErpAddress(ErpAddress erpAddress)
        {
            //DAI-1001 

            string zipCode;

            if (Convert.ToBoolean(ConfigurationHelper.TruncateZipCode).Equals(true))
            {
                
                if (erpAddress.ZipCode.Contains("-"))
                {

                    zipCode = erpAddress.ZipCode.Substring(0, erpAddress.ZipCode.IndexOf("-")).Trim();
                }
                else
                {
                    zipCode = erpAddress.ZipCode;

                }
            }
            else
            {
                zipCode = erpAddress.ZipCode;

            }

            return new Address
            {
                RecordId = erpAddress.RecordId,
                Name = erpAddress.Name,
                Street = erpAddress.Street,
                City = erpAddress.City,
                State = erpAddress.State,
                ZipCode = zipCode,
                Phone = erpAddress.Phone,
                //DAI-866 Starts
                AddressType = (AddressType)ConfigurationHelper.DefaultAXAddressType,
                AddressTypeValue = ConfigurationHelper.DefaultAXAddressType,
                //DAI-866 Ends
                County = erpAddress.County,
                
                ThreeLetterISORegionName = erpAddress.ThreeLetterISORegionName
            };
        }




       /// <summary>
        /// TODO need discussion  ProcessSalesOrderLineExtension
       /// </summary>
       /// <param name="SalesOrder"></param>
       /// <param name="SalesOrderLine"></param>
       /// <returns></returns>
        private int ProcessSalesOrderLineExtension(SalesOrderExtension SalesOrder,List<VSI.Commerce.Runtime.Entities.SalesLineExtension> SalesOrderLine )
        {
                    // Update Sales Order Extension 
             /*****
              * Uncomment this line
              * *****       // return this.currentChannelState.SalesLineExtensionManager.UpdateSalesLineExtension(SalesOrder,SalesOrderLine);
            return 0;
        }
        public int ProcessOrderAddressExtension(SalesOrderAdderssExtension SalesOrderAddressExt)
        {
            try
            {
                return this.currentChannelState.SalesOrderAddressExtensionManager.UpdateSalesOrderAddressExtension(SalesOrderAddressExt);
        
            }
            catch (Exception)
            {
                throw;
            } 
        }
    }
    **/

}