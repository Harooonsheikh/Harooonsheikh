//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VSI.EDGEAXConnector.Logging;
//using VSI.EDGEAXConnector.Enums;
//namespace VSI.EDGEAXConnector.Data
//{
//    public class WorkFlowStatusDAL : BaseClass
//    {
//        public WorkFlowStatusDAL(string storeKey) : base(storeKey)
//        {

//        }
//        public WorkFlowStatusDAL(string connectionString, string storeKey, string user) : base(connectionString, storeKey, user)
//        {

//        }
//        //public WorkFlowStatus InsertWorkFlowStatus(string instanceName, int jobId, Entities entity, WorkFlowStates workFlowStates)
//        //{
//        //    WorkFlowStatus wkfStatus = null;
//        //    try
//        //    {
//        //        using (var db = this.GetConnection())
//        //        {
//        //            wkfStatus = new WorkFlowStatus()
//        //            {
//        //                InstanceName = instanceName,
//        //                //JobId = jobId,
//        //                StoreId = this.StoreId,
//        //                EntityId = (short)entity,
//        //                WorkFlowStateId = (short)workFlowStates,
//        //                CreatedOn = DateTime.UtcNow,
//        //                ModifiedOn = DateTime.UtcNow
//        //            };
//        //            db.WorkFlowStatus.Add(wkfStatus);
//        //            db.SaveChanges();
//        //        }

//        //        return wkfStatus;
//        //    }
//        //    catch (Exception exp)
//        //    {
//        //        customLogger.LogException(exp);
//        //        return wkfStatus;
//        //    }
//        //}

//        public void UpdateWorkFlowStatus(string fileName, WorkFlowStates workFlowStates)
//        {
//            using (var db = this.GetConnection())
//            {
//                try
//                {
//                    var workFlowStatus = db.WorkFlowStatus.Where(x => x.InstanceName.Equals(fileName)).FirstOrDefault();
//                    workFlowStatus.WorkFlowStateId = (short)workFlowStates;
//                    workFlowStatus.ModifiedOn = DateTime.UtcNow;
//                    db.Entry(workFlowStatus).State = System.Data.Entity.EntityState.Modified;
//                    db.SaveChanges();

//                }
//                catch (Exception ex)
//                {
//                    customLogger.LogException(ex);
//                }
//            }
//        }

//        public void UpdateWorkFlowStatusByID(int workflowStatusId, WorkFlowStates workFlowStates)
//        {
//            using (var db = this.GetConnection())
//            {
//                try
//                {
//                    var workFlowStatus = db.WorkFlowStatus.Where(x => x.WorkFlowStatusId == workflowStatusId).FirstOrDefault();
//                    workFlowStatus.WorkFlowStateId = (short)workFlowStates;
//                    workFlowStatus.ModifiedOn = DateTime.UtcNow;
//                    db.Entry(workFlowStatus).State = System.Data.Entity.EntityState.Modified;
//                    db.SaveChanges();

//                }
//                catch (Exception ex)
//                {
//                    customLogger.LogException(ex);
//                }
//            }
//        }

//        public int WorkflowsCount(Entities[] entities, int daysCount, List<int> transitionStates)
//        {
//            int totalFlows = 0;
//            List<WorkFlowTransition> transitions = null;
//            int[] entitiesIdentifier = null;

//            try
//            {
//                entitiesIdentifier = Array.ConvertAll(entities, value => (int)value);
//                DateTime desiredDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(daysCount));
//                transitions = new List<WorkFlowTransition>();
//                totalFlows = 0;
//                using (var db = this.GetConnection())
//                {
//                    if (daysCount == -1)
//                    {
//                        totalFlows = db.WorkFlowStatus.Where(m => (m.StoreId == StoreId) && (entitiesIdentifier.Contains(m.EntityId)) && ((transitionStates.Contains(m.WorkFlowStateId)))).Count();
//                    }
//                    else
//                    {
//                        totalFlows = db.WorkFlowStatus.Where(m => (m.StoreId == StoreId) && (entitiesIdentifier.Contains(m.EntityId)) && (m.CreatedOn > desiredDate) && ((transitionStates.Contains(m.WorkFlowStateId)))).Count();
//                    }

