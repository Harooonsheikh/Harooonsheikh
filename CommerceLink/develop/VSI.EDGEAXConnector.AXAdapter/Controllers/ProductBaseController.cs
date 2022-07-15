//using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Mapster;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    /// <summary>
    /// ProductController class is product controller for AX.
    /// </summary>
    public class ProductBaseController : BaseController
    {
        #region Properties
        //private List<string> AdditionalProductImageFiles { get; set; }
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProductBaseController(string storeKey) : base(storeKey)
        {
            //this.LoadAdditionalProductImageFiles();
        }

        #endregion

        #region protected Methods
        /// <summary>
        /// ProcessProducts process to provided products with variants.
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        protected List<ErpProduct> ProcessProducts(List<KeyValuePair<long, IEnumerable<ErpProduct>>> products)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpProduct> erpProducts = new List<ErpProduct>();
            List<ErpProduct> catalogMasterProducts = new List<ErpProduct>();
            List<ErpProduct> catalogSimpleAndVariantsProducts = new List<ErpProduct>();

            bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();
            ErpProduct erpProductMasterAsMaster = null;
            ErpProduct erpProd = new ErpProduct();

            StringBuilder missingRelatedProducts = new StringBuilder();
            StringBuilder missingIsMaster = new StringBuilder();
            StringBuilder relatedAndMasterProductsTrace = new StringBuilder();
            StringBuilder productsDetailTrace = new StringBuilder();

            //List<ErpRelatedProduct> erpKitRelatedProducts = new List<ErpRelatedProduct>();

            try
            {
                if (products != null && products.Count > 0)
                {
                    //++ getting list of all RECID's from Catalog products.
                    //ReadOnlyCollection<CatalogActiveProductExtension> productExtensions = base.currentChannelState.ProductExtensionManager.GetCatalogActiveProductsExtension(products[0].Key);
                    //var activeCatalogProducts = productExtensions.Select(x => x.Product.ToString()).ToList(); 
                    //KAR
                    if (isFlatProductHierarchy)
                    {
                        foreach (var productSet in products)
                        {
                            foreach (var erpProduct in productSet.Value)
                            {
                                if (erpProduct.CustomAttributes != null)
                                {
                                    var isCustomMasterAttributeExists = erpProduct.CustomAttributes.Where(x => x.Key.Equals(configurationHelper.GetSetting(PRODUCT.Attr_IsMaster))).FirstOrDefault();
                                    productsDetailTrace.Append(JsonConvert.SerializeObject(erpProduct) + Environment.NewLine);
                                    if (isCustomMasterAttributeExists.Key == null || isCustomMasterAttributeExists.Value == null)
                                    {
                                        // As per discussion with KAR we will treat those products as is Master = false on which we have no Is Master attribute, instead of Throw we will warn that message
                                        //throw new NullReferenceException(string.Format("Attribute {0} is missing on Product {1} ", ConfigurationHelper.GetSetting(PRODUCT.Attr_IsMaster), erpProduct.ItemId));
                                        // REMOVE THIS LINE // missingIsMaster.Append(string.Format("Attribute {0} is missing on Product {1} ", ConfigurationHelper.GetSetting(PRODUCT.Attr_IsMaster), erpProduct.ItemId) + Environment.NewLine);
                                        missingIsMaster.Append(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10206), configurationHelper.GetSetting(PRODUCT.Attr_IsMaster), erpProduct.ItemId) + Environment.NewLine);
                                        catalogSimpleAndVariantsProducts.Add(erpProduct);
                                    }
                                    else
                                    {
                                        if (isCustomMasterAttributeExists.Key.Equals(configurationHelper.GetSetting(PRODUCT.Attr_IsMaster)) && isCustomMasterAttributeExists.Value.Equals("True"))
                                        {
                                            catalogMasterProducts.Add(erpProduct);
                                            //Commented Kit Products by AF
                                            //if (erpProduct.IsKit)
                                            //{
                                            //    erpKitRelatedProducts = erpProduct.RelatedProducts.ToList();
                                            //}
                                            //End Commented Kit Products by AF
                                        }

                                        else if (isCustomMasterAttributeExists.Key.Equals(configurationHelper.GetSetting(PRODUCT.Attr_IsMaster)) && isCustomMasterAttributeExists.Value.Equals("False"))
                                        {
                                            catalogSimpleAndVariantsProducts.Add(erpProduct);
                                        }
                                        else // is Master is there but with blank value, we will take it as false
                                        {
                                            catalogSimpleAndVariantsProducts.Add(erpProduct);
                                        }
                                    }
                                }
                            }
                        }
                        foreach (var erpProduct in catalogMasterProducts)
                        {
                            // make replica master product and set isMasterProduct=true;, by default IsMasterProduct=false for Master product so we will treat it master as
                            //variant , and we will create master as master
                            //List<long> relatedProducts=erpProduct.RelatedProducts.Select(p => p.RelatedProductRecordId).Distinct().ToList();
                            //Commented due to Hash Set Container
                            //Commented Kit Products by AF
                            //if (erpProduct.IsKit)
                            //{
                            //    erpProducts.Add(erpProduct);
                            //}
                            //End Commented Kit Products by AF
                            // else
                            // {
                            if (erpProduct.RelatedProducts.Count > 0)
                            {
                                foreach (var rProd in erpProduct.RelatedProducts)
                                {
                                    erpProd = catalogSimpleAndVariantsProducts.FirstOrDefault(p => p.RecordId == rProd.RelatedProductRecordId);
                                    if (erpProd == null)
                                    {
                                        // As per discussion with KAR , we will warn these products in Db Logs and will skip in catalog generation
                                        // throw new NullReferenceException(string.Format("Related product : {0} for Master Product Id : {1} not found in catalog,either add in catalog or removed from related products", rProd.EntityName, erpProduct.ItemId));
                                        // REMOVE THIS LINE // missingRelatedProducts.Append(string.Format("Related product : {0} for Master Product Id : {1} not found in catalog,either add in catalog or remove from related products", rProd.EntityName, erpProduct.ItemId) + Environment.NewLine);
                                        missingRelatedProducts.Append(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10207), rProd.EntityName, erpProduct.ItemId) + Environment.NewLine);
                                    }
                                    else
                                    {
                                        this.GenerateSKUs(erpProd);

                                        erpProducts.Add(erpProd);
                                        // REMOVE THIS LINE //relatedAndMasterProductsTrace.Append(string.Format("Related/Child product {0} => of Parent Product {1} found and ready for xml generation", erpProd.ItemId, erpProduct.ItemId) + Environment.NewLine);
                                        relatedAndMasterProductsTrace.Append(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10208), erpProd.ItemId, erpProduct.ItemId) + Environment.NewLine);
                                    }
                                }
                            }
                            //if (erpProd != null) // Commented due to Related Products changed imlementation
                            //{
                            this.GenerateSKUs(erpProduct);

                            erpProducts.Add(erpProduct);

                            erpProductMasterAsMaster = new ErpProduct();

                            // To- Do (ErpChangeMode)Enum.Parse(typeof(ChangeAction), product.ChangeTrackingInformation.ChangeAction.ToString());
                            erpProductMasterAsMaster.Mode = ErpChangeMode.Insert;
                            erpProductMasterAsMaster.CatalogId = erpProduct.CatalogId;
                            erpProductMasterAsMaster.CustomAttributes = erpProduct.CustomAttributes;
                            erpProductMasterAsMaster.RecordId = erpProduct.RecordId;
                            erpProductMasterAsMaster.RelatedProducts = erpProduct.RelatedProducts;
                            erpProductMasterAsMaster.EntityName = erpProduct.EntityName;
                            erpProductMasterAsMaster.ItemId = erpProduct.ItemId;
                            erpProductMasterAsMaster.IsMasterProduct = true;
                            this.GenerateSKUs(erpProductMasterAsMaster);

                            erpProducts.Add(erpProductMasterAsMaster);

                            // REMOVE THIS LINE // relatedAndMasterProductsTrace.Append(string.Format("Now Added its Master Product {0} as well for xml generation", erpProduct.ItemId) + Environment.NewLine);
                            relatedAndMasterProductsTrace.Append(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10209), erpProduct.ItemId) + Environment.NewLine);

                            //}
                            // }
                        }

                        foreach (var erpProduct in catalogSimpleAndVariantsProducts)//Variant,Simple,Kit Child
                        {
                            //skip variants and kit child , only add simple
                            //erpProducts already contains (Master product), (variant Product),(Kit Master Product)
                            //Now we will add Simple Product.And will not add
                            //Kit Child Product in erpProducts and will not add again variants products
                            var isVariantProduct = erpProducts.Exists(p => p.RecordId == erpProduct.RecordId);

                            if (isVariantProduct.Equals(false)) // it not variant ,, either its simple or kit child
                            {
                                this.GenerateSKUs(erpProduct);
                                //it is Simple Product 
                                erpProducts.Add(erpProduct);
                                #region kit Products
                                //Commented Kit Products by AF
                                //var isKitChildItem = erpKitRelatedProducts.Exists(p => p.RelatedProductRecordId == erpProduct.RecordId);

                                //if (!isKitChildItem) // it not Kit child
                                //{
                                //    //it is Simple Product 
                                //    erpProducts.Add(erpProduct);

                                //}

                                //End Commented Kit Products by AF
                                #endregion
                            }
                        }
                        if (missingIsMaster.Length > 0)
                        {
                            CustomLogger.LogWarn(missingIsMaster.ToString(), currentStore.StoreId, currentStore.CreatedBy);
                        }
                        if (missingRelatedProducts.Length > 0)
                        {
                            CustomLogger.LogWarn(missingRelatedProducts.ToString(), currentStore.StoreId, currentStore.CreatedBy);
                        }
                        if (relatedAndMasterProductsTrace.Length > 0)
                        {
                            CustomLogger.LogDebugInfo(relatedAndMasterProductsTrace.ToString(), currentStore.StoreId, currentStore.CreatedBy);
                        }

                    } //if (isFlatProductHierarchy)
                    else
                    {
                        List<ErpProduct> erpCatalogProducts;

                        foreach (var productSet in products)
                        {
                            erpCatalogProducts = new List<ErpProduct>();

                            if (productSet.Value.ToList().Count > 0)
                            {
                                ProcessProductsForRetailInventItemSalesSetup(productSet.Value.ToList());
                            }

                            foreach (var product in productSet.Value)
                            {
                                var erpProduct = product;
                                erpProduct.Mode = ErpChangeMode.Insert;
                                erpProduct.CatalogId = productSet.Key;
                                this.GenerateSKUsForMagento(erpProduct);
                                // TODO : SK - IndexedProductProperties is getting null so directly added custom attributes need consideration in future
                                //erpProduct.CustomAttributes =  this.ProcessProductCustomAttributes(product.IndexedProductProperties);
                                erpProduct.CustomAttributes = product.CustomAttributes;

                                this.ProcessImages(erpProduct);
                                this.ProcessMasterProduct(erpProduct, erpCatalogProducts);
                                //List<string> activeProducts = products.First().Value.Select(m => m.RecordId.ToString()).ToList();
                                //this.ProcessMasterProduct(erpProduct, erpCatalogProducts, activeProducts);

                                erpCatalogProducts.Add(erpProduct);
                            }


                            var crtProductManager = new ProductCRTManager();

                            var allVariants = erpCatalogProducts.Where(a => !a.IsMasterProduct).ToList();
                            crtProductManager.ProcessVariantCustomAttributes(allVariants, currentStore.StoreKey);

                            if (erpCatalogProducts.Count > 0)
                            {
                                this.ProcessProductUpsell(productSet.Key, erpCatalogProducts);

                                erpProducts.AddRange(erpCatalogProducts);
                                erpCatalogProducts.Clear();
                            }
                        }
                    }
                }
                //Wahaj:
                //Not needed for standard implementation
                //this.ProcessProductExtension(erpProducts);//this.ProcessProductBarcode(erpProducts);
            }
            catch (Exception exp)
            {
                //CustomLogger.LogException(exp);
                throw exp;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpProducts;
        }

        /// <summary>
        /// ProcessProductUpsell gets upsell items.
        /// </summary>
        /// <param name="catalogId"></param>
        /// <param name="erpProducts"></param>
        private void ProcessProductUpsell(long catalogId, List<ErpProduct> erpProducts)
        {
            var crtProductManager = new ProductCRTManager();
            var upsellItems = crtProductManager.GetProductUpsell(erpProducts.Select(p => p.ItemId).Distinct().ToList(),currentStore.StoreKey);

            if (upsellItems != null && upsellItems.Count() > 0)
            {
                foreach (var upsellItem in upsellItems)
                {
                    // The upsell item and its linked item should be of same catalog
                    var product = erpProducts.FirstOrDefault(prd => prd.IsMasterProduct && prd.ItemId == upsellItem.ItemId);
                    var linkedProduct = erpProducts.FirstOrDefault(prd => prd.IsMasterProduct && prd.ItemId == upsellItem.LinkedItemId);

                    if (product != null && product.Variants != null && product.Variants.Count > 0 &&
                        linkedProduct != null && linkedProduct.Variants != null && linkedProduct.Variants.Count > 0)
                    {
                        foreach (var productVariant in product.Variants)
                        {
                            if (upsellItem.TMVCrosssellType == ErpTMVCrosssellType.Migration)
                            {
                                //below line commented as we need all variants for migration
                                //var linkedProductVariant = linkedProduct.Variants.FirstOrDefault();

                                foreach (var linkedProductVariant in linkedProduct.Variants)
                                {
                                    var linkedProductFinalItem = erpProducts.FirstOrDefault(p => p.RecordId == linkedProductVariant.DistinctProductVariantId);
                                    var productFinalItem = erpProducts.FirstOrDefault(p => p.RecordId == productVariant.DistinctProductVariantId);
                                    if (productFinalItem != null)
                                    {
                                        if (productFinalItem.UpsellItems == null)
                                        {
                                            productFinalItem.UpsellItems = new List<ErpUpsellItem>();
                                        }
                                        productFinalItem.UpsellItems.Add(new ErpUpsellItem
                                        {
                                            ItemId = upsellItem.ItemId,
                                            LinkedItemId = upsellItem.LinkedItemId,
                                            LinkedProductSKU = linkedProductFinalItem.SKU,
                                            Priority = upsellItem.Priority,
                                            RecId = upsellItem.RecId,
                                            UpsellTypeId = upsellItem.UpsellTypeId,
                                            TMVCrosssellType = upsellItem.TMVCrosssellType,
                                            TMVCrosssellBundleQuantity = upsellItem.TMVCrosssellBundleQuantity
                                        });
                                    }
                                }
                            }
                            else
                            {
                                // Color and Style of the upsell item and its linked product should be same
                                var linkedProductVariant = linkedProduct.Variants.FirstOrDefault(v => v.ColorId == productVariant.ColorId && v.StyleId == productVariant.StyleId);

                                if (linkedProductVariant != null)
                                {
                                    var linkedProductFinalItem = erpProducts.FirstOrDefault(p => p.RecordId == linkedProductVariant.DistinctProductVariantId);
                                    var productFinalItem = erpProducts.FirstOrDefault(p => p.RecordId == productVariant.DistinctProductVariantId);

                                    if (linkedProductFinalItem != null && productFinalItem != null)
                                    {
                                        if (productFinalItem.UpsellItems == null)
                                        {
                                            productFinalItem.UpsellItems = new List<ErpUpsellItem>();
                                        }

                                        productFinalItem.UpsellItems.Add(new ErpUpsellItem
                                        {
                                            ItemId = upsellItem.ItemId,
                                            LinkedItemId = upsellItem.LinkedItemId,
                                            LinkedProductSKU = linkedProductFinalItem.SKU,
                                            Priority = upsellItem.Priority,
                                            RecId = upsellItem.RecId,
                                            UpsellTypeId = upsellItem.UpsellTypeId,
                                            TMVCrosssellType = upsellItem.TMVCrosssellType,
                                            TMVCrosssellBundleQuantity = upsellItem.TMVCrosssellBundleQuantity
                                        });
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void ProcessProductsForRetailInventItemSalesSetup(List<ErpProduct> erpProductList)
        {
            var masterProductList = erpProductList.Where(p => p.IsMasterProduct == true).Select(p=> p.RecordId).ToList();
            var crtProductManager = new ProductCRTManager();

            var retailInventItemSalesSetupList = crtProductManager.GetRetailInventItemSalesSetup(masterProductList, currentStore.StoreKey);
            if (retailInventItemSalesSetupList != null)
            {

                foreach (var retailInventItemSalesSetup in retailInventItemSalesSetupList)
                {
                    ErpProduct erpProduct = erpProductList.FirstOrDefault(p => p.RecordId == retailInventItemSalesSetup.Product);

                    if (erpProduct != null)
                    {
                        erpProduct.LowestQty = retailInventItemSalesSetup.LowestQty;
                        erpProduct.HighestQty = retailInventItemSalesSetup.HighestQty;
                    }
                } 
            }
        }


        //Not required as we are processing this in ProcessProductUpsell
        ///// <summary>
        ///// ProcessUpsellSKU process and update sku of upsell/crosssell items.
        ///// </summary>
        ///// <param name="erpProducts"></param>
        //private void ProcessUpsellSKU(List<ErpProduct> erpProducts)
        //{
        //    if(erpProducts!=null && erpProducts.Count>0)
        //    {
        //        foreach(var prod in erpProducts)
        //        {
        //            if(prod.UpsellItems!=null && prod.UpsellItems.Count>0)
        //            {
        //                foreach(var upsellItem in prod.UpsellItems)
        //                {
        //                    var upsellProduct = erpProducts.FirstOrDefault(p => p.RecordId == upsellItem.LinkedProductId);
        //                    if(upsellProduct != null)
        //                    {
        //                        upsellItem.LinkedProductSKU = upsellProduct.SKU;
        //                    }
        //                }
        //            }
        //            var a = (prod.UpsellItems != null && prod.UpsellItems.Count > 0) ? string.Join(",", prod.UpsellItems.Where(ui => ui.UpsellTypeId == ErpUpsellType.Upsell).Select(ui => ui.LinkedProductSKU)) : string.Empty;
        //        }
        //    }
        //}

        /// <summary>
        /// Process Master Product
        /// </summary>
        /// <param name="product"></param>
        /// <param name="erpCatalogProducts"></param>
        private void ProcessMasterProduct(ErpProduct product, List<ErpProduct> erpCatalogProducts)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "product: " + JsonConvert.SerializeObject(product) + ", erpCatalogProducts: " + JsonConvert.SerializeObject(erpCatalogProducts));

            if (product.IsMasterProduct)
            {
                var variants = this.GetVariantsAsErpProduct(product, erpCatalogProducts);

                if (variants != null && variants.Count > 0)
                {
                    erpCatalogProducts.AddRange(variants);
                }
                //++ US Commented as below check has not in use and and has already commented.
                //if (product.DimensionSets != null)
                //{
                //    //if (product.DimensionSets.Count > 0)
                //    //{
                //    //    product.DimensionSets = Mapper.Map<ICollection<ErpProductDimensionSet>, ICollection<ErpProductDimensionSet>>(product.CompositionInformation.VariantInformation.Dimensions).ToList();
                //    //}
                //}
                if (product.DimensionSets == null)
                {
                    product.DimensionSets = new List<ErpProductDimensionSet>();
                }

                //product.PriemaryCategory = GetPrimaryCategory(variants);
                //++ 
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
        }

        /// <summary>
        /// GetProducts gets products from ERP.
        /// </summary>
        /// <param name="fetchAll"></param>
        /// <returns></returns>
        protected List<KeyValuePair<long, IEnumerable<ErpProduct>>> GetCatalogProducts(bool useDelta, List<ErpCategory> categories, bool fetchAll)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "fetchAll: " + fetchAll.ToString());

            var productLists = new List<KeyValuePair<long, IEnumerable<ErpProduct>>>();

            var crtProductManager = new ProductCRTManager();
            productLists = crtProductManager.GetCatalogProducts(configurationHelper.GetSetting(APPLICATION.Channel_Id).LongValue(), useDelta, categories, fetchAll,currentStore.StoreKey);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return productLists;

            #region Oldcode


            //NS: Remove
            //NS: Comment Start
            /*
            var productLists = new List<KeyValuePair<long, IEnumerable<Product>>>();
            List<Product> changedProductsSearchResult;
            char[] startingSyncToken = { '1' };

            try
            {
                // set the query criteria in the channel state
                currentChannelState.ChangedListingSearchCriteria = new ChangedProductsSearchCriteria();
                currentChannelState.ChangedListingSearchCriteria.AsListings = false;
                //currentChannelState.ChangedListingSearchCriteria.Context.ChannelId = currentChannelState.OnlineChannelInstance.RecordId;
                currentChannelState.ChangedListingSearchCriteria.Context.ChannelId = CommerceRuntimeHelper.ChannelId;
                currentChannelState.ChangedListingSearchCriteria.DataLevel = CommerceEntityDataLevel.Complete;

                if (!fetchAll)
                {
                    foreach (var cprop in currentChannelState.OnlineChannelInstance.ChannelProperties)
                    {
                        if (cprop.Name == "SyncAnchor")
                        {
                            startingSyncToken = cprop.Value.ToCharArray();
                            break;
                        }
                    }
                }

                if (startingSyncToken.Length == 1)
                {
                    startingSyncToken = ProductChangeTrackingAnchorSet.GetSynchronizationTokenFromAnchorSet(new ProductChangeTrackingAnchorSet(), 0);
                }

                currentChannelState.ChangedListingSearchCriteria.SynchronizationToken = startingSyncToken;

                var crtProductManager = new ProductCRTManager();
                IReadOnlyCollection<ErpProductCatalog> productCatalogs = crtProductManager.GetProductCatalogs(ConfigurationHelper.ChannelID);

                currentChannelState.ListingQueryCriteria = new QueryResultSettings(PagingInfo.AllRecords); //KAR
                currentChannelState.ListingQueryCriteria.Paging.Top = 10000;


                //IReadOnlyCollection<ProductCatalog> productCatalogs = currentChannelState.ProductManager.GetProductCatalogs(
                //    currentChannelState.OnlineChannelInstance.RecordId,
                //    currentChannelState.ListingQueryCriteria).Results;

                IReadOnlyCollection<ProductCatalog> productCatalogs = currentChannelState.ProductManager.GetProductCatalogs(CommerceRuntimeHelper.ChannelId, currentChannelState.ListingQueryCriteria).Results;

                //TODO: what is purpose of the following code
                // save the low sync anchor as the starting point for this job run
                //startingSyncToken = _currentChannelState.ChangedListingSearchCriteria.SynchronizationToken;
                //_currentChannelState.ChangedListingSearchCriteria.SynchronizationToken = startingSyncToken;


                currentChannelState.ChangedListingSearchCriteria.Session = currentChannelState.ProductManager.BeginReadChangedProducts(currentChannelState.ChangedListingSearchCriteria);

                try
                {
                    // cycle through the product catalogs, retrieving and publishing products using the same set of sync anchors.
                    foreach (var productCatalog in productCatalogs)
                    {
                        var productCatalogId = productCatalog.RecordId;

                        // set the catalog id on the search criteria
                        currentChannelState.ChangedListingSearchCriteria.Context.CatalogId = productCatalogId;
                        currentChannelState.ChangedListingSearchCriteria.Session.ResetNumberOfProductsRead();

                        changedProductsSearchResult = this.GetChangedCatalogProducts(currentChannelState.ProductManager, currentChannelState.ChangedListingSearchCriteria, currentChannelState.ListingQueryCriteria);

                        if (changedProductsSearchResult != null && changedProductsSearchResult.Count() > 0)
                        {
                            productLists.Add(new KeyValuePair<long, IEnumerable<Product>>(productCatalogId, changedProductsSearchResult));
                        }
                    } // for each product catalog
                }
                finally
                {
                    currentChannelState.ProductManager.EndReadChangedProducts(currentChannelState.ChangedListingSearchCriteria.Session);
                }

                // update the sync status of the listings and save the next sync anchor as the current 
                // high mark. If the last query returned an empty set of listings, the next sync anchor 
                // will be set to 0, and the sync status will be set incorrectly in the CatalogPublishStatus
                // table. There seems to be no workaround this, as we can't set directly the next sync anchor
                // value in the ListingQueryCriteria.
                //// currentChannelState.ProductManager.UpdateListingsSyncStatus(currentChannelState.ListingQueryCriteria);
                if (!fetchAll)
                {
                    var channelProperty = new ChannelProperty
                    {
                        Name = "SyncAnchor",
                        Value = new string(currentChannelState.ChangedListingSearchCriteria.Session.NextSynchronizationToken)
                    };
                    currentChannelState.ChannelManager.UpdateChannelProperties(new[] { channelProperty });
                }
            } // end try
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
                throw;
            }
            */
            //NS: Comment End
            #endregion

        }

        //NS: Remove
        //NS: Comment Start
        /*
        /// <summary>
        /// LoadChangedProductsPage loads the specified listing page, and returns true if a full page was loaded.
        /// </summary>
        /// <param name="productManager"></param>
        /// <param name="changedListingSearchCriteria"></param>
        /// <param name="listingQueryCriteria"></param>
        /// <returns></returns>
        protected List<Product> GetChangedCatalogProducts(ProductManager productManager, ChangedProductsSearchCriteria changedListingSearchCriteria, QueryResultSettings listingQueryCriteria)
        {
            ChangedProductsSearchResult currentPageListings = null;
            List<Product> products = new List<Product>();
            try
            {
                do
                {

                    currentPageListings = productManager.GetChangedProducts(changedListingSearchCriteria, listingQueryCriteria);

                    if (currentPageListings != null && currentPageListings.Results != null && currentPageListings.Results.Count() > 0)
                    {
                        products.AddRange(currentPageListings.Results);
                    }


                    //var atts = productManager.GetChannelProductAttributes(new QueryResultSettings(new PagingInfo(PagingInfo.MaximumPageSize)));
                    //var at = productManager.GetCurrentChannelProductAttribute(atts[0].RecordId);

                    //var ps = productManager.GetProducts(new QueryResultSettings(new PagingInfo(PagingInfo.MaximumPageSize)));
                }
                while (currentChannelState.ChangedListingSearchCriteria.Session.NumberOfProductsRead < currentChannelState.ChangedListingSearchCriteria.Session.TotalNumberOfProducts);
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
                throw;
            }

            return products;
        }
        */
        //NS: Comment End

        /// <summary>
        /// GetVariantAsErpProduct get variants and transform them to ErpProduct.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="erpMasterProdct"></param>
        /// <returns></returns>
        protected List<ErpProduct> GetVariantsAsErpProduct(ErpProduct erpMasterProduct, List<ErpProduct> activeCatalogProducts)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "erpMasterProduct: " + JsonConvert.SerializeObject(erpMasterProduct) + ", activeCatalogProducts: " + JsonConvert.SerializeObject(activeCatalogProducts));

            var erpProducts = new List<ErpProduct>();
            //NS: Comment Start
            //IReadOnlyCollection<ErpProductVariant> variants = product.GetVariants();
            //NS: Comment End
            ICollection<ErpProductVariant> variants = erpMasterProduct.Variants;

            //++ filtering the varients with catalog varients
            //IList<ErpProductVariant> activeVarients = variants.ToList();//KAR
            //if (activeCatalogProducts != null && activeCatalogProducts.Count > 0)//KAR
            //    activeVarients = variants.Where(t => activeCatalogProducts.Contains(t.DistinctProductVariantId.ToString())).ToList(); //KAR

            foreach (var productVariant in variants)
            {
                //if (activeCatalogProducts.Any(x => x.Contains(productVariant.DistinctProductVariantId.ToString())))
                //{
                //var erpProduct = Mapper.Map<Product, ErpProduct>(product);
                ErpProduct erpProduct = new ErpProduct();

                erpProduct.Mode = erpMasterProduct.Mode;
                //TODO: If we need to process Custom Attributes from variants but from existing code we are copint it from parent product
                if (productVariant.IndexedProperties != null && erpMasterProduct.CustomAttributes != null)
                {
                    erpProduct.CustomAttributes = this.ProcessProductCustomAttributes(productVariant.IndexedProperties, erpMasterProduct.CustomAttributes);
                }
                else if (erpMasterProduct.CustomAttributes != null)
                {
                    erpProduct.CustomAttributes = new List<KeyValuePair<string, string>>(erpMasterProduct.CustomAttributes);
                }

                erpProduct.AdjustedPrice = productVariant.AdjustedPrice;//TODO: Added for Testing
                erpProduct.BasePrice = productVariant.BasePrice;//TODO: Added for Testing
                erpProduct.ItemId = productVariant.VariantId;
                erpProduct.ConfigId = productVariant.ConfigId;
                erpProduct.Configuration = productVariant.Configuration;
                erpProduct.RecordId = erpProduct.DistinctProductVariantId = productVariant.DistinctProductVariantId;
                erpProduct.InventoryDimensionId = productVariant.InventoryDimensionId;
                erpProduct.IsMasterProduct = false;
                erpProduct.MasterProductId = productVariant.MasterProductId;//TODO: Added for Testing
                erpProduct.MasterProductNumber = erpMasterProduct.ProductNumber;
                erpProduct.ProductName = erpMasterProduct.ProductName;
                erpProduct.Price = productVariant.Price;//TODO: Added for Testing
                erpProduct.ProductNumber = productVariant.ProductNumber;
                erpProduct.ImageList = erpMasterProduct.ImageList;
                erpProduct.Size = productVariant.Size;
                erpProduct.SizeId = productVariant.SizeId;
                erpProduct.Style = productVariant.Style;
                erpProduct.StyleId = productVariant.StyleId;
                erpProduct.VariantId = productVariant.VariantId;
                erpProduct.EcomProductId = productVariant.ItemId;
                erpProduct.Status = erpMasterProduct.Status;
                erpProduct.CatalogId = erpMasterProduct.CatalogId;
                erpProduct.Color = productVariant.Color;
                erpProduct.ColorId = productVariant.ColorId;
                erpProduct.Description = erpMasterProduct.Description;
                erpProduct.CategoryIds = erpMasterProduct.CategoryIds;
                // Add Translation Dictionary to the variant as well
                erpProduct.ProductDetailTranslationsDictionary = erpMasterProduct.ProductDetailTranslationsDictionary;
                erpProduct.Rules = erpMasterProduct.Rules;
                erpProduct.HighestQty = erpMasterProduct.HighestQty;
                erpProduct.LowestQty = erpMasterProduct.LowestQty;
                //erpProduct.PriemaryCategory = SetPrimaryCaegory(productVariant.IndexedProperties);
                //this.ProcessImages(erpProduct);
                this.GenerateSKUsForMagento(erpProduct);
                productVariant.SKU = erpProduct.SKU;

                // TV: AH: Set master product Image Url to all variants
                erpProduct.ProductImageUrl = erpMasterProduct.ProductImageUrl;

                erpProduct.AvaTax_TaxClassId = erpMasterProduct.AvaTax_TaxClassId;

                erpProduct.ChannelId = erpMasterProduct.ChannelId;


                erpProducts.Add(erpProduct);
                // }

            }

            //erpMasterProduct.ProductVariants = erpProducts;

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpProducts));

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpProducts;
        }

        /// <summary>
        /// Get Primary Category
        /// </summary>
        /// <param name="varients"></param>
        /// <returns></returns>
        private string GetPrimaryCategory(List<ErpProduct> varients)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(varients));

            var primaryCategory = string.Empty;

            var productwithPrimaryKey = varients.FirstOrDefault(t => t.PriemaryCategory != "");

            if (productwithPrimaryKey != null)
            {
                primaryCategory = productwithPrimaryKey.PriemaryCategory;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, primaryCategory);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return primaryCategory;
        }

        /// <summary>
        /// Set Primary Category
        /// </summary>
        /// <param name="indexedProductProperties"></param>
        /// <returns></returns>
        private string SetPrimaryCategory(ErpProductPropertyTranslationDictionary indexedProductProperties)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(indexedProductProperties));

            var primaryCategory = string.Empty;

            var primaryCategoryKey = configurationHelper.GetSetting(PRODUCT.Attr_PrimaryCategory_Name);

            foreach (var prop in indexedProductProperties.Values)
            {
                foreach (var a in prop.Values)
                {
                    if (a.Value.ToString() == primaryCategoryKey)
                        primaryCategory = a.ValueString;
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, primaryCategory.ToString());

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return primaryCategory;
        }

        //Extension Method
        //NS: Comment Start
        /*
        /// <summary>
        /// ProcessProductExtension get and update Product extension data.
        /// </summary>
        /// <param name="erpProducts"></param>
        protected void ProcessProductExtension(List<ErpProduct> erpProducts)
        {
            if (erpProducts.Count > 0)
            {
                List<long> recordIds = erpProducts.Select(p => p.RecordId).ToList();
                List<ErpProduct> product;
                if (recordIds != null && recordIds.Count > 0)
                {
                    // Get Product Extension List
                    IReadOnlyCollection<ProductExtension> producExtensions = this.currentChannelState.ProductExtensionManager.GetProductExtension(recordIds);
                    //TODO: optimize GetProductExtension to bring only those items having values.
                    foreach (var extension in producExtensions)
                    {
                        //In case of Multiple Catalogs a product can be duplicate products.
                        product = erpProducts.Where(prod => prod.RecordId == extension.RecordId).ToList();
                        if (product != null)
                        {
                            foreach (ErpProduct p in product)
                            {
                                p.SKU = extension.SKU;
                                p.Barcode = extension.ItemBarcode;
                            }
                        }
                    }
                }
            }
        }
        */
        //NS: Comment End

        /// <summary>
        /// ProcessProductCustomAttributes process and transforms CustomAttributes
        /// </summary>
        /// <param name="indexedProductProperties"></param>
        /// <param name="masterIndexedProductProperties"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> ProcessProductCustomAttributes(ErpProductPropertyTranslationDictionary indexedProductProperties,
            List<KeyValuePair<string, string>> masterProductCustomAttributes)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "indexedProductProperties: " + JsonConvert.SerializeObject(indexedProductProperties) + ", masterProductCustomAttributes: " + JsonConvert.SerializeObject(masterProductCustomAttributes));

            KeyValuePair<string, string> item;
            List<KeyValuePair<string, string>> customAttributes = new List<KeyValuePair<string, string>>();
            if (indexedProductProperties != null)
            {
                customAttributes = this.ProcessProductCustomAttributes(indexedProductProperties);
            }

            foreach (KeyValuePair<string, string> masterItem in masterProductCustomAttributes)
            {
                item = customAttributes.FirstOrDefault(i => i.Key.Equals(masterItem.Key, StringComparison.CurrentCultureIgnoreCase));
                if (string.IsNullOrWhiteSpace(item.Key))
                {
                    customAttributes.Add(masterItem);
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return customAttributes;
        }

        /// <summary>
        /// ProcessProductCustomAttributes process and transforms CustomAttributes
        /// </summary>
        /// <param name="indexedProductProperties"></param>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> ProcessProductCustomAttributes(ErpProductPropertyTranslationDictionary indexedProductProperties)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, "indexedProductProperties: " + JsonConvert.SerializeObject(indexedProductProperties));

            List<KeyValuePair<string, string>> customAttributes = new List<KeyValuePair<string, string>>();

            foreach (var prop in indexedProductProperties.Values)
            {
                foreach (var a in prop.Values)
                {
                    //TODO: Apply Generalization to following checks for ErpProduct
                    //if (a.Key != "Description" && a.Key != "ProductName" && a.Key != "ItemNumber" && a.Key != "Image")
                    //{
                    //customAttributes.Add(new KeyValuePair<string, string>(a.Value.FriendlyName.Trim(), a.Value.ValueString));
                    //}

                    customAttributes.Add(new KeyValuePair<string, string>(a.Value.ToString(), a.ValueString));
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return customAttributes;
        }

        /// <summary>
        /// ProcessImage processes images from Product.
        /// </summary>
        /// <param name="erpProduct"></param>
        protected void ProcessImages(ErpProduct erpProduct)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpProduct));

            if (!bool.Parse(configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable)))
            {
                erpProduct.ImageList = new List<ErpRichMediaLocationsRichMediaLocation>();
                //NS: Custom image processing
                if (erpProduct.Image != null)
                {
                    foreach (var imageItem in erpProduct.Image.Items)
                    {
                        erpProduct.ImageList.Add(new ErpRichMediaLocationsRichMediaLocation
                        {
                            Url = imageItem.Url,
                            AltText = imageItem.AltText
                        });
                    }
                }
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            //TODO: revert to normal image processing 
            //string imageTagMid = string.Empty;
            //int totalImageFiles = int.Parse(ConfigurationHelper.MainProductImageFiles + this.AdditionalProductImageFiles.Count);
            //if (!erpProduct.IsMasterProduct && !string.IsNullOrWhiteSpace(erpProduct.Color))
            //{
            //    imageTagMid = "_" + erpProduct.Color;
            //}

            //string imageTag = ConfigurationHelper.MediaServerBaseURL + erpProduct.ItemId + imageTagMid + "_{0}." + ConfigurationHelper.ImageFileExtension;

            //erpProduct.Image = new ErpRichMediaLocations();

            //erpProduct.Image.Items = new ErpRichMediaLocationsRichMediaLocation[totalImageFiles];

            //for (int i = 0; i < 10; i++)
            //{
            //    erpProduct.Image.Items[i] = new ErpRichMediaLocationsRichMediaLocation { Url = string.Format(imageTag, i + 1) };
            //}

            //for (int i = 0; i < this.AdditionalProductImageFiles.Count; i++)
            //{
            //    erpProduct.Image.Items[int.Parse(ConfigurationHelper.MainProductImageFiles) + i] = new ErpRichMediaLocationsRichMediaLocation { Url = string.Format(imageTag, this.AdditionalProductImageFiles[i]) };
            //}
        }

        ///// <summary>
        ///// ProcessProductVariantImage process images from Product Variant.
        ///// </summary>
        ///// <param name="productVariant"></param>
        ///// <param name="erpProduct"></param>
        //private void ProcessProductVariantImage(ProductVariant productVariant, ErpProduct erpProduct)
        //{
        //    //TODO: If we need to process images for variants or may use of Parent               
        //    if (productVariant.Images != null)
        //    {
        //        List<ErpRichMediaLocationsRichMediaLocation> imageItems = new List<ErpRichMediaLocationsRichMediaLocation>();
        //        if (erpProduct.Image == null)
        //        {
        //            erpProduct.Image = new ErpRichMediaLocations();
        //        }
        //        foreach (var image in productVariant.Images)
        //        {
        //            if (image != null)
        //            {
        //                imageItems.AddRange(Mapper.Map<RichMediaLocationsRichMediaLocation[], ErpRichMediaLocationsRichMediaLocation[]>(image.Items));
        //            }
        //        }
        //        erpProduct.Image.Items = imageItems.ToArray();
        //    }
        //}


        //Extension Method
        //NS: Comment Start
        /*
        /// <summary>
        /// GetAllProducts returns All Products using CRT in Connector ERP Data Model.
        /// </summary>
        /// <param name="fetchAll"></param>
        /// <returns></returns>
        public List<ErpProduct> GetAllAssortmentProducts()
        {
            List<ErpProduct> erpProducts;
            try
            {
                ReadOnlyCollection<ProductExtension> productExtensions = base.currentChannelState.ProductExtensionManager.GetProductAssortmentExtension();

                erpProducts = this.ProcessProducts(productExtensions);
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
                throw;
            }
            return erpProducts;
        }
        */
        //NS: Comment Start

        //old Assortment based logic, it had paging issue
        ///// <summary>
        ///// GetAssortmentProduct loads all the product from assortment.
        ///// </summary>
        ///// <param name="productManager"></param>
        ///// <returns></returns>
        //protected List<KeyValuePair<long, IEnumerable<Product>>> GetAssortmentProducts()
        //{
        //    List<KeyValuePair<long, IEnumerable<Product>>> productSet = new List<KeyValuePair<long, IEnumerable<Product>>>();
        //    List<Product> products = new List<Product>();
        //    PagedResult<Product> productsPage = null;
        //    QueryResultSettings querySettings = new QueryResultSettings(new PagingInfo(10000));//PagingInfo.MaximumPageSize

        //    try
        //    {
        //        //TODO: paging has been disabled due to issue in paging logic.
        //        //do
        //        //{
        //        productsPage = base.currentChannelState.ProductManager.GetProducts(querySettings);
        //        products.AddRange(productsPage.Results);
        //        //    querySettings.Paging.Skip += querySettings.Paging.Top;
        //        //}
        //        //while (productsPage.Results.Count() == querySettings.Paging.Top);
        //        productSet.Add(new KeyValuePair<long, IEnumerable<Product>>(0, products));
        //    }
        //    catch (Exception exp)
        //    {
        //        CustomLogger.LogException(exp);
        //        throw;
        //    }

        //    return productSet;
        //}

        ///// <summary>
        ///// LoadAdditionalProductImageFiles loads AdditionalProductImageFiles
        ///// </summary>
        //private void LoadAdditionalProductImageFiles()
        //{
        //    this.AdditionalProductImageFiles = ConfigurationHelper.AdditionalProductImageFiles.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
        //}
        //++ UY
        //private List<string> GetCatalogProducts(long catalogID)
        //{
        //    var catalogProducts = new List<string>();

        //    string cs = ConfigurationManager.ConnectionStrings["OnlineStoreDBConnectionString"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("DBO.GETCATALOGPRODUTSVSI", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            cmd.Parameters.Add("@CATALOGID", SqlDbType.BigInt).Value = catalogID;
        //            con.Open();
        //            cmd.ExecuteNonQuery();

        //            SqlDataAdapter adp = new SqlDataAdapter(cmd);
        //            DataTable dt = new DataTable();

        //            adp.Fill(dt);

        //            //var  tt= dt.AsEnumerable().ToList();
        //            catalogProducts = (from row in dt.AsEnumerable() select Convert.ToString(row["PRODUCT"])).ToList();
        //        }
        //    }
        //    return catalogProducts;
        //}

        private string PrefixSKU(string sku)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            return configurationHelper.GetSetting(PRODUCT.SKU_Prefix) + sku;
        }

        private string PostfixSKU(string sku)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            return sku + configurationHelper.GetSetting(PRODUCT.SKU_Postfix);
        }

        protected void PaddingZeros(ErpProduct prod)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

          

            //?? commented code is doing same work in both of else
            prod.SKU = prod.EcomProductId = PrefixSKU(prod.ItemId);
            prod.PaddedItemId = prod.ItemId;
        }

        /// <summary>
        /// Generate SKU (Stock Keeping Unit) for products in product list
        /// </summary>
        /// <param name="erpProducts"></param>
        protected void GenerateSKUs(List<ErpProduct> erpProducts)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpProducts));

            foreach (ErpProduct prod in erpProducts)
            {
                this.GenerateSKUs(prod);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpProducts));
        }

        /// <summary>
        /// Generate SKU (Stock Keeping Unit) for product
        /// </summary>
        /// <param name="prod"></param>
        protected void GenerateSKUs(ErpProduct prod)
        {
            this.PaddingZeros(prod);

            //// Image Processing for Master products
            //prod.ImageList = new List<ErpRichMediaLocationsRichMediaLocation>();

            //prod.ImageList.Add(new ErpRichMediaLocationsRichMediaLocation
            //{
            //    Url = ConfigurationHelper.GetSetting(APPLICATION.Retail_Media_Path) + prod.PaddedItemId + ".jpg",
            //    AltText = prod.ItemId
            //});

            if (prod.IsMasterProduct == false)
            {
                prod.MasterProductId = 1;
            }
            else // Master Product
            {
                prod.SKU = this.PostfixSKU(prod.SKU);
                prod.EcomProductId = prod.SKU;
            }
        }


        protected void GenerateSKUsForMagento(List<ErpProduct> erpProducts)
        {
            CustomLogger.LogDebugInfo(string.Format("Enter in GenerateSKUsForMagento()"), currentStore.StoreId, currentStore.CreatedBy);
            foreach (ErpProduct prod in erpProducts)
            {
                this.GenerateSKUsForMagento(prod);
            }
            CustomLogger.LogDebugInfo(string.Format("Exit from GenerateSKUsForMagento()"), currentStore.StoreId, currentStore.CreatedBy);
        }

        /// <summary>
        /// Generate SKU (Stock Keeping Unit) for product.
        /// </summary>
        /// <param name="prod"></param>
        protected void GenerateSKUsForMagento(ErpProduct prod)
        {
            //Master Product
            if (prod.IsMasterProduct)
            {
                prod.SKU = prod.ItemId;
            }
            // Variant
            else if (!string.IsNullOrWhiteSpace(prod.MasterProductNumber))
            {
                prod.SKU = prod.MasterProductNumber + "_" + prod.VariantId;
            }
            // Simple Product
            else
            {
                prod.SKU = prod.ItemId;
            }
            prod.EcomProductId = prod.SKU = this.PostfixSKU(this.PrefixSKU(prod.SKU));
            //this.PaddingZeros(prod);
            //prod.SKU = prod.EcomProductId = prod.MasterProductNumber + "_" + prod.ColorId + "_" + prod.SizeId + "_" + prod.StyleId + "_" + prod.ItemId;
        }
        #endregion
    }
}