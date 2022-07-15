using System;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
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
        public DiscountController()
            : base(false)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushAllProductDiscounts pushes discounts.
        /// </summary>
        /// <param name="SpecialPrice"></param>
        public void PushAllProductDiscounts(ErpDiscount discounts)
        {
            try
            {
               // this.ProcessDiscount(discounts);
                this.CreateDiscountFile(discounts);
            }
            catch (Exception exp)
            {
                CustomLogger customLogger = new CustomLogger();
                customLogger.LogException(exp);
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
        private void CreateDiscountFile(ErpDiscount discount)
        {
            try
            {
                if (discount != null)
                {
                    string fileNameDiscount = configurationHelper.GetSetting(DISCOUNT.Filename_Prefix) + discount.OfferId + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";
                    XmlTemplateHelper xmlHelper = new XmlTemplateHelper();
                    xmlHelper.GenerateXmlUsingTemplate(fileNameDiscount, ConfigurationHelper.GetDirectory(configurationHelper.GetSetting(DISCOUNT.Local_Output_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, discount);
                }
            }
            catch (Exception ex)
            {
                CustomLogger customLogger = new CustomLogger();
                customLogger.LogException(ex);
                throw;
            }
        }

        #endregion
    }
}
