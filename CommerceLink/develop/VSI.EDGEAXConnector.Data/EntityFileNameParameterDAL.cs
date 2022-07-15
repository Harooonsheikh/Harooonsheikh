using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.Data
{
    public class EntityFileNameParameterDAL
    {
        public EntityFileNameParameterDAL()
        {
        }
        public  List<EntityFileNameParameter> GetEntityFileNameAndStore(int storeId)
        {
            List<EntityFileNameParameter> entityFileName = new List<EntityFileNameParameter>();
           
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                try
                {
                    entityFileName = db.EntityFileNameParameter.Where(p => p.StoreId == storeId).ToList();
                }
                catch (Exception ex)
                {

                    CustomLogger.LogException(ex, 0, string.Empty);
                }
                return entityFileName;
            }
        }

        public EntityFileNameParameter GetEntityFileNameAndStore(Entities Entity, int storeId)
        {
            short entityId = (short)Entity;
            EntityFileNameParameter entityFileName = new EntityFileNameParameter();
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                try
                {
                    entityFileName = db.EntityFileNameParameter.Where(p => p.EntityId == entityId && p.StoreId == storeId).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, 0, string.Empty);
                }
                return entityFileName;
            }
        }

        public bool UpdateFileNameParameter(Entities Entity)
        {
            short entityId = (short)Entity;
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                try
                {
                    var appsObj = db.EntityFileNameParameter.FirstOrDefault(p => p.EntityId == entityId);
                    appsObj.Parameters = appsObj.Parameters + 1;
                    appsObj.Postfix = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                    db.Entry(appsObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    CustomLogger.LogException(ex, 0, string.Empty);
                }
            }
            return true;
        }
    }
}
