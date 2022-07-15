using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ErpProcessContractTerminateRequest
    {
        public string SalesOrderId { get; set; }
        public bool IsLineOperation { get; set; }
        public string SalesLineRecIds { get; set; }
        public bool CreateOpportunityInCRM { get; set; }
        public bool RequestTermination { get; set; }
        public string ReasonId { get; set; }
        public string ReasonCode { get; set; }
        public string Comments { get; set; }
        public int Interval { get; set; }
        public bool IsFutureTermination { get; set; }

    }
}
