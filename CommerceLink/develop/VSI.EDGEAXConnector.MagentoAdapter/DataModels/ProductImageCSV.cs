namespace VSI.EDGEAXConnector.MagentoAdapter.DataModels
{
    /// <summary>
    /// ProductCSV
    /// </summary>
    public class ProductImageCSV
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProductImageCSV()
        {
            //this.sku = string.Empty;
            //this.image = string.Empty;
            //this.small_image = string.Empty;
            //this.thumbnail = string.Empty;
            //this.catalog_image_front = string.Empty;
            //this.catalog_image_back = string.Empty;
            //this.color_swatch = string.Empty;
        }

        #endregion

        #region Properties

        //public string store { get; set; }

        //public string websites { get; set; }

        public string sku { get; set; }

        public string media_gallery { get; set; }

        public string image { get; set; }

        public string image_label{ get; set; }

        public string small_image { get; set; }

        public string small_image_label{ get; set; }

        public string thumbnail { get; set; }

        public string thumbnail_label { get; set; }

        //public string catalog_image_front { get; set; }

        //public string catalog_image_back { get; set; }

        //public string color_swatch { get; set; }
        #endregion Properties

    }
}
