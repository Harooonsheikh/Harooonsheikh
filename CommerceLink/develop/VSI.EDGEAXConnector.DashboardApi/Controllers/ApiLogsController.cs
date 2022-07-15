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
    [RoutePrefix("api/Logs")]
    public class ApiLogsController : ApiBaseController
    {
        public ApiLogsController()
        {
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("Get for logs is deprecated, please use Get with POST parameter instead.")]
        public IHttpActionResult Get(int daysCount)
        {
            try
            {
                ApiLogsDAL logsDAL = new ApiLogsDAL(this.DbConnStr, this.StoreKey, this.User);
                var logs = logsDAL.GetLogs(daysCount);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get([FromBody] GetLogsRequest getLogs)
        {
            try
            {
                ApiLogsDAL logsDAL = new ApiLogsDAL(this.DbConnStr, this.StoreKey, this.User);
                var logs = logsDAL.GetLogs(getLogs.DaysCount);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("GetFilterResult is deprecated, please use GetFilterResult with POST parameter instead.")]
        public IHttpActionResult GetFilterResult(string logType, string methodName, string directionType, string fromDate, string toDate, string searchQuery)
        {
            try
            {
                DateTime startDate = Convert.ToDateTime(fromDate);
                DateTime endDate = (Convert.ToDateTime(toDate).Date).AddDays(1);

                ApiLogsDAL logsDAL = new ApiLogsDAL(this.DbConnStr, this.StoreKey, this.User);

                List<RequestResponse> requestResponseLogResult = new List<RequestResponse>();

                if (logType.ToUpper().Equals("ACTIVE"))
                {
                    var logs = logsDAL.GetFilterdLogs(methodName, directionType, startDate, endDate, searchQuery);
                    return Ok(logs);
                }
                else
                {
                    var logs = logsDAL.GetFilterdArchiveLogs(methodName, directionType, startDate, endDate, searchQuery);
                    return Ok(logs);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetFilterResult([FromBody] GetFilterResultRequest getFilterResult)
        {
            try
            {
                DateTime startDate = Convert.ToDateTime(getFilterResult.FromDate);
                DateTime endDate = (Convert.ToDateTime(getFilterResult.ToDate).Date).AddDays(1);

                ApiLogsDAL logsDAL = new ApiLogsDAL(this.DbConnStr, this.StoreKey, this.User);

                List<RequestResponse> requestResponseLogResult = new List<RequestResponse>();

                if (getFilterResult.LogType.ToUpper().Equals("ACTIVE"))
                {
                    var logs = logsDAL.GetFilterdLogs(getFilterResult.MethodName, getFilterResult.DirectionType, startDate, endDate, getFilterResult.SearchQuery);
                    return Ok(logs);
                }
                else
                {
                    var logs = logsDAL.GetFilterdArchiveLogs(getFilterResult.MethodName, getFilterResult.DirectionType, startDate, endDate, getFilterResult.SearchQuery);
                    return Ok(logs);
                }

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Methods()
        {
            try
            {
                ApiLogsDAL apiLogsDAL = new ApiLogsDAL(this.DbConnStr, this.StoreKey, this.User);
                var methods = apiLogsDAL.GetMethods();
                return Ok(methods);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        #region APILogs Request, Response classes

        public class GetLogsRequest
        {
            public int DaysCount { get; set; }

        }
        /// <summary>
        /// GetFilterResultRequest class
        /// </summary>
        public class GetFilterResultRequest
        {
            public string LogType { get; set; }
            public string MethodName { get; set; }
            public string DirectionType { get; set; }
            public string FromDate { get; set; }
            public string ToDate { get; set; }
            public string SearchQuery { get; set; }

        }

        #endregion
    }
}