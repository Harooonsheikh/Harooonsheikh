using System;
using VSI.CommerceLink.MagentoAdapter.Controllers;
////using Autofac;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.Web
{
    #region ERP Adapter Factory
    /// <summary>
    /// ERP Adapter implementation factory.
    /// </summary>
    public interface IErpAdapterFactory
    {
        /// <summary>
        /// Create a shipping controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.IShippingController CreateShippingController(string storeKey);

        /// <summary>
        /// Create a gift card controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.IGiftCardController CreateGiftCardController(string storeKey);

        /// <summary>
        /// Create a store controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.IStoreController CreateStoreController(string storeKey);

        /// <summary>
        /// Create a sales order controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.ISaleOrderController CreateSalesOrderController(string storeKey);

        /// <summary>
        /// Create a update sales order controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.IUpdateSalesOrderController CreateUpdateSalesOrderController(string storeKey);

        /// <summary>
        /// Create a sales order controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.ICustomerController CreateCustomerController(string storeKey);

        /// <summary>
        /// Create a contact person controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.IContactPersonController CreateContactPersonController(string storeKey);

        /// <summary>
        /// Create a Wish List controller
        /// </summary>
        /// <returns></returns>
        IWishListController CreateWishListController(string storeKey);

        /// <summary>
        /// Create a discount controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.IDiscountController CreateDiscountController(string storeKey);
        /// <summary>
        /// Create a quotation controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.IQuotationController CreateQuotationController(string storeKey);

        /// <summary>
        /// Create a payment controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.IPaymentController CreatePaymentController(string storeKey);

        /// <summary>
        /// Create a cart controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.ICartController CreateCartController(string storeKey);

        /// <summary>
        /// Create a dqs controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.IDQSController CreateDQSController(string storeKey);

        /// <summary>
        /// Creates an InAppPurchase Controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.IInAppPurchaseController CreateInAppPurchaseController(string storeKey);

        /// <summary>
        /// Creates an Product Controller.
        /// </summary>
        /// <returns></returns>
        ErpAdapter.Interface.IProductController CreateProductController(string storeKey);

        /// <summary>
        /// Creates an Customer Portal Controller
        /// </summary>
        /// <param name="storeKey"></param>
        /// <returns></returns>
        ErpAdapter.Interface.ICustomerPortalController CreateCustomerPortalController(string storeKey);

        /// <summary>
        /// Creates an Discount with affiliation Controller
        /// </summary>
        /// <param name="storeKey"></param>
        /// <returns></returns>
        ErpAdapter.Interface.IDiscountWithAffiliationController CreateDiscountWithAffiliationController(string storeKey);
    }

    /// <summary>
    /// Find the right ERP adapter implementation.
    /// </summary>
    public class ErpAdapterFactory : IErpAdapterFactory
    {
        //private readonly IComponentContext _container;

        /// <summary>
        /// Factory to find right implementation.
        /// </summary>
        /// <param name="container"></param>
        //public ErpAdapterFactory(IComponentContext container)
        //{
        //    _container = container;
        //}

        /// <summary>
        /// Resolve the right implementation of shipping controller.
        /// </summary>
        /// <returns></returns>
        public ErpAdapter.Interface.IShippingController CreateShippingController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.ShippingController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of gift card controller.
        /// </summary>
        /// <returns></returns>
        public ErpAdapter.Interface.IGiftCardController CreateGiftCardController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.GiftCardController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of store controller.
        /// </summary>
        /// <returns></returns>
        public ErpAdapter.Interface.IStoreController CreateStoreController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.StoreLocationController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of sales order controller.
        /// </summary>
        /// <returns></returns>
        public ErpAdapter.Interface.ISaleOrderController CreateSalesOrderController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.SaleOrderController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of update sales order controller.
        /// </summary>
        /// <returns></returns>
        public ErpAdapter.Interface.IUpdateSalesOrderController CreateUpdateSalesOrderController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.UpdateSalesOrderController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of customer controller.
        /// </summary>
        /// <returns></returns>
        public ErpAdapter.Interface.ICustomerController CreateCustomerController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.CustomerController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of wish list controller
        /// </summary>
        /// <returns></returns>
        public IWishListController CreateWishListController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.WishListController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of Contact Person Controller
        /// </summary>
        /// <returns></returns>
        public IContactPersonController CreateContactPersonController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.ContactPersonController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of Discount Controller
        /// </summary>
        /// <returns></returns>
        public ErpAdapter.Interface.IDiscountController CreateDiscountController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.DiscountController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of Quotation Controller
        /// </summary>
        /// <returns></returns>
        public ErpAdapter.Interface.IQuotationController CreateQuotationController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.QuotationController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of Payment Controller
        /// </summary>
        /// <returns></returns>
        public ErpAdapter.Interface.IPaymentController CreatePaymentController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.PaymentController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of Cart Controller
        /// </summary>
        /// <returns></returns>
        public ICartController CreateCartController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.CartController(storeKey);
        }


        /// <summary>
        /// Resolve the right implementation of DQS Controller
        /// </summary>
        /// <returns></returns>
        public IDQSController CreateDQSController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.DQSController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of InAppPurchase Controller
        /// </summary>
        /// <returns></returns>
        public ErpAdapter.Interface.IInAppPurchaseController CreateInAppPurchaseController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.InAppPurchaseController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of InAppPurchase Controller
        /// </summary>
        /// <returns></returns>
        public ErpAdapter.Interface.IProductController CreateProductController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.ProductController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of CustomerPortal Controller
        /// </summary>
        /// <param name="storeKey"></param>
        /// <returns></returns>
        public ICustomerPortalController CreateCustomerPortalController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.CustomerPortalController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of CustomerPortal Controller
        /// </summary>
        /// <param name="storeKey"></param>
        /// <returns></returns>
        public ErpAdapter.Interface.IDiscountWithAffiliationController CreateDiscountWithAffiliationController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.DiscountWithAffiliationController(storeKey);
        }
    }

    #endregion

    #region ECom Adapter Factory

    /// <summary>
    /// ECom Adapter implementation factory.
    /// </summary>
    public interface IEComAdapterFactory
    {
        /// <summary>
        /// Create a sales order controller.
        /// </summary>
        /// <returns></returns>
        ECommerceAdapter.Interface.ISaleOrderController CreateSalesOrderController(string storeKey);

        /// <summary>
        /// Create a Customer Controller
        /// </summary>
        /// <returns></returns>
        ECommerceAdapter.Interface.ICustomerController CreateCustomerController(string storeKey);
    }

    /// <summary>
    /// Find the right ECom adapter implementation.
    /// </summary>
    public class EcomAdapterFactory : IEComAdapterFactory
    {
        //private readonly IComponentContext _container;

        /// <summary>
        /// Factory to find right implementation.
        /// </summary>
        /// <param name="container"></param>
        //public EcomAdapterFactory(IComponentContext container)
        //{
        //    _container = container;
        //}
        
        /// <summary>
        /// Resolve the right implementation of sales order controller.
        /// </summary>
        /// <returns></returns>
        public ECommerceAdapter.Interface.ISaleOrderController CreateSalesOrderController(string storeKey)
        {
            return new CommerceLink.MagentoAdapter.Controllers.SaleOrderController(storeKey);
        }

        /// <summary>
        /// Resolve the right implementation of customer controller
        /// </summary>
        /// <returns></returns>
        public ECommerceAdapter.Interface.ICustomerController CreateCustomerController(string storeKey)
        {
            return new CommerceLink.MagentoAdapter.Controllers.CustomerController(storeKey);
        }
    }

    #endregion
}