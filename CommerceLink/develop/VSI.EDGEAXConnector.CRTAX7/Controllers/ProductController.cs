using AutoMapper;
using EdgeAXCommerceLink.Commerce.RetailProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.CRTAX7.Controllers
{

    public class ProductController : BaseController, IProductController
    {

        #region Public Methods

        public IEnumerable<ErpProductExistenceId> VerifyProductExistence(long channelId, long catalogId, IEnumerable<ErpProductExistenceId> productExistenceIds)
        {
            return null;
        }

        public List<KeyValuePair<long, IEnumerable<ErpProduct>>> GetCatalogProducts(long channelId, bool useDelta, List<ErpCategory> categories, bool fetchAll)
        {
            try
            {
                return AsyncGetCatalogProducts(baseChannelId, fetchAll).Result;
            }
            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);

                throw exp;
            }


        }


        public IEnumerable<ErpProductCustomFields> GetProductCustomFields(IEnumerable<long> productIds)
        {
            //TV related Customization.
            throw new NotImplementedException();
        }
        #endregion

        #region Private Methods

        private async Task<List<KeyValuePair<long, IEnumerable<ErpProduct>>>> AsyncGetCatalogProducts(long channelId, bool fetchAll)
        {
           
            CustomLogger.LogDebugInfo(string.Format("Enter in AsyncGetCatalogProducts()"));

            StringBuilder productsWithCustomAttributes = new StringBuilder();
            StringBuilder productsWithoutCustomAttributes = new StringBuilder();


            long lngChannelID, lngCatalogID, lngCategoryID;
            List<KeyValuePair<long, IEnumerable<ErpProduct>>> kvpERPProducts = new List<KeyValuePair<long, IEnumerable<ErpProduct>>>();

            // Default
            lngChannelID = channelId;

            // Query Result Settings
            QueryResultSettings objQueryResultSettings = new QueryResultSettings();
            objQueryResultSettings.Paging = new PagingInfo();
            objQueryResultSettings.Paging.Skip = 0;
            objQueryResultSettings.Paging.Top = (long?)Convert.ToInt64(ConfigurationHelper.GetSetting(PRODUCT.Retail_Server_Paging));

            // Get the Catalogs
            var pcManager = RPFactory.GetManager<IProductCatalogManager>();
            PagedResult<ProductCatalog> prCatalogs = pcManager.GetCatalogs(lngChannelID, true, objQueryResultSettings).Result;

            if (prCatalogs.Count() > 0)
            {

                foreach (ProductCatalog objProductCatalog in prCatalogs)
                {
                    lngCatalogID = objProductCatalog.RecordId;

                    CustomLogger.LogDebugInfo(string.Format("Get catalog \"{0}\" catalog id = {1} from Retail Server for Channel ID {2}", objProductCatalog.Name, lngCatalogID, channelId));

                    List<ErpProduct> lstERPProd = new List<ErpProduct>();

                    // Get the Catagories
                    var cgManager = RPFactory.GetManager<ICategoryManager>();
                    PagedResult<Category> prCategories = cgManager.GetCategories(lngChannelID, objQueryResultSettings).Result;

                    if (prCategories.Count() > 0)
                    {

                        //Start: Aqeel

                        QueryResultSettings objProductQueryResultSettings = new QueryResultSettings();
                        objProductQueryResultSettings.Paging = new PagingInfo();
                        objProductQueryResultSettings.Paging.Skip = 0;
                        objProductQueryResultSettings.Paging.Top = (long?)Convert.ToInt64(ConfigurationHelper.GetSetting(PRODUCT.Retail_Server_Paging));

                        //End: Aqeel

                        foreach (Category objCategory in prCategories)
                        {

                            lngCategoryID = (long)objCategory.RecordId;

                            //Start: Aqeel
                            //Start the Page Skip by 0
                            objProductQueryResultSettings.Paging.Skip = 0;

                            // Create Product Manager Object
                            var prManager = RPFactory.GetManager<IProductManager>();

                            // PagedResults to 
                            PagedResult<ProductSearchResult> objProducts;

                            // Commenting do while loop because related products are not being fetched 

                            do
                            {

                                // Fetch the products using Product Manager Object
                                objProducts = prManager.SearchByCategory(lngChannelID, lngCatalogID, lngCategoryID, objProductQueryResultSettings).Result;

                                //if (objProducts.Count() == 0)
                                //{


                                //    throw new NullReferenceException("Found 0 Products, Exception CRT Products");

                                //}

                                //if (objProducts == null)
                                //{


                                //    throw new NullReferenceException("Found NULL Products, Exception CRT Products");

                                //}



                                CustomLogger.LogDebugInfo(string.Format("AX Category: {0} have {1} No of Products Setup in that category", objCategory.Name, objProducts.Count().ToString()));

                                //Start: Aqeel
                                //Update the Page Skip ...
                                objProductQueryResultSettings.Paging.Skip += objProductQueryResultSettings.Paging.Top;

                                if (objProducts != null)
                                {
                                    foreach (ProductSearchResult objProductSearchResult in objProducts)
                                    {

                                        // Map the Product Search Result to ErpProduct 
                                        ErpProduct objERPProduct = Mapper.Map<ErpProduct>(objProductSearchResult);


                                        // Fetch "CustomeAttributes" and "RelatedProducts"
                                        if (fetchAll)
                                        {

                                            //Start:AF
                                            PagedResult<AttributeValue> prAttributeValues = await prManager.GetAttributeValues(objERPProduct.RecordId, lngChannelID, lngCatalogID, objQueryResultSettings);

                                            if (prAttributeValues.Count() > 0)
                                            {
                                                productsWithCustomAttributes.Append(string.Format("Product {0} has {1} Custom Attributes", objERPProduct.ItemId, prAttributeValues.Count()) + Environment.NewLine);
                                                GetCustomAttributes(objERPProduct, prAttributeValues);
                                            }
                                            else 
                                            {
                                                productsWithoutCustomAttributes.Append(string.Format("Product {0}, ", objERPProduct.ItemId) + Environment.NewLine);

                                            }
                                         
                                            //To-Do (ErpChangeMode)Enum.Parse(typeof(ErpChangeAction), product.ChangeTrackingInformation.ChangeAction.ToString());

                                            objERPProduct.Mode = ErpChangeMode.Insert;
                                            objERPProduct.CatalogId = lngCatalogID;

                                            // Get related products
                                            ICollection<ErpRelatedProduct> relatedProducts;
                                            relatedProducts = GetRelatedProducts(objERPProduct.RecordId, lngChannelID, lngCatalogID).Result;
                                            if (objERPProduct.RelatedProducts == null && relatedProducts.Count > 0)
                                            {
                                                objERPProduct.RelatedProducts = new List<ErpRelatedProduct>();

                                                //Comment Kit Implementation by AF
                                                //var kitItem=relatedProducts.FirstOrDefault(d => d.RelationName == "Kit Child Item");
                                                //if(kitItem!=null)
                                                //{
                                                //    objERPProduct.IsKit = true;
                                                //}

                                                //End Comment Kit Implementation by AF
                                            }
                                            objERPProduct.RelatedProducts = relatedProducts;

                                        } // if (fetchAll)

                                        // Add the ErpProduct to List
                                        lstERPProd.Add(objERPProduct);  // Moved down after discussion with AF


                                    } // foreach Product 
                                } // if (objProduct != null)
                            } // End of Do-While loop
                            while (objProducts.Count() == objProductQueryResultSettings.Paging.Top);

                        } // foreach Category

                        // Add the List to Key Value Pair List
                        kvpERPProducts.Add(new KeyValuePair<long, IEnumerable<ErpProduct>>(lngCatalogID, lstERPProd)); // objProductSearchResult.RecordId updated to lngCatalogID
                    }
                    else
                    {
                        if (prCategories.Count() == 0)
                        {
                            throw new NullReferenceException("Found 0 Categories, Exception CRT Products");
                        }


                        if (prCategories == null)
                        {
                            throw new NullReferenceException("Found NULL Categories, Exception CRT Products");
                        }


                    }
                }
            }
            else
            {


                if (prCatalogs.Count() == 0)
                {
                    throw new NullReferenceException("Found 0 Catalog, Exception CRT Products");
                }


                if (prCatalogs == null)
                {
                    throw new NullReferenceException("Found NULL Catalog, Exception CRT Products");
                }


            }


            if(productsWithCustomAttributes.Length>0)
            {
                CustomLogger.LogDebugInfo(productsWithCustomAttributes.ToString());
            }
            if(productsWithoutCustomAttributes.Length>0)
            {
                CustomLogger.LogDebugInfo(productsWithoutCustomAttributes.ToString());

            }
           
            CustomLogger.LogDebugInfo(string.Format("Exit from AsyncGetCatalogProducts()"));
            // Return the Key Value Pair List containing the ErpProducts ...

            if (kvpERPProducts.Count == 0)
            {
                throw new NullReferenceException("Found 0 Products, Exception CRT Products");
            }
            else
            {
                return kvpERPProducts;
            }
           // return kvpERPProducts;

        }

        /// <summary>
        /// Asynchronous Get Related Products
        /// </summary>
        /// <param name="recordId">Product Record Id</param>
        /// <param name="channelId">ChannelId</param>
        /// <param name="catalogId">Catalog Id</param>
        /// <returns></returns>
        private async Task<ICollection<ErpRelatedProduct>> GetRelatedProducts(long recordId, long channelId, long catalogId)
        {
            ICollection<ErpRelatedProduct> relatedProductsToReturn = new List<ErpRelatedProduct>();

            var productManager = RPFactory.GetManager<IProductManager>();

            QueryResultSettings relationTypeQueryResultSettings = new QueryResultSettings();
            relationTypeQueryResultSettings.Paging = new PagingInfo();
            relationTypeQueryResultSettings.Paging.Skip = 0;
            relationTypeQueryResultSettings.Paging.Top = (long?)Convert.ToInt64(ConfigurationHelper.GetSetting(PRODUCT.Retail_Server_Paging));

            // Reset the Skip for Relation Type
            relationTypeQueryResultSettings.Paging.Skip = 0;

            PagedResult<ProductRelationType> productRelationTypes;

            do
            {
                productRelationTypes = await productManager.GetRelationTypes(recordId, channelId, catalogId, relationTypeQueryResultSettings);

                relationTypeQueryResultSettings.Paging.Skip += relationTypeQueryResultSettings.Paging.Top;

                foreach (ProductRelationType productRelationType in productRelationTypes)
                {
                    // Process only for specific Product Relation Type
                    if (productRelationType.Name.Equals(ConfigurationHelper.GetSetting(PRODUCT.Attr_Flat_Hierarchy_Related))) //productRelationType.Name.Equals("Kit Child Item")
                    {


                        QueryResultSettings relatedProductQueryResultSettings = new QueryResultSettings();
                        relatedProductQueryResultSettings.Paging = new PagingInfo();
                        relatedProductQueryResultSettings.Paging.Skip = 0;
                        relatedProductQueryResultSettings.Paging.Top = (long?)Convert.ToInt64(ConfigurationHelper.GetSetting(PRODUCT.Retail_Server_Paging));

                        relatedProductQueryResultSettings.Paging.Skip = 0;

                        PagedResult<ProductSearchResult> relatedProduct;

                        do
                        {
                            relatedProduct = await productManager.GetRelatedProducts(
                                recordId, channelId, catalogId, (long)productRelationType.RecordId, relatedProductQueryResultSettings);

                            if (relatedProduct.Count() == 0)
                            {

                                throw new NullReferenceException("Found 0 Related Products ,Exception CRT Products");
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


            return relatedProductsToReturn.Distinct().ToList();
        }

        private void GetCustomAttributes(ErpProduct objERPProduct, PagedResult<AttributeValue> prAttributeValues)
        {
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

        }

        public IEnumerable<ErpUpsellItem> GetProductUpsell(List<string> itemIds)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ErpProductCustomFields> GetProductCustomFields(List<long> productIds)
        {
            throw new NotImplementedException();
        }


        #endregion

    }
}

