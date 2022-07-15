using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
//HK: D365 Update 10.0 Application change start
    public class ErpOrderShipments
    {

        public string SalesId { get; set; }
        public ObservableCollection<ErpShipment> Shipments { get; set; }
        public ObservableCollection<ErpSalesLine> SalesLines { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }
        public string Status { get; set; }
        public string CurrencyCode { get; set; }
        public string CustomerId { get; set; }
        public string DeliveryMode { get; set; }
        public decimal? GrossAmount { get; set; }
        public string InventoryLocationId { get; set; }
        public string ReceiptId { get; set; }
        public DateTimeOffset? RequestedDeliveryDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public decimal? TaxAmount { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
//HK: D365 Update 10.0 Application change end
}
