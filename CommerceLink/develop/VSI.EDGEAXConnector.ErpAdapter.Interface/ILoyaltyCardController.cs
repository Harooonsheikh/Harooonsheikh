using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.ErpAdapter.Interface
{
    public interface ILoyaltyCardController
    {
        ErpLoyaltyCard GetLoyaltyCardStatus(string loyaltyCardNo);
        ErpLoyaltyCard IssueLoyaltyCard(string loyaltyCardNo, string customerAcct);
        List<ErpLoyaltyCard> GetLoyaltyCardRewardPointsStatus(DateTimeOffset channelLocalDate, string loyaltyCardNumber, bool excludeBlocked, 
            bool excludeNoTender, bool includeRelatedCardsForContactTender, bool includeNonRedeemablePoints, bool includeActivePointsOnly, string locale);
        List<ErpLoyaltyCardTransaction> GetLoyaltyCardTransactions(string loyaltyCardNumber, string rewardPointId, long top, long skip, bool calculateRecordCount);

        List<ErpLoyaltyCard> GetCustomerLoyaltyCards(string customerAcct);
        bool PostLoyaltyCardRewardPointTrans(ErpSalesTransaction transcation, ErpLoyaltyRewardPointEntryType type);

    }
}
