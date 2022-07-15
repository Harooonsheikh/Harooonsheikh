using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Data.Helper;
using VSI.EDGEAXConnector.Data.ViewModels;

namespace VSI.EDGEAXConnector.Data
{
    public class StoreDAL : BaseClass
    {

        /// <summary>
        /// 
        /// </summary>
        public StoreDAL(string storeKey, int i) : base(storeKey)
        {

        }

        public StoreDAL(string connection)
        {
            this.ConnectionString = connection;
        }


        public StoreDAL(string connection, string storeKey, string user) : base(connection, storeKey, user)
        {

        }

        public List<Store> GetStores()
        {
            List<Store> lstStores = new List<Store>();
            try
            {
                using (var db = this.GetConnection())
                {
                    lstStores = db.Store.ToList();
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
            return lstStores;
        }



        public List<Store> GetActiveStores()
        {
            List<Store> lstStores = new List<Store>();
            try
            {
                using (var db = this.GetConnection())
                {
                    lstStores = db.Store.Where(m => m.IsActive == true).ToList();
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
            return lstStores;
        }

        public Store Get(int storeId)
        {
            Store st = null;
            try
            {
                using (var db = this.GetConnection())
                {
                    st = db.Store.Where(m => m.StoreId == storeId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
            return st;
        }

        public Store GetStoreByKey(string storeKey)
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    var store = db.Store.FirstOrDefault(k => k.StoreKey == storeKey);
                    return store;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public Store GetByRetailChannelId(string retailChannelId)
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                var store = db.Store.FirstOrDefault(k => k.RetailChannelId == retailChannelId);
                return store;
            }
        }

        public bool Disable(int storeId)
        {
            Store store = null;
            try
            {
                using (var db = this.GetConnection())
                {
                    store = db.Store.Where(m => m.StoreId == storeId).FirstOrDefault();

                    if (store != null)
                    {
                        store.IsActive = false;
                        store.ModifiedBy = this.UserId;
                        store.ModifiedOn = System.DateTime.UtcNow;
                        db.Entry(store).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
            return false;
        }
        public bool Verify(string storeName, int storeId)
        {
            Store store = null;
            try
            {
                using (var db = this.GetConnection())
                {
                    if (storeId == -1)
                    {
                        store = db.Store.Where(m => m.Name.Equals(storeName.Trim())).FirstOrDefault();
                    }
                    else
                    {
                        store = db.Store.Where(m => m.Name.Equals(storeName.Trim()) && m.StoreId != storeId).FirstOrDefault();
                    }

                }

                if (store == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }
        public bool VerifyStoreKey(string storeKey, int storeId)
        {
            Store store = null;
            try
            {
                using (var db = this.GetConnection())
                {
                    if (storeId == -1)
                    {
                        store = db.Store.Where(m => m.StoreKey.Equals(storeKey.Trim())).FirstOrDefault();
                    }
                    else
                    {
                        store = db.Store.Where(m => m.StoreKey.Equals(storeKey.Trim()) && m.StoreId != storeId).FirstOrDefault();
                    }

                }

                if (store == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }
        public bool Update(Store store)
        {
            try
            {
                using (var db = this.GetConnection())
                {
                    db.Entry(store).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }
        public bool Add(Store store)
        {
            try
            {
                using (var db = this.GetConnection())
                {
                    db.Store.Add(store);
                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }

        //Duplicate store data for newly added store
        public bool DuplicateStore(int? defaultStore, int storeId, List<KeyValuePair<string, string>> ActiveAppsettings)
        {

            try
            {
                using (var db = this.GetConnection())
                {
                    int index = 0;
                    List<KeyValuePair<string, string>> activeKeys = new List<KeyValuePair<string, string>>();
                    List<ApplicationSetting> lstAppSettings = db.ApplicationSetting.Where(s => s.StoreId == defaultStore).ToList();


                    foreach (var setting in lstAppSettings)
                    {
                        if (index < ActiveAppsettings.Count)
                        {
                            activeKeys = ActiveAppsettings.Where(k => k.Key.ToUpper().Equals(setting.Key.ToUpper())).ToList();

                            if (activeKeys != null && activeKeys.Count > 0)
                            {
                                setting.Value = activeKeys[0].Value;
                                setting.IsUserForDuplicateStore = true;
                                index = index + 1;
                            }
                        }

                        //ApplicationSetting appSetting = StoreHelper.MapAppSettings(setting, storeId);

                        setting.CreatedOn = DateTime.UtcNow;
                        setting.CreatedBy = "System";
                        //setting.IsUserForDuplicateStore = false;
                        setting.StoreId = storeId;
                        db.ApplicationSetting.Add(setting);
                    }

                    List<PaymentMethod> lstPaymentMethods = db.PaymentMethod.Where(s => s.StoreId == defaultStore).ToList();
                    foreach (var setting in lstPaymentMethods)
                    {
                        PaymentMethod paymentMethod = StoreHelper.MapPaymentMethod(setting, storeId);
                        db.PaymentMethod.Add(paymentMethod);
                    }

                    List<ConfigurableObject> lstconfigObjects = db.ConfigurableObject.Where(s => s.StoreId == defaultStore).ToList();
                    foreach (var setting in lstconfigObjects)
                    {
                        ConfigurableObject configObject = StoreHelper.MapConfigObject(setting, storeId);
                        db.ConfigurableObject.Add(configObject);
                    }

                    List<JobSchedule> lstJobSchedules = db.JobSchedule.Where(s => s.StoreId == defaultStore).ToList();
                    foreach (var setting in lstJobSchedules)
                    {
                        JobSchedule jobSchedule = StoreHelper.MapJobSchedule(setting, storeId);
                        db.JobSchedule.Add(jobSchedule);
                    }

                    List<DimensionSet> lstDimensionSets = db.DimensionSet.Where(s => s.StoreId == defaultStore).ToList();
                    foreach (var setting in lstDimensionSets)
                    {
                        DimensionSet dimensionSet = StoreHelper.MapDimensionSet(setting, storeId);
                        db.DimensionSet.Add(dimensionSet);
                    }

                    List<MappingTemplate> lstMappingTemplates = db.MappingTemplate.Where(s => s.StoreId == defaultStore).ToList();
                    foreach (var setting in lstMappingTemplates)
                    {
                        MappingTemplate mappingTemplate = StoreHelper.MapTemplates(setting, storeId);
                        db.MappingTemplate.Add(mappingTemplate);
                    }

                    //List<EmailTemplate> lstEmailTemplates = db.EmailTemplate.Where(s => s.StoreId == defaultStore).ToList();
                    //foreach (var setting in lstEmailTemplates)
                    //{
                    //    EmailTemplate emailTemplate = StoreHelper.MapEmailTemplate(setting, storeId);
                    //    db.EmailTemplate.Add(emailTemplate);
                    //}

                    List<Subscriber> lstSubscriber = db.Subscriber.Where(s => s.StoreId == defaultStore).ToList();
                    foreach (var setting in lstSubscriber)
                    {
                        Subscriber subscriber = StoreHelper.MapSubscriber(setting, storeId);
                        db.Subscriber.Add(subscriber);
                    }

                    List<EmailSubscriber> lstEmailSubscriber = db.EmailSubscriber.Where(s => s.StoreId == defaultStore).ToList();
                    foreach (var setting in lstEmailSubscriber)
                    {
                        EmailSubscriber emailSubscriber = StoreHelper.MapEmailSubscriber(setting, storeId);
                        db.EmailSubscriber.Add(emailSubscriber);
                    }

                    List<EntityFileNameParameter> lstEntityFileNameParameters = db.EntityFileNameParameter.Where(s => s.StoreId == defaultStore).ToList();
                    foreach (var setting in lstEntityFileNameParameters)
                    {
                        EntityFileNameParameter mappingTemplate = StoreHelper.MapEntityFileNameParameter(setting, storeId);
                        db.EntityFileNameParameter.Add(mappingTemplate);
                    }

                    List<DeliveryMethod> lstDeliveryMethod = db.DeliveryMethod.Where(s => s.StoreId == defaultStore).ToList();
                    foreach (var setting in lstDeliveryMethod)
                    {
                        DeliveryMethod deliveryMethod = StoreHelper.MapDeliveryMethod(setting, storeId);
                        db.DeliveryMethod.Add(deliveryMethod);
                    }

                    db.SaveChanges();
                    UpdateParentPaymentMethodId(defaultStore, storeId);
                    UpdateSubscriberIdInEmailSubscriber(defaultStore, storeId);
                    //UpdateTemplateIdInEmailSubscriber(defaultStore, storeId);
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }

        public bool Delete(int storeId)
        {
            try
            {
                using (var db = this.GetConnection())
                {
                    var store = db.Store.Where(s => s.StoreId == storeId).FirstOrDefault();
                    if (store != null)
                    {
                        db.Store.Remove(store);
                        db.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }

        //Update ParentPaymentMethodIds after replicating PaymentMethod table data for newly added store
        public void UpdateParentPaymentMethodId(int? defaultStore, int storeId)
        {
            List<PaymentMethod> lstPaymentMethods = null;
            string parentPaymentMethod = string.Empty;
            int parentPaymentMethodId = 0;
            using (var db = this.GetConnection())
            {
                lstPaymentMethods = db.PaymentMethod.Where(s => s.StoreId == storeId).ToList();
                foreach (var paymentMethod in lstPaymentMethods)
                {
                    if (paymentMethod.ParentPaymentMethodId != null)
                    {
                        parentPaymentMethod = db.PaymentMethod.Where(s => s.StoreId == defaultStore && s.PaymentMethodId == paymentMethod.ParentPaymentMethodId).FirstOrDefault().ErpCode;
                        parentPaymentMethodId = lstPaymentMethods.Find(s => s.ErpCode == parentPaymentMethod).PaymentMethodId;
                        paymentMethod.ParentPaymentMethodId = parentPaymentMethodId;
                        db.Entry(paymentMethod).State = System.Data.Entity.EntityState.Modified;
                    }
                }

                db.SaveChanges();
            }
        }

        //Update subscriberId after replicating EmailSubscriber table data for newly added store
        public void UpdateSubscriberIdInEmailSubscriber(int? defaultStore, int storeId)
        {
            List<EmailSubscriber> lstEmailSubscriber = null;
            List<Subscriber> lstSubscriber = null;
            int subscriberId = -1;
            using (var db = this.GetConnection())
            {
                lstEmailSubscriber = db.EmailSubscriber.Where(s => s.StoreId == storeId).ToList();
                lstSubscriber = db.Subscriber.Where(s => s.StoreId == storeId || s.StoreId == defaultStore).ToList();
                foreach (var emailSubscriber in lstEmailSubscriber)
                {
                    string subscriberEmail = lstSubscriber.Where(s => s.StoreId == defaultStore && s.SubscriberId == emailSubscriber.SubscriberId).FirstOrDefault().Email;
                    subscriberId = lstSubscriber.Find(s => s.Email == subscriberEmail && s.StoreId == storeId).SubscriberId;

                    emailSubscriber.SubscriberId = subscriberId;
                    db.Entry(emailSubscriber).State = System.Data.Entity.EntityState.Modified;

                }

                db.SaveChanges();
            }
        }

        //Update TemplateId after replicating EmailSubscriber table data for newly added store
        public void UpdateTemplateIdInEmailSubscriber(int? defaultStore, int storeId)
        {
            List<EmailSubscriber> lstEmailSubscriber = null;
            List<EmailTemplate> lstEmailTemplate = null;
            int templateId = -1;
            using (var db = this.GetConnection())
            {
                lstEmailSubscriber = db.EmailSubscriber.Where(s => s.StoreId == storeId).ToList();
                lstEmailTemplate = db.EmailTemplate.Where(s => s.StoreId == storeId || s.StoreId == defaultStore).ToList();
                foreach (var emailSubscriber in lstEmailSubscriber)
                {
                    string templateName = lstEmailTemplate.Where(s => s.StoreId == defaultStore && s.EmailTemplateId == emailSubscriber.TemplateId).FirstOrDefault().Name;
                    templateId = lstEmailTemplate.Find(s => s.Name == templateName && s.StoreId == storeId).EmailTemplateId;

                    emailSubscriber.TemplateId = templateId;
                    db.Entry(emailSubscriber).State = System.Data.Entity.EntityState.Modified;

                }

                db.SaveChanges();
            }
        }
        public string StoreKeyOfCountry(string countryCode)
        {
            try
            {
                using (var db = this.GetConnection())
                {
                    var Id = db.CountryNames.Where(x => x.ThreeLetterRegion.Contains(countryCode.Trim())).Select(x => x).FirstOrDefault();
                    if (Id == null)
                    {
                        return "Country Not Found " + countryCode;
                    }
                    return Id.Store.StoreKey;
                };
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }
        public List<Countries> CountriesList()
        {
            try
            {
                using (var db = this.GetConnection())
                {
                    List<CountryNames> countryNames = new List<CountryNames>();
                    List<Countries> countries = new List<Countries>();
                    countryNames = db.CountryNames.ToList();
                    foreach (var dbCountry in countryNames)
                    {
                        Countries country = new Countries();
                        country.CountryName = dbCountry.CountryName;
                        country.CountryCode = dbCountry.ThreeLetterRegion;
                        country.Description = dbCountry.Description;
                        countries.Add(country);
                    }
                    return countries;
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }
    }
}
