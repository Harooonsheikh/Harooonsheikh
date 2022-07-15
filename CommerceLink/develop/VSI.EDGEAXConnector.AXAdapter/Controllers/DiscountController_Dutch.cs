using Microsoft.Dynamics.Commerce.Runtime;
using Microsoft.Dynamics.Commerce.Runtime.Client;
using Microsoft.Dynamics.Commerce.Runtime.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EdgeCommerceConnector.adptAX2012R3;

using VSI.EDGEAXConnector.Common;

using VSI.Commerce.Runtime.Entities;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class DiscountController_Dutch : ProductBaseController, IDiscountController
    {
        bool IsCategoryLevelDiscount = false;
        List<ErpProduct> erpProducts = new List<ErpProduct>();
        IEnumerable<ProductDiscountExtension> ExclusiveDiscount;
        IEnumerable<ProductDiscountExtension> CurrentExclusiveDiscount;
        IEnumerable<ProductDiscountExtension> AlreadyAppliedExclusiveDiscount = Enumerable.Empty<ProductDiscountExtension>();
        List<string> xProcessedIds = new List<string>();
        List<long> xVariantsProcessedIds = new List<long>();
        List<string> processedCompProds = new List<string>();
        List<string> CompVariantsProcessedIds = new List<string>();

        public void ProcessExclusiveDiscount(IReadOnlyCollection<ProductDiscountExtension> producDiscountExtensions)
        {
            if (producDiscountExtensions != null && producDiscountExtensions.Count > 0)
            {
                foreach (var pde in producDiscountExtensions)
                {
                    if (pde.DISCAPPLICATIONUPON == "Variant")
                    {
                        long recId = pde.RECID;
                        if (!xVariantsProcessedIds.Any(x => x.Equals(recId))) //Make sure this variant is not already processed
                        {
                            var variant = erpProducts.FirstOrDefault(x => x.RecordId.Equals(recId));
                            ApplyExclusiveDiscount(variant, pde);
                            xVariantsProcessedIds.Add(recId);
                        }
                    }
                    else if (!xProcessedIds.Any(x => x.Equals(pde.ItemID)))
                    {
                        var products = erpProducts.Where(x => x.ItemId.Equals(pde.ItemID));
                        foreach (var product in products)
                        {
                            if (product != null && product.RecordId > 0 && !xVariantsProcessedIds.Any(x => x.Equals(product.RecordId))) // Check if this variant is not already processed
                            {
                                ApplyExclusiveDiscount(product, pde);
                            }
                        }
                        xProcessedIds.Add(pde.ItemID);
                    }
                }
            }
        }

        public void ProcessCompoundedDiscount(IReadOnlyCollection<ProductDiscountExtension> producDiscountExtensions)
        {
            if (producDiscountExtensions != null && producDiscountExtensions.Count > 0)
            {
                foreach (var pde in producDiscountExtensions)
                {


                    if (pde.DISCAPPLICATIONUPON == "Variant")
                    {
                        long recId = pde.RECID;
                        string vKey = pde.OFFERID + pde.ItemID + Convert.ToString(pde.RECID);
                        if (processedCompProds.IndexOf(vKey) < 0 && !xVariantsProcessedIds.Any(x => x.Equals(recId))) //Make sure this variant is not already processed for this compound and also never processed in Exlusive discounts
                        {
                            var variant = erpProducts.FirstOrDefault(x => x.RecordId.Equals(recId));
                            ApplyCompDiscount(variant, pde);
                            processedCompProds.Add(vKey);
                        }
                    }
                    else
                    {
                        var products = erpProducts.Where(x => x.ItemId.Equals(pde.ItemID));
                        foreach (var product in products)
                        {
                            string key = pde.OFFERID + pde.ItemID + product.RecordId.ToString();
                            if (product != null && product.RecordId > 0 && processedCompProds.IndexOf(key) < 0)
                            {
                                ApplyCompDiscount(product, pde);
                                processedCompProds.Add(key);
                            }
                        }
                    }
                }
            }
        }
        void ApplyExclusiveDiscount(ErpProduct product, ProductDiscountExtension pde)
        {
            if (product != null && pde != null)
            {
                if (!string.IsNullOrEmpty(pde.OFFERID) && pde.ValidFromDate != null && pde.ValidToDate != null
                    && (pde.DISCPCT > 0 || pde.OFFERPRICE > 0 || pde.DISCAMOUNT > 0))
                {
                    product.OfferId = pde.OFFERID;
                    product.ValidFromDate = Convert.ToDateTime(pde.ValidFromDate).ToString("yyyy-MM-dd 00:00:00");
                    product.ValidToDate = Convert.ToDateTime(pde.ValidToDate).ToString("yyyy-MM-dd 00:00:00");
                    if (pde.DISCPCT > 0) // percentage Discount 
                    { product.SpecialPrice = (decimal.Round(product.AdjustedPrice - ((pde.DISCPCT / 100) * product.AdjustedPrice), 2, MidpointRounding.AwayFromZero)).ToString(); }
                    else if (pde.OFFERPRICE > 0)
                    { product.SpecialPrice = (decimal.Round((pde.OFFERPRICE), 2, MidpointRounding.AwayFromZero)).ToString(); }
                    else if (pde.DISCAMOUNT > 0)
                    { product.SpecialPrice = (decimal.Round((product.AdjustedPrice - pde.DISCAMOUNT), 2, MidpointRounding.AwayFromZero)).ToString(); }
                }
            }
        }
        void ApplyCompDiscount(ErpProduct product, ProductDiscountExtension pde)
        {
            if (product != null && pde != null)
            {
                if (!string.IsNullOrEmpty(pde.OFFERID) && pde.ValidFromDate != null && pde.ValidToDate != null
                    && (pde.DISCPCT > 0 || pde.OFFERPRICE > 0 || pde.DISCAMOUNT > 0))
                {
                    product.OfferId = pde.OFFERID;
                    product.ValidFromDate = Convert.ToDateTime(pde.ValidFromDate).ToString("yyyy-MM-dd 00:00:00");
                    product.ValidToDate = Convert.ToDateTime(pde.ValidToDate).ToString("yyyy-MM-dd 00:00:00");
                    decimal price = string.IsNullOrEmpty(product.SpecialPrice) ? product.AdjustedPrice : Convert.ToDecimal(product.SpecialPrice);
                    if (pde.DISCPCT > 0) // percentage Discount 
                    {
                        product.SpecialPrice = (decimal.Round(price - ((pde.DISCPCT / 100) * price), 2, MidpointRounding.AwayFromZero)).ToString();
                    }
                    else if (pde.OFFERPRICE > 0)
                    {
                        product.SpecialPrice = (decimal.Round((pde.OFFERPRICE), 2, MidpointRounding.AwayFromZero)).ToString();
                    }
                    else if (pde.DISCAMOUNT > 0)
                    {
                        product.SpecialPrice = (decimal.Round((price - pde.DISCAMOUNT), 2, MidpointRounding.AwayFromZero)).ToString();
                    }
                }
            }
        }
         public List<ErpProduct> GetDiscounts()
        {return null;}
         /// <summary>
         /// Uncomment this function if you need to use this.
         /// </summary>
         /// <returns></returns>
 /*
        public List<ErpProduct> GetDiscounts()
        {
            string traceInfo = "Entered into GetDiscount() Function" + Environment.NewLine;
            //CustomLogger.LogTraceInfo("Entered into GetDiscount Function");
            xProcessedIds = new List<string>();
            processedCompProds = new List<string>();
            xVariantsProcessedIds = new List<long>();
            PriceController pc = new PriceController();
            try
            {
                erpProducts = pc.GetAllProductPrice();

                //var productJson = erpProducts.SerializeToJson(1);

                //using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(@"erpproducts.txt", true))
                //{
                //    streamWriter.Write(productJson.ToString());
                //}

                //erpProducts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ErpProduct>>(System.IO.File.ReadAllText(@"erpproducts.txt"));


                if (erpProducts != null && erpProducts.Count > 0)
                {

                    traceInfo += string.Format("All Products with price received. Count:{0}" + Environment.NewLine, erpProducts.Count);
                    //Get Products on Discount.
                    IReadOnlyCollection<ProductDiscountExtension> producDiscountExtensions = this.ProcessProductDiscountExtension(erpProducts);
                    traceInfo += string.Format("All Discounts reterieved. Count:{0}" + Environment.NewLine, producDiscountExtensions.Count);
                    //All Exclusive Discounts Items
                    var xDisc = producDiscountExtensions.Where(x => x.CONCURRENCYMODE == 0).ToList();
                    traceInfo += string.Format("Total Excludsive Discounts :{0}" + Environment.NewLine, xDisc.Count);
                    //Exclusive discount items Item level
                    var xDiscVariants = xDisc.Where(x => x.DISCAPPLICATIONUPON == "Variant").ToList();
                    traceInfo += string.Format("Variant Level Products Discounts :{0}" + Environment.NewLine, xDiscVariants.Count);
                    //Exclusive discount items Item level
                    var xDiscProds = xDisc.Where(x => x.DISCAPPLICATIONUPON == "Product").ToList();
                    traceInfo += string.Format("Product Level Excludsive Discounts :{0}" + Environment.NewLine, xDiscProds.Count);
                    //Exclusive discount Category Item level
                    var xDiscCats = xDisc.Where(x => x.DISCAPPLICATIONUPON == "Category").ToList();
                    traceInfo += string.Format("Category Level Excludsive Discounts :{0}" + Environment.NewLine, xDiscCats.Count);


                    //Exclude Product & Variant level Exclusive discounts from category level Exclusive discounts
                    #region Excluded Product level Exclusive discounts from category level ex discounts

                    var xDiscVariantLevelProductIds = xDiscVariants.Select(y => y.RECID);
                    xDiscCats = xDiscCats.Where(x => !xDiscVariantLevelProductIds.Contains(x.RECID)).ToList();
                    traceInfo += string.Format("Category Level Excludsive Discounts after excluding varaint level discount :{0}" + Environment.NewLine, xDiscCats.Count);

                    var xDiscProdsLevelProductIds = xDiscProds.Select(y => y.ItemID);
                    xDiscCats = xDiscCats.Where(x => !xDiscProdsLevelProductIds.Contains(x.ItemID)).ToList();
                    traceInfo += string.Format("Category Level Excludsive Discounts after excluding product level discount :{0}" + Environment.NewLine, xDiscCats.Count);


                    //All Compounded Discounts Items
                    var compDisc = producDiscountExtensions.Where(x => x.CONCURRENCYMODE == 2).ToList();
                    traceInfo += string.Format("Total Compounded Discounts :{0}" + Environment.NewLine, compDisc.Count);

                    // Also exclude the variant level eclusive discounts from compound discounts
                    compDisc = compDisc.Where(x => !xDiscVariantLevelProductIds.Contains(x.RECID)).ToList();
                    var xDiscProdsIds = xDisc.Select(y => y.ItemID);
                    compDisc = compDisc.Where(x => !xDiscProdsIds.Contains(x.ItemID)).ToList();
                    traceInfo += string.Format("Total Compunded Discounts after excluding Exclusive discounts :{0}" + Environment.NewLine, compDisc.Count);
                    #endregion

                    #region Categories Compund Discount in Product and Category

                    var compDiscVariants = compDisc.Where(x => x.DISCAPPLICATIONUPON == "Variant").ToList();
                    traceInfo += string.Format("Variant Level Compunded Discounts after excluding Exclusive discounts :{0}" + Environment.NewLine, compDiscVariants.Count);

                    var compDiscProds = compDisc.Where(x => x.DISCAPPLICATIONUPON == "Product").ToList();
                    traceInfo += string.Format("Product Level Compunded Discounts after excluding Exclusive discounts :{0}" + Environment.NewLine, compDiscProds.Count);

                    var compDiscCats = compDisc.Where(x => x.DISCAPPLICATIONUPON == "Category").ToList();
                    traceInfo += string.Format("Category Level Compunded Discounts after excluding Exclusive discounts :{0}" + Environment.NewLine, compDiscCats.Count);
                    #endregion

                    if (xDiscVariants != null && xDiscVariants.Count > 0)
                    {
                        ProcessExclusiveDiscount(xDiscVariants);
                        traceInfo += "Exclusive Variant Level Discounts calculated." + Environment.NewLine;
                    }
                    if (xDiscProds != null && xDiscProds.Count > 0)
                    {
                        ProcessExclusiveDiscount(xDiscProds);
                        traceInfo += "Exclusive Product Level Discounts calculated." + Environment.NewLine;
                    }

                    if (xDiscCats != null && xDiscCats.Count > 0)
                    {
                        ProcessExclusiveDiscount(xDiscCats);
                        traceInfo += "Exclusive Category Level Discounts calculated." + Environment.NewLine;
                    }

                    if (compDiscVariants != null && compDiscVariants.Count > 0)
                    {
                        ProcessCompoundedDiscount(compDiscVariants);
                        traceInfo += "Compunded Variant Level Discounts calculated." + Environment.NewLine;
                    }
                    if (compDiscProds != null && compDiscProds.Count > 0)
                    {
                        ProcessCompoundedDiscount(compDiscProds);
                        traceInfo += "Compunded Product Level Discounts calculated." + Environment.NewLine;
                    }
                    if (compDiscCats != null && compDiscCats.Count > 0)
                    {
                        ProcessCompoundedDiscount(compDiscCats);
                        traceInfo += "Compunded Category Level Discounts calculated." + Environment.NewLine;
                    }

                }
                else
                {
                    CustomLogger.LogTraceInfo("No Products Are being loaded for Discount");
                }

            }

            catch (Exception exp)
            {

                throw;
            }
            finally
            {
                CustomLogger.LogTraceInfo(traceInfo);
            }
            return erpProducts;
        }
        */


         List<ErpProductDiscount> IDiscountController.GetDiscounts()
         {
             throw new NotImplementedException();
         }
    }
}



