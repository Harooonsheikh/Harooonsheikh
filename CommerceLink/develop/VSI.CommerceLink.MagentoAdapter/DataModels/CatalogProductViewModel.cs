using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.CommerceLink.MagentoAdapter.DataModels
{
    public class CatalogProductViewModel
    {
        public bool IsMasterProduct { get; set; }

        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }

        public string ProductName { get; set; }

        public string Description { get; set; }

        public ErpProductRules Rules { get; set; }

        public string AvaTax_TaxClassId { get; set; }

        public string Locale { get; set; }

        public string ColorId { get; set; }

        public string StyleId { get; set; }

        public List<ErpUpsellItem> UpsellItems { get; set; }

        public System.Collections.Generic.ICollection<ErpProductVariant> Variants { get; set; }

        public string ProductImageUrl { get; set; }

        public string SKU { get; set; }

        public string ItemId { get; set; }//;

        public decimal HighestQty { get; set; }

        public decimal LowestQty { get; set; }

        public long ReplenishmentWeight { get; set; }

        public decimal Price { get; set; }//;

        public string StoreViewCode { get; set; }

        public ErpChangeMode Mode { get; set; }


    }
}