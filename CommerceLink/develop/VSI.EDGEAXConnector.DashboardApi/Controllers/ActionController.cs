using System;
using System.Collections.Generic;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.DashboardApi.Infrastructure;
using VSI.EDGEAXConnector.DashboardApi.ViewModel;
using System.Linq;
using VSI.EDGEAXConnector.Logging;
using System.Configuration;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    public class ActionController : ApiBaseController
    {
        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetAPIURL()
        {
            try
            {
                string apiURL = ConfigurationManager.AppSettings["apiurl"];
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
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("Get for MapAction is deprecated, please use Get with POST parameter instead.")]
        public IHttpActionResult Get(string actionName, int storeId)
        {
            ActionRequestVM requestVM = null;
            ActionRequestDAL actionRequestDAL = null;
            try
            {
                requestVM = new ActionRequestVM();
                actionRequestDAL = new ActionRequestDAL(this.DbConnStr, this.StoreKey, this.User);
                var request = actionRequestDAL.Get(actionName, storeId);
                if (request != null)
                {
                    requestVM = MapAction(requestVM, request);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(requestVM);
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get([FromBody] GetUserActionRequest getUserAction)
        {
            ActionRequestVM requestVM = null;
            ActionRequestDAL actionRequestDAL = null;
            try
            {
                requestVM = new ActionRequestVM();
                actionRequestDAL = new ActionRequestDAL(this.DbConnStr, this.StoreKey, this.User);
                var request = actionRequestDAL.Get(getUserAction.ActionName, getUserAction.StoreId);
                if (request != null)
                {
                    requestVM = MapAction(requestVM, request);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok(requestVM);
        }

        [HttpPut]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Save(ActionRequestVM requestVM)
        {
            ActionRequestDAL actionRequestDAL = null;
            bool result = false;
            try
            {
                actionRequestDAL = new ActionRequestDAL(this.DbConnStr, this.StoreKey, this.User);
                var request = actionRequestDAL.Get(requestVM.ActionName, requestVM.StoreId);
                
                if (request == null)
                {
                    result = Add(requestVM);
                }
                else
                {
                    result = Update(requestVM, request);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
            return Ok("Success");
        }
        private ActionRequestVM MapAction(ActionRequestVM requestVM, ActionRequest request)
        {
            try
            {
                requestVM.ActionName = request.ActionName;
                requestVM.StoreId = request.StoreId;
                requestVM.Request = request.Request;
                requestVM.CreatedBy = request.CreatedBy;
                requestVM.CreatedOn = request.CreatedOn;
                requestVM.ModifiedBy = request.ModifiedBy;
                requestVM.ModifiedOn = request.ModifiedOn;
            }
            catch (Exception)
            {
                throw;
            }

            return requestVM;
        }
        private bool Add(ActionRequestVM requestVM)
        {
            ActionRequestDAL actionRequestDAL = null;
            try
            {
                actionRequestDAL = new ActionRequestDAL(this.DbConnStr, this.StoreKey, this.User);
                ActionRequest request = new ActionRequest();
                request.ActionName = requestVM.ActionName;
                request.StoreId = requestVM.StoreId;
                request.Request = requestVM.Request;
                request.CreatedBy = this.User;
                actionRequestDAL.Add(request);
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private bool Update(ActionRequestVM requestVM, ActionRequest request)
        {
            ActionRequestDAL actionRequestDAL = null;
            try
            {
                actionRequestDAL = new ActionRequestDAL(this.DbConnStr, this.StoreKey, this.User);
                request.Request = requestVM.Request;
                request.ModifiedOn = System.DateTime.UtcNow;
                request.ModifiedBy = this.User;
                actionRequestDAL.Update(request);
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        #region Action Request, Response classes

        public class GetUserActionRequest
        {
            public string ActionName { get; set; }
            public int StoreId { get; set; }

        }


        #endregion
    }
}

