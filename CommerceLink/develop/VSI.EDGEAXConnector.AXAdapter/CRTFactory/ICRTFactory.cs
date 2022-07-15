using System;
using VSI.EDGEAXConnector.AXAdapter.Controllers;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.AXAdapter.CRTFactory
{
    public interface ICRTFactory
    {

		IUpdateSalesOrderController CreateUpdateSalesOrderController(string storeKey);
        ISalesOrderController CreateSalesOrderController(string storeKey);
        IStoreController CreateStoreController(string storeKey);
        ICustomerController CreateCustomerController(string storeKey);
        IInventoryController CreateInventoryController(string storeKey);
        IDiscountController CreateDiscountController(string storeKey);
        IDiscountWithAffiliationController CreateDiscountWithAffiliationController(string storeKey);
        IProductController CreateProductController(string storeKey);
        IPriceController CreatePriceController(string storeKey);
        IShippingController CreateShippingController(string storeKey);
        IGiftCardController CreateGiftCardController(string storeKey);
        IChannelPublishingController CreateChannelPublishingController(string storeKey);
        IWishListController CreateWishListController(string storeKey);
        IContactPersonController CreateContactPersonController(string storeKey);
        IQuotationController CreateQuotationController(string storeKey);
        IPaymentController CreatePaymentController(string storeKey);
        ICartController CreateCartController(string storeKey);        
        IChannelConfigurationController CreateConfigurationController(string storeKey);
        IOfferTypeGroupsController CreateOfferTypeGroupController(string storeKey);
        IDQSController CreateDQSController(string storeKey);
        IQuantityDiscountController CreateQuantityDiscountController(string storeKey);
        IQuantityDiscountWithAffiliationController CreateQuantityDiscountWithAffiliationController(string storeKey);
        IInAppPurchaseController CreateInAppPurchaseController(string storeKey);
        ICustomerPortalController CreateCustomerPortalController(string storeKey);
        ISubscriptionController CreateSubscriptionController(string storeKey);
    }

    public class CRTFactory : ICRTFactory
    {
        
        public ISalesOrderController CreateSalesOrderController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.SalesOrderController(storeKey);
        }

        public IUpdateSalesOrderController CreateUpdateSalesOrderController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.UpdateSalesOrderController(storeKey);
        }

        public IStoreController CreateStoreController(string storeKey)

        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.StoreController(storeKey);
        }

        public ICustomerController CreateCustomerController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.CustomerController(storeKey);
        }

        public IInventoryController CreateInventoryController(string storeKey)
        {
            throw new NotImplementedException();
            //return new VSI.EDGEAXConnector.CRTD365.Controllers.InventoryController(storeKey);
        }

        public IDiscountController CreateDiscountController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.DiscountController(storeKey);
        }

        public IDiscountWithAffiliationController CreateDiscountWithAffiliationController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.DiscountWithAffiliationController(storeKey);
        }

        public IProductController CreateProductController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.ProductController(storeKey);
        }

        public IPriceController CreatePriceController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.PriceController(storeKey);
        }

        public IShippingController CreateShippingController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.ShippingController(storeKey);
        }

        public IGiftCardController CreateGiftCardController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.GiftCardController(storeKey);
        }

        public IChannelPublishingController CreateChannelPublishingController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.ChannelPublishingController(storeKey);
        }

        public IWishListController CreateWishListController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.WishListController(storeKey);
        }

        public IContactPersonController CreateContactPersonController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.ContactPersonController(storeKey);
        }

        public IQuotationController CreateQuotationController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.QuotationController(storeKey);
        }
        public IPaymentController CreatePaymentController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.PaymentController(storeKey);
        }
        public IChannelConfigurationController CreateConfigurationController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.ChannelConfigurationController(storeKey);
        }

        public IOfferTypeGroupsController CreateOfferTypeGroupController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.OfferTypeGroupController(storeKey);
        }

        public ICartController CreateCartController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.CartController(storeKey);
        }

        public IDQSController CreateDQSController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.DQSController(storeKey);
        }

        public IQuantityDiscountController CreateQuantityDiscountController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.QuantityDiscountController(storeKey);
        }

        public IQuantityDiscountWithAffiliationController CreateQuantityDiscountWithAffiliationController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.QuantityDiscountWithAffiliationController(storeKey);
        }

        public IInAppPurchaseController CreateInAppPurchaseController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.InAppPurchaseController(storeKey);
        }

        public ICustomerPortalController CreateCustomerPortalController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.CustomerPortalController(storeKey);
        }

        public ISubscriptionController CreateSubscriptionController(string storeKey)
        {
            return new VSI.EDGEAXConnector.CRTD365.Controllers.SubscriptionController(storeKey);
        }
    }
}
