using Microsoft.Dynamics.Commerce.RetailProxy;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using NewRelic.Api.Agent;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using IOrgUnitManager = Microsoft.Dynamics.Commerce.RetailProxy.IOrgUnitManager;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class StoreController : BaseController, IStoreController
    {
        public StoreController(string storeKey) : base(storeKey)
        {

        }
        public List<ErpStore> SearchStores()
        {
            return AsyncSearchStores().Result;
        }

        private async Task<List<ErpStore>> AsyncSearchStores()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpStore> lstErpStores = new List<ErpStore>();


            SearchStoreCriteria storeSearchCriteria = new SearchStoreCriteria();

            QueryResultSettings queryResultSettings = new QueryResultSettings();
            queryResultSettings.Paging = Paging_0_1000;
            queryResultSettings.Sorting = SortingChannel;

            PagedResult<OrgUnit> stores;

            try
            {
                stores = await Search(storeSearchCriteria, queryResultSettings);
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }

            foreach (OrgUnit st in stores.Results)
            {
                lstErpStores.Add(new ErpStore()
                {
                    Status = "Enabled",
                    StoreId = st.RecordId.ToString(),
                    Name = st.OrgUnitAddress.Name.ToString(),
                    isEnabled = true,
                    City = st.OrgUnitAddress.City,
                    State = st.OrgUnitAddress.StateName,
                    Country = st.OrgUnitAddress.ThreeLetterISORegionName,
                    ZipCode = st.OrgUnitAddress.ZipCode,
                    Latitude = st.Latitude.ToString(),
                    Longitude = st.Longitude.ToString(),
                    Fax = "",
                    Email = st.OrgUnitAddress.Email.ToString(),
                    Link = st.OrgUnitAddress.Url.ToString(),
                    Zoom = "0",
                    ImageName = "",
                    TagStore = "",
                    Phone = "",//(st.OrgUnitAddress.Phone.ToString()!=null?st.OrgUnitAddress.Phone.ToString():string.Empty),
                    Address = (st.OrgUnitFullAddress != null ? st.OrgUnitFullAddress.Replace("\n", " ").Replace(",", " ") : string.Empty)
                });
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return lstErpStores;
        }
        public List<ErpCategory> GetChannelCategoryHierarchy()
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            //TODO : SK - enalble with RS function
            try
            {
                List<ErpCategory> lstCategory = new List<ErpCategory>();
                QueryResultSettings getCategoriesCriteria = new QueryResultSettings();
                getCategoriesCriteria.Paging = new PagingInfo();
                getCategoriesCriteria.Paging.Skip = 0;
                getCategoriesCriteria.Paging.Top = 1000; //  long.MaxValue - 1;
                var receivedPgResults = GetCategories(getCategoriesCriteria);
                foreach (var item in receivedPgResults.Results)
                {
                    ErpCategory category = _mapper.Map<ErpCategory>(item);
                    lstCategory.Add(category);
                }

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

                return lstCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<ErpAttributeCategory> GetChannelCategoryAttributes(ErpCategory category)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpAttributeCategory> lstAttributeCategory = new List<ErpAttributeCategory>();

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return lstAttributeCategory;
        }
        public List<ErpInventoryInfo> GetStoreAvailability(string itemId, string variantId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            List<ErpInventoryInfo> lstInventoryInfo = new List<ErpInventoryInfo>();

            QueryResultSettings queryResultSettings = new QueryResultSettings();
            queryResultSettings.Paging = new PagingInfo();
            queryResultSettings.Paging.Skip = 0;
            queryResultSettings.Paging.Top = Paging_0_1000.Top;

            try
            {
                PagedResult<string> itemAvailability;

                var orgUnitManager = RPFactory.GetManager<IOrgUnitManager>();
                //var result = Task.Run(async () => await orgUnitManager.GetAvailableInventory(itemId, variantId, String.Empty, queryResultSettings)).Result;
                itemAvailability = AsyncInventoryLookup(itemId, variantId, queryResultSettings);

                foreach (var inventData in itemAvailability.Results)
                {
                    string[] inventInfo = inventData.Split(',');

                    if (inventInfo.Length > 0)
                    {
                        ErpInventoryInfo erpInventInfo = new ErpInventoryInfo();

                        erpInventInfo.ItemId = inventInfo[0];
                        erpInventInfo.InventoryLocationId = inventInfo[1];
                        erpInventInfo.StoreName = inventInfo[2];
                        erpInventInfo.InventoryAvailable = inventInfo[3];

                        lstInventoryInfo.Add(erpInventInfo);
                    }
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                throw exp;
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return lstInventoryInfo;
        }
        public ErpChannelCustomDiscountThresholdResponse GetDiscountThreshold()
        {
            ErpChannelCustomDiscountThresholdResponse response = new ErpChannelCustomDiscountThresholdResponse(false, string.Empty, string.Empty);
            try
            {
                var discountThreshold = ECL_TV_GetDiscountThreshold();
                if (discountThreshold != null)
                {
                    response = new ErpChannelCustomDiscountThresholdResponse(discountThreshold.Success, discountThreshold.Message, discountThreshold.Result);
                }
            }
            catch (RetailProxyException rpe)
            {
                string message = CommerceLinkLogger.GetMessage(CommerceLinkLoggerMessages.VSICL40005);
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, message + rpe.Message, rpe);
                return new ErpChannelCustomDiscountThresholdResponse(false, rpe.Message, string.Empty);
            }
            catch (Exception exception)
            {
                CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exception.Message);
                return new ErpChannelCustomDiscountThresholdResponse(false, exception.Message, string.Empty);
            }
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return response;
        }
        /// <summary>
        /// Asynchronous Inventory lookup search
        /// </summary>
        /// <param name="itemId">item id</param>
        /// <param name="variantId">variant id</param>
        /// <param name="queryResultSettings">Query Result Settings for asynchronous call</param>
        /// <returns></returns>
        private PagedResult<string> AsyncInventoryLookup(string itemId, string variantId, QueryResultSettings queryResultSettings)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var itemAvailability = ECL_InventoryLookup(itemId, variantId, queryResultSettings);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return itemAvailability;
        }
        #region RetailServer API
        [Trace]
        private PagedResult<string> ECL_InventoryLookup(string itemId, string variantId, QueryResultSettings queryResultSettings)
        {
            throw new NotImplementedException();
            //var orgUnitManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IOrgUnitManager>();
            //PagedResult<string> itemAvailability = Task.Run(async () =>
            //    await orgUnitManager.ECL_InventoryLookup(itemId, variantId, baseCompany, queryResultSettings)).Result;
            //return itemAvailability;
        }
        [Trace]
        private async Task<PagedResult<OrgUnit>> Search(SearchStoreCriteria storeSearchCriteria, QueryResultSettings queryResultSettings)
        {
            throw new NotImplementedException();
            //PagedResult<OrgUnit> stores;
            //var ouManager = RPFactory.GetManager<IOrgUnitManager>();
            //stores = await ouManager.Search(storeSearchCriteria, queryResultSettings);
            //return stores;
        }
        [Trace]
        private PagedResult<Category> GetCategories(QueryResultSettings getCategoriesCriteria)
        {
            var mgr = RPFactory.GetManager<ICategoryManager>();
            var receivedPgResults = mgr.GetCategories(baseChannelId, null, getCategoriesCriteria).Result;
            return receivedPgResults;
        }
        [Trace]
        private ErpChannelCustomDiscountThresholdResponse ECL_TV_GetDiscountThreshold()
        {
            throw new NotImplementedException();
            //var channelManager =
            //    RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.IChannelCustomPropertiesManager>();
            //var discountThreshold = Task.Run(async () =>
            //    await channelManager.ECL_TV_GetDiscountThreshold(base.ChannelNaturalId, base.baseCompany)).Result;
            //return discountThreshold;
        }
        #endregion

    }
}
