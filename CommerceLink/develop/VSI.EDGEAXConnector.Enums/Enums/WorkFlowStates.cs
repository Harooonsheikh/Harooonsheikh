using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Enums
{
    public enum WorkFlowStates : short
    {
        AXDataFetching = 1,
        CommerceLinkProcessing = 2,
        FileGeneration = 3,
        FileGenerated = 4,
        UploadingToSftp = 5,
        GetSaleOrderNumber = 6,
        ProcessSaleOrder = 7,
        PushSaleOrderToAX = 8,
        Failed = 9,
        Completed = 10
    }
    public enum WorkFlowType : short
    {
        Merchandise = 1,
        SaleOrder = 2,
    }
}
