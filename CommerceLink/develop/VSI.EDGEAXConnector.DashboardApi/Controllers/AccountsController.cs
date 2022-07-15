using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.DashboardApi.Infrastructure;
using VSI.EDGEAXConnector.DashboardApi.Models;
using VSI.EDGEAXConnector.DashboardApi.Common;
using Microsoft.AspNet.Identity.EntityFramework;

namespace VSI.EDGEAXConnector.DashboardApi
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiBaseController
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_erpAdapterFactory"></param>
        public AccountsController()
        {
            ControllerName = "AccountsController";
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetUsers()
        {
            try
            {
                return Ok(this.AppUserManager.Users.ToList().Select(u => this.TheModelFactory.Create(u)));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("GetUserById")]
        [Obsolete("GetUserById is deprecated, please use GetUserById with POST parameter instead.")]
        public async Task<IHttpActionResult> GetUser(string Id)
        {
            try
            {
                var user = await this.AppUserManager.FindByIdAsync(Id);
                if (user != null)
                {
                    return Ok(this.TheModelFactory.Create(user));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [Route("GetUserById")]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public async Task<IHttpActionResult> GetUser([FromBody] GetUserRequest userRequest)
        {
            try
            {
                var user = await this.AppUserManager.FindByIdAsync(userRequest.Id);
                if (user != null)
                {
                    return Ok(this.TheModelFactory.Create(user));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpGet]
        [AllowAnonymous]
        [Obsolete("GetUserByUserId is deprecated, please use GetUserByUserId with POST parameter instead.")]
        public async Task<IHttpActionResult> GetUserByUserId(string Id)
        {
            try
            {
                var user = await this.AppUserManager.FindByIdAsync(Id);
                if (user != null)
                {
                    return Ok(user);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetUserByUserId([FromBody] GetUserRequest userRequest)
        {
            try
            {
                var user = await this.AppUserManager.FindByIdAsync(userRequest.Id);
                if (user != null)
                {
                    return Ok(user);
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
        [Obsolete("GetUserByToken is deprecated, please use GetUserByToken with POST parameter instead.")]
        public async Task<IHttpActionResult> GetUserByToken(string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    return BadRequest();
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
                if (jwtToken == null)
                    return NotFound();
                return Ok(jwtToken.Claims);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public async Task<IHttpActionResult> GetUserByToken(GetUserByTokenRequest userByTokenRequest)
        {
            try
            {
                if (string.IsNullOrEmpty(userByTokenRequest.Token))
                {
                    return BadRequest();
                }
                var tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = tokenHandler.ReadToken(userByTokenRequest.Token) as JwtSecurityToken;
                if (jwtToken == null)
                    return NotFound();
                return Ok(jwtToken.Claims);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }



        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model");
                }
                IdentityResult result = await this.AppUserManager.ChangePasswordAsync(model.UserId, model.OldPassword, model.NewPassword);
                if (!result.Succeeded)
                {
                    return BadRequest(JsonConvert.SerializeObject(result.Errors));
                }
                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("GetUserByName is deprecated, please use GetUserByName with POST parameter instead.")]
        public async Task<IHttpActionResult> GetUserByName(string username)
        {
            try
            {
                var user = await this.AppUserManager.FindByNameAsync(username);
                if (user != null)
                {
                    return Ok(this.TheModelFactory.Create(user));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }


        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public async Task<IHttpActionResult> GetUserByName([FromBody] GetUserByNameRequest userByNameRequest)
        {
            try
            {
                var user = await this.AppUserManager.FindByNameAsync(userByNameRequest.UserName);
                if (user != null)
                {
                    return Ok(this.TheModelFactory.Create(user));
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> CreateUser(CreateUserBindingModel createUserModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid model");
                }
                var user = new ApplicationUser()
                {
                    UserName = createUserModel.Username,
                    Email = createUserModel.Email,
                    FirstName = createUserModel.FirstName,
                    LastName = createUserModel.LastName
                };
                IdentityResult addUserResult = await this.AppUserManager.CreateAsync(user, createUserModel.Password);
                if (!addUserResult.Succeeded)
                {
                    return BadRequest(JsonConvert.SerializeObject(addUserResult.Errors));
                }
                Uri locationHeader = new Uri(Url.Link("GetUserById", new { id = user.Id }));
                string code = await this.AppUserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                //code = System.Web.HttpUtility.UrlEncode(code);
                byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(code);
                code = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                //var callbackUrl = new Uri(Url.Link("ConfirmEmailRoute", new { userId = user.Id, code = code }));
                var callbackUrl = new Uri(ConfigurationManager.AppSettings["url"] + "login?userId=" + user.Id + "&code=" + code);
                await this.AppUserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                //List<UserStore> list = new List<UserStore>();
                //for(var i=0; i< createUserModel.StoreIds.Length; i++)
                //{
                //    UserStore userStore = new UserStore();
                //    userStore.UserId_FK = user.Id;
                //    userStore.StoreId_FK = createUserModel.StoreIds[i];
                //    userStore.OrganizationId_FK = createUserModel.OrganizationId;
                //    userStore.Created = DateTime.UtcNow;
                //    list.Add(userStore);
                //}
                //if(list.Count > 0)
                //    StoreDAL.AddUserStores(list,RequestSharedParams.StoreConnectionString);

                return Created(locationHeader, TheModelFactory.Create(user));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public async Task<IHttpActionResult> AssignRolesToUser([FromUri] string id, [FromBody] string[] rolesToAssign)
        {
            try
            {
                var appUser = await this.AppUserManager.FindByIdAsync(id);

                if (appUser == null)
                {
                    return NotFound();
                }
                var currentRoles = await this.AppUserManager.GetRolesAsync(appUser.Id);

                var rolesNotExists = rolesToAssign.Except(this.AppRoleManager.Roles.Select(x => x.Name)).ToArray();

                if (rolesNotExists.Count() > 0)
                {
                    return BadRequest(string.Format("Roles '{0}' does not exixts in the system", string.Join(",", rolesNotExists)));
                }
                IdentityResult removeResult = await this.AppUserManager.RemoveFromRolesAsync(appUser.Id, currentRoles.ToArray());

                if (!removeResult.Succeeded)
                {
                    return BadRequest("Failed to remove user roles");
                }

                IdentityResult addResult = await this.AppUserManager.AddToRolesAsync(appUser.Id, rolesToAssign);
                if (!addResult.Succeeded)
                {
                    return BadRequest(JsonConvert.SerializeObject(addResult.Errors));
                }

                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Obsolete("ConfirmEmail is deprecated, please use ConfirmEmail with POST parameter instead.")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
                {
                    return BadRequest("User Id and Code are required");
                }
                var codeDecodedBytes = WebEncoders.Base64UrlDecode(code);
                code = Encoding.UTF8.GetString(codeDecodedBytes);
                IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(userId, code);

                if (result.Succeeded)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest(JsonConvert.SerializeObject(result.Errors));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ConfirmEmail([FromBody]  ConfirmEmailRequest confirmEmail)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(confirmEmail.UserId) || string.IsNullOrWhiteSpace(confirmEmail.Code))
                {
                    return BadRequest("User Id and Code are required");
                }
                var codeDecodedBytes = WebEncoders.Base64UrlDecode(confirmEmail.Code);
                confirmEmail.Code = Encoding.UTF8.GetString(codeDecodedBytes);
                IdentityResult result = await this.AppUserManager.ConfirmEmailAsync(confirmEmail.UserId, confirmEmail.Code);

                if (result.Succeeded)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest(JsonConvert.SerializeObject(result.Errors));
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("IsUserConfirmed", Name = "IsUserConfirmed")]
        [Obsolete("IsUserConfirmed is deprecated, please use IsUserConfirmed with POST parameter instead.")]
        public async Task<IHttpActionResult> IsUserConfirmed(string userId = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest("User Id is required");
                }
                if ((await this.AppUserManager.IsEmailConfirmedAsync(userId)))
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest("Email is not confirmed");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
    
        public async Task<IHttpActionResult> IsUserConfirmed([FromBody] IsUserConfirmedRequest isUserConfirmed)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(isUserConfirmed.UserId))
                {
                    return BadRequest("User Id is required");
                }
                if ((await this.AppUserManager.IsEmailConfirmedAsync(isUserConfirmed.UserId)))
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest("Email is not confirmed");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Obsolete("ResetPassword is deprecated, please use ResetPassword with POST parameter instead.")]
        public async Task<IHttpActionResult> ResetPassword(string email = "", string code = "", string newPassword = "")
        {
            try
            {

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(code) || string.IsNullOrWhiteSpace(newPassword))
                {
                    return BadRequest("User Email, Code and Password are required");
                }
                var user = await this.AppUserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var codeDecodedBytes = WebEncoders.Base64UrlDecode(code);
                    code = Encoding.UTF8.GetString(codeDecodedBytes);
                    var result = await this.AppUserManager.ResetPasswordAsync(user.Id, code, newPassword);
                    if (result.Succeeded)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return BadRequest(JsonConvert.SerializeObject(result.Errors));
                    }
                }
                else
                {
                    return BadRequest("Invalid Email Entered");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ResetPassword([FromBody]  ResetPasswordRequest resetPassword)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(resetPassword.Email) || string.IsNullOrWhiteSpace(resetPassword.Code) || string.IsNullOrWhiteSpace(resetPassword.NewPassword))
                {
                    return BadRequest("User Email, Code and Password are required");
                }
                var user = await this.AppUserManager.FindByEmailAsync(resetPassword.Email);
                if (user != null)
                {
                    var codeDecodedBytes = WebEncoders.Base64UrlDecode(resetPassword.Code);
                    resetPassword.Code = Encoding.UTF8.GetString(codeDecodedBytes);
                    var result = await this.AppUserManager.ResetPasswordAsync(user.Id, resetPassword.Code, resetPassword.NewPassword);
                    if (result.Succeeded)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return BadRequest(JsonConvert.SerializeObject(result.Errors));
                    }
                }
                else
                {
                    return BadRequest("Invalid Email Entered");
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        [Obsolete("ForgotPassword is deprecated, please use ForgotPassword with POST parameter instead.")]
        public async Task<IHttpActionResult> ForgotPassword(string email = "")
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    return BadRequest("Email is required");
                }
                var user = await this.AppUserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var code = await this.AppUserManager.GeneratePasswordResetTokenAsync(user.Id);
                    //code = System.Web.HttpUtility.UrlEncode(code);
                    byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(code);
                    code = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                    var callbackUrl = new Uri(ConfigurationManager.AppSettings["url"] + "login?userId=" + user.Id + "&code=" + code);
                    await this.AppUserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    return Ok(true);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ForgotPassword([FromBody] ForgotPasswordRequest forgotPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(forgotPassword.Email))
                {
                    return BadRequest("Email is required");
                }
                var user = await this.AppUserManager.FindByEmailAsync(forgotPassword.Email);
                if (user != null)
                {
                    var code = await this.AppUserManager.GeneratePasswordResetTokenAsync(user.Id);
                    //code = System.Web.HttpUtility.UrlEncode(code);
                    byte[] tokenGeneratedBytes = Encoding.UTF8.GetBytes(code);
                    code = WebEncoders.Base64UrlEncode(tokenGeneratedBytes);
                    var callbackUrl = new Uri(ConfigurationManager.AppSettings["url"] + "login?userId=" + user.Id + "&code=" + code);
                    await this.AppUserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                    return Ok(true);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("DeleteUser is deprecated, please use DeleteUser with POST parameter instead.")]
        public async Task<IHttpActionResult> DeleteUser(string id)
        {
            try
            {
                var appUser = await this.AppUserManager.FindByIdAsync(id);
                if (appUser != null)
                {
                    IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);
                    if (!result.Succeeded)
                    {
                        return BadRequest(JsonConvert.SerializeObject(result.Errors));
                    }
                    return Ok(true);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public async Task<IHttpActionResult> DeleteUser([FromBody] DeleteUserRequest deleteUser)
        {
            try
            {
                var appUser = await this.AppUserManager.FindByIdAsync(deleteUser.Id);
                if (appUser != null)
                {
                    IdentityResult result = await this.AppUserManager.DeleteAsync(appUser);
                    if (!result.Succeeded)
                    {
                        return BadRequest(JsonConvert.SerializeObject(result.Errors));
                    }
                    return Ok(true);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        #region Accounts Request, Response classes

        public class GetUserRequest {
            public string Id { get; set; }

        }

        public class GetUserByTokenRequest
        {
            public  string Token { get; set; }

        }
        public class GetUserByNameRequest
        {
            public string UserName { get; set; }

        }
        public class ConfirmEmailRequest
        {
            public string UserId { get; set; } = "";
            public string Code { get; set; } = "";

        }
        public class IsUserConfirmedRequest
        {
            public string UserId { get; set; }

        }
        public class ResetPasswordRequest
        {
            public string Email { get; set; } = "";
            public string Code { get; set; } = "";

            public string NewPassword { get; set; } = "";

        }

        public class ForgotPasswordRequest
        {
            public string Email { get; set; } = "";

        }

        public class DeleteUserRequest
        {
            public string Id { get; set; }

        }

        #endregion

    }
}
