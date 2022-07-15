using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.ERPDataModels;
//using Autofac;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class DQSController : BaseController, IDQSController
    {
        public DQSController(string storeKey) : base(storeKey)
        {

        }

        public ErpDQSResponse GetDQS(string username, string password, string workflowName, string jsonInput, string endPoint)
        {
            DQSController dqsController = new DQSController(currentStore.StoreKey);
            ErpDQSResponse erpDQSResponse = new ErpDQSResponse(false, string.Empty, null);

            var crtDQSManager = new DQSCRTManager();
            erpDQSResponse =  crtDQSManager.GetDQS(username, password, workflowName, jsonInput, endPoint, currentStore.StoreKey);

            if (erpDQSResponse.Success && !string.IsNullOrEmpty(erpDQSResponse.Result))
            {
                string[] strSplittedArray = erpDQSResponse.Result.Split(new string[] { "SanctionListStatus:" }, StringSplitOptions.RemoveEmptyEntries);

                if (strSplittedArray != null && strSplittedArray.Length > 1)
                {
                    erpDQSResponse.Result = strSplittedArray[1].Replace("\n", "").Trim();
                }
            }

            return erpDQSResponse;
        }
    }
}
