using System.Collections.Generic;
namespace VSI.EDGEAXConnector.ECommerceDataModels.Ingram
{
    public class IngramSalesOrderAssetAttributeResponse
    {
        public IngramSalesOrderAssetAttributeResponse()
        {
        }
        
        public string external_id { get; set; }
        public string external_uid { get; set; }
        public string id { get; set; }
        
        public IngramAssetConnectionAttributeResponse connection { get; set; }
        public List<IngramAssetItemsAttributeResponse> items { get; set; }
        public List<IngramAssetParamsAttributeResponse> @params { get; set; }
        public IngramAssetProductAttributeResponse product { get; set; }
        public IngramAssetTiersAttributeResponse tiers { get; set; }
    }
}
