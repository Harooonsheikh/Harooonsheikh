using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{

    public partial class ErpCardTokenInfo
    {
        public string CardToken { get; set; }
        public string UniqueCardId { get; set; }
        public string ServiceAccountId { get; set; }
        public string MaskedCardNumber { get; set; }
        public string CardTokenBlob { get; set; }
        public bool UseShippingAddress { get; set; }//;
        public string Country { get; set; }//;
        public string Address1 { get; set; }//;
        public string Address2 { get; set; }//;
        public string City { get; set; }//;
        public string State { get; set; }//;
        public string Zip { get; set; }//;
        public string Phone { get; set; }//;
        public string NameOnCard { get; set; }//;
        public string CardTypes { get; set; }//;
    }
    public partial class ErpCardAuthorizeRequest
    {
        public ErpCardTokenInfo CardInfo { get; set; }
        public decimal Amount { get; set; }
        public bool IsSwipe { get; set; }
    }
    public partial class ErpCardAuthorizeResponse
    {
        public string AuthorizationBlob { get; set; }
        public ErpCardTokenInfo CardInfo { get; set; }
        public string AuthorizationToken { get; set; }
        public decimal Amount { get; set; }
        public bool IsSwipe { get; set; }
    }
}
