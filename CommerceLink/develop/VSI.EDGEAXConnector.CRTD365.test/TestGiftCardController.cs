using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VSI.EDGEAXConnector.CRTD365.Controllers;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRTD365.test
{
    [TestClass]
    public class TestGiftCardController
    {
        string storeKey = "";

        [TestMethod]
        public void testGetGiftCardBalance()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ErpGiftCard giftCard = new ErpGiftCard();

            //Always change this number
            string giftCardId = "GC1001001113";

            try
            {
                GiftCardController giftCardController = new GiftCardController(storeKey);
                giftCard = giftCardController.GetGiftCardBalance(giftCardId);
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }
            
            Assert.IsNotNull(giftCard);
            System.Console.WriteLine("GiftCardId = " + giftCard.Id.ToString() + ", CurrencyCode = " + giftCard.CardCurrencyCode + ", Balance = " + giftCard.Balance);
        }


        [TestMethod]
        public void testIssueGiftCard()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ErpGiftCard giftCard = new ErpGiftCard();

            long channelId = 5637145359;

            //Always change this number
            string giftCardId = "GC1001001113";
            string transactionId = "CL_0004e53a-0cf6-4224-86c2-c09d2ffe78fg";
            decimal amount = 1000;
            string currencyCode = "USD";

            try
            {
                GiftCardController giftCardController = new GiftCardController(storeKey);
                giftCard = giftCardController.IssueGiftCard(giftCardId, amount, currencyCode, channelId, string.Empty, string.Empty, transactionId, string.Empty);
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }
            
            Assert.IsNotNull(giftCard);
            System.Console.WriteLine("NewGiftCardId = " + giftCard.Id + ", Balance = " + giftCard.Balance);
        }

        [TestMethod]
        public void testPayGiftCard()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ErpGiftCard giftCard = new ErpGiftCard();

            long channelId = 5637145359;

            //Always change this number
            string giftCardId = "GC1001001113";
            string transactionId = "CL_0004e53a-0cf6-4224-86c2-c09d2ffe78fg";
            decimal paymentAmount = 50;
            string paymentCurrencyCode = "USD";

            bool response = false;

            try
            {
                GiftCardController giftCardController = new GiftCardController(storeKey);
                response = giftCardController.PayGiftCard(giftCardId, paymentAmount, paymentCurrencyCode, channelId, string.Empty, string.Empty, transactionId, string.Empty);
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(response);
            System.Console.WriteLine("GiftCardId = " + giftCard.Id);
        }

        [TestMethod]
        public void testLockGiftCard()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            ErpGiftCard giftCard = new ErpGiftCard();

            long channelId = 5637145359;

            //Always change this number
            string giftCardId = "GC1001001113";

            try
            {
                GiftCardController giftCardController = new GiftCardController(storeKey);
                giftCard = giftCardController.LockGiftCard(giftCardId, channelId, string.Empty);
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(giftCard);
            System.Console.WriteLine("GiftCardId = " + giftCard.Id + ", Balance = " + giftCard.Balance);
        }

        [TestMethod]
        public void testUnlockGiftCard()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                       
            //Always change this number
            string giftCardId = "GC1001001113";

            bool response = false;

            try
            {
                GiftCardController giftCardController = new GiftCardController(storeKey);
                response = giftCardController.UnlockGiftCard(giftCardId);                
            }
            catch (System.Exception e)
            {
                Assert.Fail("Exception : " + e.StackTrace);
            }

            Assert.IsNotNull(response);
            System.Console.WriteLine("GiftCardId = " + giftCardId);
        }

    }
}
