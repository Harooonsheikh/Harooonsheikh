using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;


namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface IProductController
    {
        List<ErpProduct> GetAllProducts(bool useDelta, List<ErpCategory> categories, bool fetchAll);
        ErpLinkedProduct GetProductLinkedItemExtension(string itemId, string variantId);
        List<ErpProduct> GetCatalogMasterProducts(long catalogId);
        ProductImageUrlResponse GetProductImageUrl(ProductImageUrlRequest productImageUrlRequest);
    }
}
