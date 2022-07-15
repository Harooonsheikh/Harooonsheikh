using System;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.CommerceLink.DemandwareAdapter.Controllers
{

    /// <summary>
    /// DiscountController class performs Discount related activities.
    /// </summary>
    public class DiscountController : ProductBaseController, IDiscountController
    {
    
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DiscountController (string storeKey) : base(false, storeKey)
        {
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
                // this.ProcessDiscount(discounts);
                this.CreateDiscountFile(discounts, fileName);
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
        /// it creates Product discount file.
        /// </summary>
        /// <param name="productsDiscount"></param>
        private void CreateDiscountFile(ErpDiscount discount, string fileName)
        {
            String strFileName = String.Empty;
            String strFileDirectory = String.Empty;
            ObjectToCsvConverter objectToCsvConverter = new ObjectToCsvConverter();
            try
            {
                if (discount != null)
                {
                    if (configurationHelper.GetSetting(ECOM.Discount_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_CSV)
                    {
                        #region CSV File Generation
                        strFileName = configurationHelper.GetSetting(DISCOUNT.Filename_Prefix) + DateTime.UtcNow.ToString("yyyyMMddhhmm") + "." + ApplicationConstant.FILE_TYPE_CSV.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(DISCOUNT.Local_Output_Path));
                        if (!String.IsNullOrEmpty(strFileDirectory) && !strFileDirectory.EndsWith("\\"))
                        {
                            strFileDirectory = strFileDirectory + "\\";
                        }
                        string strTemplateObjectToCsvMappingXmlFileLocation = configurationHelper.GetSetting(DISCOUNT.CSV_Map_Path);
                        // Product Discount
                        objectToCsvConverter.ConvertObjectToCsv(discount.Discounts.ToArray(), strTemplateObjectToCsvMappingXmlFileLocation,
                            strFileDirectory, strFileName, true, null);
                        #endregion
                    }
                    else if (configurationHelper.GetSetting(ECOM.Discount_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_XML)
                    {
                        #region XML File Generation
                        strFileName = configurationHelper.GetSetting(DISCOUNT.Filename_Prefix) + discount.OfferId + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + "." + ApplicationConstant.FILE_TYPE_XML.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(DISCOUNT.Local_Output_Path));

                        XmlTemplateHelper xmlHelper = new XmlTemplateHelper(currentStore);
                        xmlHelper.GenerateXmlUsingTemplate(strFileName, strFileDirectory, XmlTemplateHelper.XmlSourceDirection.CREATE, discount);
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

        #endregion
    }
}
