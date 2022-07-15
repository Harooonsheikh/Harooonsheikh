using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpTokenizedPaymentCard
    {
        public ErpTokenizedPaymentCard()
        {

        }

        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public ErpCardTokenInfo CardTokenInfo { get; set; }
        public string CardTypeId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int? ExpirationMonth { get; set; }
        public int? ExpirationYear { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
        public bool? IsSwipe { get; set; }
        public string NameOnCard { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string TenderType { get; set; }
        public string Zip { get; set; }

//HK: D365 Update 10.0 Application change start
        public string House { get; set; }
//HK: D365 Update 10.0 Application change end
    }
}
