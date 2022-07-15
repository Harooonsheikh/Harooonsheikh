using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Enums.Enums
{
    /// <summary>
    /// Entity types for Trigger Data Sync
    /// </summary>
    public enum TriggerDataSyncEntityTypes
    {
        /// <summary>
        /// Entity type not provided
        /// </summary>
        None,

        /// <summary>
        /// Entity type Customer
        /// </summary>
        Customer,

        /// <summary>
        /// Entity type Invoice
        /// </summary>
        Invoice,

        /// <summary>
        /// Entity type PAC License
        /// </summary>
        PACLicense,

        /// <summary>
        /// Entity type Contract
        /// </summary>
        Contract
    }
}
