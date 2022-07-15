using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.ViewModels
{
    public class EmailTemplateVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Footer { get; set; }
        public bool IsActive { get; set; }
        public int StoreId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime ModifiedOn { get; set; }
        public string CreatedBy{ get; set; }
        public string ModifiedBy { get; set; }
    }
}
