using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpProductDiscountWithAffiliation
    {
        public ErpProductDiscountWithAffiliation()
        {
        }

        public string OfferId { get; set; }//;
        public string OfferName { get; set; }
        public string ItemId { get; set; }//;	
        public string DiscountName { get; set; }//;
        public decimal DiscAmount { get; set; }//;
        public decimal DiscPct { get; set; }//;
        public decimal OfferPrice { get; set; }//;
        public decimal Quantity { get; set; }//;
        public string CurrencyCode { get; set; }//;	
        public string RetailvariantId { get; set; }
        public string SKU { get; set; }
        public int Status { get; set; }
        public System.DateTime ValidFrom { get; set; }//;
        public System.DateTime ValidTo { get; set; }//;	
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }//;
        public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties { get; set; }//;
                                                                                                            // Required for Magento CSV
        public string ColorId { get; set; }
        // Required for Magento CSV
        public string SizeId { get; set; }
        public string StyleId { get; set; }
        // Required for Magento CSV for Date in MM/dd/yyyy format
        public string ValidationFrom { get; set; }
        // Required for Magento CSV for Date in MM/dd/yyyy format
        public string ValidationTo { get; set; }

        public string TMV_ProductType { get; set; }

        public string AffiliationName { get; set; }
        public long AffiliationId { get; set; }
        public int PeriodicDiscountType { get; set; }
        public int DiscountType { get; set; }
        public int LineType { get; set; }
        public ErpRetailDiscountOfferLineDiscMethodBase DiscountMethod { get; set; }
        public decimal DiscPrice { get; set; }

    }
}
