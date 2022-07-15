using System.Collections.Generic;
using System.Linq;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.CommerceLink.DemandwareAdapter.Controllers
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

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public CategoryController(string storeKey) : base(false, storeKey)
        {
            this.TransactionLog = new TransactionLogging(currentStore.StoreKey);
        }

        #endregion

        #region Public Methods

        ///// <summary>
        ///// PushCategories process provided list of categories and add and or update categories in Magento.
        ///// </summary>
        ///// <param name="categories"></param>
        //public void PushCategories(List<EcomcatalogCategoryEntityCreate> categories)
        //{
        //    string comKey;
        //    IntegrationKey key;
        //    List<IntegrationKey> integrationKeys = null;
        //    int parentCategoryId;
        //    try
        //    {
        //        if (categories != null && categories.Count > 0)
        //        {
        //            integrationKeys = IntegrationManager.GetAllEntityKeys(Entities.ProductCategory);

        //            foreach (var comCategory in categories)
        //            {
        //                // Skipping Root Category as we do not move root category
        //                if (comCategory.parentCategoryId != 0)
        //                {
        //                    key = this.GetIntegrationKey(integrationKeys, comCategory.categoryId);

        //                    if (key != null && !string.IsNullOrEmpty(key.ComKey))
        //                    {
        //                        // Update Categorty in ECOM
        //                        bool status = this.UpdateCategory(comCategory, int.Parse(key.ComKey));
        //                    }
        //                    else
        //                    {

        //                        key = this.GetIntegrationKey(integrationKeys, comCategory.parentCategoryId);

        //                        if (key != null && !string.IsNullOrEmpty(key.ComKey))
        //                        {
        //                            parentCategoryId = int.Parse(key.ComKey);
        //                        }
        //                        else
        //                        {
        //                            parentCategoryId = Convert.ToInt32(ConfigurationHelper.MageRootCatId);
        //                        }

        //                        // Create Category in ECOM
        //                        comKey = this.CreateCategory(comCategory, parentCategoryId);

        //                        if (!string.IsNullOrWhiteSpace(comKey))
        //                        {
        //                            // Create New Key in Integration DB
        //                            IntegrationManager.CreateIntegrationKey(Entities.ProductCategory, comCategory.categoryId.ToString(), comKey);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        CustomLogger.LogException(exp);
        //        throw;
        //    }
        //    finally
        //    {
        //        if (integrationKeys != null)
        //        {
        //            integrationKeys.Clear();
        //        }
        //    }
        //}

        /// <summary>
        /// PushCategories process provided list of categories and add and or update categories in Magento.
        /// </summary>
        /// <param name="catalog"></param>
        public void PushCategories(ErpCatalog catalog)
        {
        }
        #endregion

        #region Private Methods

        ///// <summary>
        ///// GetIntegrationKey gets integration key from list.
        ///// </summary>
        ///// <param name="integrationKeys"></param>
        ///// <param name="erpId"></param>
        ///// <returns></returns>
        //private IntegrationKey GetIntegrationKey(List<IntegrationKey> integrationKeys, long erpId)
        //{
        //    string key = erpId.ToString();
        //    return integrationKeys.FirstOrDefault(k => k.ErpKey == key);
        //}

        ///// <summary>
        ///// CreateCategory creates category in Magento.
        ///// </summary>
        ///// <param name="category"></param>
        ///// <param name="parentId"></param>
        ///// <returns></returns>
        //private string CreateCategory(EcomcatalogCategoryEntityCreate category, int parentId)
        //{
        //    string comKey = string.Empty;
        //    try
        //    {
        //        if (category != null)
        //        {
        //            var magCategory = Mapper.Map<EcomcatalogCategoryEntityCreate, catalogCategoryEntityCreate>(category);
        //            ////TODO: Put it in map
        //            //magCategory.is_active = 1;
        //            //magCategory.is_activeSpecified = true;
        //            //magCategory.include_in_menu = 1;
        //            //magCategory.include_in_menuSpecified = true;
        //            //magCategory.available_sort_by = new string[] { "0" };//{ "false" };

        //            //magCategory.default_sort_by = "0";// "false";

        //            //magCategory.is_core = 0;
        //            //magCategory.is_coreSpecified = true;

        //            int key = base.Service.catalogCategoryCreate(SessionId, parentId, magCategory, "default");
        //            comKey = key.ToString();

        //            this.TransactionLog.LogTransaction(SyncJobs.CategorySync, "Catagory: " + comKey + " has been created successfully", DateTime.UtcNow, null);

        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        this.TransactionLog.LogTransaction(SyncJobs.CategorySync, "Catagory: " + parentId + " has failed to create", DateTime.UtcNow, null);
        //        CustomLogger.LogException(exp);
        //        throw;
        //    }

        //    return comKey;
        //}

        ///// <summary>
        ///// UpdateCategory updates category in Magento.
        ///// </summary>
        ///// <param name="category"></param>
        ///// <param name="categoryId"></param>
        ///// <returns></returns>
        //private bool UpdateCategory(EcomcatalogCategoryEntityCreate category, int categoryId)
        //{
        //    bool isUpdated = false;
        //    try
        //    {
        //        if (category != null)
        //        {
        //            var magCategory = Mapper.Map<EcomcatalogCategoryEntityCreate, catalogCategoryEntityCreate>(category);
        //            ////TODO: Put it in map
        //            //magCategory.is_active = 1;
        //            //magCategory.is_activeSpecified = true;
        //            //magCategory.include_in_menu = 1;
        //            //magCategory.include_in_menuSpecified = true;
        //            //magCategory.available_sort_by = new string[] {"0"};//{ "false" };
        //            //magCategory.default_sort_by = "0";// "false";
        //            //magCategory.is_core = 0;
        //            //magCategory.is_coreSpecified = true;

        //            isUpdated = base.Service.catalogCategoryUpdate(SessionId, categoryId, magCategory, "default");

        //            if (isUpdated)
        //            {
        //                this.TransactionLog.LogTransaction(SyncJobs.CategorySync, "Catagory: " + categoryId + " has been updated successfully", DateTime.UtcNow, null);
        //            }
        //            else
        //            {
        //                this.TransactionLog.LogTransaction(SyncJobs.CategorySync, "Catagory: " + categoryId + " update failed", DateTime.UtcNow, null);
        //            }
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        CustomLogger.LogException(exp);
        //        throw;
        //    }

        //    return isUpdated;
        //}


        /// <summary>
        /// ProcessProductCategories processes product categories data to map with Ecom ids.
        /// </summary>
        /// <param name="products"></param>
        internal void ProcessCategories(List<ErpCategory> categories)
        {
            //AF:Start
            if (categories != null)
            {
                IntegrationKey key;
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                List<IntegrationKey> integrationKeys = integrationManager.GetAllEntityKeys(Entities.ProductCategory);
                ErpCategory parentCategory;
                int categoryPosition = 0;

                foreach (ErpCategory category in categories)
                {
                    key = integrationKeys.FirstOrDefault(k => k.ErpKey == category.RecordId.ToString());
                    if (key != null)
                    {
                        category.EcomCategoryId = key.ComKey;
                    }
                    else if (category.ParentCategory == 0)
                    {
                        category.EcomCategoryId = configurationHelper.GetSetting(ECOM.Root_Category_Id);
                    }
                    else
                    {
                        category.EcomCategoryId = category.RecordId.ToString();
                    }
                    category.Position = categoryPosition;
                    categoryPosition += 1;
                }

                foreach (ErpCategory category in categories)
                {
                    if (category.ParentCategory != 0)
                    {
                        parentCategory = categories.FirstOrDefault(cat => cat.RecordId == category.ParentCategory);

                        if (parentCategory != null)
                        {
                            category.EcomParentCategoryId = parentCategory.EcomCategoryId;
                        }
                        else
                        {
                            category.EcomParentCategoryId = category.ParentCategory.ToString();
                        }
                    }
                    else
                    {
                        category.EcomParentCategoryId = configurationHelper.GetSetting(ECOM.Root_Category_Id);
                    }
                }
            }
        }

        /// <summary>
        /// ProcessProductCategories processes product categories data to map with Ecom ids.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="categories"></param>
        /// <param name="categoryAssignments"></param>
        internal void ProcessProductCategory(ErpProduct product, List<ErpCategory> categories, List<ErpCategoryAssignment> categoryAssignments)
        {
            ErpCategory category;
            ErpCategoryAssignment catAssignment;
            bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();

            if (isFlatProductHierarchy)
            {


                catAssignment = new ErpCategoryAssignment
                {

                    CategoryId = categories.Select(d => d.EcomCategoryId).FirstOrDefault(),
                    ProductId = product.EcomProductId,
                    Mode = ErpChangeMode.Insert,
                    PrimaryFlag = true  //TODO: map to product attribute
                };
                categoryAssignments.Add(catAssignment);



            }
            else
            {

                if (product.CategoryIds != null)
                {
                    foreach (long categoryId in product.CategoryIds)
                    {
                        category = categories.FirstOrDefault(cat => cat.RecordId == categoryId);

                        //++ Setting primary category 
                        //var primaryCategory = product.CustomAttributes.FirstOrDefault(p => p.Key == "primaryCategory");

                        //if (primaryCategory.Value != null)
                        //{
                        //    var isPrimary = categories.FirstOrDefault(cat => cat.Name == primaryCategory.Value);
                        //    primaryCateotryid = isPrimary.EcomCategoryId;
                        //}

                        if (category != null)
                        {
                            catAssignment = new ErpCategoryAssignment
                            {
                                CategoryId = category.EcomCategoryId,
                                ProductId = product.EcomProductId,
                                Mode = ErpChangeMode.Insert,
                                PrimaryFlag = category.Name == product.PriemaryCategory ? true : false //TODO: map to product attribute
                            };
                            categoryAssignments.Add(catAssignment);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ProcessDeletedCategoryAssignments adds deleted categroy aassignments with mode="delete"
        /// </summary>
        /// <param name="catalog">ErpCatalog object</param>
        internal void ProcessDeletedCategoryAssignments(ErpCatalog catalog)
        {
            List<ErpCategoryAssignment> deletedAssignments = GetDeletedCategoryAssignments(catalog);

            if (deletedAssignments != null && deletedAssignments.Count > 0)
            {
                catalog.CategoryAssignments.AddRange(deletedAssignments);
            }

        }

        /// <summary>
        /// GetDeletedCategoryAssignments returns delete categorya assignments by analyzing Integration DB
        /// </summary>
        /// <param name="catalog">List of ErpCategoryAssignment objects</param>
        /// <returns></returns>
        internal List<ErpCategoryAssignment> GetDeletedCategoryAssignments(ErpCatalog catalog)
        {
            List<ErpCategoryAssignment> deletedCategoryAssignments = new List<ErpCategoryAssignment>();

            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            var publishedCategoryAssignmentKeys = integrationManager.GetAllEntityKeys(Entities.CategoryAssignment);

            var categoryAssignmens = catalog.CategoryAssignments;
            List<ErpCategoryAssignment> categoryAssignmensExt = null;
            List<long> masterIds = null;
            List<IntegrationKey> masterKeys = null;
            if (catalog.CatalogMasterProducts != null)
            {
                masterIds = catalog.CatalogMasterProducts.Select(x => x.MasterProductId).ToList();
                masterKeys = integrationManager.GetMasterProductEcomKey(masterIds);
                categoryAssignmensExt = masterKeys.Select(x => new ErpCategoryAssignment() { CategoryId = x.ErpKey, ProductId = x.ComKey }).ToList();
            }

            List<IntegrationKey> deleted = (from a in publishedCategoryAssignmentKeys
                                            where !categoryAssignmens.Any(x => x.CategoryId == a.ErpKey && x.ProductId == a.ComKey) && !categoryAssignmensExt.Any(x => x.CategoryId == a.ErpKey && x.ProductId == a.ComKey)
                                            select a).ToList<IntegrationKey>();

            if (deleted.Count > 0)
            {
                foreach (var d in deleted)
                {
                    deletedCategoryAssignments.Add(
                        new ErpCategoryAssignment()
                        {
                            CategoryId = d.ErpKey,
                            ProductId = d.ComKey,
                            Mode = ErpChangeMode.Delete
                        }
                        );
                }
            }

            #region Commented by KAR - Delete is not inscope for MF and also need to refine this logic to work with Delta as well
            /*
            List<IntegrationKey> deleted = (from a in publishedCategoryAssignmentKeys
                                            where !categoryAssignmens.Any(x => x.CategoryId == a.ErpKey
                                                                          && x.ProductId == a.ComKey)
                                            select a).ToList<IntegrationKey>();

            if (deleted.Count > 0)
            {
                foreach (var d in deleted)
                {
                    deletedCategoryAssignments.Add(
                        new ErpCategoryAssignment()
                        {
                            CategoryId = d.ErpKey,
                            ProductId = d.ComKey,
                            Mode = ErpChangeMode.Delete
                        }
                        );

                }
            }
            */
            #endregion
            return deletedCategoryAssignments;
        }

        /// <summary>
        /// UpdateIntegrationData updates integration table.
        /// </summary>
        /// <param name="products"></param>
        internal void UpdateIntegrationData(ErpCatalog catalog)
        {
            // Update to Integration Table for Catagories
            foreach (var item in catalog.Categories)
            {
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                var key = integrationManager.GetErpKey(Entities.ProductCategory, item.EcomCategoryId);
                if (key == null)
                {
                    integrationManager.CreateIntegrationKey(Entities.ProductCategory, item.RecordId.ToString(), item.EcomCategoryId);
                }
            }
        }
        #endregion
    }
}