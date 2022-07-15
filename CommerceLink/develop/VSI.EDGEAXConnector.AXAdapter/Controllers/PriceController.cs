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
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using System.Configuration;
using VSI.EDGEAXConnector.Common.Constants;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    /// <summary>
    /// PricingController
    /// </summary>
    public class PriceController : ProductController, IPriceController
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PriceController(string storeKey) : base(storeKey)
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// GetAllProductPrice gets Product Prices based on Trade Agreemnts. 
        /// </summary>
        /// <returns></returns>
        public List<ErpProduct> GetAllProductPrice()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpProduct> erpProducts = new List<ErpProduct>();
            try
            {
                bool defaultPricingFlow = true;

                if (defaultPricingFlow)
                {
                    //TODO: has to be tested.
                    erpProducts = base.GetAllProducts(false, null, true);
                }
                else
                {
                    //Temp comment
                    //NS: TODO
                    //erpProducts = base.GetAllAssortmentProducts();
                }

                if (erpProducts != null && erpProducts.Count > 0)
                {
                    List<long> catalogIds = erpProducts.Select(prod => prod.CatalogId).Distinct().ToList();
                    foreach (long catId in catalogIds)
                    {

                        this.ProcessProductPrice(1L, catId, erpProducts); // dummy channel ID - should be in CRT layer
                    }
                    catalogIds.Clear();
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

        public List<ErpProductPrice> GetAllProductPriceExtension()
        {
            throw new NotImplementedException();
        }

        public List<ErpProductPrice> GetDefaultProductPrice()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpProductPrice> erpProductPrices = new List<ErpProductPrice>();
            try
            {
                var masterProducts = this.GetCatalogProducts(false, null, false);
                var allProducts = this.ProcessProducts(masterProducts);

                //NS: As TeamViewer is not using fully qulified SKU so no need of SKUs generation
                //if (ConfigurationManager.AppSettings["EcomAdapter"] == ApplicationConstant.ECOM_MAGENTO_ADAPTER_ASSEMBLY)
                //{
                //    this.GenerateSKUs(allProducts);
                //}            

                if (allProducts == null || allProducts.Count < 0)
                {
                    CustomLogger.LogWarn("Found no Catalog/CRT Products while fetching Prices", currentStore.StoreId, currentStore.CreatedBy);
                }
                if (allProducts != null && allProducts.Count > 0)
                {
                    erpProductPrices = this.ProcessProductPrice(allProducts);
                }
            }
            catch (Exception exp)
            {

                throw;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpProductPrices;
        }

        public ErpProductPrice GetProductPriceExtension(string ErpKey)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods
        ///// <summary>
        ///// ProcessProductPrice get and update Product Prices based on Trade Agreemnts.
        ///// </summary>
        ///// <param name="channelId"></param>
        ///// <param name="catalogId"></param>
        ///// <param name="erpProducts"></param>
        //private List<ErpProductPrice> ProcessProductPrice(long channelId, long catalogId, List<ErpProduct> erpProducts)
        //{
        //    List<ErpProductPrice> productCatalogPrices;
        //    var context = new ProjectionDomain(Utility._ChannelID, catalogId);
        //    List<long> productIds = erpProducts.Select(prod=>prod.RecordId).ToList();

        //    // Get Product Trade Agreement Prices
        //    IReadOnlyCollection<ProductPrice> prices = this.currentChannelState.ProductManager.GetActiveProductPrice(context, productIds, DateTime.UtcNow, this.currentChannelState.OnlineChannelInstance.DefaultCustomerAccount);
        //    if (prices.Count > 0)
        //    {
        //        productCatalogPrices = (from prod in erpProducts
        //                                join price in prices on prod.RecordId equals price.ProductId
        //                                where prod.CatalogId == catalogId
        //                                select new ErpProductPrice
        //                                {
        //                                    ProductId = prod.RecordId,
        //                                    SKU = prod.SKU,
        //                                    AdjustedPrice = price.AdjustedPrice,
        //                                    BasePrice = price.BasePrice,
        //                                    TradeAgreementPrice = price.TradeAgreementPrice
        //                                }).ToList();
        //    }
        //    else
        //    {
        //        productCatalogPrices = new List<ErpProductPrice>();
        //    }
        //    productIds.Clear();
        //    return productCatalogPrices;
        //}

        /// <summary>
        /// ProcessProductPrice get and update Product Prices based on Trade Agreemnts.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="catalogId"></param>
        /// <param name="erpProducts"></param>
        private void ProcessProductPrice(long channelId, long catalogId, List<ErpProduct> erpProducts)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            try
            {
                ErpProductPrice productPrice;
                //++US TODO
               // List<long> testExclude = new List<long> { 22565422010, 22565422297, 22565422308, 22565422309, 22565422310, 22565429485, 22565429486, 22565429502, 22565437740 };
                //List<long> productIds = erpProducts.Select(prod => prod.RecordId).Where(p=>!testExclude.Contains(p)).ToList();
                List<long> productIds = erpProducts.Select(prod => prod.RecordId).Distinct().ToList();
                // KAR - IJZ to validate this logic
                List<ErpProductPrice> prices = new List<ErpProductPrice>();

                // Stopwatch sw = Stopwatch.StartNew();
                //int pages = (int)Math.Ceiling((double)productIds.Count / PagingInfo.MaximumPageSize);


                //Parallel.For(0, pages, page =>
                //{
                //    int startIndex = page * PagingInfo.MaximumPageSize;
                //    int count = PagingInfo.MaximumPageSize;

                //    if (startIndex + count > productIds.Count)
                //    {
                //        count = productIds.Count - (page * PagingInfo.MaximumPageSize);
                //    }

                //List<long> pds = productIds.GetRange(startIndex, count);

                var crtPriceManager = new PriceCRTManager();
                List<ErpProductPrice> pricePage = crtPriceManager.GetActiveProductPrice(channelId, catalogId, productIds, DateTime.UtcNow, string.Empty, currentStore.StoreKey); //passing empty string for all customers, if needed default customer then should add New Key in application settings table
                prices.AddRange(pricePage);

                //NS: Remove
                //NS: Comment Start
                /*
                ReadOnlyCollection<ErpProductPrice> pricePage = this.currentChannelState.ProductManager.GetActiveProductPrice(
                    productIds, DateTime.UtcNow,
                    this.currentChannelState.OnlineChannelInstance.DefaultCustomerAccount,
                    new QueryResultSettings(PagingInfo.AllRecords)).Results;
                prices.AddRange(pricePage);
                */
                //NS: Comment End

                //    lock (this)
                //    {
                //        prices.AddRange(pricePage);
                //    }
                //});
                //sw.Stop();


                foreach (var erpProd in erpProducts)
                {
                    productPrice = prices.FirstOrDefault(pr => pr.ProductId == erpProd.RecordId);

                    if (productPrice != null)
                    {
                        erpProd.AdjustedPrice = productPrice.AdjustedPrice;
                        erpProd.BasePrice = productPrice.BasePrice;
                        erpProd.Price = productPrice.TradeAgreementPrice;
                    }
                }

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            }
            catch (Exception ex)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                throw ex;
            }
        }

        /// <summary>
        /// Default Price method
        /// </summary>
        /// <param name="erpProducts"></param>
        /// <returns></returns>
        private List<ErpProductPrice> ProcessProductPrice(List<ErpProduct> erpProducts)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpProducts));

            List<ErpProductPrice> erpPrices = new List<ErpProductPrice>();
            StringBuilder priceTrace = new StringBuilder();
            try
            {
                ErpProductPrice productPrice = null;
                List<long> productIds = erpProducts.Select(prod => prod.RecordId).Distinct().ToList();
                List<ErpProductPrice> prices = new List<ErpProductPrice>();

                long catalogId = 0;
                //if(erpProducts.Count > 0)
                //{
                //    catalogId = erpProducts[0].CatalogId;
                //}

                var crtPriceManager = new PriceCRTManager();
                List<ErpProductPrice> pricePage = crtPriceManager.GetActiveProductPrice(configurationHelper.GetSetting(APPLICATION.Channel_Id).LongValue(), catalogId, productIds, DateTime.UtcNow, string.Empty, currentStore.StoreKey);
                prices.AddRange(pricePage);

                //NS: Remove
                //NS: Comment Start
                /*
                ReadOnlyCollection<ErpProductPrice> pricePage = this.currentChannelState.ProductManager.GetActiveProductPrice(
                    productIds, DateTime.UtcNow,
                    this.currentChannelState.OnlineChannelInstance.DefaultCustomerAccount,
                    new QueryResultSettings(PagingInfo.AllRecords)).Results;
                prices.AddRange(pricePage);
                */
                //NS: Comment End
                bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();

                if (isFlatProductHierarchy)
                {
                    erpProducts = erpProducts.Where(d => d.IsMasterProduct == false).ToList();
                    // REMOVE THIS LINE // CustomLogger.LogDebugInfo(string.Format("Fetched products with IsMasterProduct == false in case of Flat Product HIerarchy"));
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10210, currentStore);
                }

                foreach (var erpProd in erpProducts)
                {
                    if (erpProd.IsMasterProduct == false)
                    {
                        productPrice = prices.FirstOrDefault(pr => pr.ProductId == erpProd.RecordId);
                        if (productPrice != null)
                        {
                            ErpProductPrice erpProdPrice = new ErpProductPrice();
                            if (!isFlatProductHierarchy)
                            {
                                erpProdPrice.SKU = erpProd.IsMasterProduct ? erpProd.ItemId : erpProd.VariantId;
                                erpProdPrice.SKU = configurationHelper.GetSetting(PRODUCT.SKU_Prefix) + erpProdPrice.SKU;
                            }
                            else
                            {
                                base.PaddingZeros(erpProd);
                                erpProdPrice.SKU = erpProd.SKU;
                            }

                            if (ConfigurationManager.AppSettings["EcomAdapter"] == ApplicationConstant.ECOM_MAGENTO_ADAPTER_ASSEMBLY)
                            {
                                erpProdPrice.SKU = erpProd.SKU;
                            }

                            erpProdPrice.ItemId = productPrice.ItemId;
                            erpProdPrice.InventoryDimensionId = productPrice.InventoryDimensionId;
                            erpProdPrice.RetailVariantId = productPrice.IsVariantPrice ? productPrice.ItemId : string.Empty;

                            erpProdPrice.AdjustedPrice = productPrice.AdjustedPrice;
                            if (productPrice.TradeAgreementPrice > productPrice.BasePrice || productPrice.TradeAgreementPrice < productPrice.BasePrice)
                            {

                                erpProdPrice.BasePrice = productPrice.TradeAgreementPrice;
                                // REMOVE THIS LINE // priceTrace.Append(string.Format("Product Price as Trade AgreementPrice: {0} for Product :{1}", erpProdPrice.TradeAgreementPrice, erpProdPrice.ItemId));
                                string.Format(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10211), erpProdPrice.TradeAgreementPrice, erpProdPrice.ItemId));
                            }
                            else
                            {
                                erpProdPrice.BasePrice = productPrice.BasePrice;
                                // REMOVE THIS LINE // priceTrace.Append(string.Format("Product Price as Base Price: {0} for Product :{1}", erpProdPrice.BasePrice, erpProdPrice.ItemId));
                                string.Format(string.Format(CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL10212), erpProdPrice.BasePrice, erpProdPrice.ItemId));
                            }

                            erpProdPrice.TradeAgreementPrice = productPrice.TradeAgreementPrice;
                            erpProdPrice.ValidFrom = productPrice.ValidFrom.Date;
                            erpProdPrice.ValidTo = DateTime.UtcNow.AddYears(100);
                            erpProdPrice.Quantity = 1;
                            erpProdPrice.SizeId = productPrice.SizeId;
                            erpProdPrice.ColorId = erpProdPrice.ColorId;

                            //NS: TMV Customization for Ecom
                            erpProdPrice.TMV_ProductType = erpProd.IsMasterProduct ?
                                configurationHelper.GetSetting(PRODUCT.MasterProductTypeEcomName) :
                                configurationHelper.GetSetting(PRODUCT.VariantProductTypeEcomName);

                            erpPrices.Add(erpProdPrice);

                        }
                    }
                }

                CustomLogger.LogDebugInfo(priceTrace.ToString(), currentStore.StoreId, currentStore.CreatedBy);
            }
            catch (Exception ex)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name,  JsonConvert.SerializeObject(ex));
                throw ex;
            }
            return erpPrices;
        }

        private void GenerateSKUs(List<ErpProduct> erpProducts)
        {
            CustomLogger.LogDebugInfo(string.Format("Enter in GenerateSKUs()"), currentStore.StoreId, currentStore.CreatedBy);
            foreach (ErpProduct prod in erpProducts)
            {
                base.PaddingZeros(prod);
                prod.SKU = prod.EcomProductId = prod.MasterProductNumber + "_" + prod.ColorId + "_" + prod.SizeId + "_" + prod.StyleId + "_" + prod.ItemId;

            }
            CustomLogger.LogDebugInfo(string.Format("Exit from GenerateSKUs()"), currentStore.StoreId, currentStore.CreatedBy);
        }
        #endregion
    }
}

