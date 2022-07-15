using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpProductDimension
    {
        public ErpProductDimension()
        {

        }

        public int DimensionTypeValue { get; set; }
        public ErpProductDimensionValue DimensionValue { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
}
