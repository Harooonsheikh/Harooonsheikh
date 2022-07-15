using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data.ViewModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class JobsDAL : BaseClass
    {
        public JobsDAL(string storeKey) : base(storeKey)
        {

        }
        public JobsDAL(string connectionString, string storeKey, string user) : base(connectionString, storeKey, user)
        {

        }
        public List<Job> GetAllJobs()
        {
            List<Job> lstJobs = new List<Job>();

            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    lstJobs = db.Job.ToList();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstJobs;
        }
        public List<Job> GetAllJobsByType(bool type)
        {
            List<Job> lstJobs = new List<Job>();

            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    lstJobs = db.Job.Where(j=>j.JobTypeId == type).ToList();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstJobs;
        }
        public bool UpdateJobById(Job jobObj)
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    var job = db.Job.FirstOrDefault(j => j.JobID == jobObj.JobID);
                    //job.JobInterval = jobObj.JobInterval;
                    //job.IsRepeatable = jobObj.IsRepeatable;
                    //job.IsActive = jobObj.IsActive;
                    job.ModifiedOn = DateTime.UtcNow;
                    job.ModifiedBy = UserId;
                    //job.StoreId = StoreId;

                    db.Entry(job).State = System.Data.Entity.EntityState.Modified;
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

        public List<JobVM> Get(int storeId, bool type)
        {
            List<JobVM> lstJobs = new List<JobVM>();

            IntegrationDBEntities db = this.GetConnection();
            using (db)
            {
                try
                {
                    var jobs = from job in db.Job
                               join jobSchedule in db.JobSchedule on job.JobID equals jobSchedule.JobId
                               where (job.JobTypeId == type && job.Enabled == true && jobSchedule.StoreId == StoreId)
                               select new { job.JobID, job.JobName, job.JobTypeId, jobSchedule.JobInterval, jobSchedule.IsRepeatable, jobSchedule.IsActive, jobSchedule.JobStatus, jobSchedule.StartTime, jobSchedule.StoreId };

                    foreach (var job in jobs)
                    {
                        JobVM tempJob = new JobVM();
                        tempJob.JobID = job.JobID;
                        tempJob.JobName = job.JobName;
                        tempJob.JobStatus = job.JobStatus;
                        tempJob.JobInterval = job.JobInterval;
                        tempJob.JobTypeId = job.JobTypeId;
                        tempJob.StoreId = job.StoreId;
                        tempJob.IsActive = job.IsActive;
                        tempJob.IsRepeatable = job.IsRepeatable;
                        tempJob.StartTime = job.StartTime;

                        lstJobs.Add(tempJob);
                    }
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    throw;
                }
            }

            return lstJobs;
        }

        public bool Update(JobSchedule jobSchedule)
        {
            try
            {
                IntegrationDBEntities db = this.GetConnection();
                using (db)
                { 
                    db.Entry(jobSchedule).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw;
            }
        }
        public JobSchedule Get(long jobId,int StoreId)
        {
            JobSchedule jobSchedule = null;
            try
            {
                IntegrationDBEntities db = this.GetConnection();
                using (db)
                {
                    jobSchedule = db.JobSchedule.FirstOrDefault(j => j.JobId == jobId && j.StoreId == StoreId);
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw;
            }

            return jobSchedule;
        }
    }
}
