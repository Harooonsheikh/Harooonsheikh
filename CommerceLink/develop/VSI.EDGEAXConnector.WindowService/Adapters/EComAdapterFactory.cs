using System;
using System.Configuration;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.Enums;

namespace VSI.EDGEAXConnector.WindowService
{
    public interface IEComAdapterFactory
    {
        ICategoryController CreateCategoryController(string storeKey);
        IProductController CreateProductController(string storeKey);
        ISaleOrderController CreateSalesOrderController(string storeKey);
        ISalesOrderStatusController CreateSalesOrderStatusController(string storeKey);
        IInventoryController CreateInventoryController(string storeKey);
        IPriceController CreatePriceController(string storeKey);
        ICustomerController CreateCustomerController(string storeKey);
        IStoreController CreateStoreController(string storeKey);
        IDiscountController CreateDiscountController(string storeKey);
        IDiscountWithAffiliationController CreateDiscountWithAffiliationController(string storeKey);
        IAddressController CreateAddressController(string storeKey);
        IDeletedAddressController CreateDeletedAddressController(string storeKey);
        IChannelConfigurationController CreateChannelConfigurationController(string storeKey);
        IOfferTypeGroupController CreateIOfferTypeGroupController(string storeKey);
        IQuotationReasonGroupController CreateIQuotationReasonController(string storeKey);
        IQuantityDiscountController CreateIQuantityDiscountController(string storeKey);
        IQuantityDiscountWithAffiliationController CreateIQuantityDiscountWithAffiliationController(string storeKey);
        IDataDeleteController CreateDataDeleteController(string storeKey);
    }

    public class EComAdapterFactory : IEComAdapterFactory
    {
        //private readonly IComponentContext _container;

        public EComAdapterFactory()
        {
            
        }

        public ICategoryController CreateCategoryController(string storeKey)
        {
            return new CommerceLink.MagentoAdapter.Controllers.CategoryController(storeKey);
        }

        public IProductController CreateProductController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.ProductController(storeKey);
        }

        public IInventoryController CreateInventoryController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.InventoryController(storeKey);
        }

        public IPriceController CreatePriceController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.PriceController(storeKey);
        }

        public ISaleOrderController CreateSalesOrderController(string storeKey)
        {
            if (ConfigurationManager.AppSettings["EcomAdapter"].Equals("VSI.CommerceLink.ThirdPartyAdapter"))
            {
                return new VSI.CommerceLink.ThirdPartyAdapter.Controllers.SaleOrderController(storeKey);
            }
            return new VSI.CommerceLink.MagentoAdapter.Controllers.SaleOrderController(storeKey);
        }

        public IStoreController CreateStoreController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.StoreController(storeKey);
        }

        public ICustomerController CreateCustomerController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.CustomerController(storeKey);
        }

        public IDiscountController CreateDiscountController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.DiscountController(storeKey);
        }

        public IDiscountWithAffiliationController CreateDiscountWithAffiliationController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.DiscountWithAffiliationController(storeKey);
        }

        public IAddressController CreateAddressController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.AddressController(storeKey);
        }
        public ISalesOrderStatusController CreateSalesOrderStatusController(string storeKey)
        {
            if (ConfigurationManager.AppSettings["EcomAdapter"].Equals("VSI.CommerceLink.ThirdPartyAdapter"))
            {
                return new VSI.CommerceLink.ThirdPartyAdapter.Controllers.SalesOrderStatusController(storeKey);
            }
            return new VSI.CommerceLink.MagentoAdapter.Controllers.SalesOrderStatusController(storeKey);
        }

        public IDeletedAddressController CreateDeletedAddressController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.DeletedAddressController(storeKey);
        }

        /// <summary>
        /// CreateChannelConfigurationController resolves the IChannelConfigurationController object.
        /// </summary>
        /// <returns></returns>
        public IChannelConfigurationController CreateChannelConfigurationController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.ChannelConfigurationController(storeKey);

        }
        public IOfferTypeGroupController CreateIOfferTypeGroupController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.OfferTypeGroupsController(storeKey);
        }

        public IQuotationReasonGroupController CreateIQuotationReasonController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.QuotationReasonGroupsController(storeKey);
        }

        public IQuantityDiscountController CreateIQuantityDiscountController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.QuantityDiscountController(storeKey);
        }

        public IQuantityDiscountWithAffiliationController CreateIQuantityDiscountWithAffiliationController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.QuantityDiscountWithAffiliationController(storeKey);
        }

        public IDataDeleteController CreateDataDeleteController(string storeKey)
        {
            return new VSI.CommerceLink.MagentoAdapter.Controllers.DataDeleteController(storeKey);
        }
    }
}