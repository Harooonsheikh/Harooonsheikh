using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using VSI.EDGEAXConnector.DashboardApi.Infrastructure;

namespace VSI.EDGEAXConnector.DashboardApi.Providers
{
    public class CustomOAuthProvider : OAuthAuthorizationServerProvider
    {

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            var allowedOrigin = "*";

            if (!context.OwinContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
            {
                context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
            }

            var userManager = context.OwinContext.GetUserManager<ApplicationUserManager>();

            var user = await userManager.FindByNameAsync(context.UserName);

            if (user == null)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect."); //user not found
                return;
            }

            await userManager.SetLockoutEnabledAsync(user.Id, true);

            if (await userManager.IsLockedOutAsync(user.Id))
            {
                var lockOpenTime = user.LockoutEndDateUtc;
                var timeSpan = lockOpenTime.Value - DateTime.UtcNow;

                context.SetError("locked_out", $"User locked out. Please try again in {Math.Ceiling(timeSpan.TotalMinutes)} minute[s].");
                return;
            }

            var check = await userManager.CheckPasswordAsync(user, context.Password);

            if (!check)
            {
                await userManager.AccessFailedAsync(user.Id);
                context.SetError("invalid_grant", "The user name or password is incorrect."); //wrong password
                return;
            }

            await userManager.ResetAccessFailedCountAsync(user.Id);

            ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(userManager, "JWT");

            var ticket = new AuthenticationTicket(oAuthIdentity, null);

            context.Validated(ticket);

        }
    }
}