using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.UI.Managers
{
    public class LogsManager
    {
        private static LogsDAL logDAL = new LogsDAL(StoreService.StoreLkey);

        public static Log GetLog(int Id)
        {
            Log log = new Log();
            log = logDAL.GetLog(Id);
            return log;
        }

        public static List<Log> GetAllLogs()
        {
            List<Log> lstLogs = new List<Log>();

            lstLogs = logDAL.GetAllLogs();

            return lstLogs;
        }

        public static List<Log> GetLogsByType(string type)
        {
            List<Log> lstLogs = new List<Log>();

            lstLogs = logDAL.GetLogsByType(type);

            return lstLogs;
        }

        public static List<Log> GetLogs(string type, DateTime fromDate, DateTime toDate)
        {
            List<Log> lstLogs = new List<Log>();

            lstLogs = logDAL.GetLogs(type, fromDate, toDate);

            return lstLogs;
        }

        public static bool DeleteLog(int Id)
        {
            return logDAL.DeleteLog(Id);
        }

        public static bool DeleteLogs(string type, DateTime fromDate, DateTime toDate)
        {
            return logDAL.DeleteLogs(type, fromDate, toDate); 
        }
    }
}
