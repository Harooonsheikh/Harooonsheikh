using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Configuration
{
    public class ConfigurationHelper_Dutch

    {
        public static string ProductOutputPath = ConfigurationManager.AppSettings.Get("ProductOutputPath");
        public static string ProductImageOutputPath = ConfigurationManager.AppSettings.Get("ProductImageOutputPath");
        public static string ProductFTPPath = ConfigurationManager.AppSettings.Get("ProductFTPPath");
        //public static string ProductRelatedFTPPath = ConfigurationManager.AppSettings.Get("ProductRelatedFTPPath");
        public static string ProductImageFTPPath = ConfigurationManager.AppSettings.Get("ProductImageFTPPath");
        public static string ProductPriceFTPPath = ConfigurationManager.AppSettings.Get("ProductPriceFTPPath");
        public static string ProductInventoryFTPPath = ConfigurationManager.AppSettings.Get("ProductInventoryFTPPath");
        public static string ProductDiscountFTPPath = ConfigurationManager.AppSettings.Get("ProductDiscountFTPPath");
        public static string ProductFileTag = ConfigurationManager.AppSettings.Get("ProductFileTag");
        public static string ProductInventoryTag = ConfigurationManager.AppSettings.Get("ProductInventoryTag");
        public static string ProductDiscountTag = ConfigurationManager.AppSettings.Get("ProductDiscountTag");
        public static string ProductImageFileTag = ConfigurationManager.AppSettings.Get("ProductImageFileTag");
        //public static string ProductRelatedFileTag = ConfigurationManager.AppSettings.Get("ProductRelatedFileTag");
        public static string ProductPriceOutputPath = ConfigurationManager.AppSettings.Get("ProductPriceOutputPath");
        public static string ProductPriceFileTag = ConfigurationManager.AppSettings.Get("ProductPriceFileTag");

        public static string InventoryOutputPath = ConfigurationManager.AppSettings.Get("InventoryOutputPath");
        public static string DiscountOutputPath = ConfigurationManager.AppSettings.Get("DiscountOutputPath");
        public static string StoreOutputPath = ConfigurationManager.AppSettings.Get("StoreOutputPath");

        #region Customer Config
        public static string CustomerDefaultCurrencyCode = ConfigurationManager.AppSettings.Get("CustomerDefaultCurrencyCode");
        public static string CustomerOutputPathLocal = ConfigurationManager.AppSettings.Get("CustomerOutputPathLocal");
        public static string CustomerDownloadPathFTP = ConfigurationManager.AppSettings.Get("CustomerDownloadPathFTP");
        public static string ECommerceCustomerCSVPath = ConfigurationManager.AppSettings.Get("ECommerceCustomerCSVPath");
        public static string ConnectorCustomerCSVPath = ConfigurationManager.AppSettings.Get("ConnectorCustomerCSVPath");
        public static string ECommerceDeletedAddressesCSVPath = ConfigurationManager.AppSettings.Get("ECommerceDeletedAddressesCSVPath");
        public static string ConnectorDeletedAddressesCSVPath = ConfigurationManager.AppSettings.Get("ConnectorDeletedAddressesCSVPath");
        public static string AXCustomerGroup = ConfigurationManager.AppSettings.Get("AXCustomerGroup");
        #endregion

        public static string ConnectorSalesrOrderXMLPath = ConfigurationManager.AppSettings.Get("ConnectorSalesrOrderXMLPath");
        public static string ECommerceSalesrOrderXMLPath = ConfigurationManager.AppSettings.Get("ECommerceSalesrOrderXMLPath");
        public static string MapsXmlPath = ConfigurationManager.AppSettings.Get("MapsXmlPath");
        public static string MapsPath = ConfigurationManager.AppSettings.Get("MapsPath");
        public static string CultureName = ConfigurationManager.AppSettings.Get("CultureName");
        public static string RetailMediaDirectory = ConfigurationManager.AppSettings.Get("RetailMediaDirectory");
        //public static string ProductBackImageFileTag = ConfigurationManager.AppSettings.Get("ProductBackImageFileTag");
        //public static string ProductFrontImageFileTag = ConfigurationManager.AppSettings.Get("ProductFrontImageFileTag");

        //public static string ProductSwatchImageFileTag = ConfigurationManager.AppSettings.Get("ProductSwatchImageFileTag");
        //public static string ProductThumbnailImageFileTag = ConfigurationManager.AppSettings.Get("ProductThumbnailImageFileTag");
        //public static string ProductSmallImageFileTag = ConfigurationManager.AppSettings.Get("ProductSmallImageFileTag");
        public static string MediaServerBaseURL = ConfigurationManager.AppSettings.Get("MediaServerBaseURL");
        //
        public static string EmailDestination = ConfigurationManager.AppSettings.Get("EmailDestination");
        public static string EmailSMTP = ConfigurationManager.AppSettings.Get("EmailSMTP");
        public static string EmailSource = ConfigurationManager.AppSettings.Get("EmailSource");
        public static string EmailCC = ConfigurationManager.AppSettings.Get("EmailCC");
        public static string EmailSubject = ConfigurationManager.AppSettings.Get("EmailSubject");
        public static string EmailPort = ConfigurationManager.AppSettings.Get("EmailPort");
        public static string EmailUserName = ConfigurationManager.AppSettings.Get("EmailUserName");
        public static string EmailPassword = ConfigurationManager.AppSettings.Get("EmailPassword");
        public static string EmailEnableSsl = ConfigurationManager.AppSettings.Get("EmailEnableSsl");
        //
        public static string CustomerFunctionId = ConfigurationManager.AppSettings.Get("CustomerFunctionId");
        public static string TestAllProduct = ConfigurationManager.AppSettings.Get("TestAllProduct");
        //TestAllProduct,CustomerFunctionId

        //
        public static string MapsResolverPath = ConfigurationManager.AppSettings.Get("MapsResolverPath");
        public static string ERPAdapterOption = ConfigurationManager.AppSettings.Get("ERPAdapterOption");
        public static string ERPAdapterAssembly = ConfigurationManager.AppSettings.Get("ERPAdapterAssembly");
        public static string EComAdapterAssembly = ConfigurationManager.AppSettings.Get("EComAdapterAssembly");

        //MapsResolverPath,ERPAdapterOption,ERPAdapterAssembly,EComAdapterAssembly


        public static int FileWaitSleepingTimeinMilliSeconds = string.IsNullOrEmpty(ConfigurationManager.AppSettings["FileWaitSleepingTimeinMilliSeconds"]) ? 1000 : Convert.ToInt32(ConfigurationManager.AppSettings["FileWaitSleepingTimeinMilliSeconds"]);

        #region SFTP Configuration
        public static string SFTPHost = ConfigurationManager.AppSettings.Get("SFTPHost");
        public static string SFTPUserName = ConfigurationManager.AppSettings.Get("SFTPUserName");
        public static string SFTPPassword = ConfigurationManager.AppSettings.Get("SFTPPassword");
        //SFTPExtension will contains values like jpg|png|JPG|gif|GIF|doc|DOC|pdf|PDF|csv|CSV|txt|TXT
        public static string SFTPExtension = ConfigurationManager.AppSettings.Get("SFTPExtension");

        #endregion


        #region CSV Parser
        public static string CSVColumnDelimiter = ConfigurationManager.AppSettings.Get("CSVColumnDelimiter");
        public static string CSVFirstRowHasHeader = ConfigurationManager.AppSettings.Get("CSVFirstRowHasHeader");
        public static string CSVMaxBufferSize = ConfigurationManager.AppSettings.Get("CSVMaxBufferSize");
        public static string CSVMaxRows = ConfigurationManager.AppSettings.Get("CSVMaxRows");
        public static string CSVSkipStartingDataRows = ConfigurationManager.AppSettings.Get("CSVSkipStartingDataRows");
        #endregion

        public static string ProductCSVMap = ConfigurationManager.AppSettings.Get("ProductCSVMap");
        public static string ProductPriceCSVMap = ConfigurationManager.AppSettings.Get("ProductPriceCSVMap");
        public static string ProductInventoryCSVMap = ConfigurationManager.AppSettings.Get("ProductInventoryCSVMap");
        public static string ProductDiscountCSVMap = ConfigurationManager.AppSettings.Get("ProductDiscountCSVMap");
        public static string ProductAttributeNotForVariant = ConfigurationManager.AppSettings.Get("ProductAttributeNotForVariant");
        public static string ProductAttributeNotForSimple = ConfigurationManager.AppSettings.Get("ProductAttributeNotForSimple");
        public static string ProductWebColorAttribute = ConfigurationManager.AppSettings.Get("ProductWebColorAttribute");
        public static string ImageFileExtension = ConfigurationManager.AppSettings.Get("ImageFileExtension");
        public static string SimpleProductDefaultColor = ConfigurationManager.AppSettings.Get("SimpleProductDefaultColor");
        public static string AdditionalProductImageFiles = ConfigurationManager.AppSettings.Get("AdditionalProductImageFiles");
        public static int MainProductImageFiles = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("MainProductImageFiles")) ? 0 : int.Parse(ConfigurationManager.AppSettings.Get("MainProductImageFiles"));


        public static string OnlineStoreDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["OnlineStoreDBConnectionString"].ConnectionString;
        public static string ConnectorDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectorDBConnectionString"].ConnectionString;
        public static long ChannelID = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("ChannelID")) ? 0 : long.Parse(ConfigurationManager.AppSettings.Get("ChannelID"));

        public static string OnlineCustomerTaxGroup = ConfigurationManager.AppSettings["OnlineCustomerTaxGroup"];
        public static int UpdateTimeStampDiff = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["UpdateTimeStampDiff"]) ? 0 : int.Parse(ConfigurationManager.AppSettings["UpdateTimeStampDiff"]);
        public static int ConnectorEcomStoreTimeZoneDifferenceInHrs = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("ConnectorEcomStoreTimeZoneDifferenceInHrs")) ? 0 : int.Parse(ConfigurationManager.AppSettings.Get("ConnectorEcomStoreTimeZoneDifferenceInHrs"));
        public static string CustomerUploadPathFTP = ConfigurationManager.AppSettings.Get("CustomerUploadPathFTP");
        public static string CustomerAddressOutputPathLocal = ConfigurationManager.AppSettings.Get("CustomerAddressOutputPathLocal");
        public static string CustomerAddressUploadPathFTP = ConfigurationManager.AppSettings.Get("CustomerAddressUploadPathFTP");
        public static int MageRootCatId = string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings.Get("MageRootCatId")) ? 0 : int.Parse(ConfigurationManager.AppSettings.Get("MageRootCatId"));
        public static string MageAPIUser = ConfigurationManager.AppSettings.Get("MageAPIUser");
        public static string MageAPIKey = ConfigurationManager.AppSettings.Get("MageAPIKey");
        public static string EncryptionKey = ConfigurationManager.AppSettings.Get("EncryptionKey");
        public static string VectorKey = ConfigurationManager.AppSettings.Get("VectorKey");
        public static string ColorAttributeName = ConfigurationManager.AppSettings.Get("ColorAttributeName");
        public static string SizeAttributeName = ConfigurationManager.AppSettings.Get("SizeAttributeName");
        public static string WidthAttributeName = ConfigurationManager.AppSettings.Get("WidthAttributeName");
        public static string MageStoreID = ConfigurationManager.AppSettings.Get("MageStoreID");


        public static string DeletedAddressDownloadFTPPath = ConfigurationManager.AppSettings.Get("DeletedAddressDownloadFTPPath");
        public static string DeletedAddressDownloadLocalPath = ConfigurationManager.AppSettings.Get("DeletedAddressDownloadLocalPath");

        public static string ResetTimeInMinutes = ConfigurationManager.AppSettings.Get("ResetTimeInMinutes");
        public static int DefaultAXAddressType = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultAXAddressType"]) ? 7 : Convert.ToInt32(ConfigurationManager.AppSettings["DefaultAXAddressType"]);
        public static string DefaultAXSalesOrderUnitOfMeasure = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultAXSalesOrderUnitOfMeasure"]) ? "ea" : ConfigurationManager.AppSettings["DefaultAXSalesOrderUnitOfMeasure"];
        public static string DefaultAXTenderLineCurrencyCode = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultAXTenderLineCurrencyCode"]) ? "USD" : ConfigurationManager.AppSettings["DefaultAXTenderLineCurrencyCode"];
        public static string DefaultAXGiftCardDeliveryMode = string.IsNullOrEmpty(ConfigurationManager.AppSettings["DefaultAXGiftCardDeliveryMode"]) ? "Electronic" : ConfigurationManager.AppSettings["DefaultAXGiftCardDeliveryMode"];
        public static string SaleOrderStausUpdateTimeFrameInDays = string.IsNullOrEmpty(ConfigurationManager.AppSettings["SaleOrderStausUpdateTimeFrameInDays"]) ? "1" : ConfigurationManager.AppSettings["SaleOrderStausUpdateTimeFrameInDays"];
        public static string TaxExemptNumber = ConfigurationManager.AppSettings["TaxExemptNumber"];

        public static string TruncateZipCode = ConfigurationManager.AppSettings["TruncateZipCode"];

    }
}
