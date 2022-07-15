using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpProductDimensionCombination
    {
        public ErpProductDimensionCombination()
        {

        }

        public ObservableCollection<ErpProductDimension> ProductDimensions { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
}
