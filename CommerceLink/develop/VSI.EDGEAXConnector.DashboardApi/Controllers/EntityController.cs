using System;
using System.Collections.Generic;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.Data.DAL;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    public class EntityController : ApiBaseController
    {
        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get()
        {
            EntityDAL entityDAL = null;
            List<KeyValuePair<int, string>> entityList = null;
            try
            {
                entityDAL = new EntityDAL(this.DbConnStr, this.StoreKey, this.User);
                entityList = entityDAL.GetEntityList();
                return Ok(entityList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult MerchandizingEntities()
        {
            try
            {
                EntityDAL entityDAL = new EntityDAL(this.DbConnStr, this.StoreKey, this.User);
                List<KeyValuePair<int, string>> entityList = entityDAL.GetMerchandizingEntities();
                return Ok(entityList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult SaleOrderEntities()
        {
            try
            {
                EntityDAL entityDAL = new EntityDAL(this.DbConnStr, this.StoreKey, this.User);
                List<KeyValuePair<int, string>> entityList = entityDAL.GetSaleOrderEntitities();
                return Ok(entityList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}