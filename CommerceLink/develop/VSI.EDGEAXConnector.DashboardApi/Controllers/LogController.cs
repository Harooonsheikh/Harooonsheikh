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
    public class LogController : ApiBaseController
    {
        public LogController()
        {
        }

        [HttpGet]
        [Route("get")]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get(int? storeId, int? pageSize, int? offSet)
        {
            int pageSizeCalculated = 1;
            int offsetCalculated = 20;
            int storeIdCalculated = 0;

            if (pageSize != null)
            {
                if (pageSize.Value > 1)
                {
                    pageSizeCalculated = pageSize.Value;
                }
            }

            if (offSet != null)
            {
                if (offSet.Value > 1)
                {
                    offsetCalculated = offSet.Value;
                }
            }

            if (storeId != null)
            {
                if (storeId.Value > 0)
                {
                    storeIdCalculated = storeId.Value;
                }
            }

            LogsDAL logMgr = null;
            try
            {
                logMgr = new LogsDAL(this.DbConnStr, this.StoreKey, this.User);
                var log = logMgr.AllLogs(storeIdCalculated, pageSizeCalculated, offsetCalculated);
                List<LogVM> listVM = new List<LogVM>();
                log.ForEach(m =>
                {
                    listVM.Add(MapLog(m));
                });
                return Ok(listVM);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [Route("get")]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get(GetRequest getRequest)
        {
            int pageSizeCalculated = 1;
            int offsetCalculated = 20;
            int storeIdCalculated = 0;

            if (getRequest.pageSize != null)
            {
                if (getRequest.pageSize.Value > 1)
                {
                    pageSizeCalculated = getRequest.pageSize.Value;
                }
            }

            if (getRequest.offSet != null)
            {
                if (getRequest.offSet.Value > 1)
                {
                    offsetCalculated = getRequest.offSet.Value;
                }
            }

            if (getRequest.storeId != null)
            {
                if (getRequest.storeId.Value > 0)
                {
                    storeIdCalculated = getRequest.storeId.Value;
                }
            }

            LogsDAL logMgr = null;
            try
            {
                logMgr = new LogsDAL(this.DbConnStr, this.StoreKey, this.User);
                var log = logMgr.AllLogs(storeIdCalculated, pageSizeCalculated, offsetCalculated);
                List<LogVM> listVM = new List<LogVM>();
                log.ForEach(m =>
                {
                    listVM.Add(MapLog(m));
                });
                return Ok(listVM);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private LogVM MapLog(Log log)
        {
            LogVM logvm = new LogVM();
            logvm.StackTrace = log.EventMessage;
            logvm.Level = log.EventLevel;
            logvm.LogId = log.LogId;
            logvm.CreatedOn = log.CreatedOn;
            logvm.StoreId = log.StoreId;
            return logvm;
        }


        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get(int daysCount)
        {
            try
            {
                LogsDAL logsDAL = new LogsDAL(this.DbConnStr, this.StoreKey, this.User);
                var logs = logsDAL.GetLogs(daysCount);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }

    #region Integration Controller, Response classes

    public class GetRequest
    {
        public int? storeId { get; set; }
        public int? offSet { get; set; }
        public int? pageSize { get; set; }
    }


    #endregion
}