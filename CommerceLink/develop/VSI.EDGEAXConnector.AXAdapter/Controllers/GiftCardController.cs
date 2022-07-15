//using Autofac;
using System;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class GiftCardController : BaseController, IGiftCardController
    {
        public GiftCardController(string storeKey) : base(storeKey)
        {
        }

        public ErpGiftCard GetGiftCardBalance(string giftCardId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var giftCardCRTManager = new GiftCardCRTManager();
            return giftCardCRTManager.GetGiftCardBalance(giftCardId, currentStore.StoreKey);
        }

        public ErpGiftCard IssueGiftCard(string giftCardId, decimal amount, string currencyCode, long channelId, string termincalId, string staffId, string transactionId, string receiptId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var giftCardCRTManager = new GiftCardCRTManager();
            return giftCardCRTManager.IssueGiftCard(giftCardId, amount, currencyCode, channelId, termincalId, staffId, transactionId, receiptId, currentStore.StoreKey);
        }

        public ErpGiftCard LockGiftCard(string giftCardId, long channelId, string terminalId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var giftCardCRTManager = new GiftCardCRTManager();
            return giftCardCRTManager.LockGiftCard(giftCardId, channelId, terminalId, currentStore.StoreKey);
        }

        public bool PayGiftCard(string giftCardId, decimal paymentAmount, string paymentCurrencyCode, long channelId, string terminalId, string staffId, string transactionId, string receiptId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var giftCardCRTManager = new GiftCardCRTManager();
            return giftCardCRTManager.PayGiftCard(giftCardId, paymentAmount, paymentCurrencyCode, channelId, terminalId, staffId, transactionId, receiptId, currentStore.StoreKey);
        }

        public bool UnlockGiftCard(string giftCardId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var giftCardCRTManager = new GiftCardCRTManager();
            return giftCardCRTManager.UnlockGiftCard(giftCardId, currentStore.StoreKey);
        }
    }
}
