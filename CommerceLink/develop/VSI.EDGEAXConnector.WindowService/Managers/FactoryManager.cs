using System.Configuration;
using System.Reflection;
using VSI.EDGEAXConnector.WindowService.Managers;

namespace VSI.EDGEAXConnector.WindowService
{
    public class FactoryManager
    {
        //public IContainer Configure()
        //{
        //    var container = BuildContainer();
        //    var erpContainerBuilder = new ContainerBuilder();

        //    erpContainerBuilder.RegisterInstance(container);

        //    return container;
        //}

        //private static IContainer BuildContainer()
        //{
        //    var builder = new ContainerBuilder();
        //    builder.RegisterType<EDGEAXConnectorWindowsService>();

        //    var erpAssembly = Assembly.Load(ConfigurationManager.AppSettings["ErpAdapter"]);
        //    var eComAssembly = Assembly.Load(ConfigurationManager.AppSettings["EcomAdapter"]);

        //    builder.RegisterAssemblyModules(erpAssembly);
        //    builder.RegisterAssemblyModules(eComAssembly);

        //    builder.RegisterType<ErpAdapterFactory>().As<IErpAdapterFactory>();
        //    builder.RegisterType<EComAdapterFactory>().As<IEComAdapterFactory>();

        //    builder.RegisterType<CustomerManager>();
        //    builder.RegisterType<ProductManager>();
        //    builder.RegisterType<InventoryManager>();
        //    builder.RegisterType<SalesOrderManager>();
        //    builder.RegisterType<InventoryManager>();
        //    builder.RegisterType<PriceManager>();
        //    builder.RegisterType<StoreManager>();
        //    builder.RegisterType<DiscountManager>();
        //    builder.RegisterType<DiscountWithAffiliationManager>();
        //    builder.RegisterType<PaymentManager>();
        //    builder.RegisterType<ChannelPublishingManager>();
        //    builder.RegisterType<ChannelConfigurationManager>();
        //    builder.RegisterType<OfferTypeGroupManager>();
        //    builder.RegisterType<QuotationReasonGroupManager>();
        //    builder.RegisterType<CartManager>();
        //    builder.RegisterType<QuantityDiscountManager>();
        //    builder.RegisterType<QuantityDiscountWithAffiliationManager>();
        //    builder.RegisterType<DataDeleteManager>();

        //    return builder.Build();

        //}
    }
}
