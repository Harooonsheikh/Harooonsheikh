using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;

namespace VSI.EDGEAXConnector.Configuration
{
    public class ConfigurationHelper
    {

        #region Public Properites

        private List<Setting> settings;
        string Local_Base_Path = "";
        string Remote_Base_Path = "/files";
        public StoreDto currentStore;

        private static ConcurrentDictionary<string, ConfigurationHelper> configurationHelpers = new ConcurrentDictionary<string, ConfigurationHelper>();

        public IEnumerable<DimensionSet> DimensionSets { get; set; }
        #endregion

        #region Constructor
        public ConfigurationHelper(string storeKey)
        {
            settings = SettingManager.GetSettings(storeKey);
            SetStore(storeKey);
            Local_Base_Path = GetSetting(APPLICATION.Local_Base_Path);
            Remote_Base_Path = GetSetting(APPLICATION.Remote_Base_Path);
        }
        #endregion

        #region Public Functions

        public static void LoadAllConfigurations()
        {
            SettingManager.GetAllSettings();
        }
        public string GetDirectory(string pathToFind)
        {
            if (!System.IO.Directory.Exists(pathToFind))
            {
                return System.IO.Directory.CreateDirectory(pathToFind).FullName;
            }
            else
            {
                return pathToFind;
            }
        }
        public string GetSetting(ECOM key)
        {
            return GetSettingsByEnum(GROUP.ECOM, key);
        }
        public string GetSetting(NOTIFICATION key)
        {
            return GetSettingsByEnum(GROUP.NOTIFICATION, key);
        }
        public string GetSetting(STORE key)
        {
            return GetSettingsByEnum(GROUP.STORE, key);
        }
        public string GetSetting(ADDRESS key)
        {
            return GetSettingsByEnum(GROUP.ADDRESS, key);
        }
        public string GetSetting(SALESORDER key)
        {
            return GetSettingsByEnum(GROUP.SALESORDER, key);
        }
        public string GetSetting(INVENTORY key)
        {
            return GetSettingsByEnum(GROUP.INVENTORY, key);
        }
        public string GetSetting(APPLICATION key)
        {
            return GetSettingsByEnum(GROUP.APPLICATION, key);
        }
        public string GetSetting(PRODUCT key)
        {
            return GetSettingsByEnum(GROUP.PRODUCT, key);
        }
        public string GetSetting(CUSTOMER key)
        {
            return GetSettingsByEnum(GROUP.CUSTOMER, key);
        }
        public string GetSetting(PRICE key)
        {
            return GetSettingsByEnum(GROUP.PRICE, key);
        }
        public string GetSetting(DISCOUNT key)
        {
            return GetSettingsByEnum(GROUP.DISCOUNT, key);
        }
        public string GetSetting(DISCOUNTWITHAFFILIATION key)
        {
            return GetSettingsByEnum(GROUP.DISCOUNTWITHAFFILIATION, key);
        }
        public string GetSetting(QUANTITYDISCOUNT key)
        {
            return GetSettingsByEnum(GROUP.QUANTITYDISCOUNT, key);
        }
        public string GetSetting(QUANTITYDISCOUNTWITHAFFILIATION key)
        {
            return GetSettingsByEnum(GROUP.QUANTITYDISCOUNTWITHAFFILIATION, key);
        }
        public string GetSetting(CHANNELCONFIGURATION key)
        {
            return GetSettingsByEnum(GROUP.CHANNELCONFIGURATION, key);
        }
        public string GetSetting(OFFERTYPEGROUPS key)
        {
            return GetSettingsByEnum(GROUP.OFFERTYPEGROUPS, key);
        }
        public string GetSetting(QUOTATIONREASONGROUP key)
        {
            return GetSettingsByEnum(GROUP.QUOTATIONREASONGROUP, key);
        }
        public string GetSetting(EXTERNALWEBAPI key)
        {
            return GetSettingsByEnum(GROUP.EXTERNALWEBAPI, key);
        }
        public string GetSetting(PAYMENTCONNECTOR key)
        {
            return GetSettingsByEnum(GROUP.PAYMENTCONNECTOR, key);
        }
        public string GetSetting(INGRAM key)
        {
            return GetSettingsByEnum(GROUP.INGRAM, key);
        }
        string GetSettingsByEnum(GROUP group, Enum key)
        {
            string value = string.Empty;

            var defaultObject = settings.FirstOrDefault(x =>
                   (x.Key.Equals(key.ToString(), StringComparison.CurrentCultureIgnoreCase))
                   && (x.Group.Equals(group) && x.StoreId == currentStore.StoreId));

            if (defaultObject != null)
            {
                value = defaultObject.Value;
            }
            if (value.Contains("{APPLICATION."))
            {
                value = value.Replace("{APPLICATION.Local_Base_Path}", Local_Base_Path)
                             .Replace("{APPLICATION.Remote_Base_Path}", Remote_Base_Path);
            }
            return value;
        }
        public void SetStore(string storeKey)
        {
            this.currentStore = StoreService.GetStoreByKey(storeKey);
        }

        public static ConfigurationHelper GetConfigurationHelperInstanceByStore(string storeKey)
        {
            bool isRefreshCache = SettingManager.GetCacheSetting(storeKey);
            if (configurationHelpers.ContainsKey(storeKey) && !isRefreshCache)
            {
                return configurationHelpers.FirstOrDefault(x => x.Key == storeKey).Value;
            }
            
            var configurationHelper = new ConfigurationHelper(storeKey);
            configurationHelpers.AddOrUpdate(storeKey, configurationHelper, (key, oldValue) => configurationHelper);
            return configurationHelper;
        }

