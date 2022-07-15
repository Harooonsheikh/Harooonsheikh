using System.Reflection;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using Newtonsoft.Json;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using System;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using NewRelic.Api.Agent;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    /// <summary>
    /// ChannelConfigurationController class provides features to get Channel Level Configurations.
    /// </summary>
    public class ChannelConfigurationController : BaseController, IChannelConfigurationController
    {
        public ChannelConfigurationController(string storeKey):base(storeKey)
        {
        }

        #region Public Methods
        /// <summary>
        /// GetRetailServiceProfile gets Retail Service Profile data associated with channel.
        /// </summary>
        /// <returns></returns>
        public ErpRetailServiceProfileResponse GetRetailServiceProfile()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpRetailServiceProfileResponse retailServiceResponse = new ErpRetailServiceProfileResponse(false, "", null);

            try
            {
                //ErpRetailServiceProfile erpRetailSerivceProfile = new ErpRetailServiceProfile();

                //RetailServiceProfileResponse response = ECL_GetRetailServiceProfile();

                //if (response.Success)
                //{
                //    RetailServiceProfile retailSerivceProfile = JsonConvert.DeserializeObject<RetailServiceProfile>(response.Result);
                //    erpRetailSerivceProfile = _mapper.Map<RetailServiceProfile, ErpRetailServiceProfile>(retailSerivceProfile);

                //    retailServiceResponse = new ErpRetailServiceProfileResponse(response.Success, response.Message, erpRetailSerivceProfile);
                //}
                //else
                //{
                //    retailServiceResponse = new ErpRetailServiceProfileResponse(response.Success, response.Message, null);
                //}
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return retailServiceResponse;
        }

        
        /// <summary>
        /// GetRetailChannelProfile gets Retail Channel Profile data associated with channel.
        /// </summary>
        /// <returns></returns>
        public ErpRetailChannelProfileResponse GetRetailChannelProfile()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpRetailChannelProfileResponse retailChannelResponse = new ErpRetailChannelProfileResponse(false, "", null);

            try
            {
                //ErpRetailChannelProfile erpRetailChannelProfile = new ErpRetailChannelProfile();
                //RetailChannelProfileResponse response = ECL_GetRetailChannelProfile();

                //if (response.Success)
                //{
                //    RetailChannelProfile retailSerivceProfile = JsonConvert.DeserializeObject<RetailChannelProfile>(response.Result);
                //    erpRetailChannelProfile = _mapper.Map<RetailChannelProfile, ErpRetailChannelProfile>(retailSerivceProfile);
                //    retailChannelResponse = new ErpRetailChannelProfileResponse(response.Success, response.Message, erpRetailChannelProfile);
                //}
                //else
                //{
                //    retailChannelResponse = new ErpRetailChannelProfileResponse(response.Success, response.Message, null);
                //}
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return retailChannelResponse;
        }

       
        /// <summary>
        /// GetChannelInformation get channel information of currently configured store.
        /// </summary>
        /// <returns></returns>
        public ErpChannel GetChannelInformation()
        {
            var erpChannel = new ErpChannel
            {
                RecordId = this.baseChannelId,
                OperatingUnitNumber = base.operatingUnitNumber,
                DefaultLanguage = base.defaultLanguage,
                Languages = base.companyLanguagesList
            };

            var CustomProperties = GetChannelCustomProperties();

            if (CustomProperties.Success)
            {
                ErpChannelCustomProperties channelCustomProperties = CustomProperties.CustomProperties;

                if (channelCustomProperties != null)
                {
                    erpChannel.CustomerSatifactionPeriod = channelCustomProperties.TMVCUSTOMERSATISFACTIONPERIOD;
                }
            }

            return erpChannel;               
        }

        public ErpChannelCustomPropertiesResponse GetChannelCustomProperties()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            ErpChannelCustomPropertiesResponse channelResponse = new ErpChannelCustomPropertiesResponse(false, "", null);

            try
            {
                //ErpChannelCustomProperties erpChannelCustomProperties = new ErpChannelCustomProperties();

                //ChannelCustomPropertiesResponse response = ECL_GetChannelCustomProperties();

                //if (response.Success)
                //{
                //    ChannelCustomProperties customProperties = JsonConvert.DeserializeObject<ChannelCustomProperties>(response.Result);
                //    erpChannelCustomProperties = _mapper.Map<ChannelCustomProperties, ErpChannelCustomProperties>(customProperties);

                //    channelResponse = new ErpChannelCustomPropertiesResponse(response.Success, response.Message, erpChannelCustomProperties);
                //}
                //else
                //{
                //    channelResponse = new ErpChannelCustomPropertiesResponse(response.Success, response.Message, null);
                //}
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
            catch (Exception exp)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return channelResponse;
        } 
        #endregion

        #region RetailServer API Calls
        //[Trace]
        //private RetailServiceProfileResponse ECL_GetRetailServiceProfile()
        //{
        //    var retailServerManager = RPFactory.GetManager<IRetailServiceProfileManager>();
        //    return Task.Run(async () => await retailServerManager.ECL_GetRetailServiceProfile(this.baseChannelId)).Result;
        //}
        //[Trace]
        //private RetailChannelProfileResponse ECL_GetRetailChannelProfile()
        //{
        //    var retailChannelManager = RPFactory.GetManager<IRetailChannelProfileManager>();
        //    return Task.Run(async () => await retailChannelManager.ECL_GetRetailChannelProfile(this.baseChannelId)).Result;
        //}
        //[Trace]
        //private ChannelCustomPropertiesResponse ECL_GetChannelCustomProperties()
        //{
        //    var channelCustomManager = RPFactory.GetManager<IChannelCustomPropertiesManager>();
        //    return Task.Run(async () => await channelCustomManager.ECL_GetChannelCustomProperties(this.baseChannelId)).Result;
        //}
        #endregion
    }
}
