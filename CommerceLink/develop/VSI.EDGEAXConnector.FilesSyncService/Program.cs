using System.ServiceProcess;

namespace VSI.EDGEAXConnector.FilesSyncService
{
    public static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {

            //var proManager = new ProductManager();

            //proManager.SetStore(new Data.Store { StoreKey = "E550E995-1D34-4E65-9222-FA4C15712ADA", StoreId = 1, CreatedBy = "uy" });
            //proManager.InitializeParameter();
            //proManager.Sync();

            //If the mode is in debugging
            //create a new service instance
#if DEBUG
            //var service = new FileSyncService.EDGEAXConnectorFileSyncService();
            //service.Start();
            
#else
ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            {
                new FileSyncService.EDGEAXConnectorFileSyncService()
            };
            ServiceBase.Run(ServicesToRun);
#endif

        }
    }
}
