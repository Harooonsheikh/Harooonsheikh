using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using VSI.EDGEAXConnector.Configuration;

namespace VSI.EDGEAXConnector.DashboardApi.Common
{
    public class MongoHelper
    {
        public static void SetMongoDBCreds(string storeKey, ref string MongoDBConn,ref string MongoDBName)
        {
            try
            {
                ConfigurationHelper configurationHelper = new ConfigurationHelper(storeKey);
                MongoDBConn = configurationHelper.GetSetting(APPLICATION.Mongo_Connection);
                MongoDBName = configurationHelper.GetSetting(APPLICATION.Mongo_DBName);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}