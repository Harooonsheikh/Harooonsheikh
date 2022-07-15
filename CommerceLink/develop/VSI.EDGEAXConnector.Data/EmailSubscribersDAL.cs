using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data.ViewModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class EmailSubscribersDAL : BaseClass
    {
        public EmailSubscribersDAL(string storeKey) : base(storeKey)
        {

        }
        public EmailSubscribersDAL(string connectionString, string storeKey, string user) : base(connectionString, storeKey, user)
        {

        }
        public List<Subscriber> GetAllSubscriber()
        {
            List<Subscriber> lstSubscriber = new List<Subscriber>();

            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    lstSubscriber = db.Subscriber.Where(x => x.StoreId == StoreId).ToList();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstSubscriber;
        }

        public List<SubscriberVM> GetAllSubscriberVM()
        {
            List<SubscriberVM> lstSubscriber = new List<SubscriberVM>();

            IntegrationDBEntities db = this.GetConnection();
            using (db)
            {
                try
                {
                    var subscribers = db.Subscriber.Where(e => e.StoreId == this.StoreId).ToList();
                    foreach (var sub in subscribers)
                    {
                        SubscriberVM subscriber = new SubscriberVM();
                        var emailSub = db.EmailSubscriber.Where(es => es.SubscriberId == sub.SubscriberId).ToList();

                        subscriber.Id = sub.SubscriberId;
                        subscriber.Email = sub.Email;
                        subscriber.Name = sub.Name;
                        subscriber.IsActive = sub.IsActive;
                        subscriber.StoreId_FK = sub.StoreId;

                        foreach (var esub in emailSub)
                        {
                            var emailSubscriber = new EmailSubscriberVM();
                            emailSubscriber.TemplateId = esub.TemplateId;
                            emailSubscriber.SubscriberId = esub.SubscriberId;
                            subscriber.EmailSubscribers.Add(emailSubscriber);
                        }


                        lstSubscriber.Add(subscriber);
                    }

                }

                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstSubscriber;
        }

        public List<EmailSubscriber> GetEmailSubscribtionByID(int subscriberId)
        {
            List<EmailSubscriber> lstEmailSubscriber = new List<EmailSubscriber>();

            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    lstEmailSubscriber = db.EmailSubscriber.Where(s => s.SubscriberId == subscriberId && s.StoreId == StoreId).ToList();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstEmailSubscriber;
        }
        public bool AddSubscribers(Subscriber subs, string selectedTempaltes)
        {
            using (var db = this.GetConnection())
            {
                try
                {
                    db.Subscriber.Add(subs);
                    db.SaveChanges();

                    foreach (var tempId in selectedTempaltes.Split(','))
                    {
                        if (tempId != "")
                        {
                            EmailSubscriber emailSubObj = new EmailSubscriber();
                            emailSubObj.TemplateId = Convert.ToInt32(tempId);
                            emailSubObj.SubscriberId = subs.SubscriberId;
                            emailSubObj.StoreId = StoreId;
                            emailSubObj.CreatedOn= DateTime.UtcNow;
                            emailSubObj.CreatedBy = UserId;

                            db.EmailSubscriber.Add(emailSubObj);
                        }
                    }
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
        public bool UpdateSubscriber(Subscriber subs, string selectedTempaltes)
        {
            using (var db = this.GetConnection())
            {
                try
                {
                    var subObj = db.Subscriber.Where(s => s.SubscriberId == subs.SubscriberId).FirstOrDefault();
                    subObj.Name = subs.Name;
                    subObj.Email = subs.Email;
                    subObj.IsActive = subs.IsActive;
                    subObj.ModifiedOn = subs.ModifiedOn;
                    subObj.StoreId = StoreId;
                    subObj.CreatedBy = UserId;

                    db.Entry(subObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var emailSubs = db.EmailSubscriber.Where(es => es.SubscriberId == subs.SubscriberId).ToList();
                    foreach (EmailSubscriber es in emailSubs)
                    {

                        db.EmailSubscriber.Remove(es);
                    }
                    db.SaveChanges();

                    foreach (var tempId in selectedTempaltes.Split(','))
                    {
                        if (tempId != "")
                        {
                            EmailSubscriber emailSubObj = new EmailSubscriber();
                            emailSubObj.TemplateId = Convert.ToInt32(tempId);
                            emailSubObj.SubscriberId = subs.SubscriberId;
                            emailSubObj.StoreId = StoreId;
                            emailSubObj.CreatedOn = DateTime.UtcNow;
                            emailSubObj.CreatedBy = UserId;

                            db.EmailSubscriber.Add(emailSubObj);
                        }
                    }
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
        public bool DeleteSubscriber(Subscriber subs)
        {
            using (var db = this.GetConnection())
            {
                try
                {
                    var emailSubs = db.EmailSubscriber.Where(es => es.SubscriberId == subs.SubscriberId).ToList();
                    foreach (EmailSubscriber es in emailSubs)
                    {
                        db.EmailSubscriber.Remove(es);
                    }
                    db.SaveChanges();


                    var subObj = db.Subscriber.Where(s => s.SubscriberId == subs.SubscriberId).FirstOrDefault();
                    db.Subscriber.Remove(subObj);
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

        public bool UpdateSubscriber(SubscriberVM subs)
        {
            IntegrationDBEntities db = this.GetConnection();
            using (db)
            {
                try
                {
                    var subObj = db.Subscriber.Where(s => s.SubscriberId == subs.Id).FirstOrDefault();
                    subObj.Name = subs.Name;
                    subObj.Email = subs.Email;
                    subObj.IsActive = subs.IsActive;
                    subObj.ModifiedOn = DateTime.UtcNow;
                    subObj.ModifiedBy = this.UserId;
                    db.Entry(subObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var emailSubs = db.EmailSubscriber.Where(es => es.SubscriberId == subs.Id).ToList();
                    foreach (EmailSubscriber es in emailSubs)
                    {

                        db.EmailSubscriber.Remove(es);
                    }
                    db.SaveChanges();
                    foreach (var temp in subs.EmailSubscribers)
                    {
                        EmailSubscriber emailSubObj = new EmailSubscriber();
                        emailSubObj.TemplateId = temp.TemplateId;
                        emailSubObj.SubscriberId = temp.SubscriberId;
                        emailSubObj.StoreId = this.StoreId;
                        emailSubObj.CreatedBy = this.UserId;
                        emailSubObj.CreatedOn = DateTime.UtcNow;
                        db.EmailSubscriber.Add(emailSubObj);
                    }
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
        public SubscriberVM AddSubscriber(SubscriberVM sub)
        {
            IntegrationDBEntities db = this.GetConnection();
            Subscriber subscriber = new Subscriber();
            subscriber.Email = sub.Email;
            subscriber.Name = sub.Name;
            subscriber.IsActive = sub.IsActive;
            subscriber.StoreId = this.StoreId;
            subscriber.CreatedBy = this.UserId;
            subscriber.CreatedOn = DateTime.UtcNow;
            using (db)
            {
                try
                {
                    db.Subscriber.Add(subscriber);
                    db.SaveChanges();

                    foreach (var temp in sub.EmailSubscribers)
                    {
                        EmailSubscriber emailSubObj = new EmailSubscriber();
                        emailSubObj.TemplateId = temp.TemplateId;
                        emailSubObj.SubscriberId = subscriber.SubscriberId;
                        emailSubObj.StoreId = this.StoreId;
                        emailSubObj.CreatedBy = this.UserId;
                        emailSubObj.CreatedOn = DateTime.UtcNow;
                        db.EmailSubscriber.Add(emailSubObj);
                    }
                    db.SaveChanges();
                    sub.Id = subscriber.SubscriberId;
                    return sub;
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    throw ex;
                }
            }
        }

        public bool DeleteSubscriber(SubscriberVM subs)
        {
            IntegrationDBEntities db = this.GetConnection();
            using (db)
            {
                try
                {
                    var emailSubs = db.EmailSubscriber.Where(es => es.SubscriberId == subs.Id).ToList();
                    foreach (EmailSubscriber es in emailSubs)
                    {
                        db.EmailSubscriber.Remove(es);
                    }
                    db.SaveChanges();

                    var subObj = db.Subscriber.Where(s => s.SubscriberId == subs.Id).FirstOrDefault();
                    db.Subscriber.Remove(subObj);
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
    }
}
