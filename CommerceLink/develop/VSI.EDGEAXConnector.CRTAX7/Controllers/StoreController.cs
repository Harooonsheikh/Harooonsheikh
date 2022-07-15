using EdgeAXCommerceLink.Commerce.RetailProxy;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRTAX7.Controllers
{
    public class StoreController : BaseController, IStoreController
    {

        public List<ErpStore> SearchStores()
        {
            return AsyncSearchStores().Result;
        }

        private async Task<List<ErpStore>> AsyncSearchStores()
        {
            List<ErpStore> lstErpStores = new List<ErpStore>();

            var ouManager = RPFactory.GetManager<IOrgUnitManager>();
            SearchStoreCriteria storeSearchCriteria = new SearchStoreCriteria();

            QueryResultSettings queryResultSettings = new QueryResultSettings();
            queryResultSettings.Paging = Paging_0_1000;
            queryResultSettings.Sorting = SortingChannel;

            PagedResult<OrgUnit> stores;

            try
            {
                stores = await ouManager.Search(storeSearchCriteria, queryResultSettings);

            }
            catch (RetailProxyException rpe)
            {
                AXAdapterException exp = new AXAdapterException(rpe.ErrorResourceId, "Retail Proxy Exception: " + rpe.Message, rpe);
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
            return lstErpStores;
        }

        public List<ErpCategory> GetChannelCategoryHierarchy()
        {
            List<ErpCategory> lstCategory = new List<ErpCategory>();

            return lstCategory;
        }

        public List<ErpAttributeCategory> GetChannelCategoryAttributes(ErpCategory category)
        {
            List<ErpAttributeCategory> lstAttributeCategory = new List<ErpAttributeCategory>();

            return lstAttributeCategory;
        }

        List<ErpStore> IStoreController.SearchStores()
        {
            throw new NotImplementedException();
        }

        List<ErpCategory> IStoreController.GetChannelCategoryHierarchy()
        {
            throw new NotImplementedException();
        }

        List<ErpAttributeCategory> IStoreController.GetChannelCategoryAttributes(ErpCategory category)
        {
            throw new NotImplementedException();
        }

        List<ErpInventoryInfo> IStoreController.GetStoreAvailability(string itemId, string variantId)
        {
            throw new NotImplementedException();
        }
    }
}
