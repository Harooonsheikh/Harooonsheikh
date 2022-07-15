using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpProductDiscount
    {
        public ErpProductDiscount()
		{
		}
		public string OfferId	{ get; set; }//;
        public string ItemId { get; set; }//;	
		public string DiscountName	{ get; set; }//;
		public decimal DiscAmount	{ get; set; }//;
        public decimal DiscPct { get; set; }//;
        public decimal DiscPrice { get; set; }
        public decimal OfferPrice { get; set; }//;
        public decimal Quantity { get; set; }//;
		public string CurrencyCode	{ get; set; }//;	
        public string RetailvariantId { get; set; }
        public string SKU { get; set; }
        public int Status { get; set; }
		public System.DateTime ValidFrom	{ get; set; }//;
        public System.DateTime ValidTo { get; set; }//;	
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
	
    }
}
