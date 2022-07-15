using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Filters;
using VSI.EDGEAXConnector.Common;
using MapsterMapper;
using VSI.CommerceLink.EcomDataModel;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ErpAdapter.Interface;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Web.Controllers;
using VSI.EDGEAXConnector.Logging.CommerceLinkExceptions;
using VSI.EDGEAXConnector.Data.DTO;

namespace VSI.EDGEAXConnector.Web
{

    /// <summary>
    /// ApiBaseController class defines common custom properties and methods for all controller. 
    /// </summary>
    public abstract class ApiBaseController : ApiController
    {
        /// <summary>
        /// Children should put a valid name.
        /// </summary>
        protected string ControllerName = string.Empty;

        /// <summary>
        ///  Using this property access LoyaltyCardController functionality.
        /// </summary>
        protected ILoyaltyCardController loyaltyCardController;

        /// <summary>
        /// Injected ERP Adapter Factory
        /// </summary>
        protected IErpAdapterFactory erpAdapterFactory;
        /// <summary>
        /// Injected ECom Adapter Factory
        /// </summary>
        protected IEComAdapterFactory ecomAdapterFactory;
        /// <summary>
        /// 
        /// </summary>
        protected string StoreKey = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public StoreDto currentStore = null;

        protected IMapper _mapper;

        /// <summary>
        /// Initilaze properties
        /// </summary>
        internal ConfigurationHelper configurationHelper;

        public ApiBaseController(IErpAdapterFactory _erpAdapterFactory)
        {
            try
            {
                this.erpAdapterFactory = _erpAdapterFactory;
                SetStoreKey();
                this.currentStore = StoreService.GetStoreByKey(StoreKey);
                this.configurationHelper = ConfigurationHelper.GetConfigurationHelperInstanceByStore(StoreKey);
                InitializeMapper();
            }
            catch (Exception ex)
            {
                CustomLogger.LogException("Inside ApiBaseController", currentStore.StoreId, currentStore.CreatedBy);
            }
        }

        /// <summary>
        /// Initilaze properties
        /// </summary>
        public ApiBaseController()
        {
            SetStoreKey();
            this.erpAdapterFactory = new ErpAdapterFactory();
            this.ecomAdapterFactory = new EcomAdapterFactory();
            this.currentStore = StoreService.GetStoreByKey(StoreKey);
            this.configurationHelper = ConfigurationHelper.GetConfigurationHelperInstanceByStore(StoreKey);

            InitializeMapper();
        }
        public ApiBaseController(IErpAdapterFactory _erpAdapterFactory, IEComAdapterFactory _ecomAdapterFactory)
        {
            SetStoreKey();
            this.erpAdapterFactory = _erpAdapterFactory;
            this.ecomAdapterFactory = _ecomAdapterFactory;
            this.currentStore = StoreService.GetStoreByKey(StoreKey);
            this.configurationHelper = ConfigurationHelper.GetConfigurationHelperInstanceByStore(StoreKey);
            InitializeMapper();
        }
        private void SetStoreKey()
        {
            IEnumerable<string> apiKey = System.Web.HttpContext.Current.Request.Headers.GetValues("x-api-key");
            this.StoreKey = apiKey.FirstOrDefault();
        }

        public bool ValidateCustomerLicenseIDLength(string licenseID)
        {
            bool result = true;

            if (!string.IsNullOrWhiteSpace(licenseID))
            {
                result = licenseID.Length <= 36 ? true : false;
            }

            return result;
        }

        /// <summary>
        /// For Request Management
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GetRequestGUID(HttpRequestMessage request)
        {
            if (request != null)
            {
                if (request.Properties.ContainsKey("RequestId"))
                {
                    return request.Properties["RequestId"].ToString();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Extract EcomTransactionId from request header
        /// </summary>
        /// <returns></returns>
        public Guid GetTransactionIdFromHeader()
        {
            CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10000, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod(), DateTime.UtcNow);

            try
            {
                IEnumerable<string> ecomTransactionIds = null;
                Guid ecomTransactionId = Guid.Empty;

                Request.Headers.TryGetValues("TransactionId", out ecomTransactionIds);
                if (ecomTransactionIds != null && ecomTransactionIds.Count() > 0)
                {
                    ecomTransactionId = Guid.Parse(ecomTransactionIds.First());
                }
                else
                {
                    throw new CommerceLinkError(CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40010, currentStore, "TransactionId"));
                }

                CommerceLinkLogger.LogTraceCSV(CommerceLinkLoggerMessages.VSICL10004, currentStore, GetRequestGUID(Request), MethodBase.GetCurrentMethod(), DateTime.UtcNow);

                return ecomTransactionId;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void InitializeMapper()
        {
            _mapper = AutoMapBootstrapper.MapperInstance;
        }

        /// <summary>
        /// Validation errors
        /// </summary>
        /// <returns></returns>
        public string GetModelErrors()
        {
            return String.Join(". ", ModelState.Values.SelectMany(a => a.Errors.Select(x => x.ErrorMessage)));
        }
    }
}
