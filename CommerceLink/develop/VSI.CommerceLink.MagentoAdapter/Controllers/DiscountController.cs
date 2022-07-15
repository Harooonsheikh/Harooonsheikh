using System;
using System.Collections.Generic;
using System.Configuration;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.CommerceLink.MagentoAdapter.Controllers
{

    /// <summary>
    /// DiscountController class performs Discount related activities.
    /// </summary>
    public class DiscountController : ProductBaseController, IDiscountController
    {
      
        XmlTemplateHelper xmlHelper = null;
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DiscountController(string storeKey) : base(false, storeKey)
        {
            xmlHelper = new XmlTemplateHelper(currentStore);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushAllProductDiscounts pushes discounts.
        /// </summary>
        /// <param name="SpecialPrice"></param>
        public string PushAllProductDiscounts(ErpDiscount discounts, string fileName)
        {
            string content = string.Empty;
            try
            {
                //this.ProcessDiscount(discounts);
                this.ProcessDiscountForDate(discounts);
                discounts.Discounts = ExcludeExpireDiscount(discounts.Discounts);
                content = this.CreateDiscountFile(discounts, fileName);
                return content;
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(currentStore.StoreKey);
                byte[] filebyte = new byte[] { };
                obj.LogTransaction(19, "Discount xml generation Failed: " + exp, DateTime.UtcNow, filebyte);
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This function process Discount.
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        private void ProcessDiscount(ErpDiscount discounts)
        {
            if (discounts != null)
            {
                // SQL converts never expires date into 1900, so converting back in AX format,also formating date in ISO format
                if (Convert.ToDateTime(discounts.ValidFrom).Year == 1900)
                {
                    DateTime originalDate = DateTime.SpecifyKind(new DateTime(2154, 12, 31, 12, 00, 00), DateTimeKind.Utc);
                    String dateString = originalDate.ToString("o");
                    discounts.ValidFrom = dateString;
                }
                else
                {
                    DateTime originalDate = DateTime.SpecifyKind(Convert.ToDateTime(discounts.ValidFrom), DateTimeKind.Utc);
                    String dateString = originalDate.ToString("o");
                    discounts.ValidFrom = dateString;
                }

                if (Convert.ToDateTime(discounts.ValidTo).Year == 1900)
                {
                    DateTime originalDate = DateTime.SpecifyKind(new DateTime(2154, 12, 31, 12, 00, 00), DateTimeKind.Utc);
                    String dateString = originalDate.ToString("o");
                    discounts.ValidTo = dateString;
                }
                else
                {
                    DateTime originalDate = DateTime.SpecifyKind(Convert.ToDateTime(discounts.ValidTo), DateTimeKind.Utc);
                    String dateString = originalDate.ToString("o");
                    discounts.ValidTo = dateString;
                }
            }
            if (discounts.Discounts != null)
            {
                foreach (ErpProductDiscount p in discounts.Discounts)
                {
                    p.SKU = p.RetailvariantId;
                    p.SKU = base.PrefixSKU(p.SKU);
                    p.SKU = base.PostfixSKU(p.SKU);
                    p.OfferPrice = Math.Round(p.OfferPrice, 2);
                    p.Quantity = 1;
                }
            }
        }

        /// <summary>
        /// Set ValidationFrom and ValidationTo fields
        /// </summary>
        /// <param name="erpDiscounts"></param>
        private void ProcessDiscountForDate(ErpDiscount erpDiscounts)
        {
            if (erpDiscounts != null && erpDiscounts.Discounts != null)
            {
                for (int i=0; i<erpDiscounts.Discounts.Count; i++)
                {
                    erpDiscounts.Discounts[i].ValidationFrom = erpDiscounts.Discounts[i].ValidFrom.ToString("MM/dd/yyyy");
                    erpDiscounts.Discounts[i].ValidationTo = erpDiscounts.Discounts[i].ValidTo.ToString("MM/dd/yyyy");
                }
            }
        }

        /// <summary>
        /// it creates Product discount file.
        /// </summary>
        /// <param name="productsDiscount"></param>
        private string CreateDiscountFile(ErpDiscount discount, string fileName)
        {
            String catalogContent = String.Empty;
            String strFileDirectory = String.Empty;
            ObjectToCsvConverter objectToCsvConverter = new ObjectToCsvConverter();
            MappingTemplateDAL mappingTemplateDAL = new MappingTemplateDAL(this.currentStore.StoreKey);

            try
            {
                if (discount != null)
                {
                    if (configurationHelper.GetSetting(ECOM.Discount_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_CSV)
                    {
                        #region CSV File Generation
                        fileName = fileName + "." + ApplicationConstant.FILE_TYPE_CSV.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(DISCOUNT.Local_Output_Path));
                        if (!String.IsNullOrEmpty(strFileDirectory) && !strFileDirectory.EndsWith("\\"))
                        {
                            strFileDirectory = strFileDirectory + "\\";
                        }

                        MappingTemplate mappingTemplate = mappingTemplateDAL.GetMappingTemplate(discount.GetType().Name, ApplicationConstant.FILE_TYPE_CSV);

                        // Product Discount
                        catalogContent = objectToCsvConverter.ConvertObjectToCsv(discount.Discounts.ToArray(), mappingTemplate.XML,
                            strFileDirectory, fileName, true, null);
                        #endregion
                    }
                    else if (configurationHelper.GetSetting(ECOM.Discount_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_XML)
                    {
                        #region XML File Generation
                        fileName = configurationHelper.GetSetting(DISCOUNT.Filename_Prefix) + discount.OfferId + "-" + currentStore.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + "." + ApplicationConstant.FILE_TYPE_XML.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(DISCOUNT.Local_Output_Path));

                   
                        xmlHelper.GenerateXmlUsingTemplate(fileName, strFileDirectory, XmlTemplateHelper.XmlSourceDirection.CREATE, discount);
                        #endregion
                    }
                }
                return catalogContent;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }
        }

        /// <summary>
        /// Function to enrich ErpProductPrice
        /// </summary>
        /// <param name="erpProductDiscountCollection"></param>
        /// <param name="hirarchy"></param>
        private void processErpProductDiscount(List<ErpProductDiscount> erpProductDiscountCollection)
        {
            if (erpProductDiscountCollection != null)
            {
                string strConfigurableVariations = String.Empty;

                for (int i = 0; i < erpProductDiscountCollection.Count; i++)
                {
                    // Set SKU in desisred format for Magento
                    erpProductDiscountCollection[i].SKU = erpProductDiscountCollection[i].SKU + "_" + erpProductDiscountCollection[i].ColorId + "_" + erpProductDiscountCollection[i].SizeId + "_" + erpProductDiscountCollection[i].ItemId;
                }
            }
        }

        private List<ErpProductDiscount> ExcludeExpireDiscount(List<ErpProductDiscount> erpProductDiscountCollection)
        {
            List<ErpProductDiscount> responseErpProductDiscount = new List<ErpProductDiscount>();

            try
            {
                foreach (var discount in erpProductDiscountCollection)
                {
                    var validToDate = discount.ValidTo.Date;
                    var currentDate = DateTime.UtcNow.Date;
                    var resultDays = (validToDate - currentDate).Days;

                    if (resultDays >= 0)
                    {
                        responseErpProductDiscount.Add(discount);
                    }
                }

            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }

            return responseErpProductDiscount;
        }

        #endregion
    }
}
