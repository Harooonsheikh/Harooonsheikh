using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    /// <summary>
    /// IChannelConfigurationController interface defines features to get Channel Level Configurations.
    /// </summary>
    public interface IOfferTypeGroupController
    {
        /// <summary>
        /// GetERPOfferTypeGroups gets OfferType groups from channel db.
        /// </summary>
        /// <returns></returns>
        ERPOfferTypeGroupsResponse GetERPOfferTypeGroups();
       
    }
}
