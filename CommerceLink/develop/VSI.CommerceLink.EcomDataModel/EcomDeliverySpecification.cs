using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.CommerceLink.EcomDataModel
{
    public class EcomDeliverySpecification
    {
        public EcomAddress DeliveryAddress { get; set; }
        public string DeliveryModeId { get; set; }
        public int DeliveryPreferenceTypeValue { get; set; }
    }
}
