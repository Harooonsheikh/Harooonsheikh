using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IDQSController
    {
        ErpDQSResponse GetDQS(string username, string password, string workflowName, string jsoninput, string endPoint);
    }
}
