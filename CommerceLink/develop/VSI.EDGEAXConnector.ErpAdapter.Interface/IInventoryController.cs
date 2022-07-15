using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;


namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
   public interface IInventoryController
    {
       List<ErpProduct> GetInventory();    
    }
}
