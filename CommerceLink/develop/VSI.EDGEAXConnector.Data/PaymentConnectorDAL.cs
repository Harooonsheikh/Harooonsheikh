using System;
using System.Collections.Generic;
using System.Linq;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class PaymentConnectorDAL : BaseClass
    {
        public PaymentConnectorDAL(string storeKey) : base(storeKey)
        {

        }

        public PaymentConnectorDAL(string connectionString, string storeKey, string user) : base(connectionString, storeKey,user)
        {

        }

        public List<PaymentConnector> GetAllPaymentConnectors()
        {
            List<PaymentConnector> paymentConnectorList = new List<PaymentConnector>();
            try
            {
                using (IntegrationDBEntities db = this.GetConnection())
                {
                    paymentConnectorList = db.PaymentConnector.ToList();
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }

            return paymentConnectorList;
        }

        public PaymentConnector GetPaymentConnectorMethod(string erpPaymentConnector, string erpPaymentMethodCode)
        {
            try
            {
                using (IntegrationDBEntities db = this.GetConnection())
                {
                    PaymentConnector paymentConnector = (from pc in db.PaymentConnector
                                                         join pm in db.PaymentMethod on pc.PaymentConnectorId equals pm.PaymentConnectorId
                                                         where pc.ERPCreditCardProcessorName == erpPaymentConnector
                                                         && pm.ErpCode == erpPaymentMethodCode
                                                         select pc).FirstOrDefault();
                    return paymentConnector;
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }

        public PaymentConnector GetPaymentConnectorUsingErpPaymentConnector(string erpPaymentConnector)
        {
            try
            {
                using (IntegrationDBEntities db = this.GetConnection())
                {
                    PaymentConnector paymentConnector = db.PaymentConnector.FirstOrDefault(pc => pc.ERPCreditCardProcessorName == erpPaymentConnector);
                    return paymentConnector;
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }
    }
}
