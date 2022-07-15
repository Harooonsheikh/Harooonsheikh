using AutoMapper;
using EdgeAXCommerceLink.Commerce.RetailProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRTAX7.Controllers
{
    public class InAppPurchaseController : BaseController, IInAppPurchaseController
    {

        #region "Public"

        public ERPAppAutoRenewContractResponse AutoRenewContract(AutoRenewContractRequest request)
        {
            throw new NotImplementedException();
        }

        public ErpCancelContractResponse CancelContract(CancelContractRequest request)
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
