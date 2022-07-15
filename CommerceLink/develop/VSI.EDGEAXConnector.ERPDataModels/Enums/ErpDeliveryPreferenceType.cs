using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels
{
    public enum ErpDeliveryPreferenceType
    {
        None = 0,
        ShipToAddress = 1,
        PickupFromStore = 2,
        ElectronicDelivery = 3,
        DeliverItemsIndividually = 4
    }
}
