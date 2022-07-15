using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.CommerceLink.DemandwareAdapter.Controllers
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

        /// <summary>
        /// PushProducts push products to Ecom.
        /// </summary>
        /// <param name="catalog"></param>
        public string PushProducts(ErpCatalog catalog, ErpChannel channel, string fileName)
        {
            CustomLogger.LogDebugInfo(string.Format("Enter in PushProducts()"), currentStore.StoreId, currentStore.CreatedBy);
            string content = string.Empty;
            try
            {
                //AF:Start   

                string categoryAssignment = configurationHelper.GetSetting(ECOM.Category_Assignment);

                CategoryController categoryController = new CategoryController(currentStore.StoreKey);

                if ((configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue()))
                {

                    if (catalog != null && catalog.Products != null)
                    {
                        content= this.CreateProductFile(catalog, fileName);
                        CustomLogger.LogDebugInfo(string.Format("Exit from CreateProductFile()"), currentStore.StoreId, currentStore.CreatedBy);
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

                        content= this.CreateProductFile(catalog, fileName);

                    }

                    this.UpdateIntegrationData(catalog);
                    categoryController.UpdateIntegrationData(catalog);

                }

                //AF:End
                return content;

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
                    //prod.SKU = prod.EcomProductId = ConfigurationHelper.SiteCode + prod.ItemId;

                    prod.SKU = prod.EcomProductId = base.PrefixSKU(prod.ItemId);

                    categoryController.ProcessProductCategory(prod, catalog.Categories, catalog.CategoryAssignments);

                    //this.ProcessProductDimensions(catalog.DimensionSets, prod);

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
        private string CreateProductFile(ErpCatalog catalog, string fileName)
        {
            String content = String.Empty;
            String strFileDirectory = String.Empty;
            ObjectToCsvConverter objectToCsvConverter = new ObjectToCsvConverter();
            CustomLogger.LogDebugInfo(string.Format("Enter in CreateProductFile()"), currentStore.StoreId, currentStore.CreatedBy);
            try
            {
                if (configurationHelper.GetSetting(ECOM.Product_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_CSV)
                {
                    #region CSV File Generation
                    fileName = fileName + "." + ApplicationConstant.FILE_TYPE_CSV.ToLower();
                    strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(PRODUCT.Local_Output_Path));
                    if (!String.IsNullOrEmpty(strFileDirectory) && !strFileDirectory.EndsWith("\\"))
                    {
                        strFileDirectory = strFileDirectory + "\\";
                    }
                    string strTemplateObjectToCsvMappingXmlFileLocation = configurationHelper.GetSetting(PRODUCT.CSV_Map_Path);
                    // Product Inventory
                    objectToCsvConverter.ConvertObjectToCsv(catalog.Products.ToArray(), strTemplateObjectToCsvMappingXmlFileLocation,
                        strFileDirectory, fileName, true, null);
                    return content;
                    #endregion
                }
                else if (configurationHelper.GetSetting(ECOM.Product_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_XML)
                {
                    #region XML File Generation
                    fileName = fileName + "." + ApplicationConstant.FILE_TYPE_XML.ToLower();
                    strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(PRODUCT.Local_Output_Path));

                    XmlTemplateHelper xmlHelper = new XmlTemplateHelper(currentStore);
                    xmlHelper.GenerateXmlUsingTemplate(fileName, strFileDirectory, XmlTemplateHelper.XmlSourceDirection.CREATE, catalog);
                    return content;
                    #endregion
                }
                return content;
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

            CustomLogger.LogDebugInfo(string.Format("Enter in UpdateIntegrationData()"), currentStore.StoreId, currentStore.CreatedBy);
            bool isFlatProductHierarchy = configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue();

            //AF:Start
            // Update Products to Integration Table
            foreach (var item in catalog.Products.Where(p => p.Mode != ErpChangeMode.Delete))
            {
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
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
                IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
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
            CustomLogger.LogDebugInfo(string.Format("Exit from UpdateIntegrationData()"), currentStore.StoreId, currentStore.CreatedBy);
        }

        public void PushProductImages(List<KeyValuePair<string, string>> images)
        {
            throw new NotImplementedException();
        }

        //#region CSV Methods

        ///// <summary>
        ///// CreateProductsCSV creates CSV of provided products.
        ///// </summary>
        ///// <param name="products"></param>
        //private void CreateProductsCSV(List<EcomcatalogProductCreateEntity> products)
        //{
        //    //byte[] filebyte = new byte[] { };
        //    StringBuilder sb = new StringBuilder();
        //    try
        //    {
        //        this.PushProductColor(products);
        //        this.PushProductSize(products);
        //        //this.PushProductWidth(products);
        //        List<ProductCSV> csvProducts = this.ProcessProductCSVData(products);
        //        this.CreateProductCSVFile(csvProducts);
        //        this.UpdateIntegrationData(products);
        //    }
        //    catch (Exception exp)
        //    {
        //        TransactionLogging obj = new TransactionLogging();
        //        obj.LogTransaction(SyncJobs.ProductSync, "Product CSV generation Failed", DateTime.UtcNow, null);
        //        CustomLogger.LogException(exp);
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// CreateProductsCSVExt creates CSV of provided products.
        ///// </summary>
        ///// <param name="products"></param>
        //private void CreateProductsCSVExt(List<EcomcatalogProductCreateEntity> products)
        //{
        //    List<ProductCSV> csvProducts = new List<ProductCSV>();
        //    //List<RelatedProductCSV> csvRelatedProducts = new List<RelatedProductCSV>();
        //    List<ProductImageCSV> csvProductImages = new List<ProductImageCSV>();
        //    try
        //    {
        //        this.PushProductColor(products);
        //        this.PushProductSize(products);
        //        //this.PushProductWidth(products);
        //        this.ProcessProductCSVDataExt(products, csvProducts, csvProductImages);//csvRelatedProducts, 
        //        this.CreateProductCSVFile(csvProducts);
        //        //this.CreateProductRelatedCSVFile(csvRelatedProducts);
        //        this.CreateProductImageCSVFile(csvProductImages);
        //        this.UpdateIntegrationData(products);
        //    }
        //    catch (Exception exp)
        //    {
        //        TransactionLogging obj = new TransactionLogging();
        //        obj.LogTransaction(SyncJobs.ProductSync, "Product CSV generation Failed", DateTime.UtcNow, null);
        //        CustomLogger.LogException(exp);
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// CreateProductCSVFile create product csv.
        ///// </summary>
        ///// <param name="products"></param>
        //private void CreateProductCSVFile(List<ProductCSV> products)
        //{
        //    if (products != null && products.Count > 0)
        //    {
        //        string fileName = FileHelper.GetProductCSVFileName();
        //        List<MapTemplate> fieldMaps = XMLHelper.LoadMap(ConfigurationHelper.ProductCSVMap);
        //        string csvData = CSVWriter.Write(products, true, fileName, fieldMaps);

        //        //bool upladFiles = true;
        //        ////TODO: Temp test code to disbale products upload
        //        //try
        //        //{
        //        //    upladFiles = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("UploadFileToFTP"));
        //        //}
        //        //catch
        //        //{
        //        //}

        //        //if (upladFiles)
        //        //{
        //        //    SFTPManager.UploadFile(fileName, ConfigurationHelper.ProductFTPPath);
        //        //    FileHelper.MoveFileToLocalFolder(fileName, "Processed", ConfigurationHelper.ProductOutputPath);
        //        //}

        //        byte[] filebyte = System.Text.Encoding.UTF8.GetBytes(csvData);
        //        TransactionLogging obj = new TransactionLogging();
        //        obj.LogTransaction(SyncJobs.ProductSync, "Product Sync CSV generated Successfully", DateTime.UtcNow, filebyte);
        //    }
        //}

        ///// <summary>
        ///// CreateProductRelatedCSVFile creates related product csv.
        ///// </summary>
        ///// <param name="products"></param>
        //private void CreateProductRelatedCSVFile(List<RelatedProductCSV> products)
        //{
        //    if (products != null && products.Count > 0)
        //    {
        //        string fileName = FileHelper.GetProductRelatedCSVFileName();

        //        string csvData = CSVWriter.Write(products, true, fileName, null);

        //        SFTPManager.UploadFile(fileName, ConfigurationHelper.ProductRelatedFTPPath);

        //        FileHelper.MoveFileToLocalFolder(fileName, "Processed", ConfigurationHelper.ProductOutputPath);

        //        byte[] filebyte = System.Text.Encoding.UTF8.GetBytes(csvData);
        //        TransactionLogging obj = new TransactionLogging();
        //        obj.LogTransaction(SyncJobs.ProductSync, "Product Related Sync CSV generated Successfully", DateTime.UtcNow, filebyte);
        //    }
        //}

        ///// <summary>
        ///// CreateProductImageCSVFile creates product image csv.
        ///// </summary>
        ///// <param name="products"></param>
        //private void CreateProductImageCSVFile(List<ProductImageCSV> products)
        //{
        //    if (products != null && products.Count > 0)
        //    {
        //        string fileName = FileHelper.GetProductImageCSVFileName();
        //        string csvData = CSVWriter.Write(products, true, fileName, null);

        //        //bool upladFiles = true;
        //        ////TODO: Temp test code to disbale products upload
        //        //try
        //        //{
        //        //    upladFiles = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("UploadFileToFTP"));
        //        //}
        //        //catch
        //        //{
        //        //}
        //        //if (upladFiles)
        //        //{
        //        //    SFTPManager.UploadFile(fileName, ConfigurationHelper.ProductImageFTPPath);
        //        //    FileHelper.MoveFileToLocalFolder(fileName, "Processed", ConfigurationHelper.ProductOutputPath);
        //        //}

        //        byte[] filebyte = System.Text.Encoding.UTF8.GetBytes(csvData);
        //        TransactionLogging obj = new TransactionLogging();
        //        obj.LogTransaction(SyncJobs.ProductSync, "Product Image Sync CSV generated Successfully", DateTime.UtcNow, filebyte);
        //    }
        //}

        //#endregion

        //#region Push Attributes

        ///// <summary>
        ///// PushProductColor identifies new Colors and push them to Magento.
        ///// </summary>
        ///// <param name="products"></param>
        //private void PushProductColor(List<EcomcatalogProductCreateEntity> products)
        //{
        //    List<string> colorsList = products.Where(p => !string.IsNullOrEmpty(p.Color) && p.Color.ToLower() != "null").Select(p => p.Color).Distinct().ToList();

        //    this.PushProductAttribute(colorsList, ConfigurationHelper.ColorAttributeName);
        //}

        ///// <summary>
        ///// PushProductSize identifies new Sizes and push them to Magento.
        ///// </summary>
        ///// <param name="products"></param>
        //private void PushProductSize(List<EcomcatalogProductCreateEntity> products)
        //{
        //    List<string> sizeList = products.Where(p => !string.IsNullOrEmpty(p.Size) && p.Size.ToLower() != "null").Select(p => p.Size).Distinct().ToList();

        //    this.PushProductAttribute(sizeList, ConfigurationHelper.SizeAttributeName);
        //}

        ///// <summary>
        ///// PushProductWidth identifies new Width and push them to Magento.
        ///// </summary>
        ///// <param name="products"></param>
        //private void PushProductWidth(List<EcomcatalogProductCreateEntity> products)
        //{
        //    List<string> widthList = products.Where(p => !string.IsNullOrEmpty(p.Style) && p.Style.ToLower() != "null").Select(p => p.Style).Distinct().ToList();

        //    this.PushProductAttribute(widthList, ConfigurationHelper.WidthAttributeName);
        //}

        ///// <summary>
        ///// PushProductAttribute pushes newly identifed attribute values to Magento.
        ///// </summary>
        ///// <param name="attributes"></param>
        ///// <param name="attributeName"></param>
        //private void PushProductAttribute(List<string> attributes, string attributeName)
        //{
        //    if (base.Service != null)
        //    {
        //        catalogProductAttributeEntity attributeEntity = base.Service.catalogProductAttributeInfo(base.SessionId, attributeName);
        //        IEnumerable<string> newAttributes;

        //        if (attributeEntity != null && attributeEntity.options != null && attributeEntity.options.Count() > 0)
        //        {
        //            List<string> availableAttributes = attributeEntity.options.Select(a => a.label).Distinct().ToList();
        //            newAttributes = attributes.Except(availableAttributes);
        //        }
        //        else
        //        {
        //            newAttributes = attributes;
        //        }

        //        // Create new Attributes
        //        if (newAttributes != null)
        //        {
        //            foreach (var attribute in newAttributes)
        //            {
        //                this.CreateAttributeOption(attributeName, attribute);
        //            }
        //        }
        //    }
        //}

        ///// <summary>
        ///// CreateAttributeOption calls API to create attribute in Magento.
        ///// </summary>
        ///// <param name="attributeName"></param>
        ///// <param name="optionValue"></param>
        //private void CreateAttributeOption(string attributeName, string optionValue)
        //{
        //    catalogProductAttributeOptionEntityToAdd option = new catalogProductAttributeOptionEntityToAdd();
        //    catalogProductAttributeOptionLabelEntity[] labels = new catalogProductAttributeOptionLabelEntity[1];

        //    labels[0] = new catalogProductAttributeOptionLabelEntity();
        //    labels[0].store_id = new string[] { ConfigurationHelper.MageStoreID };
        //    labels[0].value = optionValue;

        //    option.label = labels;
        //    option.is_default = 0;

        //    base.Service.catalogProductAttributeAddOption(base.SessionId, attributeName, option);
        //}

        //#endregion

        #endregion
    }
}
