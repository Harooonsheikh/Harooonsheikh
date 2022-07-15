using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Data.DAL
{
    public class EntityDAL :BaseClass
    {
        public EntityDAL(string storeKey) : base(storeKey)
        {

        }
        public EntityDAL(string conn, string storeKey, string user) : base(conn, storeKey, user)
        {

        }

        public Entity Entity(int id)
        {
            try
            {
                Entity ent = new Data.Entity();
                using (IntegrationDBEntities db = this.GetConnection())
                {
                    ent = db.Entity.Where(m => m.EntityId == id).First();
                }
                return ent;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw new CommerceLinkError("Unable to read entity", ex);
            }
        }

        public List<KeyValuePair<int, string>> GetEntityList()
        {
            List<KeyValuePair<int, string>> listEntiitiesViewModel = null;

            try
            {
                listEntiitiesViewModel = new List<KeyValuePair<int, string>>();

                foreach (Entities entity in Enum.GetValues(typeof(Entities)))
                {
                    listEntiitiesViewModel.Add(new KeyValuePair<int, string>((int)entity, entity.ToString()));
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
            }

            return listEntiitiesViewModel;
        }

        public List<KeyValuePair<int, string>> GetMerchandizingEntities()
        {
            List<Entity> entityList = null;
            List<KeyValuePair<int, string>> listEntiitiesViewModel = null;

            try
            {
                listEntiitiesViewModel = new List<KeyValuePair<int, string>>();

                using (IntegrationDBEntities db = this.GetConnection())
                {
                    entityList = db.Entity.Where(p => p.WorkFlowId ==1).ToList();
                }

                for (int i=0; i< entityList.Count; i++)
                {
                    listEntiitiesViewModel.Add(new KeyValuePair<int, string>(entityList[i].EntityId, entityList[i].Name));
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
            }

            return listEntiitiesViewModel;
        }

        public List<KeyValuePair<int, string>> GetSaleOrderEntitities()
        {
            List<Entity> entityList = null;
            List<KeyValuePair<int, string>> listEntiitiesViewModel = null;

            try
            {
                listEntiitiesViewModel = new List<KeyValuePair<int, string>>();

                using (IntegrationDBEntities db = this.GetConnection())
                {
                    entityList = db.Entity.Where(p => p.WorkFlowId == 2).ToList();
                }

                for (int i = 0; i < entityList.Count; i++)
                {
                    listEntiitiesViewModel.Add(new KeyValuePair<int, string>(entityList[i].EntityId, entityList[i].Name));
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
            }

            return listEntiitiesViewModel;
        }

        public List<Entity> GetEntities()
        {
            using (var db = this.GetConnection())
            {
                return db.Entity.ToList();
            }
        }
    }
}
