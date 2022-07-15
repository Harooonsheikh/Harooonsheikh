namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomapiEntity
	{
		public EcomapiEntity()
		{
		}
		public string title	{ get; set; }//;
		public string name	{ get; set; }//;
		public string[] aliases	{ get; set; }//;
		public EcomapiMethodEntity[] methods	{ get; set; }//;
	}
}
