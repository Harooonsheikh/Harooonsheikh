using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class LogsDAL : BaseClass
    {

        public LogsDAL(string storeKey) : base(storeKey)
        {

        }
        public LogsDAL(string connectionString,string storeKey, string user) : base(connectionString, storeKey, user)
        {

        }

        public Log GetLog(int Id)
        {
            Log log = new Log();
            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    log = db.Log.Single(x => x.LogId == Id && x.StoreId == this.StoreId);
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }
            return log;
        }

        public bool DeleteLog(int Id)
        {
            Log log = new Log();
            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    log = db.Log.Remove(db.Log.Single(x => x.LogId == Id && x.StoreId == StoreId));
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    return false;
                }
            }
        }

        public List<Log> GetAllLogs()
        {
            List<Log> lstLogs = new List<Log>();
            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    lstLogs = db.Log.Where(x=>x.StoreId==StoreId) .ToList();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstLogs;
        }

        public List<Log> GetLogsByType(string type)
        {
            List<Log> lstLogs = new List<Log>();

            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    lstLogs = db.Log.Where(l=>l.EventLevel == type && l.StoreId == StoreId).OrderByDescending(l=>l.LogId).ToList();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstLogs;
        }

        //public List<Log> GetLogs(string InstanceId)
        //{
        //    List<Log> lstLogs = new List<Log>();

        //    using (IntegrationDBEntities db = this.GetConnection())
        //    {
        //        try
        //        {
        //            lstLogs = db.Log.Where(l => l.InstanceId == InstanceId && l.StoreId == StoreId).OrderByDescending(l => l.LogId).ToList();
        //        }
        //        catch (Exception ex)
        //        {
        //            customLogger.LogException(ex);
        //        }
        //    }

        //    return lstLogs;
        //}
        public dynamic GetLogs(int daysCount)
        {
            DateTime desiredDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(daysCount));
            IQueryable<Log> records = null;
            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    if (daysCount == -1)
                    {
                        records = db.Log.Where(l => l.StoreId == StoreId).OrderByDescending(l => l.LogId);
                        
                    }
                    else
                    {
                        records = db.Log.Where(l => (l.CreatedOn > desiredDate) && l.StoreId == StoreId).OrderByDescending(l => l.LogId);
                    }
                    
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
                var logs = records.Select(c => new { c.LogId, c.EventLevel, c.ErrorSource, c.EventMessage, c.MachineName, c.CreatedOn, c.ErrorMessage, c.InnerErrorMessage, c.CreatedBy }).ToList();
                return logs;
            }
            
        }

        public List<Log> GetLogs(string type, DateTime fromDate, DateTime toDate)
        {
            List<Log> lstLogs = new List<Log>();

            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    if (type.ToLower().Equals("all"))
                    {
                        lstLogs = db.Log.Where(l => l.CreatedOn >= fromDate && l.CreatedOn <= toDate && l.StoreId == StoreId).OrderByDescending(l => l.LogId).ToList();
                    }
                    else
                    {
                        lstLogs = db.Log.Where(l => l.EventLevel == type && l.CreatedOn >= fromDate && l.CreatedOn <= toDate && l.StoreId == StoreId).OrderByDescending(l => l.LogId).ToList();
                    }
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstLogs;
        }

        public bool DeleteLogs(string type, DateTime fromDate, DateTime toDate)
        {
            List<Log> lstLogs = new List<Log>();

            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    if (type.ToLower().Equals("all"))
                    {
                        lstLogs = db.Log.Where(l => l.CreatedOn >= fromDate && l.CreatedOn <= toDate && l.StoreId == StoreId).AsEnumerable().ToList();
                    }
                    else
                    {
                        lstLogs = db.Log.Where(l => l.EventLevel == type && l.CreatedOn >= fromDate && l.CreatedOn <= toDate && l.StoreId == StoreId).AsEnumerable().ToList();
                    }


                    db.Log.RemoveRange(lstLogs);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    return false;
                }
            }
          
        }

        public List<Log> AllLogs(int storeId, int pageSize, int offSet)
        {
            List<Log> logs = null;
            try
            {
                using (var conn = this.GetConnection())
                {
                    if (storeId > 0)
                    {
                        logs = conn.Log.Where(m => (m.StoreId == storeId)).ToList();
                    }
                    else
                    {
                        logs = conn.Log.ToList();
                    }
                    logs = logs.OrderByDescending(m=>m.CreatedOn).Skip(pageSize * offSet).Take(pageSize).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return logs;
        }
    }
}
