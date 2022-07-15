using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.DashboardApi.Infrastructure;
using VSI.EDGEAXConnector.DashboardApi.ViewModel;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.ViewModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.MongoData.Helpers;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    //[DashboardActionFilter]
    [RoutePrefix("api/store")]
    public class StoreController : ApiBaseController
    {
        public string MongoDBName = string.Empty;
        public string MongoDBConn = string.Empty;

        [HttpGet]
        public IHttpActionResult Get()
        {
            StoreDAL storeMgr = null;
            List<Store> stores = null;
            List<KeyValuePair<string, string>> storeKeyPair = new List<KeyValuePair<string, string>>();
            try
            {

                storeMgr = new StoreDAL(this.DbConnStr);
                stores = storeMgr.GetActiveStores();
                stores.ForEach(m =>
                {
                    storeKeyPair.Add(new KeyValuePair<string, string>(m.StoreId.ToString(), m.Name));
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(storeKeyPair);
        }

        [HttpGet]
        public IHttpActionResult Get(int storeId)
        {
            StoreDAL storeMgr = null;
            Store store = null;
            StoreVM storeVM = null;

            try
            {
                storeMgr = new StoreDAL(this.DbConnStr);
                store = storeMgr.Get(storeId);

                if (store != null)
                {
                    storeVM = new StoreVM();
                    storeVM = MapStore(storeVM, store);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok(storeVM);
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetAll()
        {
            StoreDAL storeMgr = null;
            List<StoreVM> lstStoreVM = null;
            List<Store> stores = null;

            try
            {
                storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
                lstStoreVM = new List<StoreVM>();
                stores = new List<Store>();
                stores = storeMgr.GetActiveStores();
                stores.ForEach(s =>
                {
                    StoreVM storeVM = new StoreVM();
                    storeVM = MapStore(storeVM, s);
                    lstStoreVM.Add(storeVM);
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(lstStoreVM);

        }
        [HttpDelete]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Disable(int storeId)
        {
            StoreDAL storeMgr = null;
            try
            {
                storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
                bool result = storeMgr.Disable(storeId);
                if (result)
                {
                    return Ok("Success");
                }
                return BadRequest("Failure");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Add(StoreVM storeVM)
        {
            StoreDAL storeMgr = null;
            Store store = null;

            var AppSettingValues = new List<KeyValuePair<string, string>>();
            try
            {
                storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
                store = new Store();
                store.Name = storeVM.Name;
                store.Description = storeVM.Description;
                store.StoreKey = storeVM.StoreKey;
                store.EcomTypeId = storeVM.EcomType.Key;
                store.OrganizationId = storeVM.OrganizationId;
                store.ERPTypeId = storeVM.ERPType.Key;
                store.CreatedBy = this.User;
                store.IsActive = storeVM.IsActive;
                store.StoreKey = Convert.ToString(Guid.NewGuid());
                store.IsSFTPDirTreeCreated = false;

                foreach (AppSettingVM settings in storeVM.applist)
                {
                    AppSettingValues.Add(new KeyValuePair<string, string>(settings.Key, settings.Value));
                }

                store.DuplicateOf = storeVM.DuplicateOf == -1 ? null : storeVM.DuplicateOf;
                storeMgr.Add(store);

                storeVM = MapStore(storeVM, store);

                if (storeVM.DuplicateOf != null)
                {
                    storeMgr.DuplicateStore(storeVM.DuplicateOf, storeVM.StoreId, AppSettingValues);
                }

                return Ok(storeVM);
            }
            catch (Exception ex)
            {
                storeMgr.Delete(storeVM.StoreId);
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Update(StoreVM storeVM)
        {
            StoreDAL storeMgr = null;
            Store store = null;
            try
            {
                storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
                store = storeMgr.Get(storeVM.StoreId);

                if (store != null)
                {
                    store.Name = storeVM.Name;
                    store.Description = storeVM.Description;
                    store.StoreKey = storeVM.StoreKey;
                    store.EcomTypeId = storeVM.EcomType.Key;
                    store.ModifiedBy = this.User;
                    store.ModifiedOn = System.DateTime.UtcNow;
                    storeMgr.Update(store);
                    storeVM = MapStore(storeVM, store);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(storeVM);
        }
        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("Verify is deprecated, please use Verify with POST parameter instead.")]
        public IHttpActionResult Verify(string storeName, int storeId)
        {
            StoreDAL storeMgr = null;
            try
            {
                storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
                bool result = storeMgr.Verify(storeName, storeId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Verify([FromBody] GetVerifyRequest VerifyRequest)
        {
            StoreDAL storeMgr = null;
            try
            {
                storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
                bool result = storeMgr.Verify(VerifyRequest.StoreName, VerifyRequest.StoreId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        private StoreVM MapStore(StoreVM storeVM, Store store)
        {
            EcomTypeDAL ecomTypeMgr = null;
            ERPTypeDAL erpTypeMgr = null;
            try
            {
                ecomTypeMgr = new EcomTypeDAL(this.DbConnStr);
                erpTypeMgr = new ERPTypeDAL(this.DbConnStr);
                storeVM.CreatedBy = store.CreatedBy;
                storeVM.CreatedOn = store.CreatedOn;
                storeVM.Description = store.Description;
                storeVM.EcomType = ecomTypeMgr.Get(store.EcomTypeId);
                storeVM.ERPType = erpTypeMgr.Get(store.ERPTypeId);
                storeVM.IsActive = store.IsActive;
                storeVM.ModifiedBy = store.ModifiedBy;
                storeVM.ModifiedOn = store.ModifiedOn;
                storeVM.Name = store.Name;
                storeVM.OrganizationId = store.OrganizationId;
                storeVM.StoreId = store.StoreId;
                storeVM.StoreKey = store.StoreKey;
                storeVM.DuplicateOf = store.DuplicateOf;
            }
            catch (Exception)
            {
                throw;
            }

            return storeVM;
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("VerifyStoreKey is deprecated, please use VerifyStoreKey with POST parameter instead.")]

        public IHttpActionResult VerifyStoreKey(string storeKey, int storeId)
        {
            StoreDAL storeMgr = null;
            try
            {
                storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
                bool result = storeMgr.VerifyStoreKey(storeKey, storeId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult VerifyStoreKey([FromBody] GetVerifyStoreKeyRequest VerifyStoreKeyRequest)
        {
            StoreDAL storeMgr = null;
            try
            {
                storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
                bool result = storeMgr.VerifyStoreKey(VerifyStoreKeyRequest.StoreKey, VerifyStoreKeyRequest.StoreId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetPaymentConnectorAPIURL()
        {
            try
            {
                string apiURL = ConfigurationManager.AppSettings["paymentConnectorAPIURL"];
                if (apiURL != string.Empty)
                {
                    return Ok(apiURL);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Obsolete("StoreKeyOfCountry is deprecated, please use StoreKeyOfCountry with POST parameter instead.")]
        public IHttpActionResult StoreKeyOfCountry(string countryCode)
        {
            StoreDAL storeMgr = null;
            try
            {
                storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
                if (string.IsNullOrWhiteSpace(countryCode))
                {
                    return Ok(this.CountriesList());
                }
                string result = storeMgr.StoreKeyOfCountry(countryCode);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        public IHttpActionResult StoreKeyOfCountry([FromBody] GetStoreKeyOfCountryRequest StoreKeyOfCountryRequest)
        {
            StoreDAL storeMgr = null;
            try
            {
                storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
                if (string.IsNullOrWhiteSpace(StoreKeyOfCountryRequest.CountryCode))
                {
                    return Ok(this.CountriesList());
                }
                string result = storeMgr.StoreKeyOfCountry(StoreKeyOfCountryRequest.CountryCode);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        public List<Countries> CountriesList()
        {
            StoreDAL storeMgr = null;
            storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
            List<Countries> countryNames = new List<Countries>();
            countryNames = storeMgr.CountriesList();
            return countryNames;
        }

        [HttpGet]
        public IHttpActionResult AutoIncrement(string fileName)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                SaleOrderMongoHelper helper = new SaleOrderMongoHelper(MongoDBConn, MongoDBName);
                helper.IncrementalData(fileName);
                return Ok(helper.IncrementalData(fileName));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #region Integration Controller, Response classes

        public class GetVerifyRequest
        {
            public string StoreName { get; set; }
            public int StoreId { get; set; }
        }
        public class GetVerifyStoreKeyRequest
        {
            public string StoreKey { get; set; }
            public int StoreId { get; set; }

        }
        public class GetStoreKeyOfCountryRequest
        {
            public string CountryCode { get; set; }

        }

        #endregion
    }
}