        #endregion
    }


    #region Member Classes

    public static class UtilityMethods
    {
        public static bool BoolValue(this string s)
        {
            return Convert.ToBoolean(s);
        }
        public static Int32 IntValue(this string s)
        {
            return Convert.ToInt32(s);
        }
        public static long LongValue(this string s)
        {
            return Convert.ToInt64(s);
        }
    }
    class SettingManager
    {
        static Dictionary<string, List<Setting>> Settings { get; set; }
        static SettingManager()
        {
            if (Settings == null)
            {
                Settings = new Dictionary<string, List<Setting>>();
            }
        }
        public static List<Setting> GetSettings(string storeKey)
        {
            List<Setting> storeSettings = null;
            bool refreshCache = GetCacheSetting(storeKey);

            if (!refreshCache && Settings.ContainsKey(storeKey))
            {
                storeSettings = Settings[storeKey];
            }
            else
            {
                lock (Settings)
                {
                    if (!Settings.ContainsKey(storeKey) || refreshCache)
                    {
                        storeSettings = GetSettingData(storeKey);
                        Settings.Remove(storeKey);
                        Settings.Add(storeKey, storeSettings);
                        if (refreshCache)
                        {
                            ResetCacheSetting(storeKey);
                        }
                    }
                    else
                    {
                        storeSettings = Settings[storeKey];
                    }
                }
            }

            return storeSettings;
        }

        public static void GetAllSettings()
        {
            var applicationSettings = ApplicationSettingsDAL.GetAllApplicationSettingsForStores();
            var settings = new List<Setting>();
            foreach (var appSetting in applicationSettings)
            {
                var kp = appSetting.Key.Split('.');
                if (kp.Count() > 1)
                {
                    settings.Add(new Setting
                    {
                        Group = (GROUP)Enum.Parse(typeof(GROUP), kp[0]),
                        Key = kp[1],
                        Value = appSetting.Value,
                        StoreId = appSetting.StoreId,
                        StoreKey = StoreService.GetStoreById(appSetting.StoreId).StoreKey,
                        IsActive = appSetting.IsActive
                    });
                }
            }

            Settings = settings.GroupBy(c => c.StoreKey).ToDictionary(k => k.Key, v => v.ToList());
        }

        private static List<Setting> GetSettingData(string storeKey)
        {
            var settings = new List<Setting>();
            var dal = new ApplicationSettingsDAL(storeKey);
            var appSettings = dal.GetAllApplicationSettings();

            foreach (var appSetting in appSettings)
            {
                var kp = appSetting.Key.Split('.');
                if (kp.Count() > 1)
                {
                    settings.Add(new Setting
                    {
                        Group = (GROUP)Enum.Parse(typeof(GROUP), kp[0]),
                        Key = kp[1],
                        Value = appSetting.Value,
                        StoreId = appSetting.StoreId,
                        StoreKey = storeKey,
                        IsActive = appSetting.IsActive
                    });
                }
            }

            return settings;
        }

        public static Boolean GetCacheSetting(string storeKey)
        {
            ApplicationSettingsDAL objAppSettingsDAL = new ApplicationSettingsDAL(storeKey);
            var appSetting = objAppSettingsDAL.GetCacheSetting();
            bool.TryParse(appSetting?.Value, out bool isCacheRefresh);
            return isCacheRefresh;
        }

        private static void ResetCacheSetting(string storeKey)
        {
            ApplicationSettingsDAL appSettingsDal = new ApplicationSettingsDAL(storeKey);
            appSettingsDal.ResetCacheSetting();
        }
    }
    class DimensionSetManager
    {
        static Dictionary<string, List<DimensionSet>> DimensionSets { get; set; }
        static DimensionSetManager()
        {
            if (DimensionSets == null)
            {
                DimensionSets = new Dictionary<string, List<DimensionSet>>();
            }
        }
        public static List<DimensionSet> GetDimensionSets(string storeKey)
        {
            List<DimensionSet> storeDimensionSets = null;

            if (DimensionSets.ContainsKey(storeKey))
            {
                storeDimensionSets = DimensionSets[storeKey];
            }
            else //if(storeSettings == null)
            {
                lock (DimensionSets)
                {
                    if (!DimensionSets.ContainsKey(storeKey))
                    {
                        storeDimensionSets = GetDimensionSetData(storeKey);
                        DimensionSets.Add(storeKey, storeDimensionSets);
                    }
                    else
                    {
                        storeDimensionSets = DimensionSets[storeKey];
                    }
                }
            }

            return storeDimensionSets;
        }
        private static List<DimensionSet> GetDimensionSetData(string storeKey)
        {
            DimensionSetDAL objDimensionSetDAL = new DimensionSetDAL(storeKey);
            return objDimensionSetDAL.GetAllDimensionSets();
        }
    }
    class Setting
    {
        public GROUP Group { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public string ScreenName { get; set; }
        public bool Required { get; set; }
        public string Datatype { get; set; }
        public string Value { get; set; }
        public bool IsActive { get; set; }
        public bool boolValue { get; set; }
        public int intValue { get; set; }
        public long longValue { get; set; }
        public Enum AcceptableValues { get; set; }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Setting other = obj as Setting;
            if (Key != null && other.Key != null && Key == other.Key
                && Group == other.Group)
            {
                return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Group.GetHashCode() ^ Key.GetHashCode();
        }
        public int StoreId { get; set; }
        public string StoreKey { get; set; }
    }

    #endregion
}
