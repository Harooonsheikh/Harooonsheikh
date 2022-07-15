using System;
using System.Collections.Generic;
using System.Linq;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.Data
{
    public class PaymentMethodDAL : BaseClass
    {
        public PaymentMethodDAL(string storeKey) : base(storeKey)
        {

        }

        public PaymentMethodDAL(string connectionString, string storeKey, string user) : base(connectionString, storeKey,user)
        {

        }

        public List<PaymentMethod> GetAllPaymentMethods()
        {
            List<PaymentMethod> lstPaymentMethods = new List<PaymentMethod>();

            using (IntegrationDBEntities db = this.GetConnection())
            {
                try
                {
                    lstPaymentMethods = db.PaymentMethod.Where(x => x.StoreId == StoreId).ToList();
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    throw ex;
                }
            }

            return lstPaymentMethods;
        }

        public PaymentMethod AddPaymentMethod(PaymentMethod pm)
        {
            using (var db = this.GetConnection())
            {
                try
                {
                    pm.CreatedBy = UserId;
                    pm.CreatedOn = DateTime.UtcNow;
                    pm.StoreId = StoreId;
                    db.PaymentMethod.Add(pm);
                    db.SaveChanges();   
                    
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    throw ex;
                }
                return pm;
            }
        }

        public PaymentMethod UpdatePaymentMethod(PaymentMethod pm)
        {
            using (var db = this.GetConnection())
            {
                try
                {
                    var pmObj = db.PaymentMethod.Where(s => s.PaymentMethodId == pm.PaymentMethodId).FirstOrDefault();
                    pmObj.ECommerceValue = pm.ECommerceValue;
                    pmObj.ErpCode = pm.ErpCode;
                    pmObj.ErpValue = pm.ErpValue;
                    pmObj.HasSubMethod = pm.HasSubMethod;
                    pmObj.ParentPaymentMethodId = pm.ParentPaymentMethodId;
                    pmObj.IsPrepayment = pm.IsPrepayment;
                    pmObj.IsCreditCard = pm.IsCreditCard;
                    pmObj.UsePaymentConnector = pm.UsePaymentConnector;
                    pmObj.ServiceAccountId = pm.ServiceAccountId;
                    pmObj.StoreId = StoreId;
                    pmObj.ModifiedOn = DateTime.UtcNow;
                    pmObj.ModifiedBy = UserId;
                    db.Entry(pmObj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return pmObj;
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    throw ex;
                }
            }
        }

        public Boolean UpdateServiceAccountIdForPaymentMethod(int storeId, string serviceAccountId, int paymentConnectorId)
        {
            try
            {
                using (var db = this.GetConnection())
                {
                    var paymentMethodRow = db.PaymentMethod.Where(s => s.StoreId == storeId && s.PaymentConnectorId == paymentConnectorId &&
                        s.ParentPaymentMethodId == null).FirstOrDefault();

                    if (paymentMethodRow != null)
                    {
                        paymentMethodRow.ServiceAccountId = serviceAccountId;
                        db.Entry(paymentMethodRow).State = System.Data.Entity.EntityState.Modified;

                        var paymentMethodResult = db.PaymentMethod.Where(k => k.ParentPaymentMethodId == paymentMethodRow.PaymentMethodId && k.StoreId == StoreId);

                        if (paymentMethodResult != null)
                        {
                            List<PaymentMethod> paymentMethodList = paymentMethodResult.ToList();

                            if (paymentMethodList != null)
                            {
                                foreach (PaymentMethod paymentMethod in paymentMethodList)
                                {
                                    paymentMethod.ServiceAccountId = serviceAccountId;
                                    db.Entry(paymentMethod).State = System.Data.Entity.EntityState.Modified;
                                }
                            }
                        }

                        db.SaveChanges();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                CustomLogger.LogException(ex, StoreId, UserId);
                throw ex;
            }
        }

        public string DeletePaymentMethod(PaymentMethod pm)
        {
            using (var db = this.GetConnection())
            {
                try
                {
                    var parentPaymentObj = db.PaymentMethod.Where(s => s.ParentPaymentMethodId == pm.PaymentMethodId && s.StoreId == StoreId).FirstOrDefault();
                    if (parentPaymentObj !=null)
                    {
                        return "";
                    }
                    else
                    {
                        var dmObj = db.PaymentMethod.Where(s => s.PaymentMethodId == pm.PaymentMethodId && pm.StoreId == StoreId).FirstOrDefault();
                        db.PaymentMethod.Remove(dmObj);
                        db.SaveChanges();
                        return "true";
                    }
                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    return "false";
                }
            }
        }

        public string DeletePaymentMethod(int paymentMethodId)
        {
            using (var db = this.GetConnection())
            {
                try
                {
                    var parentPaymentObj = db.PaymentMethod.Where(s => s.ParentPaymentMethodId == paymentMethodId && s.StoreId == this.StoreId).FirstOrDefault();

                    if (parentPaymentObj != null)

                    { return "ParentMethod"; }

                    else
                    {
                        var paymentMethodObj = db.PaymentMethod.Where(s => s.PaymentMethodId == paymentMethodId && s.StoreId == this.StoreId).FirstOrDefault();

                        if (paymentMethodObj != null)
                        {
                            db.PaymentMethod.Remove(paymentMethodObj);
                            db.SaveChanges();

                            return "Success";
                        }

                        return "NotFound";
                    }

                }
                catch (Exception ex)
                {
                    CustomLogger.LogException(ex, StoreId, UserId);
                    return "Failure";
                }
            }
        }

        public  string GetErpPaymentMode(string comDlvMode)
        {
            int type = Convert.ToInt32(ConfigObjecsType.PaymentMethods);
            using (IntegrationDBEntities db = this.GetConnection())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ComValue.Equals(comDlvMode) && k.EntityType == type && k.StoreId == StoreId);
                return row != null ? row.ErpValue : "";
            }
        }
        public  ConfigurableObject GetPaymentModeByKey(int key)
        {
            int type = Convert.ToInt32(ConfigObjecsType.PaymentMethods);
            using (IntegrationDBEntities db = this.GetConnection())
            {
                var row = db.ConfigurableObject.FirstOrDefault(k => k.ConnectorKey == key && k.EntityType == type && k.StoreId == StoreId);
                return row;
            }
        }
        public  List<ConfigurableObject> GetAllPaymentModes()
        {
            int type = Convert.ToInt32(ConfigObjecsType.PaymentMethods);
            using (IntegrationDBEntities db = this.GetConnection())
            {
                var row = db.ConfigurableObject.Where(k => k.EntityType == type && k.StoreId == StoreId);
                return row.ToList();
            }
        }
        public  PaymentMethod GetPaymentMothodByEcommerceKey(string ecommerceKey, int? parentPaymentMethod = null)
        {
            using (IntegrationDBEntities db = this.GetConnection())
            {
                PaymentMethod paymentMethod = parentPaymentMethod == null ? db.PaymentMethod.FirstOrDefault(k => k.ECommerceValue.Equals(ecommerceKey, StringComparison.InvariantCulture) && k.StoreId == StoreId) : db.PaymentMethod.FirstOrDefault(k => k.ECommerceValue.Equals(ecommerceKey, StringComparison.InvariantCulture) && k.ParentPaymentMethodId == parentPaymentMethod && k.StoreId == StoreId);
                return paymentMethod;
            }
        }
    }
}
