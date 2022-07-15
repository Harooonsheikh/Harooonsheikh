using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Infrastructure;
using VSI.EDGEAXConnector.DashboardApi.Models;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.DashboardApi
{

    /// <summary>
    /// ApiBaseController class defines common custom properties and methods for all controller. 
    /// </summary>
    public abstract class ApiBaseController : ApiController
    {
        private string storeKey = null;
        public string StoreKey
        {
            get
            {

                //return RequestContext.RouteData.Values.ToDictionary(x => x.Key, y => y.Value)["storeKey"].ToString();
                return Request.Headers.GetValues("storeKey").FirstOrDefault().ToString();
            }
            
        }

        private int storeId = -1;
        public int StoreId
        {
            get
            {
                StoreDAL storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
                return storeMgr.StoreId;
                
            }
        }

        public string StoreName
        {
            get
            {
                StoreDAL storeMgr = new StoreDAL(this.DbConnStr, this.StoreKey, this.User);
                return storeMgr.StoreName;

            }
        }

        private string dbConnStr = null;
        public string DbConnStr
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["storeConnection_EF"].ConnectionString;
            }

        }

        private string user = null;
        public string User {
            get
            {
                return RequestContext.Principal.Identity.Name;
            }
        }


        public ApiBaseController()
        {
          
        }

        private ModelFactory _modelFactory;
        private ApplicationUserManager _AppUserManager = null;
        private ApplicationRoleManager _AppRoleManager = null;

        protected ApplicationUserManager AppUserManager
        {
            get
            {
                return _AppUserManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        protected ApplicationRoleManager AppRoleManager
        {
            get
            {
                return _AppRoleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
        /// <summary>
        /// Children should put a valid name.
        /// </summary>
        protected string ControllerName = string.Empty;

        protected ModelFactory TheModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(this.Request, this.AppUserManager);
                }
                return _modelFactory;
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        

    }
}
