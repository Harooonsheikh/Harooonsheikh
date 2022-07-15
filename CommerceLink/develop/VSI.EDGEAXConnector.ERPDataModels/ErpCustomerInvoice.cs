using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpCustomerInvoice
    {
        public long RecId { get; set; }
        public string Email { get; set; }
        public string InvoiceId { get; set; }
        public string Voucher { get; set; }
        public string SalesId { get; set; }
        public string TransactionType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CurrencyCode { get; set; }
        public decimal InvoiceAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public string InvoiceAccount { get; set; }
        public string Address { get; set; }
        public string FileName { get; set; }
        public string FileURL { get; set; }
    }
}
