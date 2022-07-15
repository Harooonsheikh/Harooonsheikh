using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomCommerceProperty
    {
        public EcomCommerceProperty()
        {
        }

        public EcomCommerceProperty(string key, object value)
        {
            this.Key = key;
            this.Value = value as EcomCommercePropertyValue;
        }
        public string Key { get; set; }//;
        public EcomCommercePropertyValue Value { get; set; }//;
    }
}
