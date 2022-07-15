using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.UI.Managers
{
    public class EmailTemplateManager
    {
        private static EmailTemplateDAL tempalteDAL = new EmailTemplateDAL(StoreService.StoreLkey);
        public static List<EmailTemplate> GetAllTemplates()
        {
            List<EmailTemplate> lstTemplates = new List<EmailTemplate>();
            lstTemplates = tempalteDAL.GetAllTemplates();
            return lstTemplates;
        }

        public static bool AddTemplate(EmailTemplate temp)
        {
            return tempalteDAL.AddTemplate(temp);
        }

        public static bool UpdateTemplate(EmailTemplate temp)
        {
            return tempalteDAL.UpdateTemplate(temp);
        }

        public static bool DeleteTemplate(EmailTemplate temp)
        {
            return tempalteDAL.DeleteTemplate(temp);
        }
    }
}
