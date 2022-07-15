using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 9 Platform new class
    public class ErpReturnLabelContent
    {
        public ErpReturnLabelContent()
        {

        }

        public string ReturnLocationText { get; set; }
        public string ReturnWarehouseText { get; set; }
        public string ReturnReasonText { get; set; }
        public string ReturnPalleteText { get; set; }

        //NS: D365 Update 12 Platform change start
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
        //NS: D365 Update 12 Platform change end
    }
}
