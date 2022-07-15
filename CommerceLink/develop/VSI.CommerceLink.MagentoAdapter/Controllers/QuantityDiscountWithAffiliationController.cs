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
    public class QuantityDiscountWithAffiliationController : ProductBaseController, IQuantityDiscountWithAffiliationController
    {
        XmlTemplateHelper xmlHelper = null;

        #region Constructor

        public QuantityDiscountWithAffiliationController(string storeKey) : base(false, storeKey)
        {
            xmlHelper = new XmlTemplateHelper(currentStore);
        }

        #endregion

        #region Public Methods

        public void PushAllQuantityDiscountWithAffiliations(ErpQuantityDiscountWithAffiliation erpQuantityDiscountWithAffiliation, List<ErpProduct> erpProdcutList)
        {
            ErpItemQuantityDiscountWithAffiliation erpItemQuantityDiscountWithAffiliation = new ErpItemQuantityDiscountWithAffiliation();

            List<ErpProductQuantityDiscountWithAffiliation> erpQuantityDiscountWithAffiliationList = ExcludeExpireDiscount(erpQuantityDiscountWithAffiliation.QuantityDiscountWithAffiliations);

            ProcessQuantityDiscountWithAffiliationByCategoryProductVariant(erpQuantityDiscountWithAffiliationList, erpProdcutList);

            erpItemQuantityDiscountWithAffiliation.ProductItemQuantityDiscountWithAffiliations = FillItemProductQuantityDiscountWithAffiliation(erpQuantityDiscountWithAffiliationList);

            this.ProcessQuantityDiscountWithAffiliationFromAndToDates(erpItemQuantityDiscountWithAffiliation);

            this.CreateQuantityDiscountWithAffiliationFile(erpItemQuantityDiscountWithAffiliation);
        }

        private List<ErpProductQuantityDiscountWithAffiliation> ExcludeExpireDiscount(List<ErpProductQuantityDiscountWithAffiliation> quantityDiscountWithAffiliations)
        {
            List<ErpProductQuantityDiscountWithAffiliation> responseQuantityDiscountWithAffiliations = new List<ErpProductQuantityDiscountWithAffiliation>();

            try
            {
                foreach (var discount in quantityDiscountWithAffiliations)
                {
                    var validToDate = discount.ValidTo.Date;
                    var currentDate = DateTime.UtcNow.Date;
                    var resultDays = (validToDate - currentDate).Days;

                    if (resultDays >= 0)
                    {
                        responseQuantityDiscountWithAffiliations.Add(discount);
                    }
                }
                
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }

            return responseQuantityDiscountWithAffiliations;
        }

        #endregion

        #region Private Methods

        private void ProcessQuantityDiscountWithAffiliationByCategoryProductVariant(List<ErpProductQuantityDiscountWithAffiliation> erpQuantityDiscountWithAffiliationList, List<ErpProduct> erpProdcutList)
        {
            foreach (ErpProductQuantityDiscountWithAffiliation erpProductQuantityDiscountWithAffiliation in erpQuantityDiscountWithAffiliationList)
            {
                foreach (KeyValuePair<long, Dictionary<long, List<long>>> categoryProductVariant in erpProductQuantityDiscountWithAffiliation.CategoryProductVariant)
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
                                        AddSKUAndVariants(erpProductQuantityDiscountWithAffiliation, erpProduct);
                                    }
                                    else
                                    {
                                        if (erpProduct.IsMasterProduct == false && variantList.Contains(erpProduct.RecordId))
                                        {
                                            AddSKUOnly(erpProductQuantityDiscountWithAffiliation, erpProduct);
                                        }
                                        else
                                        {
                                            CheckIfVariantExistsThenAddItsSKU(erpProductQuantityDiscountWithAffiliation, variantList, erpProduct);
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
                                            AddSKUOnly(erpProductQuantityDiscountWithAffiliation, erpProduct);
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

        private ErpItemQuantityDiscountWithAffiliation UpdateDataForCSV(ErpItemQuantityDiscountWithAffiliation erpItemQuantityDiscountWithAffiliation)
        {
            foreach (var item in erpItemQuantityDiscountWithAffiliation.ProductItemQuantityDiscountWithAffiliations)
            {
                if (item.TierPriceValueType == ErpMultiBuyDiscountType.UnitPrice)
                {
                    item.TierPriceValueTypeUpdatedValue = "Fixed";
                }
                else
                {
                    item.TierPriceValueTypeUpdatedValue = "Discount";
                }

                if (item.TierPriceValueType == ErpMultiBuyDiscountType.DiscountPercentage)
                {
                    item.DiscountPercentageUpdatedValue = item.TierPrice.ToString();
                    item.UnitPriceUpdatedValue = "0";
                }
                else
                {
                    item.DiscountPercentageUpdatedValue = "0";
                    item.UnitPriceUpdatedValue = item.TierPrice.ToString();
                }

            }
            return erpItemQuantityDiscountWithAffiliation;
        }

        private void CreateQuantityDiscountWithAffiliationFile(ErpItemQuantityDiscountWithAffiliation erpItemQuantityDiscountWithAffiliation)
        {
            String strFileName = String.Empty;
            String strFileDirectory = String.Empty;
            MappingTemplateDAL mappingTemplateDAL = new MappingTemplateDAL(currentStore.StoreKey);
            ObjectToCsvConverter objectToCsvConverter = new ObjectToCsvConverter();

            try
            {
                if (erpItemQuantityDiscountWithAffiliation != null)
                {
                    if (configurationHelper.GetSetting(ECOM.Quantity_Discount_With_Affiliation_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_CSV)
                    {
                        erpItemQuantityDiscountWithAffiliation = UpdateDataForCSV(erpItemQuantityDiscountWithAffiliation);

                        #region CSV File Generation
                        strFileName = configurationHelper.GetSetting(QUANTITYDISCOUNTWITHAFFILIATION.Filename_Prefix) + currentStore.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + "." + ApplicationConstant.FILE_TYPE_CSV.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(QUANTITYDISCOUNTWITHAFFILIATION.Local_Output_Path));
                        if (!String.IsNullOrEmpty(strFileDirectory) && !strFileDirectory.EndsWith("\\"))
                        {
                            strFileDirectory = strFileDirectory + "\\";
                        }

                        MappingTemplate mappingTemplate = mappingTemplateDAL.GetMappingTemplate(erpItemQuantityDiscountWithAffiliation.GetType().Name, ApplicationConstant.FILE_TYPE_CSV);

                        // Product Discount
                        objectToCsvConverter.ConvertObjectToCsv(erpItemQuantityDiscountWithAffiliation.ProductItemQuantityDiscountWithAffiliations.ToArray(), mappingTemplate.XML,
                            strFileDirectory, strFileName, true, null);
                        #endregion
                    }
                    else if (configurationHelper.GetSetting(ECOM.Quantity_Discount_With_Affiliation_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_XML)
                    {
                        #region XML File Generation
                        strFileName = configurationHelper.GetSetting(QUANTITYDISCOUNTWITHAFFILIATION.Filename_Prefix) + currentStore.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + "." + ApplicationConstant.FILE_TYPE_XML.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(QUANTITYDISCOUNTWITHAFFILIATION.Local_Output_Path));


                        xmlHelper.GenerateXmlUsingTemplate(strFileName, strFileDirectory, XmlTemplateHelper.XmlSourceDirection.CREATE, erpItemQuantityDiscountWithAffiliation);
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

        private void AddSKUAndVariants(ErpProductQuantityDiscountWithAffiliation erpProductQuantityDiscountWithAffiliation, ErpProduct erpProduct)
        {
            if (erpProductQuantityDiscountWithAffiliation.SKUs == null)
            {
                erpProductQuantityDiscountWithAffiliation.SKUs = new List<string>();
            }

            if (erpProductQuantityDiscountWithAffiliation.SKUs.Contains(erpProduct.SKU) == false && erpProduct.IsMasterProduct == false)
            {
                erpProductQuantityDiscountWithAffiliation.SKUs.Add(erpProduct.SKU);
            }

            if (erpProduct.IsMasterProduct)
            {
                foreach (var erpProductVariant in erpProduct.Variants)
                {
                    if (erpProductQuantityDiscountWithAffiliation.SKUs.Contains(erpProductVariant.SKU) == false)
                    {
                        erpProductQuantityDiscountWithAffiliation.SKUs.Add(erpProductVariant.SKU);
                    }
                }
            }
        }

        private void AddSKUOnly(ErpProductQuantityDiscountWithAffiliation erpProductQuantityDiscountWithAffiliation, ErpProduct erpProduct)
        {
            if (erpProductQuantityDiscountWithAffiliation.SKUs == null)
            {
                erpProductQuantityDiscountWithAffiliation.SKUs = new List<string>();
            }

            if (erpProductQuantityDiscountWithAffiliation.SKUs.Contains(erpProduct.SKU) == false && erpProduct.IsMasterProduct == false)
            {
                erpProductQuantityDiscountWithAffiliation.SKUs.Add(erpProduct.SKU);
            }
        }

        private void CheckIfVariantExistsThenAddItsSKU(ErpProductQuantityDiscountWithAffiliation erpProductQuantityDiscountWithAffiliation, List<long> variantList, ErpProduct erpProduct)
        {
            if (erpProductQuantityDiscountWithAffiliation.SKUs == null)
            {
                erpProductQuantityDiscountWithAffiliation.SKUs = new List<string>();
            }

            foreach (var erpProductVariant in erpProduct.Variants)
            {
                if (variantList.Contains(erpProductVariant.DistinctProductVariantId))
                {
                    if (erpProductQuantityDiscountWithAffiliation.SKUs.Contains(erpProductVariant.SKU) == false)
                    {
                        erpProductQuantityDiscountWithAffiliation.SKUs.Add(erpProductVariant.SKU);
                    }
                }
            }
        }

        private List<ErpProductItemQuantityDiscountWithAffiliation> FillItemProductQuantityDiscountWithAffiliation(List<ErpProductQuantityDiscountWithAffiliation> erpQuantityDiscountWithAffiliationList)
        {
            List<ErpProductItemQuantityDiscountWithAffiliation> erpNewProductQuantityDiscountWithAffiliationList = new List<ErpProductItemQuantityDiscountWithAffiliation>();

            foreach (ErpProductQuantityDiscountWithAffiliation erpProductQuantityDiscountWithAffiliation in erpQuantityDiscountWithAffiliationList)
            {
                if (erpProductQuantityDiscountWithAffiliation.SKUs == null)
                {
                    continue;
                }

                foreach (string sku in erpProductQuantityDiscountWithAffiliation.SKUs)
                {
                    foreach (ErpQuantityDiscountConfiguration erpQuantityDiscountConfiguration in erpProductQuantityDiscountWithAffiliation.QuantityDiscountConfiguration)
                    {
                        ErpProductItemQuantityDiscountWithAffiliation erpNewProductQuantityDiscountWithAffiliation = new ErpProductItemQuantityDiscountWithAffiliation();
                        erpNewProductQuantityDiscountWithAffiliation.SKU = sku;
                        erpNewProductQuantityDiscountWithAffiliation.TierPriceQuantity = erpQuantityDiscountConfiguration.MinimumQuantity;
                        erpNewProductQuantityDiscountWithAffiliation.TierPrice = erpQuantityDiscountConfiguration.UnitPriceOrDiscountPercentage;
                        erpNewProductQuantityDiscountWithAffiliation.TierPriceValueType = erpProductQuantityDiscountWithAffiliation.MultiBuyDiscountType;
                        erpNewProductQuantityDiscountWithAffiliation.DiscountName = erpProductQuantityDiscountWithAffiliation.Name;
                        erpNewProductQuantityDiscountWithAffiliation.OfferId = erpProductQuantityDiscountWithAffiliation.OfferId;
                        erpNewProductQuantityDiscountWithAffiliation.PeriodicDiscountType = erpProductQuantityDiscountWithAffiliation.PeriodicDiscountType;
                        erpNewProductQuantityDiscountWithAffiliation.AffiliationId = erpProductQuantityDiscountWithAffiliation.AffiliationId;
                        erpNewProductQuantityDiscountWithAffiliation.AffiliationName = erpProductQuantityDiscountWithAffiliation.AffiliationName;
                        erpNewProductQuantityDiscountWithAffiliation.PeriodicDiscountType = erpProductQuantityDiscountWithAffiliation.PeriodicDiscountType;
                        erpNewProductQuantityDiscountWithAffiliation.ValidFrom = erpProductQuantityDiscountWithAffiliation.ValidFrom.Date;
                        erpNewProductQuantityDiscountWithAffiliation.ValidTo = erpProductQuantityDiscountWithAffiliation.ValidTo.Date;
                        erpNewProductQuantityDiscountWithAffiliation.PeriodicDiscountType = erpProductQuantityDiscountWithAffiliation.PeriodicDiscountType;
                        erpNewProductQuantityDiscountWithAffiliation.DiscountType = erpProductQuantityDiscountWithAffiliation.DiscountType;
                        erpNewProductQuantityDiscountWithAffiliation.CurrencyCode = erpProductQuantityDiscountWithAffiliation.CurrencyCode;
                        erpNewProductQuantityDiscountWithAffiliationList.Add(erpNewProductQuantityDiscountWithAffiliation);

                    }
                }
            }

            return erpNewProductQuantityDiscountWithAffiliationList;
        }

        /// <summary>
        /// Set ValidationFrom and ValidationTo fields
        /// </summary>
        /// <param name="erpQuantityDiscountWithAffiliation"></param>
        private void ProcessQuantityDiscountWithAffiliationFromAndToDates(ErpItemQuantityDiscountWithAffiliation erpItemQuantityDiscountWithAffiliation)
        {
            if (erpItemQuantityDiscountWithAffiliation != null && erpItemQuantityDiscountWithAffiliation.ProductItemQuantityDiscountWithAffiliations != null)
            {
                for (int i = 0; i < erpItemQuantityDiscountWithAffiliation.ProductItemQuantityDiscountWithAffiliations.Count; i++)
                {
                    erpItemQuantityDiscountWithAffiliation.ProductItemQuantityDiscountWithAffiliations[i].ValidationFrom = erpItemQuantityDiscountWithAffiliation.ProductItemQuantityDiscountWithAffiliations[i].ValidFrom.ToString("MM/dd/yyyy");
                    erpItemQuantityDiscountWithAffiliation.ProductItemQuantityDiscountWithAffiliations[i].ValidationTo = erpItemQuantityDiscountWithAffiliation.ProductItemQuantityDiscountWithAffiliations[i].ValidTo.ToString("MM/dd/yyyy");
                }
            }
        }

        #endregion
    }
}
