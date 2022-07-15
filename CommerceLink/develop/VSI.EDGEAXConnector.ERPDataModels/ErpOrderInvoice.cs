using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 8.1 Application change start
    public class ErpOrderInvoice
    {
        public ErpOrderInvoice()
        {
        }

        public string Id { get; set; }
        public string CustomerAccountNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTimeOffset InvoiceDate { get; set; }
        public Decimal TotalAmount { get; set; }
        public Decimal AmountPaid { get; set; }
        public Decimal AmountBalance { get; set; }
        public int InvoiceTypeValue { get; set; }
        public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }

    //NS: D365 Update 8.1 Application change end
}