using System;
using VSI.EDGEAXConnector.Configuration;

namespace VSI.EDGEAXConnector.DemandwareAdapter.Controllers
{
    /// <summary>
    /// BaseController class is base for all Demandware Controllers.
    /// </summary>
    public class BaseController : IDisposable
    {
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
        public BaseController(bool initializeService)
        {
            this.configurationHelper = ConfigurationHelper.GetInstance;
            if (initializeService)
            {
                // put initialization logic here
            }
        }

        #endregion        

        #region Constructor
        protected  string GetThreeDigitCountryCode(string twoDigitCountryCode)
        {
            switch (twoDigitCountryCode.ToUpper())
            {
                default:
                case "US":
                    return "USA";
                case "CA":
                    return "CAN";
                case "AF":
                    return "AFG";
                case "GB":
                    return "GBR";
            }
        }

        protected string GetTwoDigitCountryCode(string threeDigitCountryCode)
        {
            switch (threeDigitCountryCode.ToUpper())
            {
                default:
                case "USA":
                    return "US";
                case "CAN":
                    return "CA";
                case "AFG":
                    return "AF";
                case "GBR":
                    return "GB";
            }
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
                    //if (this.Service != null)
                    //{
                    //    this.Service.endSession(this.SessionId);
                    //    this.Service.Dispose();
                    //}
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
