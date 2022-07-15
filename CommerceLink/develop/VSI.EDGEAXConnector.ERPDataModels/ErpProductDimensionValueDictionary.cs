using System.Collections.Generic;
namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public class ErpProductDimensionValueDictionaryEntry
	{
		public ErpProductDimensionValueDictionaryEntry()
		{
		}
        //CU 9
        public decimal DisplayOrder { get; set; }
        public string Key { get; set; }
        public IList<long> Value { get; set; }

        //CU 8
        //public System.Collections.Generic.IEqualityComparer<string> Comparer { get; set; }//;
        //public int Count { get; set; }//;
        //public System.Collections.Generic.Dictionary<string, System.Collections.Generic.ISet<long>>.KeyCollection Keys { get; set; }//;
        //public System.Collections.Generic.Dictionary<string, System.Collections.Generic.ISet<long>>.ValueCollection Values { get; set; }//;
        //public System.Collections.Generic.ISet<long> Item { get; set; }//;
	}

}
