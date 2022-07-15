using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using VSI.CommerceLink.MagentoAdapter.DataModels;
using VSI.EDGEAXConnector.AXCommon;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.CommerceLink.MagentoAdapter.Controllers
{

    /// <summary>
    /// ProductController class performs Product related activities.
    /// </summary>
    public class ProductController : ProductBaseController, IProductController
    {
        private static List<ConfigurableObject> configurableObjects = null;
        private static List<StoreDto> stores = null;

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
            CommerceLinkLogger.LogSyncTrace($"Enter Method [PushProducts]" +
                                                          $"Magento Logic Started");

            CustomLogger.LogDebugInfo(string.Format("Enter in PushProducts()"), currentStore.StoreId, currentStore.CreatedBy);
            string content = string.Empty;
            string categoryAssignment = configurationHelper.GetSetting(ECOM.Category_Assignment);

            CategoryController categoryController = new CategoryController(currentStore.StoreKey);

            // For Master Products
            //SetEcomLanguageByRetailChannelId(catalog.CatalogMasterProducts);
            //For Other Products
            SetEcomLanguageByRetailChannelId(catalog.Products);

            if ((configurationHelper.GetSetting(PRODUCT.Flat_Hierarchy_Enable).BoolValue()))
            {
                #region If flat hirarchy enabled

                //if (categoryAssignment.Equals(CATEGORYASSIGNMENT.SINGLE.ToString()))
                //{
                //    CustomLogger.LogDebugInfo(string.Format("{0} Category Assignment", CATEGORYASSIGNMENT.SINGLE.ToString()));

                //    categoryController.ProcessCategories(catalog.Categories);

                //    this.categoryAssignmentForFlatProducts(catalog, categoryController);
                //}

                //else if (categoryAssignment.Equals(CATEGORYASSIGNMENT.ALL.ToString()))
                //{
                //    //TO-DO we will check All category assignment in case of Flat product hierarchy

                //    categoryController.ProcessCategories(catalog.Categories);

                //    this.ProcessProducts(catalog, categoryController);
                //}

                if (catalog != null && catalog.Products != null)
                {
                    content = this.CreateProductFile(catalog, channel, fileName);

                    CustomLogger.LogDebugInfo(string.Format("Exit from CreateProductFile()"),currentStore.StoreId, currentStore.CreatedBy);
                }

                this.UpdateIntegrationData(catalog);
                // categoryController.UpdateIntegrationData(catalog);

                #endregion
            }
            else // standard Product flow
            {
                #region If flat hirarchy not enabled
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
                    content = this.CreateProductFile(catalog, channel, fileName);
                }

                CommerceLinkLogger.LogSyncTrace($"Updated Integration Data Started" +
                                                              $"");

                this.UpdateIntegrationData(catalog);
                categoryController.UpdateIntegrationData(catalog);

                CommerceLinkLogger.LogSyncTrace($"Updated Integration Data Completed" +
                                                              $"");

                #endregion
            }

            CommerceLinkLogger.LogSyncTrace($"Exit Method [PushProducts]" +
                                                          $"Magento Logic Ended");

            return content;
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
                    // Commented the below line as the EcomProductId should be same as SKU for Magento. So no need to override it with ItemId now.
                    //prod.EcomProductId = base.PrefixSKU(prod.ItemId);

                    categoryController.ProcessProductCategory(prod, catalog.Categories, catalog.CategoryAssignments);

                    this.ProcessProductDimensions(catalog.DimensionSets, prod);
                }
                else
                {
                    // Commented the below line as the EcomProductId should be same as SKU for Magento. So no need to override it with VariantId now.
                    //prod.EcomProductId = base.PrefixSKU(prod.VariantId);
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
        private string CreateProductFile(ErpCatalog catalog, ErpChannel channel, string fileName)
        {
            //String strCatalog = JsonConvert.SerializeObject(catalog);
            CommerceLinkLogger.LogSyncTrace($"Enter Method [CreateProductFile] " +
                                                          $"");
            String catalogContent = String.Empty;
            XmlDocument content = new XmlDocument();
            String strFileDirectory = String.Empty;
            Dictionary<int, ErpProduct> erpProductsDictionary;
            ObjectToCsvConverter objectToCsvConverter = new ObjectToCsvConverter();
            CustomLogger.LogDebugInfo(string.Format("Enter in CreateProductFile()"), currentStore.StoreId, currentStore.CreatedBy);

            List<KeyValuePair<string, string>> hirarchy = new List<KeyValuePair<string, string>>();
            //hirarchy = getTheCategoriesParentHirarchy(catalog.Categories, VSI.EDGEAXConnector.AXCommon.ChannelInfo.defaultLanguage);
            hirarchy = getTheCategoriesParentHirarchy(catalog.Categories);
            setErpCategoryNameAsItsParentHirarcy(catalog.Categories, hirarchy);
            
            AddMissingTransalationAttributes(catalog.Products, channel);

            foreach (var product in catalog.Products)
            {
                if(product.UpsellItems != null && product.UpsellItems.Count > 0)
                {
                    product.UpsellItems = product.UpsellItems.OrderBy(u => u.LinkedProductSKU).ToList();
                }
                if (product.Variants != null && product.Variants.Count > 0)
                {
                    product.Variants = product.Variants.OrderBy(u => u.VariantId).ToList();
                }
                if (product.CustomAttributes != null && product.CustomAttributes.Count > 0)
                {
                    product.CustomAttributes = product.CustomAttributes.OrderBy(u => u.Key).ToList();
                }
            }

            erpProductsDictionary = processCatalogErpProductForTranslations(catalog, channel);

            var deltaCatalog = bool.Parse(configurationHelper.GetSetting(PRODUCT.Enable_Catalog_Delta))
                ? GetDelta(catalog)
                : _mapper.Map<ErpCatalog>(catalog);

            Stopwatch timerSorting = Stopwatch.StartNew();
            CommerceLinkLogger.LogSyncTrace($"deltaCatalog.Products Sorting Started.");
            //Sort Products
            deltaCatalog.Products = deltaCatalog.Products.OrderBy(a => a.SKU).ThenBy(a => a.StoreViewCode).ToList();

            CommerceLinkLogger.LogSyncTrace($"deltaCatalog.Products Sorting Ended. {timerSorting.Elapsed.TotalSeconds} seconds");
            timerSorting.Stop();

            if (bool.Parse(configurationHelper.GetSetting(PRODUCT.Single_Consolidated_Catalog)) 
                && bool.Parse(configurationHelper.GetSetting(PRODUCT.Enable_Region_Catalog)))
            {
                var listOfProducts = deltaCatalog.Products;
                var lstRegions = StoreCodesDAL.GetRegionWiseStoreCodes();
                lstRegions?.RemoveAll(x => string.IsNullOrWhiteSpace(x.Key));

                foreach (var region in lstRegions)
                {
                    string regionFileName;
                    var comKeys = region.Select(x => x.ComValue).Distinct().ToList();
                    comKeys.Add(string.Empty); // To handle default language store code
                    var regionProducts = listOfProducts.Where(x => comKeys.Contains(x.StoreViewCode)).ToList();
                    deltaCatalog.Products = regionProducts;

                    if (configurationHelper.GetSetting(ECOM.Product_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_CSV)
                    {
                        #region CSV File Generation
                        regionFileName = fileName + "-" + region.Key + "." + ApplicationConstant.FILE_TYPE_CSV.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(PRODUCT.Local_Output_Path));
                        if (!String.IsNullOrEmpty(strFileDirectory) && !strFileDirectory.EndsWith("\\"))
                        {
                            strFileDirectory = strFileDirectory + "\\";
                        }
                        string strTemplateObjectToCsvMappingXmlFileLocation = configurationHelper.GetSetting(PRODUCT.CSV_Map_Path);
                        processErpProduct(deltaCatalog.Products, hirarchy);
                        // Product Inventory
                        catalogContent = objectToCsvConverter.ConvertObjectToCsv(deltaCatalog.Products.ToArray(), strTemplateObjectToCsvMappingXmlFileLocation,
                            strFileDirectory, regionFileName, true, null);
                        #endregion
                    }
                    else if (configurationHelper.GetSetting(ECOM.Product_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_XML)
                    {
                        #region XML File Generation
                        regionFileName = fileName + "-" + region.Key + "." + ApplicationConstant.FILE_TYPE_XML.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(PRODUCT.Local_Output_Path));

                        XmlTemplateHelper xmlHelper = new XmlTemplateHelper(currentStore);
                        content = xmlHelper.GenerateXmlUsingTemplate(regionFileName, strFileDirectory,
                            XmlTemplateHelper.XmlSourceDirection.CREATE, deltaCatalog);
                        catalogContent = XML.GetXML(content);
                        #endregion
                    }

                    deltaCatalog.Products = listOfProducts;
                }
            }
            else
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
                    processErpProduct(deltaCatalog.Products, hirarchy);
                    // Product Inventory
                    catalogContent = objectToCsvConverter.ConvertObjectToCsv(deltaCatalog.Products.ToArray(), strTemplateObjectToCsvMappingXmlFileLocation,
                        strFileDirectory, fileName, true, null);
                    #endregion
                }
                else if (configurationHelper.GetSetting(ECOM.Product_Output_Type).ToUpper() ==
                         ApplicationConstant.FILE_TYPE_XML)
                {
                    #region XML File Generation

                    fileName = fileName + "." + ApplicationConstant.FILE_TYPE_XML.ToLower();
                    strFileDirectory =
                        this.configurationHelper.GetDirectory(
                            configurationHelper.GetSetting(PRODUCT.Local_Output_Path));
                    CommerceLinkLogger.LogSyncTrace($"Entered Method [xmlHelper.GenerateXmlUsingTemplate] " +
                                                                  $"XML File Generation Started");
                    Stopwatch timerXML = Stopwatch.StartNew();
                    XmlTemplateHelper xmlHelper = new XmlTemplateHelper(currentStore);
                    content = xmlHelper.GenerateXmlUsingTemplate(fileName, strFileDirectory,
                        XmlTemplateHelper.XmlSourceDirection.CREATE, deltaCatalog);
                    catalogContent = XML.GetXML(content);
                    CommerceLinkLogger.LogSyncTrace($"Exit Method [xmlHelper.GenerateXmlUsingTemplate] " +
                                                                  $"XML File Generation Completed. Time taken {timerXML.Elapsed.TotalSeconds}");

                    #endregion
                }
            }

            removeTranslatedErpProducts(erpProductsDictionary, catalog);

            CommerceLinkLogger.LogSyncTrace($"Exit Method [CreateProductFile] " +
                                                          $"");

            return catalogContent;
        }

        private ErpCatalog GetDelta(ErpCatalog catalog)
        {
            CommerceLinkLogger.LogSyncTrace($"Entered Method [GetDelta] " +
                                                          $"XML File Generation Started");
            Stopwatch timerXML = Stopwatch.StartNew();
            try
            {
                SingleCatalogProductDAL.ClearStagingTable();

                List<SingleCatalogProductStaging> completeData = new List<SingleCatalogProductStaging>();
                var newCatalog = _mapper.Map<ErpCatalog>(catalog);
                var products = newCatalog.Products;
                foreach (ErpProduct product in products)
                {
                    SingleCatalogProductStaging data = new SingleCatalogProductStaging();

                    data.ProductRecId = product.RecordId.ToString();
                    data.SKU = product.SKU;
                    data.StoreViewCode = product.StoreViewCode;
                    data.ProductJson = _mapper.Map<CatalogProductViewModel>(product).SerializeToJson();
                    completeData.Add(data);
                }

                int.TryParse(configurationHelper.GetSetting(PRODUCT.Catalog_Delta_Batch_Size), out int batchSize);

                SingleCatalogProductDAL.Insert(completeData, batchSize == 0 ? 5000 : batchSize);

                SingleCatalogProductDAL.ProcessAndPopulateDeltaTable();

                List<SingleCatalogProductUpdatedData> updatedData = SingleCatalogProductDAL.GetDataFromDeltaTable();

                List<ErpProduct> updatedProducts = new List<ErpProduct>();

                foreach (SingleCatalogProductUpdatedData ud in updatedData)
                {
                    List<ErpProduct> selectedProducts = products.Where(p => p.RecordId == ud.RecordId && p.SKU == ud.SKU && p.StoreViewCode == ud.StoreViewCode).ToList();
                    updatedProducts.AddRange(selectedProducts);
                }

                newCatalog.Products = updatedProducts;

                CommerceLinkLogger.LogSyncTrace($"Exit Method [GetDelta] " +
                                                              $"Catalog Delta Completed. Time taken {timerXML.Elapsed.TotalSeconds}");

                return newCatalog;
            }
            catch (Exception e)
            {
                CommerceLinkLogger.LogSyncTrace($"Exit Method [GetDelta] " +
                                                              $"Catalog Delta Completed. Time taken {timerXML.Elapsed.TotalSeconds}");

                CommerceLinkLogger.LogException(currentStore, e, "GetDelta", Guid.NewGuid().ToString());
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
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);

            // Update Products to Integration Table
            foreach (var item in catalog.Products.Where(p => p.Mode != ErpChangeMode.Delete))
            {
                // Check if the item.EcomProductId is present in the IntegreationKey table. item.EcomProductId is equal to item id of ax
                var key = integrationManager.GetErpKey(Entities.Product, item.EcomProductId);

                if (key == null)
                {
                    if (isFlatProductHierarchy)
                    {
                        integrationManager.CreateIntegrationKey(Entities.Product, item.RecordId.ToString(), item.EcomProductId, item.ItemId);
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

        /// <summary>
        /// Function to enrich erpProduct
        /// </summary>
        /// <param name="erpProductsCollection"></param>
        /// <param name="hirarchy"></param>
        private void processErpProduct(List<ErpProduct> erpProductsCollection, List<KeyValuePair<string, string>> hirarchy)
        {
            if (erpProductsCollection != null)
            {
                string strConfigurableVariations = String.Empty;

                for (int i = 0; i < erpProductsCollection.Count; i++)
                {
                    // Set ProductType in desisred format for Magento
                    erpProductsCollection[i].ProductType = erpProductsCollection[i].IsMasterProduct ? "configurable" : "simple";
                    // Set Visibility in desisred format for Magento
                    erpProductsCollection[i].Visibility = erpProductsCollection[i].IsMasterProduct ? "Catalog, Search" : "Not Visible Individually";
                    // Set the Visibility value for Simple products (i.e, those that are not master product and does not have any variants and has MasterProductId equal to 0) to "Catalog, Search"
                    if (erpProductsCollection[i].IsMasterProduct == false && erpProductsCollection[i].MasterProductId <= 0 && erpProductsCollection[i].Variants == null)
                    {
                        erpProductsCollection[i].Visibility = "Catalog, Search";
                    }
                    // Set SKU in desisred format for Magento
                    //erpProductsCollection[i].SKU = erpProductsCollection[i].SKU + "_" + erpProductsCollection[i].ColorId + "_" + erpProductsCollection[i].SizeId + "_" + erpProductsCollection[i].ItemId;
                    #region Set Additional Attributes in desired format for Magento
                    if (String.IsNullOrEmpty(erpProductsCollection[i].ColorId) && String.IsNullOrEmpty(erpProductsCollection[i].SizeId) && String.IsNullOrEmpty(erpProductsCollection[i].StyleId))
                    {
                        erpProductsCollection[i].AdditionalAttributes = "";
                    }
                    else if (String.IsNullOrEmpty(erpProductsCollection[i].ColorId) && String.IsNullOrEmpty(erpProductsCollection[i].StyleId))
                    {
                        erpProductsCollection[i].AdditionalAttributes = "size=\"\"" + erpProductsCollection[i].SizeId + "\"\"";
                    }
                    else if (String.IsNullOrEmpty(erpProductsCollection[i].SizeId) && String.IsNullOrEmpty(erpProductsCollection[i].StyleId))
                    {
                        erpProductsCollection[i].AdditionalAttributes = "color=\"\"" + erpProductsCollection[i].ColorId + "\"\"";
                    }
                    else if (String.IsNullOrEmpty(erpProductsCollection[i].ColorId) && String.IsNullOrEmpty(erpProductsCollection[i].SizeId))
                    {
                        erpProductsCollection[i].AdditionalAttributes = "style\"\"" + erpProductsCollection[i].StyleId + "\"\"";
                    }
                    else if (String.IsNullOrEmpty(erpProductsCollection[i].ColorId))
                    {
                        erpProductsCollection[i].AdditionalAttributes = "size=\"\"" + erpProductsCollection[i].SizeId + "\"\",style=\"\"" + erpProductsCollection[i].StyleId + "\"\"";
                    }
                    else if (String.IsNullOrEmpty(erpProductsCollection[i].SizeId))
                    {
                        erpProductsCollection[i].AdditionalAttributes = "color=\"\"" + erpProductsCollection[i].ColorId + "\"\",style=\"\"" + erpProductsCollection[i].StyleId + "\"\"";
                    }
                    else if (String.IsNullOrEmpty(erpProductsCollection[i].StyleId))
                    {
                        erpProductsCollection[i].AdditionalAttributes = "color=\"\"" + erpProductsCollection[i].ColorId + "\"\",size=\"\"" + erpProductsCollection[i].SizeId + "\"\"";
                    }
                    else
                    {
                        erpProductsCollection[i].AdditionalAttributes =
                            "color=\"\"" + erpProductsCollection[i].ColorId + "\"\",size=\"\"" + erpProductsCollection[i].SizeId + "\"\",style=\"\"" + erpProductsCollection[i].StyleId + "\"\"";
                    }
                    #endregion

                    #region Set ConfigutableVariations in desired Magento format 
                    if (erpProductsCollection[i].IsMasterProduct)
                    {
                        erpProductsCollection[i].ConfigurableVariations = strConfigurableVariations;
                        strConfigurableVariations = String.Empty;
                        erpProductsCollection[i].ConfigurableVariationLabels = "color = Color,size = Size, ,style = Style";
                    }

                    strConfigurableVariations += "sku=" + erpProductsCollection[i].SKU + "," + erpProductsCollection[i].AdditionalAttributes.Replace("\"", "");

                    if (i + 1 < erpProductsCollection.Count)
                    {
                        if (erpProductsCollection[i + 1] != null && !erpProductsCollection[i + 1].IsMasterProduct)
                        {
                            strConfigurableVariations += "|";
                        }
                    }
                    #endregion

                    #region Set CompleteHirarchy in desired format in Magento
                    if (hirarchy != null && erpProductsCollection[i].CategoryIds != null)
                    {
                        List<String> strHirarchyCollection = new List<String>();

                        for (int j = 0; j < erpProductsCollection[i].CategoryIds.Count; j++)
                        {
                            string strDesiredHirarchy = hirarchy.First(c => c.Key == Convert.ToString(erpProductsCollection[i].CategoryIds.ToArray()[j])).Value;

                            strHirarchyCollection.Add(strDesiredHirarchy);
                        }

                        erpProductsCollection[i].CompleteHirarchy = string.Join(",", strHirarchyCollection);
                    }
                    #endregion

                    //Set ImageUrl as the first image of the ImageList
                    erpProductsCollection[i].ImageUrl =
                        (erpProductsCollection[i].ImageList != null && erpProductsCollection[i].ImageList.Count > 0) ? erpProductsCollection[i].ImageList[0].Url : "";
                }
            }
        }

        #region Private Methods

        private List<KeyValuePair<string, string>> getTheCategoriesParentHirarchy(List<ErpCategory> categoryList)
        {
            List<KeyValuePair<string, string>> categoryParentHirarchyList = new List<KeyValuePair<string, string>>();

            string strValue = "";
            string strRootNodeValue = "Default Category";
            List<String> categoriesList = new List<String>();

            for (int i = 0; i < categoryList.Count; i++)
            {
                categoriesList.Add(categoryList[i].Name);

                //if (categoryList[i].ParentCategory == 0)
                //{
                //    strRootNodeValue = categoryList[i].Name;
                //}

                long iCategoryIdToMatch = categoryList[i].ParentCategory;

                for (int j = 0; j < categoryList.Count; j++)
                {
                    // if (categoryList[i].EcomParentCategoryId == categoryList[j].EcomCategoryId)
                    if (iCategoryIdToMatch == categoryList[j].RecordId)
                    {
                        categoriesList.Add(categoryList[j].Name);
                        iCategoryIdToMatch = categoryList[j].ParentCategory;
                        j = 0;
                    }
                }

                for (int iCategoriesListCount = categoriesList.Count - 1; iCategoriesListCount >= 0; iCategoriesListCount--)
                {
                    if (iCategoriesListCount == categoriesList.Count - 1)
                    {
                        strValue = strRootNodeValue + "/" + categoriesList[iCategoriesListCount];
                        // strValue = categoriesList[iCategoriesListCount];
                    }
                    else
                    {
                        strValue = strValue + "/" + categoriesList[iCategoriesListCount];
                    }
                }

                // categoryParentHirarchyList.Add(new KeyValuePair<string, string>(categoryList[i].Name, strValue));
                categoryParentHirarchyList.Add(new KeyValuePair<string, string>(categoryList[i].RecordId.ToString(), strValue));

                categoriesList.Clear();
                strValue = "";
            }

            return categoryParentHirarchyList;
        }

        private List<KeyValuePair<string, string>> getTheCategoriesParentHirarchy(List<ErpCategory> categoryList, string language)
        {
            List<KeyValuePair<string, string>> categoryParentHirarchyList = new List<KeyValuePair<string, string>>();

            string strValue = "";
            string strRootNodeValue = "Default Category";
            List<String> categoriesList = new List<String>();

            for (int i = 0; i < categoryList.Count; i++)
            {
                categoriesList.Add(getLanguageSpecificCategory(categoryList[i], language));

                //if (categoryList[i].ParentCategory == 0)
                //{
                //    strRootNodeValue = categoryList[i].Name;
                //}

                long iCategoryIdToMatch = categoryList[i].ParentCategory;

                for (int j = 0; j < categoryList.Count; j++)
                {
                    // if (categoryList[i].EcomParentCategoryId == categoryList[j].EcomCategoryId)
                    if (iCategoryIdToMatch == categoryList[j].RecordId)
                    {
                        categoriesList.Add(getLanguageSpecificCategory(categoryList[j], language));
                        iCategoryIdToMatch = categoryList[j].ParentCategory;
                        j = 0;
                    }
                }

                for (int iCategoriesListCount = categoriesList.Count - 1; iCategoriesListCount >= 0; iCategoriesListCount--)
                {
                    if (iCategoriesListCount == categoriesList.Count - 1)
                    {
                        strValue = strRootNodeValue + "/" + categoriesList[iCategoriesListCount];
                        // strValue = categoriesList[iCategoriesListCount];
                    }
                    else
                    {
                        strValue = strValue + "/" + categoriesList[iCategoriesListCount];
                    }
                }

                // categoryParentHirarchyList.Add(new KeyValuePair<string, string>(categoryList[i].Name, strValue));
                categoryParentHirarchyList.Add(new KeyValuePair<string, string>(categoryList[i].RecordId.ToString(), strValue));

                categoriesList.Clear();
                strValue = "";
            }

            return categoryParentHirarchyList;
        }

        private void setErpCategoryNameAsItsParentHirarcy(List<ErpCategory> allErpCategories, List<KeyValuePair<string, string>> hirarchy)
        {
            foreach (ErpCategory erpCategory in allErpCategories)
            {
                int index = 0;
                for (index = 0; index <= allErpCategories.Count - 1; index++)
                {
                    if (erpCategory.RecordId.ToString() == hirarchy[index].Key)
                    {
                        erpCategory.Name = hirarchy[index].Value;
                    }
                }
            }
        }

        private String getLanguageSpecificCategory(ErpCategory erpCategory, String language)
        {
            if (erpCategory == null)
                return "";

            if (String.IsNullOrEmpty(language))
            {
                return erpCategory.Name;
            }

            language = language.Trim().ToLower();

            foreach (ErpTextValueTranslation translation in erpCategory.NameTranslations)
            {
                if (translation != null && translation.Language != null)
                {
                    if (translation.Language.Trim().ToLower() == language)
                    {
                        return translation.Text;
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// This function is used to generate additional nodes similar to the base nodes but containing translated data
        /// </summary>
        /// <param name="catalog"></param>
        /// <returns></returns>
        private Dictionary<int, ErpProduct> processCatalogErpProductForTranslations(ErpCatalog catalog, ErpChannel channel)
        {
            CommerceLinkLogger.LogSyncTrace($"Entered Method [{MethodBase.GetCurrentMethod().Name}] " +
                                                          $"");
            int iNoOfTrranslations = 0;
            int iTotalProductsWithTranslation = 0;
            Dictionary<int, ErpProduct> erpProductsDictionary = new Dictionary<int, ErpProduct>();
            StoreCodesDAL storeCodes = new StoreCodesDAL(currentStore.StoreKey);

            if (catalog == null || catalog.Products == null)
                return null;

            for (int iCatalogProductCount = 0; iCatalogProductCount < catalog.Products.Count; iCatalogProductCount++)
            {
                // reset values for every iteration
                catalog.Products[iCatalogProductCount].Locale = String.Empty;
                iTotalProductsWithTranslation = erpProductsDictionary.Count;

                for (int iLannguageCount = 0; iLannguageCount < channel.Languages.Count; iLannguageCount++)
                {
                    // Do not process for default language
                    if (channel.Languages[iLannguageCount] == channel.DefaultLanguage)
                    {
                        if ((bool.Parse(configurationHelper.GetSetting(PRODUCT.Single_Consolidated_Catalog))))
                        {
                            // No store view code for default language
                            catalog.Products[iCatalogProductCount].StoreViewCode = string.Empty;
                        }
                        else
                        {
                            catalog.Products[iCatalogProductCount].StoreViewCode = storeCodes.GetEcomStoreCode(channel.DefaultLanguage);
                        }

                        continue;
                    }

                    if (catalog.Products[iCatalogProductCount].ProductDetailTranslationsDictionary != null)
                    {
                        ErpProduct erpProduct = new ErpProduct();

                        Dictionary<String, String> keyValueProperties = new Dictionary<String, String>();

                        catalog.Products[iCatalogProductCount].ProductDetailTranslationsDictionary.TryGetValue(channel.Languages[iLannguageCount], out keyValueProperties);

                        erpProduct = populateErpProductForTranslation(catalog.Products[iCatalogProductCount], keyValueProperties, channel.Languages[iLannguageCount]);

                        if ((bool.Parse(configurationHelper.GetSetting(PRODUCT.Single_Consolidated_Catalog))))
                        {
                            List<String> storeViewCodesList = storeCodes.GetAllRelevantEcomStoreCode(channel.Languages[iLannguageCount]);

                            for (int iStoreViewCodesList = 0; iStoreViewCodesList < storeViewCodesList.Count; iStoreViewCodesList++)
                            {
                                ErpProduct copiedErpProduct = copyErpProduct(erpProduct, storeViewCodesList[iStoreViewCodesList]);

                                iNoOfTrranslations = iNoOfTrranslations + 1;

                                erpProductsDictionary.Add(iCatalogProductCount + iTotalProductsWithTranslation + iNoOfTrranslations, copiedErpProduct);
                            }
                        }
                        else
                        {
                            iNoOfTrranslations = iNoOfTrranslations + 1;

                            erpProductsDictionary.Add(iCatalogProductCount + iTotalProductsWithTranslation + iNoOfTrranslations, erpProduct);
                        }
                    }
                }

                iNoOfTrranslations = 0;
            }

            foreach (KeyValuePair<int, ErpProduct> entry in erpProductsDictionary)
            {
                catalog.Products.Insert(entry.Key, entry.Value);
            }

            CommerceLinkLogger.LogSyncTrace($"Exit Method [{MethodBase.GetCurrentMethod().Name}] " +
                                                          $"");

            return erpProductsDictionary;
        }

        /// <summary>
        /// This function will removed the additional objects added for translation
        /// </summary>
        /// <param name="erpProductsDictionary"></param>
        /// <param name="catalog"></param>
        private void removeTranslatedErpProducts(Dictionary<int, ErpProduct> erpProductsDictionary, ErpCatalog catalog)
        {
            if (erpProductsDictionary != null)
            {
                foreach (KeyValuePair<int, ErpProduct> entry in erpProductsDictionary.Reverse())
                {
                    catalog.Products.RemoveAt(entry.Key);
                }
            }
        }

        /// <summary>
        /// Function to create translated ErpProduct object
        /// </summary>
        /// <param name="baseLanguageErpObject"></param>
        /// <param name="keyValueProperties"></param>
        /// <returns></returns>
        private ErpProduct populateErpProductForTranslation(ErpProduct baseLanguageErpObject, Dictionary<String, String> keyValueProperties, String translationLocale)
        {
            const string strDescriptionKey = "Description";
            const string strProductNamePriorityKey = "EcomProductName";
            const string strProductNameKey = "ProductName";
            const string strQuantityText = "QuantityText";
            const string strQuantityBreakpoints = "QuantityBreakpoints";
            const string strQuantityLabel = "QuantityLabel";
            const string strClConfigBodyContent = "clconfigbodycontent";
            const string strClConfigHeaderContent = "clconfigheadercontent";
            const string strCartDescription = "cartdescription";
            const string strClQtySingleQtyText1 = "clqtysingleqtytext1";
            const string strLogoLinkURL = "logolinkurl";
            const string strPlpDescription = "plpdescription";
            const string strMobileDescription = "mobiledescription";
            const string strClAddonHeaderText = "claddonheadertext";
            const string strClAddonBodyText = "claddonbodytext";
            const string strProductnamecart = "productnamecart";
            const string strProductAddButton = "productaddbutton";
            const string strErpProductDescription = "erpproductdescription";

            String strValue = String.Empty;

            ErpProduct erpProductForTranslation = new ErpProduct();
            StoreCodesDAL storeCodes = new StoreCodesDAL(currentStore.StoreKey);

            erpProductForTranslation.AdditionalAttributes = baseLanguageErpObject.AdditionalAttributes;
            erpProductForTranslation.AdjustedPrice = baseLanguageErpObject.AdjustedPrice;
            erpProductForTranslation.AllocationTimeStamp = baseLanguageErpObject.AllocationTimeStamp;
            erpProductForTranslation.AvailableQuantity = baseLanguageErpObject.AvailableQuantity;
            erpProductForTranslation.Barcode = baseLanguageErpObject.Barcode;
            erpProductForTranslation.BasePrice = baseLanguageErpObject.BasePrice;
            erpProductForTranslation.CatalogId = baseLanguageErpObject.CatalogId;
            erpProductForTranslation.Categories = baseLanguageErpObject.Categories;
            erpProductForTranslation.CategoryIds = baseLanguageErpObject.CategoryIds;
            erpProductForTranslation.ChangeTrackingInformation = baseLanguageErpObject.ChangeTrackingInformation;
            erpProductForTranslation.Color = baseLanguageErpObject.Color;//
            erpProductForTranslation.ColorId = baseLanguageErpObject.ColorId;
            erpProductForTranslation.CompleteHirarchy = baseLanguageErpObject.CompleteHirarchy;
            erpProductForTranslation.CompositionInformation = baseLanguageErpObject.CompositionInformation;
            erpProductForTranslation.ConfigId = baseLanguageErpObject.ConfigId;
            erpProductForTranslation.Configuration = baseLanguageErpObject.Configuration;
            erpProductForTranslation.Context = baseLanguageErpObject.Context;
            erpProductForTranslation.CustomAttributes = new List<KeyValuePair<string, string>>(baseLanguageErpObject.CustomAttributes);
            erpProductForTranslation.DimensionSets = baseLanguageErpObject.DimensionSets;
            erpProductForTranslation.DistinctProductVariantId = baseLanguageErpObject.DistinctProductVariantId;
            erpProductForTranslation.EcomProductId = baseLanguageErpObject.EcomProductId;
            erpProductForTranslation.EntityName = baseLanguageErpObject.EntityName;
            erpProductForTranslation.ExtensionData = baseLanguageErpObject.ExtensionData;
            erpProductForTranslation.ExtensionProperties = baseLanguageErpObject.ExtensionProperties;
            erpProductForTranslation.HasLinkedProducts = baseLanguageErpObject.HasLinkedProducts;
            erpProductForTranslation.Image = baseLanguageErpObject.Image;
            erpProductForTranslation.ImageList = baseLanguageErpObject.ImageList;
            erpProductForTranslation.ImageUrl = baseLanguageErpObject.ImageUrl;
            erpProductForTranslation.IndexedProductProperties = baseLanguageErpObject.IndexedProductProperties;
            erpProductForTranslation.InventoryDimensionId = baseLanguageErpObject.InventoryDimensionId;
            erpProductForTranslation.IsKit = baseLanguageErpObject.IsKit;
            erpProductForTranslation.IsMasterProduct = baseLanguageErpObject.IsMasterProduct;
            erpProductForTranslation.IsRemote = baseLanguageErpObject.IsRemote;
            erpProductForTranslation.Item = baseLanguageErpObject.Item;
            erpProductForTranslation.ItemId = baseLanguageErpObject.ItemId;
            erpProductForTranslation.LinkedProducts = baseLanguageErpObject.LinkedProducts;
            #region Bug 23126
            erpProductForTranslation.Locale = string.Empty;// storeCodes.GetEcomStoreCode(translationLocale);
            erpProductForTranslation.StoreViewCode = storeCodes.GetEcomStoreCode(translationLocale); //erpProductForTranslation.Locale;
            #endregion
            erpProductForTranslation.MasterProductId = baseLanguageErpObject.MasterProductId;
            erpProductForTranslation.MasterProductNumber = baseLanguageErpObject.MasterProductNumber;
            erpProductForTranslation.Mode = baseLanguageErpObject.Mode;
            erpProductForTranslation.OfferId = baseLanguageErpObject.OfferId;
            erpProductForTranslation.PaddedItemId = baseLanguageErpObject.PaddedItemId;
            erpProductForTranslation.Price = baseLanguageErpObject.Price;
            erpProductForTranslation.PriemaryCategory = baseLanguageErpObject.PriemaryCategory;
            erpProductForTranslation.PrimaryCategoryId = baseLanguageErpObject.PrimaryCategoryId;
            if (keyValueProperties != null)
            {
                keyValueProperties.TryGetValue(strDescriptionKey, out strValue);
                erpProductForTranslation.Description = strValue;
                strValue = string.Empty;

                keyValueProperties.TryGetValue(strProductNamePriorityKey, out strValue);
                if (!String.IsNullOrEmpty(strValue))
                {
                    erpProductForTranslation.ProductName = strValue;
                }
                else
                {
                    keyValueProperties.TryGetValue(strProductNameKey, out strValue);
                    erpProductForTranslation.ProductName = strValue;
                }

                strValue = string.Empty;

                var listKeyValuePairForTranslatedCustomAttributes = new List<KeyValuePair<string, string>>();

                keyValueProperties.TryGetValue(strQuantityLabel, out strValue);
                listKeyValuePairForTranslatedCustomAttributes.Add(new KeyValuePair<string, string>("Quantity Label", strValue));
                strValue = string.Empty;

                keyValueProperties.TryGetValue(strClConfigBodyContent, out strValue);
                listKeyValuePairForTranslatedCustomAttributes.Add(new KeyValuePair<string, string>("cl_config_body_content", strValue));
                strValue = string.Empty;

                keyValueProperties.TryGetValue(strClConfigHeaderContent, out strValue);
                listKeyValuePairForTranslatedCustomAttributes.Add(new KeyValuePair<string, string>("cl_config_header_content", strValue));
                strValue = string.Empty;

                keyValueProperties.TryGetValue(strCartDescription, out strValue);
                listKeyValuePairForTranslatedCustomAttributes.Add(new KeyValuePair<string, string>("cart_description", strValue));
                strValue = string.Empty;

                keyValueProperties.TryGetValue(strClQtySingleQtyText1, out strValue);
                listKeyValuePairForTranslatedCustomAttributes.Add(new KeyValuePair<string, string>("cl_qty_single_qty_text1", strValue));
                strValue = string.Empty;

                keyValueProperties.TryGetValue(strLogoLinkURL, out strValue);
                listKeyValuePairForTranslatedCustomAttributes.Add(new KeyValuePair<string, string>("logo_link_url", strValue));
                strValue = string.Empty;

                keyValueProperties.TryGetValue(strProductNamePriorityKey, out strValue);
                listKeyValuePairForTranslatedCustomAttributes.Add(new KeyValuePair<string, string>("Ecom Product Name", strValue));
                strValue = string.Empty;

                keyValueProperties.TryGetValue(strClAddonHeaderText, out strValue);
                listKeyValuePairForTranslatedCustomAttributes.Add(new KeyValuePair<string, string>("cl_addon_header_text", strValue));
                strValue = string.Empty;

                keyValueProperties.TryGetValue(strClAddonBodyText, out strValue);
                listKeyValuePairForTranslatedCustomAttributes.Add(new KeyValuePair<string, string>("cl_addon_body_text", strValue));
                strValue = string.Empty;

                keyValueProperties.TryGetValue(strProductnamecart, out strValue);
                listKeyValuePairForTranslatedCustomAttributes.Add(new KeyValuePair<string, string>("product_name_cart", strValue));
                strValue = string.Empty;

                keyValueProperties.TryGetValue(strProductAddButton, out strValue);
                listKeyValuePairForTranslatedCustomAttributes.Add(new KeyValuePair<string, string>("product_add_button", strValue));
                strValue = string.Empty;

                keyValueProperties.TryGetValue(strErpProductDescription, out strValue);
                listKeyValuePairForTranslatedCustomAttributes.Add(new KeyValuePair<string, string>("erp_product_description", strValue));

                foreach (var customAttrbute in listKeyValuePairForTranslatedCustomAttributes)
                {
                    erpProductForTranslation.CustomAttributes.RemoveAll(x => x.Key == customAttrbute.Key);
                    erpProductForTranslation.CustomAttributes.Add(new KeyValuePair<string, string>(customAttrbute.Key, customAttrbute.Value));
                }
            }
            erpProductForTranslation.ProductNumber = baseLanguageErpObject.ProductNumber;
            erpProductForTranslation.ProductSchema = baseLanguageErpObject.ProductSchema;
            erpProductForTranslation.ProductsRelatedToThis = baseLanguageErpObject.ProductsRelatedToThis;
            erpProductForTranslation.ProductType = baseLanguageErpObject.ProductType;
            erpProductForTranslation.ProductVariants = baseLanguageErpObject.ProductVariants;
            erpProductForTranslation.RecordId = baseLanguageErpObject.RecordId;
            erpProductForTranslation.RelatedProducts = baseLanguageErpObject.RelatedProducts;
            erpProductForTranslation.RootCategory = baseLanguageErpObject.RootCategory;
            erpProductForTranslation.Rules = baseLanguageErpObject.Rules;
            erpProductForTranslation.Size = baseLanguageErpObject.Size;
            erpProductForTranslation.SizeId = baseLanguageErpObject.SizeId;
            erpProductForTranslation.SKU = baseLanguageErpObject.SKU;
            erpProductForTranslation.SpecialPrice = baseLanguageErpObject.SpecialPrice;
            erpProductForTranslation.Status = baseLanguageErpObject.Status;
            erpProductForTranslation.Style = baseLanguageErpObject.Style;//
            erpProductForTranslation.StyleId = baseLanguageErpObject.StyleId;
            erpProductForTranslation.UnitsOfMeasureSymbol = baseLanguageErpObject.UnitsOfMeasureSymbol;
            erpProductForTranslation.ValidFromDate = baseLanguageErpObject.ValidFromDate;
            erpProductForTranslation.ValidToDate = baseLanguageErpObject.ValidToDate;
            erpProductForTranslation.VariantId = baseLanguageErpObject.VariantId;
            erpProductForTranslation.Variants = baseLanguageErpObject.Variants;
            erpProductForTranslation.Visibility = baseLanguageErpObject.Visibility;
            erpProductForTranslation.HighestQty = baseLanguageErpObject.HighestQty;
            erpProductForTranslation.LowestQty = baseLanguageErpObject.LowestQty;
            erpProductForTranslation.ProductImageUrl = baseLanguageErpObject.ProductImageUrl;
            erpProductForTranslation.AvaTax_TaxClassId = baseLanguageErpObject.AvaTax_TaxClassId;

            #region Bug 23126
            if (baseLanguageErpObject.UpsellItems == null)
            {
                erpProductForTranslation.UpsellItems = new List<EDGEAXConnector.ERPDataModels.Custom.ErpUpsellItem>();
            }
            else
            {
                erpProductForTranslation.UpsellItems = baseLanguageErpObject.UpsellItems;
            }
            #endregion
            return erpProductForTranslation;
        }


        private ErpProduct copyErpProduct(ErpProduct erpProduct, string storeViewCode)
        {
            ErpProduct copiedErpProduct = new ErpProduct();

            copiedErpProduct = _mapper.Map<ErpProduct>(erpProduct);

            copiedErpProduct.StoreViewCode = storeViewCode;

            return copiedErpProduct;
        }

        private void SetEcomLanguageByRetailChannelId(List<ErpProduct> products)
        {
            if (configurableObjects == null)
            {
                configurableObjects = ConfigurableObjectDAL.GetConfirableObjects();
            }
            if (stores == null)
            {
                stores = StoreService.GetAllStores();
            }
            if (products != null)
            {
                foreach (var product in products)
                {
                    string ecomLanguageCodes = string.Empty;
                    if (product.CustomAttributes != null)
                    {
                        var retailChannelIds = product.CustomAttributes.FirstOrDefault(c => c.Key == "Display on Store Website").Value;
                        if (!string.IsNullOrWhiteSpace(retailChannelIds))
                        {
                            var listOfRetailChannelIds = retailChannelIds.Split(',');
                            foreach (var retailChannelId in listOfRetailChannelIds)
                            {
                                var store = stores.FirstOrDefault(s => s.RetailChannelId == retailChannelId);
                                if (store != null)
                                {
                                    var languageCodes = configurableObjects.Where(c => c.StoreId == store.StoreId && c.EntityType == 7);
                                    foreach (var languageCode in languageCodes)
                                    {
                                        if (languageCode.ComValue != null)
                                        {
                                            ecomLanguageCodes += languageCode.ComValue + ",";
                                        }
                                    }
                                }
                            }
                            if (!string.IsNullOrWhiteSpace(ecomLanguageCodes))
                            {
                                ecomLanguageCodes = ecomLanguageCodes.TrimEnd(',');
                            }
                            product.CustomAttributes.RemoveAll(c => c.Key == "Display on Store Website");
                            product.CustomAttributes.Add(new KeyValuePair<string, string>("Display on Store Website", ecomLanguageCodes));
                        }
                    }
                }
            }
        }
        private void AddMissingTransalationAttributes(List<ErpProduct> products, ErpChannel channel)
        {
            foreach (var product in products)
            {
                if (product.ProductDetailTranslationsDictionary != null)
                {
                    Dictionary<string, Dictionary<string, string>> translations = new Dictionary<string, Dictionary<string, string>>(product.ProductDetailTranslationsDictionary);
                    const string englishLangAbbrivation = "en-us";
                    var englishTranslations = translations.FirstOrDefault(x => x.Key.ToLower() == englishLangAbbrivation);
                    if (englishTranslations.Value != null && englishTranslations.Value.Count > 0)
                    {
                        if (translations.Remove(englishTranslations.Key))
                        {
                            foreach (var otherLang in translations)
                            {
                                foreach (var engLang in englishTranslations.Value)
                                {
                                    if (otherLang.Value.ContainsKey(engLang.Key))
                                    {
                                        var value = string.Empty;
                                        otherLang.Value.TryGetValue(engLang.Key, out value);
                                        if (string.IsNullOrWhiteSpace(value))
                                        {
                                            otherLang.Value[engLang.Key] = engLang.Value;
                                        }
                                    }
                                    else
                                    {
                                        otherLang.Value.Add(engLang.Key, engLang.Value);
                                    }
                                }
                            }
                            translations.Add(englishTranslations.Key, englishTranslations.Value);
                        }
                    }
                    foreach (var channelLang in channel.Languages)
                    {
                        if (!translations.ContainsKey(channelLang))
                        {
                            translations.Add(channelLang, englishTranslations.Value);
                        }
                    }
                    product.ProductDetailTranslationsDictionary = translations;
                }
            }
        }
        #endregion

        #endregion
    }
}
