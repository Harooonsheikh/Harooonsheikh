using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ECommerceDataModels
{
    public partial class EcomcatalogProductCreateEntity
    {
        public long RecordId { get; set; }
        public string SKU { get; set; }
        public string _attribute_set { get; set; }

        public string _type { get; set; }

        public string Color { get; set; }

        public string ColorId { get; set; }

        public string ConfigId { get; set; }

        public string Configuration { get; set; }

        public long DistinctProductVariantId { get; set; }

        public string InventoryDimensionId { get; set; }

        public long MasterProductId { get; set; }

        public bool IsMasterProduct { get; set; }

        public string Size { get; set; }

        public string SizeId { get; set; }

        public string Style { get; set; }

        public string StyleId { get; set; }

        public string VariantId { get; set; }

        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }

        public string MasterProductSKU { get; set; }

        //public string use_config_qty_increments { get; set; }

        //public string qty_increments { get; set; }

        //public string use_config_enable_qty_inc { get; set; }
        //public string enable_qty_increments { get; set; }
        //public string is_decimal_divided { get; set; }
        //public string RootCategroy { get; set; }

        public string associated { get; set; }

        public string config_attributes { get; set; }

        public List<EcomcatalogProductImageEntity> Images { get; set; }
        public string Barcode { get; set; }
        public string ItemId { get; set; }
        public decimal AvailableQuantity { get; set; }

        public string special_price { get; set; }//;
        public string special_from_date { get; set; }//;
        public string ax_discount_code { get; set; }
        public string special_to_date { get; set; }//;

        public string Mode { get; set; }
     
        //public int is_in_stock { get; set; }
    }
}
