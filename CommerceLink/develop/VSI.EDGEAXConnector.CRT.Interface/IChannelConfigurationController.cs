using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    /// <summary>
    /// IChannelConfigurationController interface defines features to get Channel Level Configurations.
    /// </summary>
    public interface IChannelConfigurationController
    {
        /// <summary>
        /// GetRetailServiceProfile gets Retail Service Profile data associated with channel.
        /// </summary>
        /// <returns></returns>
        ErpRetailServiceProfileResponse GetRetailServiceProfile();

        /// <summary>
        /// GetRetailChannelProfile gets Retail Channel Profile data associated with channel.
        /// </summary>
        /// <returns></returns>
        ErpRetailChannelProfileResponse GetRetailChannelProfile();

        /// <summary>
        /// GetChannelCustomProperties gets custom fields data associated with channel.
        /// </summary>
        /// <returns></returns>
        ErpChannelCustomPropertiesResponse GetChannelCustomProperties();

        /// <summary>
        /// GetChannelInformation get channel information of currently configured store.
        /// </summary>
        /// <returns></returns>
        ErpChannel GetChannelInformation();
    }
}
