//using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using System.Configuration;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    /// <summary>
    /// ProductController class is product controller for AX.
    /// </summary>
    public class ProductController : ProductBaseController, IProductController
    {
        StringBuilder trace;
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProductController(string storeKey) : base(storeKey)
        {
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Get All Products returns All Products using CRT in Connector ERP Data Model
        /// </summary>
        /// <param name="fetchAll"></param>
        /// <param name="categories"></param>
        /// <returns></returns>
        public List<ErpProduct> GetAllProducts(bool useDelta, List<ErpCategory> categories, bool fetchAll)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            List<ErpProduct> erpProducts = new List<ErpProduct>();
            bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();
            var crtProducts = base.GetCatalogProducts(useDelta, categories, fetchAll);
            if (crtProducts != null && crtProducts.Count < 0)
            {
                CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30202, currentStore);
            }
            //TODO: Commented as not required yet this.ProcessProductDeleted();
            erpProducts = base.ProcessProducts(crtProducts);
            erpProducts = this.ProcessDimensionSets(erpProducts);
            this.ProcessProductCustomFields(erpProducts);
            if (erpProducts.Count < 0)
            {
                CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30203, currentStore);
            }
            if (isFlatProductHierarchy)
            {
                if (erpProducts.Count() > 0)
                {
                    erpProducts = this.ProcessCustomMasterProduct(erpProducts);
                }
            }
            if (configurationHelper.GetSetting(PRODUCT.Delete_Disable).ToLower() == "false")
            {
                this.ProcessDeletedProducts(erpProducts);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpProducts;
        }

        /// <summary>
        /// Update the Dimension Set for ECom
        /// </summary>
        /// <param name="erpProducts"></param>
        /// <returns></returns>
        private List<ErpProduct> ProcessDimensionSets(List<ErpProduct> erpProducts)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpProducts));

            if (erpProducts.Count() > 0)
            {
                foreach (ErpProduct erpProduct in erpProducts)
                {
                    if (erpProduct.DimensionSets != null && erpProduct.DimensionSets.Count > 0)
                    {
                        foreach (ErpProductDimensionSet erpProductDimensionSet in erpProduct.DimensionSets)
                        {
                            var variationAttribute = configurationHelper.DimensionSets.Where(d => d.ErpValue.Equals(erpProductDimensionSet.DimensionKey) && d.IsActive == true).FirstOrDefault();
                            if (variationAttribute != null)
                            {
                                erpProductDimensionSet.DimensionKey = variationAttribute.ComValue;
                                foreach (ErpProductDimensionValueSet erpProductDimensionValueSet in erpProductDimensionSet.DimensionValues)
                                {
                                    erpProductDimensionValueSet.DimensionKey = variationAttribute.ComValue;
                                }
                            }
                        }
                    }
                }
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpProducts));
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return erpProducts;
        }

        /// <summary>
        /// ProcessProductCustomFields gets ProductCustomFields and update CustomAttributes.
        /// </summary>
        /// <param name="erpProducts"></param>
        private void ProcessProductCustomFields(List<ErpProduct> erpProducts)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpProducts));

            var crtProductManager = new ProductCRTManager();
            var customFieldsResponse = crtProductManager.GetProductCustomFields(erpProducts.Select(p=>p.RecordId).ToList(),currentStore.StoreKey);
            

            if (customFieldsResponse != null && customFieldsResponse.Count()>0)
            {

                foreach (var customField in customFieldsResponse)
                {
                    var product = erpProducts.FirstOrDefault(prd => prd.RecordId == customField.RECID);
                    if (product != null && product.CustomAttributes==null)
                    {
                        product.CustomAttributes = new List<KeyValuePair<string, string>>();
                    }
                    if (product != null)
                    {
                        product.CustomAttributes.Add(new KeyValuePair<string, string>("TMVProductType", customField.TMVProductType.ToString()));
                        product.CustomAttributes.Add(new KeyValuePair<string, string>("TMVCancellationPeriod", customField.TMVCancellationPeriod.ToString()));
                        product.CustomAttributes.Add(new KeyValuePair<string, string>("TMVTerminationPeriod", customField.TMVTerminationPeriod.ToString()));
                        product.CustomAttributes.Add(new KeyValuePair<string, string>("TMVAutoRenewal", customField.TMVAutoRenewal));
                        product.CustomAttributes.Add(new KeyValuePair<string, string>("ReplenishmentWeight", customField.ReplenishmentWeight.ToString()));
                        product.ReplenishmentWeight = customField.ReplenishmentWeight;
                    }
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpProducts));
        }
        private List<ErpProduct> ProcessCustomMasterProduct(List<ErpProduct> erpProducts)
        {
            trace = new StringBuilder();

            StringBuilder twoDimensionProducts = new StringBuilder();

            // REMOVE THIS LINE // trace.Append(string.Format("Enter in ProcessCustomMasterProduct()") + Environment.NewLine);
            trace.Append(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10000) + Environment.NewLine);
            try
            {
                if (erpProducts.Count > 0 && erpProducts != null)
                {
                    foreach (var erpProduct in erpProducts)
                    {
                        var prod = erpProduct.CustomAttributes.Where(x => x.Key.Equals(configurationHelper.GetSetting(PRODUCT.Attr_IsMaster)) && x.Value.Equals("True")).FirstOrDefault();

                        if (prod.Key != null && prod.Value != null
                                && prod.Key.Equals(configurationHelper.GetSetting(PRODUCT.Attr_IsMaster))
                                && erpProduct.IsMasterProduct == true)
                        {
                            trace.Append(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL30203), erpProduct.ItemId) + Environment.NewLine);

                            erpProduct.DimensionSets = new List<ErpProductDimensionSet>();
                            List<ErpRichMediaLocationsRichMediaLocation> ImageList = new List<ErpRichMediaLocationsRichMediaLocation>();
                            List<ErpProductDimensionSet> erpProductDimSetList = new List<ErpProductDimensionSet>();

                            // REMOVE THIS LINE // trace.Append(string.Format("Going to traverse Custom Attributes which are present in Dimension Set table") + Environment.NewLine);
                            trace.Append(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10202)) + Environment.NewLine);

                            foreach (var proddim in erpProduct.CustomAttributes)
                            {
                                if (!proddim.Key.Equals(configurationHelper.GetSetting(PRODUCT.Attr_IsMaster))) // generate variations attributes other than these attributes
                                {
                                    var variationAttribute = configurationHelper.DimensionSets.Where(d => d.ErpValue.Equals(proddim.Key.ToLower()) && d.IsActive == true).FirstOrDefault();

                                    if (variationAttribute != null)
                                    {
                                        ErpProductDimensionSet erpProductDimSet = new ErpProductDimensionSet();

                                        erpProductDimSet.DimensionKey = variationAttribute.ComValue;

                                        // REMOVE THIS LINE // trace.Append(string.Format("Traversing custom attributes => Dimension Key {0}", proddim.Key) + Environment.NewLine);
                                        trace.Append(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10203), proddim.Key) + Environment.NewLine);

                                        // Now prepare DimensionValues for  DimensionSet 
                                        List<ErpProductDimensionValueSet> dimensionSetValuesList = new List<ErpProductDimensionValueSet>();
                                        ErpProductDimensionValueSet erpDimValueSet = new ErpProductDimensionValueSet();

                                        if (proddim.Value.ToString() != "" && proddim.Value.ToString() != null && proddim.Value.ToString() != "NA" && proddim.Value.ToString() != "-1")
                                        {
                                            if (variationAttribute.ComValue.ToString() == "Size")
                                            {
                                                var QuantityMeasureAsSecondValue = variationAttribute.AdditionalErpValue.ToString().Split(',');
                                                var quantityMeasureAsVariationAttribute = erpProduct.CustomAttributes.Where(x => x.Key.Equals(QuantityMeasureAsSecondValue[0])).FirstOrDefault();
                                                if (quantityMeasureAsVariationAttribute.Value != "NA" && quantityMeasureAsVariationAttribute.Value != "" && quantityMeasureAsVariationAttribute.Value != null)
                                                {
                                                    if (proddim.Value.ToString().Contains("-1"))
                                                    {
                                                        erpDimValueSet.DimensionValue = quantityMeasureAsVariationAttribute.Value; // only Quantity Measure as Size
                                                    }
                                                    else // Quantity + Quantity Measure as Size
                                                    {
                                                        erpDimValueSet.DimensionValue = proddim.Value.ToString().TrimEnd('0').TrimEnd('.') + " " + quantityMeasureAsVariationAttribute.Value;

                                                    }
                                                }
                                                else // only Quantity as Size
                                                {
                                                    erpDimValueSet.DimensionValue = proddim.Value.ToString().TrimEnd('0').TrimEnd('.');
                                                }
                                            }
                                            else
                                            {
                                                erpDimValueSet.DimensionValue = proddim.Value.ToString();
                                            }

                                            // REMOVE THIS LINE // trace.Append(string.Format("Traversing custom attributes=> Dimension values {0}", proddim.Value.ToString()) + Environment.NewLine);
                                            trace.Append(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10204), proddim.Value.ToString()) + Environment.NewLine);
                                            #region "TFS-1956"
                                            //NS:
                                            //BugFix:TFS-1956 Start

                                            //if (erpProduct.ImageList == null)
                                            //{
                                            //    erpProduct.ImageList = new List<ErpRichMediaLocationsRichMediaLocation>();
                                            //}

                                            ////having already image for master
                                            ////TODO: for master we need to decide which product will be set as master
                                            //if (erpProduct.ImageList.Where(d => d.DimensionValue == "" && d.DimensionKey == null).FirstOrDefault() == null)
                                            //{
                                            //    erpProduct.ImageList.Add(new ErpRichMediaLocationsRichMediaLocation
                                            //    {
                                            //        Url = ConfigurationHelper.GetSetting(APPLICATION.Retail_Media_Path) + erpProduct.PaddedItemId + ".jpg",
                                            //        DimensionValue = string.Empty
                                            //    });
                                            //}


                                            //erpProduct.ImageList.Add(new ErpRichMediaLocationsRichMediaLocation
                                            //{
                                            //    Url = ConfigurationHelper.GetSetting(APPLICATION.Retail_Media_Path) + erpProduct.PaddedItemId + ".jpg",
                                            //    DimensionKey = variationAttribute.ComValue,
                                            //    DimensionValue = proddim.Value.ToString()
                                            //});
                                            //BugFix:TFS-1956 End

                                            //NS: Old

                                            #endregion
                                            erpDimValueSet.DimensionKey = variationAttribute.ComValue;
                                            erpDimValueSet.ImageURL = configurationHelper.GetSetting(APPLICATION.Retail_Media_Path) + erpProduct.PaddedItemId + ".jpg";

                                            ErpProductDimensionValueSet erpDimValueSetMaster = new ErpProductDimensionValueSet();
                                            erpDimValueSetMaster.ImageURL = configurationHelper.GetSetting(APPLICATION.Retail_Media_Path) + erpProduct.PaddedItemId + ".jpg";
                                            erpDimValueSetMaster.DimensionValue = String.Empty;
                                            dimensionSetValuesList.Add(erpDimValueSetMaster);

                                            dimensionSetValuesList.Add(erpDimValueSet);
                                        }

                                        // Related products are basically variant products of that master product
                                        var variantProducts = erpProduct.RelatedProducts.Where(k => k.RelationName == configurationHelper.GetSetting(PRODUCT.Attr_Flat_Hierarchy_Related));

                                        foreach (var relprod in variantProducts)
                                        {
                                            ErpProductDimensionValueSet erpDimValueSetVariants = new ErpProductDimensionValueSet();

                                            ErpProduct product = erpProducts.FirstOrDefault(k => k.RecordId == relprod.RelatedProductRecordId);

                                            if (product != null)
                                            {

                                                var dimValue = product.CustomAttributes.Where(x => x.Key.Equals(proddim.Key)).Select(d => d.Value).FirstOrDefault();

                                                if (dimValue != "" && dimValue != null && dimValue != "NA" && proddim.Value.ToString() != "-1")
                                                {

                                                    bool alreadyExists = dimensionSetValuesList.Any(x => x.DimensionValue == dimValue);

                                                    if (!alreadyExists)
                                                    {
                                                        // REMOVE THIS LINE // trace.Append(string.Format("Traversing custom attributes=> Dimension values {0}", dimValue) + Environment.NewLine);
                                                        trace.Append(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10204), dimValue) + Environment.NewLine);

                                                        if (variationAttribute.ComValue.ToString() == "Size")
                                                        {
                                                            var QuantityMeasureAsSecondValue = variationAttribute.AdditionalErpValue.ToString().Split(',');
                                                            var quantityMeasureAsVariationAttribute = product.CustomAttributes.Where(x => x.Key.Equals(QuantityMeasureAsSecondValue[0])).FirstOrDefault();

                                                            if (quantityMeasureAsVariationAttribute.Value != "NA"
                                                                    && quantityMeasureAsVariationAttribute.Value != ""
                                                                    && quantityMeasureAsVariationAttribute.Value != null)
                                                            {
                                                                if (dimValue.Contains("-1"))
                                                                {
                                                                    erpDimValueSetVariants.DimensionValue = quantityMeasureAsVariationAttribute.Value; // only Quantity Measure as Size
                                                                }

                                                                else //Quantity + Quantity Measure as Size
                                                                {
                                                                    erpDimValueSetVariants.DimensionValue = dimValue.ToString().TrimEnd('0').TrimEnd('.') + " " + quantityMeasureAsVariationAttribute.Value;
                                                                }
                                                            }
                                                            else // only Quantity as Size
                                                            {
                                                                erpDimValueSetVariants.DimensionValue = dimValue.ToString().TrimEnd('0').TrimEnd('.');
                                                            }
                                                        }
                                                        else
                                                        {
                                                            erpDimValueSetVariants.DimensionValue = dimValue;
                                                        }

                                                        #region "TFS-1956"

                                                        #endregion

                                                        erpDimValueSetVariants.DimensionKey = variationAttribute.ComValue;
                                                        erpDimValueSetVariants.ImageURL = configurationHelper.GetSetting(APPLICATION.Retail_Media_Path) + product.PaddedItemId + ".jpg";

                                                        dimensionSetValuesList.Add(erpDimValueSetVariants);
                                                    } // discussion with KAR, we will warn these products in Db Logs and will skip in catalog generation ==> Related missing in Catalog
                                                }
                                            }
                                        }

                                        //NS: to stop variant for single dimension value
                                        //need > 2 becuase of master imageURL
                                        if (dimensionSetValuesList.Count > 1)
                                        {
                                            // Now assign DimensionValues to DimensionSet objects
                                            erpProductDimSet.DimensionValues = dimensionSetValuesList;
                                            erpProductDimSetList.Add(erpProductDimSet);
                                        }
                                    }
                                } // End IF

                                // } // End Custom Attributes loop
                                //in case of variation attribute / Dimension set are more than one.  

                                if (erpProductDimSetList.Count > 1)
                                {
                                    // two or more dimension sets in case of no size
                                    if (erpProductDimSetList.Count(r => r.DimensionKey == "Size") == 0)
                                    {
                                        foreach (var dim in erpProductDimSetList)
                                        {
                                            // REMOVE THIS LINE // twoDimensionProducts.Append(string.Format("Dimension {0} for Product {1}, cannot have more than one dimensions", dim.DimensionKey, erpProduct.ItemId));
                                            twoDimensionProducts.Append(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10205), dim.DimensionKey, erpProduct.ItemId));
                                        }
                                    }
                                    else
                                    {
                                        var itemToRemove = erpProductDimSetList.Single(r => r.DimensionKey == "Size");
                                        if (itemToRemove.DimensionValues != null)
                                        {
                                            erpProductDimSetList.Remove(itemToRemove);
                                        }
                                    }
                                }

                                erpProduct.DimensionSets = erpProductDimSetList;

                            } // End if Custom Master  
                        }

                        // CustomLogger.LogDebugInfo(trace);
                        if (twoDimensionProducts.Length > 0)
                        {
                            CustomLogger.LogWarn(twoDimensionProducts.ToString(), currentStore.StoreId, currentStore.CreatedBy);
                        }
                    }
                }
            }

            catch (Exception exp)
            {
                throw exp;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpProducts;
        }

        /// <summary>
        /// Get all master products which no matter comes in catalog delta or not.
        /// InstanceRelationType == 3267 means it is master product.
        /// </summary>
        /// <param name="catalogId">CatalogId</param>
        /// <returns>All Master products in catalog</returns>
        public List<ErpProduct> GetCatalogMasterProducts(long catalogId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpProduct> erpProducts = new List<ErpProduct>();

            try
            {
                if (catalogId > 0)
                {
                    //NS: Call RS
                    /*
                    //NS: Comment Start
                    ReadOnlyCollection<CatalogActiveProductExtension> productExtensions = base.currentChannelState.ProductExtensionManager.GetCatalogActiveProductsExtension(catalogId);
                    erpProducts.AddRange(productExtensions.Select(x => new ErpProduct() { IsMasterProduct = x.InstanceRelationType == 3267, MasterProductId = x.Product }).Where(x => x.IsMasterProduct == true).ToList());
                    //NS: Comment End
                    */
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpProducts;
        }

        public ProductImageUrlResponse GetProductImageUrl(ProductImageUrlRequest productImageUrlRequest)
        {
            ProductCRTManager productManager = new ProductCRTManager();
            return productManager.ProductImageUrl(productImageUrlRequest, currentStore.StoreKey);
        }

        /// <summary>
        /// GetProductLinkedItemExtension gets Single Linked Product based on channel dbax,RetailInventLinkedItem table.
        /// </summary>
        /// <param name="erpKey">Product itemId</param>
        /// <param name="erpKey">Product variantId</param>
        /// <returns>ErpLinkedProduct</returns>
        public ErpLinkedProduct GetProductLinkedItemExtension(string parentVariantId, string optionVariantId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                //NS: Call RS
                //NS: Comment Start
                /*
                IReadOnlyCollection<MFProductLinkedItemExtension> productLinkedItemExtensions = this.currentChannelState.MFProductLinkedItemExtensionManager.GetMFProductLinkedItemExtension(parentVariantId, optionVariantId);
                
                if (productLinkedItemExtensions != null && productLinkedItemExtensions.Count > 0)
                {
                    ErpLinkedProduct erpProdLinkedItem = new ErpLinkedProduct();
                    erpProdLinkedItem = ProcessProductLinkedItemExtension(productLinkedItemExtensions.FirstOrDefault());

                    return erpProdLinkedItem;
                }
                else
                {
                    return null;
                }
                */
                //NS: Comment End

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

                return null;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy);
                return null;
            }
        }

        #endregion

        #region Helper Methods

        ///// <summary>
        ///// LoadProductAttributes loads AX product attributes.
        ///// </summary>
        ///// <param name="productManager">Instance of ProductManager.</param>
        ///// <param name="attributes">Upon return, contains the product attribute space of the channel.</param>
        //private IEnumerable<AttributeProduct> LoadProductAttributes()
        //{
        //    var getProductAttributesCriteria = new QueryResultSettings(new PagingInfo(1000, 0));

        //    var attributes = new List<AttributeProduct>();
        //    IEnumerable<AttributeProduct> currentAttributePage;
        //    do
        //    {
        //        currentAttributePage = _currentChannelState.ProductManager.GetChannelProductAttributes(getProductAttributesCriteria);
        //        attributes.AddRange(currentAttributePage);
        //        getProductAttributesCriteria.Paging.Skip = getProductAttributesCriteria.Paging.Skip +
        //                                                   getProductAttributesCriteria.Paging.Top;
        //    } while (currentAttributePage.Count() == getProductAttributesCriteria.Paging.Top);

        //    return attributes;
        //}

        /// <summary>
        /// ProcessDeletedProducts processes and added deleted products in erpProducts.
        /// </summary>
        private void ProcessDeletedProducts(List<ErpProduct> erpProducts)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpProducts));

            List<long> catalogIds = erpProducts.Select(p => p.CatalogId).Where(c => c != 0).Distinct().ToList();

            List<ErpProduct> deletedProducts = this.GetDeletedProducts(catalogIds);

            erpProducts.AddRange(deletedProducts);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpProducts));

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// GetDeletedProducts gets list of deleted products.
        /// </summary>
        /// <param name="catalogIds"></param>
        /// <returns></returns>
        private List<ErpProduct> GetDeletedProducts(List<long> catalogIds)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(catalogIds));

            List<ErpProduct> deletedProducts = new List<ErpProduct>();
            ErpProduct deletedItem;
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

            var publishedProductKeys = integrationManager.GetAllEntityKeys(Entities.Product);

            var productExistenceIds = publishedProductKeys.Select(p => new ErpProductExistenceId { ProductId = long.Parse(p.ErpKey) }).Distinct().ToList();

            List<long> allVarifiedProductIds = new List<long>();

            if (productExistenceIds.Count > 0)
            {
                foreach (long catalogId in catalogIds)
                {
                    IEnumerable<ErpProductExistenceId> varifiedProductIds = this.GetVerifiedProducts(catalogId, productExistenceIds);
                    allVarifiedProductIds.AddRange(varifiedProductIds.Select(p => p.ProductId));
                }

                var deletedProductKeys = publishedProductKeys.Where(p => !allVarifiedProductIds.Contains(long.Parse(p.ErpKey))).ToList();
                //publishedProducts.Except()

                foreach (var dpKey in deletedProductKeys)
                {
                    deletedItem = new ErpProduct();
                    deletedItem.RecordId = long.Parse(dpKey.ErpKey);
                    deletedItem.SKU = dpKey.ComKey;
                    deletedItem.EcomProductId = dpKey.ComKey;
                    deletedItem.Mode = ErpChangeMode.Delete;
                    deletedProducts.Add(deletedItem);
                }

                integrationManager.DeleteEntityKeys(deletedProductKeys);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(deletedProducts));

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return deletedProducts;
        }

        /// <summary>
        /// GetVerifiedProducts get list of Deleted products.
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="productExistenceIds"></param>
        /// <returns></returns>
        private IEnumerable<ErpProductExistenceId> GetVerifiedProducts(long catalogId, IEnumerable<ErpProductExistenceId> productExistenceIds)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "catalogId: " + catalogId.ToString() + "productExistenceIds: " + JsonConvert.SerializeObject(productExistenceIds));
            var crtProductManager = new ProductCRTManager();
            IEnumerable<ErpProductExistenceId> verifiedProductsIds = crtProductManager.VerifyProductExistence(configurationHelper.GetSetting(APPLICATION.Channel_Id).LongValue(), catalogId, productExistenceIds,currentStore.StoreKey);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(verifiedProductsIds));
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return verifiedProductsIds;

        }

        #endregion
    }

}