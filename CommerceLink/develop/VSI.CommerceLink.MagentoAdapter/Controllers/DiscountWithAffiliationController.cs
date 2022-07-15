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
    public class DiscountWithAffiliationController : ProductBaseController, IDiscountWithAffiliationController
    {

        XmlTemplateHelper xmlHelper = null;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DiscountWithAffiliationController(string storeKey) : base(false, storeKey)
        {
            xmlHelper = new XmlTemplateHelper(currentStore);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushAllProductDiscounts pushes discounts.
        /// </summary>
        /// <param name="SpecialPrice"></param>
        public void PushAllProductDiscountWithAffiliations(ErpDiscountWithAffiliation discountWithAffiliations)
        {
            try
            {
                //this.ProcessDiscount(discounts);
                discountWithAffiliations.Discounts = ExcludeExpireDiscount(discountWithAffiliations.Discounts);
                this.ProcessDiscountWithAffiliationForDate(discountWithAffiliations);
                this.CreateDiscountWithAffiliationFile(discountWithAffiliations);
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(currentStore.StoreKey);
                byte[] filebyte = new byte[] { };
                obj.LogTransaction(19, "Discount With Affilation xml generation Failed: " + exp, DateTime.UtcNow, filebyte);
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set ValidationFrom and ValidationTo fields
        /// </summary>
        /// <param name="erpDiscounts"></param>
        private void ProcessDiscountWithAffiliationForDate(ErpDiscountWithAffiliation erpDiscountWithAffiliations)
        {
            if (erpDiscountWithAffiliations != null && erpDiscountWithAffiliations.Discounts != null)
            {
                for (int i = 0; i < erpDiscountWithAffiliations.Discounts.Count; i++)
                {
                    erpDiscountWithAffiliations.Discounts[i].ValidationFrom = erpDiscountWithAffiliations.Discounts[i].ValidFrom.ToString("MM/dd/yyyy");
                    erpDiscountWithAffiliations.Discounts[i].ValidationTo = erpDiscountWithAffiliations.Discounts[i].ValidTo.ToString("MM/dd/yyyy");
                }
            }
        }

        /// <summary>
        /// it creates Product discount file.
        /// </summary>
        /// <param name="productsDiscount"></param>
        private void CreateDiscountWithAffiliationFile(ErpDiscountWithAffiliation discountWithAffiliation)
        {
            String strFileName = String.Empty;
            String strFileDirectory = String.Empty;
            MappingTemplateDAL mappingTemplateDAL = new MappingTemplateDAL(currentStore.StoreKey);
            ObjectToCsvConverter objectToCsvConverter = new ObjectToCsvConverter();
            try
            {
                if (discountWithAffiliation != null)
                {
                    if (configurationHelper.GetSetting(ECOM.Discount_With_Affiliation_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_CSV)
                    {
                        #region CSV File Generation

                        strFileName = configurationHelper.GetSetting(DISCOUNTWITHAFFILIATION.Filename_Prefix) + currentStore.Name
                            + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + "." + ApplicationConstant.FILE_TYPE_CSV.ToLower();

                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(DISCOUNTWITHAFFILIATION.Local_Output_Path));

                        if (!String.IsNullOrEmpty(strFileDirectory) && !strFileDirectory.EndsWith("\\"))
                        {
                            strFileDirectory = strFileDirectory + "\\";
                        }

                        MappingTemplate mappingTemplate = mappingTemplateDAL.GetMappingTemplate(discountWithAffiliation.GetType().Name, ApplicationConstant.FILE_TYPE_CSV);

                        // Product Discount With Affiliation
                        objectToCsvConverter.ConvertObjectToCsv(discountWithAffiliation.Discounts.ToArray(), mappingTemplate.XML,
                            strFileDirectory, strFileName, true, null);

                        #endregion
                    }
                    else if (configurationHelper.GetSetting(ECOM.Discount_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_XML)
                    {
                        #region XML File Generation

                        strFileName = configurationHelper.GetSetting(DISCOUNTWITHAFFILIATION.Filename_Prefix) + discountWithAffiliation.OfferId
                            + "-" + currentStore.Name + "-" + DateTime.UtcNow.ToString("yyyyMMddhhmmssfff") + "." + ApplicationConstant.FILE_TYPE_XML.ToLower();

                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(DISCOUNTWITHAFFILIATION.Local_Output_Path));


                        xmlHelper.GenerateXmlUsingTemplate(strFileName, strFileDirectory, XmlTemplateHelper.XmlSourceDirection.CREATE, discountWithAffiliation);

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

        private List<ErpProductDiscountWithAffiliation> ExcludeExpireDiscount(List<ErpProductDiscountWithAffiliation> erpProductDiscountWithAffiliation)
        {
            List<ErpProductDiscountWithAffiliation> responseErpProductDiscountWithAffiliation = new List<ErpProductDiscountWithAffiliation>();

            try
            {
                foreach (var discount in erpProductDiscountWithAffiliation)
                {
                    var validToDate = discount.ValidTo.Date;
                    var currentDate = DateTime.UtcNow.Date;
                    var resultDays = (validToDate - currentDate).Days;

                    if (resultDays >= 0)
                    {
                        responseErpProductDiscountWithAffiliation.Add(discount);
                    }
                }

            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
                throw;
            }

            return responseErpProductDiscountWithAffiliation;
        }

        #endregion

    }
}
