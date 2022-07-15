namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductPropertyTranslation
	{
		public ErpProductPropertyTranslation()
		{
		}
		public string TranslationLanguage	{ get; set; }//;
		public ErpProductPropertyDictionary IndexedTranslatedProperties	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpProductProperty> TranslatedProperties	{ get; set; }//;
	}
}
