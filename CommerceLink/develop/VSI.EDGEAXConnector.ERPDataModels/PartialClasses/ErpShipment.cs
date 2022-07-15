using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public partial class ErpShipment
    {
        public List<ErpShipmentItem> Containers { get; set; }
        
        // Custom For MF Start
        public string ShippingStatus { get; set; }
        public bool IsGift { get; set; }
        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }
        // Custom For MF End
    }
    public partial class ErpShipmentItem
    {
        public string SKU { get; set; }
        public string ItemId { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int Qty { get; set; }
        public string ShipmentId { get; set; }
        public string TrackingNo { get; set; }
    }
}
