using System.Collections.Generic;
namespace VSI.EDGEAXConnector.ECommerceDataModels
{
	
	public partial class EcomcatalogCategoryEntityCreate
	{
		public long parentCategoryId	{ get; set; }//;
        public long categoryId { get; set; }//;
        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }
	}
}
