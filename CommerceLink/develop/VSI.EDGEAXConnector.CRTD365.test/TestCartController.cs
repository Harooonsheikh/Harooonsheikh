using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.CRTD365.Controllers;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    [TestClass]
    public class TestCartController
    {
        string storeKey = "E550E995-1D34-4E65-9222-FA4C15712ADA";

        [TestMethod]
        public void TestGetCart()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

            CartController cartController = new CartController(storeKey);

            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            string cartId = "TMV_8602d2ff-ac07-44ad-adc5-eeb8f1e30600";
            try
            {
                erpCartResponse = cartController.GetCart(cartId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestCreateOrUpdateCart()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

            CartController cartController = new CartController(storeKey);

            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            ErpCart cart = new ErpCart();

            cart.Id = "TMV_" + Guid.NewGuid().ToString();

            try
            {
                erpCartResponse = cartController.CreateOrUpdateCart(cart, ErpCalculationModes.All);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestAddCartLines()
        {
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

                CartController cartController = new CartController(storeKey);
                
                string cartId = "TMV_8602d2ff-ac07-44ad-adc5-eeb8f1e30190";
                long cartVersion = 1955630;
                List<ErpCartLine> CartLines = new List<ErpCartLine>();

                ErpCartLine line = new ErpCartLine();

                //line.AttributeValues = null;
                //line.Barcode = "";
                //line.CatalogId = 0;
                //line.ChargeLines = null;
                //line.Comment = "";
                //line.CommissionSalesGroup = "";
                //line.DeliveryMode = "";
                //line.DeliveryModeChargeAmount = 0;
                //line.Description = "TeamViewer Corporate";
                //line.DiscountAmount = 0;
                //line.DiscountLines = null;
                //line.ElectronicDeliveryEmail = "";
                //line.ElectronicDeliveryEmailContent = "";
                //line.EntryMethodTypeValue = 2;
                //line.ExtendedPrice = 0;
                //line.ExtensionProperties = null;
                //line.FulfillmentStoreId = "";
                //line.InventoryDimensionId = "";
                //line.InvoiceAmount = 200.00M;
                //line.InvoiceId = "";
                //line.IsCustomerAccountDeposit = false;
                //line.IsGiftCardLine = false;
                //line.IsInvoiceLine = true;
                //line.IsPriceKeyedIn = false;
                //line.IsPriceOverridden = false;
                //line.IsVoided = false;
                //line.ItemId = "VN-004844";
                //line.ItemTaxGroupId = "";
                //line.LineDiscount = 0;
                //line.LineId = "01";
                //line.LineManualDiscountAmount = 0;
                //line.LineManualDiscountPercentage = 0;
                //line.LineNumber = 0;
                //line.LinePercentageDiscount = 0;
                //line.LinkedParentLineId = "";
                //line.NetAmountWithoutTax = 200.00M;
                //line.OriginalPrice = 0;
                //line.Price = 200.00M;
                //line.ProductId = 68719490059;
                //line.PromotionLines = null;
                //line.Quantity = 1;
                //line.QuantityInvoiced = 0;
                //line.QuantityOrdered = 0;
                //line.ReasonCodeLines = null;
                //line.RequestedDeliveryDate = DateTimeOffset.Now;
                //line.ReturnInventTransId = "";
                //line.ReturnLineNumber = 0;
                //line.ReturnTransactionId = "";
                //line.SalesStatusValue = 0;
                //line.SerialNumber = "";
                //line.ShippingAddress = null;
                //line.StaffId = "";
                //line.TaxAmount = 0;
                //line.TaxOverrideCode = "";
                //line.TaxRatePercent = 0;
                //line.TotalAmount = 200.00M;
                //line.TrackingId = "";
                //line.UnitOfMeasureSymbol = "";
                //line.WarehouseId = "";

                line.CatalogId = 0;
                line.CommissionSalesGroup = null;
                line.Description = "Team Viewer Corporate";
                line.EntryMethodTypeValue = 3;
                line.ItemId = "TVC0001"; //TVC0001_TVDE-00085
                line.ProductId = 5637144761;
                line.Quantity = 1;
                line.UnitOfMeasureSymbol = "pcs";
                line.LineId = "01";

                CartLines.Add(line);
            
                erpCartResponse = cartController.AddCartLines(cartId, CartLines, ErpCalculationModes.All, cartVersion);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestUpdateDeliverySpecification()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

            CartController cartController = new CartController(storeKey);

            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            //Dev02
            //string cartId = "TMV_8602d2ff-ac07-44ad-adc5-eeb8f1e30528";

            //INT
            string cartId = "TMV_8602d2ff-ac07-44ad-adc5-eeb8f1e11120";

            ErpDeliverySpecification deliverySpecification = new ErpDeliverySpecification();

            deliverySpecification.DeliveryModeId = "10";
            deliverySpecification.DeliveryPreferenceTypeValue = 1;

            ErpAddress shippingAddress = new ErpAddress();
            //shippingAddress.Name = "Jane Doe";
            //shippingAddress.Street = "One Microsoft Way";
            //shippingAddress.City = "Redmond";
            //shippingAddress.State = "WA";
            //shippingAddress.ZipCode = "98052";
            //shippingAddress.ThreeLetterISORegionName = "USA";
            //shippingAddress.Email = "JaneDoe@demo.com";
            //shippingAddress.AddressTypeValue = 2;
            //shippingAddress.IsPrimary = true;

            shippingAddress.Name = "Jane Doe";
            shippingAddress.Street = "Surinamestraat 27";
            shippingAddress.City = "Den Haag";
            //shippingAddress.State = "";
            shippingAddress.ZipCode = "2585 GJ";
            shippingAddress.ThreeLetterISORegionName = "NLD";
            shippingAddress.Email = "JaneDoe@demo.com";
            shippingAddress.AddressTypeValue = 2;
            shippingAddress.IsPrimary = true;
            shippingAddress.TaxGroup = "C-NL-STA";
            //shippingAddress.TaxGroup = "C-NL-RC";

            deliverySpecification.DeliveryAddress = shippingAddress;

            try
            {
                erpCartResponse = cartController.UpdateDeliverySpecification(cartId, deliverySpecification);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestVoidCartLines()
        {
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

                CartController cartController = new CartController(storeKey);

                string cartId = "8602d2ff-ac07-44ad-adc5-eeb8f1e30215";
                List<ErpCartLine> CartLines = new List<ErpCartLine>();

                ErpCartLine line = new ErpCartLine();

                line.AttributeValues = null;
                line.Barcode = "";
                line.CatalogId = 0;
                line.ChargeLines = null;
                line.Comment = "";
                line.CommissionSalesGroup = "";
                line.DeliveryMode = "";
                line.DeliveryModeChargeAmount = 0;
                line.Description = "TeamViewer Corporate";
                line.DiscountAmount = 0;
                line.DiscountLines = null;
                line.ElectronicDeliveryEmail = "";
                line.ElectronicDeliveryEmailContent = "";
                line.EntryMethodTypeValue = 2;
                line.ExtendedPrice = 0;
                line.ExtensionProperties = null;
                line.FulfillmentStoreId = "";
                line.InventoryDimensionId = "";
                line.InvoiceAmount = 200.00M;
                line.InvoiceId = "";
                line.IsCustomerAccountDeposit = false;
                line.IsGiftCardLine = false;
                line.IsInvoiceLine = true;
                line.IsPriceKeyedIn = false;
                line.IsPriceOverridden = false;
                line.IsVoided = true;
                line.ItemId = "VN-004844";
                line.ItemTaxGroupId = "";
                line.LineDiscount = 0;
                line.LineId = "01";
                line.LineManualDiscountAmount = 0;
                line.LineManualDiscountPercentage = 0;
                line.LineNumber = 0;
                line.LinePercentageDiscount = 0;
                line.LinkedParentLineId = "";
                line.NetAmountWithoutTax = 200.00M;
                line.OriginalPrice = 0;
                line.Price = 200.00M;
                line.ProductId = 68719490059;
                line.PromotionLines = null;
                line.Quantity = 1;
                line.QuantityInvoiced = 0;
                line.QuantityOrdered = 0;
                line.ReasonCodeLines = null;
                line.RequestedDeliveryDate = DateTimeOffset.Now;
                line.ReturnInventTransId = "";
                line.ReturnLineNumber = 0;
                line.ReturnTransactionId = "";
                line.SalesStatusValue = 0;
                line.SerialNumber = "";
                line.ShippingAddress = null;
                line.StaffId = "";
                line.TaxAmount = 0;
                line.TaxOverrideCode = "";
                line.TaxRatePercent = 0;
                line.TotalAmount = 200.00M;
                line.TrackingId = "";
                line.UnitOfMeasureSymbol = "";
                line.WarehouseId = "";

                CartLines.Add(line);

                erpCartResponse = cartController.VoidCartLines(cartId, CartLines, ErpCalculationModes.All);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestRemoveCartLines()
        {
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

                CartController cartController = new CartController(storeKey);

                string cartId = "8602d2ff-ac07-44ad-adc5-eeb8f1e30215";
                List<string> LineIds = new List<string>();

                string lineId = "01";
                LineIds.Add(lineId);

                erpCartResponse = cartController.RemoveCartLines(cartId, LineIds, ErpCalculationModes.All);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestUpdateCartLines()
        {
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

                CartController cartController = new CartController(storeKey);

                string cartId = "TMV_8602d2ff-ac07-44ad-adc5-eeb8f1e30186";
                List<ErpCartLine> CartLines = new List<ErpCartLine>();

                ErpCartLine line = new ErpCartLine();

                line.AttributeValues = null;
                line.Barcode = "";
                line.CatalogId = 0;
                line.ChargeLines = null;
                line.Comment = "";
                line.CommissionSalesGroup = "";
                line.DeliveryMode = "";
                line.DeliveryModeChargeAmount = 0;
                line.Description = "TeamViewer Corporate";
                line.DiscountAmount = 0;
                line.DiscountLines = null;
                line.ElectronicDeliveryEmail = "";
                line.ElectronicDeliveryEmailContent = "";
                line.EntryMethodTypeValue = 2;
                line.ExtendedPrice = 0;
                line.ExtensionProperties = null;
                line.FulfillmentStoreId = "";
                line.InventoryDimensionId = "";
                line.InvoiceAmount = 200.00M;
                line.InvoiceId = "";
                line.IsCustomerAccountDeposit = false;
                line.IsGiftCardLine = false;
                line.IsInvoiceLine = true;
                line.IsPriceKeyedIn = false;
                line.IsPriceOverridden = false;
                line.IsVoided = false;
                line.ItemId = "VN-004844";
                line.ItemTaxGroupId = "";
                line.LineDiscount = 0;
                line.LineId = "01";
                line.LineManualDiscountAmount = 0;
                line.LineManualDiscountPercentage = 0;
                line.LineNumber = 0;
                line.LinePercentageDiscount = 0;
                line.LinkedParentLineId = "";
                line.NetAmountWithoutTax = 200.00M;
                line.OriginalPrice = 0;
                line.Price = 200.00M;
                line.ProductId = 68719490059;
                line.PromotionLines = null;
                line.Quantity = 2;
                line.QuantityInvoiced = 0;
                line.QuantityOrdered = 0;
                line.ReasonCodeLines = null;
                line.RequestedDeliveryDate = DateTimeOffset.Now;
                line.ReturnInventTransId = "";
                line.ReturnLineNumber = 0;
                line.ReturnTransactionId = "";
                line.SalesStatusValue = 0;
                line.SerialNumber = "";
                line.ShippingAddress = null;
                line.StaffId = "";
                line.TaxAmount = 0;
                line.TaxOverrideCode = "";
                line.TaxRatePercent = 0;
                line.TotalAmount = 200.00M;
                line.TrackingId = "";
                line.UnitOfMeasureSymbol = "";
                line.WarehouseId = "";

                CartLines.Add(line);

                erpCartResponse = cartController.UpdateCartLines(cartId, CartLines, ErpCalculationModes.All);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestAddCouponsToCart()
        {
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

                CartController cartController = new CartController(storeKey);

                string cartId = "TMV_8602d2ff-ac07-44ad-adc5-eeb8f1e30204";
                bool isLegacyDiscountCode = false;

                List<string> CouponsCode = new List<string>();

                string couponCode = "CPN0004";
                CouponsCode.Add(couponCode);

                erpCartResponse = cartController.AddCouponsToCart(cartId, CouponsCode, isLegacyDiscountCode);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestRemoveCouponsFromCart()
        {
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

                CartController cartController = new CartController(storeKey);

                string cartId = "8602d2ff-ac07-44ad-adc5-eeb8f1e30215";

                List<string> CouponsCode = new List<string>();

                string couponCode = "2534";
                CouponsCode.Add(couponCode);

                erpCartResponse = cartController.RemoveCouponsFromCart(cartId, CouponsCode);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestAddDiscountCodesToCart()
        {
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

                CartController cartController = new CartController(storeKey);

                string cartId = "TMV_8602d2ff-ac07-44ad-adc5-eeb8f1e30207";

                List<string> DiscountCodes = new List<string>();

                //string discountCode = "CODE-0005";
                string discountCode = "CPN0004";
                DiscountCodes.Add(discountCode);

                erpCartResponse = cartController.AddDiscountCodesToCart(cartId, DiscountCodes);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestRemoveDiscountCodesFromCart()
        {
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

                CartController cartController = new CartController(storeKey);

                string cartId = "8602d2ff-ac07-44ad-adc5-eeb8f1e30215";

                List<string> DiscountCodes = new List<string>();

                string discountCode = "2534";
                DiscountCodes.Add(discountCode);

                erpCartResponse = cartController.RemoveDiscountCodesFromCart(cartId, DiscountCodes);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestAddPreprocessedTenderLine()
        {
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

                CartController cartController = new CartController(storeKey);

                string cartId = "8602d2ff-ac07-44ad-adc5-eeb8f1e30215";
                long cartVersion = 14196437;

                ErpTenderLine preprocessedTenderLine = new ErpTenderLine();

                preprocessedTenderLine.Amount = 200;
                preprocessedTenderLine.AmountInCompanyCurrency = 200;
                preprocessedTenderLine.AmountInTenderedCurrency = 200;
                preprocessedTenderLine.Authorization = "";
                preprocessedTenderLine.CardToken = "";
                preprocessedTenderLine.CardTypeId = "Visa";
                preprocessedTenderLine.CashBackAmount = 0;
                preprocessedTenderLine.CompanyCurrencyExchangeRate = 1;
                preprocessedTenderLine.CreditMemoId = "";
                preprocessedTenderLine.Currency = "USD";
                preprocessedTenderLine.CustomerId = "004822";
                preprocessedTenderLine.ExchangeRate = 1;
                preprocessedTenderLine.ExtensionProperties = null;
                preprocessedTenderLine.GiftCardId = "";
                preprocessedTenderLine.IncomeExpenseAccountTypeValue = 0;
                preprocessedTenderLine.IsChangeLine = true;
                preprocessedTenderLine.IsDeposit = true;
                preprocessedTenderLine.IsHistorical = true;
                preprocessedTenderLine.IsPreProcessed = true;
                preprocessedTenderLine.IsVoidable = true;
                preprocessedTenderLine.LineNumber = 0;
                preprocessedTenderLine.LoyaltyCardId = "";
                preprocessedTenderLine.MaskedCardNumber = "XXXX - XXXX - XXXX - 1111";
                preprocessedTenderLine.ReasonCodeLines = null;
                preprocessedTenderLine.SignatureData = "";
                preprocessedTenderLine.StatusValue = 1;
                preprocessedTenderLine.TenderDate = DateTimeOffset.Now;
                preprocessedTenderLine.TenderLineId = "01";
                preprocessedTenderLine.TenderTypeId = "3";
                preprocessedTenderLine.TransactionStatusValue = 0;
                preprocessedTenderLine.VoidStatusValue = 0;
                preprocessedTenderLine.CustomAttributes = null;


                erpCartResponse = cartController.AddPreprocessedTenderLine(cartId, preprocessedTenderLine, cartVersion);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestCheckout()
        {
            ErpCartCheckoutResponse erpCartCheckoutResponse = new ErpCartCheckoutResponse(false, "", null);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

                CartController cartController = new CartController(storeKey);

                string cartId = "TMV_8602d2ff-ac07-44ad-adc5-eeb8f1e30199";
                string receiptEmail = "test@customer.com";
                string receiptNumberSequence = "";
                long cartVersion = 14198988;

                erpCartCheckoutResponse = cartController.Checkout(cartId, receiptEmail, receiptNumberSequence, cartVersion);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartCheckoutResponse.SalesOrder.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartCheckoutResponse));
        }

        [TestMethod]
        public void TestRecalculateCustomerOrder()
        {
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

                CartController cartController = new CartController(storeKey);

                string cartId = "TMV_8602d2ff-ac07-44ad-adc5-eeb8f1e30157";
                
                erpCartResponse = cartController.RecalculateCustomerOrder(cartId);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse.Cart.Id);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }

        [TestMethod]
        public void TestDeleteCarts()
        {
            ErpCartResponse erpCartResponse = new ErpCartResponse(false, "", null);

            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ErpToRSMappingConfiguration objERPMappingConfiguration = new EDGEAXConnector.CRTD365.ErpToRSMappingConfiguration();

                CartController cartController = new CartController(storeKey);

                List<string> cartIds = new List<string>();

                string cartId = "TMV_8602d2ff-ac07-44ad-adc5-eeb8f1b1005";
                cartIds.Add(cartId);
                cartId = "TMV_8602d2ff-ac07-44ad-adc5-eeb8f1b1007";
                cartIds.Add(cartId);

                erpCartResponse = cartController.DeleteCarts(cartIds);
            }
            catch (System.Exception ex)
            {
                Assert.Fail("Exception: " + ex.Message.ToString());
            }

            Assert.IsNotNull(erpCartResponse);
            System.Console.WriteLine(JsonConvert.SerializeObject(erpCartResponse));
        }
    }
}
