using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class SalesOrderHelper : BaseController
    {
        #region Public

        public SalesOrderHelper(string storeKey) : base(storeKey)
        {

        }
        public string InventoryLocation { get; set; }

        public ErpSalesOrder PopulateSalesOrder(ErpSalesOrder salesOrder, ErpSalesOrder salesOrderParam, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "salesOrder: " + JsonConvert.SerializeObject(salesOrder) + ", salesOrderParam: " + JsonConvert.SerializeObject(salesOrderParam));

            var order = new ErpSalesOrder();

            if (salesOrder == null) return order;
            //NS:
            order.CustomAttributes = salesOrderParam.CustomAttributes;

            //NS: TFS-1976
            order.SourceCode = salesOrderParam.SourceCode;
            //NS: TFS-1976

            // Get header attributes
            order.RecordId = salesOrder.RecordId;
            //NS:
            //TMV:FDD-007
            //order.RequestedDeliveryDate = DateTime.UtcNow;
            order.RequestedDeliveryDate = salesOrderParam.RequestedDeliveryDate;
            order.SalesId = "";
            order.StaffId = salesOrder.StaffId;
            order.BeginDateTime = salesOrderParam.OrderPlacedDate;
            order.TransactionType = ErpSalesTransactionType.PendingSalesOrder;
            order.DocumentStatus = ErpDocumentStatus.None;
            order.ChannelReferenceId = salesOrder.ChannelReferenceId;
            order.ChannelCurrencyExchangeRate = salesOrder.ChannelCurrencyExchangeRate;
            order.ChannelId = salesOrder.ChannelId;
            order.Name = salesOrder.Name;
            order.StoreId = salesOrder.StoreId;
            order.CustomerId = salesOrder.CustomerId;
            order.ReceiptEmail = salesOrder.ReceiptEmail;
            order.Id = salesOrder.Id;
            //NS
            //order.OrderPlacedDate = GetOrderDateTimeOffset(salesOrderParam.OrderPlacedDate, salesOrderParam.transTime); //salesOrder.OrderPlacedDate;
            order.CreatedDateTime = GetOrderDateTimeOffset(salesOrderParam.OrderPlacedDate, salesOrderParam.transTime); //salesOrder.OrderPlacedDate;
            order.TotalAmount = salesOrder.TotalAmount;
            order.NetAmountWithNoTax = salesOrder.NetAmountWithNoTax;
            // in magento (using 3rd party payment) sales order can only be processed once amount is fully paid, so sales diff will be 0.
            // Custom For MF payment can be in multiple types but total amount paid will be order total amount
            order.AmountPaid = salesOrder.TotalAmount;
            order.SalesPaymentDifference = order.TotalAmount - order.AmountPaid;
            order.TaxAmount = salesOrder.TaxAmount;
            order.TotalDiscount = salesOrder.TotalDiscount;
            order.NetAmountWithNoTax = salesOrder.NetAmountWithNoTax;
            //order.DiscountAmount = salesOrder.DiscountAmount * -1; //discounts are negative from magento
            order.ChargeAmount = salesOrder.ChargeAmount;
            //order.InventoryLocationId = salesOrder.InventoryLocationId;
            order.InventoryLocationId = InventoryLocation;
            order.StoreId = InventoryLocation;
            order.DeliveryMode = salesOrder.DeliveryMode;
            order.DeliveryModeChargeAmount = salesOrder.DeliveryModeChargeAmount;
            order.Status = salesOrder.Status;
            order.SalesLines = salesOrder.SalesLines;
            order.ShippingAddress = salesOrder.ShippingAddress;
            order.ShippingDiscounts = salesOrder.ShippingDiscounts;
            order.OrderDiscounts = salesOrder.OrderDiscounts;
            order.ContactInformationCollection = salesOrder.ContactInformationCollection;
            order.DiscountAmount = salesOrder.DiscountAmount;
            order.BillingAddress = salesOrder.BillingAddress;
            order.CurrencyCode = salesOrder.CurrencyCode;
            //order.DiscountCodes = salesOrder.DiscountCodes;
            if (salesOrderParam.DiscountCode != null)
            {
                if (order.DiscountCodes == null)
                {
                    order.DiscountCodes = new List<string>();
                }
                order.DiscountCodes.Add(salesOrderParam.DiscountCode);
            }
            DeliveryModesDAL deliveryModes = new DeliveryModesDAL(currentStore.StoreKey);

            var shopRunnerDlv = deliveryModes.GetDeliveryModeByKey(Convert.ToInt32(ErpDeliveryPreferenceType.DeliverItemsIndividually));
            if (shopRunnerDlv != null && shopRunnerDlv.ComValue == order.DeliveryMode)
            {
                salesOrderParam.ShopRunner = order.DeliveryMode;
            }
            else
            {
                salesOrderParam.ShopRunner = "";
            }

            PopulateSalesLineItemIds(order, salesOrderParam);

            if (configurationHelper.GetSetting(APPLICATION.ERP_AX_InferPeriodicDiscount).BoolValue())
            {
                GetSalesLinePriceInfoFromAX(order.SalesLines);
            }

            // Get lines
            PopulateSalesLine(order, salesOrderParam);

            // Managing header level discount
            ProcessOrderHeaderDiscount(order);

            // Discounts
            //Remove becuase not using CRT object now
            //NS: Comment Start
            //order.PeriodicDiscountAmount = Enumerable.Sum<ErpSalesLine>((IEnumerable<ErpSalesLine>)order.ActiveSalesLines, (Func<ErpSalesLine, Decimal>)(s => s.PeriodicDiscount));
            //order.TotalDiscount = order.TotalManualDiscountAmount = Enumerable.Sum<ErpSalesLine>((IEnumerable<ErpSalesLine>)order.ActiveSalesLines, (Func<ErpSalesLine, Decimal>)(s => s.TotalDiscount));
            //NS: Comment End
            order.PeriodicDiscountAmount = Enumerable.Sum<ErpSalesLine>((IEnumerable<ErpSalesLine>)order.SalesLines, (Func<ErpSalesLine, Decimal>)(s => s.PeriodicDiscount));
            order.TotalDiscount = order.TotalManualDiscountAmount = Enumerable.Sum<ErpSalesLine>((IEnumerable<ErpSalesLine>)order.SalesLines, (Func<ErpSalesLine, Decimal>)(s => s.TotalDiscount));
            order.DiscountAmount = order.TotalDiscount + order.PeriodicDiscountAmount;

            SetupShippingMethod(order);

            //Process sales order shipment if enabled
            if (!bool.Parse(configurationHelper.GetSetting(SALESORDER.Disable_Shippment_Process)))
            {
                this.ProcessSalesOrderShipment(order, salesOrderParam);
            }

            order.TenderLines = salesOrder.TenderLines;
            order.TenderLines.ToList().ForEach(tl => { this.SetupPaymentMethod(tl, order, requestId); });

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(order));
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return order;
        }

        #endregion

        #region Private

        private void ProcessSalesOrderShipment(ErpSalesOrder order, ErpSalesOrder salesOrderParam)
        {
            #region Shipment

            order.ChargeLines = new List<ErpChargeLine>();
            CodesDAL codes = new CodesDAL(currentStore.StoreKey);

            //adding shipping charge codes
            decimal orderDeliveryModeChargeAmount = order.DeliveryModeChargeAmount.Value;
            if (order.DeliveryModeChargeAmount != null)
            {
                //NS:TFS-1989
                if (order.ShippingDiscounts != null)
                {
                    decimal totalShippingDiscount = 0;
                    totalShippingDiscount = order.ShippingDiscounts.Sum(discount => Math.Abs(discount.Amount));
                    if (order.DeliveryModeChargeAmount.Value == totalShippingDiscount)
                    {
                        orderDeliveryModeChargeAmount = 0;
                    }
                }
                //NS:TFS-1989

                int customShippingChargeMethod = (int)ErpChargeMethod.Fixed;

                //Custom for VW
                if (configurationHelper.GetSetting(SALESORDER.AX_VW_Shipping_Custom_Category) != null &&
                    configurationHelper.GetSetting(SALESORDER.AX_VW_Shipping_Custom_Category).Contains(order.DeliveryMode))
                {
                    customShippingChargeMethod = configurationHelper.GetSetting(SALESORDER.AX_VW_VitaminWorldShipping_Category).IntValue();
                }

                order.ChargeLines.Add(new ErpChargeLine()
                {
                    //NS:TFS-1901
                    ChargeCode = codes.GetShippingChargeCode(),
                    //VW 
                    //VW has one to one relation with shipping method and 
                    //ChargeCode = order.DeliveryMode,
                    //VW 
                    Value = orderDeliveryModeChargeAmount,
                    CalculatedAmount = orderDeliveryModeChargeAmount,
                    //NS:TFS-1901
                    ChargeMethod = ErpChargeMethod.Fixed,
                    ChargeMethodValue = (int)ErpChargeMethod.Fixed,
                    //ChargeMethodValue = customShippingChargeMethod,
                    SaleLineNumber = 0,
                    Keep = 1,
                    ChargeType = ErpChargeType.AutoCharge
                });
            }

            #endregion

            #region Shipping Discount

            //NS:
            //Adding shipping discounts as negative charges
            if (order.ShippingDiscounts != null && orderDeliveryModeChargeAmount > 0)
            {
                int customShippingChargeMethod = (int)ErpChargeMethod.Fixed;

                if (configurationHelper.GetSetting(SALESORDER.AX_VW_Shipping_Custom_Category) != null &&
                    configurationHelper.GetSetting(SALESORDER.AX_VW_Shipping_Custom_Category).Contains(order.DeliveryMode))
                {
                    customShippingChargeMethod = configurationHelper.GetSetting(SALESORDER.AX_VW_VitaminWorldShipping_Category).IntValue();
                }

                foreach (ErpDiscountLine dLine in order.ShippingDiscounts)
                {
                    order.ChargeLines.Add(new ErpChargeLine()
                    {
                        //ChargeCode = CodesDAL.GetShippingDiscountChargeCode(),
                        //VW has one to one relation with shipping method and 
                        ChargeCode = order.DeliveryMode,
                        Value = dLine.Amount,
                        Description = dLine.DiscountCode + " : " + dLine.OfferId,
                        //NS:TFS-1991
                        //ChargeMethod = ErpChargeMethod.Fixed,
                        ChargeMethodValue = customShippingChargeMethod,
                        SaleLineNumber = 0,
                        Keep = 1,
                        ChargeType = ErpChargeType.AutoCharge
                    });
                }
            }

            #endregion

            #region Shipping Tax

            order.TaxLines = new Collection<ErpTaxLine>();

            //NS:
            //Custom for VW, Else part is standard part
            if (bool.Parse(configurationHelper.GetSetting(SALESORDER.Order_Tax_As_Charges)))
            {
                decimal totalShippingTaxDiscount = 0;
                if (order.ShippingDiscounts != null)
                {
                    totalShippingTaxDiscount = order.ShippingDiscounts.Sum(discount => Math.Abs(discount.Tax));
                }

                order.ChargeLines.Add(new ErpChargeLine()
                {
                    ChargeCode = configurationHelper.GetSetting(SALESORDER.Order_Shipping_Tax_As_Charges_Code),
                    Description = configurationHelper.GetSetting(SALESORDER.Order_Shipping_Tax_As_Charges_Description),
                    Value = salesOrderParam.Shipping_Tax - totalShippingTaxDiscount,
                    //NS:1901
                    //ChargeMethod = ErpChargeMethod.Fixed,
                    ChargeMethodValue = configurationHelper.GetSetting(SALESORDER.AX_VW_AvalaraShippingTax_Category).IntValue(),
                    SaleLineNumber = 0,
                    Keep = 1,
                    ChargeType = ErpChargeType.AutoCharge
                });
            }
            else
            {
                order.TaxLines.Add(new ErpTaxLine
                {
                    Amount = salesOrderParam.Shipping_Tax,
                    IsIncludedInPrice = false,
                    TaxGroup = codes.GetTaxCode(TaxGroups.SalesTaxGroup)
                });
            }

            #endregion
        }

        private List<ErpProductPrice> SalesLinesPriceInfo = new List<ErpProductPrice>();

        //NS: TFS-1806
        private void PopulateSalesLineItemIds(ErpSalesOrder order, ErpSalesOrder salesOrderParam)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "order: " + JsonConvert.SerializeObject(order) + ", salesOrderParam:" + JsonConvert.SerializeObject(salesOrderParam));

            int lineNumber = 1;
            foreach (var erpLineItem in salesOrderParam.SalesLines)
            {
                ErpSalesLine line = order.SalesLines.Where(l => l.ItemId == erpLineItem.ItemId && l.LineNumber == lineNumber).FirstOrDefault();

                //Get line itemId & productId from integration DB
                this.GetLineItemIdProductIdFromIntegrationDB(erpLineItem, line);
                //NS: to handle discounts if a sales order has same item multiples times.
                lineNumber++;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }

        private void PopulateSalesLine(ErpSalesOrder order, ErpSalesOrder salesOrderParam)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "order: " + JsonConvert.SerializeObject(order) + ", salesOrderParam: " + JsonConvert.SerializeObject(salesOrderParam));

            int lineNumber = 1;

            foreach (var erpLineItem in salesOrderParam.SalesLines)
            {
                ErpSalesLine line = order.SalesLines.Where(l => l.ProductId == erpLineItem.ProductId && l.LineNumber == lineNumber).FirstOrDefault();

                //NS: TFS-1806
                //Get line itemId & productId from integration DB
                //GetLineItemIdProductIdFromIntegrationDB(erpLineItem, line);

                // Custom For MF
                // Get quantity of option items from SalesOrderLinkedItemExtenstion
                // GetMFOptionItemQuantity(line);

                // Custom For MF
                //No Need
                //var giftCard = CodesDAL.GetGiftCardCode();  
                //if (giftCard != null)
                //{
                //    if (line.ItemId.ToLower().Equals(giftCard.ComValue.ToLower()))
                //    {
                //        line.IsGiftCardLine = true;
                //        //Assiging Electronic delivery mode to giftcard item
                //        //If from eCom we can order physical giftcard then we have to change this line
                //        line.DeliveryMode = DeliveryModesDAL.GetErpDeliveryMode(ConfigurationHelper.DefaultAXGiftCardDeliveryMode);
                //        line.GiftCardCurrencyCode = CommerceRuntimeHelper.ChannelConfiguration.Currency;
                //    }
                //}
                //else
                //{
                //    CustomLogger.LogTraceInfo("Gift card not setup in connector DB");
                //}

                CodesDAL codes = new CodesDAL(currentStore.StoreKey);
                line.ItemTaxGroupId = codes.GetTaxCode(TaxGroups.ItemTaxGroup);

                line.OriginalItemTaxGroupId = codes.GetTaxCode(TaxGroups.ItemTaxGroup);

                //NS: Changed for AX7
                string defualtSalesTaxGroupId = configurationHelper.GetSetting(APPLICATION.Defualt_SalesTaxGroupId_Constant);
                if (defualtSalesTaxGroupId == line.SalesTaxGroupId)
                {
                    line.SalesTaxGroupId = codes.GetTaxCode(TaxGroups.SalesTaxGroup);
                    line.OriginalSalesTaxGroupId = codes.GetTaxCode(TaxGroups.SalesTaxGroup);
                }
                else
                    line.OriginalSalesTaxGroupId = line.SalesTaxGroupId;

                //Noman:
                //line.SalesDate = order.OrderPlaceDate;
                line.SalesDate = order.CreatedDateTime;
                line.BeginDateTime = DateTime.UtcNow;

                //Awais: Copying NetAmount to NetAmountWithoutTax to fix Net Amount issue , which was not displaying in 
                // case of AX7 Implementation.
                line.NetAmountWithoutTax = erpLineItem.NetAmount;
                //NS: TMV FDD-007
                //TMV have some custom logic so we also need to set retailtransactionsalestrans.netAmountInclTax from CommerceLink
                line.NetAmountWithAllInclusiveTax = erpLineItem.TotalAmount;

                //Process sales line shipment
                this.ProcessSalesLineShipment(line, order, salesOrderParam, erpLineItem);

                DeliveryModesDAL deliveryModes = new DeliveryModesDAL(currentStore.StoreKey);

                var pickUpStore = deliveryModes.GetDeliveryModeByKey(Convert.ToInt32(ErpDeliveryPreferenceType.PickupFromStore));
                //check if store pickup then set locationid or store
                if (pickUpStore.ErpValue == line.DeliveryMode)
                {
                    line.InventoryLocationId = line.InventoryLocationId;
                    line.FulfillmentStoreId = line.InventoryLocationId;
                    line.Store = order.StoreId; //KAR
                }
                else
                {
                    //NS:
                    // Assigning from order header if sales order have custom attribute for this
                    //line.InventoryLocationId = line.InventoryLocationId;
                    line.InventoryLocationId = configurationHelper.GetSetting(INVENTORY.LocationId);
                }

                if (string.IsNullOrWhiteSpace(line.UnitOfMeasureSymbol))
                {
                    line.SalesOrderUnitOfMeasure = line.UnitOfMeasureSymbol = configurationHelper.GetSetting(SALESORDER.AX_Default_UnitofMeasure);
                }

                this.ProcessLineDiscount(line, order);

                line.ChargeLines = new Collection<ErpChargeLine>();

                if (bool.Parse(configurationHelper.GetSetting(SALESORDER.Order_Tax_As_Charges)))
                {
                    if (line.ChargeLines == null)
                    {
                        line.ChargeLines = new Collection<ErpChargeLine>();
                    }
                    line.ChargeLines.Add(new ErpChargeLine
                    {
                        ChargeCode = configurationHelper.GetSetting(SALESORDER.SalesLine_Tax_As_Charges_Code),
                        Description = configurationHelper.GetSetting(SALESORDER.SalesLine_Tax_As_Charges_Description),
                        //NS: 1901
                        //ChargeMethod = ErpChargeMethod.Fixed,
                        ChargeMethodValue = configurationHelper.GetSetting(SALESORDER.AX_VW_AvalaraProductTax_Category).IntValue(),
                        Keep = 1,
                        Value = line.TaxAmount,
                        SaleLineNumber = line.LineNumber,
                        ChargeType = ErpChargeType.AutoCharge
                    });
                }
                else
                {
                    line.TaxLines = new Collection<ErpTaxLine>() {new ErpTaxLine
                        {
                            Amount = line.TaxAmount,
                            IsIncludedInPrice = false,
                            //NS: Changed for AX7
                            //TaxCode = line.ItemTaxGroupId // have to confirm tax code is itemtaxgroup?           
                            TaxCode = line.OriginalItemTaxGroupId // have to confirm tax code is itemtaxgroup?           
                        }
                    };
                }

                //NS: to handle discounts if a sales order has same item multiples times.
                lineNumber++;
            }

            order.NumberOfItems = order.SalesLines.Sum(s => s.Quantity);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }

        private void ProcessSalesLineShipment(ErpSalesLine line, ErpSalesOrder order, ErpSalesOrder salesOrderParam, ErpSalesLine erpLineItem)
        {
            //NS: TMV FDD-007
            //line.RequestedDeliveryDate = DateTime.UtcNow;
            line.RequestedDeliveryDate = order.RequestedDeliveryDate;

            if (bool.Parse(configurationHelper.GetSetting(SALESORDER.Disable_Shippment_Process)))
            {
                line.DeliveryMode = configurationHelper.GetSetting(SALESORDER.AX_Default_Delivery_Mode);
                line.ShippingAddress = new ErpAddress();
            }
            else
            {
                //Setting orderDeliveryMode to line level from shipment
                line.DeliveryMode = order.DeliveryMode;

                // Custom For MF 
                // Assigning shipment details                
                var shipment = salesOrderParam.Shipments.Where(s => s.ShipmentId == erpLineItem.ShipmentId).FirstOrDefault();
                if (shipment != null)
                {

                    DeliveryModesDAL deliveryModes = new DeliveryModesDAL(currentStore.StoreKey);
                    line.DeliveryMode = deliveryModes.GetErpDeliveryMode(shipment.DeliveryMode);
                    SaleOrderController sCon = new SaleOrderController(currentStore.StoreKey);
                    line.ShippingAddress = sCon.CreateAddressFromErpAddress(shipment.DeliveryAddress);
                }
            }
        }

        private void ProcessLineDiscount(ErpSalesLine line, ErpSalesOrder order)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "line: " + JsonConvert.SerializeObject(line) + ", order: " + JsonConvert.SerializeObject(order));

            if (line.DiscountLines == null)
            {
                line.DiscountLines = new Collection<ErpDiscountLine>(); // need to initialize is as CRT gives error
            }

            //Handing Promotional discounts
            foreach (ErpDiscountLine dLine in line.DiscountLines)
            {
                //DW provides negative values so taking absolute 
                dLine.Amount = Math.Abs(dLine.Amount);
                dLine.EffectiveAmount = dLine.Amount;
                dLine.SaleLineNumber = line.LineNumber;
                //offerId is already filled

                //NS: no need to these 2 line becuase getting all types info from cart and mapped in sales order mapping
                //dLine.ManualDiscountType = ErpManualDiscountType.TotalDiscountAmount;
                //dLine.DiscountLineTypeValue = Convert.ToInt32(ErpDiscountLineType.ManualDiscount);

                //NS: TFS-1990
                line.TaxAmount -= Math.Abs(dLine.Tax);
                //NS: TFS-1990
            }
            line.TotalDiscount = Enumerable.Sum<ErpDiscountLine>((IEnumerable<ErpDiscountLine>)line.DiscountLines, (Func<ErpDiscountLine, Decimal>)(s => s.Amount));

            //Handing special price discounts
            //NS:KAR: We will get the discount amount from AX and will populate PeriodicDiscount property to function properly.
            //Previously it is being done in EcomAdpator.
            //Making it configureable as well
            if (configurationHelper.GetSetting(APPLICATION.ERP_AX_InferPeriodicDiscount).BoolValue())
            {
                //NS: TFS-1806
                //var price = GetProductPriceFromAX(line.ProductId, string.Empty);
                var price = SalesLinesPriceInfo.Where(item => item.ProductId == line.ProductId).FirstOrDefault();

                //KAR says
                //line.PeriodicDiscount = price.DiscountAmount; // We are not using this as there can be a difference in this amount if discounts are applied in AX but not synched into Ecomeerce.
                // THis is generic scenario and need to be discussed thoroughly.
                // SCENARIO: If we chaned List Price (Adjusted Price) OR some discounts are applied which caused variance in sales price but EdgeAX CommerceLink Pricing / Discount jobs has not been executed and ecommerce has old discounts/prices setup.
                // What will happen or How we will deal if order comes with old values but prices are changed in AX ?? 
                if (price != null)
                {
                    //Contoso
                    //line.OriginalPrice = price.BasePrice; // This is List price for the product. Product Sales Price should be  (price.AdjustedPrice - price.DiscountAmount)

                    //NS: VW, If trade agreement price has been expired then get value of base price
                    line.OriginalPrice = price.TradeAgreementPrice > 0 ? price.TradeAgreementPrice : price.BasePrice;
                    if (line.OriginalPrice.Value > 0)
                    {
                        line.Price = line.OriginalPrice.Value;
                        line.PeriodicDiscount = (line.OriginalPrice.Value - line.BasePrice) * line.Quantity; // Base Price is Sales Price from Ecommerce
                    }
                }
            }

            try
            {
                //NS: Custom For MF
                //Delivery Items may have price = 0 so need to add check for them
                //if (line.Comment != null && !line.Comment.Equals(ConfigurationHelper.GetSetting(SALESORDER.DeliveryItem_Constant)))
                //{
                //    //line.PeriodicPercentageDiscount = (line.PeriodicDiscount / (line.Price * line.Quantity)) * 100;
                //    //line.TotalPercentageDiscount = (line.TotalDiscount / ((line.Price - line.PeriodicDiscount) * line.Quantity)) * 100;
                //    line.PeriodicPercentageDiscount = Convert.ToDecimal((line.PeriodicDiscount / (line.OriginalPrice * line.Quantity)) * 100);
                //}
                if (line.Price > 0)
                {
                    line.PeriodicPercentageDiscount = Convert.ToDecimal((line.PeriodicDiscount / (line.Price * line.Quantity)) * 100);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogTraceInfo(ex.Message, currentStore.StoreId, currentStore.CreatedBy);
                throw ex;
            }

            if (line.PeriodicDiscount > 0)
            {
                ErpDiscountLine discountLine = new ErpDiscountLine
                {
                    EffectiveAmount = line.PeriodicDiscount,
                    SaleLineNumber = line.LineNumber,
                    Percentage = line.PeriodicPercentageDiscount,
                    DiscountLineTypeValue = Convert.ToInt32(ErpDiscountLineType.PeriodicDiscount),
                    //Not a offer it is specail price discount
                    //OfferId = line.Comment
                };
                line.DiscountLines.Add(discountLine);
            }

            //Calculating DiscountAmount
            line.DiscountAmount = line.PeriodicDiscount + line.TotalDiscount;

            //Manage SalesLine NetAmount after subtract discount
            if (configurationHelper.GetSetting(SALESORDER.Subtract_Discount_NetAmount_Enable).BoolValue())
            {
                //Subtracting only TotalDiscount becuase DiscountAmount = TotalDiscount + PeriodicDiscount 
                //We have no need to subtract PeriodicDiscount amount from NetAmount
                line.NetAmount -= line.TotalDiscount;
                //NS: AX7
                line.NetAmountWithoutTax -= line.TotalDiscount;
            }
        }

        private void GetMFOptionItemQuantity(ErpSalesLine line)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(line));

            if (line.Comment.Contains(configurationHelper.GetSetting(SALESORDER.OptionItem_Constant)))
            {
                string[] strComment = line.Comment.Split(':');
                if (strComment.Count() > 1)
                {
                    string parentVariantId = strComment[1];
                    string optionVariantId = line.Variant.VariantId;
                    //getting from product extension method
                    ProductController erpProductController = new ProductController(currentStore.StoreKey);
                    ErpLinkedProduct linkedProduct = erpProductController.GetProductLinkedItemExtension(parentVariantId, optionVariantId);
                    if (linkedProduct != null)
                    {
                        line.Quantity = linkedProduct.Quantity;
                    }
                    else
                    {
                        line.Quantity = 1;
                    }
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

        }

        private void GetLineItemIdProductIdFromIntegrationDB(ErpSalesLine erpLineItem, ErpSalesLine line)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name,
                "erpLineLtem: " + JsonConvert.SerializeObject(erpLineItem) + ", line: " + JsonConvert.SerializeObject(line));

            bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();


            if (line.ItemId != null && (!string.IsNullOrWhiteSpace(line.ItemId)))
            {

                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                var key = integrationManager.GetErpKey(Entities.Product, line.ItemId);

                if (key == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40503, currentStore, line.ItemId);
                    throw new CommerceLinkError(message);
                }

                string itemId = string.Empty;
                string variantId = string.Empty;

                if (!isFlatProductHierarchy) //Standard Product Hierarchy
                {
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
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40504, currentStore);
                        throw new CommerceLinkError(message);
                    }

                    //var variant = GetProductFromSku(key.Description);
                    line.Variant = string.IsNullOrEmpty(variantId) ? new ErpProductVariant { VariantId = "" } : new ErpProductVariant { VariantId = variantId };
                    line.ProductId = Convert.ToInt64(key.ErpKey); //variant != null ? variant.DistinctProductVariantId : Convert.ToInt64(line.ItemId); //TODO: Once Products are finalized. Get RecordID
                    line.ItemId = itemId;// variant != null ? variant.ItemId : line.ItemId;
                                         //Maintaining to get pricing details
                    erpLineItem.ProductId = Convert.ToInt64(key.ErpKey);
                }

                else
                {
                    line.Variant = string.IsNullOrEmpty(variantId) ? new ErpProductVariant { VariantId = "" } : new ErpProductVariant { VariantId = variantId };
                    line.ProductId = Convert.ToInt64(key.ErpKey);
                    line.ItemId = key.Description;
                    erpLineItem.ProductId = Convert.ToInt64(key.ErpKey);
                }

                //}

            }
            else
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40523, currentStore, line.ItemId);
                throw new CommerceLinkError(message);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }

        //NS: Old Method
        /*
        private void ProcessOrderHeaderDiscount(ErpSalesOrder order)
        {
            //DW provides negative values so taking absolute
            order.DiscountAmount = Math.Abs(order.DiscountAmount);

            if (order.DiscountAmount > 0)
            {
                ErpReasonCodeLine reasonCode = new ErpReasonCodeLine
                {
                    LineNumber = 1,
                    LineType = ErpReasonCodeLineType.Header,
                    //ReasonCodeId = "DISCOUNT", 
                    ReasonCodeId = ConfigurationHelper.OrderHeaderDiscountReasonCodeId,
                    //Field to give promotionCode
                    Information = order.DiscountCodes.Any() ? order.DiscountCodes.First() : string.Empty,
                    //below two fields are not saved by MPOS
                    Amount = order.DiscountAmount,
                    InformationAmount = order.TotalAmount
                };
                order.ReasonCodeLines = new Collection<ErpReasonCodeLine>();
                order.ReasonCodeLines.Add(reasonCode);

                //Breaking header level discount into line level
                decimal orderTotalWithoutCharges = order.TotalAmount - order.TaxAmount - order.DeliveryModeChargeAmount ?? Convert.ToDecimal(order.DeliveryModeChargeAmount);

                decimal headerDicountInPercentage = (order.DiscountAmount * 100) / (order.DiscountAmount + orderTotalWithoutCharges);

                foreach(ErpSalesLine line in order.SalesLines)
                {
                    decimal lineDiscountAmountSharesFromHeaderDiscount = ((line.TotalAmount - line.TotalDiscount) * headerDicountInPercentage) / 100;
                    if (lineDiscountAmountSharesFromHeaderDiscount > 0)
                    {
                        line.DiscountLines.Add(new ErpDiscountLine
                        {
                            Amount = lineDiscountAmountSharesFromHeaderDiscount,
                            EffectiveAmount = lineDiscountAmountSharesFromHeaderDiscount,
                            SaleLineNumber = line.LineNumber,
                            DiscountCode = order.DiscountCodes.Any() ? order.DiscountCodes.First() : string.Empty,
                            ManualDiscountType = ErpManualDiscountType.TotalDiscountAmount,
                            DiscountLineTypeValue = Convert.ToInt32(ErpDiscountLineType.ManualDiscount)
                        });
                        //Re-calculating line total discount
                        line.TotalDiscount = Enumerable.Sum<ErpDiscountLine>((IEnumerable<ErpDiscountLine>)line.DiscountLines, (Func<ErpDiscountLine, Decimal>)(s => s.Amount));
                        line.DiscountAmount = line.PeriodicDiscount + line.TotalDiscount;
                    }
                }
            }            
        }
        */

        #region Process Discounts

        private void GetSalesLinePriceInfoFromAX(IList<ErpSalesLine> products)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(products));

            List<ErpProductPrice> productPriceList = new List<ErpProductPrice>();
            try
            {
                List<ErpProduct> erpProducts = new List<ErpProduct>();
                foreach (var line in products)
                {
                    erpProducts.Add(new ErpProduct { RecordId = line.ProductId });
                }

                List<long> productIds = new List<long>();
                if (erpProducts != null && erpProducts.Count > 0)
                    productIds = erpProducts.Select(prod => prod.RecordId).Distinct().ToList();

                var crtDiscountManager = new DiscountCRTManager();
                productPriceList = crtDiscountManager.GetIndependentProductPriceDiscount(erpProducts, configurationHelper.GetSetting(APPLICATION.Channel_Id).LongValue(), productIds, string.Empty,currentStore.StoreKey);

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40505, currentStore);
                throw new CommerceLinkError(message);
            }
            SalesLinesPriceInfo.Clear();
            if (productPriceList.Count > 0)
            {
                SalesLinesPriceInfo = productPriceList;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }
        //NS: TFS-1806
        /*
        private ErpProductPrice GetProductPriceFromAX(long productId, string customerAC)
        {
            List<ErpProductPrice> productPriceList = new List<ErpProductPrice>();
            try
            {
                List<ErpProduct> erpProducts = new List<ErpProduct>();
                erpProducts.Add(new ErpProduct { RecordId = productId });

                List<long> productIds = new List<long>();
                if (erpProducts != null && erpProducts.Count > 0)
                    productIds = erpProducts.Select(prod => prod.RecordId).Distinct().ToList();

                var crtDiscountManager = new DiscountCRTManager();
                productPriceList = crtDiscountManager.GetIndependentProductPriceDiscount(erpProducts, ConfigurationHelper.GetSetting(APPLICATION.Channel_Id).LongValue(), productIds, string.Empty);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (productPriceList.Count > 0)
            {
                return productPriceList.Where(pd => pd.ProductId == productId).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
        */
        private void ProcessOrderHeaderDiscount(ErpSalesOrder order)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(order));

            if (order.OrderDiscounts != null)
            {
                foreach (ErpDiscountLine discount in order.OrderDiscounts)
                {
                    //NS: for rebate & OOB-coupon, no discount division on lines
                    if (discount.OfferName != null && discount.OfferName.ToLower() == configurationHelper.GetSetting(SALESORDER.Rebate_Reason_Code).ToString().ToLower())
                    {
                        ProcessBonusRebate(discount, order);
                    }
                    else
                    {
                        ProcessOrderHeaderDiscountDivisionOnLines(discount, order);
                    }

                    ProcessOrderHeaderDiscountTaxDivisionOnLines(discount, order);
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }
        private void ProcessOrderHeaderDiscountDivisionOnLines(ErpDiscountLine discount, ErpSalesOrder order)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "discount: " + JsonConvert.SerializeObject(discount) + ", order:" + JsonConvert.SerializeObject(order));

            decimal discountAmount = Math.Abs(discount.EffectiveAmount);
            order.DiscountAmount += discountAmount;

            //Adding InfoCodes start
            ErpReasonCodeLine reasonCode = new ErpReasonCodeLine
            {
                LineNumber = 0,
                LineType = ErpReasonCodeLineType.Header,
                ReasonCodeId = configurationHelper.GetSetting(SALESORDER.Header_Discount_Reason_Code) + " : " + discount.DiscountCode + " : " + discount.OfferId,
                Information = discount.DiscountCode,
                Amount = discountAmount,
                InformationAmount = discount.Amount
            };
            if (order.ReasonCodeLines == null)
            {
                order.ReasonCodeLines = new Collection<ErpReasonCodeLine>();
            }
            order.ReasonCodeLines.Add(reasonCode);
            //Adding InfoCodes end


            //Breaking header level discount into line level
            decimal orderTotalWithoutCharges = 0;
            orderTotalWithoutCharges = order.SalesLines.Where(line => line.NetAmount > 0).ToList().Sum(lineItem => lineItem.NetAmount);

            // Get the Discount Percentage
            decimal headerDicountInPercentage = 0;
            if (orderTotalWithoutCharges > 0)
            {
                headerDicountInPercentage = (discountAmount * 100) / (orderTotalWithoutCharges);
            }

            foreach (ErpSalesLine line in order.SalesLines.Where(line => line.NetAmount > 0))
            {
                // Calculate the Discount Share for current line
                decimal lineDiscountAmountSharesFromHeaderDiscount = 0;
                lineDiscountAmountSharesFromHeaderDiscount = (line.NetAmount * headerDicountInPercentage) / 100;

                if (lineDiscountAmountSharesFromHeaderDiscount > 0)
                {
                    line.DiscountLines.Add(new ErpDiscountLine
                    {
                        Amount = lineDiscountAmountSharesFromHeaderDiscount,
                        EffectiveAmount = lineDiscountAmountSharesFromHeaderDiscount,
                        SaleLineNumber = line.LineNumber,
                        DiscountCode = discount.DiscountCode,
                        ManualDiscountType = ErpManualDiscountType.TotalDiscountAmount,
                        DiscountLineTypeValue = Convert.ToInt32(ErpDiscountLineType.ManualDiscount)
                    });
                    //Re-calculating line total discount
                    line.TotalDiscount = Enumerable.Sum<ErpDiscountLine>((IEnumerable<ErpDiscountLine>)line.DiscountLines, (Func<ErpDiscountLine, Decimal>)(s => s.Amount));
                    line.DiscountAmount = line.PeriodicDiscount + line.TotalDiscount;

                    //Manage SalesLine NetAmount after subtract discount
                    if (configurationHelper.GetSetting(SALESORDER.Subtract_Discount_NetAmount_Enable).BoolValue())
                    {
                        //Subtracting only TotalDiscount becuase DiscountAmount = TotalDiscount + PeriodicDiscount 
                        //We have no need to subtract PeriodicDiscount amount from NetAmount
                        line.NetAmount -= lineDiscountAmountSharesFromHeaderDiscount;
                        //NS: AX7
                        line.NetAmountWithoutTax -= lineDiscountAmountSharesFromHeaderDiscount;
                    }
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }
        private void ProcessOrderHeaderDiscountTaxDivisionOnLines(ErpDiscountLine discount, ErpSalesOrder order)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "discount: " + JsonConvert.SerializeObject(discount) + ", order: " + JsonConvert.SerializeObject(order));

            decimal discountTaxAmount = Math.Abs(discount.Tax);

            //Breaking header level discount tax into line level
            decimal orderTotalTax = 0;
            orderTotalTax = order.SalesLines.Sum(lineItem => lineItem.TaxAmount);

            // Get the Discount tax Percentage
            decimal headerDicountTaxInPercentage = 0;
            if (orderTotalTax > 0)
            {
                headerDicountTaxInPercentage = (discountTaxAmount * 100) / (orderTotalTax);
            }

            foreach (ErpSalesLine line in order.SalesLines)
            {
                // Calculate the Discount tax Share for current line
                decimal lineDiscountTaxAmountSharesFromHeaderDiscount = 0;
                lineDiscountTaxAmountSharesFromHeaderDiscount = (line.TaxAmount * headerDicountTaxInPercentage) / 100;

                if (lineDiscountTaxAmountSharesFromHeaderDiscount > 0)
                {
                    var lineTaxCharges = line.ChargeLines.Where(charge => charge.ChargeCode == configurationHelper.GetSetting(SALESORDER.SalesLine_Tax_As_Charges_Code) && charge.Value > 0).FirstOrDefault();
                    if (lineTaxCharges != null)
                    {
                        lineTaxCharges.Value -= lineDiscountTaxAmountSharesFromHeaderDiscount;
                    }
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }
        private void ProcessBonusRebate(ErpDiscountLine rebateDiscount, ErpSalesOrder order)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "rebateDiscount: " + JsonConvert.SerializeObject(rebateDiscount) + " order: " + JsonConvert.SerializeObject(order));

            decimal rebateAmount = Math.Abs(rebateDiscount.EffectiveAmount);

            ErpReasonCodeLine reasonCode = new ErpReasonCodeLine
            {
                LineNumber = 0,
                LineType = ErpReasonCodeLineType.Header,
                ReasonCodeId = rebateDiscount.OfferName,
                Information = rebateDiscount.RebateCode,
                Amount = rebateAmount,
                InformationAmount = rebateDiscount.Amount,
                //NS: 
                //TODO: using it for SPP-No, can be change in future
                StatementCode = rebateDiscount.SppNumber
            };
            if (order.ReasonCodeLines == null)
            {
                order.ReasonCodeLines = new Collection<ErpReasonCodeLine>();
            }
            order.ReasonCodeLines.Add(reasonCode);

            //update order total amount to actuall totals
            order.NetAmountWithNoTax += rebateAmount;
            order.TotalAmount += rebateAmount;

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }
        #endregion
        private void SetupShippingMethod(ErpSalesOrder order)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(order));

            if (bool.Parse(configurationHelper.GetSetting(SALESORDER.Disable_Shippment_Process)))
            {
                order.DeliveryMode = configurationHelper.GetSetting(SALESORDER.AX_Default_Delivery_Mode);
                order.ShippingAddress = new ErpAddress();
            }
            else
            {
                //NS: If delivey mode is at order header level then use this line of code
                //order.DeliveryMode = DeliveryModesDAL.GetErpDeliveryMode(order.DeliveryMode);

                // Set order header delivery mode from first line item
                if (order.SalesLines != null)
                {
                    order.DeliveryMode = order.SalesLines[0].DeliveryMode;
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }

        public void SetupPaymentMethod(ErpTenderLine tenderLine, ErpSalesOrder salesOrder, string requestId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                JsonConvert.SerializeObject(tenderLine));

            if (string.IsNullOrWhiteSpace(salesOrder.CurrencyCode))
            {
                tenderLine.Currency = configurationHelper.GetSetting(APPLICATION.Default_Currency_Code);
            }
            else
            {
                tenderLine.Currency = salesOrder.CurrencyCode;
            }

            PaymentMethodDAL paymentMethodDAL = new PaymentMethodDAL(currentStore.StoreKey);

            PaymentMethod paymentMethod = paymentMethodDAL.GetPaymentMothodByEcommerceKey(tenderLine.TenderTypeId);
            if (paymentMethod != null)
            {
                bool isPayPal = false;

                if (paymentMethod.ECommerceValue.Equals(configurationHelper.GetSetting(PAYMENTCONNECTOR.PayPal_Ecom_Value)))
                {
                    isPayPal = true;
                    //NS: RetailTransactionPaymentTrans field length is 30 characters and CreditCard table field length is 40.
                    //After getting go-head from business truncating email greater then 30 characters
                    if (tenderLine.MaskedCardNumber.Length > 30)
                    {
                        tenderLine.MaskedCardNumber = tenderLine.MaskedCardNumber.Substring(0, 30);
                    }
                }

                if (paymentMethod.HasSubMethod == true)
                {
                    PaymentMethod paymentSubMethod = paymentMethodDAL.GetPaymentMothodByEcommerceKey(tenderLine.CardTypeId, paymentMethod.PaymentMethodId);
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
                //ZS code to create card token
                if (paymentMethod.UsePaymentConnector == true)
                {
                    PaymentController paymentController = new PaymentController(currentStore.StoreKey);
                    var paymentManager = new PaymentCRTManager();
                    var paymentConnectorData = new PaymentConnectorData();

                    string uniqueCardIdFromJSON = string.IsNullOrEmpty(tenderLine.UniqueCardId) ? tenderLine.Authorization : tenderLine.UniqueCardId;

                    ErpPaymentCard cardProp = new ErpPaymentCard
                    {
                        IsPayPal = isPayPal,
                        CardNumber = tenderLine.MaskedCardNumber,
                        NameOnCard = tenderLine.CardOrAccount,
                        ExpirationMonth = tenderLine.ExpMonth,
                        ExpirationYear = tenderLine.ExpYear,
                        CardToken = tenderLine.CardToken,
                        CardTypes = tenderLine.CardTypeId,
                        ECommerceValue = paymentMethod.ECommerceValue,
                        //UniqueCardId = tenderLine.Authorization, //Setting this as unique card id should transactionid  //  tenderLine.UniqueCardId,
                        UniqueCardId = uniqueCardIdFromJSON,
                        Email = tenderLine.Email,
                        Note = tenderLine.Note,
                        PayerId = tenderLine.PayerId,
                        ParentTransactionId = tenderLine.ParentTransactionId,
                        NumberOfInstallments = tenderLine.NumberOfInstallments,
                        BankIdentificationNumberStart = tenderLine.BankIdentificationNumberStart,
                        ApprovalCode = tenderLine.ApprovalCode,
                        shopperReference = tenderLine.shopperReference,
                        TransactionId = tenderLine.CustomAttributes.FirstOrDefault(x => x.Key == "transaction-id").Value,
                        ThreeDSecure = tenderLine.ThreeDSecure,
                        IssuerCountry = tenderLine.IssuerCountry
                    };

                    // Allpago Change VSTS # 29903
                    if (paymentMethod.ECommerceValue == PaymentCon.ALLPAGO_CC.ToString())
                    {
                        cardProp.IP = tenderLine.IP;
                        cardProp.TransactionId = tenderLine.TransactionId;
                        cardProp.LocalTaxId = tenderLine.LocalTaxId;
                    }

                    else if (paymentMethod.ECommerceValue == PaymentCon.ADYEN_HPP.ToString() )
                    {
                        // Its Alipay Card Type
                        if (tenderLine.Alipay != null && !string.IsNullOrEmpty(tenderLine.Alipay.OutTradeNo) && !string.IsNullOrEmpty(tenderLine.Alipay.BuyerId))
                        {
                            tenderLine.Authorization = tenderLine.PspReference;
                            cardProp.UniqueCardId = tenderLine.Alipay.OutTradeNo;
                            cardProp.CardToken = tenderLine.Alipay.BuyerId;
                            cardProp.CardTypes = tenderLine.CardTypeId;
                            cardProp.NameOnCard = tenderLine.Alipay.BuyerEmail;
                            cardProp.CardNumber = cardProp.NameOnCard;
                            cardProp.shopperReference = tenderLine.PspReference;
                            cardProp.ExpirationMonth = DateTime.UtcNow.Month;
                            cardProp.ExpirationYear = DateTime.UtcNow.Year; 
                        }
                        else
                        {
                            tenderLine.Authorization = tenderLine.PspReference;
                            cardProp.UniqueCardId = tenderLine.PspReference;
                            cardProp.CardToken = tenderLine.PspReference;
                            cardProp.CardTypes = tenderLine.CardTypeId;
                            cardProp.NameOnCard = salesOrder.ReceiptEmail;
                            cardProp.CardNumber = cardProp.NameOnCard;
                            cardProp.shopperReference = tenderLine.PspReference;
                            cardProp.ExpirationMonth = DateTime.UtcNow.Month;
                            cardProp.ExpirationYear = DateTime.UtcNow.Year;
                        }
                    }
                    //ZS if we recevie only card token, code will check if it already exists.Otherwise create new
                    if (string.IsNullOrEmpty(cardProp.UniqueCardId))
                    {
                        try
                        {
                            string uniqueCardId;
                            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

                            if (integrationManager.GetErpKey(Entities.CreditCard, tenderLine.CardToken) != null)
                                uniqueCardId = integrationManager.GetErpKey(Entities.CreditCard, tenderLine.CardToken).ErpKey;
                            else
                            {
                                uniqueCardId = Guid.NewGuid().ToString();
                                integrationManager.CreateIntegrationKey(Entities.CreditCard, uniqueCardId, tenderLine.CardToken, salesOrder.CustomerId);
                            }
                            cardProp.UniqueCardId = uniqueCardId;
                        }
                        catch(Exception ex)
                        {
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40514, currentStore);
                            throw new CommerceLinkError(message);
                        }
                    }
                    if (salesOrder.BillingAddress != null)
                    {
                        cardProp.BillingName = salesOrder.BillingAddress.Name;
                        cardProp.Address1 = salesOrder.BillingAddress.Street;
                        cardProp.City = salesOrder.BillingAddress.City;
                        cardProp.State = salesOrder.BillingAddress.State;
                        cardProp.Zip = salesOrder.BillingAddress.ZipCode;
                        cardProp.Country = salesOrder.BillingAddress.ThreeLetterISORegionName;
                    }

                    try
                    {
                        tenderLine.CardToken = paymentManager.GenerateCardBlob(cardProp, tenderLine.Authorization, paymentMethod, currentStore.StoreKey, requestId, paymentConnectorData);
                    }
                    catch (Exception ex)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40515, currentStore);
                        throw new CommerceLinkError(message + " - " + ex.Message);
                    }

                    try
                    { 
                        tenderLine.Authorization = paymentManager.GenerateAuthBlob(cardProp, tenderLine.Authorization, tenderLine.ApprovalCode, tenderLine.Amount.Value, DateTime.UtcNow, salesOrder.CurrencyCode, currentStore.StoreKey, paymentConnectorData);
                    }
                    catch (Exception ex)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40516, currentStore);
                        throw new CommerceLinkError(message);
                    }

                    //++ if cardToken is empty we will not send CardBlob 
                    if (string.IsNullOrEmpty(cardProp.CardToken))
                        tenderLine.CardToken = "";
                }

                //NS: AX7
                tenderLine.AmountInTenderedCurrency = tenderLine.Amount;
                tenderLine.AmountInCompanyCurrency = tenderLine.Amount;
                tenderLine.ExchangeRate = 1;
                tenderLine.CompanyCurrencyExchangeRate = 1;

                //NS: VW_Payment 
                if (paymentMethod.IsPrepayment)
                {
                    tenderLine.IsDeposit = true;
                    //HK: D365 Update 10.0 Application change start
                    tenderLine.VoidStatusValue = (int)ErpTenderLineStatus.Committed;
                    //HK: D365 Update 10.0 Application change end
                }
                else
                {
                    tenderLine.IsDeposit = false;
                    //HK: D365 Update 10.0 Application change start
                    tenderLine.VoidStatusValue = (int)ErpTenderLineStatus.PendingCommit;
                    //HK: D365 Update 10.0 Application change end
                }
            }
            else
            {
                throw new CommerceLinkError(String.Format(
                    CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40534), tenderLine.TenderTypeId));
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }

        private DateTimeOffset GetOrderDateTimeOffset(DateTimeOffset orderDate, DateTimeOffset orderTime)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "orderDate: " + orderDate.ToString() + ", orderTime: " + orderTime.ToString());

            DateTimeOffset trasnDate = new DateTimeOffset(orderDate.Year, orderDate.Month, orderDate.Day, orderTime.Hour, orderTime.Minute, orderTime.Second, TimeSpan.Zero);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, trasnDate.ToString());
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return trasnDate;
        }

        #endregion
    }
}