using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class JobRepository : BaseClass
    {
        public JobRepository(string storeKey) : base(storeKey)
        {
              
        }

        private IntegrationDBEntities _context;
        public void JobLog(Int64 JobId, int Status)
        {
            try
            {
                using (_context = new IntegrationDBEntities())
                {
                    JobLog obj = new JobLog();
                    obj.JobId = JobId;
                    obj.Status = Status;
                    obj.LastExecutionTimeStamp = DateTime.UtcNow;
                    obj.CreatedOn = DateTime.UtcNow;
                    obj.CreatedBy = UserId;
                    obj.StoreId = StoreId;

                    _context.JobLog.Add(obj);
                    _context.SaveChanges();
                }

            }

            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
                throw;
            }
        }
        public bool IsJobCompletedTodayInJobLog(Int64 JobId, int jobStatus)
        {
            bool isJobCompletedToday = false;
            try
            {
                List<JobLog> jobLogList = new List<JobLog>();

                using (IntegrationDBEntities db = new IntegrationDBEntities())
                {
                    DateTime dateTime = DateTime.UtcNow;
                    jobLogList = db.JobLog.Where(x => x.JobId == JobId && x.LastExecutionTimeStamp.Value.Year == dateTime.Year && x.LastExecutionTimeStamp.Value.Month == dateTime.Month && x.LastExecutionTimeStamp.Value.Day == dateTime.Day && x.Status == jobStatus && x.StoreId == StoreId).ToList();
                }

                isJobCompletedToday = jobLogList.Count == 0 ? false : true;

                return isJobCompletedToday;
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
                return isJobCompletedToday;
            }
        }
        public List<Job> getAllJobs()
        {
            List<Job> job = new List<Job>();
            try
            {
                using (_context = new IntegrationDBEntities())
                {

                    var jobQuery = (from c in _context.Job
                                    select c);
                    job = jobQuery.ToList();
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
            return job;
        }
        public List<string> GetEmailAdresses(int entity)
        {
            List<string> emailIds = new List<string>();
            try
            {
                using (_context = new IntegrationDBEntities())
                {
                    var jobQuery = (from emailSub in _context.EmailSubscriber
                                    where emailSub.TemplateId == entity && emailSub.StoreId == StoreId
                                    join sub in _context.Subscriber on emailSub.SubscriberId
                                    equals sub.SubscriberId
                                    where sub.IsActive == true
                                    select sub.Email);


                    emailIds = jobQuery.ToList();
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
            return emailIds;
        }
        public string GetEmailFooter(int entity)
        {
            string footer = string.Empty;
            try
            {
                using (_context = new IntegrationDBEntities())
                {

                    var query = (from emailTemp in _context.EmailTemplate
                                 where emailTemp.EmailTemplateId == entity
                                 select emailTemp.Footer);

                    foreach (string emailSubject in query.ToList())
                    {
                        footer = emailSubject;
                    }
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
            return footer;
        }
        public string GetEmailBody(int entity)
        {
            string body = string.Empty;
            try
            {
                using (_context = new IntegrationDBEntities())
                {

                    var query = (from emailTemp in _context.EmailTemplate
                                 where emailTemp.EmailTemplateId == entity && emailTemp.StoreId == StoreId
                                 select emailTemp.Body);

                    foreach (string tempBody in query.ToList())
                    {
                        body = tempBody;
                    }
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
            return body;
        }
        public string GetEmailSubject(int entity)
        {
            string subject = string.Empty;
            try
            {
                using (_context = new IntegrationDBEntities())
                {
                    var query = (from emailTemp in _context.EmailTemplate
                                 where emailTemp.EmailTemplateId == entity && emailTemp.StoreId == StoreId
                                 select emailTemp.Subject);

                    foreach (string emailSubject in query.ToList())
                    {
                        subject = emailSubject;
                    }

                }

            }

            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
            return subject;
        }
        public string GetEmailSubscriberName(string emailId)
        {
            string name = string.Empty;
            try
            {
                using (_context = new IntegrationDBEntities())
                {
                    var query = (from subcribers in _context.Subscriber
                                 where subcribers.Email == emailId && subcribers.StoreId == StoreId
                                 select subcribers.Name);
                    foreach (string tempName in query.ToList())
                    {
                        name = tempName;
                    }
                }
            }

            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
            return name;
        }
        public List<Log> getAllLogs(int n)
        {
            List<Log> log = new List<Log>();
            try
            {
                using (_context = new IntegrationDBEntities())
                {
                    var logQuery = (from c in _context.Log
                                    orderby c.CreatedOn descending
                                    where c.StoreId == StoreId
                                    select c).Take(n);
                    log = logQuery.ToList();
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
            return log;
        }
        public List<Log> getAllLogs(int numberOfLogs, string nameDb)
        {
            List<Log> log = new List<Log>();
            try
            {
                using (_context = new IntegrationDBEntities(nameDb))
                {
                    var logQuery = (from c in _context.Log
                                    where c.StoreId == StoreId
                                    orderby c.CreatedOn descending
                                    select c).Take(numberOfLogs);
                    log = logQuery.ToList();
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
            return log;
        }
        //TODO: Nothing.Pure beauty.
        public List<Job> getRunningJobs(string nameDb, bool sync)
        {
            List<Job> jobs = new List<Job>();
            try
            {
                using (_context = new IntegrationDBEntities(nameDb))
                {
                    if (sync == true)
                    {
                        var jobQuery = (from c in _context.Job
                                        where c.JobName.ToLower().Contains("upload") | c.JobName.ToLower().Contains("download") // c.JobStatus == 2 &&

                                        select c);
                        return jobQuery.ToList();
                    }
                    else
                    {
                        var jobQuery = (from c in _context.Job
                                        where !c.JobName.ToLower().Contains("upload") && !c.JobName.ToLower().Contains("download") //c.JobStatus == 2 &&

                                        select c);
                        jobs = jobQuery.ToList();
                        return jobs;
                    }
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
            return jobs;
        }
        public void UpdateJobStatus(Int64 JobId, int JobStatus, int storeId)
        {
            try
            {
                using (_context = new IntegrationDBEntities())
                {
                    var jobRecord = _context.JobSchedule.SingleOrDefault(job => job.JobId == JobId && job.StoreId == storeId);
                    if (jobRecord != null)
                    {
                        jobRecord.JobStatus = JobStatus;
                        //jobRecord.StartTime = DateTime.UtcNow.TimeOfDay; // To save time of current status update
                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
        }
        public Job GetJob(int JobId)
        {
            Job resultJob = null;
            try
            {
                using (_context = new IntegrationDBEntities())
                {
                    var jobRecord = _context.Job.SingleOrDefault(job => job.JobID == JobId);
                    resultJob = jobRecord;
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
            return resultJob;
        }
        public DateTime GetLastJobCallTimeStamp(long JobId)
        {
            DateTime LatestJobTime = DateTime.UtcNow;
            try
            {
                using (_context = new IntegrationDBEntities())
                {
                    var LastJobCalledRow = (from LastJobCalled in _context.JobLog
                                            where LastJobCalled.Status == 1 && LastJobCalled.JobId == JobId && LastJobCalled.StoreId == StoreId
                                            select LastJobCalled).OrderByDescending(x => x.LastExecutionTimeStamp).FirstOrDefault();

                    if (LastJobCalledRow != null)
                        LatestJobTime = DateTime.Parse(LastJobCalledRow.LastExecutionTimeStamp.ToString());
                    return LatestJobTime;
                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
                return LatestJobTime;
            }
        }
        public void ResetAllJobsStatus()
        {
            try
            {
                using (_context = new IntegrationDBEntities())
                {
                    var jobRecord = _context.JobSchedule;
                    if (jobRecord != null)
                    {
                        foreach (var job in jobRecord)
                        {
                            job.JobStatus = 1;
                            job.StoreId = StoreId;
                            job.CreatedBy = UserId;
                            job.ModifiedOn = DateTime.UtcNow;
                        }
                        _context.SaveChanges();
                    }

                }
            }
            catch (Exception exp)
            {
                CustomLogger.LogException(exp, StoreId, UserId);
            }
        }

        public JobSchedule GetJobSchedule(Int64 JobId, int storeId)
        {
            using (_context = new IntegrationDBEntities())
            {
                JobSchedule jobRecord = _context.JobSchedule.SingleOrDefault(job => job.JobId == JobId && job.StoreId == storeId);
                return jobRecord;
            }
        }
    }
}
