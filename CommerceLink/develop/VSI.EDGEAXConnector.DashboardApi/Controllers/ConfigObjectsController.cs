using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.DashboardApi.ViewModel;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    public class ConfigObjectsController : ApiBaseController
    {
        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get()
        {
            ConfigurableObjectDAL configObjectDAL = null;
            List<ConfigurableObject> lstConfigObjects = null;
            List<ConfigObjectVM> lstConfigObjectsVM = null;
            try
            {
                configObjectDAL = new ConfigurableObjectDAL(this.DbConnStr, this.StoreKey, this.User);
                lstConfigObjects = configObjectDAL.GetAllConfigurableObjects();
                lstConfigObjectsVM = lstConfigObjects.Select(m => MapConfigObject(m)).ToList();
                return Ok(lstConfigObjectsVM);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPut]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Update(ConfigObjectVM configObject)
        {
            ConfigurableObjectDAL configObjectDAL = null;
            ConfigurableObject updatedConfigObject = null;
            try
            {
                configObjectDAL = new ConfigurableObjectDAL(this.DbConnStr, this.StoreKey, this.User);
                updatedConfigObject = configObjectDAL.UpdateConfigurableObject(MapConfigObject(configObject));
                if (updatedConfigObject != null)
                {
                    return Ok(MapConfigObject(updatedConfigObject));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private ConfigurableObject MapConfigObject(ConfigObjectVM vm)
        {
            ConfigurableObject configOb = new ConfigurableObject();
            configOb.ComValue = vm.ComValue;
            configOb.ConfigurableObjectId = vm.Id;
            configOb.ErpValue = vm.ErpValue;
            configOb.StoreId = vm.StoreId_FK;
            configOb.EntityType = vm.EntityType;
            configOb.ConnectorKey = vm.ConnectorKey;
            return configOb;
        }

        private ConfigObjectVM MapConfigObject(ConfigurableObject config)
        {
            ConfigObjectVM vm = new ConfigObjectVM();
            vm.ComValue = config.ComValue;
            vm.ErpValue = config.ErpValue;
            vm.Id = config.ConfigurableObjectId;
            vm.EntityType = config.EntityType;
            vm.ConnectorKey = config.ConnectorKey;

            if (config.StoreId != null)
            {
                vm.StoreId_FK = config.StoreId.Value;
            }
            return vm;
        }
    }
}

