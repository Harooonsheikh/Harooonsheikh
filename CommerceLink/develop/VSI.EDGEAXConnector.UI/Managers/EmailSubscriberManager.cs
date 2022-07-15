using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.UI.Managers
{
    public class EmailSubscriberManager
    {
        private static EmailSubscribersDAL subscriberDAL = new EmailSubscribersDAL(StoreService.StoreLkey);
        public static List<Subscriber> GetAllSubscriber()
        {
            List<Subscriber> lstSubscriber = new List<Subscriber>();
            lstSubscriber = subscriberDAL.GetAllSubscriber();
            return lstSubscriber;
        }

        public static List<EmailSubscriber> GetEmailSubscribtionByID(int subscriberId)
        {
            List<EmailSubscriber> lstEmailSubscriber = new List<EmailSubscriber>();
            lstEmailSubscriber = subscriberDAL.GetEmailSubscribtionByID(subscriberId);
            return lstEmailSubscriber;
        }

        public static bool AddSubscriber(Subscriber subs,string selectedTempaltes)
        {
            return subscriberDAL.AddSubscribers(subs, selectedTempaltes);
        }

        public static bool UpdateSubscriber(Subscriber subs, string selectedTempaltes)
        {
            return subscriberDAL.UpdateSubscriber(subs, selectedTempaltes);
        }

        public static bool DeleteSubscriber(Subscriber subs)
        {
            return subscriberDAL.DeleteSubscriber(subs);
        }
    }
}
