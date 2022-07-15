namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductRules
	{
		public ErpProductRules()
		{
		}
		public long ProductId	{ get; set; }//;
		public bool HasLinkedProducts	{ get; set; }//;
		public bool IsBlocked	{ get; set; }//;
		public System.DateTimeOffset DateOfBlocking	{ get; set; }//;
		public System.DateTimeOffset DateToActivate	{ get; set; }//;
		public System.DateTimeOffset DateToBlock	{ get; set; }//;
		public ErpKeyInPrices PriceKeyingRequirement	{ get; set; }//;
		public int PriceKeyingRequirementValue	{ get; set; }//;
		public ErpKeyInQuantities QuantityKeyingRequirement	{ get; set; }//;
		public int QuantityKeyingRequirementValue	{ get; set; }//;
		public bool MustKeyInComment	{ get; set; }//;
		public bool CanQuantityBecomeNegative	{ get; set; }//;
		public bool MustScaleItem	{ get; set; }//;
		public bool CanPriceBeZero	{ get; set; }//;
		public bool IsSerialized	{ get; set; }//;
		public string DefaultUnitOfMeasure	{ get; set; }//;
		public string EntityName	{ get; set; }//;
		public System.Runtime.Serialization.ExtensionDataObject ExtensionData	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties	{ get; set; }//;
		public object Item	{ get; set; }//;
	}
}
