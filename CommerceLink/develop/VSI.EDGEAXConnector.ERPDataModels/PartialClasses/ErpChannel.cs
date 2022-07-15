using System.Collections.Generic;

namespace VSI.EDGEAXConnector.ERPDataModels
{
	
	public partial class ErpChannel
	{
		public string OperatingUnitNumber { get; set; }

        //NS: TMV FDD-013
		public int CustomerSatifactionPeriod { get; set; }

        public List<string> Languages { get; set; }
        public string DefaultLanguage { get; set; }
    }
}
