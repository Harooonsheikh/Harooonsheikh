using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Enums
{
    //NS: D365 Update 8.1 Application change start
    public enum ErpInvoiceType
    {
        None = 0,
        SalesOrderInvoice = 1,
        FreeTextInvoice = 2,
        ProjectInvoice = 3,
        CreditNoteInvoice = 4
    }
    //NS: D365 Update 8.1 Application change end
}
