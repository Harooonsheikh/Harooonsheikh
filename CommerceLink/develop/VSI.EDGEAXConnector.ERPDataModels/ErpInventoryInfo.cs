using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpInventoryInfo
    {
        public string InventoryAvailable { get; set; }
        public string InventoryLocationId { get; set; }
        public string ItemId { get; set; }
        public string StoreName { get; set; }
    }
}
