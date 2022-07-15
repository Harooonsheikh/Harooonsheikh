using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class EcomTypeDAL: BaseClass
    {
        public EcomTypeDAL(string connection)
        {
            this.ConnectionString = connection;
        }
        public EcomTypeDAL(string connectionString, string storeKey, string user) : base(connectionString, storeKey, user)
        {

        }
        public List<EcomType> Get()
        {
            List<EcomType> lstEcomType = null;
            try
            {
                using (var db = this.GetConnection())
                {
                    lstEcomType = db.EcomType.Where(e => e.IsActive == true).ToList();
                    
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
            return lstEcomType;
        }
        public KeyValuePair<int, string> Get(int ecomTypeId)
        {
            KeyValuePair<int, string> EcomType;
            try
            {
                using (var db = this.GetConnection())
                {
                    var ecom = db.EcomType.Where(e => e.EcomTypeId == ecomTypeId && e.IsActive == true).FirstOrDefault();
                    EcomType = new KeyValuePair<int, string>(ecom.EcomTypeId, ecom.EcomName);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
            return EcomType;
        }

    }
}
