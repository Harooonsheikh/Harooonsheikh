namespace VSI.EDGEAXConnector.MagentoAdapter.DataModels
{
    public class DiscountCSV
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DiscountCSV()
        {
        }
        #endregion

        #region Properties


        public string sku { get; set; }
        public string special_price { get; set; }
        public string special_from_date { get; set; }
        public string special_to_date { get; set; }
        
        #endregion Properties
    }
}
