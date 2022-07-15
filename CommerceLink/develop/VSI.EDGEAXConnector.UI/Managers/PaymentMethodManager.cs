using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.UI.Managers
{
    public class PaymentMethodManager
    {

        private static PaymentMethodDAL pmObjDAL = new PaymentMethodDAL(StoreService.StoreLkey);

        public static List<PaymentMethod> GetAllPaymentObjects()
        {
            List<PaymentMethod> lstPMObjects = new List<PaymentMethod>();

            lstPMObjects = pmObjDAL.GetAllPaymentMethods();

            return lstPMObjects;
        }

        public static bool AddPaymentMethod(PaymentMethod pm)
        {
            return pmObjDAL.AddPaymentMethod(pm);
        }

        public static bool UpdatePaymentMethod(PaymentMethod pm)
        {
            return pmObjDAL.UpdatePaymentMethod(pm);
        }

        public static string DeletePaymentMethod(PaymentMethod pm)
        {
            return pmObjDAL.DeletePaymentMethod(pm);
        }
    }
}
