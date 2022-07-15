using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.UI.Managers
{
    public class ServiceProductManager
    {

        private static ServiceProductDAL sProObjDAL = new ServiceProductDAL();

        public static List<ServiceProduct> GetAllServiceProducts()
        {
            List<ServiceProduct> lstsProObjects = new List<ServiceProduct>();

            lstsProObjects = sProObjDAL.GetAllServiceProducts();

            return lstsProObjects;
        }

        public static bool AddServiceProduct(ServiceProduct sPro)
        {
            return sProObjDAL.AddServiceProduct(sPro);
        }

        public static bool UpdateServiceProduct(ServiceProduct sPro)
        {
            return sProObjDAL.UpdateServiceProduct(sPro);
        }

        public static bool DeleteServiceProduct(ServiceProduct sPro)
        {
            return sProObjDAL.DeleteServiceProduct(sPro);
        }
    }
}
