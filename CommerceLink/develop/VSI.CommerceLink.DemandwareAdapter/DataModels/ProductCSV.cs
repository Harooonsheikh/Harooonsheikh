using System.Collections.Generic;

namespace VSI.CommerceLink.DemandwareAdapter.DataModels
{
    /// <summary>
    /// ProductCSV
    /// </summary>
    public class ProductCSV
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProductCSV()
        {
        }
        #endregion

        #region Properties
        public string category_ids { get; set; }
        public string color { get; set; }
        public string description { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        //public string special_price { get; set; }//TODO: to be implemented with Discount and Markdown flow
        //public string upsell { get; set; }//TODO: to be implemented in next phase left for CRP2
        public string short_description { get; set; }
        public string simples_skus { get; set; }
        public string size { get; set; }
        public string sku { get; set; }
        public string type { get; set; }
        public string upc { get; set; }
        public string visibility { get; set; } 
        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }
        public decimal qty { get; set; }
        public int is_in_stock { get; set; }

        public string special_price { get; set; }
        public string special_from_date { get; set; }
        public string special_to_date { get; set; }

        public string ax_discount_code { get; set; }

        #endregion Properties

    }
}
