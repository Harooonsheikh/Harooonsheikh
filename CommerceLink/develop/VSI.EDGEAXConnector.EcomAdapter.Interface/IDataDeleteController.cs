using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ECommerceDataModels;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface IDataDeleteController : IDisposable
    {
        void DataDeleteSync();
    }
}
