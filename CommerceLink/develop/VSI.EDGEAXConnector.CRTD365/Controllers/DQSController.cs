using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EdgeAXCommerceLink.RetailProxy.Extensions;
using NewRelic.Api.Agent;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class DQSController : BaseController, IDQSController
    {
        public DQSController(string storeKey) : base(storeKey)
        {

        }
        public ErpDQSResponse GetDQS(string username, string password, string workflowName, string jsonInput, string endPoint)
        {
            ErpDQSResponse erpDQSResponse = new ErpDQSResponse(false, string.Empty, string.Empty);
            //var rsResponse = ECL_GetDQS(username, password, workflowName, jsonInput, endPoint);
            //if (rsResponse != null)
            //{
            //    erpDQSResponse = new ErpDQSResponse(rsResponse.Success, rsResponse.Message, rsResponse.Result);
            //}
            return erpDQSResponse;
        }
        #region RetailServer API
        //[Trace]
        //private DQSResponse ECL_GetDQS(string username, string password, string workflowName, string jsonInput, string endPoint)
        //{
        //    //IDQSItemManager dqsItemManager = RPFactory.GetManager<IDQSItemManager>();
        //    //return Task.Run(async () => await dqsItemManager.ECL_GetDQS(username, password, workflowName, jsonInput, endPoint)).Result;
        //}
        #endregion
    }
}
