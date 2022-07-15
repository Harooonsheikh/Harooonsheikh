using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 12 Platform new class
    public class ErpTransferOrderJournalLine
    {
        public ErpTransferOrderJournalLine()
        {

        }

        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string InventDimId { get; set; }
        public decimal TotalQuantityShipped { get; set; }
        public decimal QuantityShipped { get; set; }
        public decimal TransferQuantity { get; set; }
        public string UnitId { get; set; }
        public string ConfigId { get; set; }
        public string InventSizeId { get; set; }
        public string InventColorId { get; set; }
        public string InventStyleId { get; set; }
        public string DeliveryMethod { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }

        //NS: D365 Update 8.1 Application change start
        public string Barcode { get; set; }

        //NS: D365 Update 8.1 Application change end
    }
}
