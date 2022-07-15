using System;
using System.Collections.Generic;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.ViewModels;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    public class JobsController : ApiBaseController
    {
        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("Get is deprecated, please use Get with POST parameter instead.")]
        public IHttpActionResult Get(int storeId, bool type)
        {
            try
            {
                JobsDAL jobDAL = new JobsDAL(this.DbConnStr, this.StoreKey, this.User);
                List<JobVM> lstJob = jobDAL.Get(storeId, type);
                return Ok(lstJob);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get([FromBody] GetRequest GetRequest)
        {
            {
                try
                {
                    JobsDAL jobDAL = new JobsDAL(this.DbConnStr, this.StoreKey, this.User);
                    List<JobVM> lstJob = jobDAL.Get(GetRequest.StoreId, GetRequest.Type);
                    return Ok(lstJob);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
        }

        [HttpPut]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Update(JobVM jobVM)
        {
            try
            {
                JobsDAL jobDAL = new JobsDAL(this.DbConnStr, this.StoreKey, this.User);
                JobSchedule jobSchedule = jobDAL.Get(jobVM.JobID, jobVM.StoreId);
                jobSchedule.JobInterval = jobVM.JobInterval;
                jobSchedule.IsRepeatable = jobVM.IsRepeatable;
                jobSchedule.IsActive = jobVM.IsActive;
                jobSchedule.StartTime = jobVM.StartTime;
                jobSchedule.ModifiedBy = this.User;
                jobSchedule.ModifiedOn = System.DateTime.UtcNow;
                bool result = jobDAL.Update(jobSchedule);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        #region Jobs Controller, Response classes

        public class GetRequest
        {

            public int StoreId { get; set; }
            public bool Type { get; set; }

        }


        #endregion
    }
}

