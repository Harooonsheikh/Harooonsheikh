using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Dynamics.Commerce.Runtime.Client;
using Microsoft.Dynamics.Commerce.Runtime.DataModel;
using VSI.EdgeCommerceConnector.adptAX2012R3;
using System;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.AXCommon;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class SalesOrderHelper_Dutch
    {
        public string InventoryLocation { get; set; }
        public SalesOrder PopulateSalesOrder(SalesOrder salesOrder, ErpSalesOrder salesOrderParam)
        {
            var order = new SalesOrder();

            if (salesOrder == null) return order;
            // Get header attributes
            order.RecordId = salesOrder.RecordId;
            order.RequestedDeliveryDate = DateTime.UtcNow;
            order.SalesId = "";
            order.BeginDateTime = DateTime.UtcNow;
            order.TransactionType = SalesTransactionType.PendingSalesOrder;
            order.Status = SalesStatus.Created;
            order.DocumentStatus = DocumentStatus.None;
            order.ChannelReferenceId = salesOrder.ChannelReferenceId;
            order.ChannelCurrencyExchangeRate = salesOrder.ChannelCurrencyExchangeRate;
            order.ChannelId = salesOrder.ChannelId;
            order.Name = salesOrder.Name;
            order.StoreId = salesOrder.StoreId;
            order.CustomerId = salesOrder.CustomerId;
            order.ReceiptEmail = salesOrder.ReceiptEmail;
            order.Id = salesOrder.Id;
            order.OrderPlacedDate = GetOrderDateTimeOffset(salesOrderParam.OrderPlacedDate, salesOrderParam.transTime); //salesOrder.OrderPlacedDate;
            order.TotalAmount = salesOrder.TotalAmount;
            order.NetAmountWithNoTax = salesOrder.NetAmountWithNoTax;
            // in magento (using 3rd party payment) sales order can only be processed once amount is fully paid, so sales diff will be 0.
            order.AmountPaid = salesOrder.TotalAmount;
            order.SalesPaymentDifference = order.TotalAmount - order.AmountPaid;
            order.TaxAmount = salesOrder.TaxAmount;
            order.TotalDiscount = salesOrder.TotalDiscount; 
            //order.NetAmountWithNoTax = salesOrder.NetAmount;
            //order.DiscountAmount = salesOrder.DiscountAmount * -1; //discounts are negative from magento
            order.ChargeAmount = salesOrder.ChargeAmount;
            order.InventoryLocationId = salesOrder.InventoryLocationId;
            order.DeliveryMode = salesOrder.DeliveryMode;
            order.DeliveryModeChargeAmount = salesOrder.DeliveryModeChargeAmount;
            order.Status = salesOrder.Status;
            order.SalesLines = salesOrder.SalesLines;
            order.ShippingAddress = salesOrder.ShippingAddress;
            order.ContactInformationCollection = salesOrder.ContactInformationCollection;
            order.DiscountCodes = salesOrder.DiscountCodes;


            //adding shipping charge codes
            if (order.DeliveryModeChargeAmount != null && order.DeliveryModeChargeAmount.Value > 0)
            {
                order.ChargeLines.Add(new ChargeLine()
                {
                    ChargeCode = CodesDAL.GetShippingChargeCode(),
                    Value = order.DeliveryModeChargeAmount.Value,
                    ChargeMethod = ChargeMethod.Fixed,
                    Keep = 1,
                });
            }

            var shopRunnerDlv = DeliveryModesDAL.GetDeliveryModeByKey(Convert.ToInt32(DeliveryPreferenceType.DeliverItemsIndividually));
            if (shopRunnerDlv != null && shopRunnerDlv.ComValue == order.DeliveryMode)
            {
                salesOrderParam.ShopRunner = order.DeliveryMode;
            }
            else
            {
                salesOrderParam.ShopRunner = "";
            }


            // Get lines

            SetupShippingMethod(order);
            PopulateSalesLine(order);


            salesOrderParam.SalesLines.ToList().ForEach(sl =>
            {
                var line = salesOrder.SalesLines.FirstOrDefault(l => l.LineNumber == sl.LineNumber);
                if (line != null)
                {
                    line.ChargeLines = new Collection<ChargeLine>();
                    if (sl.Monogram_Price > 0)
                    {
                        line.ChargeLines.Add(new ChargeLine()
                        {
                            ChargeCode = CodesDAL.GetMonogramCodes(),
                            Value = sl.Monogram_Price,
                            ChargeMethod = ChargeMethod.Fixed,
                            Keep = 1,
                            TaxLines = new Collection<TaxLine>(),
                        });
                        //line.ChargeLines[0].TaxLines.Add(new TaxLine
                        //{
                        //    Amount = sl.Monogram_Tax,
                        //    IsIncludedInPrice = false,
                        //});
                    }
                }
            });

            if (salesOrderParam.Shipping_Tax > 0)
            {
                salesOrder.TaxLines = new Collection<TaxLine>();
                salesOrder.TaxLines.Add(new TaxLine
                {
                    Amount = salesOrderParam.Shipping_Tax,
                    IsIncludedInPrice = false,
                });
            }

            order.TenderLines = salesOrder.TenderLines;
            order.TenderLines.ToList().ForEach(tl => { this.SetupPaymentMethod(tl); });
            return order;
        }

        /// <summary>
        ///     copy sale lines.
        /// </summary>
        /// <param name="order">The sales order.</param>
        private void PopulateSalesLine(SalesOrder order)
        {
            var pickUpStore = DeliveryModesDAL.GetDeliveryModeByKey(Convert.ToInt32(DeliveryPreferenceType.PickupFromStore));
            order.NumberOfItems = order.SalesLines.Sum(s => s.Quantity);
            foreach (var line in order.SalesLines)
            {
                var key = new IntegrationKey();// IntegrationManager.GetErpKey(Entities.Product, line.ItemId);
                var giftCard = CodesDAL.GetGiftCardCode();

                //Setting orderDeliveryMode to all lines
                line.DeliveryMode = order.DeliveryMode;

                if (giftCard != null)
                {
                    if (line.ItemId.ToLower().Equals(giftCard.ComValue.ToLower()))
                    {
                        line.IsGiftCardLine = true;
                        //Assiging Electronic delivery mode to giftcard item
                        //If from eCom we can order physical giftcard then we have to change this line
                        line.DeliveryMode = DeliveryModesDAL.GetErpDeliveryMode(ConfigurationHelper.DefaultAXGiftCardDeliveryMode);
                        line.GiftCardCurrencyCode = CommerceRuntimeHelper.ChannelConfiguration.Currency;  
                    }
                }
                else
                {
                    CustomLogger.LogTraceInfo("Gift card not setup in connector DB");
                }

                if (key == null) throw new NullReferenceException("Product not found in integration DB. Sales order cannot be processed");

                string itemId = string.Empty;
                string variantId = string.Empty;

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
                    throw new Exception("Problem in getting Product from Connector DB");
                }

                //var variant = GetProductFromSku(key.Description);
                line.Variant = string.IsNullOrEmpty(variantId) ? new ProductVariant { VariantId = "" } : new ProductVariant { VariantId = variantId };
                line.ProductId = Convert.ToInt64(key.ErpKey); //variant != null ? variant.DistinctProductVariantId : Convert.ToInt64(line.ItemId); //TODO: Once Products are finalized. Get RecordID
                line.ItemId = itemId;// variant != null ? variant.ItemId : line.ItemId;
                line.InventoryLocationId = this.InventoryLocation;
            //    line.ItemTaxGroupId = CodesDAL.GetTaxCode(TaxGroups.ItemTaxGroup);
            //    line.SalesTaxGroupId = CodesDAL.GetTaxCode(TaxGroups.SalesTaxGroup);
                line.SalesDate = order.OrderPlacedDate;
                
                line.RequestedDeliveryDate = DateTime.UtcNow;
                line.BeginDateTime = DateTime.UtcNow;

                line.SalesOrderUnitOfMeasure = ConfigurationHelper.DefaultAXSalesOrderUnitOfMeasure;                
                

                line.TaxLines = new Collection<TaxLine>
                {
                    new TaxLine
                    {
                        Amount = line.TaxAmount,
                        IsIncludedInPrice = false,
                        TaxCode = line.ItemTaxGroupId // have to confirm tax code is itemtaxgroup?           
                    }
                };

                line.DiscountLines = new Collection<DiscountLine>();
                try
                {
                    line.PeriodicPercentageDiscount = (line.PeriodicDiscount / (line.Price * line.Quantity)) * 100;
                    line.TotalPercentageDiscount = (line.TotalDiscount / ((line.Price - line.PeriodicDiscount) * line.Quantity)) * 100;

                }
                catch (Exception ex)
                {
                    CustomLogger.LogTraceInfo(ex.Message);
                }

                if (line.PeriodicDiscount > 0)
                {
                    DiscountLine discountLine = new DiscountLine
                           {

                               EffectiveAmount = line.PeriodicDiscount,
                               SaleLineNumber = line.LineNumber,
                               Percentage = line.PeriodicPercentageDiscount,
                               DiscountLineTypeValue = Convert.ToInt32(DiscountLineType.PeriodicDiscount),
                               OfferId = line.Comment
                           };
                    line.DiscountLines.Add(discountLine);
                }
                if (line.TotalDiscount > 0)
                {
                    DiscountLine discountLine = new DiscountLine
                    {
                        EffectiveAmount = line.TotalDiscount,
                        SaleLineNumber = line.LineNumber,
                        ManualDiscountType = Microsoft.Dynamics.Commerce.Runtime.DataModel.ManualDiscountType.TotalDiscountAmount,
                        Amount = line.TotalDiscount,
                        DiscountLineTypeValue = Convert.ToInt32(DiscountLineType.ManualDiscount),
                    };
                    line.DiscountLines.Add(discountLine);
                }
                if (order.ShippingAddress != null)
                {
                    line.ShippingAddress = order.ShippingAddress;
                }

                line.InventoryLocationId = order.InventoryLocationId;
                //check if store pickup then set locationid or store
                if (pickUpStore.ErpValue == order.DeliveryMode)
                {

                    line.InventoryLocationId = order.StoreId;
                    line.Store = order.StoreId;
                }
            }

            order.PeriodicDiscountAmount = Enumerable.Sum<SalesLine>((IEnumerable<SalesLine>)order.ActiveSalesLines, (Func<SalesLine, Decimal>)(s => s.PeriodicDiscount));
            //order.TotalDiscount = order.TotalManualDiscountAmount = Enumerable.Sum<SalesLine>((IEnumerable<SalesLine>)order.ActiveSalesLines, (Func<SalesLine, Decimal>)(s => s.TotalDiscount));
            order.DiscountAmount = order.TotalDiscount + order.PeriodicDiscountAmount;

            if (order.TotalDiscount > 0)
            {
                ReasonCodeLine reasonCode = new ReasonCodeLine
                {
                    LineNumber = 1,
                    LineType = ReasonCodeLineType.Header,
                    ReasonCodeId = "DISCOUNT", // TODO:  to be changed to configurable object
                    //Field to give promotionCode
                    Information = order.DiscountCodes.Any() ? order.DiscountCodes.First() : string.Empty,
                    //below two fields are not saved by MPOS
                    Amount = order.TotalDiscount,
                    InformationAmount = order.TotalAmount
                };
                order.ReasonCodeLines = new Collection<ReasonCodeLine>();
                order.ReasonCodeLines.Add(reasonCode);
            }
        }
        private void SetupShippingMethod(SalesOrder order)
        {
            order.DeliveryMode = DeliveryModesDAL.GetErpDeliveryMode(order.DeliveryMode);
        }
        private void SetupPaymentMethod(TenderLine tenderLine)
        {
            tenderLine.Currency = ConfigurationHelper.DefaultAXTenderLineCurrencyCode;
            PaymentMethod paymentMethod = PaymentModesDAL.GetPaymentMothodByEcommerceKey(tenderLine.TenderTypeId);
            if (paymentMethod != null)
            {
                if (paymentMethod.HasSubMethod == true)
                {
                    PaymentMethod paymentSubMethod = PaymentModesDAL.GetPaymentMothodByEcommerceKey(tenderLine.CardTypeId);
                    if (paymentSubMethod != null)
                    {
                        tenderLine.TenderTypeId = paymentSubMethod.ErpValue;
                        tenderLine.CardTypeId = paymentSubMethod.ErpCode;
                    }
                }
                else
                {
                    tenderLine.TenderTypeId = paymentMethod.ErpValue;
                }
            }
            //if (tenderLine.TenderTypeId == "Paypal" || tenderLine.TenderTypeId == "Amazon")
            //{
            //    tenderLine.TenderTypeId = PaymentModesDAL.GetErpPaymentMode(tenderLine.TenderTypeId);
            //}
            //else
            //{
            //    tenderLine.TenderTypeId = PaymentModesDAL.GetErpPaymentMode(tenderLine.CardTypeId);
            //    if (tenderLine.CardTypeId == "American Express")
            //    {
            //        tenderLine.CardTypeId = "AMEX";
            //    }
            //}
            //if (tenderLine.TenderTypeId == Convert.ToInt32(PaymentMethods.CreditCard).ToString())
            //{
            //    tenderLine.CardTypeId = PaymentModesDAL.GetErpPaymentMode(tenderLine.CardTypeId);
            //}
        }

        private DateTimeOffset GetOrderDateTimeOffset(DateTimeOffset orderDate, DateTimeOffset orderTime)
        {
            DateTimeOffset trasnDate = new DateTimeOffset(orderDate.Year, orderDate.Month, orderDate.Day, orderTime.Hour, orderTime.Minute, orderTime.Second, TimeSpan.Zero);
            return trasnDate;
        }


    }
}