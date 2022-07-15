using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class CustInvoiceJour
    {
        public string RecId { get; set; }
        public string Email { get; set; }
        public string InvoiceId { get; set; }
        public string SalesId { get; set; }
        public string IsMigratedOrder { get; set; }
        public string SalesType { get; set; }
        public string InvoiceDate { get; set; }
        public string CurrencyCode { get; set; }
        public string InvoiceAmount { get; set; }
        public string MigratedInvoiceAmount { get; set; }
        public string SalesBalance { get; set; }
        public string MCRDueAmount { get; set; }
        public string BalanceAmount { get; set; }
        public string SumLineDisc { get; set; }
        public string SumTax { get; set; }
        public string InvoiceAccount { get; set; }
        public string InvoicingName { get; set; }
        public string RetailChannel { get; set; }
        public string RetailChannelId { get; set; }
        public string ThreeLetterISORegionName { get; set; }
        public string Language { get; set; }
        public string LocalTaxId { get; set; }
        public string TMVBoletoURL { get; set; }
        public string TMVBoletoPaymStatus { get; set; }
    }
    public class CustInvoiceTrans
    {
        public string CurrencyCode { get; set; }
        public string DiscAmount { get; set; }
        public string InventDimId { get; set; }
        public string ItemId { get; set; }
        public string RetailVariantId { get; set; }
        public string SalesPrice { get; set; }
        public string SalesUnit { get; set; }
        public string LineAmount { get; set; }
        public string LineDisc { get; set; }
        public string LineAmountTax { get; set; }
        public string LineNum { get; set; }
        public string Name { get; set; }
        public string Qty { get; set; }
        public string TaxWriteCode { get; set; }
        public string TMVProductType { get; set; }
        public string RecId { get; set; }
        public string SalesLineRecId { get; set; }
        public string TMVParent { get; set; }
    }
    public class CustomerInvoiceDetail
    {
        public CustInvoiceJour CustInvoiceJour { get; set; }
        public List<CustInvoiceTrans> CustInvoiceTrans { get; set; }
    }

    public class ErpCustomerInvoiceJour
    {
        public long? RecId { get; set; }
        public string Email { get; set; }
        public string InvoiceId { get; set; }
        public string SalesId { get; set; }
        public string IsMigratedOrder { get; set; }
        public string SalesType { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string CurrencyCode { get; set; }
        public double? InvoiceAmount { get; set; }
        public double? MigratedInvoiceAmount { get; set; }
        public double? SalesBalance { get; set; }
        public double? MCRDueAmount { get; set; }
        public double? BalanceAmount { get; set; }
        public double? SumLineDisc { get; set; }
        public double? SumTax { get; set; }
        public string InvoiceAccount { get; set; }
        public string InvoicingName { get; set; }
        public string RetailChannel { get; set; }
        public string RetailChannelId { get; set; }
        public string ThreeLetterISORegionName { get; set; }
        public string SiteCode { get; set; }
        public string Language { get; set; }
        public string LocalTaxId { get; set; }
        public string TMVBoletoURL { get; set; }
        public string TMVBoletoPaymStatus { get; set; }
    }
    public class ErpCustomerInvoiceTrans
    {
        public string CurrencyCode { get; set; }
        public double? DiscAmount { get; set; }
        public string InventDimId { get; set; }
        public string ItemId { get; set; }
        public double? SalesPrice { get; set; }
        public string SalesUnit { get; set; }
        public double? LineAmount { get; set; }
        public double? LineDisc { get; set; }
        public double? LineAmountTax { get; set; }
        public double? LineNum { get; set; }
        public string Name { get; set; }
        public double? Qty { get; set; }
        public string TaxWriteCode { get; set; }
        public string TMVProductType { get; set; }
        public long? RecId { get; set; }
        public long? SalesLineRecId { get; set; }
        public long TMVParent { get; set; }
    }
    public class ErpCustomerInvoiceDetail
    {
        public ErpCustomerInvoiceDetail()
        {
            this.CustomerInvoiceJour = new ErpCustomerInvoiceJour();
            this.CustomerInvoiceTrans = new List<ErpCustomerInvoiceTrans>();
        }
        public ErpCustomerInvoiceJour CustomerInvoiceJour { get; set; }
        public List<ErpCustomerInvoiceTrans> CustomerInvoiceTrans { get; set; }
    }
}
