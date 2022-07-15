using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.Infrastructure
{
    /// <summary>
    /// Represents a user who register in our membership system
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// First name of user
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of user
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }
        public Nullable<int> StoreId { get; set; }
        public virtual ApplicationStore Store { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here

            return userIdentity;
        }
    }
}