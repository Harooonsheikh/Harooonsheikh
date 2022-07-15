using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Demandware;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
{

    /// <summary>
    /// SalesOrderStatusController class performs Sales Order Status related activities.
    /// </summary>
    public class SalesOrderStatusController : BaseController, ISalesOrderStatusController
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SalesOrderStatusController()
            : base(true)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// UpdateOrderStatus updated sale order status.
        /// </summary>
        /// <param name="ecomOrders"></param>
        /// <returns></returns>
        public List<ErpSalesOrderStatus> UpdateOrderStatus(List<ErpSalesOrderStatus> ecomOrders)
        {
            if (ecomOrders != null && ecomOrders.Count > 0)
            {
                orders orderData = this.ProcessOrderStatusData(ecomOrders);

                this.CreateOrderStatusFile(orderData);
            }
            //var items = new List<orderItemIdQty>();
            //if (orderStatuses.Any())
            //{
            //    orderStatuses.ToList().ForEach(o =>
            //    {
            //        try
            //        {
            //            CustomLogger.LogTraceInfo(string.Format("Calling api for {0}, Status {1}  ", o.orderId, o.status));

            //            switch (o.status)
            //            {
            //                case "Delivered":
            //                case "Invoiced":
            //                    if (o.shipments != null && o.shipments.Any())
            //                    {
            //                        o.shipments.ForEach(sh =>
            //                        {
            //                            bool isAlreadyCompleted = MapItemsOnSku(o.orderId, sh.shipmentItems);
            //                            sh.shipmentItems.ForEach(si =>
            //                            {
            //                                if (items.Any(i => i.order_item_id == si.id))
            //                                {
            //                                    items.FirstOrDefault(it => it.order_item_id == si.id).qty =
            //                                        items.FirstOrDefault(it => it.order_item_id == si.id).qty + si.qty;
            //                                }
            //                                else
            //                                {
            //                                    items.Add(new orderItemIdQty { order_item_id = si.id, qty = si.qty });
            //                                }
            //                            });

            //                            if (!isAlreadyCompleted)
            //                            {
            //                                string shipmentNo = base.Service.salesOrderShipmentCreate(SessionId, o.orderId, items.ToArray(), "", 0, 0);
            //                                List<string> trackingNos = sh.shipmentItems.Select(si => si.tracking_numbers).Distinct().ToList();
            //                                trackingNos.ForEach(tr =>
            //                                {
            //                                    base.Service.salesOrderShipmentAddTrack(SessionId, shipmentNo, "ups", "Tracking No", tr);
            //                                });
            //                            }

            //                            // string shipmentNo = base.Service.salesOrderShipmentCreate(SessionId, o.orderId, new orderItemIdQty[] { new orderItemIdQty { order_item_id = 2, qty = 2 } }, "", 0, 0);
            //                            // base.Service.salesOrderShipmentAddTrack(SessionId, shipmentNo, "ups", "Tracking No", sh.TrackingNumbers);
            //                        });
            //                    }
            //                    else
            //                    {
            //                        base.Service.salesOrderShipmentCreate(SessionId, o.orderId, null, "", 0, 0);
            //                    }
            //                    var ecomOrder = base.Service.salesOrderInfo(base.SessionId, o.orderId);
            //                    IntegrationManager.CreateIntegrationKey(Entities.SalesOrderStatus, o.status, o.orderId, ecomOrder.status);
            //                    break;

            //                case "Canceled":
            //                    base.Service.salesOrderCancel(SessionId, o.orderId);
            //                    var canceledOrder = base.Service.salesOrderInfo(base.SessionId, o.orderId);
            //                    IntegrationManager.CreateIntegrationKey(Entities.SalesOrderStatus, o.status, o.orderId, canceledOrder.status);
            //                    break;
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            // absorb exception and continue for other
            //            CustomLogger.LogException(new Exception("error in parsing data for " + o.orderId + " :" + ex.Message));
            //        }
            //    });
            //}

            return ecomOrders;
        }

        /// <summary>
        /// This function maps Items on SKU.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="shipItems"></param>
        /// <returns></returns>
        public bool MapItemsOnSku(string orderId, List<ErpShipmentItem> shipItems)
        {
            //var salesOrder = base.Service.salesOrderInfo(base.SessionId, orderId);

            //if (salesOrder.status == "complete")
            //{
            //    return true;
            //}
            //if (salesOrder != null)
            //{
            //    shipItems.ForEach(si =>
            //    {
            //        var lineItem = salesOrder.items.Where(i => i.sku.Equals(si.sku)).FirstOrDefault();
            //        si.id = Convert.ToInt32(lineItem.item_id);
            //    });
            //}
            return false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// This function creates product Sales Order Status CSV files.
        /// </summary>
        /// <param name="products"></param>
        private void CreateOrderStatusFile(orders orderData)
        {
            if (orderData != null)
            {
                string fileName = FileHelper.GetSalesOrderStatusFileName();
                var serializer = new XmlSerializer(typeof(orders));
                using (var stream = new StreamWriter(fileName))
                {
                    serializer.Serialize(stream, orderData);
                }

                TransactionLogging obj = new TransactionLogging(StoreService.StoreLkey);
                obj.LogTransaction(SyncJobs.SalesOrderStatusSync, "Sales Order Status Sync CSV generated Successfully", DateTime.UtcNow, null);
            }
        }

        /// <summary>
        /// FormatProductCSVDataExt format and arrange Products as per requirements for CSV Export.
        /// </summary>
        /// <param name="ecomOrders"></param>
        /// <param name="csvProducts"></param>
        /// <param name="csvRelatedProducts"></param>
        /// <param name="csvProductImages"></param>
        private orders ProcessOrderStatusData(List<ErpSalesOrderStatus> ecomOrders)
        {
            orders orderData = this.IntializeOrderStatusData(ecomOrders);
            List<complexTypeOrder> orderStatusItems = new List<complexTypeOrder>();

            complexTypeOrder orderItem;

            foreach (ErpSalesOrderStatus ord in ecomOrders)
            {
                orderItem = new complexTypeOrder();
                orderItem.orderno = ord.SalesId;
                orderItem.status = this.ProcessOrderStatus(ord);
                orderItem.productlineitems = this.ProcessOrderStatusLineItems(ord.Shipments);

                orderStatusItems.Add(orderItem);
                IntegrationManager integrationManager = new IntegrationManager(StoreService.StoreLkey);
                integrationManager.CreateIntegrationKey(Entities.SalesOrderStatus, ord.SalesId, ord.ChannelRefId, ord.Status);
            }

            orderData.order = orderStatusItems.ToArray();

            return orderData;
        }

        private complexTypeProductLineItem[] ProcessOrderStatusLineItems(List<ErpShipment> shipments)
        {
            List<complexTypeProductLineItem> lineItems = new List<complexTypeProductLineItem>();
            complexTypeProductLineItem lineItem;

            if (shipments != null)
            {
                foreach (ErpShipment shipment in shipments)
                {
                    foreach (var shipmentLine in shipment.ShipmentLines)
                    {
                        lineItem = new complexTypeProductLineItem();

                        lineItem.productid = shipmentLine.ItemId;

                        lineItem.customattributes = this.ProcessOrderStatusLineItemsCustumAttributes(shipmentLine);

                        lineItems.Add(lineItem);
                    }
                }
            }

            if (lineItems.Count > 0)
            {
                return lineItems.ToArray();
            }
            else
            {
                return null;
            }
        }

        private sharedTypeCustomAttribute[] ProcessOrderStatusLineItemsCustumAttributes(ErpShipmentLine shipmentLine)
        {
            List<sharedTypeCustomAttribute> customattributes = new List<sharedTypeCustomAttribute>();

            sharedTypeCustomAttribute attribute = new sharedTypeCustomAttribute();
            attribute.attributeid = "trackingNo";
            attribute.Text = new string[] { "TODO - tracking number"};
            customattributes.Add(attribute);

            return customattributes.ToArray();
        }

        private complexTypeOrderStatusSet ProcessOrderStatus(ErpSalesOrderStatus orderStatus)
        {
            complexTypeOrderStatusSet status = new complexTypeOrderStatusSet();

            if (orderStatus.Status.Equals("Delivered", StringComparison.CurrentCultureIgnoreCase))
            {
                status.orderstatus = simpleTypeOrderOrderStatus.OPEN;
                status.shippingstatus = simpleTypeOrderShippingStatus.SHIPPED;
            }
            else if (orderStatus.Status.Equals("Invoiced", StringComparison.CurrentCultureIgnoreCase))
            {
                status.orderstatus = simpleTypeOrderOrderStatus.COMPLETED;
                status.shippingstatus = simpleTypeOrderShippingStatus.SHIPPED;
            }
            else if (orderStatus.Status.Equals("Canceled", StringComparison.CurrentCultureIgnoreCase))
            {
                status.orderstatus = simpleTypeOrderOrderStatus.CANCELLED;
                status.shippingstatus = Demandware.simpleTypeOrderShippingStatus.NOT_SHIPPED;
            }

            status.orderstatusSpecified = status.shippingstatusSpecified = true;
            return status;
        }

        private orders IntializeOrderStatusData(List<ErpSalesOrderStatus> ecomOrders)
        {
            orders orderData = new orders();


            return orderData;
        }



        #endregion
    }
}
