using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class DQSCRTManager
    {
        #region Properties
        private readonly ICRTFactory _crtFactory;
        #endregion Properties

        #region Constructor      
        public DQSCRTManager()
        {
            _crtFactory = new CRTFactory();
        }
        #endregion Constructor


        /// <summary>
        /// GetDQS resolves CRT Controller to call it's GetDQS method.
        /// </summary>
        /// <returns></returns>
        public ErpDQSResponse GetDQS(string username, string password, string workflowName, string jsonInput, string endPoint, string storeKey)
        {
            var dqsController = _crtFactory.CreateDQSController(storeKey);

            return dqsController.GetDQS(username, password, workflowName, jsonInput, endPoint);
        }

    }
}
