using System.ComponentModel.DataAnnotations;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpCommerceListLine
	{
		public ErpCommerceListLine()
		{
		}
        [Required]
        public string CommerceListId	{ get; set; }//;
        [Required]
        public string LineId	{ get; set; }//;
        [Required]
        public string CustomerId	{ get; set; }//;
        [Required]
        public long ProductId	{ get; set; }//;
		public decimal Quantity	{ get; set; }//;
        [Required]
        public string UnitOfMeasure	{ get; set; }//;
		public bool IsFavorite	{ get; set; }//;
		public bool IsRecurring	{ get; set; }//;
		public bool IsPrivate	{ get; set; }//;
        [Required]
        public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
