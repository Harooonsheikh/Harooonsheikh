using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpProduct
    {
        public string Color { get; set; }
       
        public string ColorId { get; set; }
       
        public string ConfigId { get; set; }
      
        public string Configuration { get; set; }
       
        public long DistinctProductVariantId { get; set; }

        public string InventoryDimensionId { get; set; }
               
        public long MasterProductId { get; set; }
     
        public string Size { get; set; }
       
        public string SizeId { get; set; }
       
        public string Style { get; set; }
       
        public string StyleId { get; set; }
       
        public string VariantId { get; set; }

        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }

        public string MasterProductNumber { get; set; }
        /// <summary>
        /// Root Category Name
        /// </summary>
        public string RootCategory { get; set; }
        public string PriemaryCategory { get; set; }
        /// <summary>
        /// All Category Hierarchies
        /// </summary>
        public List<string> Categories { get; set; }
        public string Status { get; set; }
        public string Barcode { get; set; }
        public string SKU { get; set; }
        public long CatalogId { get; set; }
        public decimal AvailableQuantity { get; set; }

        public string ValidToDate { get; set; }
        public string ValidFromDate { get; set; }
        public string SpecialPrice { get; set; }
        public ErpChangeMode Mode { get; set; } 
        public List<ErpProductVariant> ProductVariants { get; set; }
        public string EcomProductId { get; set; }
        public List<ErpProductDimensionSet> DimensionSets { get; set; }

        public List<ErpRichMediaLocationsRichMediaLocation> ImageList { get; set; }

        public string PaddedItemId { get; set; }
        public string AllocationTimeStamp { get; set; }

        // This would contain data like language, languageDetail
        public Dictionary<String, Dictionary<String, String>> ProductDetailTranslationsDictionary { get; set; }

        // This value would contain highest quantity to be rounded
        public decimal HighestQty { get; set; }
        // This value would contain lowest quantity to be rounded
        public decimal LowestQty { get; set; }
        public string StoreViewCode { get; set; }

        #region Following propertiees are for Magento CSV creation
        public string ProductType { get; set; }
        public string AdditionalAttributes { get; set; }
        public string ConfigurableVariations { get; set; }
        public string ConfigurableVariationLabels { get; set; }
        public string Visibility { get; set; }
        public string CompleteHirarchy { get; set; }
        public string ImageUrl { get; set; }
        public List<ErpUpsellItem> UpsellItems { get; set; }
        public long ReplenishmentWeight { get; set; }
        #endregion

        public long ChannelId { get; set; }
    }
}
