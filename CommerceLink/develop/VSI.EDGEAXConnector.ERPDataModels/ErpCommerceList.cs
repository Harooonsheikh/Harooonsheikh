using System.ComponentModel.DataAnnotations;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpCommerceList
	{
		public ErpCommerceList()
		{
		}
        [Required]
        public string Id	{ get; set; }//;
        [Required]
        public string Name	{ get; set; }//;
        [Required]
        public string CustomerId	{ get; set; }//;
		public bool IsFavorite	{ get; set; }//;
		public bool IsRecurring	{ get; set; }//;
		public bool IsPrivate	{ get; set; }//;
		public int CommerceListTypeValue	{ get; set; }//;
		public ErpCommerceListType CommerceListType	{ get; set; }//;
		public System.Collections.Generic.IList<ErpCommerceListLine> CommerceListLines	{ get; set; }//;
        [Required]
        public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
