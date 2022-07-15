using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
//HK: D365 Update 10.0 Application change start
    public class ErpPaymentError
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public bool? IsLocalized { get; set; }
    }
//HK: D365 Update 10.0 Application change end
}
