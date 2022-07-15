using VSI.EDGEAXConnector.ECommerceAdapter.Interface;
using VSI.CommerceLink.DemandwareAdapter.Controllers;
using VSI.EDGEAXConnector.Enums;

namespace VSI.CommerceLink.DemandwareAdapter
{

    /// <summary>
    /// MagentoAdapter class binds controllers to interfaces.
    /// </summary>
    public class DemandwareAdapter 
    {

        #region Protected Methods

        /// <summary>
        /// This function binds controllers to interfaces.
        /// </summary>
        /// <param name="builder"></param>
        //protected override void Load(ContainerBuilder builder)
        //{
        //    builder.Register((c, p) => new CategoryController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<ICategoryController>();
        //    builder.Register((c, p) => new ProductController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<IProductController>();
        //    builder.Register((c, p) => new SaleOrderController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<ISaleOrderController>();
        //    builder.Register((c, p) => new SalesOrderStatusController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<ISalesOrderStatusController>();
        //    builder.Register((c, p) => new InventoryController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<IInventoryController>();
        //    builder.Register((c, p) => new PriceController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<IPriceController>();
        //    builder.Register((c, p) => new CustomerController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<ICustomerController>();
        //    builder.Register((c, p) => new StoreController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<IStoreController>();
        //    builder.Register((c, p) => new DiscountController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<IDiscountController>();
        //    builder.Register((c, p) => new AddressController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<IAddressController>();
        //    builder.Register((c, p) => new DeletedAddressController(p.Named<string>(CURRENTSTORE.storeKey.ToString()))).As<IDeletedAddressController>();

        //    AutoMapper.Mapper.AddProfile(new EcomMappingConfiguration());
        //}

        #endregion
    }
}