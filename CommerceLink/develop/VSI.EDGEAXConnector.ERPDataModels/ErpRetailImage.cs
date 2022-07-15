namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpRetailImage
	{
		public ErpRetailImage()
		{
		}
		public int PictureId	{ get; set; }//;
		public string PictureAsBase64	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
