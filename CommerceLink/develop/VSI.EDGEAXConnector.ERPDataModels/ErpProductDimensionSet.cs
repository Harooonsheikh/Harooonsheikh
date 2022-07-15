namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductDimensionSet
	{
		public ErpProductDimensionSet()
		{
		}
		public string DimensionKey	{ get; set; }//;
		//CU 8
        //public System.Collections.Generic.Dictionary<string, System.Collections.Generic.ISet<long>> IndexedDimensionValues	{ get; set; }//;
		public System.Collections.Generic.ICollection<ErpProductDimensionValueSet> DimensionValues	{ get; set; }//;
        //CU 9
        public System.Collections.Generic.IList<ErpProductDimensionValueDictionaryEntry> IndexedDimensionValues { get;  set; }
	}
}
