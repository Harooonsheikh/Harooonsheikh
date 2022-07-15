using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Enums
{


    //NS: D365 Update 8.1 Application change start
    public enum ErpTransactionServiceAuthenticationType
    {
        CertificateAuthentication = 0,
        ServiceToServiceAuthentication = 1,
        AdfsServiceToServiceClientSecretAuthentication = 2
    }

    //NS: D365 Update 8.1 Application change end
}
