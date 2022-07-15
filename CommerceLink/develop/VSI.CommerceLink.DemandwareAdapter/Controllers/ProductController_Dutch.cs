using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.GenericCSVParser;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.MagentoAdapter.DataModels;
using VSI.EDGEAXConnector.MagentoAPI.MageAPI;
using VSI.EDGEAXConnector.SFTPlib;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
{

    /// <summary>
    /// ProductController class performs Product related activities.
    /// </summary>
    public class ProductController_Dutch : ProductBaseController, IProductController
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public ProductController_Dutch()
            : base(true)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushProducts push products to Magento.
        /// </summary>
        /// <param name="products"></param>
        public void PushProducts(List<EcomcatalogProductCreateEntity> products)
        {
            try
            {
                if (products != null && products.Count > 0)
                {
                    this.ProcessProductCategories(products);
                    this.CreateProductsCSVExt(products);
                    //this.CreateProductsAPI(products);
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
                throw;
            }
        }

        /// <summary>
        /// PushProductImages push product images to Magento
        /// </summary>
        /// <param name="images"></param>
        public void PushProductImages(List<KeyValuePair<string, string>> images)
        {
            try
            {
                string fileName = FileHelper.GetProductImageCSVFileName();

                using (var w = new StreamWriter(fileName))
                {
                    var lineH = string.Format("{0},{1},{2},{3},{4}", "store", "websites", "type", "SKU", "image");
                    w.WriteLine(lineH);
                    w.Flush();

                    foreach (var item in images)
                    {
                        var line = string.Format("{0},{1},{2},{3},{4}", "admin", "base", "configurable", item.Key, item.Value);
                        w.WriteLine(line);
                        w.Flush();
                    }
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp);
                throw;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// UpdateIntegrationData updates integration table.
        /// </summary>
        /// <param name="products"></param>
        private void UpdateIntegrationData(List<EcomcatalogProductCreateEntity> products)
        {
            // Update to Integration Table
            foreach (var item in products)
            {
             //   var key = IntegrationManager.GetErpKey(Entities.Product, item.SKU);

           //     if (key == null)
                {
                 //   IntegrationManager.CreateIntegrationKey(Entities.Product, item.RecordId.ToString(), item.SKU, item.ItemId + ":" + item.VariantId);
                }

            }
            //var masterProducts = from p in products
            //                     where p._type.Equals("configurable")
            //                     select p;
            //foreach (var item in masterProducts)
            //{
            //    var key = IntegrationManager.GetErpKey(Entities.Product, item.SKU);

            //    if (key == null)
            //    {
            //        IntegrationManager.CreateIntegrationKey(Entities.Product, item.RecordId.ToString(), item.SKU);
            //    }

            //}
        }

        /// <summary>
        /// ProcessProductCategories processes product categories data to map with ECom ids.
        /// </summary>
        /// <param name="products"></param>
        private void ProcessProductCategories(List<EcomcatalogProductCreateEntity> products)
        {
            IntegrationKey key;
            List<IntegrationKey> integrationKeys=null;// IntegrationManager.GetAllEntityKeys(Entities.ProductCategory);
            foreach (EcomcatalogProductCreateEntity p in products)
            {
                if (p.category_ids != null)
                {
                    for (int i = 0; i < p.category_ids.Length; i++)
                    {
                        key = integrationKeys.FirstOrDefault(k => k.ErpKey == p.category_ids[i]);
                        if (key != null)
                        {
                            p.category_ids[i] = key.ComKey;
                        }
                        else
                        {
                            p.category_ids[i] = string.Empty;
                        }
                        p.category_ids[i] = key == null ? string.Empty : key.ComKey;
                    }
                }
            }

            //TODO: Old Code kept for performance comarision only, To be Deleted
            //products.ForEach(p =>
            //{
            //    if (p.category_ids != null)
            //    {
            //        for (int i = 0; i < p.category_ids.Length; i++)
            //        {
            //            var key = IntegrationManager.GetComKey(Entities.ProductCategory, p.category_ids[i]);
            //            p.category_ids[i] = key == null ? string.Empty : key.ComKey;
            //        }
            //    }
            //});
        }

        //Obseleted Code to handle Product with API call.
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="products"></param>
        //private void CreateProductsAPI(List<EcomcatalogProductCreateEntity> products)
        //{

        //    //var magentoProducts = Mapper.Map<List<EcomcatalogProductCreateEntity>, List<
        //    //                        catalogProductCreateEntity>>(products);

        //    foreach (var p in products)
        //    {

        //        var mp = Mapper.Map<EcomcatalogProductCreateEntity, catalogProductCreateEntity>(p);

        //        mp.stock_data = new catalogInventoryStockItemUpdateEntity()
        //        {
        //            is_in_stock = 1,
        //            is_in_stockSpecified = true,
        //            qty = "10",
        //            min_qty = 0,
        //            min_qtySpecified = false,
        //            use_config_min_qty = 1,
        //            use_config_min_qtySpecified = true,
        //            use_config_backorders = 1,
        //            use_config_backordersSpecified = true,
        //            min_sale_qtySpecified = false,
        //            min_sale_qty = 0
        //        };
        //        //mp.visibility = "4";

        //        //mp.status = "1";

        //        //mp.tax_class_id = "2";

        //        mp.websites = new string[] { "base" };

        //        //mp.weight = "10";
        //        //if (p._type.Equals("simple"))
        //        //{
        //        //    mp.additional_attributes = new catalogProductAdditionalAttributesEntity();
        //        //    //List<associativeEntity> associativeEntities = new List<associativeEntity>();
        //        //    associativeEntity aeColor = new associativeEntity();
        //        //    aeColor.key = "color";
        //        //    aeColor.value = p.Color;

        //        //    associativeEntity aeSize = new associativeEntity();
        //        //    aeSize.key = "size";
        //        //    aeSize.value = p.Size;

        //        //    mp.additional_attributes.single_data = new associativeEntity[] { aeColor, aeSize };

        //        //    int productId = service.catalogProductCreate(sessionId, "simple", "4", p.SKU + "-" + p.VariantId, mp, "");

        //        //}
        //        //else
        //        //{
        //        //    int productId = service.catalogProductCreate(sessionId, "configurable", "4", p.SKU, mp, "");
        //        //}


        //    }
        //}

        #region CSV Methods

        /// <summary>
        /// CreateProductsCSV creates CSV of provided products.
        /// </summary>
        /// <param name="products"></param>
        private void CreateProductsCSV(List<EcomcatalogProductCreateEntity> products)
        {
            //byte[] filebyte = new byte[] { };
            StringBuilder sb = new StringBuilder();
            try
            {
                this.PushProductColor(products);
                this.PushProductSize(products);
                //this.PushProductWidth(products);
                List<ProductCSV> csvProducts = null;//= this.ProcessProductCSVData(products);
                this.CreateProductCSVFile(csvProducts);
                this.UpdateIntegrationData(products);
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging();
             //   obj.LogTransaction(SyncJobs.ProductSync, "Product CSV generation Failed", DateTime.Now, null);
                CustomLogger.LogException(exp);
                throw;
            }
        }

        /// <summary>
        /// CreateProductsCSVExt creates CSV of provided products.
        /// </summary>
        /// <param name="products"></param>
        private void CreateProductsCSVExt(List<EcomcatalogProductCreateEntity> products)
        {
            List<ProductCSV> csvProducts = new List<ProductCSV>();
            //List<RelatedProductCSV> csvRelatedProducts = new List<RelatedProductCSV>();
            List<ProductImageCSV> csvProductImages = new List<ProductImageCSV>();
            try
            {
                this.PushProductColor(products);
                this.PushProductSize(products);
                //this.PushProductWidth(products);
            //    this.ProcessProductCSVDataExt(products, csvProducts, csvProductImages);//csvRelatedProducts, 
                this.CreateProductCSVFile(csvProducts);
                //this.CreateProductRelatedCSVFile(csvRelatedProducts);
                this.CreateProductImageCSVFile(csvProductImages);
                this.UpdateIntegrationData(products);
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging();
             //   obj.LogTransaction(SyncJobs.ProductSync, "Product CSV generation Failed", DateTime.Now, null);
                CustomLogger.LogException(exp);
                throw;
            }
        }

        /// <summary>
        /// CreateProductCSVFile create product csv.
        /// </summary>
        /// <param name="products"></param>
        private void CreateProductCSVFile(List<ProductCSV> products)
        {
            if (products != null && products.Count > 0)
            {
                string fileName = FileHelper.GetProductCSVFileName();
                List<MapTemplate> fieldMaps = XMLHelper.LoadMap(ConfigurationHelper.ProductCSVMap);
                string csvData = CSVWriter.Write(products, true, fileName, fieldMaps);

                //bool upladFiles = true;
                ////TODO: Temp test code to disbale products upload
                //try
                //{
                //    upladFiles = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("UploadFileToFTP"));
                //}
                //catch
                //{
                //}

                //if (upladFiles)
                //{
                //    SFTPManager.UploadFile(fileName, ConfigurationHelper.ProductFTPPath);
                //    FileHelper.MoveFileToLocalFolder(fileName, "Processed", ConfigurationHelper.ProductOutputPath);
                //}

                byte[] filebyte = System.Text.Encoding.UTF8.GetBytes(csvData);
                TransactionLogging obj = new TransactionLogging();
           //     obj.LogTransaction(SyncJobs.ProductSync, "Product Sync CSV generated Successfully", DateTime.Now, filebyte);
            }
        }

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
        //        obj.LogTransaction(SyncJobs.ProductSync, "Product Related Sync CSV generated Successfully", DateTime.Now, filebyte);
        //    }
        //}

        /// <summary>
        /// CreateProductImageCSVFile creates product image csv.
        /// </summary>
        /// <param name="products"></param>
        private void CreateProductImageCSVFile(List<ProductImageCSV> products)
        {
            if (products != null && products.Count > 0)
            {
                string fileName = FileHelper.GetProductImageCSVFileName();
                string csvData = CSVWriter.Write(products, true, fileName, null);

                //bool upladFiles = true;
                ////TODO: Temp test code to disbale products upload
                //try
                //{
                //    upladFiles = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings.Get("UploadFileToFTP"));
                //}
                //catch
                //{
                //}
                //if (upladFiles)
                //{
                //    SFTPManager.UploadFile(fileName, ConfigurationHelper.ProductImageFTPPath);
                //    FileHelper.MoveFileToLocalFolder(fileName, "Processed", ConfigurationHelper.ProductOutputPath);
                //}

                byte[] filebyte = System.Text.Encoding.UTF8.GetBytes(csvData);
                TransactionLogging obj = new TransactionLogging();
            //    obj.LogTransaction(SyncJobs.ProductSync, "Product Image Sync CSV generated Successfully", DateTime.Now, filebyte);
            }
        }

        #endregion

        #region Push Attributes

        /// <summary>
        /// PushProductColor identifies new Colors and push them to Magento.
        /// </summary>
        /// <param name="products"></param>
        private void PushProductColor(List<EcomcatalogProductCreateEntity> products)
        {
            List<string> colorsList = products.Where(p => !string.IsNullOrEmpty(p.Color) && p.Color.ToLower() != "null").Select(p => p.Color).Distinct().ToList();

            this.PushProductAttribute(colorsList, ConfigurationHelper.ColorAttributeName);
        }

        /// <summary>
        /// PushProductSize identifies new Sizes and push them to Magento.
        /// </summary>
        /// <param name="products"></param>
        private void PushProductSize(List<EcomcatalogProductCreateEntity> products)
        {
            List<string> sizeList = products.Where(p => !string.IsNullOrEmpty(p.Size) && p.Size.ToLower() != "null").Select(p => p.Size).Distinct().ToList();

            this.PushProductAttribute(sizeList, ConfigurationHelper.SizeAttributeName);
        }

        /// <summary>
        /// PushProductWidth identifies new Width and push them to Magento.
        /// </summary>
        /// <param name="products"></param>
        private void PushProductWidth(List<EcomcatalogProductCreateEntity> products)
        {
            List<string> widthList = products.Where(p => !string.IsNullOrEmpty(p.Style) && p.Style.ToLower() != "null").Select(p => p.Style).Distinct().ToList();

            this.PushProductAttribute(widthList, ConfigurationHelper.WidthAttributeName);
        }

        /// <summary>
        /// PushProductAttribute pushes newly identifed attribute values to Magento.
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="attributeName"></param>
        private void PushProductAttribute(List<string> attributes, string attributeName)
        {
            catalogProductAttributeEntity attributeEntity = base.Service.catalogProductAttributeInfo(base.SessionId, attributeName);
            IEnumerable<string> newAttributes;

            if (attributeEntity != null && attributeEntity.options != null && attributeEntity.options.Count() > 0)
            {
                List<string> availableAttributes = attributeEntity.options.Select(a => a.label).Distinct().ToList();
                newAttributes = attributes.Except(availableAttributes);
            }
            else
            {
                newAttributes = attributes;
            }

            // Create new Attributes
            if (newAttributes != null)
            {
                foreach (var attribute in newAttributes)
                {
                    this.CreateAttributeOption(attributeName, attribute);
                }
            }
        }

        /// <summary>
        /// CreateAttributeOption calls API to create attribute in Magento.
        /// </summary>
        /// <param name="attributeName"></param>
        /// <param name="optionValue"></param>
        private void CreateAttributeOption(string attributeName, string optionValue)
        {
            catalogProductAttributeOptionEntityToAdd option = new catalogProductAttributeOptionEntityToAdd();
            catalogProductAttributeOptionLabelEntity[] labels = new catalogProductAttributeOptionLabelEntity[1];

            labels[0] = new catalogProductAttributeOptionLabelEntity();
            labels[0].store_id = new string[] { ConfigurationHelper.MageStoreID };
            labels[0].value = optionValue;

            option.label = labels;
            option.is_default = 0;

            base.Service.catalogProductAttributeAddOption(base.SessionId, attributeName, option);
        }

        #endregion

        #region "Commented code"
        ///// <summary>
        ///// GetDataRow process provide data item and returns CSV data item.
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="p"></param>
        ///// <returns></returns>
        //private CsvRow GetDataRow<T>(T p)//(ProductCSV p) 
        //{
        //    CsvRow dataRow = new CsvRow();
        //    string valueString = string.Empty;
        //    PropertyInfo[] props = p.GetType().GetProperties().Where(pp => pp.Name != "_category_ids").ToArray();
        //    foreach (PropertyInfo prp in props) // Get Attribute Values
        //    {
        //        valueString = string.Empty;
        //        object value = prp.GetValue(p, new object[] { });
        //        if (value != null)
        //        {
        //            if (value.GetType().Name.Equals("String"))
        //            {
        //                //object value = prp.GetValue(p, new object[] { });
        //                valueString = value.ToString();
        //            }
        //            else if (value.GetType().Name.Equals("String[]"))
        //            {
        //                String[] valueArray = (String[])prp.GetValue(p, new object[] { });
        //                valueString = string.Join(",", valueArray);
        //            }
        //            else if (value.GetType().Name.Equals("Int") || GetType().Name.Equals("Decimal"))
        //            {
        //                valueString = value.ToString();
        //            }
        //            else if (prp.Name.Equals("CustomAttributes"))
        //            {
        //                System.Collections.Generic.List<KeyValuePair<string, string>> attributes =
        //                    (System.Collections.Generic.List<KeyValuePair<string, string>>)value;

        //                foreach (var attr in attributes)
        //                {
        //                    dataRow.Add(attr.Value);
        //                }
        //            }
        //            else
        //            {
        //                //valueString = ",";
        //            }
        //        }
        //        else
        //        {
        //            //valueString = ",";
        //        }

        //        dataRow.Add(valueString);
        //    }
        //    return dataRow;
        //}

        ///// <summary>
        ///// Gets all variants of the product based on Size, Color, Style and Configuration
        ///// </summary>
        ///// <param name="products"></param>
        ///// <param name="SKU"></param>
        ///// <returns></returns>
        //private List<ProductCSV> GetProductVariants(List<EcomcatalogProductCreateEntity> products, string SKU)
        //{
        //    List<ProductCSV> csvVariants = null;
        //    var vProducts = products.Where(p => p.MasterProductSKU == SKU);

        //    if (vProducts != null && vProducts.Count() > 0)
        //    {
        //        csvVariants = new List<ProductCSV>();
        //        foreach (var product in vProducts)
        //        {
        //            if (!string.IsNullOrWhiteSpace(product.MasterProductSKU))
        //            {
        //                if (!string.IsNullOrWhiteSpace(product.Size))
        //                {
        //                    csvVariants.Add(this.GetVariant(product.SKU, "size", product.Size, string.Empty));
        //                }
        //                if (!string.IsNullOrWhiteSpace(product.Style))
        //                {
        //                    csvVariants.Add(this.GetVariant(product.SKU, "width", product.Style, string.Empty));
        //                }
        //                if (!string.IsNullOrWhiteSpace(product.Color))
        //                {
        //                    csvVariants.Add(this.GetVariant(product.SKU, "color", product.Color, string.Empty));
        //                }
        //                if (!string.IsNullOrWhiteSpace(product.Configuration))
        //                {
        //                    //csvVariants.Add(GetVariant(product.MasterProductSKU, "configuration", product.Configuration, product.price));
        //                }
        //            }
        //        }
        //    }
        //    return csvVariants;
        //}

        ///// <summary>
        ///// Returns a variants object
        ///// </summary>
        ///// <param name="sku"></param>
        ///// <param name="code"></param>
        ///// <param name="value"></param>
        ///// <param name="price"></param>
        ///// <returns></returns>
        //private ProductCSV GetVariant(string sku, string code, string value, string price)
        //{

        //    ProductCSV v = new ProductCSV();
        //    //v._super_products_sku = sku;
        //    //v._super_attribute_code = code;
        //    //v._super_attribute_option = value;
        //    //v._super_attribute_price_corr = price;
        //    return v;
        //}

        //public void GetClassAttributes(Object p)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    PropertyInfo[] props = p.GetType().GetProperties();


        //    for (int i = 0; i <= props.Length - 1; i++)
        //    {
        //        sb.Append(props[i].Name);

        //        if (i < props.Length - 1)
        //        {
        //            sb.Append(",");
        //        }
        //    }
        //    sb.AppendLine();



        //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\List1Averages.csv", false))
        //    {

        //        file.Write(sb);
        //    }

        //}

        //public void GetClassAttributeValues(List<catalogProductCreateEntity> products)
        //{

        //    StringBuilder sb = new StringBuilder();

        //    foreach (var p in products)
        //    {
        //        PropertyInfo[] props = p.GetType().GetProperties();

        //        foreach (PropertyInfo prp in props) // Get Attribute Values
        //        {
        //            string valueString = string.Empty;
        //            object value = prp.GetValue(p, new object[] { });

        //            if (value != null)
        //            {
        //                if (value.GetType().Name.Equals("String"))
        //                {
        //                    //object value = prp.GetValue(p, new object[] { });
        //                    valueString = value.ToString().Contains(",") ? "\"" + value.ToString() + "\"," : value.ToString();
        //                }
        //                else if (value.GetType().Name.Equals("String[]"))
        //                {
        //                    String[] valueArray = (String[])prp.GetValue(p, new object[] { });

        //                    foreach (var s in valueArray)
        //                    {
        //                        valueString = valueString + s.ToString() + ",";
        //                    }

        //                    valueString = "\"" + valueString + "\",";


        //                }
        //                else if (value.GetType().Name.Equals("Int") || GetType().Name.Equals("Decimal"))
        //                {
        //                    valueString = value.ToString() + ",";
        //                }
        //                else
        //                {
        //                    valueString = ",";
        //                }

        //            }
        //            else
        //            {
        //                valueString = ",";
        //            }



        //            sb.Append(valueString);
        //        }

        //        sb.AppendLine();
        //    }




        //    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"E:\List1Averages.csv", true))
        //    {
        //        file.Write(sb);
        //    }

        //}

        #endregion

        #endregion

        public void PushProducts(ERPDataModels.ErpCatalog catalog)
        {
            throw new NotImplementedException();
        }
    }
}
