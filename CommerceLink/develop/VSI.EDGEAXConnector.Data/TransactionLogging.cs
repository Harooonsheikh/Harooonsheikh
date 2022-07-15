using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using VSI.EDGEAXConnector.Logging;
using System.Data.Entity.Validation;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.Data
{
    public class TransactionLogging : BaseClass
    {

        public TransactionLogging(string storeKey) : base(storeKey)
        {

        }

        private IntegrationDBEntities _Context;
        public bool LogTransaction(SyncJobs EntityId, string Description, DateTime TimeStamp, byte[] filebyte)
        {
            return this.LogTransaction((int)EntityId, Description, TimeStamp, filebyte);
        }

        public bool LogTransaction(int EntityId, string Description, DateTime TimeStamp, byte[] filebyte)
        {
            try
            {
                if (filebyte == null)
                {
                    filebyte = new byte[0];
                }

                    //TransactionsLog obj = new TransactionsLog();
                    //obj.Description = Description;
                    //obj.EntityId = EntityId;
                    //obj.TimeStamp = TimeStamp;
                    //obj.CreatedOn = DateTime.UtcNow;
                    //obj.CreatedBy = UserId;
                    //obj.StoreId = StoreId;
               
                    //_Context.TransactionsLog.Add(obj);
                    //_Context.SaveChanges();
            }

            catch (DbEntityValidationException e)
            {
                //TODO: what is the purpose of following code, please review it.
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
                return false;
            }

            return true;
        }

        public byte[] GetCSVFromLocalFolder(string path)
        {
            byte[] FileBytes = { };
            try
            {
                if (File.Exists(path))
                {
                    FileStream stream = File.OpenRead(path);
                    FileBytes = new byte[stream.Length];

                    stream.Read(FileBytes, 0, FileBytes.Length);
                    stream.Close();
                }

                else
                {
                    //Log Customised Message
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
            return FileBytes;
        }
    }
}
