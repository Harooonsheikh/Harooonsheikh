using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class ProductCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public ProductCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public IEnumerable<ErpProductExistenceId> VerifyProductExistence(long channelId, long catalogId, IEnumerable<ErpProductExistenceId> productExistenceIds, string storeKey)
        {
            var productController = _crtFactory.CreateProductController(storeKey);
            return productController.VerifyProductExistence(channelId, catalogId, productExistenceIds);
        }

        public List<KeyValuePair<long, IEnumerable<ErpProduct>>> GetCatalogProducts(long channelId, bool useDelta, List<ErpCategory> categories, bool fetchAll, string storeKey)
        {
            var productController = _crtFactory.CreateProductController(storeKey);
            return productController.GetCatalogProducts(channelId, useDelta, categories, fetchAll);
        }

        public IEnumerable<ErpProductCustomFields> GetProductCustomFields(List<long> productIds, string storeKey)
        {
            var productController = _crtFactory.CreateProductController(storeKey);
            return productController.GetProductCustomFields(productIds);
        }

        public IEnumerable<ErpUpsellItem> GetProductUpsell(List<string> itemIds, string storeKey)
        {
            var productController = _crtFactory.CreateProductController(storeKey);
            return productController.GetProductUpsell(itemIds);
        }

        public IEnumerable<ErpRetailInventItemSalesSetup> GetRetailInventItemSalesSetup(List<long> productsList, string storeKey)
        {
            var productController = _crtFactory.CreateProductController(storeKey);
            return productController.GetRetailInventItemSalesSetup(productsList);
        }

        public ProductImageUrlResponse ProductImageUrl(ProductImageUrlRequest productImageUrlRequest, string storeKey)
        {
            var productController = _crtFactory.CreateProductController(storeKey);
            return productController.GetProductImageUrl(productImageUrlRequest);
        }

        public void ProcessVariantCustomAttributes(List<ErpProduct> erpProducts, string storeKey)
        {
            var productController = _crtFactory.CreateProductController(storeKey);
            productController.ProcessCustomAttributeForVariant(erpProducts);
        }
    }
}
