using System;
using System.Xml;
using VSI.EDGEAXConnector.ECommerceDataModels;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface ISaleOrderController : IDisposable
    {
        //List<EcomsalesOrderEntity> GetSalesOrders();
        ErpSalesOrder GetSalesOrders(string filePath);
        ErpSalesOrder GetSalesOrderFromXML(XmlDocument xmlDoc);

        bool SaveThirdPartySalesOrder();
    }
}
