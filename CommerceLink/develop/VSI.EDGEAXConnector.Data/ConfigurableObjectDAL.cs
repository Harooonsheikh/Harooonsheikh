using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class ConfigurableObjectDAL : BaseClass
    {
        public ConfigurableObjectDAL(string storeKey) : base(storeKey)
        {

        }
        public ConfigurableObjectDAL(string conn, string storeKey, string user) : base(conn, storeKey, user)
        {

        }

        public List<ConfigurableObject> GetAllConfigurableObjects()
        {
            List<ConfigurableObject> lstConfigObjects = new List<ConfigurableObject>();

            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    lstConfigObjects = db.ConfigurableObject.Where(x=>x.StoreId==StoreId).ToList();
                }
                catch(Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstConfigObjects;
        }

        public ConfigurableObject UpdateConfigurableObject(ConfigurableObject conObj)
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    var con = db.ConfigurableObject.FirstOrDefault(o => o.ConfigurableObjectId == conObj.ConfigurableObjectId && conObj.StoreId==StoreId);
                    con.ComValue = conObj.ComValue;
                    con.ErpValue = conObj.ErpValue;
                    con.EntityType = conObj.EntityType;
                    con.ConnectorKey = conObj.ConnectorKey;
                    con.ModifiedOn = DateTime.UtcNow;
                    con.ModifiedBy = UserId;
                    db.Entry(con).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return con;
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    return null;
                }
            }
        }

        public static List<ConfigurableObject> GetConfirableObjects()
        {
            List<ConfigurableObject> configurableObject = new List<ConfigurableObject>();
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                try
                {
                    configurableObject = db.ConfigurableObject.ToList();
                }
                catch (Exception)
                {
                    return new List<ConfigurableObject>();
                }
            }
            return configurableObject;
        }
    }
}
