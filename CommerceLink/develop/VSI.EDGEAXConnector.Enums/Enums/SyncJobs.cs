using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Enums
{
    public enum SyncJobs : short
    {
        InventorySynch = 1,
        ProductSync = 2,
        StoreSync = 3,
        PriceSync = 4,

        CustomerSync = 13,
        CategorySync = 15,
        SalesOrderSync = 17,
        CustomerSyncInMagento = 18,
        DiscountSync = 19,
        SalesOrderStatusSync = 20,
        CustomerDeletedAddressesSync = 21,

        UploadInventorySynch = 11,
        UploadProductSync = 12,
        UploadPriceSync = 14,
        DownloadCustomerSync = 113,
        DownloadSalesOrderSync = 117,
        UploadCustomerSyncInMagento = 118,
        UploadDiscountSync = 119,
        DownloadCustomerDeletedAddressesSync = 121,
        UploadSalesOrderStatusSync = 122,

        ChannelPublishingSync = 123,

        ConfigurationSync = 24,
        UploadConfigurationSync = 124,

        OfferTypeGroupSync = 25,
        UploadOfferTypeGroupSync = 125,

        QuotationReasonGroupSync = 26,
        UploadQuotationReasonGroupSync = 126,

        CartSync = 27,

        QuantityDiscountSync = 5,
        UploadQuantityDiscountSync = 115
    }
}
