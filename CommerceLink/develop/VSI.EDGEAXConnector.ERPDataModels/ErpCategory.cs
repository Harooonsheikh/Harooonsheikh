using System.Collections.Generic;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpCategory
	{
		public ErpCategory()
		{
		}
		public long RecordId	{ get; set; }//;
		public string Name	{ get; set; }//;
		public long ParentCategory	{ get; set; }//;
		public ErpRichMediaLocations Image	{ get; set; }//;
        public System.Collections.Generic.IEnumerable<ErpTextValueTranslation> NameTranslations { get; set; }//;
        public string EntityName { get; set; }//;
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData { get; set; }//;
        public List<ErpCommerceProperty> ExtensionProperties { get; set; }//;
		public object Item	{ get; set; }//;
	}
}
