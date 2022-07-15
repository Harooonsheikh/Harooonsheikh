using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpCustInvoiceJour
    {
        public string RecId { get; set; }
        public string InvoiceId { get; set; }
        public string SalesId { get; set; }
        public string SalesType { get; set; }
        public string InvoiceDate { get; set; }
        public string CurrencyCode { get; set; }
        public string InvoiceAmount { get; set; }
        public string InvoiceAccount { get; set; }
        public string InvoicingName { get; set; }
        public string TMVResellerAccount { get; set; }
        public string TMVDistributorAccount { get; set; }
        public string TMVIndirectCustomer { get; set; }
        public string ContractInvoiceStartDate { get; set; }
        public string ContractInvoiceEndDate { get; set; }
        public string TMVAutoProlongation { get; set; }
        public string TMVPurchOrderFormNum { get; set; }
        public string CustomerRef { get; set; }
        public string LicenseId { get; set; }
    }
}
