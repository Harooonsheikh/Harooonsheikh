using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.Data
{
    public  class DeliveryModesDAL : BaseClass
    {
        public DeliveryModesDAL(string storeKey) : base(storeKey)
        {

        }
        public  string GetErpDeliveryMode(string comDlvMode)
        {
            int type = Convert.ToInt32(ConfigObjecsType.DeliveryModes);
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ComValue.Equals(comDlvMode) && k.EntityType == type && k.StoreId == StoreId);
                return row != null ? row.ErpValue : "";
            }
        }
        public  ConfigurableObject GetDeliveryModeByKey(int key)
        {
            int type = Convert.ToInt32(ConfigObjecsType.DeliveryModes);
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ConnectorKey == key && k.EntityType == type && k.StoreId == StoreId);
                return row;
            }
        }
        public  List<ConfigurableObject> GetAllDeliveryModes()
        {
            int type = Convert.ToInt32(ConfigObjecsType.DeliveryModes);
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                var row = db.ConfigurableObject.Where(k => k.EntityType == type && k.StoreId == StoreId);
                return row.ToList();
            }
        }
    }
}