#region IJZ code
/*
using Microsoft.Dynamics.Commerce.Runtime;
using Microsoft.Dynamics.Commerce.Runtime.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.AXCommon;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EdgeCommerceConnector.adptAX2012R3;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    /// <summary>
    /// PricingController
    /// </summary>
    public class PriceController : ProductBaseController, IPriceController
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PriceController()
        {
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// GetAllProductPrice gets Product Prices based on Trade Agreemnts. 
        /// </summary>
        /// <returns></returns>
        public List<ErpProduct> GetAllProductPrice()
        {
            List<ErpProduct> erpProducts;
            try
            {
                ProductController productController = new ProductController();
                erpProducts = base.GetAllAssortmentProducts();

                if (erpProducts != null && erpProducts.Count > 0)
                {
                    List<long> catalogIds = erpProducts.Select(prod => prod.CatalogId).Distinct().ToList();
                    foreach (long catId in catalogIds)
                    {
                        this.ProcessProductPrice(CommerceRuntimeHelper.ChannelId, catId, erpProducts);
                    }
                    catalogIds.Clear();
                }
            }
            catch (Exception exp)
            {
                //CustomLogger.LogException(exp);
                throw;
            }
            return erpProducts;
        }
        #endregion

        #region Private Methods
        ///// <summary>
        ///// ProcessProductPrice get and update Product Prices based on Trade Agreemnts.
        ///// </summary>
        ///// <param name="channelId"></param>
        ///// <param name="catalogId"></param>
        ///// <param name="erpProducts"></param>
        //private List<ErpProductPrice> ProcessProductPrice(long channelId, long catalogId, List<ErpProduct> erpProducts)
        //{
        //    List<ErpProductPrice> productCatalogPrices;
        //    var context = new ProjectionDomain(Utility._ChannelID, catalogId);
        //    List<long> productIds = erpProducts.Select(prod=>prod.RecordId).ToList();

        //    // Get Product Trade Agreement Prices
        //    IReadOnlyCollection<ProductPrice> prices = this.currentChannelState.ProductManager.GetActiveProductPrice(context, productIds, DateTime.UtcNow, this.currentChannelState.OnlineChannelInstance.DefaultCustomerAccount);
        //    if (prices.Count > 0)
        //    {
        //        productCatalogPrices = (from prod in erpProducts
        //                                join price in prices on prod.RecordId equals price.ProductId
        //                                where prod.CatalogId == catalogId
        //                                select new ErpProductPrice
        //                                {
        //                                    ProductId = prod.RecordId,
        //                                    SKU = prod.SKU,
        //                                    AdjustedPrice = price.AdjustedPrice,
        //                                    BasePrice = price.BasePrice,
        //                                    TradeAgreementPrice = price.TradeAgreementPrice
        //                                }).ToList();
        //    }
        //    else
        //    {
        //        productCatalogPrices = new List<ErpProductPrice>();
        //    }
        //    productIds.Clear();
        //    return productCatalogPrices;
        //}

        /// <summary>
        /// ProcessProductPrice get and update Product Prices based on Trade Agreemnts.
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="catalogId"></param>
        /// <param name="erpProducts"></param>
        private void ProcessProductPrice(long channelId, long catalogId, List<ErpProduct> erpProducts)
        {
            ProductPrice productPrice;
            //List<long> testExclude = new List<long>{5637154335,  5637154336, 5637154337, 5637154338, 5637154339, 5637155076, 5637155077, 5637155078, 5637155826, 5637155827, 5637155828, 5637158828, 5637159577, 5637159578, 5637159579,5637159580, 5637159581,5637159582};
            //List<long> productIds = erpProducts.Select(prod => prod.RecordId).Where(p=>!testExclude.Contains(p)).ToList();
            List<long> productIds = erpProducts.Select(prod => prod.RecordId).Distinct().ToList();

            Stopwatch sw = Stopwatch.StartNew();
            List<ProductPrice> prices = new List<ProductPrice>();
            bool priceChuncks = false;

            if (priceChuncks)
            {
                int pages = (int)Math.Ceiling((double)productIds.Count / PagingInfo.PageSizeHasNoLimit);

                Parallel.For(0, pages, page =>
                {
                    int startIndex = page * PagingInfo.PageSizeHasNoLimit;
                    int count = PagingInfo.PageSizeHasNoLimit;

                    if (startIndex + count > productIds.Count)
                    {
                        count = productIds.Count - (page * PagingInfo.PageSizeHasNoLimit);
                    }

                    List<long> pds = productIds.GetRange(startIndex, count);

                    //lock (this)
                    //{
                    //    pds = productIds.GetRange(startIndex, count);
                    //}

                    QueryResultSettings qrs = new QueryResultSettings(new PagingInfo(PagingInfo.PageSizeHasNoLimit));

                    PagedResult<ProductPrice> pricePage = this.currentChannelState.ProductManager.GetActiveProductPrice(pds, DateTime.UtcNow, this.currentChannelState.OnlineChannelInstance.DefaultCustomerAccount, qrs);

                    lock (this)
                    {
                        prices.AddRange(pricePage.Results);
                    }
                });
            }
            else
            {
                QueryResultSettings qrs = new QueryResultSettings(new PagingInfo(PagingInfo.PageSizeHasNoLimit));

                PagedResult<ProductPrice> pricePage = this.currentChannelState.ProductManager.GetActiveProductPrice(productIds, DateTime.UtcNow, this.currentChannelState.OnlineChannelInstance.DefaultCustomerAccount, qrs);

                lock (this)
                {
                    prices.AddRange(pricePage.Results);
                }
            }
            sw.Stop();


            foreach (var erpProd in erpProducts)
            {
                productPrice = prices.FirstOrDefault(pr => pr.ProductId == erpProd.RecordId);

                if (productPrice != null)
                {
                    erpProd.AdjustedPrice = productPrice.AdjustedPrice;
                    erpProd.BasePrice = productPrice.BasePrice;
                    erpProd.Price = productPrice.TradeAgreementPrice;
                }
            }
        }
        #endregion
    }
}

*/
#endregion