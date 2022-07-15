using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class GiftCardCRTManager
    {
        private readonly ICRTFactory _crtFactory;

        public GiftCardCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public ErpGiftCard GetGiftCardBalance(string giftCardId, string storeKey)
        {
            var giftCardController = _crtFactory.CreateGiftCardController(storeKey);
            return giftCardController.GetGiftCardBalance(giftCardId);
        }

        public ErpGiftCard IssueGiftCard(string giftCardId, decimal amount, string currencycode, long channelId, string terminalId, string staffId, string transactionId, string receiptId, string storeKey)
        {
            var giftCardController = _crtFactory.CreateGiftCardController(storeKey);
            return giftCardController.IssueGiftCard(giftCardId, amount, currencycode, channelId, terminalId, staffId, transactionId, receiptId);
            
        }

        public ErpGiftCard LockGiftCard(string giftCardId, long channelId, string terminalId, string storeKey)
        {
            var giftCardController = _crtFactory.CreateGiftCardController(storeKey);
            return giftCardController.LockGiftCard(giftCardId, channelId, terminalId);
            
        }

        public bool PayGiftCard(string giftCardId, decimal paymentAmount, string paymentCurrencyCode, long channelId, string terminalId, string staffId, string transactionId, string receiptId, string storeKey)
        {
            var giftCardController = _crtFactory.CreateGiftCardController(storeKey);
            return giftCardController.PayGiftCard(giftCardId, paymentAmount, paymentCurrencyCode, channelId, terminalId, staffId, transactionId, receiptId);            
        }

        public bool UnlockGiftCard(string giftCardId, string storeKey)
        {
            var giftCardController = _crtFactory.CreateGiftCardController(storeKey);
             return giftCardController.UnlockGiftCard(giftCardId);
        }

    }
}
