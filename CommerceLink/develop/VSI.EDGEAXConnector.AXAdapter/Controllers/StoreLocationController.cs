//using Autofac;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class StoreLocationController : BaseController, IStoreController
    {

        // initialize the channel: instantiate CRT, create CRT data managers, load channel settings

        //CommerceRuntime runtime;
        //ChannelState currentChannelState;

        public StoreLocationController(string storeKey) : base(storeKey)
        {
            //// initialize the channel: instantiate CRT, create CRT data managers, load channel settings
            //Utility.InitializeCommerceRuntime();
            //runtime = Utility._CommerceRuntime;
            //currentChannelState = ChannelState.InitializeChannel(runtime, new Guid());
            //AutoMapper.Mapper.Initialize(cnf => cnf.AddProfile(new ErpMappingConfiguration()));
        }

        public List<ErpInventoryInfo> GetStoreAvailability(string itemId, string variantId)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<object> InventoryLookup(string itemId, string variantId)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<object> InvokeExtensionMethod(string methodName, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        /*
        //NS: Comment Start
        public ReadOnlyCollection<OrgUnitLocation> GetNearbyStores(decimal latitude, decimal longitude, int distance)
        {
            Collection<StoreLocation> stores = new Collection<StoreLocation>();

            QueryResultSettings criteria = new QueryResultSettings(PagingInfo.AllRecords);

            SearchArea searchArea = new SearchArea();
            searchArea.Latitude = latitude;
            searchArea.Longitude = longitude;
            searchArea.DistanceUnit = DistanceUnit.Miles;
            searchArea.Radius = distance;

            var manager = StoreLocatorManager.Create(base.currentChannelState.Runtime);
            return manager.GetStoreLocations(criteria, searchArea).Results;
        }
        //NS: Comment Start
        */

        List<ErpStore> IStoreController.GetStores()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpStore> StoreList = new List<ErpStore>();
            try
            {
                var storeCRTManager = new StoreCRTManager();
                StoreList = storeCRTManager.SearchStores(currentStore.StoreKey);

                /*
                //NS: Comment Start
                SearchStoreCriteria storeSearchCriteria = new SearchStoreCriteria();
                QueryResultSettings settings = new QueryResultSettings(PagingInfo.AllRecords);
                PagedResult<OrgUnit> Stores = currentChannelState.ChannelManager.SearchStores(storeSearchCriteria, settings);

                List<long> ProductIds = new List<long>();
                ErpStore ErpStoreObj = new ErpStore();

                foreach (OrgUnit st in Stores.Results)
                {

                    StoreList.Add(new ErpStore()
                    {
                        Status = "Enabled",
                        StoreId = st.RecordId.ToString(),
                        Name = st.OrgUnitAddress.Name.ToString(),
                        isEnabled = true,
                        City = st.OrgUnitAddress.City,
                        State = st.OrgUnitAddress.StateName,
                        Country = st.OrgUnitAddress.ThreeLetterISORegionName,
                        zipcode = st.OrgUnitAddress.ZipCode,
                        Latitute = st.Latitude.ToString(),
                        Longitute = st.Longitude.ToString(),
                        Fax = "",
                        Email = st.OrgUnitAddress.Email.ToString(),
                        link = st.OrgUnitAddress.Url.ToString(),
                        zoom = "0",
                        ImageName = "",
                        tagstore = "",
                        phone = "",//(st.OrgUnitAddress.Phone.ToString()!=null?st.OrgUnitAddress.Phone.ToString():string.Empty),
                        Address = (st.OrgUnitFullAddress != null ? st.OrgUnitFullAddress.Replace("\n", " ").Replace(",", " ") : string.Empty)
                    });
                    //StoreList.Add(ErpStoreObj);
                }
                //NS: Comment Start
                */
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy);
            }
            return StoreList;
        }

        List<ErpInventoryInfo> IStoreController.GetStoreAvailability(string itemId, string variantId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var storeCRTManager = new StoreCRTManager();
            return storeCRTManager.GetStoreAvailability(itemId, variantId, currentStore.StoreKey);
        }

        ReadOnlyCollection<object> IStoreController.InvokeExtensionMethod(string methodName, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public ErpChannelCustomDiscountThresholdResponse GetDiscountThreshold()
        {
            var storeCRTManager = new StoreCRTManager();
            return storeCRTManager.GetDiscountThreshold(currentStore.StoreKey);
        }
    }
}
