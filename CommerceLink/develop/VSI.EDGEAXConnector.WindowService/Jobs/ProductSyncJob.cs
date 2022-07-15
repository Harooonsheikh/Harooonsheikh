using Autofac;
using Quartz;
using System;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.WindowService
{
    /// <summary>
    /// ProductSyncJob class is Product Sync Job.
    /// </summary>
    class ProductSyncJob : IJob
    {
        /// <summary>
        /// Execute method executes job task.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            Int64 JobId = -1;
            JobRepository JobRepo = new JobRepository(StoreService.StoreLkey);
            try
            {
                Job productJob = JobRepo.GetJob((int)SyncJobs.ProductSync);
                JobId = productJob.JobID;
                //if (productJob.JobStatus != (int)SynchJobStatus.InProgress)
                //{
                    JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.InProgress);
                    var manager = new FactoryManager();
                    var container = manager.Configure();
                    var productManager = container.Resolve<ProductManager>();
                    
                    ////TODO: Temp fix to control Category Job execution.
                    //if (JobRepo.GetJob((int)SyncJobs.CategorySync).IsActive==true)
                    //{
                    //    productManager.SyncCategories();
                    //}

                    //productManager.SyncProducts();
                    productManager.SyncProducts();
                    
                    //productManager.SyncImages();
                    JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.Available);
                    JobRepo.JobLog(JobId, true);
                    
                //}
            }
            catch (Exception ex)
            {
                JobRepo.UpdateJobStatus(JobId, (int)SynchJobStatus.Available);
                CustomLogger.LogException(ex);
                JobRepo.JobLog(JobId, false);
                
            }
        }
    }

}
