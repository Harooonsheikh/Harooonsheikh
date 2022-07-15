using System;
using VSI.EDGEAXConnector.Configuration;
using VSI.CommerceLink.MagentoAPI.MageAPI;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;
using VSI.EDGEAXConnector.Data.DTO;

namespace VSI.CommerceLink.DemandwareAdapter.Controllers
{
    /// <summary>
    /// BaseController class is base for all Magento Controllers.
    /// </summary>
    public class BaseController : IDisposable
    {

        #region Properties

        /// <summary>
        /// Service object for MagentoService.
        /// </summary>
        protected MagentoService Service { get; set; }

        /// <summary>
        /// SessionId of session for MagentoService.
        /// </summary>
        protected string SessionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public StoreDto currentStore = null;
        #endregion

        #region Data Members

        /// <summary>
        /// Disposed holds if dispose method has been ever called.
        /// </summary>
        private bool Disposed = false;

       internal ConfigurationHelper configurationHelper;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class. 
        /// </summary>
        /// <param name="initializeService"></param>
        public BaseController(bool initializeService, string storeKey)
        {

            
            if (initializeService)
            {
                if (!string.IsNullOrWhiteSpace(configurationHelper.GetSetting(ECOM.Magento_API_Username)) && !string.IsNullOrWhiteSpace(configurationHelper.GetSetting(ECOM.Magento_API_Password)))
                {
                    this.Service = new MagentoService();
                    this.SessionId = this.Service.login(configurationHelper.GetSetting(ECOM.Magento_API_Username), configurationHelper.GetSetting(ECOM.Magento_API_Password));
                 
                }
                else
                {
                    throw new CommerceLinkError("MageAPIUser or MageAPIKey is not defined in configuration");
                }
            }
            else
            {
                this.SessionId = string.Empty;
            }


            //++ initialize store.
            this.configurationHelper = new ConfigurationHelper(storeKey);
            this.currentStore = StoreService.GetStoreByKey(storeKey);
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            //Dispose method is called for first time.
            if (!this.Disposed)
            {
                if (disposing)
                {
                    if (this.Service != null)
                    {
                        this.Service.endSession(this.SessionId);
                        this.Service.Dispose();
                    }
                }
            }
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor releases unmanged resources. 
        /// </summary>
        ~BaseController()
        {
            this.Dispose(false);
        }

        #endregion
    }
}
