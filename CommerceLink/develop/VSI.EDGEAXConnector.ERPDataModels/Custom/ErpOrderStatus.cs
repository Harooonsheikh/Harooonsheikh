using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpOrderStatus
    {
        public ErpOrderStatus()
        {
            this.ordersStatus = new List<ErpSalesOrderCustomStatus>();
        }
        public List<ErpSalesOrderCustomStatus> ordersStatus { get; set; }
    }
}