//                }
//                return totalFlows;
//            }
//            catch (Exception ex)
//            {
//                customLogger.LogException(ex);
//                throw new Exception("Unable to count workflows", ex);
//            }
//        }

//        public WorkFlowStatus WorkflowStatus(string instanceName, int entityId)
//        {
//            WorkFlowStatus stat = null;
//            List<WorkFlowTransition> transitions = null;
//            try
//            {
//                stat = new WorkFlowStatus();
//                transitions = new List<WorkFlowTransition>();
//                using (var db = this.GetConnection())
//                {
//                    stat = db.WorkFlowStatus.Where(m => (m.InstanceName == instanceName) && (m.StoreId == StoreId) && (m.EntityId == entityId)).FirstOrDefault();
//                }
//                return stat;
//            }
//            catch (Exception ex)
//            {
//                customLogger.LogException(ex);
//                throw new Exception("Unable to Get transitions", ex);
//            }
//        }

//        public WorkFlowStatus WorkflowStatus(string instanceName)
//        {
//            WorkFlowStatus stat = null;
//            List<WorkFlowTransition> transitions = null;
//            try
//            {
//                stat = new WorkFlowStatus();
//                transitions = new List<WorkFlowTransition>();
//                using (var db = this.GetConnection())
//                {
//                    stat = db.WorkFlowStatus.Where(m => (m.InstanceName == instanceName) && (m.StoreId == StoreId)).FirstOrDefault();
//                }
//                return stat;
//            }
//            catch (Exception ex)
//            {
//                customLogger.LogException(ex);
//                throw new Exception("Unable to Get transitions", ex);
//            }
//        }

//        public List<WorkFlowStatus> Workflows(int offset, int pageSize, int iDaysCount, Entities[] entitiesList, List<int> transitionStates)
//        {
//            List<WorkFlowStatus> statLst = null;
//            int[] entitiesIdentifier = null;
//            try
//            {
//                statLst = new List<WorkFlowStatus>();
//                entitiesIdentifier = Array.ConvertAll(entitiesList, value => (int)value);
//                DateTime desiredDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(iDaysCount));
//                using (var db = this.GetConnection())
//                {
//                    if (iDaysCount == -1)
//                    {
//                        var records = db.WorkFlowStatus.Where(m => (entitiesIdentifier.Contains(m.EntityId)) && ((transitionStates.Contains(m.WorkFlowStateId))) && (m.StoreId == StoreId));
//                        records = records.OrderByDescending(m => m.CreatedOn).Skip(offset).Take(pageSize);
//                        statLst = records.ToList();
//                    }
//                    else
//                    {
//                        var records = db.WorkFlowStatus.Where(m => (m.CreatedOn > desiredDate) && (entitiesIdentifier.Contains(m.EntityId)) && ((transitionStates.Contains(m.WorkFlowStateId))) && (m.StoreId == StoreId));
//                        records = records.OrderByDescending(m => m.CreatedOn).Skip(offset).Take(pageSize);
//                        statLst = records.ToList();
//                    }
//                }
//                return statLst;
//            }
//            catch (Exception ex)
//            {
//                customLogger.LogException(ex);
//                throw new Exception("Unable to Read Workflows", ex);
//            }
//        }

//        public List<WorkFlowStatus> Search(string strFileName, int? offset, int? pageSize, int iDaysCount, Entities[] entitiesList, List<int> transitionStates)
//        {
//            List<WorkFlowStatus> statLst = null;
//            int[] entitiesIdentifier = null;
//            try
//            {
//                statLst = new List<WorkFlowStatus>();
//                entitiesIdentifier = Array.ConvertAll(entitiesList, value => (int)value);
//                DateTime desiredDate = DateTime.UtcNow.Subtract(TimeSpan.FromDays(iDaysCount));
//                using (var db = this.GetConnection())
//                {
//                    IQueryable<WorkFlowStatus> records;
//                    if (iDaysCount == -1)
//                    {
//                        records = db.WorkFlowStatus.Where(m => (entitiesIdentifier.Contains(m.EntityId)) && ((transitionStates.Contains(m.WorkFlowStateId))));
//                    }
//                    else
//                    {
//                        records = db.WorkFlowStatus.Where(m => (m.CreatedOn > desiredDate) && (entitiesIdentifier.Contains(m.EntityId)) && ((transitionStates.Contains(m.WorkFlowStateId))));
//                    }

