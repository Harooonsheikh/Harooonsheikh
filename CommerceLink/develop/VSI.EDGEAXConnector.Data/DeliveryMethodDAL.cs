using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class DeliveryMethodDAL :BaseClass
    {
        public DeliveryMethodDAL(string storeKey) : base(storeKey)
        {

        }
        public DeliveryMethod GetDeliveryMethodByPrice(decimal price)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                DeliveryMethod dMethod = db.DeliveryMethod.First(d => d.Price == price && d.StoreId==StoreId);
                return dMethod;
            }
        }
        public DeliveryMethod GetItemIdByName(string name)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {

                DeliveryMethod dMethod = db.DeliveryMethod.First(d => d.Name == name && d.StoreId == StoreId);
                return dMethod;
            }
        }
        public List<DeliveryMethod> GetAllDeliveryMethods()
        {
            List<DeliveryMethod> lstDeliveryMethods = new List<DeliveryMethod>();
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                try
                {
                    lstDeliveryMethods = db.DeliveryMethod.Where(d=> d.StoreId == StoreId).ToList();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                }
            }

            return lstDeliveryMethods;
        }
        public bool AddDeliveryMethod(DeliveryMethod dm)
        {
            using (var db = new IntegrationDBEntities())
            {
                try
                {
                    dm.StoreId = StoreId;
                    dm.CreatedBy = UserId;
                    dm.CreatedOn = DateTime.UtcNow;

                    db.DeliveryMethod.Add(dm);
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
        public bool UpdateDeliveryMethod(DeliveryMethod dm)
        {
            using (var db = new IntegrationDBEntities())
            {
                try
                {
                    var dmObj = db.DeliveryMethod.Where(s => s.DeliveryMethodId == dm.DeliveryMethodId && dm.StoreId==StoreId).FirstOrDefault();
                    dmObj.Name = dm.Name;
                    dmObj.Price = dm.Price;
                    dmObj.ItemId = dm.ItemId;
                    dmObj.ErpKey = dm.ErpKey;
                    dmObj.StoreId = StoreId;
                    dmObj.ModifiedOn = DateTime.UtcNow;
                    dmObj.ModifiedBy = UserId;
                    db.Entry(dmObj).State = System.Data.Entity.EntityState.Modified;
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
        public bool DeleteDeliveryMethod(DeliveryMethod dm)
        {
            using (var db = new IntegrationDBEntities())
            {
                try
                {
                    var dmObj = db.DeliveryMethod.Where(s => s.DeliveryMethodId == dm.DeliveryMethodId && dm.StoreId == StoreId).FirstOrDefault();
                    db.DeliveryMethod.Remove(dmObj);
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
