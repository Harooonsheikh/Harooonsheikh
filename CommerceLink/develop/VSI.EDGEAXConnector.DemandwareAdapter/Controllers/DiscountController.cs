using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Demandware.PriceBook;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
{

    /// <summary>
    /// DiscountController class performs Discount related activities.
    /// </summary>
    public class DiscountController : ProductBaseController, IDiscountController
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public DiscountController()
            : base(false)
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// PushAllProductDiscounts pushes discounts to DW.
        /// </summary>
        /// <param name="SpecialPrice"></param>
        public void PushAllProductDiscounts(ErpDiscount discounts)
        {
            try
            {
                pricebooks priceBookData = this.ProcessProductPriceData(discounts);
                this.CreateProductDiscountFile(priceBookData);
            }
            catch (Exception exp)
            {
                TransactionLogging obj = new TransactionLogging(StoreService.StoreLkey);
                byte[] filebyte = new byte[] { };
                obj.LogTransaction(19, "Discount CSV generation Failed: " + exp, DateTime.UtcNow, filebyte);
                CustomLogger.LogException(exp);
                throw;
            }
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// it creates Product discount Csv file.
        /// </summary>
        /// <param name="productsDiscount"></param>
        private void CreateProductDiscountFile(pricebooks priceBookData)
        {
            if (priceBookData != null)
            {
                string fileName = FileHelper.GetProductDiscountCSVFileName();

                var serializer = new XmlSerializer(typeof(pricebooks));
                using (var stream = new StreamWriter(fileName))
                {
                    serializer.Serialize(stream, priceBookData);
                }

                TransactionLogging obj = new TransactionLogging(StoreService.StoreLkey);
                obj.LogTransaction(19, "Product Discount Sync CSV generated Successfully", DateTime.UtcNow, null);
            }
        }

        /// <summary>
        /// This function process Discount Csv.
        /// </summary>
        /// <param name="discounts"></param>
        /// <returns></returns>
        private pricebooks ProcessProductPriceData(ErpDiscount discounts)
        {
            pricebooks priceBookData = this.IntializeDiscountData(discounts);
            List<complexTypePriceTable> priceTables = new List<complexTypePriceTable>();

            complexTypePriceTable priceTableItem;

            foreach (ErpProductDiscount discount in discounts.Discounts)
            {
                if (! (discount.OfferPrice.Equals(decimal.Zero)))
                {
                    decimal specialPrice = Convert.ToDecimal(discount.OfferPrice);
                    if (specialPrice > 0)
                    {
                        priceTableItem = new complexTypePriceTable();
                        priceTableItem.productid = discount.SKU;
                        complexTypeAmountEntry[] amounts = new complexTypeAmountEntry[1];
                        amounts[0] = new complexTypeAmountEntry();
                        amounts[0].quantity = 1;
                        amounts[0].quantitySpecified = true;
                        amounts[0].Value = specialPrice;
                        priceTableItem.Items = amounts;
                        priceTableItem.onlinefrom = Convert.ToDateTime(discount.ValidFrom);
                        priceTableItem.onlinefromSpecified = true;
                        priceTableItem.onlineto = Convert.ToDateTime(discount.ValidTo);
                        priceTableItem.onlinetoSpecified = true;

                        priceTables.Add(priceTableItem);
                    }
                }
            }

            priceBookData.pricebook[0].pricetables = priceTables.ToArray();

            return priceBookData;
        }

        private pricebooks IntializeDiscountData(ErpDiscount discounts)
        {
            pricebooks priceBookData = new pricebooks();

            priceBookData.pricebook = new complexTypePriceBook[1];
            priceBookData.pricebook[0] = new complexTypePriceBook();
            priceBookData.pricebook[0].header = this.IntializeDiscountHeader();

            return priceBookData;
        }

        private complexTypeHeader IntializeDiscountHeader()
        {
            complexTypeHeader header = new complexTypeHeader();

            header.pricebookid = configurationHelper.GetSetting(DISCOUNT.Pricebook_Id);
            header.parent = configurationHelper.GetSetting(DISCOUNT.Parent_Pricebook_Id);
            header.currency = configurationHelper.GetSetting(APPLICATION.Default_Currency_Code);
            header.displayname = new complexTypeLocalizedString[1];
            header.displayname[0] = new complexTypeLocalizedString();
            header.displayname[0].Value = "List Discount";
            header.onlineflag = true;
            header.onlineflagSpecified = true;

            return header;
        }

        #endregion
    }
}
