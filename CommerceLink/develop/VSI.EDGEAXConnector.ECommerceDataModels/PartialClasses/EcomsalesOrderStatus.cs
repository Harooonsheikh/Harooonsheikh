using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ECommerceDataModels
{
    public class EcomsalesOrderStatus
    {
        public string orderId { get; set; }
        public string status { get; set; }
        public List<EcomShipment> shipments { get; set; }
    }
    public class EcomShipment
    {
        public string shipmentId { get; set; }
        public List<EcomShipmentItem> shipmentItems { get; set; }
    }
    public class EcomShipmentItem
    {
        public string tracking_numbers { get; set; }
        public string sku { get; set; }
        public string item_id { get; set; }
        public string color { get; set; }
        public string size { get; set; }
        public int qty { get; set; }
        public int id { get; set; }
    }
}
