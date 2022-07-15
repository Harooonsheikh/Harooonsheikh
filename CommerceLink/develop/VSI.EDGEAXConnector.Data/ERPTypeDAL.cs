using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class ERPTypeDAL : BaseClass
    {
        public ERPTypeDAL(string connection)
        {
            this.ConnectionString = connection;
        }
        public ERPTypeDAL(string connectionString, string storeKey, string user) : base(connectionString, storeKey, user)
        {

        }
        public KeyValuePair<int, string> Get(int erpTypeId)
        {
            KeyValuePair<int, string> ERPType;
            try
            {
                using (var db = this.GetConnection())
                {
                    var erp = db.ERPType.Where(e => e.ERPTypeId == erpTypeId && e.IsActive == true).FirstOrDefault();
                    ERPType = new KeyValuePair<int, string>(erp.ERPTypeId, erp.ERPName);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
            return ERPType;
        }

    }
}
