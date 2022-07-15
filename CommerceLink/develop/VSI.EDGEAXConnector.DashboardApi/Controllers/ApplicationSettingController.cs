using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Routing;
using VSI.EDGEAXConnector.DashboardApi.ViewModel;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    [RoutePrefix("api/AppSetting")]
    public class ApplicationSettingController : ApiBaseController
    {
        public ApplicationSettingController()
        {
        }

        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(List<ApplicationSetting> settings)
        {
            ApplicationSettingsDAL appDal = null;
            try
            {
                appDal =  new ApplicationSettingsDAL(this.DbConnStr, this.StoreKey,this.User);
                bool result = appDal.UpdateApplicationSettings(settings);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("Get")]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get([FromUri] string sectionName, [FromUri] string subSection)
        {
            ApplicationSettingsDAL appDal = null;
            List<ApplicationSetting> lst = null;
            try
            {
                appDal = new ApplicationSettingsDAL(this.DbConnStr,this.StoreKey, this.User);
                lst = appDal.GetApplicationSettings(sectionName,subSection);
            
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("Get")]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("Get for application settings is deprecated, please use Get with POST parameter instead.")]
        public IHttpActionResult Get(GetAppSettingRequest getAppSetting)
        {
            ApplicationSettingsDAL appDal = null;
            List<ApplicationSetting> lst = null;
            try
            {
                appDal = new ApplicationSettingsDAL(this.DbConnStr, this.StoreKey, this.User);
                lst = appDal.GetApplicationSettings(getAppSetting.SectionName, getAppSetting.SubSection);

                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Route("GetApplicationSettingsByStoreId")]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetApplicationSettingsByStoreId([FromUri] int storeId )
        {
            List<AppSettingVM> appSetting = null;
            ApplicationSettingsDAL appDal = null;
            List<ApplicationSetting> lst = null;
            try
            {
                appDal = new ApplicationSettingsDAL(this.DbConnStr, this.StoreKey, this.User);
                lst = appDal.GetApplicationSettingsByIsUsedForDuplicateStore(storeId, true);
                appSetting = new List<AppSettingVM>();
                lst.ForEach(m =>
                {
                    AppSettingVM setVM = new AppSettingVM();
                    setVM.Id = m.ApplicationSettingId;
                    setVM.Name = m.Name;
                    setVM.FieldTypeId = m.FieldTypeId;
                    setVM.Value = m.Value;
                    setVM.Key = m.Key;
                    setVM.StoreId = m.StoreId;
                    //setVM.FieldType = m.FieldType.Name;
                    appSetting.Add(setVM);
                });
                return Ok(appSetting);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #region Application Request, Response classes
        /// <summary>
        /// 
        /// </summary>
        public class GetAppSettingRequest
        {
            public string SectionName { get; set; }
            public string SubSection { get; set; }
        }


        #endregion
    }
}