using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    /// <summary>
    /// IOfferTypeGroupsController interface defines features to get Offer Type Groups.
    /// </summary>
    public interface IOfferTypeGroupsController
    {
        /// <summary>
        /// GetOfferTypeGroups gets OfferType groups  with channel.
        /// </summary>
        /// <returns></returns>
        ERPOfferTypeGroupsResponse GetOfferTypeGroups();
    }
}
