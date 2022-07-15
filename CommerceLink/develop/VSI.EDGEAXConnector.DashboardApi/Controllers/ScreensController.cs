using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.DashboardApi.ViewModel;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    [RoutePrefix("api/ScreenNames")]
    public class ScreensController : ApiBaseController
    {
        protected ScreensController()
        {
        }
        [HttpGet]
        [Route("Get")]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get()
        {
            ApplicationSettingsDAL appDal = null;
            List<string> lst = null;
            try
            {
                appDal = new ApplicationSettingsDAL(this.DbConnStr, this.StoreKey,this.User);
                lst = appDal.GetScreenNames();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Route("Get")]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get([FromUri] string screenName)
        {
            List<AppSettingVM> appSetting = null;
            ApplicationSettingsDAL appDal = null;
            List<ApplicationSetting> lst = null;
            try
            {
                string tmpScreenName = screenName.Contains("||") ? screenName.Replace("||", "&") : screenName;
                appDal = new ApplicationSettingsDAL(this.DbConnStr, this.StoreKey, this.User);
                lst = appDal.GetApplicationSettingsByScreenName(tmpScreenName);
                appSetting = new List<AppSettingVM>();
                lst.ForEach(m =>
                {
                    AppSettingVM setVM = new AppSettingVM();
                    setVM.Id = m.ApplicationSettingId;
                    setVM.Name = m.Name;
                    setVM.Value = m.Value;
                    setVM.FieldType = m.FieldType.Name;
                    appSetting.Add(setVM);
                });
                return Ok(appSetting);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(List<AppSettingVM> settingsIncoming)
        {
            ApplicationSettingsDAL appDal = null;
            List<ApplicationSetting> dbSettings = null;
            try
            {
                appDal = new ApplicationSettingsDAL(this.DbConnStr, this.StoreKey, this.User);
                dbSettings = new List<ApplicationSetting>();
                settingsIncoming.ForEach(m =>
                {
                    var setting = appDal.GetApplicationSetting(m.Id);
                    setting.Value = m.Value;
                    dbSettings.Add(setting);
                });


                bool result = appDal.UpdateApplicationSettings(dbSettings);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
