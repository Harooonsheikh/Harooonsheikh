using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 9 Platform new class
    public class ErpProductDimensionValue
    {
        public ErpProductDimensionValue()
        {

        }

        public long RecordId { get; set; }
        public string Value { get; set; }
        //NS: D365 Update 12 Platform change start
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
        //NS: D365 Update 12 Platform change end
//HK: D365 Update 10.0 Application change start
        public string DimensionId { get; set; }
//HK: D365 Update 10.0 Application change start
    }
}
