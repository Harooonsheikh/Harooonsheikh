using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using NewRelic.Api.Agent;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class QuantityDiscountWithAffiliationController : BaseController, IQuantityDiscountWithAffiliationController
    {
        StringBuilder discountTrace = new StringBuilder();
        public QuantityDiscountWithAffiliationController(string storeKey) : base(storeKey)
        {

        }
        public List<ErpProductQuantityDiscountWithAffiliation> GetQuantityDiscountWithAffiliation()
        {
            List<ErpRetailQuantityDiscountWithAffiliationItem> erpRetailQuantityDiscountWithAffilaitionItems = GetQuantityDiscountWithAffiliationItem();

            List<ErpProductQuantityDiscountWithAffiliation> erpProductQuantityDiscountWithAffiliationList = GetQuantityDiscountWithAffiliationList(erpRetailQuantityDiscountWithAffilaitionItems);

            return erpProductQuantityDiscountWithAffiliationList;

        }
        private List<ErpProductQuantityDiscountWithAffiliation> GetQuantityDiscountWithAffiliationList(List<ErpRetailQuantityDiscountWithAffiliationItem> erpRetailDiscountWithAffiliationItems)
        {
            List<ErpProductQuantityDiscountWithAffiliation> erpQuantityDiscountWithAffiliationList = new List<ErpProductQuantityDiscountWithAffiliation>();
            List<String> offerIdList = new List<string>();
            List<decimal> minimumQuantityList = new List<decimal>();
            List<long> variantList = new List<long>();
            List<long> categoriesList = new List<long>();
            List<long> productsList = new List<long>();

            foreach (ErpRetailQuantityDiscountWithAffiliationItem erpRetailDiscountWithAffiliationItem in erpRetailDiscountWithAffiliationItems)
            {
                if (offerIdList.Contains(erpRetailDiscountWithAffiliationItem.OfferId) == false)
                {
                    #region New Offer Id 
                    ErpProductQuantityDiscountWithAffiliation erpQuantityDiscountWithAffiliation = new ErpProductQuantityDiscountWithAffiliation();
                    erpQuantityDiscountWithAffiliation.OfferId = erpRetailDiscountWithAffiliationItem.OfferId;
                    erpQuantityDiscountWithAffiliation.Name = erpRetailDiscountWithAffiliationItem.Name;
                    erpQuantityDiscountWithAffiliation.CurrencyCode = erpRetailDiscountWithAffiliationItem.CurrencyCode;
                    erpQuantityDiscountWithAffiliation.ConcurrencyMode = erpRetailDiscountWithAffiliationItem.ConcurrencyMode;
                    erpQuantityDiscountWithAffiliation.MultiBuyDiscountType = (ErpMultiBuyDiscountType)Enum.Parse(typeof(ErpMultiBuyDiscountType), erpRetailDiscountWithAffiliationItem.MultiBuyDiscountType);
                    erpQuantityDiscountWithAffiliation.ValidFrom = erpRetailDiscountWithAffiliationItem.ValidFrom;
                    erpQuantityDiscountWithAffiliation.ValidTo = erpRetailDiscountWithAffiliationItem.ValidTo;
                    erpQuantityDiscountWithAffiliation.AffiliationId = (long)erpRetailDiscountWithAffiliationItem.AffiliationId;
                    erpQuantityDiscountWithAffiliation.AffiliationName = erpRetailDiscountWithAffiliationItem.AffiliationName;

                    erpQuantityDiscountWithAffiliation.QuantityDiscountConfiguration = new List<ErpQuantityDiscountConfiguration>();
                    ErpQuantityDiscountConfiguration erpQuantityDiscountConfiguration = new ErpQuantityDiscountConfiguration();
                    erpQuantityDiscountConfiguration.MinimumQuantity = erpRetailDiscountWithAffiliationItem.QtyLowest;
                    erpQuantityDiscountConfiguration.UnitPriceOrDiscountPercentage = erpRetailDiscountWithAffiliationItem.PriceDiscPct;
                    erpQuantityDiscountWithAffiliation.QuantityDiscountConfiguration.Add(erpQuantityDiscountConfiguration);

                    List<long> categoriesListToAttach = new List<long>();
                    categoriesListToAttach.Add(erpRetailDiscountWithAffiliationItem.Category);
                    erpQuantityDiscountWithAffiliation.Categories = categoriesListToAttach;

                    List<long> productsListToAttach = new List<long>();
                    Dictionary<long, List<long>> productVariant = new Dictionary<long, List<long>>();
                    if (erpRetailDiscountWithAffiliationItem.Product != 0)
                    {
                        productsListToAttach.Add(erpRetailDiscountWithAffiliationItem.Product);
                        erpQuantityDiscountWithAffiliation.Products = productsListToAttach;

                        productVariant.Add(erpRetailDiscountWithAffiliationItem.Product, new List<long>());
                    }

                    List<long> variantListToAttach = new List<long>();
                    if (erpRetailDiscountWithAffiliationItem.Variant != 0)
                    {
                        variantListToAttach.Add(erpRetailDiscountWithAffiliationItem.Variant);
                        erpQuantityDiscountWithAffiliation.Variants = variantListToAttach;

                        List<long> variantsList = new List<long>();
                        productVariant.TryGetValue(erpRetailDiscountWithAffiliationItem.Product, out variantsList);
                        variantsList.Add(erpRetailDiscountWithAffiliationItem.Variant);
                        variantList.Add(erpRetailDiscountWithAffiliationItem.Variant);
                    }

                    erpQuantityDiscountWithAffiliation.CategoryProduct = new Dictionary<long, List<long>>();
                    if (erpRetailDiscountWithAffiliationItem.Product != 0)
                    {
                        List<long> productCategoryList = new List<long>();
                        productCategoryList.Add(erpRetailDiscountWithAffiliationItem.Product);

                        erpQuantityDiscountWithAffiliation.CategoryProduct.Add(erpRetailDiscountWithAffiliationItem.Category, productCategoryList);
                    }
                    else
                    {
                        erpQuantityDiscountWithAffiliation.CategoryProduct.Add(erpRetailDiscountWithAffiliationItem.Category, new List<long>());
                    }

                    erpQuantityDiscountWithAffiliation.CategoryProductVariant = new Dictionary<long, Dictionary<long, List<long>>>();
                    erpQuantityDiscountWithAffiliation.CategoryProductVariant.Add(erpRetailDiscountWithAffiliationItem.Category, productVariant);

                    erpQuantityDiscountWithAffiliationList.Add(erpQuantityDiscountWithAffiliation);

                    #region clear and add local lists
                    offerIdList.Add(erpRetailDiscountWithAffiliationItem.OfferId);
                    minimumQuantityList.Clear();
                    minimumQuantityList.Add(erpQuantityDiscountConfiguration.MinimumQuantity);

                    categoriesList.Clear();
                    categoriesList.Add(erpRetailDiscountWithAffiliationItem.Category);

                    productsList.Clear();
                    if (erpRetailDiscountWithAffiliationItem.Product != 0)
                    {
                        productsList.Add(erpRetailDiscountWithAffiliationItem.Product);
                    }

                    variantList.Clear();
                    if (erpRetailDiscountWithAffiliationItem.Variant != 0)
                    {
                        variantList.Add(erpRetailDiscountWithAffiliationItem.Variant);
                    }
                    #endregion clear and add local lists

                    #endregion
                }
                else
                {
                    #region Existing OfferId

                    Dictionary<long, List<long>> productVariant = new Dictionary<long, List<long>>();

                    if (minimumQuantityList.Contains(erpRetailDiscountWithAffiliationItem.QtyLowest) == false)
                    {
                        ErpQuantityDiscountConfiguration erpQuantityDiscountConfiguration = new ErpQuantityDiscountConfiguration();
                        erpQuantityDiscountConfiguration.MinimumQuantity = erpRetailDiscountWithAffiliationItem.QtyLowest;
                        erpQuantityDiscountConfiguration.UnitPriceOrDiscountPercentage = erpRetailDiscountWithAffiliationItem.PriceDiscPct;

                        erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].QuantityDiscountConfiguration.Add(erpQuantityDiscountConfiguration);
                        minimumQuantityList.Add(erpQuantityDiscountConfiguration.MinimumQuantity);
                    }

                    if (categoriesList.Contains(erpRetailDiscountWithAffiliationItem.Category) == false)
                    {
                        #region if new CategorId
                        erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].Categories.Add(erpRetailDiscountWithAffiliationItem.Category);
                        categoriesList.Add(erpRetailDiscountWithAffiliationItem.Category);

                        if (erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].CategoryProduct == null)
                        {
                            erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].CategoryProduct = new Dictionary<long, List<long>>();
                        }

                        if (erpRetailDiscountWithAffiliationItem.Product != 0)
                        {
                            List<long> productCategoryList = new List<long>();
                            productCategoryList.Add(erpRetailDiscountWithAffiliationItem.Product);

                            erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].CategoryProduct.Add(erpRetailDiscountWithAffiliationItem.Category, productCategoryList);

                            productVariant.Add(erpRetailDiscountWithAffiliationItem.Product, new List<long>());
                        }
                        else
                        {
                            erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].CategoryProduct.Add(erpRetailDiscountWithAffiliationItem.Category, new List<long>());
                        }

                        if (erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].CategoryProductVariant == null)
                        {
                            erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].CategoryProductVariant = new Dictionary<long, Dictionary<long, List<long>>>();
                        }

                        erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].CategoryProductVariant.Add(erpRetailDiscountWithAffiliationItem.Category, productVariant);
                        #endregion
                    }
                    else
                    {
                        #region if existing CategoryId

                        List<long> categoryProductList = new List<long>();
                        erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].CategoryProduct.TryGetValue(erpRetailDiscountWithAffiliationItem.Category, out categoryProductList);
                        if (erpRetailDiscountWithAffiliationItem.Product != 0 && categoryProductList.Contains(erpRetailDiscountWithAffiliationItem.Product) == false)
                        {
                            categoryProductList.Add(erpRetailDiscountWithAffiliationItem.Product);
                        }

                        erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].CategoryProductVariant.TryGetValue(erpRetailDiscountWithAffiliationItem.Category, out productVariant);
                        if (erpRetailDiscountWithAffiliationItem.Product != 0 && productVariant.ContainsKey(erpRetailDiscountWithAffiliationItem.Product) == false)
                        {
                            productVariant.Add(erpRetailDiscountWithAffiliationItem.Product, new List<long>());
                        }
                        #endregion
                    }

                    if (productsList.Contains(erpRetailDiscountWithAffiliationItem.Product) == false)
                    {
                        if (erpRetailDiscountWithAffiliationItem.Product != 0)
                        {
                            erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].Products.Add(erpRetailDiscountWithAffiliationItem.Product);
                            productsList.Add(erpRetailDiscountWithAffiliationItem.Product);
                        }
                    }

                    if (variantList.Contains(erpRetailDiscountWithAffiliationItem.Variant) == false)
                    {
                        if (erpRetailDiscountWithAffiliationItem.Variant != 0)
                        {
                            if (erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].Variants == null)
                            {
                                erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].Variants = new List<long>();
                            }
                            erpQuantityDiscountWithAffiliationList[erpQuantityDiscountWithAffiliationList.Count - 1].Variants.Add(erpRetailDiscountWithAffiliationItem.Variant);
                            variantList.Add(erpRetailDiscountWithAffiliationItem.Variant);

                            List<long> variantsList = new List<long>();
                            productVariant.TryGetValue(erpRetailDiscountWithAffiliationItem.Product, out variantsList);
                            if (variantsList.Contains(erpRetailDiscountWithAffiliationItem.Variant) == false)
                            {
                                variantsList.Add(erpRetailDiscountWithAffiliationItem.Variant);
                            }
                        }
                    }

                    #endregion
                }
            }

            return erpQuantityDiscountWithAffiliationList;
        }
        private List<ErpRetailQuantityDiscountWithAffiliationItem> GetQuantityDiscountWithAffiliationItem()
        {
            List<ErpRetailQuantityDiscountWithAffiliationItem> erpRetailDiscountWithAffiliationItems = new List<ErpRetailQuantityDiscountWithAffiliationItem>();
            var rsResponse = ECL_GetRetailQuantityDiscountWithAffiliationItems();

            if ((bool)rsResponse.Status)
            {
                var quantityDiscountWithAffiliationItems = JsonConvert.DeserializeObject<List<RetailQuantityDiscountWithAffiliationItem>>(rsResponse.Result);
                erpRetailDiscountWithAffiliationItems =
                    _mapper
                        .Map<List<RetailQuantityDiscountWithAffiliationItem>,
                            List<ErpRetailQuantityDiscountWithAffiliationItem>>(quantityDiscountWithAffiliationItems) ??
                    new List<ErpRetailQuantityDiscountWithAffiliationItem>();
            }
            else
            {
                string message = CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30000, currentStore, MethodBase.GetCurrentMethod().Name, rsResponse.Message);
            }

            return erpRetailDiscountWithAffiliationItems;
        }
        #region MyRegion
        [Trace]
        private GetRetailQuantityDiscountWithAffiliationResponse ECL_GetRetailQuantityDiscountWithAffiliationItems()
        {
            IRetailQuantityDiscountWithAffiliationItemManager retailQuantityDiscountWithAffiliationItemManager =
                RPFactory.GetManager<IRetailQuantityDiscountWithAffiliationItemManager>();
            var rsResponse = Task.Run(async () =>
                await retailQuantityDiscountWithAffiliationItemManager.ECL_GetRetailQuantityDiscountWithAffiliationItems(baseCompany, null, baseChannelId)).Result;
            return rsResponse;
        }
        #endregion
    }
}
