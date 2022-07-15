namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductProperty
	{
		public ErpProductProperty()
		{
		}
		public ErpProductPropertyType PropertyType	{ get; set; }//;
		public int PropertyTypeValue	{ get; set; }//;
		public string KeyName	{ get; set; }//;
		public long RecordId	{ get; set; }//;
		public bool IsDimensionProperty	{ get; set; }//;
		public long AttributeValueId	{ get; set; }//;
		public object Value	{ get; set; }//;
		public string ValueString	{ get; set; }//;
		public string UnitText	{ get; set; }//;
		public long GroupId	{ get; set; }//;
		public ErpAttributeGroupType GroupType	{ get; set; }//;
		public int GroupTypeValue	{ get; set; }//;
		public string GroupName	{ get; set; }//;
		public string Language	{ get; set; }//;
		public string Translation	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
    }
}
