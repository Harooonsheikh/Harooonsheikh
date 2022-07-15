using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Demandware.Catalog;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
{

    /// <summary>
    /// CategoryController performs Category related activities in Magento side.
    /// </summary>
    public class CategoryController : BaseController, ICategoryController
    {

        #region Data Members

        /// <summary>
        /// TransactionLogging object records all transaction in database.
        /// </summary>
        TransactionLogging TransactionLog;
        catalog dwCatalog = new catalog();
        XmlSerializer xml = new XmlSerializer(typeof(catalog));

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CategoryController()
            : base(true)
        {
            this.TransactionLog = new TransactionLogging(StoreService.StoreLkey
);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushCategories process provided list of categories and add and or update categories in DW.
        /// </summary>
        /// <param name="categories"></param>
        public void PushCategories(ErpCatalog catalog)
        {
            string comKey;
            IntegrationKey key;
            List<IntegrationKey> integrationKeys = null;
            int parentCategoryId;
            IntegrationManager integrationManager = new IntegrationManager(StoreService.StoreLkey);
            try
            {
                if (catalog != null && catalog.Categories.Count > 0)
                {
                    integrationKeys = integrationManager.GetAllEntityKeys(Entities.ProductCategory);

                    foreach (var comCategory in catalog.Categories)
                    {
                        // Skipping Root Category as we do not move root category
                        if (comCategory.ParentCategory != 0)
                        {
                            key = this.GetIntegrationKey(integrationKeys, comCategory.RecordId);

                            if (key != null && !string.IsNullOrEmpty(key.ComKey))
                            {
                                // Update Category in ECOM
                                bool status = this.UpdateCategory(comCategory, int.Parse(key.ComKey));
                            }
                            else
                            {

                                key = this.GetIntegrationKey(integrationKeys, comCategory.ParentCategory);

                                if (key != null && !string.IsNullOrEmpty(key.ComKey))
                                {
                                    parentCategoryId = int.Parse(key.ComKey);
                                }
                                else
                                {
                                    parentCategoryId = configurationHelper.GetSetting(ECOM.Root_Category_Id).IntValue();
                                }

                                // Create Category in ECOM
                                comKey = this.CreateCategory(comCategory, parentCategoryId);

                                if (!string.IsNullOrWhiteSpace(comKey))
                                {
                                    // Create New Key in Integration DB
                                    integrationManager.CreateIntegrationKey(Entities.ProductCategory, comCategory.RecordId.ToString(), comKey);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
                throw;
            }
            finally
            {
                if (integrationKeys != null)
                {
                    integrationKeys.Clear();
                }
            }
        }

        private bool UpdateCategory(ErpCategory comCategory, int v)
        {
            throw new NotImplementedException();
        }

        private string CreateCategory(ErpCategory comCategory, int parentCategoryId)
        {
            complexTypeCategory category = new complexTypeCategory();
            category.categoryid = comCategory.EcomCategoryId;
            category.displayname = new sharedTypeLocalizedString [0];
            return category.categoryid;
        }

        #endregion
        /// <summary>
        /// GetIntegrationKey gets integration key from list.
        /// </summary>
        /// <param name="integrationKeys"></param>
        /// <param name="erpId"></param>
        /// <returns></returns>
        private IntegrationKey GetIntegrationKey(List<IntegrationKey> integrationKeys, long erpId)
        {
            string key = erpId.ToString();
            return integrationKeys.FirstOrDefault(k => k.ErpKey == key);
        }
    }
}