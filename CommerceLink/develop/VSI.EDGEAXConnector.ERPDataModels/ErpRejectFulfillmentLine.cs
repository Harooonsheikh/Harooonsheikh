using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 8.1 Application change start
    public class ErpRejectFulfillmentLine
    {
        public ErpRejectFulfillmentLine()
        { }

        public int RejectedQuantity { get; set; }
        public string InfoCodeId { get; set; }
        public string SubInfoCodeId { get; set; }
    }
    //NS: D365 Update 8.1 Application change end
}
