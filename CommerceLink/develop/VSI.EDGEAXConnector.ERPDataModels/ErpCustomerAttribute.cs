using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpCustomerAttribute
    {
        public ErpCustomerAttribute()
        {

        }

        public ErpCommercePropertyValue AttributeValue { get; set; }
        public string Units { get; set; }
        public long? RecordId { get; set; }
        public string Name { get; set; }
        public string KeyName { get; set; }
        public int? DataTypeValue { get; set; }
        public System.Collections.Generic.ICollection<ErpTextValueTranslation> NameTranslations { get; set; }
        public System.Collections.Generic.ICollection<ErpCommerceProperty> ExtensionProperties { get; set; }
    }
}
