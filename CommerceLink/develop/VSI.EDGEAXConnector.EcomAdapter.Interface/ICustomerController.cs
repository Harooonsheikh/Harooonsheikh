using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ECommerceDataModels;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface ICustomerController : IDisposable
    {
        List<EcomcustomerCustomerEntity> GetCustomer();
        List<EcomcustomerCustomerEntity> GetCustomer(string file);
        void UpdateCustomer(List<EcomcustomerCustomerEntity> customer);
        void UpdateCustomerViaXML(List<EcomcustomerCustomerEntity> customer);
        void UpdateAddress(List<EcomcustomerAddressEntityItem> customer);
    }
}
