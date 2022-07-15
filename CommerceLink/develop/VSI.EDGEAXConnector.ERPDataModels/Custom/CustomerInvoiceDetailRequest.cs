using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Custom
{
    public class CustomerInvoiceDetailRequest
    {
        public string InvoiceId { get; set; }
        public string CustomerEmail { get; set; }
    }
}
