using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Mapper;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
{

    /// <summary>
    /// SaleOrderController class performs SalesOrder related activities.
    /// </summary>
    public class SaleOrderController : BaseController, ISaleOrderController
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SaleOrderController(string storeKey) : base(false, storeKey)
        {
            //var list = this.Service.salesOrderList(SessionId,null);
            //var salesInfo = this.Service.salesOrderInfo(SessionId, "200000025");
        }

        #endregion

        #region Public Methods

        public ErpSalesOrder GetSalesOrders(string localFile)
        {
            ErpSalesOrder order = new ErpSalesOrder();
            try
            {
                XmlTemplateHelper xmlHelper = new XmlTemplateHelper();
                xmlHelper.GenerateObjectByTemplateFromXMLFile(localFile, XmlTemplateHelper.XmlSourceDirection.READ, order);
            }
            catch (Exception ex)
            {
                CustomLogger customLogger = new CustomLogger();
                customLogger.LogFatal(string.Concat("Exception logged:  : {0}", ex.Message));
                order = null;
            }

            //Custom For MF 
            //Decrypting payment fields
            //NS MF customizations
            //ProcessTenderLineDecryption(order);

            //flating product options as product
            if (order.SalesLines != null)
            {
                order = ProcessProductOptionsAsErpSalesLine(order);
            }

            ProcessProductOptionsAsErpSalesLine(order);

            return order;
        }

        public ErpSalesOrder GetSalesOrderFromXML(XmlDocument xmlDoc)
        {
            ErpSalesOrder order = new ErpSalesOrder();
            try
            {
                XmlTemplateHelper xmlHelper = new XmlTemplateHelper();
                xmlHelper.GenerateObjectByTemplateFromXML(xmlDoc, XmlTemplateHelper.XmlSourceDirection.READ, order);
            }
            catch (Exception ex)
            {
                CustomLogger customLogger = new CustomLogger();
                customLogger.LogFatal(string.Concat("Exception logged:  : {0}", ex.Message));
                order = null;
            }

            //Custom For MF 
            //Decrypting payment fields
            //NS MF customizations
            //ProcessTenderLineDecryption(order);

            //flating product options as product
            if (order.SalesLines != null)
            {
                order = ProcessProductOptionsAsErpSalesLine(order);
            }

            ProcessProductOptionsAsErpSalesLine(order);

            return order;
        }

        #endregion

        #region Private Methods

        private ErpSalesOrder ProcessProductOptionsAsErpSalesLine(ErpSalesOrder order)
        {
            List<ErpSalesLine> lstNewSalesLine = new List<ErpSalesLine>();
            if (order.SalesLines != null)
            {
                foreach (ErpSalesLine line in order.SalesLines)
                {
                    if (line.Options != null)
                    {
                        foreach (ErpSalesLine option in line.Options)
                        {
                            if (!option.ItemId.Equals(configurationHelper.GetSetting(SALESORDER.OptionItem_None_Constant)))
                            {
                                option.ShipmentId = line.ShipmentId;
                                //Maintaining these values from parent product
                                option.RequestedDeliveryDate = line.RequestedDeliveryDate;
                                option.InventoryLocationId = line.InventoryLocationId;
                                option.CustomAttributes = line.CustomAttributes;

                                //holding option parent item variant into comment to get linked item quantity
                                option.Comment += ":" + line.ItemId.Replace(configurationHelper.GetSetting(PRODUCT.SKU_Prefix), "");

                                lstNewSalesLine.Add(option);
                            }
                        }
                        line.Options.Clear();
                    }
                }
                foreach (ErpSalesLine line in lstNewSalesLine)
                {
                    order.SalesLines.Add(line);
                }
            }
            return order;
        }

        private void ProcessProductDiscountAndCharges(ErpSalesOrder order)
        {
            foreach (ErpSalesLine line in order.SalesLines)
            {
                if (line.DiscountLines != null)
                {
                    /*
                    //MF: Bug # 9402: to fix this issue we deceide to handle length of promotion-id & compaign-id in code by substring
                    */
                    foreach (ErpDiscountLine dLine in line.DiscountLines)
                    {
                        dLine.OfferId = (dLine.OfferId.Length > 15 ? dLine.OfferId.ToString().Substring(0, 15) : dLine.OfferId);
                        dLine.DiscountCode = (dLine.DiscountCode.Length > 15 ? dLine.DiscountCode.ToString().Substring(0, 15) : dLine.DiscountCode);
                    }
                }
            }
        }

        #endregion
    }
}