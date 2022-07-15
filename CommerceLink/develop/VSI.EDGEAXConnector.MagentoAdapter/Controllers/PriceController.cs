using System;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
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
        public PriceController()
            : base(false)
        {
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// PushProducts push products to Magento
        /// </summary>
        /// <param name="products"></param>
        public void PushAllProductPrice(ErpPrice price)
        {
            CustomLogger customLogger = new CustomLogger();
            customLogger.LogDebugInfo(string.Format("Enter in PushAllProductPrice()"));
            try
            {
                if (price.Prices != null && price.Prices.Count > 0)
                {
                    this.ProcessPrice(price);
                    customLogger.LogDebugInfo(string.Format("Exit from ProcessPrice()"));
                }
                this.CreatePriceFile(price);
                customLogger.LogDebugInfo(string.Format("Exit from CreatePriceFile()"));
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(StoreService.StoreLkey);
                obj.LogTransaction(SyncJobs.PriceSync, "Price XML generation Failed", DateTime.UtcNow, null);
                customLogger.LogException(exp);
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
            CustomLogger customLogger = new CustomLogger();
            customLogger.LogDebugInfo(string.Format("Enter in ProcessPrice()"));
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

        private void CreatePriceFile(ErpPrice price)
        {
            CustomLogger customLogger = new CustomLogger();
            customLogger.LogDebugInfo(string.Format("Enter in CreatePriceFile()"));
            try
            {
                if (price != null && price.Prices != null)
                {
                    string fileNamePrice = configurationHelper.GetSetting(PRICE.Filename_Prefix) + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";
                    XmlTemplateHelper xmlHelper = new XmlTemplateHelper();
                    xmlHelper.GenerateXmlUsingTemplate(fileNamePrice, ConfigurationHelper.GetDirectory(configurationHelper.GetSetting(PRICE.local_Output_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, price);                 
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

    }
}
