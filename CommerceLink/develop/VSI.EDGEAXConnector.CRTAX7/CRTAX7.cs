using Autofac;
using VSI.EDGEAXConnector.CRT.Interface;
using VSI.EDGEAXConnector.CRTAX7.Controllers;

namespace VSI.EDGEAXConnector.CRTAX7
{
    public class CRTAX7 : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ad => new CRTAX7()).As<CRTAX7>();
            builder.Register(ad => new SalesOrderController()).As<ISalesOrderController>();
            builder.Register(ad => new StoreController()).As<IStoreController>();
            builder.Register(ad => new PriceController()).As<IPriceController>();
            builder.Register(ad => new DiscountController()).As<IDiscountController>();
            builder.Register(ad => new ProductController()).As<IProductController>();
            builder.Register(ad => new InventoryController()).As<IInventoryController>();
            builder.Register(ad => new CustomerController()).As<ICustomerController>();
            builder.Register(ad => new ShippingController()).As<IShippingController>();

            //initializing AutoMapperConfiguration
            AutoMapper.Mapper.AddProfile(new ErpToRSMappingConfiguration());
        }
    }
}
