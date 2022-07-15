using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.CRTClasses
{
    public class ErpAttributeIntValue : ErpAttributeValueBase
    {
        //public AttributeIntValue();

        //[DataMember]
        public string UnitSymbol { get; set; }
        //[DataMember]
        public int Value { get; set; }

        //public override string ToString();
    }
}
