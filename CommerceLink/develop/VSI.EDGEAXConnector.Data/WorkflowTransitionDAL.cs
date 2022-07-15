using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data.ViewModels;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class WorkFlowTransitionDAL: BaseClass
    {
        public WorkFlowTransitionDAL(string storeKey) : base(storeKey)
        {

        }
        public WorkFlowTransitionDAL(string connectionString, string storeKey, string user) : base(connectionString, storeKey, user)
        {

        }

        //public  void InsertJobWorkFlow(string fileName, WorkFlowStates workflowState)
        //{
        //    try
        //    {
        //        using (var db = this.GetConnection())
        //        {
        //            //int instanceId = db.WorkFlowStatus.Where(i => i.InstanceName.Equals(fileName)).FirstOrDefault().WorkFlowStatusId;
        //            var instanceId = db.WorkFlowStatus.Where(i => i.InstanceName.Equals(fileName)).FirstOrDefault();
        //            WorkFlowTransition jobWorkflow = new WorkFlowTransition()
        //            {
        //                InstanceId = instanceId.WorkFlowStatusId,
        //                WorkFlowStateId = (short)workflowState,
        //                StoreId= this.StoreId,
        //                CreatedOn = DateTime.UtcNow
        //            };
        //            db.WorkFlowTransition.Add(jobWorkflow);
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        customLogger.LogException(exp);
        //    }
        //}
        //public  void InsertJobWorkFlowByID(int workflowStatusId, WorkFlowStates workflowState)
        //{
        //    try
        //    {
        //        using (var db = this.GetConnection())
        //        {
        //            WorkFlowTransition jobWorkflow = new WorkFlowTransition()
        //            {
        //                InstanceId = workflowStatusId,
        //                WorkFlowStateId = (short)workflowState,
        //                StoreId= this.StoreId,
        //                CreatedOn = DateTime.UtcNow
        //            };
        //            db.WorkFlowTransition.Add(jobWorkflow);
        //            db.SaveChanges();
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        customLogger.LogException(exp);
        //    }
        //}

        public List<WorkFlowTransition> WorkflowTransition(int instanceId)
        {
            try
            {
                List<WorkFlowTransition> transitions = new List<WorkFlowTransition>();
                using (var db = this.GetConnection())
                {
                    transitions = db.WorkFlowTransition.Where(m => m.InstanceId == instanceId).OrderBy(m=>m.WorkFlowStateId).ToList();
                }
                return transitions;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw new Exception("Unable to Get transitions",ex);
            }
        }

        
    }
}
