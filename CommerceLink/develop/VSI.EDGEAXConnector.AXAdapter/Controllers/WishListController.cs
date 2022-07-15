//using Autofac;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using VSI.EDGEAXConnector.AXAdapter.CRTFactory;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Logging;

namespace VSI.EDGEAXConnector.AXAdapter.Controllers
{
    public class WishListController : BaseController, IWishListController
    {


        public WishListController(string storeKey) : base(storeKey)
        {
                
        }
        #region " Private Methods "

        /// <summary>
        /// Validate and Convert Customer
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private string ValidateCustomer(string customerId)
        {
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            IntegrationKey customerIntegrationKey = integrationManager.GetErpKey(Enums.Entities.Customer, customerId);

            if (customerIntegrationKey == null)
            {
                throw new Exception("Customer does not exists, please create customer first.");
            }
            else
            {
                // Update CustomerId from ComKey to Description
                return customerIntegrationKey.Description;
            }
        }

        /// <summary>
        /// Validate Product
        /// </summary>
        /// <param name="commerceListLine"></param>
        /// <returns></returns>
        private ErpCommerceListLine ValidateAndConvertProduct(ErpCommerceListLine erpCommerceListLine, bool validateCustomerInCommerceListLine = false)
        {
            if (validateCustomerInCommerceListLine)
            {
                erpCommerceListLine.CustomerId = ValidateCustomer(erpCommerceListLine.CustomerId);
            }
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            IntegrationKey productIntegrationKey = integrationManager.GetErpKey(Enums.Entities.Product, erpCommerceListLine.ItemId.ToString());
            if (productIntegrationKey == null)
            {
                throw new Exception("Product does not exists");
            }
            else
            {
                // Update Product Id from ComKey to ErpKey
                erpCommerceListLine.ItemId = productIntegrationKey.ErpKey;
            }
            return erpCommerceListLine;
        }

        /// <summary>
        /// Validate and Convert Customer and Product
        /// </summary>
        /// <param name="commerceList"></param>
        /// <returns></returns>
        private ErpCommerceList ValidateAndConvertCustomerAndProduct(ErpCommerceList erpCommerceList)
        {
            erpCommerceList.CustomerId = ValidateCustomer(erpCommerceList.CustomerId);

            // Check if product exists, if yes then update product id
            if (erpCommerceList.CommerceListLines != null)
            { 
                for (int commerceListLineIndex = 0; commerceListLineIndex < erpCommerceList.CommerceListLines.Count; commerceListLineIndex++)
                {
                    erpCommerceList.CommerceListLines[commerceListLineIndex] = ValidateAndConvertProduct(erpCommerceList.CommerceListLines[commerceListLineIndex], true);
                }
            }

            return erpCommerceList;
        }

        #endregion

