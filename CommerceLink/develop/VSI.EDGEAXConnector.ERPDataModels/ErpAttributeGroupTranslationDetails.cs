using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
//HK: D365 Update 10.0 Application change start
    public class ErpAttributeGroupTranslationDetails
    {
        public int AttributeGroupRecordId { get; set; }
        public string LanguageId { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public string HelpText { get; set; }
        public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties { get; set; }

    }
//HK: D365 Update 10.0 Application change end
}
