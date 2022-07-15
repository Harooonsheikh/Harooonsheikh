using System;
using System.Collections.Generic;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.DashboardApi.Infrastructure;
using VSI.EDGEAXConnector.DashboardApi.ViewModel;
using System.Linq;
using VSI.EDGEAXConnector.Logging;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using Microsoft.AspNet.Identity.Owin;
using System.Configuration;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    public class UserManagerController : ApiBaseController
    {
        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get()
        {
            try
            {
                AppUserVM appUser = new AppUserVM();
                using (var db = new ApplicationDbContext())
                {
                    var stores = db.Stores.Where(s => s.Enabled == true).ToList();

                    if (stores.Count != 0)
                    {
                        var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

                        foreach (var store in stores)
                        {
                            KeyValuePair<Nullable<int>, string> tempStore = new KeyValuePair<Nullable<int>, string>(store.StoreId, store.Name);
                            appUser.AppStores.Add(tempStore);
                        }

                        foreach (var role in roleManager.Roles)
                        {
                            KeyValuePair<string, string> tempRole = new KeyValuePair<string, string>(role.Id, role.Name);
                            appUser.UserRoles.Add(tempRole);
                        }
                    }
                    else
                    {
                        return BadRequest("StoreNotFound");
                    }
                }

                return Ok(appUser);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Create(AppUserVM user)
        {
            try
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                var appUser = manager.FindByName(user.UserName.Trim());
                var tempUser = manager.FindByEmail(user.Email.Trim());
                bool isEmailRegistered = false;
                bool isUserNameTaken = false;
                string result = "";

                if (tempUser != null)
                {
                    if (tempUser.StoreId == user.AppStores[0].Key)
                    {
                        isEmailRegistered = true;
                        result = "EmailRegistered";
                    }

                }

                if (appUser != null)
                {
                    isUserNameTaken = true;
                    result = result + "UserNameTaken";
                }
                if (!(isUserNameTaken || isEmailRegistered))
                {
                    appUser = new ApplicationUser()
                    {
                        UserName = user.UserName.Trim(),
                        Email = user.Email.Trim(),
                        EmailConfirmed = user.EmailConfirmed,
                        FirstName = user.FirstName.Trim(),
                        LastName = user.LastName.Trim(),
                        StoreId = user.AppStores[0].Key

                    };
                    manager.Create(appUser, user.Password);
                    manager.AddToRoles(appUser.Id, new string[] { user.UserRoles[0].Value });
                    return Ok("Success");
                }

                return BadRequest(result);

            }
            catch (Exception ex)
            {
                CustomLogger logger = new CustomLogger(new LoggerContext { UserId = this.User, StoreId = this.StoreId });
                logger.LogException(ex);
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("GetIntegrationKeys is deprecated, please use GetIntegrationKeys with POST parameter instead.")]
        public IHttpActionResult VerifyUserName(string userName)
        {
            try
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                var appUser = manager.FindByName(userName.Trim());
                if (appUser == null)
                {
                    return Ok(false);
                }
                else
                {
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult VerifyUserName([FromBody] GetVerifyUserNameRequest VerifyUserNameRequest)
        {
            try
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                var appUser = manager.FindByName(VerifyUserNameRequest.UserName.Trim());
                if (appUser == null)
                {
                    return Ok(false);
                }
                else
                {
                    return Ok(true);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("Role is deprecated, please use Role with POST parameter instead.")]
        public IHttpActionResult Role(string userId)
        {
            try
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var appUser = manager.FindById(userId);

                if (appUser != null)
                {
                    var userRole = manager.GetRoles(appUser.Id);
                    return Ok(userRole);
                }
                else
                {
                    return BadRequest("UserNotFound");
                }
            }
            catch (Exception ex)
            {
                CustomLogger logger = new CustomLogger(new LoggerContext { UserId = this.User, StoreId = this.StoreId });
                logger.LogException(ex);
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Role([FromBody] GetRoleRequest RolesRequest)
        {
            try
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var appUser = manager.FindById(RolesRequest.UserId);

                if (appUser != null)
                {
                    var userRole = manager.GetRoles(appUser.Id);
                    return Ok(userRole);
                }
                else
                {
                    return BadRequest("UserNotFound");
                }
            }
            catch (Exception ex)
            {
                CustomLogger logger = new CustomLogger(new LoggerContext { UserId = this.User, StoreId = this.StoreId });
                logger.LogException(ex);
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("Store is deprecated, please use Store with POST parameter instead.")]
        public IHttpActionResult Store(string userId)
        {
            try
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                ApplicationUser appUser = manager.FindById(userId);

                KeyValuePair<Nullable<int>, string> userStore = new KeyValuePair<Nullable<int>, string>();

                if (appUser != null)
                {
                    var userRole = manager.GetRoles(appUser.Id);
                    if (userRole[0].Equals("SuperAdmin"))
                    {
                        StoreDAL storeMgr = new StoreDAL(this.DbConnStr);
                        var store = storeMgr.GetActiveStores().FirstOrDefault();
                        var uStore = new KeyValuePair<Nullable<int>, string>(store.StoreId, store.Name);
                        userStore = uStore;
                    }
                    else
                    {
                        using (var db = new ApplicationDbContext())
                        {
                            var store = db.Stores.Where(s => s.StoreId == appUser.StoreId).FirstOrDefault();
                            if (store.Enabled)
                            {
                                var uStore = new KeyValuePair<Nullable<int>, string>(appUser.StoreId, store.Name);
                                userStore = uStore;
                            }
                            else
                            {
                                return BadRequest("StoreDisabled");
                            }

                        }
                    }
                    return Ok(userStore);
                }
                else
                {
                    return BadRequest("UserNotFound");
                }
            }
            catch (Exception ex)
            {
                CustomLogger logger = new CustomLogger(new LoggerContext { UserId = this.User, StoreId = this.StoreId });
                logger.LogException(ex);
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Store([FromBody] GetStoreRequest StoreRequest)
        {
            try
            {
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                ApplicationUser appUser = manager.FindById(StoreRequest.UserId);

                KeyValuePair<Nullable<int>, string> userStore = new KeyValuePair<Nullable<int>, string>();

                if (appUser != null)
                {
                    var userRole = manager.GetRoles(appUser.Id);
                    if (userRole[0].Equals("SuperAdmin"))
                    {
                        StoreDAL storeMgr = new StoreDAL(this.DbConnStr);
                        var store = storeMgr.GetActiveStores().FirstOrDefault();
                        var uStore = new KeyValuePair<Nullable<int>, string>(store.StoreId, store.Name);
                        userStore = uStore;
                    }
                    else
                    {
                        using (var db = new ApplicationDbContext())
                        {
                            var store = db.Stores.Where(s => s.StoreId == appUser.StoreId).FirstOrDefault();
                            if (store.Enabled)
                            {
                                var uStore = new KeyValuePair<Nullable<int>, string>(appUser.StoreId, store.Name);
                                userStore = uStore;
                            }
                            else
                            {
                                return BadRequest("StoreDisabled");
                            }

                        }
                    }
                    return Ok(userStore);
                }
                else
                {
                    return BadRequest("UserNotFound");
                }
            }
            catch (Exception ex)
            {
                CustomLogger logger = new CustomLogger(new LoggerContext { UserId = this.User, StoreId = this.StoreId });
                logger.LogException(ex);
                return InternalServerError(ex);
            }
        }

        #region User Manager Controller, Response classes

        public class GetVerifyUserNameRequest
        {
            public string UserName { get; set; }
        }
        public class GetRoleRequest
        {
            public string UserId { get; set; }
        }
        public class GetStoreRequest
        {
            public string UserId { get; set; }
        }
        #endregion

    }
}

