using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public class ErpDeliverySpecification
    {
        public ErpAddress DeliveryAddress { get; set; }
        public string DeliveryModeId { get; set; }
        public ErpDeliveryPreferenceType DeliveryPreferenceType { get; set; }
        public int DeliveryPreferenceTypeValue { get; set; }
        public string ElectronicDeliveryEmailAddress { get; set; }
        public string ElectronicDeliveryEmailContent { get; set; }
        public string PickUpStoreId { get; set; }
    }
}
