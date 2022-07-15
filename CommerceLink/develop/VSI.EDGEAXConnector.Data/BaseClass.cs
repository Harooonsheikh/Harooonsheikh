using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Data
{
    public class BaseClass
    {
        public string StoreKey { get; set; }

        public int StoreId { get; set; }

        public string UserId { get; set; }
        public string StoreName { get; set; }
        public string ConnectionString { get; set; }

        public BaseClass(string storeKey)
        {
            if (storeKey != null)
            {
                this.InitializeStore(storeKey,null);
            }
        }
        public BaseClass()
        {

        }

        public BaseClass(string connectionString, string storeKey, string userId)
        {
            if (storeKey != null)
            {
                this.ConnectionString = connectionString;
                this.InitializeStore(storeKey, userId);
            }
        }

        protected void InitializeStore(string key, string userId)
        {
            var axSore = StoreService.GetStoreByKey(key);
            if (axSore == null)
                throw new CommerceLinkError("Store Not Found with key specified.");
            this.StoreId = axSore.StoreId;
            this.StoreKey = axSore.StoreKey;
            this.StoreName = axSore.Name;
            this.UserId = !string.IsNullOrEmpty(userId) ? userId : axSore.CreatedBy; // this will be set by loged in user 
        }
        public IntegrationDBEntities GetConnection()
        {
            return !string.IsNullOrEmpty(this.ConnectionString) ? new IntegrationDBEntities(this.ConnectionString) : new IntegrationDBEntities();
        }

    }
}
