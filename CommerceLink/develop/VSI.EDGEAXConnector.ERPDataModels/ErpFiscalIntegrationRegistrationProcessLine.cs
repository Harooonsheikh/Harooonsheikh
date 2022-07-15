using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
//HK: D365 Update 10.0 Application change start
    public class ErpFiscalIntegrationRegistrationProcessLine
    {
        public string RegistrationProcessId { get; set; }
        public int? SequenceNumber { get; set; }
        public int? Priority { get; set; }
        public int? ConnectorTypeValue { get; set; }
        public string FunctionalityProfileGroupId { get; set; }
        public bool? AllowSkip { get; set; }
        public bool? AllowMarkAsRegistered { get; set; }
        public ErpFiscalIntegrationRegistrationSettings RegistrationSettings { get; set; }
        public ObservableCollection<int> SupportedFiscalEventTypes { get; set; }
        public ObservableCollection<int> SupportedNonFiscalEventTypes { get; set; }
        public ObservableCollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
//HK: D365 Update 10.0 Application change start
}
