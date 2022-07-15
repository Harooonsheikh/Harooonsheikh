using System.IO;
using System.Linq;
using System.Xml.Linq;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.CommerceLink.MagentoAPI.MageAPI;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;

namespace VSI.CommerceLink.MagentoAdapter.Helpers
{

    /// <summary>
    /// This class is helper class for sales order xml.
    /// </summary>
    public class SalesOrderXmlHelper
    {
        public StoreDto currentStore = null;
        public SalesOrderXmlHelper( string storeKey)
        {
            this.currentStore = StoreService.GetStoreByKey(storeKey);
            //this.LoadExcludeAttributeNames();
        }
        #region Public Methods

        /// <summary>
        /// GetSalesOrderFromXML gets sales order from xml.
        /// </summary>
        /// <param name="localFile"></param>
        /// <returns></returns>
        public EcomsalesOrderEntity GetSalesOrderFromXML(string localFile)
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
            salesOrder.shipping_amount = doc.Element("SHIPPING_CHARGE").Value;
            salesOrder.tax_amount = doc.Element("TAX_CHARGE").Value;
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

            salesOrder.payment = new EcomsalesOrderPaymentEntity();
            salesOrder.payment.method = doc.Element("PAYMENT_METHOD").Value;
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
                    salesOrder.items[index].tax_amount = i.Element("TAX").Value;
                    salesOrder.items[index].net_amount = i.Element("NET_AMOUNT") != null ? i.Element("NET_AMOUNT").Value : "";
                    salesOrder.items[index].discount_amount = i.Element("DISCOUNT_AMOUNT").Value != null ? i.Element("DISCOUNT_AMOUNT").Value : "";

                    index++;
                });
            }
            salesOrder._integrationKey = new IntegrationKey()
            {
                ComKey = salesOrder.order_id
            };
            return salesOrder;
        }

        /// <summary>
        /// ConvertSalesOrderToXML converts sales order to XML.
        /// </summary>
        /// <param name="salesOrder"></param>
        public void ConvertSalesOrderToXML(salesOrderEntity salesOrder)
        {
            if (salesOrder == null)
            {
                return;
            }

            var salesOrderHeader = new XElement("Order",
                                new XElement("CUSTOMER_NAME", salesOrder.customer_firstname),
                                 new XElement("CUSTOMER_ADDRESS_1", salesOrder.billing_address.street),
           new XElement("CUSTOMER_ADDRESS_2", ""),
           new XElement("CUSTOMER_ADDRESS_CITY", salesOrder.billing_address.city),
           new XElement("CUSTOMER_ADDRESS_COUNTRY", salesOrder.billing_address.country_id),
           new XElement("CUSTOMER_ADDRESS_STATE", salesOrder.billing_address.region),
           new XElement("CUSTOMER_ADDRESS_ZIP", salesOrder.billing_address.postcode),
           new XElement("CUSTOMER_ADDRESS_COMPANY", salesOrder.billing_address.company),
           new XElement("CUSTOMER_CODE", salesOrder.customer_id),
           new XElement("CUSTOMER_TELEPHONE", salesOrder.billing_address.telephone),
           new XElement("SHIPTO_NAME", salesOrder.shipping_name), //HOME
           new XElement("DELIVERY_NAME", salesOrder.shipping_name),
           new XElement("DELIVERY_ADDRESS_1", salesOrder.shipping_address.street),
           new XElement("DELIVERY_ADDRESS_2", ""),
           new XElement("DELIVERY_ADDRESS_CITY", salesOrder.shipping_address.city),
           new XElement("DELIVERY_ADDRESS_COUNTRY", salesOrder.shipping_address.country_id),
           new XElement("DELIVERY_ADDRESS_STATE", salesOrder.shipping_address.region), //
           new XElement("DELIVERY_ADDRESS_ZIP", salesOrder.shipping_address.postcode),

           new XElement("DELIVERY_ADDRESS_COMPANY", salesOrder.shipping_address.company), //
           new XElement("DELIVERY_ADDRESS_TYPE", ""),//UNKNOWN
           new XElement("EMAIL", salesOrder.customer_email),

           new XElement("CARD_TYPE", salesOrder.payment.cc_type),
           new XElement("LAST_FOUR_CARD_DIGITS", salesOrder.payment.cc_last4),
           new XElement("ORDER_ID", salesOrder.increment_id),
           new XElement("ORDER_DATE", salesOrder.created_at),
           new XElement("ORDER_TYPE", ""),//JOIE.COM ECOMMERCE
           new XElement("SERVICE", salesOrder.shipping_method),
            new XElement("STORE_ID", salesOrder.store_id),
           new XElement("CARRIER", ""),//UPS
           new XElement("EST_SHIPPING_DATE", ""),
           new XElement("TAX_CHARGE", salesOrder.tax_amount),//
           new XElement("SHIPPING_CHARGE", salesOrder.shipping_amount),
           new XElement("DISCOUNT_AMOUNT", salesOrder.discount_amount),
            new XElement("DISCOUNT_CODE", ""),
             new XElement("TOTAL", salesOrder.grand_total));

            var lineItems = new XElement("LINE_ITEMS");
            foreach (var item in salesOrder.items)
            {
                var lineitem = new XElement("LINE_ITEM",
                    new XElement("PRICE", item.price),
                    new XElement("QUANTITY", item.qty_ordered),
                    new XElement("SIZE", ""),
                     new XElement("SKU", item.sku),
                    new XElement("UPC_CODE", ""),
                      new XElement("TAX", item.tax_amount),
                     new XElement("TAX_GROUP", ""),
                    new XElement("DISCOUNT_AMOUNT", item.discount_amount),
                     new XElement("NET_AMOUNT", item.row_total));
                lineItems.Add(lineitem);
            }
            salesOrderHeader.Add(lineItems);
        }

        /// <summary>
        /// This function moves file to local folder.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        public  void MoveToLocalFolder(string file, string folder)
        {

            ConfigurationHelper configurationHelper = new ConfigurationHelper(currentStore.StoreKey);
            string localDir = configurationHelper.GetSetting(SALESORDER.Singlefile_Input_Path);

            if (!string.IsNullOrWhiteSpace(localDir))
            {
                if (!localDir.EndsWith(@"\"))
                {
                    localDir += @"\";
                }

                if (!string.IsNullOrWhiteSpace(folder))
                {
                    localDir += folder + @"\";
                }

                if (!Directory.Exists(localDir))
                {
                    Directory.CreateDirectory(localDir);
                }

                string fileName = Path.GetFileName(file);
                File.Move(file, localDir + fileName);
            }
        }

        #endregion
    }
}
