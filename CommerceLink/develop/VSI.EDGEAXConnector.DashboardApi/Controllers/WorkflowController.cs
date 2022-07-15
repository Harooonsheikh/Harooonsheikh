using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.DashboardApi.Infrastructure;
using VSI.EDGEAXConnector.DashboardApi.ViewModel;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DAL;
using VSI.EDGEAXConnector.Data.ViewModels;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    public class WorkflowController : ApiBaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public WorkflowController()
        {
            ControllerName = "WorkflowController";
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult States(int entity) {
            EntityDAL enDal = null;
            Entity ent = null;
            List<WorkFlowStatesVM> states = null;
            try
            {
                enDal = new EntityDAL(this.DbConnStr, this.StoreKey, this.User);
                ent = enDal.Entity(entity);
                states = new List<WorkFlowStatesVM>();
                if (ent.WorkFlowId == 1)
                {
                    WorkFlowStatesVM axState = new WorkFlowStatesVM(Convert.ToString((int)WorkFlowStates.AXDataFetching), "AX Data Fetching", true);
                    WorkFlowStatesVM clState = new WorkFlowStatesVM(Convert.ToString((int)WorkFlowStates.CommerceLinkProcessing), "CommerceLink Procesing", true);
                    WorkFlowStatesVM fileState = new WorkFlowStatesVM(Convert.ToString((int)WorkFlowStates.FileGeneration), "File Generation", true);
                    WorkFlowStatesVM fileGen= new WorkFlowStatesVM(Convert.ToString((int)WorkFlowStates.FileGenerated), "File Generated", true);
                    WorkFlowStatesVM sftpState = new WorkFlowStatesVM(Convert.ToString((int)WorkFlowStates.UploadingToSftp), "Upload to SFTP", true);
                    states.Add(axState);
                    states.Add(clState);
                    states.Add(fileState);
                    states.Add(fileGen);
                    states.Add(sftpState);
                }
                else
                {
                    WorkFlowStatesVM axState = new WorkFlowStatesVM(Convert.ToString((int)WorkFlowStates.GetSaleOrderNumber), "Get Sales Order Number", true);
                    WorkFlowStatesVM clState = new WorkFlowStatesVM(Convert.ToString((int)WorkFlowStates.ProcessSaleOrder), "CommerceLink Procesing", true);
                    WorkFlowStatesVM fileState = new WorkFlowStatesVM(Convert.ToString((int)WorkFlowStates.PushSaleOrderToAX), "Push Sales Order to AX", true);
                    states.Add(axState);
                    states.Add(clState);
                    states.Add(fileState);
                }
                WorkFlowStatesVM comState = new WorkFlowStatesVM(Convert.ToString((int)WorkFlowStates.Completed), "Completed", false);
                
                WorkFlowStatesVM failState = new WorkFlowStatesVM(Convert.ToString((int)WorkFlowStates.Failed), "Failed", false);

                states.Add(comState);
                states.Add(failState);
                return Ok(states);
            }
            catch (Exception ex)
            {

                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Count([FromUri] int daysCount, [FromUri] Entities[] entitiesArray, Enums.WorkflowStatus? workFlowStepStatus = null)
        {
            long iTotalCount = 0;
            WorkFlowStatusDAL statusMgr = null;
            List<int> transitionsAllowed = null;

            try
            {
                statusMgr = new WorkFlowStatusDAL(this.DbConnStr, this.StoreKey, this.User);
                transitionsAllowed = new List<int>();
                transitionsAllowed = this.GetTransitions(workFlowStepStatus);
                iTotalCount = statusMgr.WorkflowsCount(entitiesArray, daysCount, transitionsAllowed);
                return Ok(iTotalCount);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Transition(int entityId,string fileName)
        {
            WorkFlowTransitionDAL wfTransition = null;
            WorkFlowStatusDAL statMgr = null;
            List<WorkFlowTransition> trans = null;
            try
            {

                wfTransition = new WorkFlowTransitionDAL(this.DbConnStr, this.StoreKey, this.User);
                statMgr = new WorkFlowStatusDAL(this.DbConnStr, this.StoreKey, this.User);
                var status = statMgr.WorkflowStatus(fileName, entityId);

                trans =  wfTransition.WorkflowTransition(status.WorkFlowStatusId);
                return Ok(trans);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult State(int entityId, string fileName)
        {
            WorkFlowStatusDAL statMgr = null;
            int stateId = 0;
            try
            {
                statMgr = new WorkFlowStatusDAL(this.DbConnStr, this.StoreKey, this.User);
                var status = statMgr.WorkflowStatus(fileName, entityId);
                stateId = status.WorkFlowStateId;
                return Ok(stateId);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get(string fileName)
        {
            WorkFlowTransitionDAL workflowTransitionsDAL = null;
            WorkFlowStatusDAL workflowStatMgr = null;
            List<WorkFlowTransitionVM> viewModels = null;

            try
            {
              
                workflowTransitionsDAL = new WorkFlowTransitionDAL(this.DbConnStr, this.StoreKey, this.User);
                workflowStatMgr = new WorkFlowStatusDAL(this.DbConnStr, this.StoreKey, this.User);
                var stat = workflowStatMgr.WorkflowStatus(fileName);
                List<WorkFlowTransition> trans =  workflowTransitionsDAL.WorkflowTransition(stat.WorkFlowStatusId);
                viewModels = new List<WorkFlowTransitionVM>();
                trans.ForEach(m =>
                {
                    var model = new WorkFlowTransitionVM();
                    model.InstanceID = m.InstanceId;
                    model.StateID = m.WorkFlowStateId;
                    model.Created = m.CreatedOn;
                    model.WorkFlowTransitionID = m.WorkFlowTransitionId;
                    viewModels.Add(model);
                });
                return Ok(viewModels);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult SearchCount([FromUri] string fileName, [FromUri] int daysCount, [FromUri] Entities[] entitiesArray, [FromUri] Enums.WorkflowStatus? workFlowStepStatus = null)
        {
            List<WorkFlowStatus> wfStatus = null;
            WorkFlowStatusDAL workFlowMgr;
            List<int> transitionsAllowed = null;

            try
            {
                workFlowMgr = new WorkFlowStatusDAL(this.DbConnStr, this.StoreKey, this.User);
                transitionsAllowed = new List<int>();
                if (workFlowStepStatus != null)
                {
                    transitionsAllowed = this.GetTransitions(workFlowStepStatus.Value);
                }
                else
                {
                    transitionsAllowed = this.GetTransitions(null);
                }

                wfStatus = workFlowMgr.Search(fileName, null, null, daysCount, entitiesArray, transitionsAllowed);
                return Ok(wfStatus.Count());
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get([FromUri] int offset, [FromUri] int pageSize, [FromUri] int daysCount, [FromUri] Entities[] entitiesArray, [FromUri] Enums.WorkflowStatus? workFlowStepStatus = null)
        {
            List<WorkflowVM> wfStatus = new List<WorkflowVM>();
            WorkFlowStatusDAL statusDAL = new WorkFlowStatusDAL(this.DbConnStr, this.StoreKey, this.User);
            try
            {
                List<int> transitionsAllowed = new List<int>();
                if (workFlowStepStatus != null)
                {
                    transitionsAllowed = this.GetTransitions(workFlowStepStatus.Value);
                }
                else
                {
                    transitionsAllowed = this.GetTransitions(null);
                }

                var workflows = statusDAL.Workflows(offset, pageSize, daysCount, entitiesArray, transitionsAllowed);
                workflows.ForEach(wf =>
                {
                    WorkflowVM wfVM = new WorkflowVM();
                    wfVM.Created = wf.CreatedOn;
                    wfVM.EntityId = wf.EntityId;
                    wfVM.Id = wf.WorkFlowStatusId;
                    wfVM.InstanceName = wf.InstanceName;
                    wfVM.JobId = wf.JobId;
                    if(wf.WorkFlowStateId== (int)WorkFlowStates.Completed)
                    {
                        wfVM.Status = "Success";
                    }
                    else if (wf.WorkFlowStateId == (int)WorkFlowStates.Failed)
                    {
                        wfVM.Status = "Failure";
                    }
                    else
                    {
                        wfVM.Status = "Processing";
                    }
                    wfStatus.Add(wfVM);
                });
                return Ok(wfStatus);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Search([FromUri] string fileName, [FromUri] int offset, [FromUri] int pageSize, [FromUri] int daysCount, [FromUri] Entities[] entitiesArray,
            [FromUri] Enums.WorkflowStatus? workFlowStepStatus = null)
        {
            List<WorkflowVM> wfStatus = new List<WorkflowVM>();
            WorkFlowStatusDAL workFlowMgr = null;

            try
            {
                workFlowMgr = new WorkFlowStatusDAL(this.DbConnStr, this.StoreKey, this.User);
                List<int> transitionsAllowed = new List<int>();
                if (workFlowStepStatus != null)
                {
                    transitionsAllowed = this.GetTransitions(workFlowStepStatus.Value);
                }
                else
                {
                    transitionsAllowed = this.GetTransitions(null);
                }

                var workflows = workFlowMgr.Search(fileName, offset, pageSize, daysCount, entitiesArray, transitionsAllowed);
                workflows.ForEach(wf =>
                {
                    WorkflowVM wfVM = new WorkflowVM();
                    wfVM.Created = wf.CreatedOn;
                    wfVM.EntityId = wf.EntityId;
                    wfVM.Id = wf.WorkFlowStatusId;
                    wfVM.InstanceName = wf.InstanceName;
                    wfVM.JobId = wf.JobId;
                    if (wf.WorkFlowStateId == (int)WorkFlowStates.Completed)
                    {
                        wfVM.Status = "Success";
                    }
                    else if (wf.WorkFlowStateId == (int)WorkFlowStates.Failed)
                    {
                        wfVM.Status = "Failure";
                    }
                    else
                    {
                        wfVM.Status = "Processing";
                    }
                    wfStatus.Add(wfVM);
                });

                return Ok(wfStatus);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Statistic([FromUri] int daysCount, [FromUri] Entities[] entitiesArray)
        {
            JobWorkFlowStatistics jobWorkFlowStatistics = new JobWorkFlowStatistics();
            WorkFlowStatusDAL statusMgr = null;
            List<int> transitionsAllowed = null;

            try
            {


                statusMgr = new WorkFlowStatusDAL(this.DbConnStr, this.StoreKey, this.User);
                transitionsAllowed = new List<int>();
                jobWorkFlowStatistics = new JobWorkFlowStatistics();
                transitionsAllowed = this.GetTransitions(Enums.WorkflowStatus.Processing);
                jobWorkFlowStatistics.InProcessWorkFlowsCount = statusMgr.WorkflowsCount(entitiesArray, daysCount, transitionsAllowed); 
                transitionsAllowed = this.GetTransitions(Enums.WorkflowStatus.Failure);
                jobWorkFlowStatistics.FailedWorkFlowsCount = statusMgr.WorkflowsCount(entitiesArray, daysCount, transitionsAllowed);
                transitionsAllowed = this.GetTransitions(Enums.WorkflowStatus.Success);
                jobWorkFlowStatistics.CompletedWorkFlowsCount = statusMgr.WorkflowsCount(entitiesArray, daysCount, transitionsAllowed);
                jobWorkFlowStatistics.UniqueWorkFlowsCount = jobWorkFlowStatistics.CompletedWorkFlowsCount + jobWorkFlowStatistics.FailedWorkFlowsCount + jobWorkFlowStatistics.InProcessWorkFlowsCount;
                
                return Ok(jobWorkFlowStatistics);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult WorkflowStatus(int[] wkfStatusIDs)
        {
            List<KeyValuePair<int, string>> workflowStatus = null;
            WorkFlowStatusDAL wfMgr = null;

            try
            {
                workflowStatus = new List<KeyValuePair<int, string>>();
                wfMgr = new WorkFlowStatusDAL(this.DbConnStr, this.StoreKey, this.User);
                wfMgr.Status(wkfStatusIDs).ForEach(item => {
                    string status = "";
                    if (item.Value == Enums.WorkflowStatus.Success)
                    {
                        status = "Success";
                    }
                    else if (item.Value == Enums.WorkflowStatus.Failure)
                    {
                        status = "Failure";
                    }
                    else
                    {
                        status = "Processing";
                    }
                    KeyValuePair<int, string> fileStat = new KeyValuePair<int, string>(item.Key, status);
                    workflowStatus.Add(fileStat);
                });

                return Ok(workflowStatus);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private List<int> GetTransitions(WorkflowStatus? status)
        {
            List<int> transitionsAllowed = new List<int>();
            if (status == null)
            {
                transitionsAllowed.Add((int)WorkFlowStates.AXDataFetching);
                transitionsAllowed.Add((int)WorkFlowStates.CommerceLinkProcessing);
                transitionsAllowed.Add((int)WorkFlowStates.FileGeneration);
                transitionsAllowed.Add((int)WorkFlowStates.UploadingToSftp);
                transitionsAllowed.Add((int)WorkFlowStates.Completed);
                transitionsAllowed.Add((int)WorkFlowStates.Failed);
                transitionsAllowed.Add((int)WorkFlowStates.GetSaleOrderNumber);
                transitionsAllowed.Add((int)WorkFlowStates.ProcessSaleOrder);
                transitionsAllowed.Add((int)WorkFlowStates.PushSaleOrderToAX);
                transitionsAllowed.Add((int)WorkFlowStates.FileGenerated);
            }
            else
            {
                switch (status)
                {
                    case Enums.WorkflowStatus.Success:
                        transitionsAllowed.Add((int)WorkFlowStates.Completed);
                        break;
                    case Enums.WorkflowStatus.Failure:
                        transitionsAllowed.Add((int)WorkFlowStates.Failed);
                        break;
                    case Enums.WorkflowStatus.Processing:
                        transitionsAllowed.Add((int)WorkFlowStates.AXDataFetching);
                        transitionsAllowed.Add((int)WorkFlowStates.CommerceLinkProcessing);
                        transitionsAllowed.Add((int)WorkFlowStates.FileGeneration);
                        transitionsAllowed.Add((int)WorkFlowStates.UploadingToSftp);
                        transitionsAllowed.Add((int)WorkFlowStates.GetSaleOrderNumber);
                        transitionsAllowed.Add((int)WorkFlowStates.ProcessSaleOrder);
                        transitionsAllowed.Add((int)WorkFlowStates.PushSaleOrderToAX);
                        transitionsAllowed.Add((int)WorkFlowStates.FileGenerated);
                        break;

                    default:
                        break;
                }
            }
            return transitionsAllowed;
        }

    }
}