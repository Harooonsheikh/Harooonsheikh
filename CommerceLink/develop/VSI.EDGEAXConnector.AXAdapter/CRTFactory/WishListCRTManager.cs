using System.Collections.Generic;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public class WishListCRTManager
    {

        private readonly ICRTFactory _crtFactory;

        public WishListCRTManager()
        {
            _crtFactory = new CRTFactory();
        }

        public ErpCommerceList CreateWishList(ErpCommerceList erpCommerceList, string storeKey)
        {
            var wishListController = _crtFactory.CreateWishListController(storeKey);
            return wishListController.CreateWishList(erpCommerceList);
        }

        public ErpCommerceList CreateWishListLine(ErpCommerceListLine erpCommerceListLine, string filterAccountNumber, string storeKey)
        {
            var wishListController = _crtFactory.CreateWishListController(storeKey);
            return wishListController.CreateWishListLine(erpCommerceListLine, filterAccountNumber);
        }

        public bool DeleteWishList(long WishListId, string customerId, string storeKey)
        {
            var wishListController = _crtFactory.CreateWishListController(storeKey);
            return wishListController.DeleteWishList(WishListId, customerId);
        }

        public ErpCommerceList DeleteWishListLine(long wishListLineId, long wishListId, string filterAccountNumber, string storeKey)
        {
            var wishListController = _crtFactory.CreateWishListController(storeKey);
            return wishListController.DeleteWishListLine(wishListLineId, wishListId, filterAccountNumber);
        }

        public List<ErpCommerceList> GetWishList(long wishListId, string customerId, bool favoriteFilter, bool publicFilter, string storeKey)
        {
            var wishListController = _crtFactory.CreateWishListController(storeKey);
            return wishListController.GetWishList(wishListId, customerId, favoriteFilter, publicFilter);
        }

        public ErpCommerceList UpdateWishListLine(ErpCommerceListLine erpWishListLine, string filterAccountNumber, string storeKey)
        {
            var wishListController = _crtFactory.CreateWishListController(storeKey);
            return wishListController.UpdateWishListLine(erpWishListLine, filterAccountNumber);
        }
    }
}
