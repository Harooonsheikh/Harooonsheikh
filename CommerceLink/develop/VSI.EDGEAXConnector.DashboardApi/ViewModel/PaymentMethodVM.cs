using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VSI.EDGEAXConnector.DashboardApi.ViewModel
{
    public class PaymentMethodVM
    {
        public int PaymentMethodId;
        public Nullable<int> ParentPaymentMethodId = null;
        public string ECommerceValue = string.Empty;
        public string ErpValue = string.Empty;
        public bool HasSubMethod = false;
        public string ErpCode = string.Empty;
        public bool IsPrepayment = false;
        public int StoreId_FK = 0;
        public string ServiceAccountId = string.Empty;
        public bool UsePaymentConnector = false;
        public PaymentMethodVM()
        {
        }
    }
}