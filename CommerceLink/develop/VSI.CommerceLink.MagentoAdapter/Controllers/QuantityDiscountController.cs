using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.CommerceLink.MagentoAdapter.Controllers
{
    public class QuantityDiscountController : ProductBaseController, IQuantityDiscountController
    {

        XmlTemplateHelper xmlHelper = null;
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public QuantityDiscountController(string storeKey) : base(false, storeKey)
        {
            xmlHelper = new XmlTemplateHelper(currentStore);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushAllQuantityDiscounts pushes quantity discounts.
        /// </summary>
        /// <param name="SpecialPrice"></param>
        public void PushAllQuantityDiscounts(ErpQuantityDiscount erpQuantityDiscount, List<ErpProduct> erpProdcutList)
        {

            ErpItemQuantityDiscount erpItemQuantityDiscount = new ErpItemQuantityDiscount();

            List<ErpProductQuantityDiscount> erpQuantityDiscountList = ExcludeExpireDiscount(erpQuantityDiscount.QuantityDiscounts);
            ProcessQuantityDiscountByCategoryProductVariant(erpQuantityDiscountList, erpProdcutList);
            erpItemQuantityDiscount.ProductItemQuantityDiscounts = FillItemProductQuantityDiscount(erpQuantityDiscountList);
            this.CreateQuantityDiscountFile(erpItemQuantityDiscount);

        }
        #endregion

        #region Private Methods 
        private List<ErpProductQuantityDiscount> ExcludeExpireDiscount(List<ErpProductQuantityDiscount> quantityDiscounts)
        {
            List<ErpProductQuantityDiscount> responseErpProductQuantityDiscountList = new List<ErpProductQuantityDiscount>();

            try
            {
                foreach (var discount in quantityDiscounts)
                {
                    var validToDate = discount.ValidTo.Date;
                    var currentDate = DateTime.UtcNow.Date;
                    var resultDays = (validToDate - currentDate).Days;

                    if (resultDays >= 0)
                    {
                        responseErpProductQuantityDiscountList.Add(discount);
                    }
                }

            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }

            return responseErpProductQuantityDiscountList;
        }
        /// <summary>
        /// This function process Discount.
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private void ProcessQuantityDiscountByCategoryProductVariant(List<ErpProductQuantityDiscount> erpQuantityDiscountList, List<ErpProduct> erpProdcutList)
        {
            foreach (ErpProductQuantityDiscount erpProductQuantityDiscount in erpQuantityDiscountList)
            {
                foreach (KeyValuePair<long, Dictionary<long, List<long>>> categoryProductVariant in erpProductQuantityDiscount.CategoryProductVariant)
                {
                    if (categoryProductVariant.Key == 0)
                    {
                        #region if there is no category then do nothing
                        #endregion
                    }
                    else
                    {
                        #region if there is some category
                        foreach (ErpProduct erpProduct in erpProdcutList)
                        {
                            #region if category of product matches category of quantity discount
                            if (erpProduct.CategoryIds.Contains(categoryProductVariant.Key))
                            {
                                if (categoryProductVariant.Value.ContainsKey(erpProduct.RecordId))
                                {
                                    #region if productId of erpProduct matches product of quantity discount then check if variants exist for it
                                    Dictionary<long, List<long>> productVariant = categoryProductVariant.Value;
                                    List<long> variantList = new List<long>();
                                    productVariant.TryGetValue(erpProduct.RecordId, out variantList);

                                    if (variantList.Count == 0)
                                    {
                                        AddSKUAndVariants(erpProductQuantityDiscount, erpProduct);
                                    }
                                    else
                                    {
                                        if (erpProduct.IsMasterProduct == false && variantList.Contains(erpProduct.RecordId))
                                        {
                                            AddSKUOnly(erpProductQuantityDiscount, erpProduct);
                                        }
                                        else
                                        {
                                            CheckIfVariantExistsThenAddItsSKU(erpProductQuantityDiscount, variantList, erpProduct);
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    #region if productId of erpProduct does not match product of quantity discount and there are not products for categories then add SKU of existing product containing same category 
                                    if (categoryProductVariant.Value.Count == 0)
                                    {
                                        if (erpProduct.IsMasterProduct == false)
                                        {
                                            AddSKUOnly(erpProductQuantityDiscount, erpProduct);
                                        }
                                    }
                                    #endregion
                                }
                            }
                            #endregion
                        }
                        #endregion
                    }
                }

            }
        }

        /// <summary>
        /// it creates Product Quantity Discount file.
        /// </summary>
        /// <param name="erpItemQuantityDiscount"></param>
        private void CreateQuantityDiscountFile(ErpItemQuantityDiscount erpItemQuantityDiscount)
        {
            String strFileName = String.Empty;
            String strFileDirectory = String.Empty;
            ObjectToCsvConverter objectToCsvConverter = new ObjectToCsvConverter();

            try
            {
                if (erpItemQuantityDiscount != null)
                {
                    if (configurationHelper.GetSetting(ECOM.Quantity_Discount_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_CSV)
                    {
                        #region CSV File Generation
                        strFileName = configurationHelper.GetSetting(QUANTITYDISCOUNT.Filename_Prefix) + currentStore.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + "." + ApplicationConstant.FILE_TYPE_CSV.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(QUANTITYDISCOUNT.Local_Output_Path));
                        if (!String.IsNullOrEmpty(strFileDirectory) && !strFileDirectory.EndsWith("\\"))
                        {
                            strFileDirectory = strFileDirectory + "\\";
                        }

                        string strTemplateObjectToCsvMappingXmlFileLocation = configurationHelper.GetSetting(QUANTITYDISCOUNT.CSV_Map_Path);
                        // Product Discount
                        objectToCsvConverter.ConvertObjectToCsv(erpItemQuantityDiscount.ProductItemQuantityDiscounts.ToArray(), strTemplateObjectToCsvMappingXmlFileLocation,
                            strFileDirectory, strFileName, true, null);
                        #endregion
                    }
                    else if (configurationHelper.GetSetting(ECOM.Quantity_Discount_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_XML)
                    {
                        #region XML File Generation
                        strFileName = configurationHelper.GetSetting(QUANTITYDISCOUNT.Filename_Prefix) + currentStore.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + "." + ApplicationConstant.FILE_TYPE_XML.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(QUANTITYDISCOUNT.Local_Output_Path));


                        xmlHelper.GenerateXmlUsingTemplate(strFileName, strFileDirectory, XmlTemplateHelper.XmlSourceDirection.CREATE, erpItemQuantityDiscount);
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }
        }



        private void AddSKUAndVariants(ErpProductQuantityDiscount erpProductQuantityDiscount, ErpProduct erpProduct)
        {
            if (erpProductQuantityDiscount.SKUs == null)
            {
                erpProductQuantityDiscount.SKUs = new List<string>();
            }

            if (erpProductQuantityDiscount.SKUs.Contains(erpProduct.SKU) == false && erpProduct.IsMasterProduct == false)
            {
                erpProductQuantityDiscount.SKUs.Add(erpProduct.SKU);
            }

            if (erpProduct.IsMasterProduct)
            {
                foreach (var erpProductVariant in erpProduct.Variants)
                {
                    if (erpProductQuantityDiscount.SKUs.Contains(erpProductVariant.SKU) == false)
                    {
                        erpProductQuantityDiscount.SKUs.Add(erpProductVariant.SKU);
                    }
                }
            }
        }

        private void AddSKUOnly(ErpProductQuantityDiscount erpProductQuantityDiscount, ErpProduct erpProduct)
        {
            if (erpProductQuantityDiscount.SKUs == null)
            {
                erpProductQuantityDiscount.SKUs = new List<string>();
            }

            if (erpProductQuantityDiscount.SKUs.Contains(erpProduct.SKU) == false && erpProduct.IsMasterProduct == false)
            {
                erpProductQuantityDiscount.SKUs.Add(erpProduct.SKU);
            }
        }

        private void CheckIfVariantExistsThenAddItsSKU(ErpProductQuantityDiscount erpProductQuantityDiscount, List<long> variantList, ErpProduct erpProduct)
        {
            if (erpProductQuantityDiscount.SKUs == null)
            {
                erpProductQuantityDiscount.SKUs = new List<string>();
            }

            foreach (var erpProductVariant in erpProduct.Variants)
            {
                if (variantList.Contains(erpProductVariant.DistinctProductVariantId))
                {
                    if (erpProductQuantityDiscount.SKUs.Contains(erpProductVariant.SKU) == false)
                    {
                        erpProductQuantityDiscount.SKUs.Add(erpProductVariant.SKU);
                    }
                }
            }
        }

        private List<ErpProductItemQuantityDiscount> FillItemProductQuantityDiscount(List<ErpProductQuantityDiscount> erpQuantityDiscountList)
        {
            List<ErpProductItemQuantityDiscount> erpNewProductQuantityDiscountList = new List<ErpProductItemQuantityDiscount>();

            foreach (ErpProductQuantityDiscount erpProductQuantityDiscount in erpQuantityDiscountList)
            {
                if (erpProductQuantityDiscount.SKUs == null)
                {
                    continue;
                }

                foreach (string sku in erpProductQuantityDiscount.SKUs)
                {
                    foreach (ErpQuantityDiscountConfiguration erpQuantityDiscountConfiguration in erpProductQuantityDiscount.QuantityDiscountConfiguration)
                    {
                        ErpProductItemQuantityDiscount erpNewProductQuantityDiscount = new ErpProductItemQuantityDiscount();
                        erpNewProductQuantityDiscount.SKU = sku;
                        erpNewProductQuantityDiscount.TierPriceQuantity = erpQuantityDiscountConfiguration.MinimumQuantity;
                        erpNewProductQuantityDiscount.TierPrice = erpQuantityDiscountConfiguration.UnitPriceOrDiscountPercentage;
                        erpNewProductQuantityDiscount.TierPriceValueType = erpProductQuantityDiscount.MultiBuyDiscountType;
                        erpNewProductQuantityDiscount.DiscountName = erpProductQuantityDiscount.Name;
                        erpNewProductQuantityDiscount.OfferId = erpProductQuantityDiscount.OfferId;
                        erpNewProductQuantityDiscount.PeriodicDiscountType = erpProductQuantityDiscount.PeriodicDiscountType;
                        erpNewProductQuantityDiscountList.Add(erpNewProductQuantityDiscount);
                    }
                }
            }

            return erpNewProductQuantityDiscountList;
        }
        #endregion

    }
}