        public ErpCommerceList CreateWishList(ErpCommerceList erpCommerceList)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpCommerceList));

            erpCommerceList = ValidateAndConvertCustomerAndProduct(erpCommerceList);

            var wishListCRTManager = new WishListCRTManager();
            return wishListCRTManager.CreateWishList(erpCommerceList, currentStore.StoreKey);
        }

        public ErpCommerceList CreateWishListLine(ErpCommerceListLine erpCommerceListLine, string filterAccountNumber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "erpCommerceListLine: " + JsonConvert.SerializeObject(erpCommerceListLine) + ", filterAccountNumber: " + filterAccountNumber);

            if (!String.IsNullOrEmpty(filterAccountNumber) && !String.IsNullOrWhiteSpace(filterAccountNumber))
            { 
                filterAccountNumber = ValidateCustomer(filterAccountNumber);
            }

            erpCommerceListLine = ValidateAndConvertProduct(erpCommerceListLine, true);

            var wishListCRTManager = new WishListCRTManager();
            return wishListCRTManager.CreateWishListLine(erpCommerceListLine, filterAccountNumber, currentStore.StoreKey);
        }

        public bool DeleteWishList(long WishListId, string customerId)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "WishListId: " + WishListId.ToString() + ", customerId: " + customerId);

            if (!String.IsNullOrEmpty(customerId) && !String.IsNullOrWhiteSpace(customerId))
            {
                customerId = ValidateCustomer(customerId);
            }

            var wishListCRTManager = new WishListCRTManager();
            return wishListCRTManager.DeleteWishList(WishListId, customerId, currentStore.StoreKey);
        }

        public ErpCommerceList DeleteWishListLine(long wishListLineId, long wishListId, string filterAccountNumber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "wishListLineId: " + wishListLineId.ToString() + ", wishListId: " + wishListId.ToString() + ", filterAccountNumber: " + filterAccountNumber);

            if (!String.IsNullOrEmpty(filterAccountNumber) && !String.IsNullOrWhiteSpace(filterAccountNumber))
            {
                filterAccountNumber = ValidateCustomer(filterAccountNumber);
            }

            var wishListCRTManager = new WishListCRTManager();
            return wishListCRTManager.DeleteWishListLine(wishListLineId, wishListId, filterAccountNumber, currentStore.StoreKey);
        }

        public List<ErpCommerceList> GetWishList(long wishListId, string customerId, bool favoriteFilter, bool publicFilter)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "wishListId: " + wishListId.ToString() + ", customerId: " + customerId + ", favoriteFilter: " + favoriteFilter.ToString() + ",publicFilter: " + publicFilter.ToString());

            if (!String.IsNullOrEmpty(customerId) && !String.IsNullOrWhiteSpace(customerId) && customerId != "0")
            {
                customerId = ValidateCustomer(customerId);
            }

            var wishListCRTManager = new WishListCRTManager();
            List<ErpCommerceList> erpCommerceLists = new List<ErpCommerceList>();
            erpCommerceLists = wishListCRTManager.GetWishList(wishListId, customerId, favoriteFilter, publicFilter, currentStore.StoreKey);
            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
            for (int erpCommerceListLineIndex = 0; erpCommerceListLineIndex < erpCommerceLists.Count; erpCommerceListLineIndex++)
            {
                // erpCommerceLists[erpCommerceListLineIndex].CustomerId = customerId;
                IntegrationKey customerIntegrationKey = integrationManager.GetKeyByDescription(Enums.Entities.Customer, erpCommerceLists[erpCommerceListLineIndex].CustomerId.ToString());
                erpCommerceLists[erpCommerceListLineIndex].CustomerId = customerIntegrationKey.ComKey;

                foreach (ErpCommerceListLine erpCommerceListLine in erpCommerceLists[erpCommerceListLineIndex].CommerceListLines)
                {
                    erpCommerceListLine.CustomerId = erpCommerceLists[erpCommerceListLineIndex].CustomerId;

                    string productId = erpCommerceListLine.ProductId.ToString();
                    IntegrationKey productIntegrationKey = integrationManager.GetComKey(Enums.Entities.Product, erpCommerceListLine.ItemId.ToString());
                    erpCommerceListLine.ItemId = productIntegrationKey.ComKey;
                }
            }
            return erpCommerceLists;
        }

        public ErpCommerceList UpdateWishListLine(ErpCommerceListLine erpWishListLine, string filterAccountNumber)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name,
                "erpWishListLine: " + JsonConvert.SerializeObject(erpWishListLine) + ", filterAccountNumber: " + filterAccountNumber);

            if (!String.IsNullOrEmpty(filterAccountNumber) && !String.IsNullOrWhiteSpace(filterAccountNumber))
            {
                filterAccountNumber = ValidateCustomer(filterAccountNumber);
            }

            erpWishListLine = ValidateAndConvertProduct(erpWishListLine, true);
            var wishListCRTManager = new WishListCRTManager();
            return wishListCRTManager.UpdateWishListLine(erpWishListLine, filterAccountNumber, currentStore.StoreKey);
        }
    }
}
