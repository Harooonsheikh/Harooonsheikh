using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.CalculateContract
{
    public class CLDeliverySpecification
    {
        public CLAddress DeliveryAddress { get; set; }
        public string DeliveryModeId { get; set; }
        public int DeliveryPreferenceTypeValue { get; set; }
    }
}
