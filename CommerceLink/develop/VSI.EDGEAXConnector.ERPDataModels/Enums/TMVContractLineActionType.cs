using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ERPDataModels.Enums
{
    public enum TMVContractLineActionType
    {
        None = 0,
        Invoiced = 1,
        Prolongation = 2,
        Migrated = 3,
        Updated = 4,
        Upgraded = 5,
        Switched = 6,
        Cancelled = 7,
        CreditNote = 8,
        CreditNoteInvoiceCorrection = 9,
        CreditNoteSwitch = 10,
        Correction = 11,
        Rollbacked = 12,
        Transferred = 13,
        SwitchTransfered = 14,
        UpgradedQty = 15,
        DowngradedQty = 16,
        Consolidated = 17
    }
}
