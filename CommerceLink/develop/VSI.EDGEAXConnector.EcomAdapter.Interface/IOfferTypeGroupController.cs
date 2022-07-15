using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IOfferTypeGroupController : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        void PushOfferTypeGoups(List<ERPOfferTypeGroup> offerTypeGroups);

    }
}
