using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;


namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface IPriceController
    {
        List<ErpProduct> GetAllProductPrice();
        List<ErpProductPrice> GetDefaultProductPrice();
        List<ErpProductPrice> GetAllProductPriceExtension();
        ErpProductPrice GetProductPriceExtension(string ErpKey);
    }
}
