using System;
using System.Collections.Generic;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DAL;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    public class IntegrationController : ApiBaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_erpAdapterFactory"></param>
        public IntegrationController()
        {
            ControllerName = "IntegrationController";
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("GetIntegrationKeys is deprecated, please use GetIntegrationKeys with POST parameter instead.")]
        public IHttpActionResult GetIntegrationKeys(int entityId, int startIndex, int endIndex)
        {
            try
            {
                IntegrationManager mgr = new IntegrationManager(this.DbConnStr, this.StoreKey, this.User);

                List<IntegrationKey> list = mgr.GetAllEntityKeys((Entities)entityId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetIntegrationKeys([FromBody] GetIntegrationKeysRequest IntegrationKeysRequest)
        {
            try
            {
                IntegrationManager mgr = new IntegrationManager(this.DbConnStr, this.StoreKey, this.User);

                List<IntegrationKey> list = mgr.GetAllEntityKeys((Entities)IntegrationKeysRequest.entityId);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetIntegrationEntities()
        {
            try
            {
                //IntegrationManager mgr = new IntegrationManager(this.DbConnStr, this.StoreKey);
                EntityDAL ent = new EntityDAL(this.DbConnStr, this.StoreKey, this.User);
                List<Entity> list = ent.GetEntities();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        #region Integration Controller, Response classes

        public class GetIntegrationKeysRequest
        {

            public int entityId { get; set; }

        }

        #endregion
    }
}