using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IUpdateSalesOrderController
    {
        ErpCancelContractResponse CancelContract(string salesOrderId, string salesLineRecId);
        ErpTerminateContractResponse TerminateContract(string salesOrderId, string ChannelReferenceId, string salesLineRecId);
        ErpUpdateContractResponse UpdateContract(ErpTMVCrosssellType action, string salesOrderId, string salesLineRecId, ErpAdditionalSalesLine newSalesLine, ErpCustomerOrderInfo customerOrderInfo);
        ErpAddContractRelationResponse AddContractRelation(ErpTMVContractRelationType action, string orgSalesOrderId, string orgSalesLineRecIds, string newSalesOrderId, string newSalesLineRecIds);
    }
}
