using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 9 Platform new class
    public class ErpPaymentMerchantInformation
    {
        public ErpPaymentMerchantInformation()
        { }

        public string PaymentConnectorPropertiesXml { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }

        //NS: D365 Update 12 Platform change start
        public string MerchantPropertiesHashValue { get; set; }
        //NS: D365 Update 12 Platform change end
//HK: D365 Update 10.0 Application change start
        public string ServiceAccountId { get; set; }
//HK: D365 Update 10.0 Application change end
    }
}
