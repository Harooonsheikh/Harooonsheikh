using System;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Constants;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.CommerceLink.DemandwareAdapter.Controllers
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
        public PriceController(string storeKey) : base(false, storeKey)

        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushProducts push products to Magento
        /// </summary>
        /// <param name="products"></param>
        public string PushAllProductPrice(ErpPrice price, string fileName)
        {
            CustomLogger.LogDebugInfo(string.Format("Enter in PushAllProductPrice()"), currentStore.StoreId, currentStore.CreatedBy);
            string content = string.Empty;
            try
            {
                if (price.Prices != null && price.Prices.Count > 0)
                {
                    this.ProcessPrice(price);
                    CustomLogger.LogDebugInfo(string.Format("Exit from ProcessPrice()"), currentStore.StoreId, currentStore.CreatedBy);
                }
                this.CreatePriceFile(price, fileName);
                CustomLogger.LogDebugInfo(string.Format("Exit from CreatePriceFile()"), currentStore.StoreId, currentStore.CreatedBy);
                return content;
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(currentStore.StoreKey);
                obj.LogTransaction(SyncJobs.PriceSync, "Price XML generation Failed", DateTime.UtcNow, null);
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy);
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
            CustomLogger.LogDebugInfo(string.Format("Enter in ProcessPrice()"), currentStore.StoreId, currentStore.CreatedBy);
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

        private void CreatePriceFile(ErpPrice price, string fileName)
        {
            String strFileName = String.Empty;
            String strFileDirectory = String.Empty;
            ObjectToCsvConverter objectToCsvConverter = new ObjectToCsvConverter();
            CustomLogger.LogDebugInfo(string.Format("Enter in CreatePriceFile()"), currentStore.StoreId, currentStore.CreatedBy);
            try
            {
                if (price != null && price.Prices != null)
                {
                    if (configurationHelper.GetSetting(ECOM.Price_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_CSV)
                    {
                        #region CSV File Generation
                        strFileName = configurationHelper.GetSetting(PRICE.Filename_Prefix) + DateTime.UtcNow.ToString("yyyyMMddhhmm") + "." + ApplicationConstant.FILE_TYPE_CSV.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(PRICE.local_Output_Path));
                        if (!String.IsNullOrEmpty(strFileDirectory) && !strFileDirectory.EndsWith("\\"))
                        {
                            strFileDirectory = strFileDirectory + "\\";
                        }

                        string strTemplateObjectToCsvMappingXmlFileLocation = configurationHelper.GetSetting(PRICE.CSV_Map_Path);
                        // Product Price
                        objectToCsvConverter.ConvertObjectToCsv(price.Prices.ToArray(), strTemplateObjectToCsvMappingXmlFileLocation,
                           strFileDirectory, strFileName, true, null);
                        #endregion
                    }
                    else if (configurationHelper.GetSetting(ECOM.Price_Output_Type).ToUpper() == ApplicationConstant.FILE_TYPE_XML)
                    {
                        #region XML File Generation
                        strFileName = configurationHelper.GetSetting(PRICE.Filename_Prefix) + DateTime.UtcNow.ToString("yyyyMMddhhmm") + "." + ApplicationConstant.FILE_TYPE_XML.ToLower();
                        strFileDirectory = this.configurationHelper.GetDirectory(configurationHelper.GetSetting(PRICE.local_Output_Path));

                        XmlTemplateHelper xmlHelper = new XmlTemplateHelper(currentStore);
                        xmlHelper.GenerateXmlUsingTemplate(strFileName, strFileDirectory, XmlTemplateHelper.XmlSourceDirection.CREATE, price);
                        #endregion
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        //#region CSVMethods
        ///// <summary>
        ///// This function creates product Price CSV files.
        ///// </summary>
        ///// <param name="products"></param>
        //private void CreateProductPriceCSVFile(List<ProductCSV> products)
        //{
        //    if (products != null && products.Count > 0)
        //    {
        //        string fileName = FileHelper.GetProductPriceCSVFileName();
        //        List<MapTemplate> fieldMaps = XMLHelper.LoadMap(ConfigurationHelper.ProductPriceCSVMap);
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
        //        //    SFTPManager.UploadFile(fileName, ConfigurationHelper.ProductPriceFTPPath);
        //        //    FileHelper.MoveFileToLocalFolder(fileName, "Processed", ConfigurationHelper.ProductPriceOutputPath);
        //        //}

        //        byte[] filebyte = System.Text.Encoding.UTF8.GetBytes(csvData);
        //        TransactionLogging obj = new TransactionLogging();
        //        obj.LogTransaction(SyncJobs.PriceSync, "Price Sync CSV generated Successfully", DateTime.UtcNow, filebyte);
        //    }
        //}


        ///// <summary>
        ///// FormatProductCSVDataExt format and arrange Products as per requirements for CSV Export.
        ///// </summary>
        ///// <param name="products"></param>
        ///// <param name="csvProducts"></param>
        ///// <param name="csvRelatedProducts"></param>
        ///// <param name="csvProductImages"></param>
        //private List<ProductCSV> ProcessProductCSVDataExt(List<EcomcatalogProductCreateEntity> products)
        //{
        //    List<ProductCSV> csvProducts = new List<ProductCSV>();
        //    List<ProductImageCSV> csvProductImages = new List<ProductImageCSV>();

        //    //base.ProcessProductCSVDataExt(products, csvProducts, csvProductImages);
        //    return csvProducts;
        //}
        //#endregion

        #endregion

    }
}
