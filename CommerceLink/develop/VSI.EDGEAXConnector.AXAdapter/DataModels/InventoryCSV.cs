namespace VSI.EDGEAXConnector.AXAdapter.DataModels
{
    public class InventoryCSV
    {
       
            #region Constructor
            /// <summary>
            /// Initializes a new instance of the class.
            /// </summary>
          
            #endregion

            #region Properties
            public string itemid { get; set; }
            public string color { get; set; }
            public string name { get; set; }
            public string size { get; set; }
            public string sku { get; set; }
            public string type { get; set; }
            public decimal qty { get; set; }
            public int is_in_stock { get; set; }

            #endregion Properties

        
    }
}
