using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IChannelPublishingController
    {
        int GetOnlineChannelPublishStatus();
        void SetOnlineChannelPublishingStatus(int publishingStatus, string statusMessage);
        List<ErpAttributeProduct> GetChannelProductAttributes();
        Tuple<IEnumerable<ErpCategory>, Dictionary<long, IEnumerable<ErpAttributeCategory>>> GetCategoryAttributes();
    }
}
