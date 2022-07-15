using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class DimensionSetDAL : BaseClass
    {
        public DimensionSetDAL(string storeKey) : base(storeKey)
        {

        }
        public DimensionSetDAL(string conn, string storeKey, string user) : base(conn, storeKey, user)
        {

        }
        public List<DimensionSet> GetAllDimensionSets()
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                List<DimensionSet> dimensionSetList = db.DimensionSet.Where(d=>d.StoreId == StoreId).ToList();
                return dimensionSetList;
            }
        }
        public bool UpdateDimensionSetById(DimensionSet dimObj)
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    var dim = db.DimensionSet.FirstOrDefault(o => o.DimensionSetId == dimObj.DimensionSetId);
                    dim.ComValue = dimObj.ComValue;
                    dim.ErpValue = dimObj.ErpValue;
                    dim.IsActive = dimObj.IsActive;
                    
                    dim.ModifiedOn = DateTime.UtcNow;
                    dim.ModifiedBy = UserId;
                    dim.AdditionalErpValue = dimObj.AdditionalErpValue;
                    db.Entry(dim).State = System.Data.Entity.EntityState.Modified;
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
    }
}
