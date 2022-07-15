using System.Reflection;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using Newtonsoft.Json;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using NewRelic.Api.Agent;
using System;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    /// <summary>
    /// ChannelConfigurationController class provides features to get Channel Level Configurations.
    /// </summary>
    public class OfferTypeGroupController : BaseController, IOfferTypeGroupsController
    {
        #region Public Methods
        public OfferTypeGroupController(string storeKey) : base(storeKey)
        {

        }
        public ERPOfferTypeGroupsResponse GetOfferTypeGroups()
        {
            try
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
                ERPOfferTypeGroupsResponse response = new ERPOfferTypeGroupsResponse(false, "", null);
                List<ERPOfferTypeGroup> erpOfferTypeGroups = new List<ERPOfferTypeGroup>();
                //var rsResponse = ECL_TV_GetOfferTypeGroup();
                //if (rsResponse.Success)
                //{
                //    var rsResult = JsonConvert.DeserializeObject<List<OfferTypeGroup>>(rsResponse.Result);
                //    foreach (var item in rsResult)
                //    {
                //        ERPOfferTypeGroup eRPCustomFields = new ERPOfferTypeGroup();
                //        eRPCustomFields = _mapper.Map<OfferTypeGroup, ERPOfferTypeGroup>(item);
                //        erpOfferTypeGroups.Add(eRPCustomFields);
                //    }
                //    response = new ERPOfferTypeGroupsResponse(true, "Success", erpOfferTypeGroups);
                //}
                //CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

                return response;
            }
            catch (Microsoft.Dynamics.Commerce.RetailProxy.RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }
        }
        #endregion
        #region RetailServer API
        //[Trace]
        //private ProductOfferTypeGroupsResponse ECL_TV_GetOfferTypeGroup()
        //{
        //    IOfferTypeGroupManager offerTypeGroups = RPFactory.GetManager<IOfferTypeGroupManager>();
        //    var rsResponse = Task.Run(async () => await offerTypeGroups.ECL_TV_GetOfferTypeGroup(0)).Result;
        //    return rsResponse;
        //}
        #endregion

    }
}
