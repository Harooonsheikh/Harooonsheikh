using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.CRTClasses
{
    public class ErpAttributeTextValue : ErpAttributeValueBase
    {
        //public AttributeTextValue();

        //[Column("TEXTVALUE")]
        //[DataMember]
        public string TextValue { get; set; }
        //[DataMember]
        public IEnumerable<ErpTextValueTranslation> TextValueTranslations { get; set; }

        //public override string ToString();
    }
}
