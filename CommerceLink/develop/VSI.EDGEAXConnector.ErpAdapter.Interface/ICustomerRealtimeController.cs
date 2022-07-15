using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface ICustomerRealtimeController<T>
    {
        T CreateCustomer(T Customer);

        T UpdateCustomer(T Customer);

        T GetCustomer(string AccountNuber);

        List<T> FindCustomers(string Criteria);

        List<T> GetCustomersList();
    }
}
