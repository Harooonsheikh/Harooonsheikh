using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class ServiceProductDAL
    {
        public ServiceProduct GetServiceProductByDescription(string desc)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                ServiceProduct sPro = db.ServiceProducts.First(d => d.description == desc);
                return sPro;
            }
        }
        public ServiceProduct GetServiceProductByName(string name)
        {
            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {

                ServiceProduct sPro = db.ServiceProducts.First(d => d.name == name);
                return sPro;
            }
        }

        public List<ServiceProduct> GetAllServiceProducts()
        {
            List<ServiceProduct> lstServiceProducts = new List<ServiceProduct>();

            using (IntegrationDBEntities db = new IntegrationDBEntities())
            {
                try
                {
                    lstServiceProducts = db.ServiceProducts.ToList();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex);
                }
            }

            return lstServiceProducts;
        }

        public bool AddServiceProduct(ServiceProduct sPro)
        {
            using (var db = new IntegrationDBEntities())
            {
                try
                {
                    db.ServiceProducts.Add(sPro);
                    db.SaveChanges();           

                    return true;
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex);
                    return false;
                }
            }
        }

        public bool UpdateServiceProduct(ServiceProduct sPro)
        {
            using (var db = new IntegrationDBEntities())
            {
                try
                {
                    var dmObj = db.ServiceProducts.Where(s => s.id == sPro.id).FirstOrDefault();
                    dmObj.name = sPro.name;
                    dmObj.description = sPro.description;
                    dmObj.itemId = sPro.itemId;
                    dmObj.ErpKey = sPro.ErpKey;
                    db.Entry(dmObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
       
                    return true;
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex);
                    return false;
                }
            }
        }

        public bool DeleteServiceProduct(ServiceProduct sPro)
        {
            using (var db = new IntegrationDBEntities())
            {
                try
                {
                    var dmObj = db.ServiceProducts.Where(s => s.id == sPro.id).FirstOrDefault();
                    db.ServiceProducts.Remove(dmObj);
                    db.SaveChanges();


                    return true;
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex);
                    return false;
                }
            }
        }
    }
}
