using Microsoft.Dynamics.Commerce.RetailProxy;
using NewRelic.Api.Agent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;

namespace VSI.EDGEAXConnector.CRTD365.Controllers
{
    public class WishListController : BaseController, IWishListController
    {

        public WishListController(string storeKey) : base(storeKey)
        {

        }
        public ErpCommerceList CreateWishList(ErpCommerceList erpCommerceList)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceList commerceList = new CommerceList();
            commerceList = _mapper.Map<ErpCommerceList, CommerceList>(erpCommerceList);

            var commerceListResult = ECL_CreateWishList(commerceList);
            //CommerceList commerceListResult = Task.Run(async () => await commerceListManager.Create(commerceList)).Result;

            ErpCommerceList erpCommerceListResult = new ErpCommerceList();
            erpCommerceListResult = _mapper.Map<CommerceList, ErpCommerceList>(commerceListResult);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCommerceListResult;
        }
        public List<ErpCommerceList> GetWishList(long wishListId, string customerId, bool favoriteFilter, bool publicFilter)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);


            QueryResultSettings queryResultSettings = new QueryResultSettings();
            queryResultSettings.Paging = new PagingInfo();
            queryResultSettings.Paging.Top = 1000;
            queryResultSettings.Paging.Skip = 0;
            // PagedResult<CommerceList> commerceListResult = Task.Run(async () => await commerceListManager.ECL_GetWishLists(wishListId, customerId, favoriteFilter, publicFilter, queryResultSettings)).Result;
            var commerceListResult = ECL_GetWishLists(wishListId, customerId, favoriteFilter, publicFilter, queryResultSettings);
            List<CommerceList> commerceList = new List<CommerceList>();
            commerceList = commerceListResult.ToList<CommerceList>();
            List<ErpCommerceList> erpCommerceList = new List<ErpCommerceList>();
            erpCommerceList = _mapper.Map<List<CommerceList>, List<ErpCommerceList>>(commerceList) ??
                              new List<ErpCommerceList>();

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCommerceList;
        }
        public bool DeleteWishList(long wishListId, string customerId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            var response = ECL_DeleteWishList(wishListId, customerId);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);
            return response;
        }
        public ErpCommerceList CreateWishListLine(ErpCommerceListLine erpCommerceListLine, string filterAccountNumber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceListLine commerceListLine = new CommerceListLine();
            commerceListLine = _mapper.Map<ErpCommerceListLine, CommerceListLine>(erpCommerceListLine);

            var commerceListResult = ECL_CreateWishListLine(filterAccountNumber, commerceListLine);

            ErpCommerceList erpCommerceListResult = new ErpCommerceList();
            erpCommerceListResult = _mapper.Map<CommerceList, ErpCommerceList>(commerceListResult);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCommerceListResult;
        }
        public ErpCommerceList DeleteWishListLine(long wishListLineId, long wishListId, string filterAccountNumber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            var commerceListResult = ECL_DeleteWishListLine(wishListLineId, wishListId, filterAccountNumber);

            ErpCommerceList erpCommerceListResult = new ErpCommerceList();
            erpCommerceListResult = _mapper.Map<CommerceList, ErpCommerceList>(commerceListResult);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCommerceListResult;
        }
        public ErpCommerceList UpdateWishListLine(ErpCommerceListLine erpWishListLine, string filterAccountNumber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceListLine wishListLine = new CommerceListLine();
            wishListLine = _mapper.Map<ErpCommerceListLine, CommerceListLine>(erpWishListLine);

            var commerceListResult = ECL_UpdateWishListLine(filterAccountNumber, wishListLine);

            ErpCommerceList erpCommerceListResult = new ErpCommerceList();
            erpCommerceListResult = _mapper.Map<CommerceList, ErpCommerceList>(commerceListResult);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10004, currentStore, MethodBase.GetCurrentMethod().Name);

            return erpCommerceListResult;
        }

        #region RetailServer API
        [Trace]
        private CommerceList ECL_CreateWishList(CommerceList commerceList)
        {
            throw new NotImplementedException();
            //var commerceListManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICommerceListManager>();
            //CommerceList commerceListResult =
            //    Task.Run(async () => await commerceListManager.ECL_CreateWishList(commerceList, baseCompany)).Result;
            //return commerceListResult;
        }
        [Trace]
        private PagedResult<CommerceList> ECL_GetWishLists(long wishListId, string customerId, bool favoriteFilter, bool publicFilter,
            QueryResultSettings queryResultSettings)
        {
            throw new NotImplementedException();
            //var commerceListManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICommerceListManager>();
            //PagedResult<CommerceList> commerceListResult = Task.Run(async () => await commerceListManager.ECL_GetWishLists(wishListId, customerId, favoriteFilter, publicFilter, baseCompany,
            //        queryResultSettings)).Result;
            //return commerceListResult;
        }
        [Trace]
        private bool ECL_DeleteWishList(long wishListId, string customerId)
        {
            throw new NotImplementedException();
            //var commerceListManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICommerceListManager>();
            //return Task.Run( async () => await commerceListManager.ECL_DeleteWishList(wishListId, customerId, baseCompany)).Result;
        }
        [Trace]
        private CommerceList ECL_CreateWishListLine(string filterAccountNumber, CommerceListLine commerceListLine)
        {
            throw new NotImplementedException();
            //var commerceListManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICommerceListManager>();
            //CommerceList commerceListResult = Task.Run(async () => await
            //    commerceListManager.ECL_CreateWishListLine(commerceListLine, filterAccountNumber, baseCompany)).Result;
            //return commerceListResult;
        }
        [Trace]
        private CommerceList ECL_DeleteWishListLine(long wishListLineId, long wishListId, string filterAccountNumber)
        {
            throw new NotImplementedException();
            //var commerceListManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICommerceListManager>();
            //CommerceList commerceListResult = Task.Run(async () => await
            //        commerceListManager.ECL_DeleteWishListLine(wishListLineId, wishListId, filterAccountNumber, baseCompany))
            //    .Result;
            //return commerceListResult;
        }
        [Trace]
        private CommerceList ECL_UpdateWishListLine(string filterAccountNumber, CommerceListLine wishListLine)
        {
            throw new NotImplementedException();
            //var commerceListManager = RPFactory.GetManager<EdgeAXCommerceLink.RetailProxy.Extensions.ICommerceListManager>();
            //CommerceList commerceListResult = Task.Run(async () => await
            //    commerceListManager.ECL_UpdateWishListLine(wishListLine, filterAccountNumber, baseCompany)).Result;
            //return commerceListResult;
        }

        #endregion
    }
}
