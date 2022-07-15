using Newtonsoft.Json;
using System;
using System.Linq;
using System.Xml.Linq;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
{

    /// <summary>
    /// SaleOrderController class performs SalesOrder related activities.
    /// </summary>
    public class SalesOrderController_Dutch : BaseController, ISaleOrderController
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SalesOrderController_Dutch()
            : base(false)
        {
            //var list = this.Service.salesOrderList(SessionId,null);
            //var salesInfo = this.Service.salesOrderInfo(SessionId, "200000025");
        }

        #endregion

        #region Public Methods

        public EcomsalesOrderEntity GetSalesOrders(string localFile)
        {
            try
            {
                var doc = XElement.Load(localFile);
                var salesOrder = new EcomsalesOrderEntity();

                salesOrder.customer_id = doc.Element("CUSTOMER_CODE").Value;
                salesOrder.customer_firstname = doc.Element("CUSTOMER_NAME").Value;
                salesOrder.billing_address = new EcomsalesOrderAddressEntity();
                salesOrder.billing_address.address_id = doc.Element("BILLING_ADDRESSID").Value;
                salesOrder.billing_address.street = doc.Element("CUSTOMER_ADDRESS_1").Value;
                salesOrder.billing_address.city = doc.Element("CUSTOMER_ADDRESS_CITY").Value;
                salesOrder.billing_address.country_id = doc.Element("CUSTOMER_ADDRESS_COUNTRY").Value;
                salesOrder.billing_address.region = doc.Element("CUSTOMER_ADDRESS_STATE").Value;
                salesOrder.billing_address.postcode = doc.Element("CUSTOMER_ADDRESS_ZIP").Value;
                salesOrder.billing_address.company = doc.Element("CUSTOMER_ADDRESS_COMPANY").Value;
                salesOrder.customer_email = doc.Element("EMAIL").Value;
                salesOrder.order_id = doc.Element("ORDER_ID").Value;
                salesOrder.created_at = doc.Element("ORDER_DATE").Value;
                salesOrder.shipping_method = doc.Element("SERVICE").Value;
                salesOrder.shipping_description = doc.Element("SHIPTO_NAME").Value;
                salesOrder.store_id = doc.Element("STORE_ID").Value;

                if (salesOrder.shipping_method.Equals("Store Pickup"))
                {
                    salesOrder.est_shipping_date = doc.Element("STORE_PICKUP_DATE").Value;
                }

                salesOrder.website_from = doc.Element("ORDER_TYPE").Value;
                salesOrder.shipping_amount = doc.Element("SHIPPING_CHARGE").Value;
                salesOrder.tax_amount = doc.Element("TAX_CHARGE").Value;
                salesOrder.shipping_tax = doc.Element("SHIPPING_TAX_AMOUNT").Value;
                salesOrder.grand_total = doc.Element("TOTAL").Value;
                salesOrder.subtotal = doc.Element("NET_AMOUNT").Value;
                salesOrder.discount_amount = doc.Element("DISCOUNT_AMOUNT").Value;

                salesOrder.discount_codes = doc.Element("DISCOUNT_CODE").Value;
                salesOrder.est_shipping_date = doc.Element("EST_SHIPPING_DATE") != null ? doc.Element("EST_SHIPPING_DATE").Value : "";
                salesOrder.shipping_address = new EcomsalesOrderAddressEntity();
                salesOrder.shipping_name = doc.Element("SHIPTO_NAME").Value;
                salesOrder.shipping_address.firstname = doc.Element("DELIVERY_NAME").Value;
                salesOrder.shipping_address.telephone = doc.Element("DELIVERY_TELEPHONE").Value;
                salesOrder.shipping_address.street = doc.Element("DELIVERY_ADDRESS_1").Value;
                salesOrder.shipping_address.city = doc.Element("DELIVERY_ADDRESS_CITY").Value;
                salesOrder.shipping_address.country_id = doc.Element("DELIVERY_ADDRESS_COUNTRY").Value;
                salesOrder.shipping_address.region = doc.Element("DELIVERY_ADDRESS_STATE").Value;
                salesOrder.shipping_address.postcode = doc.Element("DELIVERY_ADDRESS_ZIP").Value;
                salesOrder.shipping_address.company = doc.Element("DELIVERY_ADDRESS_COMPANY").Value;
                salesOrder.shipping_address.address_type = doc.Element("DELIVERY_ADDRESS_TYPE").Value;
                salesOrder.shipping_address.address_id = doc.Element("DELIVERY_ADDRESSID").Value;
                salesOrder.shipping_address.IsResidential = doc.Element("DELIVERY_ADDRESS_TYPE").Value.Equals("RESIDENTIAL") ? 1 : 0;
                salesOrder.payment = new EcomsalesOrderPaymentEntity();
                salesOrder.payment.method = doc.Element("PAYMENT_METHOD").Value;

                salesOrder.order_international = doc.Element("ORDER_INTERNATIONAL").Value;
                salesOrder.order_borderfree = doc.Element("ORDER_BORDERFREE").Value;
                salesOrder.is_tax_exempt_customer = doc.Element("IS_TAX_EXEMPT_CUSTOMER").Value;
                salesOrder.transtime = doc.Element("ORDER_TIME").Value;

                //gift card payments
                //Old Code
                /*
                if (salesOrder.payment.method.ToLower().Equals("free"))
                {
                    try
                    {
                        string json = doc.Element("GIFT_CARDS").Value;
                        salesOrder.gift_cards = JsonConvert.DeserializeObject<EcomGiftCard[]>(json);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                {
                    salesOrder.payment.cc_last4 = doc.Element("LAST_FOUR_CARD_DIGITS").Value;
                    salesOrder.payment.cc_exp_month = doc.Element("CC_EXP_MONTH").Value;
                    salesOrder.payment.cc_exp_year = doc.Element("CC_EXP_YEAR").Value;
                    salesOrder.payment.cc_type = doc.Element("CARD_TYPE").Value;
                    salesOrder.payment.cc_owner = doc.Element("CARD_HOLDER_NAME").Value;
                    salesOrder.payment.payment_transaction_id = doc.Element("TOKEN").Value;
                    salesOrder.payment.amount_ordered = salesOrder.grand_total;
                    salesOrder.payment.payment_id = doc.Element("TRANSARMOR_TOKEN").Value;
                }
                */
                string json = doc.Element("GIFT_CARDS").Value;
                if (json.Length > 0)
                {
                    try
                    {                        
                        salesOrder.gift_cards = JsonConvert.DeserializeObject<EcomGiftCard[]>(json);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }                
                salesOrder.payment.cc_last4 = doc.Element("LAST_FOUR_CARD_DIGITS").Value;
                salesOrder.payment.cc_exp_month = doc.Element("CC_EXP_MONTH").Value;
                salesOrder.payment.cc_exp_year = doc.Element("CC_EXP_YEAR").Value;
                salesOrder.payment.cc_type = doc.Element("CARD_TYPE").Value;
                salesOrder.payment.cc_owner = doc.Element("CARD_HOLDER_NAME").Value;
                salesOrder.payment.payment_transaction_id = doc.Element("TOKEN").Value;
                salesOrder.payment.amount_ordered = salesOrder.grand_total;
                salesOrder.payment.payment_id = doc.Element("TRANSARMOR_TOKEN").Value;

                var lineItems = doc.Elements("LINE_ITEMS").Elements("LINE_ITEM");

                if (lineItems.Any())
                {
                    salesOrder.items = new EcomsalesOrderItemEntity[lineItems.Count()];
                    int index = 0;
                    lineItems.ToList().ForEach(i =>
                    {
                        salesOrder.items[index] = new EcomsalesOrderItemEntity();
                        salesOrder.items[index].price = i.Element("PRICE").Value;
                        salesOrder.items[index].qty_ordered = i.Element("QUANTITY").Value;
                        salesOrder.items[index].sku = i.Element("SKU").Value;
                        salesOrder.items[index].monogram_font = i.Element("MONOGRAM_FONT").Value;
                        salesOrder.items[index].monogram_initials = i.Element("MONOGRAM_INITIALS").Value;
                        salesOrder.items[index].monogram_thread_color = i.Element("MONOGRAM_THREAD_COLOR").Value;
                        salesOrder.items[index].monogram_price = i.Element("MONOGRAM_PRICE").Value;
                        salesOrder.items[index].monogram_tax = i.Element("MONOGRAM_TAX").Value;
                        salesOrder.items[index].is_final_sales = Convert.ToInt32(i.Element("FINAL_SALE").Value);
                        salesOrder.items[index].tax_amount = i.Element("TAX").Value;
                        //Change these properties to get missing GiftCard attributes
                        //salesOrder.items[index].net_amount = i.Element("NET_AMOUNT") != null ? i.Element("NET_AMOUNT").Value : "";
                        //salesOrder.items[index].promotional_discount = i.Element("DISCOUNT_AMOUNT").Value != null ? i.Element("DISCOUNT_AMOUNT").Value : "";
                        //salesOrder.items[index].periodic_discount = i.Element("SPECIAL_PRICE_DISCOUNT").Value != null ? i.Element("SPECIAL_PRICE_DISCOUNT").Value : "";
                        //salesOrder.items[index].applied_rule_ids = i.Element("AX_DISCOUNT_CODE").Value != null ? i.Element("AX_DISCOUNT_CODE").Value : "";
                        //salesOrder.items[index].giftcard_recipient_name = i.Element("GIFTCARD_RECIPIENT_NAME").Value;
                        //salesOrder.items[index].giftcard_recipient_email = i.Element("GIFTCARD_RECIPIENT_EMAIL").Value != null ? i.Element("GIFTCARD_RECIPIENT_EMAIL").Value : "";
                        //salesOrder.items[index].giftcard_message = i.Element("GIFTCARD_MESSAGE").Value != null ? i.Element("GIFTCARD_MESSAGE").Value : "";
                        salesOrder.items[index].net_amount = i.Element("NET_AMOUNT").Value;
                        salesOrder.items[index].promotional_discount = i.Element("DISCOUNT_AMOUNT").Value;
                        salesOrder.items[index].periodic_discount = i.Element("SPECIAL_PRICE_DISCOUNT").Value;
                        salesOrder.items[index].applied_rule_ids = i.Element("AX_DISCOUNT_CODE").Value;
                        salesOrder.items[index].giftcard_recipient_name = i.Element("GIFTCARD_RECIPIENT_NAME").Value;
                        salesOrder.items[index].giftcard_recipient_email = i.Element("GIFTCARD_RECIPIENT_EMAIL").Value;
                        salesOrder.items[index].giftcard_message = i.Element("GIFTCARD_MESSAGE").Value;
                        index++;
                    });
                }
                salesOrder._integrationKey = new Data.IntegrationKey()
                {
                    ComKey = salesOrder.order_id
                };

                return salesOrder;
            }
            catch (Exception exp)
            {
                throw new Exception(string.Format("{0} --{1} -- {2}", "Error in Reading XML File", localFile, exp.Message));
            }
        }

        #endregion

        ErpSalesOrder ISaleOrderController.GetSalesOrders(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}