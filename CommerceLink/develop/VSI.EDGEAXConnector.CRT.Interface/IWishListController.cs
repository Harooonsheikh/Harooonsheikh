using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRT.Interface
{
    public interface IWishListController
    {
        ErpCommerceList CreateWishList(ErpCommerceList erpCommerceList);

        List<ErpCommerceList> GetWishList(long wishListId, string customerId, bool favoriteFilter, bool publicFilter);

        bool DeleteWishList(long WishListId, string customerId);

        ErpCommerceList CreateWishListLine(ErpCommerceListLine erpCommerceListLine, string filterAccountNumber);

        ErpCommerceList DeleteWishListLine(long wishListLineId, long wishListId, string filterAccountNumber);

        ErpCommerceList UpdateWishListLine(ErpCommerceListLine erpWishListLine, string filterAccountNumber);
    }
}
