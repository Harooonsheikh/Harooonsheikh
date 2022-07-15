using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomCart
    {
        public EcomCart()
        {
        }
        public int? CartTypeValue { get; set; }
        public string Id { get; set; }       
        public List<EcomAffiliation> AffiliationLines { get; set; }
    }
}
