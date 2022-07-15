using System.Collections.Generic;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpCatalog
    {
        public ErpCatalog()
        {
            //Categories = new List<ErpCategory>();
            //Products = new List<ErpProduct>();
            //CategoryAssignments = new List<ErpCategoryAssignment>();
        }
        public List<ErpCategory> Categories { get; set; }
        public List<ErpProduct> Products { get; set; }
       // public List<ErpProduct> KitProducts { get; set; } //Commented Kit Implementation
        public List<ErpCategoryAssignment> CategoryAssignments { get; set; }
        public List<ErpProductDimensionSet> DimensionSets { get; set; }
        public List<ErpProduct> CatalogMasterProducts { get; set; }
    }
}
