using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data.ViewModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class EmailTemplateDAL :  BaseClass
    {
        public EmailTemplateDAL(string storeKey) : base(storeKey)
        {
                
        }

        public EmailTemplateDAL(string connectionString, string storeKey, string user) : base(connectionString, storeKey, user)
        {

        }

        public List<EmailTemplate> GetAllTemplates()
        {
            List<EmailTemplate> lstTemplates = new List<EmailTemplate>();

            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    lstTemplates = db.EmailTemplate.Where(x => x.StoreId == StoreId).ToList();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstTemplates;
        }

        public bool AddTemplate(EmailTemplate temp)
        {
            using (var db = this.GetConnection())
            {
                try
                {
                    var time = DateTime.UtcNow;
                    temp.CreatedOn = time;
                    temp.ModifiedOn = time;
                    temp.CreatedBy = UserId;
                    temp.StoreId = StoreId;

                    db.EmailTemplate.Add(temp);
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
        public bool UpdateTemplate(EmailTemplate temp)
        {
            using (var db = this.GetConnection())
            {
                try
                {
                    var tempObj = db.EmailTemplate.Where(s => s.EmailTemplateId == temp.EmailTemplateId && temp.StoreId == StoreId).FirstOrDefault();
                    tempObj.Name = temp.Name;
                    tempObj.Subject = temp.Subject;
                    tempObj.Body = temp.Body;
                    tempObj.Footer = temp.Footer;
                    tempObj.IsActive = temp.IsActive;
                    tempObj.StoreId = StoreId;
                    tempObj.ModifiedBy = UserId;
                    tempObj.ModifiedOn = DateTime.UtcNow;
                    db.Entry(tempObj).State = System.Data.Entity.EntityState.Modified;
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

        public string DeleteTemplate(int templateToDelete)
        {
            IntegrationDBEntities db = this.GetConnection();
            using (db)
            {
                try
                {
                    var emailSubscription = db.EmailSubscriber.Where(m=>m.StoreId == this.StoreId).Where(es => es.TemplateId == templateToDelete).FirstOrDefault();
                    if (emailSubscription == null)
                    {
                        var emailTemplate = db.EmailTemplate.Where(et => et.EmailTemplateId== templateToDelete && et.StoreId == this.StoreId).FirstOrDefault();
                        if (emailTemplate != null)
                        {
                            db.EmailTemplate.Remove(emailTemplate);
                            db.SaveChanges();
                            return "Success";
                        }
                        return "NotFound";
                    }
                    return "Subscribed";

                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    return "Failure";
                }
            }
        }


        public bool UpdateTemplate(EmailTemplateVM temp)
        {
            IntegrationDBEntities db = this.GetConnection();
            using (db)
            {
                try
                {
                    //EmailTemplate tempObj = db.EmailTemplates.Where(s => s.Id == temp.Id).FirstOrDefault();
                    EmailTemplate tempObj = new EmailTemplate();
                    tempObj = db.EmailTemplate.Where(s => (s.EmailTemplateId == temp.Id)&&(s.StoreId == this.StoreId)).FirstOrDefault();
                    tempObj.Name = temp.Name;
                    tempObj.Subject = temp.Subject;
                    tempObj.Body = temp.Body;
                    tempObj.Footer = temp.Footer;
                    tempObj.IsActive = temp.IsActive;
                    tempObj.ModifiedBy = this.UserId;
                    tempObj.ModifiedOn = DateTime.UtcNow;
                    tempObj.StoreId = this.StoreId;
                    db.Entry(tempObj).State = System.Data.Entity.EntityState.Modified;
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
        public EmailTemplate AddTemplate(EmailTemplateVM template)
        {
            EmailTemplate temp = new EmailTemplate();
            temp.Name = template.Name;
            temp.Subject = template.Subject;
            temp.Body = template.Body;
            temp.Footer = template.Footer;
            temp.IsActive = template.IsActive;
            temp.StoreId = template.StoreId;
            temp.CreatedBy = this.UserId;
            temp.CreatedOn = DateTime.UtcNow;
            temp.ModifiedBy = this.UserId;
            temp.ModifiedOn = DateTime.UtcNow;

            IntegrationDBEntities db = this.GetConnection();
            using (db)
            {
                try
                {
                    db.EmailTemplate.Add(temp);
                    db.SaveChanges();
                    return temp;
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    return null;
                }
            }
        }

        public bool DeleteTemplate(EmailTemplate temp)
        {
            using (var db = this.GetConnection())
            {
                try
                {
                    var tempObj = db.EmailTemplate.Where(s => s.EmailTemplateId == temp.EmailTemplateId).FirstOrDefault();
                    db.EmailTemplate.Remove(tempObj);
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
