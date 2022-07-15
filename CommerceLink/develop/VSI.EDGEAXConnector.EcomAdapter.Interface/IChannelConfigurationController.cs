using System;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChannelConfigurationController : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        void PushConfiguration(ErpConfiguration configuration);

    }
}
