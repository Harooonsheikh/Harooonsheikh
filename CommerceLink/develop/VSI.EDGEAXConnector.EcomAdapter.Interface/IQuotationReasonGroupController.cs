using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQuotationReasonGroupController : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        void PushQuotationReasonGoups(List<ERPQuotationReasonGroup> offerTypeGroups);

    }
}
