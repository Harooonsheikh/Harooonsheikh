using Microsoft.Dynamics.Commerce.RetailProxy;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using NewRelic.Api.Agent;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using IProductManager = Microsoft.Dynamics.Commerce.RetailProxy.IProductManager;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class ProductController : BaseController, IProductController
    {

        #region Public Methods
        public ProductController(string storeKey) : base(storeKey)
        {

        }
        public IEnumerable<ErpProductExistenceId> VerifyProductExistence(long channelID, long catalogId, IEnumerable<ErpProductExistenceId> productExistenceIds)
        {
            return null;
        }
        public List<KeyValuePair<long, IEnumerable<ErpProduct>>> GetCatalogProducts(long channelID, bool useDelta, List<ErpCategory> categories, bool fetchAll)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                if (useDelta)
                {
                    // This function returns only the delta of Catalog
                    return GetCatalogProductsAsync(baseChannelId).Result;
                }
                else
                {
                    // This function returns all the products of Catalog
                    return AsyncGetCatalogProducts(baseChannelId, fetchAll, categories).Result;
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
        }
        #endregion

        #region Private Methods
        private async Task<List<KeyValuePair<long, IEnumerable<ErpProduct>>>> AsyncGetCatalogProducts(long channelID, bool fetchAll, List<ErpCategory> categories)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var productManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager>();

            //var AvaTaxCodes = new List<AvaTax>();
            //if (Convert.ToBoolean(configurationHelper.GetSetting(PRODUCT.IsAvaTaxEnabled)))
            //{
            //    var avaTaxCodeResult = ECL_TV_GetAvaTaxCode(productManager);
            //    AvaTaxCodes = JsonConvert.DeserializeObject<List<AvaTax>>(avaTaxCodeResult.Result);
            //}

            long? retailServerPagingTop = (long?)Convert.ToInt64(configurationHelper.GetSetting(PRODUCT.Retail_Server_Paging));
            // REMOVE THIS LINE // CustomLogger.LogDebugInfo(string.Format("Enter in AsyncGetCatalogProducts()"));
            string categoryFullName;
            long channelId = channelID, catalogID, categoryID;
            List<KeyValuePair<long, IEnumerable<ErpProduct>>> kvpERPProducts = new List<KeyValuePair<long, IEnumerable<ErpProduct>>>();

            QueryResultSettings catalogQuerySetting = new QueryResultSettings();
            catalogQuerySetting.Paging = new PagingInfo();
            catalogQuerySetting.Paging.Skip = 0;
            catalogQuerySetting.Paging.Top = retailServerPagingTop;

            var productCatalogs = GetCatalogs(channelId, catalogQuerySetting);

            if (productCatalogs != null && productCatalogs.Count() > 0)
            {
                foreach (ProductCatalog objProductCatalog in productCatalogs)
                {
                    ObservableCollection<long> catalogProductRecId = new ObservableCollection<long>();
                    catalogID = objProductCatalog.RecordId;
                    // REMOVE THIS LINE // CustomLogger.LogDebugInfo(string.Format("Get catalog \"{0}\" catalog id = {1} from Retail Server for Channel ID {2}", objProductCatalog.Name, catalogID, channelId));
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10215, currentStore, objProductCatalog.Name, catalogID, channelID);
                    List<ErpProduct> lstERPProd = new List<ErpProduct>();

                    if (categories == null)
                    {
                        var prCategories = GetCategories(channelId, catalogQuerySetting);

                        categories = _mapper.Map<List<ErpCategory>>(prCategories.Results) ?? new List<ErpCategory>();
                    }

                    if (categories != null && categories.Count > 0)
                    {
                        PagedResult<ProductSearchResult> searchProductResult = null;
                        PagedResult<SimpleProduct> productDetails = null;

                        QueryResultSettings productQueryResultSettings = new QueryResultSettings();
                        productQueryResultSettings.Paging = new PagingInfo();
                        productQueryResultSettings.Paging.Skip = 0;
                        productQueryResultSettings.Paging.Top = retailServerPagingTop;

                        foreach (var currentCategory in categories)
                        {
                            List<ProductSearchResult> reslts = new List<ProductSearchResult>();
                            searchProductResult = new PagedResult<ProductSearchResult>();
                            productDetails = new PagedResult<SimpleProduct>();
                            categoryID = currentCategory.RecordId;
                            categoryFullName = currentCategory.FullName;
                            productQueryResultSettings.Paging.Skip = 0;
                            do
                            {
                                //++RSCall
                                searchProductResult = ECL_SearchUsingCategory(productManager, channelId, catalogID, categoryID, productQueryResultSettings);
                                //searchProductResult.Results.AsEnumerable().Each(m => reslts.Add(m));
                                foreach (var product in searchProductResult.Results)
                                {
                                    reslts.Add(product);
                                }
                                productQueryResultSettings.Paging.Skip += productQueryResultSettings.Paging.Top;
                            }
                            while (searchProductResult.Count() == productQueryResultSettings.Paging.Top);

                            if (searchProductResult.Count() == 0)
                            {
                                continue;
                            }

                            ObservableCollection<long> searchIds = new ObservableCollection<long>();
                            reslts.Select(m => m.RecordId).ToList().ForEach(p => { searchIds.Add(p); catalogProductRecId.Add(p); });

                            ProductSearchCriteria productCriteria = new ProductSearchCriteria();
                            productCriteria.Ids = searchIds;
                            productCriteria.SkipVariantExpansion = false;
                            productCriteria.IncludeProductsFromDescendantCategories = true;
                            productCriteria.Context = new ProjectionDomain();
                            productCriteria.Context.ChannelId = channelId;
                            productCriteria.Context.CatalogId = objProductCatalog.RecordId;

                            PagedResult<Product> masterProductsResult = null;
                            List<Product> masterProducts = new List<Product>();

                            QueryResultSettings masterProductQuerySetting = new QueryResultSettings();
                            masterProductQuerySetting.Paging = new PagingInfo();
                            masterProductQuerySetting.Paging.Skip = 0;
                            masterProductQuerySetting.Paging.Top = retailServerPagingTop;

                            do
                            {
                                //++RSCall
                                masterProductsResult = ECL_SearchProduct(productManager, productCriteria, masterProductQuerySetting);
                                //masterProductsResult.Results.AsEnumerable().Each(m => masterProducts.Add(m));
                                foreach (var product in masterProductsResult.Results)
                                {
                                    masterProducts.Add(product);
                                }
                                masterProductQuerySetting.Paging.Skip += masterProductQuerySetting.Paging.Top;
                            }
                            while (masterProductsResult.Count() == masterProductQuerySetting.Paging.Top);



                            //var productSearchJson = masterProducts.SerializeToJson(1);
                            // REMOVE THIS LINE // CustomLogger.LogDebugInfo(string.Format("AX Category: {0} have {1} No of Products Setup in that category", currentCategory.Name, searchProductResult.Count().ToString()));
                            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10216, currentStore, currentCategory.Name, searchProductResult.Count().ToString());
                            if (masterProducts != null)
                            {
                                // Fetch Images
                                //List<ProductImage> productImages = new List<ProductImage>();
                                //try
                                //{
                                //    productImages = GetProductImageUrl(new ProductImageUrlRequest
                                //    {
                                //        Products = masterProducts.Select(a => a.ProductNumber).ToList()
                                //    })?.Products.Where(a => !string.IsNullOrEmpty(a.ImageUrl)).ToList();
                                //}
                                //catch (Exception ex)
                                //{
                                //    CommerceLinkLogger.LogException(currentStore, ex, "GetProductImageUrl",
                                //        Guid.NewGuid().ToString());
                                //}

                                foreach (Product product in masterProducts)
                                {
                                    // AQ BEGIN: No need to skip any product.
                                    // In case of duplicate product add categories to existing product in the list.
                                    // Category management code is written below
                                    //if (lstERPProd.Any(m => m.RecordId == product.RecordId))
                                    //{
                                    //    continue;
                                    //}
                                    // AQ END

                                    ErpProduct erpProduct = _mapper.Map<ErpProduct>(product);
                                    erpProduct.CatalogId = catalogID;
                                    erpProduct.CategoryIds = new List<long> { categoryID };
                                    erpProduct.Categories = new List<string> { categoryFullName };
                                    erpProduct.Mode = ErpChangeMode.Insert;
                                    erpProduct.MasterProductNumber = product.ItemId; //?? why are we assigning itemId to MasterProductNumber 

                                    //var productImage = productImages?.FirstOrDefault(x => x.Id == product.ProductNumber);
                                    //if (productImage != null)
                                    //{
                                    //    erpProduct.ProductImageUrl = productImage.ImageUrl;
                                    //}

                                    ProcessProductDetails(productManager, erpProduct, product, channelId, catalogID);

                                    erpProduct.ProductDetailTranslationsDictionary = processProductProperties(product);
                                    if (product.IsMasterProduct)
                                    {
                                        ProcessProductVariants(product, erpProduct, searchIds);

                                        //if (Convert.ToBoolean(configurationHelper.GetSetting(PRODUCT.IsAvaTaxEnabled)))
                                        //{
                                        //    erpProduct.AvaTax_TaxClassId = AvaTaxCodes.FirstOrDefault(t => t.ItemId == erpProduct.ItemId)?.TaxabilityCode;
                                        //}
                                    }

                                    if (fetchAll)
                                    {
                                        ProcessProductDimensions(productManager, product, erpProduct, channelId, catalogID);
                                        ProcessCustomAttribute(productManager, erpProduct, channelId, catalogID, catalogQuerySetting);
                                    }

                                    // Get related products for flat hierarchy
                                    bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();
                                    if (isFlatProductHierarchy)
                                    {
                                        ProcessRelatedProducts(erpProduct, channelId, catalogID);
                                    }

                                    ErpProduct existingProduct = lstERPProd.FirstOrDefault(m => m.RecordId == erpProduct.RecordId);
                                    if (existingProduct == null)
                                    {
                                        lstERPProd.Add(erpProduct);
                                    }
                                    else
                                    {
                                        if (existingProduct.CategoryIds == null)
                                        {
                                            existingProduct.CategoryIds = new List<long>();
                                        }
                                        foreach (long productCategoryID in erpProduct.CategoryIds)
                                        {
                                            existingProduct.CategoryIds.Add(productCategoryID);
                                        }

                                        if (existingProduct.Categories == null)
                                        {
                                            existingProduct.Categories = new List<string>();
                                        }
                                        existingProduct.Categories.AddRange(erpProduct.Categories);
                                    }

                                } // foreach Product 
                            } // if (objProduct != null)
                        } // foreach Category

                        //this.ProcessProductCustomFields(lstERPProd);

                        // Add the List to Key Value Pair List
                        kvpERPProducts.Add(new KeyValuePair<long, IEnumerable<ErpProduct>>(catalogID, lstERPProd)); // objProductSearchResult.RecordId updated to catalogID
                    }
                    else
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40206));
                        throw new NullReferenceException(message);
                    }
                }
            }
            else
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40206));
                throw new NullReferenceException(message);
            }

            // REMOVE THIS LINE // CustomLogger.LogDebugInfo(string.Format("Exit from AsyncGetCatalogProducts()"));
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            // Return the Key Value Pair List containing the ErpProducts ...
            if (kvpERPProducts.Count == 0)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name
                                , CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40207));
                throw new NullReferenceException(message);
            }
            else
                return kvpERPProducts;
        }
        /// <summary>
        /// Asynchronous Get Related Products
        /// </summary>
        /// <param name="recordId">Product Record Id</param>
        /// <param name="channelID">channelID</param>
        /// <param name="catalogId">Catalog Id</param>
        /// <returns></returns>
        private async Task<ICollection<ErpRelatedProduct>> GetRelatedProducts(long recordId, long channelID, long catalogId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ICollection<ErpRelatedProduct> relatedProductsToReturn = new List<ErpRelatedProduct>();
            PagedResult<ProductRelationType> productRelationTypes;

            var productManager = RPFactory.GetManager<IProductManager>();

            QueryResultSettings relationTypeQueryResultSettings = new QueryResultSettings();
            relationTypeQueryResultSettings.Paging = new PagingInfo();
            relationTypeQueryResultSettings.Paging.Skip = 0;
            relationTypeQueryResultSettings.Paging.Top = (long?)Convert.ToInt64(configurationHelper.GetSetting(PRODUCT.Retail_Server_Paging));

            do
            {
                productRelationTypes = await GetRelationTypes(recordId, channelID, catalogId, productManager, relationTypeQueryResultSettings);
                relationTypeQueryResultSettings.Paging.Skip += relationTypeQueryResultSettings.Paging.Top;

                foreach (ProductRelationType productRelationType in productRelationTypes)
                {
                    // Process only for specific Product Relation Type
                    if (productRelationType.Name.Equals(configurationHelper.GetSetting(PRODUCT.Attr_Flat_Hierarchy_Related))) //productRelationType.Name.Equals("Kit Child Item")
                    {
                        PagedResult<ProductSearchResult> relatedProduct = null;

                        QueryResultSettings relatedProductQueryResultSettings = new QueryResultSettings();
                        relatedProductQueryResultSettings.Paging = new PagingInfo();
                        relatedProductQueryResultSettings.Paging.Skip = 0;
                        relatedProductQueryResultSettings.Paging.Top = (long?)Convert.ToInt64(configurationHelper.GetSetting(PRODUCT.Retail_Server_Paging));

                        do
                        {
                            relatedProduct = await GetRelatedProducts(recordId, channelID, catalogId, productManager, productRelationType, relatedProductQueryResultSettings);

                            if (relatedProduct.Count() == 0)
                            {
                                // REMOVE THIS LINE // throw new NullReferenceException("Found 0 Related Products ,Exception CRT Products");
                                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name
                                , CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40209));
                                throw new NullReferenceException(message);
                            }

                            relatedProductQueryResultSettings.Paging.Skip += relatedProductQueryResultSettings.Paging.Top;

                            foreach (ProductSearchResult relatedProd in relatedProduct)
                            {
                                ErpRelatedProduct objERPRelatedProduct = new ErpRelatedProduct();
                                objERPRelatedProduct.CatalogId = catalogId;
                                objERPRelatedProduct.EntityName = relatedProd.Name;
                                //There was no Item ID in ErpRelatedProduct, Thats why assigning item id to Item
                                objERPRelatedProduct.Item = relatedProd.ItemId;
                                objERPRelatedProduct.ProductRecordId = recordId;
                                objERPRelatedProduct.RelatedProductRecordId = relatedProd.RecordId;
                                objERPRelatedProduct.RelationName = productRelationType.Name;

                                bool alreadyExists = relatedProductsToReturn.Any(r => r.RelatedProductRecordId == relatedProd.RecordId);
                                if (!alreadyExists)
                                {
                                    relatedProductsToReturn.Add(objERPRelatedProduct);
                                }
                            }
                        }
                        while (relatedProduct.Count() == relatedProductQueryResultSettings.Paging.Top);
                    }
                }
            }
            while (productRelationTypes.Count() == relationTypeQueryResultSettings.Paging.Top);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return relatedProductsToReturn.Distinct().ToList();
        }

        private void GetCustomAttributes(ErpProduct objERPProduct, PagedResult<AttributeValue> prAttributeValues)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            // Fetch Not(Indexed Product Properties) Custom Attributes
            if (objERPProduct.CustomAttributes == null && prAttributeValues.Count() > 0)
            {
                objERPProduct.CustomAttributes = new List<KeyValuePair<string, string>>();
            }
            foreach (AttributeValue objAttributeValue in prAttributeValues)
            {
                if (objAttributeValue.DataTypeValue.Equals(6))
                {
                    objERPProduct.CustomAttributes.Add(new KeyValuePair<string, string>(objAttributeValue.Name, Convert.ToString(objAttributeValue.BooleanValue)));
                }

                else if (objAttributeValue.DataTypeValue.Equals(5))
                {
                    objERPProduct.CustomAttributes.Add(new KeyValuePair<string, string>(objAttributeValue.Name, objAttributeValue.TextValue));

                }
                else if (objAttributeValue.DataTypeValue.Equals(3))
                {
                    objERPProduct.CustomAttributes.Add(new KeyValuePair<string, string>(objAttributeValue.Name, Convert.ToString(objAttributeValue.FloatValue)));

                }
                else if (objAttributeValue.DataTypeValue.Equals(4))
                {
                    objERPProduct.CustomAttributes.Add(new KeyValuePair<string, string>(objAttributeValue.Name, Convert.ToString(objAttributeValue.IntegerValue)));

                }
                else if (objAttributeValue.DataTypeValue.Equals(2))
                {
                    objERPProduct.CustomAttributes.Add(new KeyValuePair<string, string>(objAttributeValue.Name, Convert.ToString(objAttributeValue.DateTimeOffsetValue)));

                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

        }
        private void ProcessRelatedProducts(ErpProduct erpProduct, long lngChannelID, long catalogID)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ICollection<ErpRelatedProduct> relatedProducts;
            relatedProducts = GetRelatedProducts(erpProduct.RecordId, lngChannelID, catalogID).Result;
            if (erpProduct.RelatedProducts == null && relatedProducts.Count > 0)
            {
                erpProduct.RelatedProducts = new List<ErpRelatedProduct>();
                //Comment Kit Implementation by AF
                //var kitItem=relatedProducts.FirstOrDefault(d => d.RelationName == "Kit Child Item");
                //if(kitItem!=null)
                //{
                //    objERPProduct.IsKit = true;
                //}
                //End Comment Kit Implementation by AF
            }
            erpProduct.RelatedProducts = relatedProducts;

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

        }
        private void ProcessCustomAttribute(EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager, ErpProduct erpProduct, long lngChannelID, long catalogID, QueryResultSettings objQueryResultSettings)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            StringBuilder productsWithCustomAttributes = new StringBuilder();
            StringBuilder productsWithoutCustomAttributes = new StringBuilder();
            //++RSCall

            var prAttributeValues = ECL_GetAttributeValues(productManager, erpProduct, lngChannelID, catalogID, objQueryResultSettings);

            if (prAttributeValues.Count() > 0)
            {
                productsWithCustomAttributes.Append(
                    string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10217),
                    erpProduct.ItemId, prAttributeValues.Count().ToString() + Environment.NewLine));

                GetCustomAttributes(erpProduct, prAttributeValues);
            }
            else
            {
                productsWithoutCustomAttributes.Append(string.Format(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10218), erpProduct.ItemId) + Environment.NewLine));
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }
        private void ProcessProductVariants(Product product, ErpProduct erpProduct, ObservableCollection<long> searchIds)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            erpProduct.Variants = erpProduct.ProductVariants = new List<ErpProductVariant>();

            if (product.CompositionInformation != null)
            {
                if (product.CompositionInformation.VariantInformation != null)
                {
                    if (product.CompositionInformation.VariantInformation.Variants != null)
                    {
                        ObservableCollection<ProductVariant> variants = product.CompositionInformation.VariantInformation.Variants;
                        //++ US filtering the varients with catalog varients
                        //var productVarients = variants;
                        var productVarients = variants.Where(t => searchIds.Contains((long)t.DistinctProductVariantId)).ToList();
                        foreach (ProductVariant variant in productVarients)
                        {
                            ErpProductVariant var = _mapper.Map<ErpProductVariant>(variant);
                            erpProduct.Variants.Add(var);
                        }
                    }
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }
        private void ProcessProductDimensions(EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager, Product product, ErpProduct erpProduct, long lngChannelID, long catalogID)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            long? retailServerPagingTop = (long?)Convert.ToInt64(configurationHelper.GetSetting(PRODUCT.Retail_Server_Paging));
            bool includeConfig = Convert.ToBoolean(configurationHelper.GetSetting(PRODUCT.IncludeConfigurationDimension));

            erpProduct.DimensionSets = new List<ErpProductDimensionSet>();

            QueryResultSettings productDimensionQuerySetting = new QueryResultSettings();
            productDimensionQuerySetting.Paging = new PagingInfo();
            productDimensionQuerySetting.Paging.Skip = 0;
            productDimensionQuerySetting.Paging.Top = retailServerPagingTop;

            // SK - As There are 4 Dimension Set Types
            for (int i = 1; i <= 4; i++)
            {
                string key = ((ProductDimensionType)i).ToString();
                if (key.Equals("Configuration") && includeConfig == false)
                {
                    continue;
                }
                //++RSCall
                var dimensionValuePage = ECL_GetDimensionValues(productManager, product, lngChannelID, i, productDimensionQuerySetting);
                if (dimensionValuePage != null)
                {
                    ErpProductDimensionSet pDset = new ErpProductDimensionSet();
                    pDset.DimensionValues = new List<ErpProductDimensionValueSet>();
                    pDset.DimensionKey = key;

                    foreach (var setValue in dimensionValuePage.Results)
                    {
                        string value = setValue.Value;
                        long recordId = setValue.RecordId;
                        ProductDimension vSet = new ProductDimension();
                        vSet.DimensionTypeValue = i;
                        vSet.DimensionValue = new ProductDimensionValue();
                        vSet.DimensionValue.RecordId = setValue.RecordId;
                        vSet.DimensionValue.Value = value;

                        List<ProductDimension> dimension = new List<ProductDimension>();
                        dimension.Add(vSet);

                        ErpProductDimensionValueSet erpDimensionValue = new ErpProductDimensionValueSet();
                        erpDimensionValue.DimensionValue = value;
                        erpDimensionValue.DimensionKey = key;
                        pDset.DimensionValues.Add(erpDimensionValue);
                    }
                    if (dimensionValuePage.Results.Count() > 0)
                    {
                        erpProduct.DimensionSets.Add(pDset);
                    }
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

        }
        private void ProcessProductDetails(EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager, ErpProduct erpProduct, Product product, long lngChannelID, long catalogId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            long? retailServerPagingTop = (long?)Convert.ToInt64(configurationHelper.GetSetting(PRODUCT.Retail_Server_Paging));

            List<long> productLst = new List<long>();
            productLst.Add(product.RecordId);

            QueryResultSettings productDimensionQuerySetting = new QueryResultSettings();
            productDimensionQuerySetting.Paging = new PagingInfo();
            productDimensionQuerySetting.Paging.Skip = 0;
            productDimensionQuerySetting.Paging.Top = retailServerPagingTop;
            //++RSCall
            var productDetail = ECL_GetByIds(productManager, lngChannelID, productLst, productDimensionQuerySetting);

            erpProduct.ProductName = productDetail.Name;
            erpProduct.Description = productDetail.Description;

            QueryResultSettings productImageSetting = new QueryResultSettings();
            productImageSetting.Paging = new PagingInfo();
            productImageSetting.Paging.Skip = 0;
            productImageSetting.Paging.Top = retailServerPagingTop;
            //++RSCall
            var productImages = ECL_GetMediaLocations(productManager, product, lngChannelID, catalogId, productImageSetting);
            if (productImages.Count() > 0)
            {
                erpProduct.Image = new ErpRichMediaLocations();
                erpProduct.Image.Items = new ErpRichMediaLocationsRichMediaLocation[productImages.Count()];
                int index = 0;
                foreach (var media in productImages.Results)
                {
                    erpProduct.Image.Items[index] = new ErpRichMediaLocationsRichMediaLocation();
                    erpProduct.Image.Items[index].Url = productImages.Results.ElementAt(index).Uri;
                    erpProduct.Image.Items[index].AltText = productImages.Results.ElementAt(index).AltText;
                    index++;
                }
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

        }
        //public IEnumerable<ErpProductCustomFields> GetProductCustomFields(List<long> productIds)
        //{
        //    List<ErpProductCustomFields> erpProductCustomFields = null;

        //    //++ initializing paramerers
        //    IEnumerable<long> productIdsList = productIds;
        //    long channelId = baseChannelId;
        //    string company = baseCompany;
        //    var rsResponse = ECL_TV_GetProductCustomField(company, channelId, productIdsList);

        //    if (rsResponse.Success)
        //    {
        //        var rsResult = JsonConvert.DeserializeObject<List<EdgeAXCommerceLink.RetailProxy.Extensions.ProductCustomField>>(rsResponse.Result);
        //        erpProductCustomFields =
        //            _mapper
        //                .Map<List<EdgeAXCommerceLink.RetailProxy.Extensions.ProductCustomField>,
        //                    List<ErpProductCustomFields>>(rsResult) ?? new List<ErpProductCustomFields>();
        //    }
        //    else
        //    {
        //        string message = CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30000, currentStore, MethodBase.GetCurrentMethod().Name, rsResponse.Message);
        //    }

        //    return erpProductCustomFields;
        //}

        ///// <summary>
        ///// GetProductUpsell get upsell items from Retail Server.
        ///// </summary>        /// <param name="itemIds"></param>
        ///// <returns></returns>
        //public IEnumerable<ErpUpsellItem> GetProductUpsell(List<string> itemIds)
        //{
        //    List<ErpUpsellItem> erpUpsellItems = null;
        //    var rsResponse = ECL_GetUpsell(itemIds);
        //    if (rsResponse.Success)
        //    {
        //        var upsellItems = JsonConvert.DeserializeObject<List<EdgeAXCommerceLink.RetailProxy.Extensions.UpsellItem>>(rsResponse.Result);
        //        erpUpsellItems =
        //            _mapper.Map<List<EdgeAXCommerceLink.RetailProxy.Extensions.UpsellItem>, List<ErpUpsellItem>>(
        //                upsellItems) ?? new List<ErpUpsellItem>();
        //    }
        //    else
        //    {
        //        string message = CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30000, currentStore, MethodBase.GetCurrentMethod().Name, rsResponse.Message);
        //    }
        //    return erpUpsellItems;
        //}

        ///// <summary>
        ///// Gets RetailInventItemSalesSetup from Retail Server
        ///// </summary>
        ///// <param name="dataAreaId"></param>
        ///// <param name="products"></param>
        ///// <returns></returns>
        //public IEnumerable<ErpRetailInventItemSalesSetup> GetRetailInventItemSalesSetup(List<long> products)
        //{
        //    List<ErpRetailInventItemSalesSetup> erpRetailInventItemSalesSetup = null;
        //    string strProducts = String.Join(",", products);
        //    var rsResponse = ECL_RetailInventItemSalesSetup(strProducts);
        //    if (rsResponse.Success)
        //    {
        //        var retailInventItemSalesSetups = JsonConvert.DeserializeObject<List<EdgeAXCommerceLink.RetailProxy.Extensions.RetailInventItemSalesSetup>>(rsResponse.Result);
        //        erpRetailInventItemSalesSetup =
        //            _mapper
        //                .Map<List<EdgeAXCommerceLink.RetailProxy.Extensions.RetailInventItemSalesSetup>,
        //                    List<ErpRetailInventItemSalesSetup>>(retailInventItemSalesSetups) ??
        //            new List<ErpRetailInventItemSalesSetup>();
        //    }
        //    else
        //    {
        //        string message = CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30000, currentStore, MethodBase.GetCurrentMethod().Name, rsResponse.Message);
        //    }
        //    return erpRetailInventItemSalesSetup;
        //}
        public ProductImageUrlResponse GetProductImageUrl(ProductImageUrlRequest productImageUrlRequest)
        {
            ProductImageUrlResponse productImageUrlResponse = new ProductImageUrlResponse(false, "");
            string productImageUrlXmlRequest;
            LoggingDAL loggingDAL = new LoggingDAL(currentStore.StoreKey);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProductImageUrlRequest));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, productImageUrlRequest);
                productImageUrlXmlRequest = textWriter.ToString();
            }
            RequestResponse requestResponse = new RequestResponse()
            {
                ApplicationName = string.Empty,
                CreatedOn = DateTime.UtcNow,
                DataDirectionId = 1,
                DataPacket = productImageUrlXmlRequest,
                IsSuccess = true,
                MethodName = "ECL_TV_GetProductImageUrl",
            };
            try
            {
                var rsResponse = ECL_TV_GetProductImageUrl(productImageUrlXmlRequest);
                if ((bool)rsResponse.Status)
                {
                    var productImageUrlXmlResponse = rsResponse.Result;

                    XmlSerializer serializer = new XmlSerializer(typeof(ProductImageUrlResponse));
                    using (TextReader reader = new StringReader(productImageUrlXmlResponse))
                    {
                        productImageUrlResponse = (ProductImageUrlResponse)serializer.Deserialize(reader);
                    }

                    requestResponse.IsSuccess = true;
                }
                else
                {
                    requestResponse.IsSuccess = false;
                    string message = CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30000, currentStore, MethodBase.GetCurrentMethod().Name, rsResponse.Message);
                    productImageUrlResponse.Message = message;
                }

                requestResponse.OutputSentAt = DateTime.UtcNow;
                requestResponse.OutputPacket = productImageUrlResponse != null ? productImageUrlResponse.SerializeToJson() : string.Empty;
                //CommerceLinkLogger.LogException(currentStore, new Exception(
                //    $"[Request:{productImageUrlXmlRequest}] [Response:{productImageUrlResponse.SerializeToJson()}]"
                //), "GetProductImageUrl", "ProductImageURLRequestResponse");
                loggingDAL.LogRequestResponse(requestResponse);
            }
            catch (Exception e)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL30000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(e));
                productImageUrlResponse.Message = message;
                requestResponse.OutputPacket = productImageUrlResponse.SerializeToJson();
                requestResponse.OutputSentAt = DateTime.UtcNow;
                requestResponse.IsSuccess = false;

                CommerceLinkLogger.LogException(currentStore, new Exception(
                    $"[Request:{productImageUrlXmlRequest}] [Response:{productImageUrlResponse.SerializeToJson()}]"
                ), "GetProductImageUrl", "ProductImageURLRequestResponse");

                loggingDAL.LogRequestResponse(requestResponse);
            }
            return productImageUrlResponse;
        }
        private async Task<List<KeyValuePair<long, IEnumerable<ErpProduct>>>> GetCatalogProductsAsync(long channelId)
        {

            List<KeyValuePair<long, IEnumerable<ErpProduct>>> allERPProducts = new List<KeyValuePair<long, IEnumerable<ErpProduct>>>();
            EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager>();

            var AvaTaxCodes = new List<AvaTax>();
            if (Convert.ToBoolean(configurationHelper.GetSetting(PRODUCT.IsAvaTaxEnabled)))
            {
                var avaTaxCodeResult = ECL_TV_GetAvaTaxCode(productManager);
                AvaTaxCodes = JsonConvert.DeserializeObject<List<AvaTax>>(avaTaxCodeResult.Result);
            }

            //++ POC Usman 
            string startingSyncToken = string.Empty;
            // string isdeltaEnabled = "true";
            long? fetchProductPagingTop = (long?)Convert.ToInt64(configurationHelper.GetSetting(PRODUCT.Catalog_Retail_Server_Paging));
            long? retailServerPagingTop = (long?)Convert.ToInt64(configurationHelper.GetSetting(PRODUCT.Retail_Server_Paging));
            var productLists = new List<KeyValuePair<long, IEnumerable<Product>>>();
            var productCatalogManager = RPFactory.GetManager<IProductCatalogManager>();

            QueryResultSettings catalogQuerySetting = new QueryResultSettings();
            catalogQuerySetting.Paging = new PagingInfo();
            catalogQuerySetting.Paging.Skip = 0;
            catalogQuerySetting.Paging.Top = retailServerPagingTop;

            PagedResult<ProductCatalog> productCatalogs = productCatalogManager.GetCatalogs(channelId, true, catalogQuerySetting).Result;

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10220, currentStore, productCatalogs.Count());

            if (productCatalogs.Count() > 0)
            {
                foreach (ProductCatalog productCatalog in productCatalogs)
                {
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10221, currentStore, productCatalog.Name);

                    //startingSyncToken = productManager.ECL_GetAnchcorSet(isdeltaEnabled).Result;
                    // set the query criteria in the channel state

                    long catalogId = productCatalog.RecordId;
                    ObservableCollection<long> searchIds = new ObservableCollection<long>();

                    var changedlistingsearchcriteria = new ChangedProductsSearchCriteria();
                    changedlistingsearchcriteria.AsListings = false;
                    changedlistingsearchcriteria.Context = new ProjectionDomain();
                    changedlistingsearchcriteria.Context.ChannelId = channelId;
                    changedlistingsearchcriteria.Context.CatalogId = catalogId;
                    changedlistingsearchcriteria.DataLevelValue = (int)CommerceEntityDataLevel.Complete;
                    //changedlistingsearchcriteria.SynchronizationToken = startingSyncToken;

                    QueryResultSettings productQueryResultSetting = new QueryResultSettings();
                    productQueryResultSetting.Paging = new PagingInfo();
                    productQueryResultSetting.Paging.Skip = 0;
                    productQueryResultSetting.Paging.Top = fetchProductPagingTop;

                    IList<Product> allProducts = new List<Product>();

                    if (bool.Parse(configurationHelper.GetSetting(PRODUCT.Read_Changed_Enabled)))
                    {
                        List<Product> allProductList =
                            GetReadChangedProducts(changedlistingsearchcriteria, productQueryResultSetting);
                        allProducts = allProductList;
                    }
                    else
                    {
                        var eclChangeProducts = ECL_Changes(productManager, changedlistingsearchcriteria, productQueryResultSetting);
                        allProducts = eclChangeProducts.Results.ToList();
                    }
                    //Remove Multiple Records
                    List<Product> uniqueProducts = new List<Product>();
                    List<string> uniqueItemIds = allProducts.Select(x => x.ItemId).Distinct().ToList();
                    foreach (string itemId in uniqueItemIds)
                    {
                        List<Product> multipleProducts = allProducts.Where(x => x.ItemId == itemId).ToList();
                        Product product = allProducts.FirstOrDefault(x => x.ItemId == itemId);
                        uniqueProducts.Add(product);
                    }

                    allProducts = uniqueProducts;

                    #region Temp Code
                    //#if (DEBUG)
                    //var allProductsJson = allProducts.SerializeToJson();

                    //if (allProducts == null || !allProducts.Results.Any())
                    //{
                    //    allProductsJson = System.IO.File.ReadAllText(@"D:\EdgeAXCommerceLink\ECL_Products_20200718115756.json");
                    //    allProducts = Newtonsoft.Json.JsonConvert.DeserializeObject<PagedResult<Product>>(allProductsJson);
                    //}
                    //#endif
                    #endregion

                    // Fetch Images
                    List<ProductImage> productImages = new List<ProductImage>();

                    if (Convert.ToBoolean(configurationHelper.GetSetting(PRODUCT.Single_Consolidated_Catalog)))
                    {
                        try
                        {
                            productImages = GetProductImageUrl(new ProductImageUrlRequest
                            {
                                Products = allProducts.Where(a => a.IsMasterProduct).Select(a => a.ProductNumber).ToList()
                            })?.Products.Where(a => !string.IsNullOrEmpty(a.ImageUrl)).ToList();
                        }
                        catch (Exception ex)
                        {
                            CommerceLinkLogger.LogException(currentStore, ex, "GetProductImageUrl",
                                Guid.NewGuid().ToString());
                        }
                    }

                    List<ErpProduct> erpProductList = new List<ErpProduct>();
                    //Converting to ERP products. 
                    foreach (Product product in allProducts)
                    {
                        ErpProduct erpProduct = _mapper.Map<ErpProduct>(product);
                        erpProduct.CatalogId = catalogId;
                        erpProduct.Mode = ErpChangeMode.Insert;
                        erpProduct.MasterProductNumber = product.ItemId;

                        if (product.IsMasterProduct)
                        {
                            var productImage = productImages?.FirstOrDefault(x => x.Id == product.ProductNumber);
                            if (productImage != null)
                            {
                                erpProduct.ProductImageUrl = productImage.ImageUrl;
                            }

                            if (Convert.ToBoolean(configurationHelper.GetSetting(PRODUCT.IsAvaTaxEnabled)))
                            {
                                erpProduct.AvaTax_TaxClassId = AvaTaxCodes.FirstOrDefault(t => t.ItemId == erpProduct.ItemId)?.TaxabilityCode;
                            }
                        }

                        erpProduct.ProductDetailTranslationsDictionary = processProductProperties(product);
                        ProcessCustomAttribute(productManager, erpProduct, channelId, catalogId, catalogQuerySetting);
                        erpProduct.ChannelId = channelId;
                        if (product.IsMasterProduct)
                        {
                            ProcessProductVariantsandDimentions(productManager, product, erpProduct, channelId, catalogId);
                        }

                        erpProductList.Add(erpProduct);

                    } // foreach Product 

                    allERPProducts.Add(new KeyValuePair<long, IEnumerable<ErpProduct>>(catalogId, erpProductList));
                }
            }
            return allERPProducts;
        }

        public List<Product> GetReadChangedProducts(ChangedProductsSearchCriteria changedlistingsearchcriteria,
            QueryResultSettings productQueryResultSetting)
        {
            var pagedResults = new PagedResult<Product>();

            var allProductList = new List<Product>();
            var productManager = RPFactory.GetManager<IProductManager>();
            var session = productManager.BeginReadChangedProducts(changedlistingsearchcriteria).Result;
            changedlistingsearchcriteria.Session = session;

            for (int pageIndex = 0; (pageIndex * productQueryResultSetting.Paging.Top) < session.TotalNumberOfProducts; pageIndex++)
            {
                productQueryResultSetting.Paging.Skip = pageIndex * productQueryResultSetting.Paging.Top;
                pagedResults = productManager.ReadChangedProducts(changedlistingsearchcriteria, productQueryResultSetting).Result;
                foreach (var p in pagedResults)
                {
                    allProductList.Add(p);
                }
            }

            productManager.EndReadChangedProducts(session);
            return allProductList;
        }

        public void ProcessCustomAttributeForVariant(List<ErpProduct> lstErpProducts)
        {
            try
            {
                EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager>();
                long? retailServerPagingTop = (long?)Convert.ToInt64(configurationHelper.GetSetting(PRODUCT.Retail_Server_Paging));
                QueryResultSettings catalogQuerySetting = new QueryResultSettings();
                catalogQuerySetting.Paging = new PagingInfo();
                catalogQuerySetting.Paging.Skip = 0;
                catalogQuerySetting.Paging.Top = retailServerPagingTop;
                //Comma separated attribute keys to keep
                string attributesToKeep = configurationHelper.GetSetting(PRODUCT.Variant_Attributes_To_Keep);
                var lstAttributesToKeep = attributesToKeep.Split(',').ToList();

                foreach (var erpProduct in lstErpProducts)
                {
                    if (erpProduct.CustomAttributes != null)
                    {
                        var erpProdCopy = _mapper.Map<ErpProduct>(erpProduct);
                        //Clear existing attributes to avoid duplicates.
                        erpProdCopy.CustomAttributes.Clear();
                        //Adds custom attributes from RTS call to ErpProdCopy object
                        ProcessCustomAttribute(productManager, erpProdCopy, erpProduct.ChannelId, erpProduct.CatalogId, catalogQuerySetting);
                        var removeAttributes = erpProduct.CustomAttributes.Where(a => lstAttributesToKeep.Contains(a.Key))
                            .ToList();
                        //Remove attributes that needs to replaced
                        removeAttributes.ForEach(a => erpProduct.CustomAttributes.Remove(a));

                        var attributesReplacement = erpProdCopy.CustomAttributes
                            .Where(a => lstAttributesToKeep.Contains(a.Key)).ToList();

                        erpProduct.CustomAttributes.AddRange(attributesReplacement);
                    }
                }

            }
            catch (Exception e)
            {
                CommerceLinkLogger.LogException(currentStore, e, MethodBase.GetCurrentMethod().Name, string.Empty);
                throw;
            }
        }

        private void ProcessProductVariantsandDimentions(EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager, Product product, ErpProduct erpProduct, long channelId, long catalogId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(product));

            erpProduct.Variants = erpProduct.ProductVariants = new List<ErpProductVariant>();

            if (product.CompositionInformation != null)
            {
                if (product.CompositionInformation.VariantInformation != null)
                {
                    if (product.CompositionInformation.VariantInformation.Variants != null)
                    {
                        ObservableCollection<ProductVariant> variants = product.CompositionInformation.VariantInformation.Variants;
                        //++ US filtering the varients with catalog varients

                        if (variants != null && variants.Count > 0)
                        {
                            //ObservableCollection<ProductExistenceId> productExistenceIds = new ObservableCollection<ProductExistenceId>();
                            // variants.Select(m => m.DistinctProductVariantId).ToList().ForEach(p => { searchIds.Add((long)p); });

                            // ObservableCollection<ProductExistenceId> productExistenceIds = variants.Select(p => new ProductExistenceId { ProductId = p.DistinctProductVariantId });
                            // variants.Select(m => m.DistinctProductVariantId).ToList().ForEach(p => { productExistenceIds.Add(new ProductExistenceId { ProductId = p.Value }); });


                            //var productExistenceCriteria = new ProductExistenceCriteria();
                            //productExistenceCriteria.ChannelId = channelId;
                            //productExistenceCriteria.CatalogId = catalogId;
                            //productExistenceCriteria.Ids = productExistenceIds;

                            QueryResultSettings verifyproductQuerySetting = new QueryResultSettings();
                            verifyproductQuerySetting.Paging = new PagingInfo();
                            verifyproductQuerySetting.Paging.Skip = 0;
                            verifyproductQuerySetting.Paging.Top = (long?)Convert.ToInt64(configurationHelper.GetSetting(PRODUCT.Retail_Server_Paging));


                            //var verfiedProducts = productManager.ECL_GetVerifiedPorducts(productExistenceCriteria, verifyproductQuerySetting).Result;

                            PagedResult<ProductSearchResult> searchProductResult = null;

                            foreach (var item in product.CategoryIds)
                            {
                                searchProductResult = ECL_SearchUsingCategory(productManager, channelId, catalogId, item, verifyproductQuerySetting);
                            }

                            if (searchProductResult != null)
                            {
                                ObservableCollection<long> searchIds = new ObservableCollection<long>();
                                searchProductResult.Select(m => m.RecordId).ToList().ForEach(p => { searchIds.Add(p); });

                                //var productVarients = variants;
                                var productVarients = variants.Where(t => searchIds.Contains((long)t.DistinctProductVariantId)).ToList();
                                foreach (ProductVariant variant in productVarients)
                                {
                                    ErpProductVariant var = _mapper.Map<ErpProductVariant>(variant);
                                    erpProduct.Variants.Add(var);
                                }
                            }
                            else
                            {
                                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10219, currentStore, product.ItemId, product.RecordId, MethodBase.GetCurrentMethod().Name);
                            }
                        }
                    }
                }

                if (product.CompositionInformation.VariantInformation.Dimensions != null)
                {
                    erpProduct.DimensionSets = _mapper.Map<ICollection<ProductDimensionSet>, ICollection<ErpProductDimensionSet>>(product.CompositionInformation.VariantInformation.Dimensions).ToList();
                }
            }
        }

        private Dictionary<String, Dictionary<String, String>> processProductProperties(Product product)
        {
            Dictionary<String, Dictionary<String, String>> productDetailTranslationsDictionary = new Dictionary<String, Dictionary<String, String>>();

            if (product != null && product.ProductProperties != null)
            {
                for (int i = 0; i < product.ProductProperties.Count; i++)
                {
                    Dictionary<String, String> individualLanguageDetailDictionary = new Dictionary<String, String>();

                    string strTranslationLanguage = product.ProductProperties[i].TranslationLanguage;

                    for (int iTranslatedPropertiesCount = 0; iTranslatedPropertiesCount < product.ProductProperties[i].TranslatedProperties.Count; iTranslatedPropertiesCount++)
                    {
                        String strKeyName = product.ProductProperties[i].TranslatedProperties[iTranslatedPropertiesCount].KeyName;
                        String strValue = product.ProductProperties[i].TranslatedProperties[iTranslatedPropertiesCount].ValueString;

                        individualLanguageDetailDictionary.Add(strKeyName, strValue);
                    }

                    productDetailTranslationsDictionary.Add(strTranslationLanguage, individualLanguageDetailDictionary);
                }
            }

            return productDetailTranslationsDictionary;
        }

        #endregion

        #region RetailServer API
        [Trace]
        private PagedResult<ProductSearchResult> ECL_SearchUsingCategory(EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager, long channelId, long catalogID,
    long categoryID, QueryResultSettings productQueryResultSettings)
        {
            PagedResult<ProductSearchResult> searchProductResult;
            searchProductResult = productManager
                .ECL_SearchUsingCategory(channelId, catalogID, categoryID, productQueryResultSettings).Result;
            return searchProductResult;
        }
        [Trace]
        private PagedResult<ProductCatalog> GetCatalogs(long channelId, QueryResultSettings catalogQuerySetting)
        {
            var productCatalogManager = RPFactory.GetManager<IProductCatalogManager>();
            PagedResult<ProductCatalog> productCatalogs =
                productCatalogManager.GetCatalogs(channelId, true, catalogQuerySetting).Result;
            return productCatalogs;
        }
        [Trace]
        private GetAvaTaxCodeResponse ECL_TV_GetAvaTaxCode(EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager)
        {
            var avaTaxCodeResult = productManager.ECL_TV_GetAvaTaxCode(baseCompany).Result;
            return avaTaxCodeResult;
        }
        [Trace]
        private PagedResult<Product> ECL_SearchProduct(EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager, ProductSearchCriteria productCriteria,
            QueryResultSettings masterProductQuerySetting)
        {
            PagedResult<Product> masterProductsResult;
            masterProductsResult = productManager.ECL_SearchProduct(productCriteria, masterProductQuerySetting).Result;
            return masterProductsResult;
        }
        [Trace]
        private PagedResult<Category> GetCategories(long channelId, QueryResultSettings catalogQuerySetting)
        {
            var categoryManager = RPFactory.GetManager<ICategoryManager>();
            PagedResult<Category> prCategories = categoryManager.GetCategories(channelId,null, catalogQuerySetting).Result;
            return prCategories;
        }
        [Trace]
        private async Task<PagedResult<ProductSearchResult>> GetRelatedProducts(long recordId, long channelID, long catalogId,
            IProductManager productManager, ProductRelationType productRelationType,
            QueryResultSettings relatedProductQueryResultSettings)
        {
            throw new NotImplementedException();
            //PagedResult<ProductSearchResult> relatedProduct;
            //relatedProduct = await productManager.GetRelatedProducts(recordId, channelID, catalogId,
            //    (long)productRelationType.RecordId, relatedProductQueryResultSettings);
            //return relatedProduct;
        }
        [Trace]
        private async Task<PagedResult<ProductRelationType>> GetRelationTypes(long recordId, long channelID, long catalogId,
            IProductManager productManager, QueryResultSettings relationTypeQueryResultSettings)
        {
            PagedResult<ProductRelationType> productRelationTypes;
            productRelationTypes =
                await productManager.GetRelationTypes(recordId, channelID, catalogId, relationTypeQueryResultSettings);
            return productRelationTypes;
        }
        [Trace]
        private PagedResult<AttributeValue> ECL_GetAttributeValues(EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager, ErpProduct erpProduct,
            long lngChannelID, long catalogID, QueryResultSettings objQueryResultSettings)
        {
            PagedResult<AttributeValue> prAttributeValues = productManager
                .ECL_GetAttributeValues(erpProduct.RecordId, lngChannelID, catalogID, objQueryResultSettings).Result;
            return prAttributeValues;
        }
        [Trace]
        private PagedResult<ProductDimensionValue> ECL_GetDimensionValues(EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager, Product product, long lngChannelID,
            int i, QueryResultSettings productDimensionQuerySetting)
        {
            PagedResult<ProductDimensionValue> dimensionValuePage = productManager
                .ECL_GetDimensionValues(product.RecordId, lngChannelID, i.ToString(), null, null, productDimensionQuerySetting)
                .Result;
            return dimensionValuePage;
        }
        [Trace]
        private PagedResult<MediaLocation> ECL_GetMediaLocations(EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager, Product product, long lngChannelID,
            long catalogId, QueryResultSettings productImageSetting)
        {
            PagedResult<MediaLocation> productImages = productManager
                .ECL_GetMediaLocations(lngChannelID, catalogId, product.RecordId, productImageSetting).Result;
            return productImages;
        }
        [Trace]
        private SimpleProduct ECL_GetByIds(EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager, long lngChannelID, List<long> productLst,
            QueryResultSettings productDimensionQuerySetting)
        {
            SimpleProduct productDetail = productManager.ECL_GetByIds(productLst, lngChannelID, productDimensionQuerySetting)
                .Result.First();
            return productDetail;
        }
        //[Trace]
        //private ERPProductCustomFieldsResponse ECL_TV_GetProductCustomField(string company, long channelId,
        //    IEnumerable<long> productIdsList)
        //{
        //    throw new NotImplementedException();
        //    //EdgeAXCommerceLink.RetailProxy.Extensions.IProductCustomFieldManager productCustomFields =
        //    //    RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IProductCustomFieldManager>();
        //    //var rsResponse = Task.Run(async () => await productCustomFields.ECL_TV_GetProductCustomField(0, company, channelId,
        //    //    configurationHelper.GetSetting(PRODUCT.OfferTypeGroupName), productIdsList)).Result;
        //    //return rsResponse;
        //}
        //[Trace]
        //private UpsellItemResponse ECL_GetUpsell(List<string> itemIds)
        //{
        //    throw new NotImplementedException();
        //    //EdgeAXCommerceLink.RetailProxy.Extensions.IUpsellItemManager upsellItemManager =
        //    //    RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IUpsellItemManager>();

        //    //var rsResponse = Task.Run(async () =>
        //    //    await upsellItemManager.ECL_GetUpsell(0, baseCompany, configurationHelper.GetSetting(PRODUCT.CrossSellType),
        //    //        itemIds)).Result;
        //    //return rsResponse;
        //}
        //[Trace]
        //private RetailInventItemSalesSetupResponse ECL_RetailInventItemSalesSetup(string strProducts)
        //{
        //    throw new NotImplementedException();
        //    //EdgeAXCommerceLink.RetailProxy.Extensions.IRetailInventItemSalesSetupManager retailInventItemSalesSetupManager =
        //    //    RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IRetailInventItemSalesSetupManager>();

        //    //var rsResponse = Task.Run(async () =>
        //    //    await retailInventItemSalesSetupManager.ECL_RetailInventItemSalesSetup(0, baseCompany, strProducts,
        //    //        baseInventLocation)).Result;
        //    //return rsResponse;
        //}
        [Trace]
        private GetProductImageUrlResponse ECL_TV_GetProductImageUrl(string productImageUrlXmlRequest)
        {
            EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager =
                RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager>();
            var rsResponse = Task.Run(async () =>
                await productManager.ECL_TV_GetProductImageUrl(productImageUrlXmlRequest, baseCompany)).Result;
            return rsResponse;
        }
        [Trace]
        private PagedResult<Product> ECL_Changes(EdgeAXCommerceLink.RetailProxy.Extensions.IProductManager productManager,
            ChangedProductsSearchCriteria changedlistingsearchcriteria, QueryResultSettings productQueryResultSetting)
        {
            PagedResult<Product> eclChangeProducts = Task.Run(async () => await
                 productManager.ECL_Changes(changedlistingsearchcriteria, productQueryResultSetting)).Result;
            return eclChangeProducts;
        }

        public IEnumerable<ErpProductCustomFields> GetProductCustomFields(List<long> productIds)
        {
            List<ErpProductCustomFields> erpProductCustomFields = null;

            //++ initializing paramerers
            IEnumerable<long> productIdsList = productIds;
            long channelId = baseChannelId;
            string company = baseCompany;
            var rsResponse = ECL_TV_GetProductCustomField(company, channelId, productIdsList);

            if ((bool)rsResponse.Status)
            {
                var rsResult = JsonConvert.DeserializeObject<List<EdgeAXCommerceLink.RetailProxy.Extensions.ProductCustomField>>(rsResponse.Result);
                erpProductCustomFields =
                    _mapper
                        .Map<List<EdgeAXCommerceLink.RetailProxy.Extensions.ProductCustomField>,
                            List<ErpProductCustomFields>>(rsResult) ?? new List<ErpProductCustomFields>();
            }
            else
            {
                string message = CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30000, currentStore, MethodBase.GetCurrentMethod().Name, rsResponse.Message);
            }

            return erpProductCustomFields;
        }
        [Trace]
        private GetProductCustomFieldResponse ECL_TV_GetProductCustomField(string company, long channelId, IEnumerable<long> productIdsList)
        {
            EdgeAXCommerceLink.RetailProxy.Extensions.IProductCustomFieldManager productCustomFields =
                RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IProductCustomFieldManager>();
            var rsResponse = Task.Run(async () => await productCustomFields.ECL_TV_GetProductCustomField(company, channelId,
                configurationHelper.GetSetting(PRODUCT.OfferTypeGroupName), productIdsList)).Result;
            return rsResponse;
        }

        /// <summary>
        /// GetProductUpsell get upsell items from Retail Server.
        /// </summary>        /// <param name="itemIds"></param>
        /// <returns></returns>
        public IEnumerable<ErpUpsellItem> GetProductUpsell(List<string> itemIds)
        {
            List<ErpUpsellItem> erpUpsellItems = null;
            var rsResponse = ECL_GetUpsell(itemIds);
            if ((bool)rsResponse.Status)
            {
                var upsellItems = JsonConvert.DeserializeObject<List<EdgeAXCommerceLink.RetailProxy.Extensions.UpsellItem>>(rsResponse.Result);
                erpUpsellItems =
                    _mapper.Map<List<EdgeAXCommerceLink.RetailProxy.Extensions.UpsellItem>, List<ErpUpsellItem>>(
                        upsellItems) ?? new List<ErpUpsellItem>();
            }
            else
            {
                string message = CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30000, currentStore, MethodBase.GetCurrentMethod().Name, rsResponse.Message);
            }
            return erpUpsellItems;
        }
        [Trace]
        private GetUpsellResponse ECL_GetUpsell(List<string> itemIds)
        {
            EdgeAXCommerceLink.RetailProxy.Extensions.IUpsellItemManager upsellItemManager =
                RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IUpsellItemManager>();

            var rsResponse = Task.Run(async () =>
                await upsellItemManager.ECL_GetUpsell(baseCompany, configurationHelper.GetSetting(PRODUCT.CrossSellType),
                    itemIds)).Result;
            return rsResponse;
        }

        public IEnumerable<ErpRetailInventItemSalesSetup> GetRetailInventItemSalesSetup(List<long> productIds)
        {
            List<ErpRetailInventItemSalesSetup> erpRetailInventItemSalesSetup = null;
            string strProducts = String.Join(",", productIds);
            var rsResponse = ECL_RetailInventItemSalesSetup(strProducts);
            if ((bool)rsResponse.Status)
            {
                var retailInventItemSalesSetups = JsonConvert.DeserializeObject<List<EdgeAXCommerceLink.RetailProxy.Extensions.RetailInventItemSalesSetup>>(rsResponse.Result);
                erpRetailInventItemSalesSetup =
                    _mapper
                        .Map<List<EdgeAXCommerceLink.RetailProxy.Extensions.RetailInventItemSalesSetup>,
                            List<ErpRetailInventItemSalesSetup>>(retailInventItemSalesSetups) ??
                    new List<ErpRetailInventItemSalesSetup>();
            }
            else
            {
                string message = CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30000, currentStore, MethodBase.GetCurrentMethod().Name, rsResponse.Message);
            }
            return erpRetailInventItemSalesSetup;
        }
        [Trace]
        private GetRetailInventItemSalesSetupResponse ECL_RetailInventItemSalesSetup(string strProducts)
        {
            EdgeAXCommerceLink.RetailProxy.Extensions.IRetailInventItemSalesSetupManager retailInventItemSalesSetupManager =
                RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IRetailInventItemSalesSetupManager>();

            var rsResponse = Task.Run(async () =>
                await retailInventItemSalesSetupManager.ECL_RetailInventItemSalesSetup(baseCompany, strProducts, baseInventLocation)).Result;
            return rsResponse;
        }

        #endregion

    }
}

