namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpAttributeBase
	{
		public ErpAttributeBase()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public string KeyName	{ get; set; }//;
		public ErpAttributeDataType DataType	{ get; set; }//;
		public long AttributeValueRecordId	{ get; set; }//;
		public System.Collections.ObjectModel.ReadOnlyCollection<ErpTextValueTranslation> NameTranslations	{ get; set; }//;
		public ErpAttributeGroup Group	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
