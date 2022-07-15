using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Common.Enums
{
    public enum Entities : short
    {
        ProductCategory = 1,
        Product = 2,
        Customer = 3,
        Store = 4,
        SaleOrder = 5,
        CustomerAddress = 7,
        Inventory = 6,
        SalesOrderStatus = 8,
        CategoryAssignment = 9,
        Channel = 10
    }
    //Connection string names for Control Panel.
    public enum ConnectionStrings
    {
        CEIntegrationDBEntities, JOIntegrationDBEntities, EQIntegrationDBEntities

    }
    public enum EmailTemplateId : short
    {

        Product = 2,
        Customer = 3,
        Store = 4,
        SaleOrder = 5,
        CustomerAddress = 7,
        Inventory = 6,
        SalesOrderStatus = 8,
        Discount = 10,
        Price = 11,
        SimpleNotification = 22,
        QuotationReasonGroup = 26,
        QuantityDiscount = 27,
        DiscountWithAffiliation = 28,
        QuantityDiscountWithAffiliation = 29,
        DataDelete = 30,
        ThirdPartySalesOrder = 31,
    }

    /// <summary>
    /// This enum maps to EntityType of Configurable Objects
    /// </summary>
    public enum ConfigObjecsType : short
    {
        DeliveryModes = 1,
        PaymentMethods = 2,
        TaxCodes = 3,
        Charges = 4,
        GiftCards = 5,
        LanguageCode = 6
    }
    public enum PaymentMethods : short
    {
        CreditCard = 2,
        GiftCard = 3,
    }
    public enum TaxGroups : short
    {
        SalesTaxGroup = 1,
        ItemTaxGroup = 2,
    }
    public enum DeliveryModes : short
    {
        Ground = 1,
        SecondDay = 2,
        NextDay = 3,
        StorePickup = 4,
        Email = 5,
        ShopRunner = 6

    }
    public enum Charges : short
    {
        DiscountCharges = 1,
        ShippingCharges = 2,
        MonogramCharges = 3
    }
    public enum Keys : short
    {
        EcomKey,
        ErpKey
    }
    public enum SyncServices : short
    {
        ProductSync = 1,
        CustomerSync = 2,

    }

    public enum ServiceStatus : short
    {
        Available = 1,
        InProgress = 2,

    }
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
        UploadConfigurationSync = 124,
        UploadQuotationReasonGroupSync = 126,

        CartSync = 27,

        QuantityDiscountSync = 5,
        UploadQuantityDiscountSync = 115,

        DiscountWithAffiliationSync = 28,
        UploadDiscountWithAffiliationSync = 128,

        QuantityDiscountWithAffiliationSync = 29,
        UploadQuantityDiscountWithAffiliationSync = 129,
        DataDeleteSync = 130,
        DownloadThirdPartySalesOrderSync = 131,

    }

    public enum SynchJobStatus : short
    {
        Available = 1,
        InProgress = 2,

    }

    public enum JobLogStatuses : short
    {
        Error = 0,
        Started = 1,
        Completed = 2
    }

    public enum CHANNELPUBLISHINGSTATUS : int
    {
        /// <summary>
        /// The None member.
        /// </summary>
        None = 0,

        /// <summary>
        /// The Draft member.
        /// </summary>
        Draft = 1,

        /// <summary>
        /// The InProgress member.
        /// </summary>
        InProgress = 2,

        /// <summary>
        /// The Published member.
        /// </summary>
        Published = 3,

        /// <summary>
        /// The Failed member.
        /// </summary>
        Failed = 4
    }


    public enum OrderType
    {
        SalesOrder,
        Quote
    }
}
