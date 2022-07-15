using System;
using System.Collections.Generic;

namespace VSI.EDGEAXConnector.ECommerceDataModels.Ingram
{
    public class IngramSalesOrderResponse
    {
        public IngramSalesOrderResponse()
        {
        }

        public string activation_key { get; set; }
        public string id { get; set; }
        public string reason { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public List<string> errors {get; set;}
        public string error_code { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        public IngramSalesOrderAssetAttributeResponse asset { get; set; }
        public IngramSalesOrderContractAttributeResponse contract { get; set; }
        public IngramSalesOrderMarketPlaceAttributeResponse marketplace { get; set; }
    }

    public class IngramSalesOrderErrors
    {
        public List<string> error { get; set; }
    }
}
