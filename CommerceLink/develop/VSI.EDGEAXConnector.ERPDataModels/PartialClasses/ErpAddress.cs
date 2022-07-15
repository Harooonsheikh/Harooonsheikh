using System.Collections.Generic;
namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpAddress
	{
        public string Company { get; set; }

        public string Fax { get; set; }
        public string EcomAddressId {get; set;}

        public string EcomCustomerId { get; set; }
        public int Residential { get; set; }

        public List<KeyValuePair<string, string>> CustomAttributes { get; set; }

        // [MB] - TV - BR 3.0 - 12539 - Start
        public string Street2 { get; set; }
        // [MB] - TV - BR 3.0 - 12539 - End
    }
}
