using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface ICategoryController
    {
        ErpCategory GetCategory(string key);
        List<ErpCategory> GetAllCategories();
    }
}