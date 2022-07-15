using System;
using System.Collections.Generic;
using System.Linq;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.MagentoAPI.MageAPI;

namespace VSI.EDGEAXConnector.MagentoAdapter.Controllers
{

    /// <summary>
    /// SalesOrderStatusController class performs Sales Order Status related activities.
    /// </summary>
    public class SalesOrderStatusController_Dutch : BaseController, ISalesOrderStatusController
    {

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        public SalesOrderStatusController_Dutch()
            : base(true)
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// UpdateOrderStatus updated sale order status.
        /// </summary>
        /// <param name="orderStatuses"></param>
        /// <returns></returns>
        public List<EcomsalesOrderStatus> UpdateOrderStatus(List<EcomsalesOrderStatus> orderStatuses)
        {
            var items = new List<orderItemIdQty>();
            if (orderStatuses.Any())
            {
                orderStatuses.ToList().ForEach(o =>
                {
                    try
                    {
                        CustomLogger.LogTraceInfo(string.Format("Calling api for {0}, Status {1}  ", o.orderId, o.status));

                        switch (o.status)
                        {
                            case "Delivered":
                            case "Invoiced":
                                if (o.shipments != null && o.shipments.Any())
                                {
                                    o.shipments.ForEach(sh =>
                                    {
                                        bool isAlreadyCompleted = MapItemsOnSku(o.orderId, sh.shipmentItems);
                                        sh.shipmentItems.ForEach(si =>
                                        {
                                            if (items.Any(i => i.order_item_id == si.id))
                                            {
                                                items.FirstOrDefault(it => it.order_item_id == si.id).qty =
                                                    items.FirstOrDefault(it => it.order_item_id == si.id).qty + si.qty;
                                            }
                                            else
                                            {
                                                items.Add(new orderItemIdQty { order_item_id = si.id, qty = si.qty });
                                            }
                                        });

                                        if (!isAlreadyCompleted)
                                        {
                                            string shipmentNo = base.Service.salesOrderShipmentCreate(SessionId, o.orderId, items.ToArray(), "", 0, 0);
                                            List<string> trackingNos = sh.shipmentItems.Select(si => si.tracking_numbers).Distinct().ToList();
                                            trackingNos.ForEach(tr =>
                                            {
                                                base.Service.salesOrderShipmentAddTrack(SessionId, shipmentNo, "ups", "Tracking No", tr);
                                            });
                                        }

                                        // string shipmentNo = base.Service.salesOrderShipmentCreate(SessionId, o.orderId, new orderItemIdQty[] { new orderItemIdQty { order_item_id = 2, qty = 2 } }, "", 0, 0);
                                        // base.Service.salesOrderShipmentAddTrack(SessionId, shipmentNo, "ups", "Tracking No", sh.TrackingNumbers);
                                    });
                                }
                                else
                                {
                                    base.Service.salesOrderShipmentCreate(SessionId, o.orderId, null, "", 0, 0);
                                }
                                var ecomOrder = base.Service.salesOrderInfo(base.SessionId, o.orderId);
                      //          IntegrationManager.CreateIntegrationKey(Entities.SalesOrderStatus, o.status, o.orderId, ecomOrder.status);
                                break;

                            case "Canceled":
                                base.Service.salesOrderCancel(SessionId, o.orderId);
                                var canceledOrder = base.Service.salesOrderInfo(base.SessionId, o.orderId);
                          //      IntegrationManager.CreateIntegrationKey(Entities.SalesOrderStatus, o.status, o.orderId, canceledOrder.status);
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Cannot do shipment for order"))
                        {
                            CustomLogger.LogTraceInfo(new Exception("error in parsing data for " + o.orderId + " :" + ex.Message).ToString(), o.orderId.ToString());
                        }
                        else
                        {
                        // absorb exception and continue for other
                        CustomLogger.LogException(new Exception("error in parsing data for " + o.orderId + " :" + ex.Message));
                    }
                    }
                });
            }

            return orderStatuses;
        }

        /// <summary>
        /// This function maps Items on SKU.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="shipItems"></param>
        /// <returns></returns>
        public bool MapItemsOnSku(string orderId, List<EcomShipmentItem> shipItems)
        {
            var salesOrder = base.Service.salesOrderInfo(base.SessionId, orderId);

            if (salesOrder.status == "complete")
            {
                return true;
            }
            if (salesOrder != null)
            {
                shipItems.ForEach(si =>
                {
                    var lineItem = salesOrder.items.Where(i => i.sku.Equals(si.sku)).FirstOrDefault();
                    if (lineItem != null)
                    {
                    si.id = Convert.ToInt32(lineItem.item_id);
                    }   
                });
            }
            return false;
        }

        #endregion
    }
}
