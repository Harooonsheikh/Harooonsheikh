
using System;
using System.Collections.Generic;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;

public partial class ErpSalesOrder
{
    // Custom for MF Start
    public List<ErpShipment> Shipments { get; set; }
    public string DiscountCode { get; set; }
    // Custom for MF End

    // Custom for VW Start
    public ErpAddress BillingAddress { get; set; }
    public List<ErpDiscountLine> ShippingDiscounts { get; set; }
    public List<ErpDiscountLine> OrderDiscounts { get; set; }

    public string SourceCode { get; set; }
    // Custom for VW End

    public string Carrier { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerName { get; set; }
    public string WebsiteSource { get; set; }
    public IntegrationKey _integrationKey { get; set; }
    public decimal Shipping_Tax { get; set; }
    public string ShopRunner { get; set; }
    public List<ErpGiftCard> PaymentGiftCards { get; set; }

    public string isTaxExemptCustomer { get; set; }//;
    public DateTimeOffset transTime { get; set; }//;
    public List<KeyValuePair<string, string>> CustomAttributes { get; set; }


    //NS: Re-Architect
    public decimal ChannelCurrencyExchangeRate { get; set; }
    public DateTimeOffset CreatedDateTime { get; set; }
    public decimal SalesPaymentDifference { get; set; }

    //++ For TV 
    public string TMVMigratedOrderNumber { get; set; }
    public string TMVOldSalesOrderNumber { get; set; }
    public string TMVResellerAccount { get; set; }

    public string TMVMainOfferType { get; set; }

    public bool TMVInvoicePosted { get; set; }

    public string TMVDistributorAccount { get; set; }

    public string TMVSalesOrderSubType { get; set; }

    public string TMVInvoiceScheduleComplete { get; set; }

    public string TMVIndirectCustomer { get; set; }

    public string TMVProductFamily { get; set; }

    public DateTime TMVContractCalculateFrom { get; set; }

    public DateTime TMVContractCalculateTo { get; set; }
    public string TMVCardHolder { get; set; }
    public string TMVIBAN { get; set; }
    public string TMVSwiftCode { get; set; }
    public string TMVBankName { get; set; }
    public string PaymMode { get; set; }
    public string NetAmount { get; set; }
    public string TotalTax { get; set; }
    public long PaymentRecID { get; set; }
    public string PayerId { get; set; }
    public string ParenttransactionId { get; set; }
    public string EmailAddress { get; set; }
    public string Notes { get; set; }
    public string Authorization { get; set; }
    public string CardToken { get; set; }
    public string Language { get; set; }
    public string SiteCode { get; set; }
    public string RetailChannel { get; set; }
    public string ThreeLetterISORegionName { get; set; }
    public string TMVContractStartDate { get; set; }
    public string TMVContractEndDate { get; set; }
    public ErpCreditCardCust PaymentInfo { get; set; }
    public bool TMVTransferOrderAsPerOldDate { get; set; }
    public string TMVSubscriptionName { get; set; }
    public string TMVSubscriptionWeight { get; set; }


    // Custom for TV - Ingram
    public string ResellerId { get; set; }
    public string ResellerName { get; set; }
    public string ResellerEmail { get; set; }
    public string ResellerReceiptEmail { get; set; }
    public ErpAddress ResellerBillingAddress { get; set; }
    public List<ErpShipment> ResellerShipments { get; set; }


    public ErpCustomer Customer { get; set; }
    public ErpCustomer Reseller { get; set; }

    /// <summary>
    /// Asset Id from Ingram Micro
    /// </summary>
    public string IngramAssetId { get; set; }

    /// <summary>
    /// Asset Type from Ingram Micro
    /// </summary>
    public string IngramAssetType { get; set; }

    /// <summary>
    /// Contact Id from Ingram Micro
    /// </summary>
    public string IngramContractId { get; set; }

    /// <summary>
    /// Market Place Id from Ingram Micro
    /// </summary>
    public string IngramMarketPlaceId { get; set; }

    /// <summary>
    /// Parameters for Ingram Micro order
    /// </summary>
    public List<ErpIngramOrderParameter> Parameters { get; set; }

}
