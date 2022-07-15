using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpSalesOrderStatus
    {
        public string ChannelRefId { get; set; }
        public string CustomerAcc { get; set; }
        public string Status { get; set; }
        public string SalesId { get; set; }
        public List<ErpShipment> Shipments { get; set; }
        public bool Notify { get; set; }
    }
}
