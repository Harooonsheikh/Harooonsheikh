using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Data.ViewModels
{
    public class SubscriberVM
    {
        public SubscriberVM()
        {
            this.EmailSubscribers = new List<EmailSubscriberVM>();
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> StoreId_FK { get; set; }
        public int CreatedByUser { get; set; }
        public int ModifiedByUser { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> ModifiedAt { get; set; }
        public List<EmailSubscriberVM> EmailSubscribers { get; set; }
    }
}
