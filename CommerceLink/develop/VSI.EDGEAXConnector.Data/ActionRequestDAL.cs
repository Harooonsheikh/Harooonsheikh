using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class ActionRequestDAL : BaseClass
    {
        public ActionRequestDAL(string storeKey) : base(storeKey)
        {

        }
        public ActionRequestDAL(string conn, string storeKey, string user) : base(conn, storeKey, user)
        {

        }
        public ActionRequest Get(string actionName, int storeId)
        {
            ActionRequest request = null;
            try
            {
                using (var db = this.GetConnection())
                {
                    request = db.ActionRequest.Where(m => m.ActionName == actionName && m.StoreId == storeId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
            return request;
        }
        public bool Add(ActionRequest request)
        {
            try
            {
                using (var db = this.GetConnection())
                {
                    db.ActionRequest.Add(request);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }
        public bool Update(ActionRequest request)
        {
            try
            {
                using (var db = this.GetConnection())
                {
                    db.Entry(request).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }
    }
}
