using System;
using System.Collections.Generic;
using System.Linq;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Data
{
    public class StoreService
    {
        public static List<StoreDto> listStores = new List<StoreDto>();

        public static StoreDto GetStoreByKey(string storeKey)
        {
            var store = listStores.FirstOrDefault(x => x.StoreKey == storeKey && x.IsActive);
            if (store == null)
            {
                using (IntegrationDBEntities db = new IntegrationDBEntities())
                {
                    var storeDto = db.Store
                        .Where(k => k.StoreKey == storeKey && k.IsActive)
                        .Select(s => new StoreDto()
                        {
                            StoreId = s.StoreId,
                            Name = s.Name,
                            IsActive = s.IsActive,
                            StoreKey = s.StoreKey,
                            RetailChannelId = s.RetailChannelId
                        })
                        .FirstOrDefault();

                    if (storeDto == null)
                        throw new CommerceLinkError("Store not found with key = " + storeKey);
                    else
                        listStores.Add(storeDto);

                    return storeDto;
                }
            }
            return store;


        }

        public static StoreDto GetStoreById(int storeId)
        {
            var store = listStores.FirstOrDefault(x => x.StoreId == storeId && x.IsActive);

            if (store != null)
                return store;

            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var storeDto = db.Store
                                    .Where(k => k.StoreId == storeId && k.IsActive)
                                    .Select(s => new StoreDto()
                                    {
                                        StoreId = s.StoreId,
                                        Name = s.Name,
                                        IsActive = s.IsActive,
                                        StoreKey = s.StoreKey,
                                        RetailChannelId = s.RetailChannelId
                                    })
                                    .FirstOrDefault();

                if (storeDto == null)
                    throw new CommerceLinkError("Store not found with StoreId = " + storeId);
                else
                    listStores.Add(storeDto);

                return storeDto;
            }
        }

        public static List<StoreDto> GetStoreByKeys(string storeKeys, string ignoreStoreKeys)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                if (!string.IsNullOrEmpty(storeKeys))
                {
                    var configuredStores = storeKeys.Split(',').Select(a => a.Trim()).ToList();
                    return listStores = db.Store
                                    .Where(t => configuredStores.Contains(t.StoreKey) && t.IsActive)
                                    .Select(s => new StoreDto()
                                    {
                                        StoreId = s.StoreId,
                                        Name = s.Name,
                                        IsActive = s.IsActive,
                                        StoreKey = s.StoreKey,
                                        RetailChannelId = s.RetailChannelId
                                    })
                                    .ToList();
                }
                else
                {
                    var ignoreStores = ignoreStoreKeys.Split(',').Select(a => a.Trim()).ToList();
                    return listStores = db.Store
                                            .Where(t => !ignoreStores.Contains(t.StoreKey) && t.IsActive)
                                            .Select(s => new StoreDto()
                                            {
                                                StoreId = s.StoreId,
                                                Name = s.Name,
                                                IsActive = s.IsActive,
                                                StoreKey = s.StoreKey,
                                                RetailChannelId = s.RetailChannelId
                                            })
                                            .ToList();
                }
            }
        }

        public static void ResetJobsStatus(List<long> jobScheduleIds, int jobStatus)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                foreach (JobSchedule jobSchedule in db.JobSchedule)
                {
                    if (jobScheduleIds.Contains(jobSchedule.JobSchduleId))
                    {
                        jobSchedule.JobStatus = jobStatus;
                        jobSchedule.ModifiedOn = DateTime.UtcNow;
                    }
                }

                db.SaveChanges();
            }
        }

        public static List<JobAndScheduleModel> GetAllActiveJobs(List<StoreDto> stores)
        {
            var storeIds = stores.Select(x => x.StoreId).ToList();
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                return (from job in db.Job
                        join jobschedule in db.JobSchedule on job.JobID equals jobschedule.JobId
                        where job.Enabled == true && jobschedule.IsActive == true
                              && storeIds.Contains(jobschedule.StoreId)
                        select new JobAndScheduleModel
                        {
                            Enabled = job.Enabled,
                            IsActive = jobschedule.IsActive,
                            JobName = job.JobName,
                            JobTypeId = job.JobTypeId,
                            StartTime = jobschedule.StartTime,
                            JobInterval = jobschedule.JobInterval,
                            storeId = jobschedule.StoreId
                        }).ToList();
            }
        }

        public static List<JobAndScheduleModel> GetAllActiveJobsOfType(List<StoreDto> stores, bool jobType)
        {
            var storeIds = stores.Select(x => x.StoreId).ToList();

            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                return (from job in db.Job
                        join jobschedule in db.JobSchedule on job.JobID equals jobschedule.JobId
                        where job.Enabled == true && job.JobTypeId == jobType
                                && jobschedule.IsActive == true
                                && storeIds.Contains(jobschedule.StoreId)
                        select new JobAndScheduleModel
                        {
                            JobID = job.JobID,
                            Enabled = job.Enabled,
                            IsActive = jobschedule.IsActive,
                            JobName = job.JobName,
                            JobTypeId = job.JobTypeId,
                            StartTime = jobschedule.StartTime,
                            JobInterval = jobschedule.JobInterval,
                            storeId = jobschedule.StoreId,
                            JobSchduleId = jobschedule.JobSchduleId
                        }).ToList();
            }
        }

        public static List<Store> GetStoresForSFTPDirStructCreation()
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                return db.Store.Where(s => s.IsSFTPDirTreeCreated == false && s.IsActive).ToList();
            }
        }

        public static void UpdateIsSFTPDirCreatedFlag(int storeId, bool isSFTPDirCreated)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var store = db.Store.Where(s => s.StoreId == storeId).FirstOrDefault();
                store.IsSFTPDirTreeCreated = isSFTPDirCreated;

                db.Entry(store).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }

        public static List<StoreDto> GetAllStores()
        {
            List<Store> listOfStores = new List<Store>();
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                return db.Store
                            .Where(s => s.IsActive)
                            .Select(s => new StoreDto()
                            {
                                StoreId = s.StoreId,
                                Name = s.Name,
                                IsActive = s.IsActive,
                                StoreKey = s.StoreKey,
                                RetailChannelId = s.RetailChannelId
                            })
                            .ToList();
            }
        }

        public static void GetAllActiveStores()
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                listStores = db.Store
                                .Where(s => s.IsActive)
                                .Select(s => new StoreDto()
                                {
                                    StoreId = s.StoreId,
                                    Name = s.Name,
                                    IsActive = s.IsActive,
                                    StoreKey = s.StoreKey,
                                    RetailChannelId = s.RetailChannelId
                                })
                                .ToList();
            }
        }

        public static StoreDto GetByRetailChannelId(string retailChannelId)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                return db.Store
                                .Where(s => s.IsActive
                                         && s.RetailChannelId == retailChannelId)
                                .Select(s => new StoreDto()
                                {
                                    StoreId = s.StoreId,
                                    Name = s.Name,
                                    IsActive = s.IsActive,
                                    StoreKey = s.StoreKey,
                                    RetailChannelId = s.RetailChannelId
                                })
                                .FirstOrDefault();
            }
        }


    }
}
