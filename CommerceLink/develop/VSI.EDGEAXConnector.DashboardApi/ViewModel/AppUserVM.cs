using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.ViewModel
{
    public class AppUserVM
    {
        public AppUserVM()
        {
            this.UserRoles = new List<KeyValuePair<string, string>>();
            this.AppStores = new List<KeyValuePair<Nullable<int>, string>>();
        }

        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }
        public List<KeyValuePair<string, string>> UserRoles { get; set; }
        public List<KeyValuePair<Nullable<int>, string>> AppStores { get; set; }

    }
}