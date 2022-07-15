using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class ContractInvoicesRequest
    {
        public string CustomerAccount { get; set; }
        public string SalesOrderId { get; set; }
        public string InvoiceId { get; set; }
    }
}
