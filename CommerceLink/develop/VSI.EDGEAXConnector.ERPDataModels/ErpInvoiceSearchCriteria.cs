using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 8.1 Application change start
    public class ErpInvoiceSearchCriteria
    {
        public ErpInvoiceSearchCriteria()
        { }
        public string CustomerId { get; set; }
        public string InvoiceIds { get; set; }
        public System.Collections.Generic.ICollection<int> InvoiceTypeValues { get; set; }
    }
    //NS: D365 Update 8.1 Application change end
}