using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DTO;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Emailing
{


    /// <summary>
    /// Used to send emails.
    /// </summary>
    public class EmailSender
    {
        ConfigurationHelper configurationHelper;

        private StoreDto currentStore { get; set; }

        FileHelper fileHelper = null;

        public EmailSender(string storeKey)
        {
            currentStore = StoreService.GetStoreByKey(storeKey);
            configurationHelper = new ConfigurationHelper(currentStore.StoreKey);
            fileHelper = new FileHelper(currentStore.StoreKey);
        }

        #region properties

        List<string> emailAdresses = new List<string>();
        string namesOfSubscribers = string.Empty;

        #endregion

        #region Methods

        /// <summary>
        /// User for sending notification if the subscribers are not added in the data base.The subject and 
        /// body of the email is picked from app.config
        /// </summary>
        public void NotifyThroughEmail(int entity, string exception)
        {
            try
            {
                string entityName = ((EmailTemplateId)entity).ToString();
                MailMessage mail = new MailMessage();

                mail.To.Add(configurationHelper.GetSetting(NOTIFICATION.Email_Destination));
                SmtpClient SmtpServer = new SmtpClient(configurationHelper.GetSetting(NOTIFICATION.Email_SMTP));
                mail.From = new MailAddress(configurationHelper.GetSetting(NOTIFICATION.Email_Source));
                mail.CC.Add(configurationHelper.GetSetting(NOTIFICATION.Email_CC));
                mail.Subject = entityName + " " + configurationHelper.GetSetting(NOTIFICATION.Email_Subject);
                mail.Body = "\n\n" + exception;
                SmtpServer.Port = Convert.ToInt32(configurationHelper.GetSetting(NOTIFICATION.Email_Port));
                SmtpServer.Credentials = new System.Net.NetworkCredential(configurationHelper.GetSetting(NOTIFICATION.Email_Username),
                    configurationHelper.GetSetting(NOTIFICATION.Email_Password));
                SmtpServer.EnableSsl = Convert.ToBoolean(configurationHelper.GetSetting(NOTIFICATION.Email_SSL_Enable));
                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, currentStore.StoreId , currentStore.CreatedBy);
            }
        }
        /// <summary>
        /// Used only for sending service stopping notifications only.
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="exp"></param>
        /// <param name="templateId"></param>
        public void NotifyThroughEmail(string subject, string exception, int templateId)
        {
            try
            {
                JobRepository jobRepository = new JobRepository(currentStore.StoreKey);
                MailMessage mail = new MailMessage();

                if (templateId == (int)EmailTemplateId.SimpleNotification)
                {
                    emailAdresses = jobRepository.GetEmailAdresses(templateId);
                    if (emailAdresses.Count > 0)
                    {
                        foreach (string emailId in emailAdresses)
                        {
                            mail.To.Add(emailId);
                        }
                        namesOfSubscribers = "";
                        SmtpClient SmtpServer = new SmtpClient(configurationHelper.GetSetting(NOTIFICATION.Email_SMTP));
                        mail.From = new MailAddress(configurationHelper.GetSetting(NOTIFICATION.Email_Source));
                        mail.Subject = jobRepository.GetEmailSubject(templateId).Replace("{0}", subject);
                        mail.Body = jobRepository.GetEmailBody(templateId).Replace("{0}", subject + "\n\n") + jobRepository.GetEmailFooter(templateId).Replace("{1}", "\n\n" + exception);

                        SmtpServer.Port = Convert.ToInt32(configurationHelper.GetSetting(NOTIFICATION.Email_Port));
                        SmtpServer.Credentials = new System.Net.NetworkCredential(configurationHelper.GetSetting(NOTIFICATION.Email_Username), configurationHelper.GetSetting(NOTIFICATION.Email_Password));
                        SmtpServer.EnableSsl = Convert.ToBoolean(configurationHelper.GetSetting(NOTIFICATION.Email_SSL_Enable));
                        SmtpServer.Send(mail);
                    }
                    else
                    {
                        CustomLogger.LogTraceInfo("No email recipents configured, please configure to get exeption logs", currentStore.StoreId, currentStore.CreatedBy);
                        NotifyThroughEmail(templateId, exception);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy);
            }
        }

        /// <summary>
        /// Will send Email to the subscribers picked up from Database.
        /// </summary>
        /// <param name="id"> Order id, customer id ,or templateId ids, this id will be displayed in the subject</param>
        /// <param name="exception">exception</param>
        /// <param name="attachment">The file name of the file to be attached, like xml in case of sales order, or csv in case of customer. </param>
        /// <param name="templateId">Entities enum in Common.Enums.AllEnums</param>

        public void NotifyThroughEmail(string id, string exception, string attachment = "", int templateId = 99)
        {

            try
            {
                JobRepository jobRepository = new JobRepository(currentStore.StoreKey);
                MailMessage mail = new MailMessage();

                if (templateId == (int)EmailTemplateId.Store | templateId == (int)EmailTemplateId.Customer | templateId == (int)EmailTemplateId.Inventory | templateId == (int)EmailTemplateId.Product | templateId == (int)EmailTemplateId.SaleOrder | templateId == (int)EmailTemplateId.Discount | templateId == (int)EmailTemplateId.SimpleNotification | templateId == (int)EmailTemplateId.Price | templateId == (int)EmailTemplateId.SalesOrderStatus | templateId == (int)EmailTemplateId.ThirdPartySalesOrder)
                {
                    emailAdresses = jobRepository.GetEmailAdresses(templateId);

                    if (emailAdresses.Count > 0)
                    {
                        foreach (string emailId in emailAdresses)
                        {
                            mail.To.Add(emailId);
                        }
                        namesOfSubscribers = "";

                        mail.IsBodyHtml = true;
                        SmtpClient SmtpServer = new SmtpClient(configurationHelper.GetSetting(NOTIFICATION.Email_SMTP));
                        mail.From = new MailAddress(configurationHelper.GetSetting(NOTIFICATION.Email_Source));
                        mail.Subject = jobRepository.GetEmailSubject(templateId).Replace("{0}", configurationHelper.GetSetting(APPLICATION.Enviornment));

                        if (attachment != "")
                        {
                            if (File.Exists(attachment) && fileHelper.CheckFileAvailability(attachment))
                            {
                                mail.Attachments.Add(new Attachment(attachment));
                                mail.Body = (jobRepository.GetEmailBody(templateId).Replace("{0}", namesOfSubscribers).Replace("{1}", "").Replace("{2}", exception).Replace("{3}", "File Is Attached With The Email.")) + "\n \n" + jobRepository.GetEmailFooter(templateId);
                            }
                            else
                            {
                                CustomLogger.LogTraceInfo("Couldn't attach the file with the email, doesn't exists at the location.", currentStore.StoreId, currentStore.CreatedBy);
                                mail.Body = (jobRepository.GetEmailBody(templateId).Replace("{0}", namesOfSubscribers).Replace("{1}", "").Replace("{2}", exception).Replace("{3}", "")) + "\n \n" + jobRepository.GetEmailFooter(templateId);
                            }
                        }
                        if (attachment == "")
                        {
                            mail.Body =
                                (
                                jobRepository.GetEmailBody(templateId).Replace("{0}", namesOfSubscribers).Replace("{1}", exception).Replace("{2}", "")
                                )
                                + "\n" +
                                jobRepository.GetEmailFooter(templateId);
                        }
                        SmtpServer.Port = Convert.ToInt32(configurationHelper.GetSetting(NOTIFICATION.Email_Port));
                        SmtpServer.Credentials = new System.Net.NetworkCredential(configurationHelper.GetSetting(NOTIFICATION.Email_Username), configurationHelper.GetSetting(NOTIFICATION.Email_Password));
                        SmtpServer.EnableSsl = Convert.ToBoolean(configurationHelper.GetSetting(NOTIFICATION.Email_SSL_Enable));
                        SmtpServer.Send(mail);
                    }
                    else
                    {
                        CustomLogger.LogTraceInfo("No email recipents configured, please configure to get exeption logs", currentStore.StoreId, currentStore.CreatedBy);
                        NotifyThroughEmail(templateId, exception);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, currentStore.StoreId, currentStore.CreatedBy);
            }
        }
        #endregion
    }
}
