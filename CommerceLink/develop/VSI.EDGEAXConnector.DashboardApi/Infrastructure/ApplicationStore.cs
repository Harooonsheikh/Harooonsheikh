using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.Infrastructure
{
    public class ApplicationStore
    {
        public int StoreId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string MongoConnection { get; set; }
        public string MongoDbName { get; set; }
        public string APIURL { get; set; }
        public string APIKey { get; set; }
        public string DBName { get; set; }
        public string DBServer { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }

        public bool Enabled { get; set; }
        public virtual ICollection<StoreAction> Actions { get; set; }
        public virtual ICollection<ApplicationUser> ApplicationUsers { get; set; }
    }
}