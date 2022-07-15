//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VSI.EDGEAXConnector.Enums;

//namespace VSI.EDGEAXConnector.Data
//{
//    class WorkflowManger
//    {
//        private WorkFlowStatus WorkFlowStatus { get; set; }
//        private WorkFlowStatusDAL WorkFlowStatusDAL { get; set; }
//        private WorkFlowTransitionDAL WorkFlowTransitionDAL { get; set; }
//        private WorkFlowStateDAL WorkFlowStateDAL { get; set; }
//        private List<WorkFlowState> WorkFlowStatess { get; set; }

//        public WorkflowManger(string storeKey)
//        {
//            WorkFlowStatusDAL = new WorkFlowStatusDAL(storeKey);
//            WorkFlowTransitionDAL = new WorkFlowTransitionDAL(storeKey);
//            WorkFlowStateDAL = new WorkFlowStateDAL(storeKey);
//        }

//        //public void Start(string fileName, int jobId, Entities entity, WorkFlowType workFlowType)
//        //{
//        //    WorkFlowStatess = WorkFlowStateDAL.GetWorkFlowStatesByType(workFlowType);
//        //    var firststate = WorkFlowStatess.First(startstate => startstate.OrderSequence == 1);
//        //    WorkFlowStatus = WorkFlowStatusDAL.InsertWorkFlowStatus(fileName, jobId, entity, (WorkFlowStates)firststate.WorkFlowStateID);
//        //    WorkFlowTransitionDAL.InsertJobWorkFlowByID(WorkFlowStatus.WorkFlowStatusId, (WorkFlowStates)firststate.WorkFlowStateID);
//        //}

//        //public void StartFileSync(string fileName, Entities entity, WorkFlowType workFlowType)
//        //{
//        //    WorkFlowStatess = WorkFlowStateDAL.GetWorkFlowStatesByType(workFlowType);
//        //    WorkFlowStatus = WorkFlowStatusDAL.WorkflowStatus(fileName);
//        //    if (WorkFlowStatus == null)
//        //    {
//        //        WorkFlowStatus = WorkFlowStatusDAL.InsertWorkFlowStatus(fileName, 0, entity, WorkFlowStates.FileGenerated);
//        //    }
//        //}

//        //public void MoveNextState()
//        //{
//        //    var currentstate = WorkFlowStatess.First(curstate => curstate.WorkFlowStateID == WorkFlowStatus.WorkFlowStateId);
//        //    var nextstate = WorkFlowStatess.First(nexstate => nexstate.OrderSequence == (currentstate.OrderSequence + 1));
//        //    WorkFlowStatusDAL.UpdateWorkFlowStatusByID(WorkFlowStatus.WorkFlowStatusId, (WorkFlowStates)nextstate.WorkFlowStateID);
//        //    WorkFlowTransitionDAL.InsertJobWorkFlowByID(WorkFlowStatus.WorkFlowStatusId, (WorkFlowStates)nextstate.WorkFlowStateID);
//        //    WorkFlowStatus = WorkFlowStatusDAL.WorkflowStatus(WorkFlowStatus.InstanceName, WorkFlowStatus.EntityId);
//        //}

//        //public void Completed()
//        //{
//        //    WorkFlowStatusDAL.UpdateWorkFlowStatusByID(WorkFlowStatus.WorkFlowStatusId, WorkFlowStates.Completed);
//        //    WorkFlowTransitionDAL.InsertJobWorkFlowByID(WorkFlowStatus.WorkFlowStatusId, WorkFlowStates.Completed);
//        //}

//        //public void Failed()
//        //{
//        //    WorkFlowStatusDAL.UpdateWorkFlowStatusByID(WorkFlowStatus.WorkFlowStatusId, WorkFlowStates.Failed);
//        //    WorkFlowTransitionDAL.InsertJobWorkFlowByID(WorkFlowStatus.WorkFlowStatusId, WorkFlowStates.Failed);
//        //}
//    }
//}
