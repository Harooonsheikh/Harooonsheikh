using System.Collections.ObjectModel;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpItemAvailability
	{
		public ErpItemAvailability()
		{
		}
		public long RecordId	{ get; set; }//;
		public string ItemId	{ get; set; }//;
		public string VariantInventoryDimensionId	{ get; set; }//;
		public string WarehouseInventoryDimensionId	{ get; set; }//;
		public string InventorySiteId	{ get; set; }//;
		public string InventoryLocationId	{ get; set; }//;
		public long ProductId	{ get; set; }//;
		public decimal AvailableQuantity	{ get; set; }//;
		public string UnitOfMeasure	{ get; set; }//;
		public ErpItemAvailabilityPreferences Preferences	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start
        public ObservableCollection<ErpProductDimension> ProductDimensions { get; set; }
        //NS: D365 Update 12 Platform change end
    }
}
