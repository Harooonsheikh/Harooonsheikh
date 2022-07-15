using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.UI.Managers
{
    public class JobsManager
    {
        private static JobsDAL jobDAL = new JobsDAL(StoreService.StoreLkey);
        public static List<Job> GetAllJobs()
        {
            List<Job> lstJobs = new List<Job>();
            lstJobs = jobDAL.GetAllJobs();
            return lstJobs;
        }

        public static List<Job> GetAllJobsByType(bool type)
        {
            List<Job> lstJobs = new List<Job>();
            lstJobs = jobDAL.GetAllJobsByType(type);
            return lstJobs;
        }

        public static bool UpdateJobById(Job jobObj)
        {
            List<Job> lstJobs = new List<Job>();
            return jobDAL.UpdateJobById(jobObj);
        }

    }
}
