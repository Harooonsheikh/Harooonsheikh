using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpDenominationDetail
    {
        public ErpDenominationDetail()
        { }

        public int? Type { get; set; }
        public string Currency { get; set; }
        public decimal? DenominationAmount { get; set; }
        public int? QuantityDeclared { get; set; }
        public decimal? AmountDeclared { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
}
