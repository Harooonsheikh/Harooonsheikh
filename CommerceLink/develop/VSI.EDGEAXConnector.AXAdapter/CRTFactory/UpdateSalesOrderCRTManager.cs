using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class UpdateSalesOrderCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public UpdateSalesOrderCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public ErpCancelContractResponse CancelContract(string salesOrderId, string salesLineRecId, string storeKey)
        {
            var updateSalesOrderController = _crtFactory.CreateUpdateSalesOrderController(storeKey);
            return updateSalesOrderController.CancelContract(salesOrderId, salesLineRecId);
        }
        public ErpTerminateContractResponse TerminateContract(string salesOrderId, string ChannelReferenceId, string salesLineRecId, string storeKey)
        {
            var updateSalesOrderController = _crtFactory.CreateUpdateSalesOrderController(storeKey);
            return updateSalesOrderController.TerminateContract(salesOrderId, ChannelReferenceId, salesLineRecId);
        }
        public ErpUpdateContractResponse UpdateContract(ErpTMVCrosssellType action, string salesOrderId, string salesLineRecId, ErpAdditionalSalesLine newSalesLine, ErpCustomerOrderInfo customerOrderInfo, string storeKey)
        {
            var updateSalesOrderController = _crtFactory.CreateUpdateSalesOrderController(storeKey);
            return updateSalesOrderController.UpdateContract(action, salesOrderId, salesLineRecId, newSalesLine, customerOrderInfo);
        }
        public ErpAddContractRelationResponse AddContractRelation(ErpTMVContractRelationType action, string orgSalesOrderId, string orgSalesLineRecIds, string newSalesOrderId, string newSalesLineRecIds, string storeKey)
        {
            var updateSalesOrderController = _crtFactory.CreateUpdateSalesOrderController(storeKey);
            return updateSalesOrderController.AddContractRelation(action, orgSalesOrderId, orgSalesLineRecIds, newSalesOrderId, newSalesLineRecIds);
        }
    }
}
