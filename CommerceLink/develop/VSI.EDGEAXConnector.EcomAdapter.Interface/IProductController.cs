using System;
using System.Collections.Generic;
using System.Xml;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface IProductController : IDisposable
    {
        string PushProducts(ErpCatalog catalog, ErpChannel channel, string fileName);
        void PushProductImages(List<KeyValuePair<string, string>> images);

    }
}
