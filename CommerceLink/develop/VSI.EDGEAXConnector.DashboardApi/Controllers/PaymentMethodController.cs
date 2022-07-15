using System;
using System.Collections.Generic;
using System.Web.Http;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.DashboardApi.ViewModel;
using VSI.EDGEAXConnector.Data;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    public class PaymentMethodController : ApiBaseController
    {
        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Get()
        {
            List<PaymentMethodVM> paymentMethodList = null;
            PaymentMethodDAL paymentMethodDAL = null;
            try
            {
                paymentMethodDAL = new PaymentMethodDAL(this.DbConnStr, this.StoreKey, this.User);
                paymentMethodList = new List<PaymentMethodVM>();
                paymentMethodDAL.GetAllPaymentMethods().ForEach(m =>
                {
                    PaymentMethodVM vm = new PaymentMethodVM();
                    vm.ECommerceValue = m.ECommerceValue;
                    vm.ErpCode = m.ErpCode;
                    vm.ErpValue = m.ErpValue;
                    vm.ServiceAccountId = m.ServiceAccountId;
                    vm.UsePaymentConnector = m.UsePaymentConnector;
                    if (m.HasSubMethod != null)
                    {
                        vm.HasSubMethod = m.HasSubMethod.Value;
                    }

                    vm.IsPrepayment = m.IsPrepayment;
                    if (m.ParentPaymentMethodId != null)
                    {
                        vm.ParentPaymentMethodId = m.ParentPaymentMethodId.Value;
                    }

                    vm.PaymentMethodId = m.PaymentMethodId;
                    vm.StoreId_FK = m.StoreId;
                    paymentMethodList.Add(vm);
                });
                return Ok(paymentMethodList);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Delete(int paymentMethodId)
        {
            PaymentMethodDAL paymentMethodDAL = null;
            try
            {
                paymentMethodDAL = new PaymentMethodDAL(this.DbConnStr, this.StoreKey, this.User);
                string result = paymentMethodDAL.DeletePaymentMethod(paymentMethodId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Add(PaymentMethodVM paymentMethod)
        {
            PaymentMethodDAL paymentMethodDAL = null;
            try
            {
                paymentMethodDAL = new PaymentMethodDAL(this.DbConnStr, this.StoreKey, this.User);
                var mdl = MapPaymentMethod(paymentMethod);
                var vm = MapPaymentMethod(paymentMethodDAL.AddPaymentMethod(mdl));
                return Ok(vm);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Update(PaymentMethodVM paymentMethod)
        {
            PaymentMethodDAL paymentMethodDAL = null;
            PaymentMethod md = null;
            PaymentMethod updatedPaymentMethod = null;
            try
            {
                paymentMethodDAL = new PaymentMethodDAL(this.DbConnStr, this.StoreKey, this.User);
                md = MapPaymentMethod(paymentMethod);
                updatedPaymentMethod = paymentMethodDAL.UpdatePaymentMethod(md);
                var vm = MapPaymentMethod(updatedPaymentMethod);
                if (updatedPaymentMethod != null)
                {
                    return Ok(vm);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        private PaymentMethodVM MapPaymentMethod(PaymentMethod m)
        {
            PaymentMethodVM vm = new PaymentMethodVM();
            vm.ECommerceValue = m.ECommerceValue;
            vm.ErpCode = m.ErpCode;
            vm.ErpValue = m.ErpValue;
            vm.ServiceAccountId = m.ServiceAccountId;
            vm.UsePaymentConnector = m.UsePaymentConnector;
            if (m.HasSubMethod != null)
            {
                vm.HasSubMethod = m.HasSubMethod.Value;
            }
            vm.IsPrepayment = m.IsPrepayment;
            if (m.ParentPaymentMethodId != null)
            {
                vm.ParentPaymentMethodId = m.ParentPaymentMethodId.Value;
            }
            vm.PaymentMethodId = m.PaymentMethodId;
            vm.StoreId_FK = m.StoreId;
            return vm;
        }

        private PaymentMethod MapPaymentMethod(PaymentMethodVM vm)
        {
            PaymentMethod mdl = new PaymentMethod();
            mdl.ECommerceValue = vm.ECommerceValue;
            mdl.ErpCode = vm.ErpCode;
            mdl.ErpValue = vm.ErpValue;
            mdl.ServiceAccountId = vm.ServiceAccountId;
            mdl.UsePaymentConnector = vm.UsePaymentConnector;
            if (vm.HasSubMethod != null)
            {
                mdl.HasSubMethod = vm.HasSubMethod;
            }
            mdl.IsPrepayment = vm.IsPrepayment;
            if (vm.ParentPaymentMethodId != null)
            {
                mdl.ParentPaymentMethodId = vm.ParentPaymentMethodId;
            }
            mdl.PaymentMethodId = vm.PaymentMethodId;
            return mdl;
        }
    }

}
