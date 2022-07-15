using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    //NS: D365 Update 8.1 Application change start
    public class ErpEnvironmentConfiguration
    {
        public ErpEnvironmentConfiguration()
        {

        }

        public string EnvironmentId { get; set; }
        public Guid TenantId { get; set; }
        public string ClientAppInsightsInstrumentationKey { get; set; }
        public string HardwareStationAppInsightsInstrumentationKey { get; set; }
        public string WindowsPhonePosAppInsightsInstrumentationKey { get; set; }
        public string BaseVersion { get; set; }
		//HK: D365 Update 10.0 Application change start
        public string MyProEnvironmentNameperty { get; set; }
        public ErpScaleUnitConfiguration ScaleUnit { get; set; }
		//HK: D365 Update 10.0 Application change end
    }
    //NS: D365 Update 8.1 Application change end
}
