namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpPriceLine
	{
		public ErpPriceLine()
		{
		}
		public long RecordId	{ get; set; }//;
		public decimal Value	{ get; set; }//;
		public ErpPriceMethod PriceMethod	{ get; set; }//;
		public string OriginId	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;

        //NS: D365 Update 12 Platform change start
        public bool PriceChangedByExtensions { get; set; }
        //NS: D365 Update 12 Platform change end
    }
}
