using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Demandware;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
{

    /// <summary>
    /// SaleOrderController class performs SalesOrder related activities.
    /// </summary>
    public class SaleOrderController : BaseController, ISaleOrderController
    {
        XmlSerializer xml = new XmlSerializer(typeof(orders));

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SaleOrderController()
            : base(false)
        {
            // initialization if required
        }

        #endregion

        #region Public Methods

        public ErpSalesOrder GetSalesOrders(string localFile)
        {
            complexTypeOrder dwOrder;
            ErpSalesOrder order = new ErpSalesOrder();
            try
            {
                using (FileStream orderStream = new FileStream(localFile, FileMode.Open))
                {
                    dwOrder = ((orders)xml.Deserialize(orderStream)).order.First();
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogFatal(string.Concat("Exception logged:  : {0}", ex.Message));
                order = null;
                throw ex;
            }

            ProcessOrder(dwOrder, order);

            //Custom For MF 
            //Decrypting payment fields
            //KAR MF customizations
            //ProcessTenderLineDecryption(order);

            //flating product options as product
            if (order.SalesLines != null)
            {
                order = ProcessProductOptionsAsErpSalesLine(order);
            }
            //KAR MF/DW Customization required to send the list price in sales line item. Do we need it in standard flow ?
            // AAC - Infer means deduce from current list price. If false, we have explicit sales price in XML
            if (!configurationHelper.GetSetting(APPLICATION.ERP_AX_InferPeriodicDiscount).BoolValue())
            {
                ProcessSalesLinePeriodicDiscount(order);
            }

            return order;
        }

        public ErpSalesOrder GetSalesOrderFromXML(XmlDocument xmlDoc)
        {
            throw new NotImplementedException();
        }

        private void ProcessOrder(complexTypeOrder dwOrder, ErpSalesOrder order)
        {
            order.Id = dwOrder.orderno;
            order.OrderPlacedDate = dwOrder.orderdate;

            // order.InventoryLocationId = dwOrder.customattributes.Where(ca => ca.attributeid == "InventLocationId").First().value.First();
            // Use for store pick-up
            //order.StoreId = dwOrder.customattributes.Where(ca => ca.attributeid == "InventLocationId").First().value.First();

            order.ChannelReferenceId = dwOrder.originalorderno;
            order.CurrencyCode = dwOrder.currency;
            order.CustomerId = dwOrder.customer.customerno;
            order.CustomerName = dwOrder.customer.customername;
            order.CustomerEmail = dwOrder.customer.customeremail;
            order.ReceiptEmail = dwOrder.customer.customeremail;

            // order.Status = dwOrder.status.orderstatus;
            order.Status = ErpSalesStatus.Created;

            order.ChannelReferenceId = dwOrder.currentorderno; // Duplicate ??
            order.TotalAmount = dwOrder.totals.ordertotal.grossprice;
            order.TaxAmount = dwOrder.totals.ordertotal.tax;
            order.NetAmountWithNoTax = dwOrder.totals.ordertotal.netprice;
            order.NetAmountWithTax = dwOrder.totals.ordertotal.grossprice;

            // Old discount handling
            // order.DiscountAmount = dwOrder.totals.merchandizetotal.priceadjustments.First().grossprice;
            // order.DiscountCode = dwOrder.totals.merchandizetotal.priceadjustments.First().promotionid;

            ProcessDiscountLines(order, dwOrder.totals.merchandizetotal.priceadjustments);
            ProcessSalesLines(order, dwOrder.productlineitems);
            ProcessShipping(order, dwOrder);
            ProcessShipments(order, dwOrder.shipments);
            ProcessPayments(order, dwOrder.payments);

            order.CustomAttributes = toCustomAttributes(dwOrder.customattributes);
        }

        private void ProcessPayments(ErpSalesOrder order, complexTypePayment[] payments)
        {
            order.TenderLines = new List<ErpTenderLine>();
            foreach (complexTypePayment payment in payments)
            {
                var tLine = new ErpTenderLine();
                tLine.TenderTypeId = payment.processorid;
                tLine.Amount = payment.amount;

                if (payment.Item is complexTypeCreditCard)
                {
                    var cc = (complexTypeCreditCard)payment.Item;
                    tLine.CardTypeId = cc.cardtype;
                    tLine.MaskedCardNumber = cc.cardnumber;
                    //tLine.CardOrAccount = cc.cardholder;
                }

                //instantiate due to if below
                tLine.CustomAttributes = new List<KeyValuePair<string, string>>();
                if (payment.Item is complexTypeCustomPaymentMethod)
                {
                    tLine.CustomAttributes.AddRange(toCustomAttributes(((complexTypeCustomPaymentMethod)payment.Item).customattributes));
                }

                tLine.CustomAttributes.Add(new KeyValuePair<string, string>("transaction-id", payment.transactionid));
                tLine.CustomAttributes.AddRange(toCustomAttributes(payment.customattributes));
            }
        }

        private void ProcessShipments(ErpSalesOrder eOrder, complexTypeShipment[] shipments)
        {
            eOrder.Shipments = new List<ErpShipment>();
            foreach (complexTypeShipment ship in shipments)
            {
                ErpShipment eShip = new ErpShipment();
                eShip.ShipmentId = ship.shipmentid;
                eShip.DeliveryMode = ship.shippingmethod;
                eShip.ShippingStatus = ship.status.shippingstatus.ToString();
                eShip.IsGift = ship.gift;

                var shipAddress = new ErpAddress();
                shipAddress.Name = ship.shippingaddress.firstname + " " + ship.shippingaddress.lastname;
                shipAddress.Street = ship.shippingaddress.address1;
                shipAddress.City = ship.shippingaddress.city;
                shipAddress.ZipCode = ship.shippingaddress.postalcode;
                shipAddress.State = ship.shippingaddress.statecode;
                shipAddress.ThreeLetterISORegionName = GetThreeDigitCountryCode(ship.shippingaddress.countrycode);
                shipAddress.Phone = ship.shippingaddress.phone;

                eShip.DeliveryAddress = shipAddress;

                eOrder.Shipments.Add(eShip);
            }
        }

        private void ProcessShipping(ErpSalesOrder eOrder, complexTypeOrder dwOrder)
        {
            eOrder.DeliveryMode = dwOrder.shipments.First().shippingmethod;
            eOrder.DeliveryModeChargeAmount = dwOrder.shippinglineitems.First().netprice;
            eOrder.Shipping_Tax = dwOrder.shippinglineitems.First().tax;

            eOrder.ShippingDiscounts = new List<ErpDiscountLine>();
            foreach (complexTypeShippingLineItem sli in dwOrder.shippinglineitems)
            {
                foreach (complexTypePriceAdjustment slipa in sli.priceadjustments)
                {
                    var sd = new ErpDiscountLine();
                    sd.Amount = slipa.netprice;
                    sd.DiscountCode = slipa.promotionid;
                    sd.OfferId = slipa.Item.ToString();
                    eOrder.ShippingDiscounts.Add(sd);
                }
            }
        }

        private void ProcessSalesLines(ErpSalesOrder eOrder, complexTypeProductLineItem[] productlineitems)
        {
            eOrder.SalesLines = new List<ErpSalesLine>();

            foreach (complexTypeProductLineItem pl in productlineitems)
            {
                ErpSalesLine esl = new ErpSalesLine();
                esl.NetAmount = pl.netprice;
                esl.TaxAmount = pl.tax;
                esl.TotalAmount = pl.grossprice;
                esl.BasePrice = pl.baseprice;
                esl.Price = pl.baseprice;
                esl.LineNumber = pl.position;
                esl.Description = pl.productid;
                esl.ItemId = pl.productid;
                esl.Quantity = Convert.ToDecimal(pl.quantity.Value);
                esl.QuantityOrdered = Convert.ToDecimal(pl.quantity.Value);
                esl.TaxRatePercent = Convert.ToDecimal(pl.taxrate);
                esl.IsGiftCardLine = pl.gift;
                esl.ShipmentId = pl.shipmentid;

                esl.CustomAttributes = toCustomAttributes(pl.customattributes);
                esl.InventoryLocationId = esl.CustomAttributes.First(k => k.Key == "fromStoreId").Value;

                ProcessSalesLineOptions(esl, pl.optionlineitems);

                ProcessSalesLineDiscounts(esl, pl.priceadjustments);

                eOrder.SalesLines.Add(esl);
            }
        }

        private void ProcessSalesLineDiscounts(ErpSalesLine esl, complexTypePriceAdjustment[] priceadjustments)
        {
            esl.DiscountLines = new List<ErpDiscountLine>();
            foreach (complexTypePriceAdjustment lpa in priceadjustments)
            {
                ErpDiscountLine ledl = new ErpDiscountLine();
                ledl.Amount = lpa.netprice;
                ledl.DiscountCode = lpa.promotionid;
                ledl.OfferId = lpa.Item.ToString();
            }
        }

        private void ProcessDiscountLines(ErpSalesOrder eOrder, complexTypePriceAdjustment[] priceadjustments)
        {
            eOrder.OrderDiscounts = new List<ErpDiscountLine>();

            foreach (complexTypePriceAdjustment pa in priceadjustments)
            {
                ErpDiscountLine dLine = new ErpDiscountLine();
                dLine.EffectiveAmount = pa.netprice;
                dLine.DiscountCode = pa.promotionid;
                if (pa.Item is string)
                    dLine.OfferId = (string)pa.Item;
                dLine.EntityName = pa.couponid;
                if (pa.discount.Item is decimal)
                    dLine.Amount = (decimal)pa.discount.Item;
                dLine.OfferName = pa.reasoncode;

                eOrder.OrderDiscounts.Add(dLine);
            }
        }

        private ErpSalesOrder ProcessProductOptionsAsErpSalesLine(ErpSalesOrder order)
        {
            List<ErpSalesLine> lstNewSalesLine = new List<ErpSalesLine>();
            if (order.SalesLines != null)
            {
                foreach (ErpSalesLine line in order.SalesLines)
                {
                    if (line.Options != null)
                    {
                        foreach (ErpSalesLine option in line.Options)
                        {
                            if (!option.ItemId.Equals(configurationHelper.GetSetting(SALESORDER.OptionItem_None_Constant)))
                            {
                                option.ShipmentId = line.ShipmentId;
                                //Maintaining these values from parent product
                                option.RequestedDeliveryDate = line.RequestedDeliveryDate;
                                option.InventoryLocationId = line.InventoryLocationId;
                                option.CustomAttributes = line.CustomAttributes;

                                //holding option parent item variant into comment to get linked item quantity
                                option.Comment += ":" + line.ItemId.Replace(configurationHelper.GetSetting(PRODUCT.SKU_Prefix), "");

                                lstNewSalesLine.Add(option);
                            }
                        }
                        line.Options.Clear();
                    }
                }
                foreach (ErpSalesLine line in lstNewSalesLine)
                {
                    order.SalesLines.Add(line);
                }
            }
            return order;
        }

        //NS:
        //If getting listPrice in order XML then this code will work
        //If not getting then processing preiodic discount in AX Adapter under CreateSalesOrder method
        private void ProcessSalesLinePeriodicDiscount(ErpSalesOrder salesOrderParam)
        {
            salesOrderParam.SalesLines.ToList().ForEach(sl =>
            {
                if (sl.OriginalPrice != null)
                {
                    sl.PeriodicDiscount = Convert.ToDecimal(sl.OriginalPrice - sl.BasePrice);
                }
            });
        }
        #endregion
        private void ProcessSalesLineOptions(ErpSalesLine esl, complexTypeOptionLineItem[] optionlineitems)
        {
            esl.Options = new List<ErpSalesLine>();
            foreach (complexTypeOptionLineItem loli in optionlineitems)
            {
                ErpSalesLine lesl = new ErpSalesLine();
                lesl.NetAmount = loli.netprice;
                lesl.TaxAmount = loli.tax;
                lesl.TotalAmount = loli.grossprice;
                lesl.BasePrice = loli.baseprice;
                lesl.Price = loli.baseprice;
                // lesl.VariantId = loli.valueid;
                lesl.ItemId = loli.productid;
                lesl.Quantity = 1;
                lesl.Comment = "OptionItem";

                lesl.CustomAttributes = toCustomAttributes(loli.customattributes);
                // one attribute goes in a different location
                lesl.InventoryLocationId = lesl.CustomAttributes.First(k => k.Key == "fromStoreId").Value;

                ProcessSalesLineOptionLineDiscountLines(lesl, loli.priceadjustments);

                esl.Options.Add(lesl);
            }
        }
        private void ProcessSalesLineOptionLineDiscountLines(ErpSalesLine leslOption, complexTypePriceAdjustment[] optionPriceadjustments)
        {
            leslOption.DiscountLines = new List<ErpDiscountLine>();
            foreach (complexTypePriceAdjustment leslOptionPA in optionPriceadjustments)
            {
                ErpDiscountLine leslOptionDl = new ErpDiscountLine();
                leslOptionDl.Amount = leslOptionPA.netprice;
                leslOptionDl.DiscountCode = leslOptionPA.promotionid;
                if (leslOptionPA.Item is string)
                    leslOptionDl.OfferId = (string)leslOptionPA.Item;

                leslOption.DiscountLines.Add(leslOptionDl);
            }
        }
        private List<KeyValuePair<string, string>> toCustomAttributes(sharedTypeCustomAttribute[] customattributes)
        {
            var eAttrs = new List<KeyValuePair<string, string>>();
            foreach (sharedTypeCustomAttribute attr in customattributes)
            {
                eAttrs.Add(new KeyValuePair<string, string>(attr.attributeid, attr.value.First().ToString()));
            }
            return eAttrs;
        }
    }
}