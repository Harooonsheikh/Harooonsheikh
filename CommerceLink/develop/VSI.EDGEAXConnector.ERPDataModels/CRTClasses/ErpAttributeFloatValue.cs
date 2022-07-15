using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.CRTClasses
{
    public class ErpAttributeFloatValue : ErpAttributeValueBase
    {
        //public AttributeFloatValue();

        //[DataMember]
        public string UnitSymbol { get; set; }
        //[DataMember]
        public decimal Value { get; set; }

        //public override string ToString();
    }
}
