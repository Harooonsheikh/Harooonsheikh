using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface IDeletedAddressController:IDisposable
    {

        List<int> GetDeletedAddressIds(string lFile);
    }
}
