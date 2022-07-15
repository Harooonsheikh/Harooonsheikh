using System;
using System.Collections.Generic;
using System.Linq;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
{
    /// <summary>
    /// ProductController class performs Product related activities.
    /// </summary>
    public class ProductController : ProductBaseController, IProductController
    {

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProductController(string storeKey) : base(false, storeKey)
        {
        }

        #endregion

        #region Public Methods
        CustomLogger customLogger = new CustomLogger();
        /// <summary>
        /// PushProducts push products to Ecom.
        /// </summary>
        /// <param name="catalog"></param>
        public void PushProducts(ErpCatalog catalog)
        {
            CustomLogger customLogger = new CustomLogger();
            customLogger.LogDebugInfo(string.Format("Enter in PushProducts()"));
            try
            {
                string categoryAssignment = configurationHelper.GetSetting(ECOM.Category_Assignment);
                CategoryController categoryController = new CategoryController(currentStore.StoreKey);
                if ((configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue()))
                {
                    if (catalog != null && catalog.Products != null)
                    {
                        this.CreateProductFile(catalog);
                        
                        customLogger.LogDebugInfo(string.Format("Exit from CreateProductFile()"));
                    }

                    this.UpdateIntegrationData(catalog);
                }
                else // standard Product flow
                {
                    if (categoryAssignment.Equals(CATEGORYASSIGNMENT.SINGLE.ToString()))
                    {

                        //CategoryController categoryController = new CategoryController();
                        //categoryController.ProcessCategories(catalog.Categories);

                        //this.categoryAssignmentForFlatProducts(catalog, categoryController);

                        //TO-DO nedd to check single or root category in case of standard product flow
                    }
                    else if (categoryAssignment.Equals(CATEGORYASSIGNMENT.ALL.ToString()))
                    {
                        categoryController.ProcessCategories(catalog.Categories);
                        this.ProcessProducts(catalog, categoryController);
                    }

                    if (catalog != null && catalog.Categories != null && catalog.CategoryAssignments != null && catalog.DimensionSets != null && catalog.Products != null)
                    {
                        this.CreateProductFile(catalog);
                    }
                    this.UpdateIntegrationData(catalog);
                    categoryController.UpdateIntegrationData(catalog);
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Private Methods

        private void categoryAssignmentForFlatProducts(ErpCatalog catalog, CategoryController categoryController)
        {
            catalog.CategoryAssignments = new List<ErpCategoryAssignment>();

            foreach (ErpProduct prod in catalog.Products)
            {
                categoryController.ProcessProductCategory(prod, catalog.Categories, catalog.CategoryAssignments);
            }
        }
        /// <summary>
        /// ProcessProducts processes product and initialize additional and required data items.
        /// </summary>
        /// <param name="catalog"></param>
        /// <param name="categoryController"></param>
        private void ProcessProducts(ErpCatalog catalog, CategoryController categoryController)
        {
            catalog.CategoryAssignments = new List<ErpCategoryAssignment>();
            catalog.DimensionSets = new List<ErpProductDimensionSet>();

            foreach (ErpProduct prod in catalog.Products)
            {
                // Skip Category Assignments, Dimensions and Prefix for deleted products
                if (prod.Mode == ErpChangeMode.Delete)
                {
                    continue;
                }
                // Master/Variant Product Hierarchy
                // SK - verify this is a part of standard ?
                //Additional Check for performance, as category assignment is only required for products, not on variants.
                if (prod.IsMasterProduct || prod.MasterProductId == 0)
                {
                    prod.SKU = prod.EcomProductId = base.PrefixSKU(prod.ItemId);
                    categoryController.ProcessProductCategory(prod, catalog.Categories, catalog.CategoryAssignments);
                }
                else
                {
                    prod.SKU = prod.EcomProductId = base.PrefixSKU(prod.VariantId);
                }
            }
            // Process Delete Category Assignments
            if (configurationHelper.GetSetting(PRODUCT.Category_Assignment_Delete).ToLower() == "false")
                categoryController.ProcessDeletedCategoryAssignments(catalog);
        }

        /// <summary>
        /// ProcessProductDimensions processes product dimensions to prepare final list.
        /// </summary>
        /// <param name="dimensionSets"></param>
        /// <param name="product"></param>
        private void ProcessProductDimensions(List<ErpProductDimensionSet> dimensionSets, ErpProduct product)
        {
            List<string> finalDimValues;
            ErpProductDimensionSet finalDimSet;
            List<ErpProductDimensionValueSet> prodDimValueSets;

            if (product.DimensionSets != null && product.DimensionSets.Count > 0)
            {
                foreach (var prodDimSet in product.DimensionSets)
                {
                    prodDimSet.DimensionKey = prodDimSet.DimensionKey.ToLower();
                    finalDimSet = dimensionSets.FirstOrDefault(ds => ds.DimensionKey.Equals(prodDimSet.DimensionKey, StringComparison.CurrentCultureIgnoreCase));

                    if (finalDimSet != null)
                    {
                        finalDimValues = finalDimSet.DimensionValues.Select(ds => ds.DimensionValue).ToList();
                        //prodDimValues = prodDimSet.DimensionValues.Except(finalDimSet.DimensionValues).ToList();
                        prodDimValueSets = prodDimSet.DimensionValues.Where(dv => !finalDimValues.Contains(dv.DimensionValue)).ToList();
                        //prodDimSet.DimensionValues.Except().ToList();
                        if (prodDimValueSets != null && prodDimValueSets.Count() > 0)
                        {
                            foreach (var pv in prodDimValueSets)
                            {
                                finalDimSet.DimensionValues.Add(pv);
                            }
                        }
                    }
                    else
                    {
                        dimensionSets.Add(prodDimSet);
                    }
                }
            }
        }

        /// <summary>
        /// Creates Catalog XML file
        /// </summary>
        /// <param name="catalog">Catalog object containing Categories, Category Assignments and Products</param>
        private void CreateProductFile(ErpCatalog catalog)
        {
          
            customLogger.LogDebugInfo(string.Format("Enter in CreateProductFile()"));
            try
            {
                string fileNameProduct = configurationHelper.GetSetting(PRODUCT.Filename_Prefix) + DateTime.UtcNow.ToString("yyyyMMddhhmm") + ".xml";
                XmlTemplateHelper xmlHelper = new XmlTemplateHelper();
                xmlHelper.GenerateXmlUsingTemplate(fileNameProduct, ConfigurationHelper.GetDirectory(configurationHelper.GetSetting(PRODUCT.Local_Output_Path)), XmlTemplateHelper.XmlSourceDirection.CREATE, catalog);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// UpdateIntegrationData updates integration table.
        /// </summary>
        /// <param name="products"></param>
        private void UpdateIntegrationData(ErpCatalog catalog)
        {
            CustomLogger customLogger = new CustomLogger();
            customLogger.LogDebugInfo(string.Format("Enter in UpdateIntegrationData()"));
            bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            //AF:Start
            // Update Products to Integration Table
            foreach (var item in catalog.Products.Where(p => p.Mode != ErpChangeMode.Delete))
            {
                // Check if the item.EcomProductId is present in the IntegreationKey table. item.EcomProductId is equal to item id of ax
                var key = integrationManager.GetErpKey(Entities.Product, item.EcomProductId);

                if (key == null)
                {
                    if (isFlatProductHierarchy)
                    {
                        integrationManager.CreateIntegrationKey(Entities.Product, item.RecordId.ToString(), item.SKU, item.ItemId);
                    }
                    else
                    {
                        integrationManager.CreateIntegrationKey(Entities.Product, item.RecordId.ToString(), item.EcomProductId, item.MasterProductNumber + ":" + item.VariantId);
                    }
                }
            }
            if (!(bool.Parse(configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable))))
            {
                // Update Category Assignments to Integration Table
                foreach (var item in catalog.CategoryAssignments.Where(c => c.Mode == ErpChangeMode.Insert))
                {
                    var key = integrationManager.GetKey(Entities.CategoryAssignment, item.CategoryId, item.ProductId);

                    if (key == null)
                    {
                        integrationManager.CreateIntegrationKey(Entities.CategoryAssignment, item.CategoryId.ToString(), item.ProductId, "Primary:" + item.PrimaryFlag);
                    }
                }
                List<IntegrationKey> deleteKeys = new List<IntegrationKey>();
                // Delete Category Assignments to Integration Table
                foreach (var item in catalog.CategoryAssignments.Where(c => c.Mode == ErpChangeMode.Delete))
                {
                    var key = integrationManager.GetKey(Entities.CategoryAssignment, item.CategoryId, item.ProductId);

                    if (key != null)
                    {
                        deleteKeys.Add(key);
                    }
                }
                if (deleteKeys.Count > 0)
                {
                    integrationManager.DeleteEntityKeys(deleteKeys);
                }
            }

            //AF:Start
          
            customLogger.LogDebugInfo(string.Format("Exit from UpdateIntegrationData()"));
        }

        public void PushProductImages(List<KeyValuePair<string, string>> images)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
