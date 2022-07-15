using System;
using System.Linq;

namespace VSI.EDGEAXConnector.Data
{

    public static class ServiceBusRequestDAL
    {

        /// <summary>
        /// Select Service Bus Request using EcomTransactionId
        /// </summary>
        /// <param name="ecomTransactionId"></param>
        /// <returns></returns>
        public static ServiceBusRequestLog Select(Guid ecomTransactionId, int storeId)
        {
            using (var db = new IntegrationDBEntities())
            {
                return db.ServiceBusRequestLog.FirstOrDefault(e => e.StoreId == storeId &&
                    e.EcomTransactionId == ecomTransactionId);
            }
        }

        /// <summary>
        /// Select Service Bus Request using EcomTransactionId and MethodName
        /// </summary>
        /// <param name="ecomTransactionId"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        public static ServiceBusRequestLog Select(Guid ecomTransactionId, int storeId, string methodName)
        {
            using (var db = new IntegrationDBEntities())
            {
                return db.ServiceBusRequestLog.FirstOrDefault(e => e.StoreId == storeId &&
                    e.EcomTransactionId == ecomTransactionId &&
                    e.MethodName == methodName);
            }
        }

        /// <summary>
        /// Insert data in Service Bus Request
        /// </summary>
        /// <param name="ecomTransactionId"></param>
        /// <param name="status"></param>
        /// <param name="methodName"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        public static bool Insert(Guid ecomTransactionId, int storeId, bool status, string methodName)
        {
            try
            {
                ServiceBusRequestLog serviceBusRequestLog = new ServiceBusRequestLog();
                serviceBusRequestLog.StoreId = storeId;
                serviceBusRequestLog.EcomTransactionId = ecomTransactionId;
                serviceBusRequestLog.Status = status;
                serviceBusRequestLog.MethodName = methodName;
                serviceBusRequestLog.CreatedBy = "System";
                serviceBusRequestLog.CreatedOn = DateTime.UtcNow;

                using (var db = new IntegrationDBEntities())
                {
                    db.ServiceBusRequestLog.Add(serviceBusRequestLog);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Update date in Service Bus Request
        /// </summary>
        /// <param name="ecomTransactionId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static bool Update(Guid ecomTransactionId, int storeId, bool status)
        {
            try
            {
                using (var db = new IntegrationDBEntities())
                {
                    ServiceBusRequestLog serviceBusRequestLog =
                        db.ServiceBusRequestLog.FirstOrDefault(x => x.StoreId == storeId &&
                            x.EcomTransactionId == ecomTransactionId);
                    serviceBusRequestLog.Status = status;
                    serviceBusRequestLog.ModifiedBy = "System";
                    serviceBusRequestLog.ModifiedOn = DateTime.UtcNow;

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static bool InsertOrUpdate(Guid ecomTransactionId, int storeId, bool status, string methodName)
        {
            try
            {
                ServiceBusRequestLog serviceBusRequestLog;
                using (var db = new IntegrationDBEntities())
                {
                    serviceBusRequestLog = db.ServiceBusRequestLog.FirstOrDefault(e => e.StoreId == storeId &&
                        e.EcomTransactionId == ecomTransactionId &&
                        e.MethodName == methodName);

                    if (serviceBusRequestLog == null)
                    {
                        serviceBusRequestLog = new ServiceBusRequestLog();
                        serviceBusRequestLog.StoreId = storeId;
                        serviceBusRequestLog.EcomTransactionId = ecomTransactionId;
                        serviceBusRequestLog.Status = status;
                        serviceBusRequestLog.MethodName = methodName;
                        serviceBusRequestLog.CreatedBy = "System";
                        serviceBusRequestLog.CreatedOn = DateTime.UtcNow;

                        db.ServiceBusRequestLog.Add(serviceBusRequestLog);
                    }
                    else
                    {
                        serviceBusRequestLog.Status = status;
                        serviceBusRequestLog.ModifiedBy = "System";
                        serviceBusRequestLog.ModifiedOn = DateTime.UtcNow;
                    }

                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}