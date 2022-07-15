using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpThirdPartyGiftCardInfo
    {
        public ErpThirdPartyGiftCardInfo()
        {

        }
        public decimal Amount { get; set; }
        public string Authorization { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
}
