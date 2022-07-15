using System;
using System.Collections.Generic;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.ViewModels;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    public class EmailSettingsController : ApiBaseController
    {
        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetEmailTemplates()
        {
            EmailTemplateDAL emailTemplateDAL = null;
            List<EmailTemplateVM> lstEmailTemplate = null;
            try
            {
                emailTemplateDAL = new EmailTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                lstEmailTemplate = new List<EmailTemplateVM>();
                emailTemplateDAL.GetAllTemplates().ForEach(m =>
                {
                    EmailTemplateVM emailTemplate = new EmailTemplateVM();
                    emailTemplate.Id = m.EmailTemplateId;
                    emailTemplate.Name = m.Name;
                    emailTemplate.Subject = m.Subject;
                    emailTemplate.Body = m.Body;
                    emailTemplate.Footer = m.Footer;
                    emailTemplate.IsActive = m.IsActive;
                    emailTemplate.StoreId = m.StoreId;
                    lstEmailTemplate.Add(emailTemplate);
                });
                return Ok(lstEmailTemplate);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult DeleteEmailTemplate(int templateToDelete)
        {
            EmailTemplateDAL emailTemplateDAL = null;
            try
            {
                emailTemplateDAL = new EmailTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                string result = emailTemplateDAL.DeleteTemplate(templateToDelete);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult UpdateEmailTemplate(EmailTemplateVM emailTemplate)
        {
            EmailTemplateDAL emailTemplateDAL = null;
            try
            {
                emailTemplateDAL = new EmailTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                emailTemplate.ModifiedOn = DateTime.UtcNow;
                emailTemplate.ModifiedBy = emailTemplate.ModifiedBy;
                bool result = emailTemplateDAL.UpdateTemplate(emailTemplate);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult AddEmailTemplate(EmailTemplateVM emailTemplate)
        {
            EmailTemplateDAL emailTemplateDAL = null;
            try
            {
                emailTemplateDAL = new EmailTemplateDAL(this.DbConnStr, this.StoreKey, this.User);
                EmailTemplate emailTemp = emailTemplateDAL.AddTemplate(emailTemplate);
                if (emailTemp != null)
                {
                    return Ok(MapTemplate(emailTemp));
                }
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetSubscribers()
        {
            EmailSubscribersDAL emailSubscriberDAL = null;
            try
            {
                emailSubscriberDAL = new EmailSubscribersDAL(this.DbConnStr, this.StoreKey, this.User);
                List<SubscriberVM> lstEmailSubscriber = emailSubscriberDAL.GetAllSubscriberVM();
                return Ok(lstEmailSubscriber);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult UpdateSubscriber(SubscriberVM subscriber)
        {
            EmailSubscribersDAL emailSubscriberDAL = null;
            try
            {
                emailSubscriberDAL = new EmailSubscribersDAL(this.DbConnStr, this.StoreKey, this.User);
                subscriber.ModifiedAt = DateTime.UtcNow;
                subscriber.ModifiedByUser = subscriber.ModifiedByUser;
                bool result = emailSubscriberDAL.UpdateSubscriber(subscriber);
                if (result)
                {
                    return Ok();
                }
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult AddSubscriber(SubscriberVM subscriber)
        {
            EmailSubscribersDAL emailSubscriberDAL = null;
            try
            {
                emailSubscriberDAL = new EmailSubscribersDAL(this.DbConnStr, this.StoreKey, this.User);
                subscriber.CreatedAt = DateTime.UtcNow;
                subscriber.CreatedByUser = subscriber.CreatedByUser;
                SubscriberVM sub = emailSubscriberDAL.AddSubscriber(subscriber);
                if (sub != null)
                {
                    return Ok(sub);
                }
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult DeleteSubscriber(SubscriberVM subscriber)
        {
            EmailSubscribersDAL emailSubscriberDAL = null;
            try
            {
                emailSubscriberDAL = new EmailSubscribersDAL(this.DbConnStr, this.StoreKey, this.User);
                bool result = emailSubscriberDAL.DeleteSubscriber(subscriber);
                if (result)
                {
                    return Ok();
                }
                return InternalServerError();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private EmailTemplateVM MapTemplate(EmailTemplate tem)
        {
            EmailTemplateVM vm = new EmailTemplateVM();
            vm.Body = tem.Body;
            vm.Id = tem.EmailTemplateId;
            vm.CreatedBy = tem.CreatedBy;
            vm.CreatedOn = tem.CreatedOn;
            vm.Footer = tem.Footer;
            vm.IsActive = tem.IsActive;
            vm.ModifiedBy = tem.ModifiedBy;
            if (tem.ModifiedOn != null)
            {
                vm.ModifiedOn = tem.ModifiedOn.Value;
            }
            vm.Name = tem.Name;
            vm.StoreId = tem.StoreId;
            vm.Subject = tem.Subject;
            return vm;
        }
    }
}

