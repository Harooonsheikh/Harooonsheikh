using System.Collections.Generic;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class  ErpCategory
	{
        public int IsActive { get; set; }//;
        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }

        public string EcomParentCategoryId { get; set; }//;
        public string EcomCategoryId { get; set; }//;
        public int Position { get; set; }
        public string FullName { get; set; }//;
    }
}
