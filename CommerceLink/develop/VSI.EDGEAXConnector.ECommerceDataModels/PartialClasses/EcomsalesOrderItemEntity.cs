using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ECommerceDataModels
{
    public partial class EcomsalesOrderItemEntity
    {
        public string net_amount { get; set; }
        public string monogram_initials { get;set; }
        public string monogram_font { get;set; }
        public string monogram_thread_color { get;set; }
        public string monogram_price { get; set; }
        public string monogram_tax { get; set; }
        public int is_final_sales { get;set; }
        public string periodic_discount { get; set; }
        public string periodic_discount_per { get; set; }
        public string promotional_discount { get; set; }
        public string promotional_discount_per { get; set; }
        public string giftcard_recipient_email { get; set; }
        public string giftcard_recipient_name { get; set; }
        public string giftcard_message { get; set; }
    }
}
