using System.Configuration;
using System.Reflection;
using System.ServiceProcess;
using VSI.EDGEAXConnector.WindowService.Managers;

namespace VSI.EDGEAXConnector.WindowService
{
    public static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            Assembly.Load(ConfigurationManager.AppSettings["ErpAdapter"]);
            Assembly.Load(ConfigurationManager.AppSettings["EcomAdapter"]);

            // Code for release mode
#if (!DEBUG)
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new EDGEAXConnectorWindowsService()
            };
            ServiceBase.Run(ServicesToRun);
#else // Code for Debug mode. Modify the code below for debugging
            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new EDGEAXConnectorWindowsService()
            //};
            //ServiceBase.Run(ServicesToRun);

            /* comment - developers - uncomment your job to debug before service starts. DO NOT check in your changed file */

            //var inventory = new InventoryManager();
            //inventory.SetStore(new Data.DTO.StoreDto { StoreKey = "E550E995-1D34-4E65-9INT-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //inventory.InitializeParameter();
            //inventory.Sync();

            //var configManager = new ChannelConfigurationManager();
            //configManager.SetStore(new Data.DTO.StoreDto { StoreKey = "E550E995-1D34-4E65-9INT-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //configManager.InitializeParameter();
            //configManager.Sync();

            //var distManager = new DiscountManager();
            //distManager.SetStore(new Data.DTO.StoreDto { StoreKey = "E550E995-1D34-4E65-9INT-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //distManager.InitializeParameter();
            //distManager.Sync();

            //var discountWithAffilicationManager = new DiscountWithAffiliationManager();
            //discountWithAffilicationManager.SetStore(new Data.DTO.StoreDto { StoreKey = "E550E995-1D34-4E65-9INT-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //discountWithAffilicationManager.InitializeParameter();
            //discountWithAffilicationManager.Sync();

            //var quantityDiscountWithAffilicationManager = new QuantityDiscountWithAffiliationManager();
            //quantityDiscountWithAffilicationManager.SetStore(new Data.DTO.StoreDto { StoreKey = "E550E995-1D34-4E65-9INT-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //quantityDiscountWithAffilicationManager.InitializeParameter();
            //quantityDiscountWithAffilicationManager.Sync();

            //var salesOrderManager = new SalesOrderManager();
            //salesOrderManager.SetStore(new Data.DTO.StoreDto { StoreKey = "E550E995-1D34-4E65-9INT-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //salesOrderManager.InitializeParameter();
            //salesOrderManager.Sync();

            //salesOrderManager.SearchSalesOrder("S003761336");
            //var salesOrderManager = new SalesOrderManager();
            //salesOrderManager.SyncOrderStatus();

            //var productManager = new ProductManager();
            //productManager.SetStore(new Data.DTO.StoreDto { StoreKey = "E550E995-1D34-4E65-9INT-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //productManager.InitializeParameter();
            //productManager.Sync();

            //var priceManager = new PriceManager();
            //priceManager.SetStore(new Data.DTO.StoreDto { StoreKey = "E550E995-1D34-4E65-9INT-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //priceManager.InitializeParameter();
            //priceManager.Sync();

            //var storeManager = new StoreService();
            //storeManager.SetStore(new Data.DTO.StoreDto { StoreKey = "E550E995-1D34-4E65-9INT-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //storeManager.InitializeParameter();
            //storeManager.Sync();

            //var cartManager = new CartManager();
            //cartManager.SetStore(new Data.DTO.StoreDto { StoreKey = "E550E995-1D34-4E65-9INT-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //cartManager.InitializeParameter();
            //cartManager.Sync();

            //var quantityDiscountManager = new QuantityDiscountManager();
            //quantityDiscountManager.SetStore(new Data.DTO.StoreDto { StoreKey = "E550E995-1D34-4E65-9INT-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //quantityDiscountManager.InitializeParameter();
            //quantityDiscountManager.Sync();

            //var dataDeleteManager = new DataDeleteManager();
            //dataDeleteManager.SetStore(new Data.DTO.StoreDto { StoreKey = "E550E995-1D34-4E65-9INT-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //dataDeleteManager.InitializeParameter();
            //dataDeleteManager.Sync();

            //var thirdPartyManager = new SalesOrderThirdPartyDownloadManager();
            //thirdPartyManager.SetStore(new Data.DTO.StoreDto { StoreKey = "53ed81cd-bc03-4fdcINTd-0400af53a6b1", StoreId = 150, CreatedBy = "uy" });
            //thirdPartyManager.InitializeParameter();
            //thirdPartyManager.Sync();

            //var salesOrderManager = new SalesOrderManager();
            //salesOrderManager.SetStore(new Data.DTO.StoreDto { StoreKey = "53ed81cd-bc03-4fdcINTd-0400af53a6b1", StoreId = 150, CreatedBy = "uy" });
            //salesOrderManager.InitializeParameter();
            //salesOrderManager.Sync();

            //var salesOrderStatusManager = new SalesOrderStatusManager();
            //salesOrderStatusManager.SetStore(new Data.DTO.StoreDto { StoreKey = "53ed81cd-bc03-4fdcINTd-0400af53a6b1", StoreId = 150, CreatedBy = "uy" });
            //salesOrderStatusManager.InitializeParameter();
            //salesOrderStatusManager.Sync();

#endif

        }
    }
}
