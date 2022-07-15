using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.CRTD365.Controllers;
using Newtonsoft.Json;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    [TestClass]
    public class TestQuotationController
    {
        string storeKey = "";

        [TestMethod]
        public void TestCreateCustomerQuotation()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ErpCustomerOrderInfo customerQuotation = new ErpCustomerOrderInfo();

            customerQuotation = JsonConvert.DeserializeObject<ErpCustomerOrderInfo>("{\"AutoPickOrder\":false,\"OrderType\":\"Quote\",\"Status\":0,\"DocumentStatus\":0,\"CustomerAccount\":\"004136\",\"ChannelRecordId\":\"5637144592\",\"AddressRecord\":\"22565427563\",\"WarehouseId\":\"HOUSTON\",\"StoreId\":\"HOUSTON\",\"TerminalId\":\"HOUSTON-15\",\"TransactionId\":\"HOUSTON-HOUSTON-15-10123\",\"IsTaxIncludedInPrice\":false,\"RoundingDifference\":0,\"TotalManualDiscountAmount\":0,\"TotalManualDiscountPercentage\":0,\"ExpiryDateString\":\"2017-12-02\",\"LocalHourOfDay\":12,\"DeliveryMode\":\"60\",\"RequestedDeliveryDateString\":\"2017-11-02\",\"Comment\":\"\",\"PrepaymentAmountOverridden\":false,\"PrepaymentAmountApplied\":0,\"PreviouslyInvoicedAmount\":0,\"SalespersonStaffId\":\"000160\",\"CurrencyCode\":\"USD\",\"LoyaltyCardId\":\"\",\"HasLoyaltyPayment\":false,\"ChannelReferenceId\":\"STONN-15400002\",\"Email\":\"mara.gentry@visionetsystems.com\",\"OriginalTransactionTime\":\"2017-11-02T06:03:26.6994323Z\",\"IsPriceOverride\":false,\"IsFTCExempt\":false,\"IsCatalogUpSellShown\":false,\"IsContinuityOrder\":false,\"IsContinuityChild\":false,\"ContinuityLineEval\":0,\"IsInstallmentOrderSubmitted\":false,\"OutOfBalanceReleaseType\":0,\"PaymentOutOfBalanceType\":0,\"IsInstallmentBillingPrompt\":false,\"AllocationPriority\":0,\"CommissionSalesGroup\":\"\",\"Items\":[{\"ItemId\":\"81121\",\"Comment\":\"\",\"RecId\":0,\"Quantity\":1,\"QuantityPicked\":0,\"Unit\":\"ea\",\"Price\":59.99,\"Discount\":0,\"DiscountPercent\":0,\"NetAmount\":59.99,\"TaxGroup\":\"TX\",\"TaxItemGroup\":\"RP\",\"SalesMarkup\":0,\"FulfillmentStoreId\":\"HOUSTON\",\"Status\":0,\"InventDimensionId\":\"\",\"ColorId\":\"Sky Blue\",\"SizeId\":\"36\",\"StyleId\":\"Regular\",\"ConfigId\":\"\",\"DeliveryMode\":\"60\",\"RequestedDeliveryDateString\":\"2017-11-02\",\"AddressRecord\":\"22565427563\",\"BatchId\":\"\",\"SerialId\":\"\",\"VariantId\":\"VN-002109\",\"ReturnInventTransId\":\"\",\"InvoiceId\":\"\",\"LineDscAmount\":0,\"PeriodicDiscount\":0,\"PeriodicPercentageDiscount\":0,\"LineManualDiscountAmount\":0,\"LineManualDiscountPercentage\":0,\"TotalDiscount\":0,\"TotalPctDiscount\":0,\"Giftcard\":false,\"LineNumber\":0,\"CustInvoiceTransId\":0,\"IsInstallmentEligible\":false,\"LineType\":0,\"UpSellOrigin\":0,\"CommissionSalesGroup\":\"\",\"WarehouseId\":\"HOUSTON\",\"Charges\":[],\"Discounts\":[],\"Taxes\":[]}],\"Charges\":[],\"Payments\":[],\"Affiliations\":[],\"ExtensionProperties\":[],\"Taxes\":[]}");

            string quotation = string.Empty;

            try
            {
                QuotationController quotationController = new QuotationController(storeKey);
                var result = quotationController.CreateCustomerQuotation(customerQuotation);

                quotation = result.ToString();
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(quotation);
            System.Console.WriteLine("Quotation = " + quotation);
        }

        [TestMethod]
        public void TestGetCustomerQuotation()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            string quotationId = "000001";

            string quotation = string.Empty;

            try
            {
                QuotationController quotationController = new QuotationController(storeKey);
                var result = quotationController.GetCustomerQuotation("", "", "", quotationId);

                quotation = result.ToString();
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(quotation);
            System.Console.WriteLine("Quotation = " + quotation);
        }
    }
}
