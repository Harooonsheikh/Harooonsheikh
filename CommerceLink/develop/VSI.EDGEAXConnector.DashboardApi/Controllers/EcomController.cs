using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.DashboardApi.Infrastructure;
using VSI.EDGEAXConnector.DashboardApi.ViewModel;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    public class EcomController : ApiBaseController
    {
        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get()
        {

            EcomTypeDAL ecomTypeMgr = null;
            List<EcomType> ecomTypes = null;
            List<KeyValuePair<int, string>> lstEcomType = new List<KeyValuePair<int, string>>();
            try
            {
                ecomTypeMgr = new EcomTypeDAL(this.DbConnStr);
                ecomTypes = ecomTypeMgr.Get();
                ecomTypes.ForEach(e =>
                {
                    lstEcomType.Add(new KeyValuePair<int, string>(e.EcomTypeId, e.EcomName));
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(lstEcomType);

        }
    }
}