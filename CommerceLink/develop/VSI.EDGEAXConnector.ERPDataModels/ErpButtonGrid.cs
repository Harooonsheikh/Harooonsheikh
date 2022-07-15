namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpButtonGrid
	{
		public ErpButtonGrid()
		{
		}
		public string Id	{ get; set; }//;
		public string Name	{ get; set; }//;
		public int DefaultColor	{ get; set; }//;
		public int SpaceBetweenButtonsInPixels	{ get; set; }//;
		public System.Collections.Generic.IEnumerable<ErpButtonGridButton> Buttons	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
