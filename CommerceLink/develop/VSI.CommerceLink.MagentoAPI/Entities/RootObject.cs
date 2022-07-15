using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.MagentoAPI.Entities
{
    public class RootObject
    {
        public string base_currency_code { get; set; }
        public string base_discount_amount { get; set; }
        public string base_grand_total { get; set; }
        public string base_discount_tax_compensation_amount { get; set; }
        public string base_shipping_amount { get; set; }
        public string base_shipping_discount_amount { get; set; }
        public string base_shipping_incl_tax { get; set; }
        public string base_shipping_tax_amount { get; set; }
        public string base_subtotal { get; set; }
        public string base_tax_amount { get; set; }
        public string base_total_due { get; set; }
        public string base_to_global_rate { get; set; }
        public string base_to_order_rate { get; set; }
        public string billing_address_id { get; set; }
        public string created_at { get; set; }
        public string customer_email { get; set; }
        public string customer_firstname { get; set; }
        public string customer_group_id { get; set; }
        public string customer_id { get; set; }
        public string customer_is_guest { get; set; }
        public string customer_lastname { get; set; }
        public string customer_note_notify { get; set; }
        public string discount_amount { get; set; }
        public string entity_id { get; set; }
        public string global_currency_code { get; set; }
        public string grand_total { get; set; }
        public string discount_tax_compensation_amount { get; set; }
        public string increment_id { get; set; }
        public string is_virtual { get; set; }
        public string order_currency_code { get; set; }
        public string protect_code { get; set; }
        public string quote_id { get; set; }
        public string remote_ip { get; set; }
        public string shipping_amount { get; set; }
        public string shipping_description { get; set; }
        public string shipping_discount_amount { get; set; }
        public string shipping_discount_tax_compensation_amount { get; set; }
        public string shipping_incl_tax { get; set; }
        public string shipping_tax_amount { get; set; }
        public string state { get; set; }
        public string status { get; set; }
        public string store_currency_code { get; set; }
        public string store_id { get; set; }
        public string store_name { get; set; }
        public string store_to_base_rate { get; set; }
        public string store_to_order_rate { get; set; }
        public string subtotal { get; set; }
        public string subtotal_incl_tax { get; set; }
        public string tax_amount { get; set; }
        public string total_due { get; set; }
        public string total_item_count { get; set; }
        public string total_qty_ordered { get; set; }
        public string updated_at { get; set; }
        public string weight { get; set; }
        public List<Item> items { get; set; }
        public BillingAddress billing_address { get; set; }
        public Payment payment { get; set; }
        public List<StatusHistory> status_histories { get; set; }
        public ExtensionAttributes extension_attributes { get; set; }
    }
}
