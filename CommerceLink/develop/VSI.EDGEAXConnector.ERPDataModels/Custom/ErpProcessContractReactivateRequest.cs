using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpProcessContractReactivateRequest
    {
        public string SalesOrderId { get; set; }
        public bool IsLineOperation { get; set; }
        public string SalesLineRecIds { get; set; }
    }
}
