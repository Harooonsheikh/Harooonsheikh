using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class ApiLogsDAL : BaseClass
    {

        public ApiLogsDAL(string storeKey) : base(storeKey)
        {

        }
        public ApiLogsDAL(string connectionString, string storeKey, string user) : base(connectionString, storeKey, user)
        {

        }

        public List<Log> GetLogsByType(string type)
        {
            List<Log> lstLogs = new List<Log>();

            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    lstLogs = db.Log.Where(l => l.EventLevel == type && l.StoreId == StoreId).OrderByDescending(l => l.LogId).ToList();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstLogs;
        }

        public dynamic GetMethods()
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    var Methods =
                    from Method in db.RequestResponse
                    group Method by Method.MethodName into newGroup
                    orderby newGroup.Key
                    select newGroup.Key;
                    return Methods.ToList();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
                return null;
            }

        }
        public dynamic GetLogs(int daysCount)
        {
            DateTime desiredDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(daysCount));
            using (IntegrationDBEntities db = this.GetConnection())
            {

                var result = from rr in db.RequestResponse
                             join dd in db.DataDirection on rr.DataDirectionId equals dd.DataDirectionId
                             where rr.StoreId == StoreId
                             select new { rr.RequestResponseId, rr.MethodName, rr.ApplicationName, rr.Description, rr.DataPacket, rr.StoreId, rr.CreatedOn, dd.DataDirectionName };

                try
                {
                    if (daysCount == -1)
                    {
                        var logs = result.Select(c => new { c.RequestResponseId, c.MethodName, c.ApplicationName, c.Description, c.DataPacket, c.StoreId, c.CreatedOn, c.DataDirectionName }).ToList();
                        return logs;
                    }
                    else
                    {
                        var filterResult = result.Where(x => x.CreatedOn > desiredDate);
                        var logs = filterResult.Select(c => new { c.RequestResponseId, c.MethodName, c.ApplicationName, c.Description, c.DataPacket, c.StoreId, c.CreatedOn, c.DataDirectionName }).ToList();
                        return logs;
                    }

                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
                return null;
            }

        }

        /// <summary>
        /// Select request response log
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="directionType"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public dynamic GetFilterdLogs(string methodName, string directionType, DateTime fromDate, DateTime toDate, string searchQuery)
        {
            List<RequestResponse> lstLogs = new List<RequestResponse>();
            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    var result = from rr in db.RequestResponse
                                 join dd in db.DataDirection on rr.DataDirectionId equals dd.DataDirectionId
                                 where rr.StoreId == StoreId
                                 select new { rr.RequestResponseId, rr.MethodName, rr.ApplicationName, rr.Description, rr.DataPacket, rr.StoreId, rr.CreatedOn, dd.DataDirectionName };
                    var filterResult = result.Where(l => l.CreatedOn >= fromDate && l.CreatedOn <= toDate).OrderByDescending(l => l.RequestResponseId).ToList();
                    if (searchQuery == null && methodName == "-1" && directionType == "-1")
                    {
                        return filterResult.ToList();
                    }
                    else if (searchQuery != null && methodName == "-1" && directionType == "-1")
                    {
                        var searchQueryResult = filterResult.Where(x => x.DataPacket.ToLower().Contains(searchQuery.ToLower())).ToList();
                        return searchQueryResult;
                    }
                    else if (searchQuery == null && methodName != "-1" && directionType == "-1")
                    {
                        var methodBaseResult = filterResult.Where(x => x.MethodName == methodName).ToList();
                        return methodBaseResult;
                    }
                    else if (searchQuery == null && methodName != "-1" && directionType != "-1")
                    {
                        var methodDirectionBaseResult = filterResult.Where(x => x.MethodName == methodName && x.DataDirectionName == directionType).ToList();
                        return methodDirectionBaseResult;
                    }
                    else if (searchQuery != null && methodName != "-1" && directionType != "-1")
                    {
                        var methodDirectionBaseResult = filterResult.Where(x => x.MethodName == methodName && x.DataDirectionName == directionType).ToList();
                        var searchQueryResult = methodDirectionBaseResult.Where(x => x.DataPacket.ToLower().Contains(searchQuery.ToLower())).ToList();
                        return searchQueryResult;
                    }
                    else if (searchQuery == null && methodName == "-1" && directionType != "-1")
                    {
                        var methodDirectionBaseResult = filterResult.Where(x => x.DataDirectionName == directionType).ToList();
                        return methodDirectionBaseResult;
                    }
                    else if (searchQuery != null && methodName != "-1" && directionType == "-1")
                    {
                        var methodDirectionBaseResult = filterResult.Where(x => x.MethodName == methodName).ToList();
                        var searchQueryResult = methodDirectionBaseResult.Where(x => x.DataPacket.ToLower().Contains(searchQuery.ToLower())).ToList();
                        return methodDirectionBaseResult;
                    }
                    else if (searchQuery != null && methodName == "-1" && directionType != "-1")
                    {
                        var methodDirectionBaseResult = filterResult.Where(x => x.DataDirectionName == directionType).ToList();
                        var searchQueryResult = methodDirectionBaseResult.Where(x => x.DataPacket.ToLower().Contains(searchQuery.ToLower())).ToList();
                        return methodDirectionBaseResult;
                    }
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstLogs;
        }

        /// <summary>
        /// Select request response log from archive table
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="directionType"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="searchQuery"></param>
        /// <returns></returns>
        public dynamic GetFilterdArchiveLogs(string methodName, string directionType, DateTime fromDate, DateTime toDate, string searchQuery)
        {
            List<RequestResponse> lstLogs = new List<RequestResponse>();
            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    var result = from rr in db.Archive_RequestResponse
                                 join dd in db.DataDirection on rr.DataDirectionId equals dd.DataDirectionId
                                 where rr.StoreId == StoreId
                                 select new { rr.RequestResponseId, rr.MethodName, rr.ApplicationName, rr.Description, rr.DataPacket, rr.StoreId, rr.CreatedOn, dd.DataDirectionName };
                    var filterResult = result.Where(l => l.CreatedOn >= fromDate && l.CreatedOn <= toDate).OrderByDescending(l => l.RequestResponseId).ToList();
                    if (searchQuery == null && methodName == "-1" && directionType == "-1")
                    {
                        return filterResult.ToList();
                    }
                    else if (searchQuery != null && methodName == "-1" && directionType == "-1")
                    {
                        var searchQueryResult = filterResult.Where(x => x.DataPacket.ToLower().Contains(searchQuery.ToLower())).ToList();
                        return searchQueryResult;
                    }
                    else if (searchQuery == null && methodName != "-1" && directionType == "-1")
                    {
                        var methodBaseResult = filterResult.Where(x => x.MethodName == methodName).ToList();
                        return methodBaseResult;
                    }
                    else if (searchQuery == null && methodName != "-1" && directionType != "-1")
                    {
                        var methodDirectionBaseResult = filterResult.Where(x => x.MethodName == methodName && x.DataDirectionName == directionType).ToList();
                        return methodDirectionBaseResult;
                    }
                    else if (searchQuery != null && methodName != "-1" && directionType != "-1")
                    {
                        var methodDirectionBaseResult = filterResult.Where(x => x.MethodName == methodName && x.DataDirectionName == directionType).ToList();
                        var searchQueryResult = methodDirectionBaseResult.Where(x => x.DataPacket.ToLower().Contains(searchQuery.ToLower())).ToList();
                        return searchQueryResult;
                    }
                    else if (searchQuery == null && methodName == "-1" && directionType != "-1")
                    {
                        var methodDirectionBaseResult = filterResult.Where(x => x.DataDirectionName == directionType).ToList();
                        return methodDirectionBaseResult;
                    }
                    else if (searchQuery != null && methodName != "-1" && directionType == "-1")
                    {
                        var methodDirectionBaseResult = filterResult.Where(x => x.MethodName == methodName).ToList();
                        var searchQueryResult = methodDirectionBaseResult.Where(x => x.DataPacket.ToLower().Contains(searchQuery.ToLower())).ToList();
                        return methodDirectionBaseResult;
                    }
                    else if (searchQuery != null && methodName == "-1" && directionType != "-1")
                    {
                        var methodDirectionBaseResult = filterResult.Where(x => x.DataDirectionName == directionType).ToList();
                        var searchQueryResult = methodDirectionBaseResult.Where(x => x.DataPacket.ToLower().Contains(searchQuery.ToLower())).ToList();
                        return methodDirectionBaseResult;
                    }
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstLogs;
        }
    }
}
