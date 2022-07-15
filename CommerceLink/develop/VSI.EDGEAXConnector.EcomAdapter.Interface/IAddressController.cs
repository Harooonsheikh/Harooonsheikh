using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.ECommerceDataModels;

namespace VSI.EDGEAXConnector.ECommerceAdapter.Interface
{
    public interface IAddressController : IDisposable
    {
        void SyncAddress(List<EcomcustomerAddressEntityItem> addresses);

        /// <summary>
        /// Magento will generate a csv with single column for Address Ids which are deleted in Magento.
        /// THis method returnes the List of Ids found in CSV.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        // Dictionary<int, List<EcomcustomerAddressEntityItem>> GetDeletedAddressIds(string lFile);

    }
}
