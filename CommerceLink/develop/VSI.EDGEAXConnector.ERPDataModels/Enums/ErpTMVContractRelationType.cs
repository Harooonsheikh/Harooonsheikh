using System;
namespace VSI.EDGEAXConnector.ERPDataModels
{

    public enum ErpTMVContractRelationType
    {
        Migration = 10,
        Switch = 20,
        Update = 30,
        Upgrade = 40,
        Consolidation = 50,
        OpenTransfer = 59,
        Transfer = 60,
        WinBack = 70,
        Cancellation = 80,
        Prolongation = 90,
        Correction = 100,
        New = 110,
        SwitchTransfer = 111,
        UpgradedQty = 112,
        DowngradedQty = 113
    }
}


