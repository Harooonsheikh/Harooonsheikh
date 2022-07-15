using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IProductController
    {
        IEnumerable<ErpProductExistenceId> VerifyProductExistence(long channelId, long catalogId, IEnumerable<ErpProductExistenceId> productExistenceIds);

        List<KeyValuePair<long, IEnumerable<ErpProduct>>> GetCatalogProducts(long channelId, bool useDelta, List<ErpCategory> categories, bool fetchAll);

        IEnumerable<ErpProductCustomFields> GetProductCustomFields(List<long> productIds);

        IEnumerable<ErpUpsellItem> GetProductUpsell(List<string> itemIds);

        IEnumerable<ErpRetailInventItemSalesSetup> GetRetailInventItemSalesSetup(List<long> productIds);
        ProductImageUrlResponse GetProductImageUrl(ProductImageUrlRequest productImageUrlRequest);
        void ProcessCustomAttributeForVariant(List<ErpProduct> erpProduct);
    }
}
