using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;

namespace VSI.EDGEAXConnector.Data
{
    public class ApplicationSettingsDAL : BaseClass
    {
        public ApplicationSettingsDAL(string storeKey) : base(storeKey)
        { }

        public List<ApplicationSettingDto> GetAllApplicationSettings()
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                return db.ApplicationSetting
                            .Where(a => a.StoreId == StoreId)
                            .Select(a => new ApplicationSettingDto
                            {
                                Key = a.Key,
                                Value = a.Value,
                                StoreId = a.StoreId,
                                IsActive = a.IsActive
                            })
                            .ToList();
            }
        }
        public static List<ApplicationSettingDto> GetAllApplicationSettingsForStores()
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                return (from applicationsetting in db.ApplicationSetting
                        join store in db.Store on applicationsetting.StoreId equals store.StoreId
                        where store.IsActive 
                        select new ApplicationSettingDto
                        {
                            Key = applicationsetting.Key,
                            Value = applicationsetting.Value,
                            StoreId = applicationsetting.StoreId,
                            IsActive = applicationsetting.IsActive
                        }).ToList();
            }
        }
        public List<ApplicationSetting> GetApplicationSettingsByScreenName(string screenName)
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                return db.ApplicationSetting
                            .Where(appS => appS.StoreId == StoreId
                                            && appS.ScreenName == screenName
                                            && appS.IsActive)
                            .Include(m => m.FieldType)
                            .OrderBy(ap => ap.SortOrder)
                            .ToList();
            }
        }

        public List<ApplicationSetting> GetApplicationSettings(string sectionName, string subSection)
        {

            using (var db = this.GetConnection())
            {
                return db.ApplicationSetting
                            .Where(appS => appS.StoreId == StoreId
                                        && appS.Key.StartsWith(sectionName)
                                        && (appS.Key.Contains(subSection)))
                            .OrderBy(ap => ap.SortOrder)
                            .ToList();
            }
        }

        public ApplicationSetting GetApplicationSetting(int settingId)
        {
            using (var db = this.GetConnection())
            {
                return db.ApplicationSetting
                            .Where(appS => appS.StoreId == StoreId
                                        && appS.ApplicationSettingId == settingId)
                            .FirstOrDefault();
            }
        }

        public List<ApplicationSetting> GetApplicationSettingsByIsUsedForDuplicateStore(int storeId, bool IsUsedForDuplicateStore)
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                return db.ApplicationSetting
                            .Where(a => a.StoreId == storeId
                                     && a.IsActive
                                     && a.IsUserForDuplicateStore == IsUsedForDuplicateStore)
                            .Include(m => m.FieldType)
                            .OrderBy(ap => ap.SortOrder)
                            .ToList();
            }
        }

        public List<string> GetScreenNames()
        {
            var screenList = new List<string>();
            using (var db = this.GetConnection())
            {
                var screenNames = db.ApplicationSetting
                                    .Where(appS => appS.StoreId == StoreId)
                                    .Select(m => m.ScreenName)
                                    .Distinct()
                                    .ToList();

                foreach (var screen in screenNames)
                {
                    var appSetting = db.ApplicationSetting
                                        .Where(s => s.StoreId == this.StoreId
                                                 && s.ScreenName == screen
                                                 && s.IsActive)
                                        .FirstOrDefault();

                    if (appSetting != null)
                    {
                        screenList.Add(screen);
                    }
                }

                return screenList;
            }
        }


        public bool UpdateApplicationSettings(List<ApplicationSetting> lstApps)
        {
            try
            {
                foreach (var apps in lstApps)
                {
                    using (var db = this.GetConnection())
                    {
                        var appsObj = db.ApplicationSetting
                                        .Where(m => m.StoreId == StoreId)
                                        .FirstOrDefault(a => a.ApplicationSettingId == apps.ApplicationSettingId);

                        if (appsObj != null)
                        {
                            appsObj.Value = apps.Value;
                            apps.ModifiedOn = DateTime.UtcNow;
                            apps.ModifiedBy = this.UserId;

                            db.Entry(appsObj).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            throw new CommerceLinkError("Application setting not found for key = " + apps.Key);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                return false;
            }
        }

        public int GetStoreId(string threeLetterISORegionName)
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                // Using  k.Value.Contains instead of  k.Value.Equals to tackle grouped stores
                var appSetting = db.ApplicationSetting
                                    .Where(k => k.Key.Equals("CUSTOMER.Default_ThreeLetterISORegionName")
                                             && k.Value.Contains(threeLetterISORegionName))
                                    .FirstOrDefault();

                if (appSetting == null)
                {
                    throw new CommerceLinkError("Store not found with ISO region '" + threeLetterISORegionName + "'");
                }

                return appSetting.StoreId;
            }
        }

        public ApplicationSetting GetCacheSetting()
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                return db.ApplicationSetting
                            .FirstOrDefault(k => k.StoreId == StoreId
                                            && k.Key.Equals("STORE.DISABLE_CACHE"));
            }
        }
        public int ResetCacheSetting()
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                var appSetting = db.ApplicationSetting
                                    .FirstOrDefault(k => k.StoreId == StoreId
                                                      && k.Key.Equals("STORE.DISABLE_CACHE"));

                if (appSetting != null)
                {
                    appSetting.Value = false.ToString();
                    appSetting.ModifiedOn = DateTime.UtcNow;
                    appSetting.ModifiedBy = "CacheRefresh";

                    db.Entry(appSetting).State = EntityState.Modified;
                    return db.SaveChanges();
                }

                return 0;
            }
        }
    }
}
