using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    class WorkFlowStateDAL : BaseClass
    {
        public WorkFlowStateDAL(string storeKey) : base(storeKey)
        {

        }

        public List<WorkFlowState> GetWorkFlowStatesByType(WorkFlowType workFlowType)
        {
            try
            {
                using (var db = this.GetConnection())
                {
                    return db.WorkFlowState.Where(type => type.WorkFlowID == (short)workFlowType).ToList();
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }

            return null;
        }
    }
}
