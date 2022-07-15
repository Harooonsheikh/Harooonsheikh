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

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class QuantityDiscountController : BaseController, IQuantityDiscountController
    {
        StringBuilder discountTrace = new StringBuilder();
        public QuantityDiscountController(string storeKey) : base(storeKey)
        {

        }
        public List<ErpProductQuantityDiscount> GetQuantityDiscount()
        {
            List<ErpRetailQuantityDiscountItem> erpRetailDiscountItems = GetQuantityDiscountItem();
            List<ErpProductQuantityDiscount> erpProductQuantityDiscountList = GetQuantityDiscountList(erpRetailDiscountItems);
            return erpProductQuantityDiscountList;
        }
        private List<ErpProductQuantityDiscount> GetQuantityDiscountList(List<ErpRetailQuantityDiscountItem> erpRetailDiscountItems)
        {
            List<ErpProductQuantityDiscount> erpQuantityDiscountList = new List<ErpProductQuantityDiscount>();
            List<String> offerIdList = new List<string>();
            List<decimal> minimumQuantityList = new List<decimal>();
            List<long> variantList = new List<long>();
            List<long> categoriesList = new List<long>();
            List<long> productsList = new List<long>();
            foreach (ErpRetailQuantityDiscountItem erpRetailDiscountItem in erpRetailDiscountItems)
            {
                if (offerIdList.Contains(erpRetailDiscountItem.OfferId) == false)
                {
                    #region New Offer Id 
                    ErpProductQuantityDiscount erpQuantityDiscount = new ErpProductQuantityDiscount();
                    erpQuantityDiscount.OfferId = erpRetailDiscountItem.OfferId;
                    erpQuantityDiscount.Name = erpRetailDiscountItem.Name;
                    erpQuantityDiscount.CurrencyCode = erpRetailDiscountItem.CurrencyCode;
                    erpQuantityDiscount.ConcurrencyMode = erpRetailDiscountItem.ConcurrencyMode;
                    erpQuantityDiscount.MultiBuyDiscountType = (ErpMultiBuyDiscountType)Enum.Parse(typeof(ErpMultiBuyDiscountType), erpRetailDiscountItem.MultiBuyDiscountType);
                    erpQuantityDiscount.ValidFrom = erpRetailDiscountItem.ValidFrom;
                    erpQuantityDiscount.ValidTo = erpRetailDiscountItem.ValidTo;
                    erpQuantityDiscount.PeriodicDiscountType = erpRetailDiscountItem.PeriodicDiscountType;

                    erpQuantityDiscount.QuantityDiscountConfiguration = new List<ErpQuantityDiscountConfiguration>();
                    ErpQuantityDiscountConfiguration erpQuantityDiscountConfiguration = new ErpQuantityDiscountConfiguration();
                    erpQuantityDiscountConfiguration.MinimumQuantity = erpRetailDiscountItem.QtyLowest;
                    erpQuantityDiscountConfiguration.UnitPriceOrDiscountPercentage = erpRetailDiscountItem.PriceDiscPct;
                    erpQuantityDiscount.QuantityDiscountConfiguration.Add(erpQuantityDiscountConfiguration);

                    List<long> categoriesListToAttach = new List<long>();
                    categoriesListToAttach.Add(erpRetailDiscountItem.Category);
                    erpQuantityDiscount.Categories = categoriesListToAttach;

                    List<long> productsListToAttach = new List<long>();
                    Dictionary<long, List<long>> productVariant = new Dictionary<long, List<long>>();
                    if (erpRetailDiscountItem.Product != 0)
                    {
                        productsListToAttach.Add(erpRetailDiscountItem.Product);
                        erpQuantityDiscount.Products = productsListToAttach;

                        productVariant.Add(erpRetailDiscountItem.Product, new List<long>());
                    }

                    List<long> variantListToAttach = new List<long>();
                    if (erpRetailDiscountItem.Variant != 0)
                    {
                        variantListToAttach.Add(erpRetailDiscountItem.Variant);
                        erpQuantityDiscount.Variants = variantListToAttach;

                        List<long> variantsList = new List<long>();
                        productVariant.TryGetValue(erpRetailDiscountItem.Product, out variantsList);
                        variantsList.Add(erpRetailDiscountItem.Variant);
                        variantList.Add(erpRetailDiscountItem.Variant);
                    }

                    erpQuantityDiscount.CategoryProduct = new Dictionary<long, List<long>>();
                    if (erpRetailDiscountItem.Product != 0)
                    {
                        List<long> productCategoryList = new List<long>();
                        productCategoryList.Add(erpRetailDiscountItem.Product);

                        erpQuantityDiscount.CategoryProduct.Add(erpRetailDiscountItem.Category, productCategoryList);
                    }
                    else
                    {
                        erpQuantityDiscount.CategoryProduct.Add(erpRetailDiscountItem.Category, new List<long>());
                    }

                    erpQuantityDiscount.CategoryProductVariant = new Dictionary<long, Dictionary<long, List<long>>>();
                    erpQuantityDiscount.CategoryProductVariant.Add(erpRetailDiscountItem.Category, productVariant);

                    erpQuantityDiscountList.Add(erpQuantityDiscount);

                    #region clear and add local lists
                    offerIdList.Add(erpRetailDiscountItem.OfferId);
                    minimumQuantityList.Clear();
                    minimumQuantityList.Add(erpQuantityDiscountConfiguration.MinimumQuantity);

                    categoriesList.Clear();
                    categoriesList.Add(erpRetailDiscountItem.Category);

                    productsList.Clear();
                    if (erpRetailDiscountItem.Product != 0)
                    {
                        productsList.Add(erpRetailDiscountItem.Product);
                    }

                    variantList.Clear();
                    if (erpRetailDiscountItem.Variant != 0)
                    {
                        variantList.Add(erpRetailDiscountItem.Variant);
                    }
                    #endregion clear and add local lists

                    #endregion
                }
                else
                {
                    #region Existing OfferId

                    Dictionary<long, List<long>> productVariant = new Dictionary<long, List<long>>();

                    if (minimumQuantityList.Contains(erpRetailDiscountItem.QtyLowest) == false)
                    {
                        ErpQuantityDiscountConfiguration erpQuantityDiscountConfiguration = new ErpQuantityDiscountConfiguration();
                        erpQuantityDiscountConfiguration.MinimumQuantity = erpRetailDiscountItem.QtyLowest;
                        erpQuantityDiscountConfiguration.UnitPriceOrDiscountPercentage = erpRetailDiscountItem.PriceDiscPct;

                        erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].QuantityDiscountConfiguration.Add(erpQuantityDiscountConfiguration);
                        minimumQuantityList.Add(erpQuantityDiscountConfiguration.MinimumQuantity);
                    }

                    if (categoriesList.Contains(erpRetailDiscountItem.Category) == false)
                    {
                        #region if new CategorId
                        erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].Categories.Add(erpRetailDiscountItem.Category);
                        categoriesList.Add(erpRetailDiscountItem.Category);

                        if (erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].CategoryProduct == null)
                        {
                            erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].CategoryProduct = new Dictionary<long, List<long>>();
                        }

                        if (erpRetailDiscountItem.Product != 0)
                        {
                            List<long> productCategoryList = new List<long>();
                            productCategoryList.Add(erpRetailDiscountItem.Product);

                            erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].CategoryProduct.Add(erpRetailDiscountItem.Category, productCategoryList);

                            productVariant.Add(erpRetailDiscountItem.Product, new List<long>());
                        }
                        else
                        {
                            erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].CategoryProduct.Add(erpRetailDiscountItem.Category, new List<long>());
                        }

                        if (erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].CategoryProductVariant == null)
                        {
                            erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].CategoryProductVariant = new Dictionary<long, Dictionary<long, List<long>>>();
                        }

                        erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].CategoryProductVariant.Add(erpRetailDiscountItem.Category, productVariant);
                        #endregion
                    }
                    else
                    {
                        #region if existing CategoryId

                        List<long> categoryProductList = new List<long>();
                        erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].CategoryProduct.TryGetValue(erpRetailDiscountItem.Category, out categoryProductList);
                        if (erpRetailDiscountItem.Product != 0 && categoryProductList.Contains(erpRetailDiscountItem.Product) == false)
                        {
                            categoryProductList.Add(erpRetailDiscountItem.Product);
                        }

                        erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].CategoryProductVariant.TryGetValue(erpRetailDiscountItem.Category, out productVariant);
                        if (erpRetailDiscountItem.Product != 0 && productVariant.ContainsKey(erpRetailDiscountItem.Product) == false)
                        {
                            productVariant.Add(erpRetailDiscountItem.Product, new List<long>());
                        }
                        #endregion
                    }

                    if (productsList.Contains(erpRetailDiscountItem.Product) == false)
                    {
                        if (erpRetailDiscountItem.Product != 0)
                        {
                            erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].Products.Add(erpRetailDiscountItem.Product);
                            productsList.Add(erpRetailDiscountItem.Product);
                        }
                    }

                    if (variantList.Contains(erpRetailDiscountItem.Variant) == false)
                    {
                        if (erpRetailDiscountItem.Variant != 0)
                        {
                            if (erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].Variants == null)
                            {
                                erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].Variants = new List<long>();
                            }
                            erpQuantityDiscountList[erpQuantityDiscountList.Count - 1].Variants.Add(erpRetailDiscountItem.Variant);
                            variantList.Add(erpRetailDiscountItem.Variant);

                            List<long> variantsList = new List<long>();
                            productVariant.TryGetValue(erpRetailDiscountItem.Product, out variantsList);
                            if (variantsList.Contains(erpRetailDiscountItem.Variant) == false)
                            {
                                variantsList.Add(erpRetailDiscountItem.Variant);
                            }
                        }
                    }

                    #endregion
                }
            }
            return erpQuantityDiscountList;
        }
        private List<ErpRetailQuantityDiscountItem> GetQuantityDiscountItem()
        {
            List<ErpRetailQuantityDiscountItem> erpRetailDiscountItems = null;
            var rsResponse = ECL_GetRetailQuantityDiscountItems();
            if ((bool)rsResponse.Status)
            {
                var discountItems = JsonConvert.DeserializeObject<List<RetailQuantityDiscountItem>>(rsResponse.Result);
                erpRetailDiscountItems =
                    _mapper.Map<List<RetailQuantityDiscountItem>, List<ErpRetailQuantityDiscountItem>>(discountItems) ??
                    new List<ErpRetailQuantityDiscountItem>();
            }
            else
            {
                string message = CommerceLinkLogger.LogWarning(CommerceLinkLoggerMessages.VSICL30000, currentStore, MethodBase.GetCurrentMethod().Name, rsResponse.Message);
            }
            return erpRetailDiscountItems;
        }
        #region RetailServer API
        private GetRetailQuantityDiscountResponse ECL_GetRetailQuantityDiscountItems()
        {
            IRetailQuantityDiscountItemManager retailQuantityDiscountItemManager =
                RPFactory.GetManager<IRetailQuantityDiscountItemManager>();
            var rsResponse = Task.Run(async () =>
                await retailQuantityDiscountItemManager.ECL_GetRetailQuantityDiscountItems(baseCompany, ChannelCurrencyCode,
                    baseChannelId)).Result;
            return rsResponse;
        }
        #endregion
    }
}
