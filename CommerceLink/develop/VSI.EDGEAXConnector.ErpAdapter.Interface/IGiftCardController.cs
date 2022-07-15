using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface IGiftCardController
    {
        ErpGiftCard IssueGiftCard(string giftCardId, decimal amount, string currencyCode, long channelId, string termincalId, string staffId, string transactionId, string receiptId);
        ErpGiftCard GetGiftCardBalance(string giftCardId);

        ErpGiftCard LockGiftCard(string giftCardId, long channelId, string terminalId);

        bool UnlockGiftCard(string giftCardId);

        bool PayGiftCard(string giftCardId, decimal paymentAmount, string paymentCurrencyCode, long channelId, string terminalId, string staffId, string transactionId, string receiptId);

    }
}
