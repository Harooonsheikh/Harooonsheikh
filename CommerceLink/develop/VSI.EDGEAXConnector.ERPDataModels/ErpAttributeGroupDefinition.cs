using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
//HK: D365 Update 10.0 Application change start
    public class ErpAttributeGroupDefinition
    {
        public int RecordId { get; set; }
        public string Name { get; set; }
        public System.Collections.Generic.ICollection<ErpAttributeGroupTranslationDetails> TranslationDetails { get; set; }
        public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties { get; set; }
  
    }
//HK: D365 Update 10.0 Application change end

}
