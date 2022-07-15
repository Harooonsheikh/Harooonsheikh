using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomCommercePropertyValue
    {
        public EcomCommercePropertyValue()
        {
        }
        public System.Nullable<bool> BooleanValue { get; set; }//;
        public System.Nullable<byte> ByteValue { get; set; }//;
        public System.Nullable<decimal> DecimalValue { get; set; }//;
        public System.Nullable<System.DateTimeOffset> DateTimeOffsetValue { get; set; }//;
        public System.Nullable<int> IntegerValue { get; set; }//;
        public System.Nullable<long> LongValue { get; set; }//;
        public string StringValue { get; set; }//;
    }
}
