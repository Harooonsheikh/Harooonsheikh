using System;
using System.Collections.Generic;
using System.Configuration;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;
using System.Linq;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.Common;

namespace VSI.CommerceLink.MagentoAdapter.Controllers
{

    /// <summary>
    /// PriceController class performs Price related activities.
    /// </summary>
    public class PriceController : ProductBaseController, IPriceController
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public PriceController(string storeKey) : base(false, storeKey)
        {
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// PushProducts push products to Magento
        /// </summary>
        /// <param name="products"></param>
        public string PushAllProductPrice(ErpPrice price, string fileName)
        {
            CustomLogger.LogDebugInfo(string.Format("Enter in PushAllProductPrice()"), currentStore.StoreId, currentStore.CreatedBy);
            string content = string.Empty;
            try
            {
                if (price.Prices != null && price.Prices.Count > 0)
                {
                    this.ProcessPrice(price);

                    CustomLogger.LogDebugInfo(string.Format("Exit from ProcessPrice()"), currentStore.StoreId, currentStore.CreatedBy);
                }

                content = this.CreatePriceFile(price, fileName);
                CustomLogger.LogDebugInfo(string.Format("Exit from CreatePriceFile()"), currentStore.StoreId, currentStore.CreatedBy);
                return content;
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(currentStore.StoreKey);
                obj.LogTransaction(SyncJobs.PriceSync, "Price XML generation Failed", DateTime.UtcNow, null);
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Processes Prices
        /// </summary>
        /// <param name="price">ErpPrice object containing list of ErpProductPrice objects</param>
        private void ProcessPrice(ErpPrice price)
        {

            CustomLogger.LogDebugInfo(string.Format("Enter in ProcessPrice()"), currentStore.StoreId, currentStore.CreatedBy);
            foreach (ErpProductPrice p in price.Prices)
            {
                //p.SKU = p.RetailVariantId;

                //For simple products
                if (string.IsNullOrEmpty(p.SKU))
                {
                    p.SKU = p.ItemId;
                }

                p.BasePrice = Math.Round(p.BasePrice, 2);
                p.Quantity = 1;

                // SQL converts never expires date into 1900, so converting back in AX format,also formating date in ISO format
                if (p.ValidFrom.Year == 1900)
                {
                    DateTime originalDate = DateTime.SpecifyKind(new DateTime(2154, 12, 31, 12, 00, 00), DateTimeKind.Utc);
                    String dateString = originalDate.ToString("o");
                    p.ValidFromString = dateString;
                }
                else
                {
                    DateTime originalDate = DateTime.SpecifyKind(p.ValidFrom, DateTimeKind.Utc);
                    String dateString = originalDate.ToString("o");
                    p.ValidFromString = dateString;
                }

                if (p.ValidTo.Year == 1900)
                {
                    DateTime originalDate = DateTime.SpecifyKind(new DateTime(2154, 12, 31, 12, 00, 00), DateTimeKind.Utc);
                    String dateString = originalDate.ToString("o");
                    p.ValidToString = dateString;
                }
                else
                {
                    DateTime originalDate = DateTime.SpecifyKind(p.ValidTo, DateTimeKind.Utc);
                    String dateString = originalDate.ToString("o");
                    p.ValidToString = dateString;

                }
            }
        }

        private string CreatePriceFile(ErpPrice price, string fileName)
        {
            String catalogContent = String.Empty;
            String strFileDirectory = String.Empty;
            MappingTemplateDAL mappingTemplateDAL = new MappingTemplateDAL(currentStore.StoreKey);

            ObjectToCsvConverter objectToCsvConverter = new ObjectToCsvConverter();
            CustomLogger.LogDebugInfo(string.Format("Enter in CreatePriceFile()"), currentStore.StoreId, currentStore.CreatedBy);
            try
            {
                if (price != null && price.Prices != null)
                {
                    if (configurationHelper.GetSetting(ECOM.Price_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_CSV)
                    {
                        #region CSV File Generation
                        fileName = fileName + "." + ApplicationConstant.FILE_TYPE_CSV.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(PRICE.local_Output_Path));
                        if (!String.IsNullOrEmpty(strFileDirectory) && !strFileDirectory.EndsWith("\\"))
                        {
                            strFileDirectory = strFileDirectory + "\\";
                        }
                        MappingTemplate mappingTemplate = mappingTemplateDAL.GetMappingTemplate(price.GetType().Name, ApplicationConstant.FILE_TYPE_CSV);
                        // Product Price
                        catalogContent=objectToCsvConverter.ConvertObjectToCsv(price.Prices.ToArray(), mappingTemplate.XML,
                           strFileDirectory, fileName, true, null);
                        #endregion
                    }
                    else if (configurationHelper.GetSetting(ECOM.Price_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_XML)
                    {
                        #region XML File Generation
                        fileName = configurationHelper.GetSetting(PRICE.Filename_Prefix) + currentStore.Name + "_" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + "." + ApplicationConstant.FILE_TYPE_XML.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(PRICE.local_Output_Path));

                        XmlTemplateHelper xmlHelper = new XmlTemplateHelper(currentStore);
                        xmlHelper.GenerateXmlUsingTemplate(fileName, strFileDirectory, XmlTemplateHelper.XmlSourceDirection.CREATE, price);
                        #endregion
                    }
                }
                    return catalogContent;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Function to enrich ErpProductPrice
        /// </summary>
        /// <param name="erpProductPriceCollection"></param>
        /// <param name="hirarchy"></param>
        private void processErpProductPrice(List<ErpProductPrice> erpProductPriceCollection)
        {
            if (erpProductPriceCollection != null)
            {
                string strConfigurableVariations = String.Empty;
                for (int i = 0; i < erpProductPriceCollection.Count; i++)
                {
                    erpProductPriceCollection[i].SKU = erpProductPriceCollection[i].SKU + "_" + erpProductPriceCollection[i].ColorId + "_" + erpProductPriceCollection[i].SizeId + "_" + erpProductPriceCollection[i].ItemId;
                }
            }
        }
        #endregion

    }
}
