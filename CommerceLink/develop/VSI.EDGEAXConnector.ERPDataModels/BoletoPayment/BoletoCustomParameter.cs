using System;
using System.Xml.Serialization;

namespace VSI.EDGEAXConnector.ERPDataModels.BoletoPayment
{
    public class BoletoCustomParameter
    {
        public string Custom_CPF_number { get; set; }
        public string Custom_due_date { get; set; }
        public string Merchant_Sitename { get; set; }
        public string Product { get; set; }
    }
}