//                    if (!string.IsNullOrEmpty(strFileName))
//                    {
//                        records = records.Where(m => m.InstanceName.Contains(strFileName));
//                    }
//                    records = records.OrderByDescending(m => m.CreatedOn);
//                    if (transitionStates != null)
//                    {
//                        records = records.Where(m => (transitionStates.Contains(m.WorkFlowStateId)));
//                    }
//                    if (offset != null)
//                    {
//                        records = records.Skip(offset.Value);
//                    }
//                    if (pageSize != null)
//                    {
//                        records = records.Take(pageSize.Value);
//                    }
//                    statLst = records.ToList();
//                }
//                return statLst;
//            }
//            catch (Exception ex)
//            {
//                customLogger.LogException(ex);
//                throw new Exception("Unable to Read Workflows", ex);
//            }
//        }

//        public List<KeyValuePair<int, Enums.WorkflowStatus>> Status(int[] wkfStatusIDs)
//        {
//            List<KeyValuePair<int, Enums.WorkflowStatus>> statLst = null;
//            try
//            {

//                using (var db = this.GetConnection())
//                {
//                    statLst = new List<KeyValuePair<int, Enums.WorkflowStatus>>();
//                    var records = db.WorkFlowStatus.Where(m => wkfStatusIDs.Contains(m.WorkFlowStatusId));
//                    records.ToList().ForEach(m =>
//                    {
//                        Enums.WorkflowStatus status;
//                        if (m.WorkFlowStateId == (int)WorkFlowStates.Completed)
//                        {
//                            status = Enums.WorkflowStatus.Success;
//                        }
//                        else if (m.WorkFlowStateId == (int)WorkFlowStates.Failed)
//                        {
//                            status = Enums.WorkflowStatus.Failure;
//                        }
//                        else
//                        {
//                            status = Enums.WorkflowStatus.Processing;
//                        }
//                        var pair = new KeyValuePair<int, Enums.WorkflowStatus>(m.WorkFlowStatusId, status);
//                        statLst.Add(pair);
//                    });
//                }
//                return statLst;
//            }
//            catch (Exception ex)
//            {
//                customLogger.LogException(ex);
//                throw new Exception("Unable to Read Workflows", ex);
//            }
//        }

//        public List<WorkFlowStatus> GetWorkFlowOfInProgressFiles()
//        {
//            List<WorkFlowStatus> lstInProcessWorkflow = null;
//            lstInProcessWorkflow = null;
//            try
//            {
//                lstInProcessWorkflow = new List<WorkFlowStatus>();
//                using (var db = this.GetConnection())
//                {
//                    int failed = (int)WorkFlowStates.Failed;
//                    int completed = (int)WorkFlowStates.Completed;
//                    int fileGenerated = (int)WorkFlowStates.FileGenerated;
//                    lstInProcessWorkflow = db.WorkFlowStatus.Where(m => m.WorkFlowStateId != failed && m.WorkFlowStateId != completed).ToList();
//                }

//                return lstInProcessWorkflow;
//            }
//            catch (Exception ex)
//            {
//                customLogger.LogException(ex);
//                return lstInProcessWorkflow;
//            }
//        }
//        public List<WorkFlowStatus> GetWorkFlowOfInProgressUploadFiles()
//        {
//            List<WorkFlowStatus> workflowsStuckAtSFTP = new List<WorkFlowStatus>();
//            workflowsStuckAtSFTP = null;
//            try
//            {
//                int uplodingToSFTP = (int)WorkFlowStates.UploadingToSftp;

//                using (var db = this.GetConnection())
//                {
//                    workflowsStuckAtSFTP = db.WorkFlowStatus.Where(m => m.WorkFlowStateId == uplodingToSFTP).ToList();
//                }

//                return workflowsStuckAtSFTP;
//            }
//            catch (Exception ex)
//            {
//                customLogger.LogException(ex);
//                return workflowsStuckAtSFTP;
//            }
//        }
//    }
