using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class DataDeleteDAL : BaseClass
    {
        public DataDeleteDAL(string storeKey) : base(storeKey)
        {

        }
        private IntegrationDBEntities _context;

        public DataDelete GetById(string RequestId)
        {
            using (_context = new IntegrationDBEntities())
            {
                return _context.DataDelete.FirstOrDefault(d => d.RequestId == RequestId);
            }
        }

        public List<DataDelete> GetByStatus(DataDeleteStatus status)
        {
            using (_context = new IntegrationDBEntities())
            {
                return _context.DataDelete.Where(d => d.Status == (int)status).ToList();
            }
        }

        public void Add(DataDelete entity)
        {
            try
            {
                using (_context = new IntegrationDBEntities())
                {
                    entity.Status = (int)DataDeleteStatus.Created;
                    entity.CreatedBy = UserId ?? "System";
                    entity.CreatedOn = DateTime.UtcNow;

                    _context.DataDelete.Add(entity);

                    _context.SaveChanges();
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
                throw;
            }
        }

        public bool UpdateStatus(long dataDeleteId, DataDeleteStatus status)
        {
            using (_context = new IntegrationDBEntities())
            {
                var entity = _context.DataDelete.FirstOrDefault(d => d.DataDeleteId == dataDeleteId);
                if (entity == null)
                {
                    return false;
                }

                entity.Status = (short)status;
                entity.ModifiedBy = UserId ?? "System";
                entity.ModifiedOn = DateTime.UtcNow;
                _context.SaveChanges();

                return true;
            }
        }
        public void Delete(List<DataDelete> _DataDelete, string strMethodNames = "")
        {
            try
            {
                //Splitting Methods Names And Converting into String Array
                var MethodNames = strMethodNames.Split(','); 

                using (_context = new IntegrationDBEntities())
                {
                    //Setting Timeout For 10800 Seconds (3 Hours)
                    ((IObjectContextAdapter)_context).ObjectContext.CommandTimeout = 10800;

                    foreach (var dataToBeDelete in _DataDelete)
                    {
                        try
                        {
                            IQueryable<RequestResponse> query_requestResponse = null;
                            IQueryable<Archive_RequestResponse> query_archiveResponse = null;

                            //if MethodNames are not Provided Set Condition Without MethodNames
                            if (String.IsNullOrEmpty(strMethodNames))
                            {
                                query_requestResponse = _context.RequestResponse.Where(s => s.DataPacket.Contains(dataToBeDelete.ContactPersonEmail)
                                || s.OutputPacket.Contains(dataToBeDelete.ContactPersonEmail));

                                query_archiveResponse = _context.Archive_RequestResponse.Where(s => s.DataPacket.Contains(dataToBeDelete.ContactPersonEmail)
                                || s.OutputPacket.Contains(dataToBeDelete.ContactPersonEmail));
                            }
                            else
                            {
                                query_requestResponse = _context.RequestResponse.Where(s => (s.DataPacket.Contains(dataToBeDelete.ContactPersonEmail)
                                || s.OutputPacket.Contains(dataToBeDelete.ContactPersonEmail))
                                  && MethodNames.Contains(s.MethodName));

                                query_archiveResponse = _context.Archive_RequestResponse.Where(s => (s.DataPacket.Contains(dataToBeDelete.ContactPersonEmail)
                                 || s.OutputPacket.Contains(dataToBeDelete.ContactPersonEmail))
                                && MethodNames.Contains(s.MethodName));
                            }

                            _context.RequestResponse.RemoveRange(query_requestResponse);
                            _context.Archive_RequestResponse.RemoveRange(query_archiveResponse);
                            _context.SaveChanges();

                            // set Status Deleted if Data Successfully Delete
                            dataToBeDelete.Status = (short)DataDeleteStatus.Deleted;    
                            _context.Entry(dataToBeDelete).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            CustomLogger.LogException(ex, StoreId, UserId);

                            // set Status 'Error' if there is an exception
                            dataToBeDelete.Status = (short)DataDeleteStatus.Error;
                            _context.Entry(dataToBeDelete).State = System.Data.Entity.EntityState.Modified;
                            _context.SaveChanges();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }
    }
}