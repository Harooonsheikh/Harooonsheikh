

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Demandware.PriceBook;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
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
        public PriceController()
            : base(false)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushProducts push products to Magento
        /// </summary>
        /// <param name="products"></param>
        public void PushAllProductPrice(ErpPrice prices)
        {
            try
            {
                pricebooks priceBookData = this.ProcessProductPriceData(prices);
                this.CreateProductPriceFile(priceBookData);
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(StoreService.StoreLkey);
                obj.LogTransaction(SyncJobs.PriceSync, "Price CSV generation Failed", DateTime.UtcNow, null);
                CustomLogger.LogException(exp);
                throw;
            }
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// This function creates product Price CSV files.
        /// </summary>
        /// <param name="products"></param>
        private void CreateProductPriceFile(pricebooks priceBookData)
        {
            if (priceBookData != null)
            {
                string fileName = FileHelper.GetProductPriceCSVFileName();
                var serializer = new XmlSerializer(typeof(pricebooks));
                using (var stream = new StreamWriter(fileName))
                {
                    serializer.Serialize(stream, priceBookData);
                }

                TransactionLogging obj = new TransactionLogging(StoreService.StoreLkey);
                obj.LogTransaction(SyncJobs.PriceSync, "Price Sync CSV generated Successfully", DateTime.UtcNow, null);
            }
        }

        /// <summary>
        /// Format prices for Export.
        /// </summary>
        /// <param name="prices"></param>
        private pricebooks ProcessProductPriceData(ErpPrice prices)
        {
            pricebooks priceBookData = this.IntializePriceData(prices);
            List<complexTypePriceTable> priceTables = new List<complexTypePriceTable>();

            complexTypePriceTable priceTableItem;

            foreach (ErpProductPrice prod in prices.Prices)
            {
                priceTableItem = new complexTypePriceTable();
                priceTableItem.productid = prod.SKU;
                complexTypeAmountEntry[] amounts = new complexTypeAmountEntry[1];
                amounts[0] = new complexTypeAmountEntry();
                amounts[0].quantity = 1;
                amounts[0].quantitySpecified = true;
                amounts[0].Value = Convert.ToDecimal(prod.AdjustedPrice);
                priceTableItem.Items = amounts;

                priceTables.Add(priceTableItem);
            }

            priceBookData.pricebook[0].pricetables = priceTables.ToArray();

            return priceBookData;
        }

        private pricebooks IntializePriceData(ErpPrice prices)
        {
            pricebooks priceBookData = new pricebooks();

            priceBookData.pricebook = new complexTypePriceBook[1];
            priceBookData.pricebook[0] = new complexTypePriceBook();
            priceBookData.pricebook[0].header = this.IntializePriceHeader();

            return priceBookData;
        }

        private complexTypeHeader IntializePriceHeader()
        {
            complexTypeHeader header = new complexTypeHeader();

            header.pricebookid = configurationHelper.GetSetting(PRICE.Pricebook_Id);
            header.currency = "USD";
            header.displayname = new complexTypeLocalizedString[1];
            header.displayname[0] = new complexTypeLocalizedString();
            header.displayname[0].Value = "Fabrikam USD List Price";
            header.onlineflag = true;
            header.onlineflagSpecified = true;

            return header;
        }

        #endregion

    }
}
