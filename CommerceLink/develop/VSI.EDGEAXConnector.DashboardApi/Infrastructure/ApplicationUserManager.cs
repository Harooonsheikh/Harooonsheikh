using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;

namespace VSI.EDGEAXConnector.DashboardApi.Infrastructure
{
    /// <summary>
    /// Responsible for managing instances of the user class
    /// </summary>
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        /// <summary>
        /// Initialize manager
        /// </summary>
        /// <param name="store"></param>
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
           : base(store)
        {
        }

        /// <summary>
        /// Responsible to return an instance of the “ApplicationUserManager” 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        /// <returns>ApplicationUserManager</returns>
        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var appDbContext = context.Get<ApplicationDbContext>();
            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(appDbContext));
            appUserManager.UserValidator = new UserValidator<ApplicationUser>(appUserManager)
            {
                //AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };
            appUserManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = false,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            var strMaxFailedAccessAttemptsBeforeLockout = System.Configuration.ConfigurationManager.AppSettings["MaxFailedAccessAttemptsBeforeLockout"];
            int.TryParse(strMaxFailedAccessAttemptsBeforeLockout, out var maxFailedAccessAttemptsBeforeLockout);
            if (maxFailedAccessAttemptsBeforeLockout == 0)
            {
                maxFailedAccessAttemptsBeforeLockout = 3;
            }

            appUserManager.MaxFailedAccessAttemptsBeforeLockout = maxFailedAccessAttemptsBeforeLockout;

            var strLockoutTime = System.Configuration.ConfigurationManager.AppSettings["LockOutTimeInMinutes"];
            int.TryParse(strLockoutTime, out var lockoutTimeinMinutes);
            if (lockoutTimeinMinutes == 0)
            {
                lockoutTimeinMinutes = 10;
            }

            appUserManager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(lockoutTimeinMinutes);
            appUserManager.UserLockoutEnabledByDefault = true;

            //appUserManager.EmailService = new VSI.EDGEAXConnector.DashboardApi.Services.EmailService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                {
                    //Code for email confirmation and reset password life time
                    TokenLifespan = TimeSpan.FromHours(6)
                };
            }

            return appUserManager;
        }
    }
}