using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.DashboardApi.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await configEmailasync(message);
        }
        private async Task configEmailasync(IdentityMessage message)
        {
            string username = string.Empty;
            string password = string.Empty;
            string emailsource = string.Empty;
            ApplicationSettingsDAL applicationSettings = new ApplicationSettingsDAL();
            List<ApplicationSetting> listAppSettings = applicationSettings.GetApplicationSettingsByScreenNameAndStore("EmailSetting", 
                ConfigurationManager.AppSettings["StoreId"] != null ? Int32.Parse(ConfigurationManager.AppSettings["StoreId"]) : 0);
            SmtpClient client = new SmtpClient();
            for (var i = 0; i < listAppSettings.Count(); i++)
            {
                if (listAppSettings[i].Key == "NOTIFICATION.Email_Port")
                    client.Port = Int32.Parse(listAppSettings[i].Value);
                else if (listAppSettings[i].Key == "NOTIFICATION.Email_SMTP")
                    client.Host = listAppSettings[i].Value;
                else if (listAppSettings[i].Key == "NOTIFICATION.Email_SSL_Enable")
                    client.EnableSsl = Boolean.Parse(listAppSettings[i].Value);
                else if (listAppSettings[i].Key == "NOTIFICATION.Email_Username")
                    username = listAppSettings[i].Value;
                else if (listAppSettings[i].Key == "NOTIFICATION.Email_Password")
                    password = listAppSettings[i].Value;
                else if (listAppSettings[i].Key == "NOTIFICATION.Email_Source")
                    emailsource = listAppSettings[i].Value;

            }
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential(username, password);
            MailMessage mm = new MailMessage(emailsource, message.Destination, message.Subject, message.Body);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);
        }
    }
}