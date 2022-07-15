using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <summary>
    /// PriceDiscountController defines properties and methods for API controller for Product prices and discounts.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class PriceDiscountController : ApiBaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_erpAdapterFactory"></param>
        /// <param name="_eComAdapterFactory"></param>
        public PriceDiscountController()
        {
            ControllerName = "PriceDiscountController";
        }

        #region API Methods

        /// <summary>
        /// GetIndependentProductPriceDiscount
        /// </summary>
        /// <param name="getIndependentProductPriceDiscountRequest"></param>
        /// <returns>GetIndependentProductPriceDiscountResponse</returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("PriceDiscount/GetIndependentProductPriceDiscount")]
        public GetIndependentProductPriceDiscountResponse GetIndependentProductPriceDiscount([FromBody] GetIndependentProductPriceDiscountRequest getIndependentProductPriceDiscountRequest)
        {

            GetIndependentProductPriceDiscountResponse getIndependentProductPriceDiscountResponse;

            try
            {
                if (getIndependentProductPriceDiscountRequest == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    getIndependentProductPriceDiscountResponse = new GetIndependentProductPriceDiscountResponse(false, message, null);
                    return getIndependentProductPriceDiscountResponse;
                }
                else
                {
                    if (getIndependentProductPriceDiscountRequest.productIds == null || getIndependentProductPriceDiscountRequest.productIds.Count == 0)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "productIds");
                        getIndependentProductPriceDiscountResponse = new GetIndependentProductPriceDiscountResponse(false, message, null);
                        return getIndependentProductPriceDiscountResponse;
                    }
                    else if (string.IsNullOrEmpty(getIndependentProductPriceDiscountRequest.IsEcomProductIds.ToString()) || string.IsNullOrWhiteSpace(getIndependentProductPriceDiscountRequest.IsEcomProductIds.ToString()))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "useEcomProductIds");
                        getIndependentProductPriceDiscountResponse = new GetIndependentProductPriceDiscountResponse(false, message, null);
                        return getIndependentProductPriceDiscountResponse;
                    }
                }

                // Extract the data from request for processing
                List<string> productIds = getIndependentProductPriceDiscountRequest.productIds;
                bool isEcomProductIds = getIndependentProductPriceDiscountRequest.IsEcomProductIds;
                string customerAccountNumber = getIndependentProductPriceDiscountRequest.CustomerAccountNumber;
                List<ErpAffiliationLoyaltyTier> affiliations = getIndependentProductPriceDiscountRequest.AffiliationLines;

                string customerAccountNumberToLog = string.Empty;
                string affiliationToLog = string.Empty;

                if (!string.IsNullOrEmpty(customerAccountNumber))
                {
                    customerAccountNumberToLog = customerAccountNumber;
                }
                if (affiliations != null)
                {
                    affiliationToLog = JsonConvert.SerializeObject(affiliations);
                }

                // Log the received request parameters
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001
                    , currentStore , MethodBase.GetCurrentMethod().Name
                    , "productIds: " + JsonConvert.SerializeObject(productIds)
                        + " and IsEcomProductIds: " + isEcomProductIds.ToString()
                        + " and customerAccountNumber: " + customerAccountNumberToLog
                        + " and affiliations: " + affiliationToLog
                        );
                                
                // Check if ECOM Product IDs has been provided if yes, then get ERP IDs from DB
                List<long> lstProductIds = new List<long>();
                if(isEcomProductIds)
                {
                    //product ids swapping logic here before send to AX
                    IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                    foreach (var proId in productIds)
                    {
                        // Extract ErpKey from database
                        var integrationKey = integrationManager.GetErpKey(Enums.Entities.Product, proId);
                        var erpKey = (integrationKey == null) ? proId : integrationKey.ErpKey; // Check if ErpKey is null
                        long Id = 0;
                        long.TryParse(erpKey, out Id);
                        lstProductIds.Add(Id);
                    }
                }
                else
                {
                    foreach(var proId in productIds)
                    {
                        long Id = 0;
                        long.TryParse(proId, out Id);
                        lstProductIds.Add(Id);
                    }
                }

                // Send the call to AX
                var erpDiscountController = erpAdapterFactory.CreateDiscountController(currentStore.StoreKey);
                var erpResponse = erpDiscountController.GetIndependentProductPriceDiscount(lstProductIds, customerAccountNumber, affiliations);

                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10003, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));

                if (erpResponse.Success)
                {
                    if (erpResponse.ProductPrices.Count > 0)
                    {
                        if (isEcomProductIds)
                        {
                            //product ids swapping logic here after get from AX
                            IntegrationManager integrationManager = new IntegrationManager(currentStore.StoreKey);
                            foreach (var pro in erpResponse.ProductPrices)
                            {
                                // Extract ErpKey from database
                                var integrationKey = integrationManager.GetComKey(Enums.Entities.Product, pro.ProductId.ToString());
                                var comKey = (integrationKey == null) ? pro.ProductId.ToString() : integrationKey.ComKey; // Check if ComKey is null
                                pro.EcomProductId = comKey;
                            }
                        }
                        getIndependentProductPriceDiscountResponse = new GetIndependentProductPriceDiscountResponse(erpResponse.Success, erpResponse.Message, erpResponse.ProductPrices.ToList());
                        return getIndependentProductPriceDiscountResponse;
                      }
                    else
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40004, currentStore, MethodBase.GetCurrentMethod().Name);
                        getIndependentProductPriceDiscountResponse = new GetIndependentProductPriceDiscountResponse(erpResponse.Success, message, null);
                        return getIndependentProductPriceDiscountResponse;
                    }
                }
                else
                {
                    CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40004, currentStore, MethodBase.GetCurrentMethod().Name);
                    CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, erpResponse.Message);
                    getIndependentProductPriceDiscountResponse = new GetIndependentProductPriceDiscountResponse(erpResponse.Success, erpResponse.Message, erpResponse.ProductPrices);
                    return getIndependentProductPriceDiscountResponse;
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                getIndependentProductPriceDiscountResponse = new GetIndependentProductPriceDiscountResponse(false, message, null);
                return getIndependentProductPriceDiscountResponse;
            }
        }
        
        #endregion

        #region "Private Methods"
                

        #endregion

        #region Request, Response classes

        /// <summary>
        /// Request
        /// </summary>
        public class GetIndependentProductPriceDiscountRequest
        {
            /// <summary>
            /// Constructor
            /// </summary>
            public GetIndependentProductPriceDiscountRequest()
            {
                this.productIds = new List<string>();
            }

            /// <summary>
            /// productIds
            /// </summary>
            public List<string> productIds { get; set; }

            /// <summary>
            /// useEcomProductIds
            /// </summary>
            public bool IsEcomProductIds { get; set; }

            /// <summary>
            /// CustomerAccountNumber
            /// </summary>
            [Required]
            public string CustomerAccountNumber { get; set; }

            /// <summary>
            /// AffiliationLines
            /// </summary>
            public List<ErpAffiliationLoyaltyTier> AffiliationLines { get; set; }
        }

        /// <summary>
        /// Response
        /// </summary>
        public class GetIndependentProductPriceDiscountResponse
        {
            /// <summary>
         ///Message
         /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// ProductPrices
            /// </summary>
            public List<ErpProductPrice> ProductPrices { get; set; }

            /// <summary>
            /// Success
            /// </summary>
            public bool Success { get; set; }

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="success"></param>
            /// <param name="message"></param>
            /// <param name="productPrices"></param>
            public GetIndependentProductPriceDiscountResponse(bool success, string message, List<ErpProductPrice> productPrices)
            {
                this.Success = success;
                this.Message = message;
                this.ProductPrices = productPrices;
            }
        }

        #endregion
    }

}