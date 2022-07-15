using System;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;

namespace VSI.EDGEAXConnector.WindowService
{
    public interface IErpAdapterFactory
    {

        ICategoryController CreateCategoryController(string storeKey);
        IProductController CreateProductController(string storeKey);
        ISaleOrderController CreateSalesOrderController(string storeKey);
        IInventoryController CreateInventoryController(string storeKey);
        IPriceController CreatePriceController(string storeKey);
        IStoreController CreateStoreController(string storeKey);
        ICustomerController CreateCustomerController(string storeKey);
        IDiscountController CreateDiscountController(string storeKey);
        IDiscountWithAffiliationController CreateDiscountWithAffiliationController(string storeKey);
        IPaymentController CreatePaymentController(string storeKey);
        IChannelPublishingController CreateChannelPublishingController(string storeKey);
        IChannelConfigurationController CreateChannelConfigurationController(string storeKey);
        IOfferTypeGroupController CreateOfferTypeGroupController(string storeKey);
        IQuotationController CreateQuotationController(string storeKey);
		ICartController CreateCartController(string storeKey);
        IContactPersonController CreateContactPersonController(string storeKey);
        IQuantityDiscountController CreateQuantityDiscountController(string storeKey);
        IQuantityDiscountWithAffiliationController CreateQuantityDiscountWithAffiliationController(string storeKey);

    }

    public class ErpAdapterFactory : IErpAdapterFactory
    {
        
        public ICategoryController CreateCategoryController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.CategoryController(storeKey);
        }

        public IProductController CreateProductController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.ProductController(storeKey);
        }

        public ISaleOrderController CreateSalesOrderController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.SaleOrderController(storeKey);
        }

        public IInventoryController CreateInventoryController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.InventoryController(storeKey);
        }

        public IPriceController CreatePriceController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.PriceController(storeKey);
        }

        public IStoreController CreateStoreController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.StoreLocationController(storeKey);
        }

        public ICustomerController CreateCustomerController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.CustomerController(storeKey);
        }

        public IDiscountController CreateDiscountController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.DiscountController(storeKey);
        }

        public IDiscountWithAffiliationController CreateDiscountWithAffiliationController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.DiscountWithAffiliationController(storeKey);
        }

        public IPaymentController CreatePaymentController(string storeKey) 
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.PaymentController(storeKey);
        }

        public IChannelPublishingController CreateChannelPublishingController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.ChannelPublishingController(storeKey);
        }
        /// <summary>
        /// CreateChannelConfigurationController resolves the IChannelConfigurationController object.
        /// </summary>
        /// <returns></returns>
        public IChannelConfigurationController CreateChannelConfigurationController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.ChannelConfigurationController(storeKey);
        }
        public IOfferTypeGroupController CreateOfferTypeGroupController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.OfferTypeGroupsController(storeKey);
        }

        public IQuotationController CreateQuotationController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.QuotationController(storeKey);
        }

        /// <summary>
        /// CreateCartController resolves the ICartController object.
        /// </summary>
        /// <returns></returns>
        public ICartController CreateCartController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.CartController(storeKey);
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
        /// Resolve the right implementation of Quantity Discount Controller
        /// </summary>
        /// <returns></returns>
        public IQuantityDiscountController CreateQuantityDiscountController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.QuantityDiscountController(storeKey);
        }

        public IQuantityDiscountWithAffiliationController CreateQuantityDiscountWithAffiliationController(string storeKey)
        {
            return new VSI.EDGEAXConnector.AXAdapter.Controllers.QuantityDiscountWithAffiliationController(storeKey);
        }


    }
}