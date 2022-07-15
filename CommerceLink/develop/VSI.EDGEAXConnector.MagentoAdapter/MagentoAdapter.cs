using Autofac;
using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.EDGEAXConnector.MagentoAdapter.Controllers;

namespace VSI.EDGEAXConnector.MagentoAdapter
{

    /// <summary>
    /// MagentoAdapter class binds controllers to interfaces.
    /// </summary>
    public class MagentoAdapter : Module
    {

        #region Protected Methods

        /// <summary>
        /// This function binds controllers to interfaces.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ad => new CategoryController()).As<ICategoryController>();
            builder.Register(ad => new ProductController()).As<IProductController>();
            builder.Register(ad => new SaleOrderController()).As<ISaleOrderController>();
            builder.Register(ad => new SalesOrderStatusController()).As<ISalesOrderStatusController>();
            builder.Register(ad => new InventoryController()).As<IInventoryController>();
            builder.Register(ad => new PriceController()).As<IPriceController>();
            builder.Register(ad => new CustomerController()).As<ICustomerController>();
            builder.Register(ad => new StoreController()).As<IStoreController>();
            builder.Register(ad => new DiscountController()).As<IDiscountController>();
            builder.Register(ad => new AddressController()).As<IAddressController>();
            builder.Register(ad => new DeletedAddressController()).As<IDeletedAddressController>();
            builder.Register(ad => new ChannelConfigurationController()).As<IChannelConfigurationController>();
            builder.Register(ad => new OfferTypeGroupsController()).As<IOfferTypeGroupController>();
            builder.Register(ad => new QuotationReasonGroupsController()).As<IQuotationReasonGroupController>();

            AutoMapper.Mapper.AddProfile(new EcomMappingConfiguration());
        }

        #endregion
    }
}