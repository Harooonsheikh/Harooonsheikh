using System.Collections.Generic;
using System.Linq;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
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

        /// <summary>
        /// PushCategories process provided list of categories and add and or update categories in Magento.
        /// </summary>
        /// <param name="catalog"></param>
        public void PushCategories(ErpCatalog catalog)
        {
        }
        #endregion

        #region Private Methods
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
            return deletedCategoryAssignments;
        }

        /// <summary>
        /// UpdateIntegrationData updates integration table.
        /// </summary>
        /// <param name="products"></param>
        internal void UpdateIntegrationData(ErpCatalog catalog)
        {
            // Update to Integration Table for Catagories
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            foreach (var item in catalog.Categories)
            {
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