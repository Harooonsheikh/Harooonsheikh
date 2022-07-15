using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web
{
    /// <summary>
    /// ShippingController defines properties and methods for API controller for shipping.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class ShippingController : ApiBaseController
    {

        /// <summary>
        /// Shipping Controller Constructor
        /// </summary>
        /// <param name="_erpAdapterFactory"></param>
        public ShippingController()
        {
            ControllerName = "ShippingController";
        }

        #region API Methods

        // POST api/shipping/estimate
        /// <summary>
        /// Estimate shipping charges for the product according to its quantity.
        /// </summary>
        /// <param name="estimateRequest">mandatory: string value</param>
        /// <param name="detail">optional: boolean value</param>
        /// <returns>estimateResponse</returns>
        [HttpPost]
        [Route("Shipping/EstimateShippingCharges")]
        // shippingMethod will be upper case hardcoded value of 3 literals, postal code can be zip code or international postal code, 
        // country  code must be 'US' for domestic shipping.  For international shipping, specify countryCode, and write the remaining address in address1 and address 2
        // order > ProductID and Quantity
        public EstimateSmallResponse EstimateShippingCharges([FromBody] EstimateRequest estimateRequest, [FromUri] bool detail = false)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            String shippingMethod;
            String postalCode;
            String countryCode;
            List<EstimateRequestOrderLine> order;
            String address1;
            String address2;
            List<EstimateResponseOrderLine> responseOrderDetails = new List<EstimateResponseOrderLine>();

            try
            {
                if (estimateRequest == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    //throw new System.ArgumentException("Invalid parameter value for 'estimateRequest'.", "estimateRequest", new HttpResponseException(HttpStatusCode.BadRequest));
                    return new EstimateSmallResponse("false", message, 0);
                }
                if (string.IsNullOrEmpty(estimateRequest.shippingMethod) || string.IsNullOrWhiteSpace(estimateRequest.shippingMethod))  // || shippingMethod.Any(char.IsLower))
                {
                    //throw new System.ArgumentException("Invalid parameter value for 'shippingMethod'.", "shippingMethod", new HttpResponseException(HttpStatusCode.BadRequest));
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "shippingMethod");
                    return new EstimateSmallResponse("false", message, 0);
                }
                if (string.IsNullOrEmpty(estimateRequest.postalCode) || string.IsNullOrWhiteSpace(estimateRequest.postalCode))
                {
                    //throw new System.ArgumentException("Empty value for 'zipCode'", "zipCode", new HttpResponseException(HttpStatusCode.BadRequest));
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "postalCode");
                    return new EstimateSmallResponse("false", message, 0);
                }
                if (estimateRequest.order == null)
                {
                    //throw new System.ArgumentException("Empty value for 'order'", "order", new HttpResponseException(HttpStatusCode.BadRequest));
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "order");
                    return new EstimateSmallResponse("false", message, 0);
                }
                if (!string.IsNullOrEmpty(estimateRequest.countryCode) && !string.IsNullOrWhiteSpace(estimateRequest.countryCode))     // postalCode validation for US
                {
                    if (estimateRequest.countryCode.Length != 2 || !estimateRequest.countryCode.All(c => Char.IsLetter(c)))
                    {
                        //throw new System.ArgumentException("Invalid country code", "countryCode", new HttpResponseException(HttpStatusCode.BadRequest));
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "Invalid Country Code");
                        return new EstimateSmallResponse("false", message, 0);
                    }
                    if (estimateRequest.countryCode == "US" || estimateRequest.countryCode == "") // domestic
                    {
                        string _usZipRegEx = @"^\d{5}(?:[-\s]\d{4})?$";     // Regular expression for US zip code
                        if (!Regex.Match(estimateRequest.postalCode, _usZipRegEx).Success)
                        {
                            //throw new System.ArgumentException("Invalid US zip code value in postalCode parameter", "zipCode", new HttpResponseException(HttpStatusCode.BadRequest));
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "Invalid Zip Code Value");
                            return new EstimateSmallResponse("false", message, 0);
                        }
                    }
                    else // other than US
                    {
                        if (string.IsNullOrEmpty(estimateRequest.address1) || string.IsNullOrWhiteSpace(estimateRequest.address1))
                        {
                            //throw new System.ArgumentException("Address1 cannot be empty.", "address1", new HttpResponseException(HttpStatusCode.BadRequest));
                            string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Address1");
                            return new EstimateSmallResponse("false", message, 0);
                        }
                    }
                }

                foreach (EstimateRequestOrderLine orderLine in estimateRequest.order)
                {
                    if (string.IsNullOrEmpty(orderLine.product) || string.IsNullOrWhiteSpace(orderLine.product)) // orderLine.quantity <= 0)
                    {
                        //throw new System.ArgumentException("Provided order information is not valid", "order", new HttpResponseException(HttpStatusCode.BadRequest));
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "Invalid Product");
                        return new EstimateSmallResponse("false", message, 0);
                    }
                    else if (orderLine.quantity <= 0)
                    {
                        //throw new System.ArgumentException("Provided order information is not valid", "order", new HttpResponseException(HttpStatusCode.BadRequest));
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "Invalid Product Quantity");
                        return new EstimateSmallResponse("false", message, 0);
                    }
                }

                shippingMethod = estimateRequest.shippingMethod;
                postalCode = estimateRequest.postalCode;
                countryCode = estimateRequest.countryCode;
                order = estimateRequest.order;
                address1 = estimateRequest.address1;
                address2 = estimateRequest.address2;

                if (detail == true)
                {
                    foreach (EstimateRequestOrderLine estimateRequestOrderLine in estimateRequest.order)
                    {
                        EstimateResponseOrderLine estimateResponseOrderLine = new EstimateResponseOrderLine(
                            estimateRequestOrderLine.product,
                            estimateRequestOrderLine.quantity,
                            false);
                        responseOrderDetails.Add(estimateResponseOrderLine);
                    }
                }

                countryCode = LocalizationHelper.CountryThreeLetterISOCode(countryCode);

                // AX transaction
                try
                {
                    var erpShippingController = erpAdapterFactory.CreateShippingController(currentStore.StoreKey);

                    EstimateSmallResponse estimateResponse;

                    estimateResponse = AssembleResponse(
                        erpShippingController.GetShippingCharges(shippingMethod, BreakOrder(order), postalCode, countryCode, address1, address2),
                        estimateRequest
                    );

                    return estimateResponse;
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                    return new EstimateSmallResponse("false", message, 0);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new EstimateSmallResponse("false", message, 0);
            }
            catch (Exception ex)
            {
                //CustomLogger.LogException(exp, ControllerName);
                //throw exp;
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new EstimateSmallResponse("false", message, 0);
            }
        }

        #endregion

        #region ERP call
        private IEnumerable<ErpCommerceProperty> BreakOrder(List<EstimateRequestOrderLine> order)
        {
            List<ERPDataModels.ErpCommerceProperty> shippingChargesInput = new List<ERPDataModels.ErpCommerceProperty>();

            foreach (EstimateRequestOrderLine estimateRequestOrderLine in order)
            {
                ERPDataModels.ErpCommercePropertyValue erpCommercePropertyValue = new ERPDataModels.ErpCommercePropertyValue();
                erpCommercePropertyValue.DecimalValue = estimateRequestOrderLine.quantity;

                ERPDataModels.ErpCommerceProperty erpCommerceProperty = new ERPDataModels.ErpCommerceProperty();
                erpCommerceProperty.Key = estimateRequestOrderLine.product;
                erpCommerceProperty.Value = erpCommercePropertyValue;

                shippingChargesInput.Add(erpCommerceProperty);
            }
            return shippingChargesInput;
        }

        private EstimateSmallResponse AssembleResponse(IEnumerable<ErpCommerceProperty> shippingChargesOutput, EstimateRequest estimateRequest)
        {
            
            List<EstimateResponseOrderLine> estimateResponseOrderLinesReturn = new List<EstimateResponseOrderLine>();
            Decimal totalShippingCharges = 0;
            List<String> failedOrders = new List<string>();
            bool errorStatus = false;
            string orderStatus = "complete";

            foreach (ERPDataModels.ErpCommerceProperty estimateResponseOrderLine in shippingChargesOutput)
            {
                ERPDataModels.ErpCommercePropertyValue erpCommercePropertyValue = estimateResponseOrderLine.Value;

                errorStatus = false;
                if (estimateResponseOrderLine.Key.ToUpper().Contains("ERROR"))
                {
                    errorStatus = true;
                    orderStatus = "inComplete";
                }

                EstimateResponseOrderLine estimateResponseOrderLineData = new EstimateResponseOrderLine(
                    estimateResponseOrderLine.Key,
                    (decimal)erpCommercePropertyValue.DecimalValue,
                    errorStatus);

                estimateResponseOrderLinesReturn.Add(estimateResponseOrderLineData);

                totalShippingCharges += (decimal)erpCommercePropertyValue.DecimalValue;
            }

            foreach (ErpCommerceProperty shippingCharge in shippingChargesOutput)
            {
                if (shippingCharge.Key.ToUpper().Contains("ERROR"))
                { 
                    for (int index = 0; index <= estimateRequest.order.Count - 1; index++)
                    {
                        if (shippingCharge.Key.Contains(estimateRequest.order[index].product))
                        {
                            failedOrders.Add(estimateRequest.order[index].product);
                        }
                    }
                }
            }

            string failProducts = string.Join(",", failedOrders.ToArray());

            //EstimateResponse estimateResponseReturn = new EstimateResponse((float)totalShippingCharges, orderStatus, failProducts, estimateResponseOrderLinesReturn);
            EstimateSmallResponse estimateResponseReturn = new EstimateSmallResponse(orderStatus, "", (float)totalShippingCharges);

            return estimateResponseReturn;
        }
        #endregion

    }

    #region Estimate Request, Response, RequestOrderLine and ResponseOrderLine classes

    /// <summary>
    /// Order Line received in Estimate Request. Made into class for proper JSON deserialization.
    /// </summary>
    public class EstimateRequestOrderLine
    {
        /// <summary>
        /// Initializes a new instance of EstimateRequestOrderLine
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        public EstimateRequestOrderLine(string product, decimal quantity)
        {
            this.product = product;
            this.quantity = quantity;
        }

        /// <summary>
        /// Product for EstimateRequestOrderLine
        /// </summary>
        public string product { get; set; }

        /// <summary>
        /// Quantity for EstimateRequestOrderLine
        /// </summary>
        public decimal quantity { get; set; }
    }

    /// <summary>
    /// Order line sent back in Estimate Response. Made into class for proper JSON deserialization.
    /// </summary>
    public class EstimateResponseOrderLine
    {

        /// <summary>
        /// Initializes a new instance of EstimateResponseOrderLine
        /// </summary>
        /// <param name="product"></param>
        /// <param name="shippingCharges"></param>
        /// <param name="errorStatus"></param>
        public EstimateResponseOrderLine(string product, decimal shippingCharges, bool errorStatus)
        {
            this.product = product;
            this.shippingCharges = shippingCharges;
            this.errorStatus = errorStatus;
        }

        /// <summary>
        /// Product for EstimateResponseOrderLine
        /// </summary>
        public string product { get; set; }

        /// <summary>
        /// Shipping Charges for EstimateResponseOrderLine
        /// </summary>
        public decimal shippingCharges { get; set; }

        /// <summary>
        /// Error Status for EstimateResponseOrderLine
        /// </summary>
        public bool errorStatus { get; set; }

    }

    /// <summary>
    /// Represents estimate shipping charges request.
    /// </summary>
    public class EstimateRequest
    {
        /// <summary>
        /// Shipping Method for Estimate Shipping Charges request
        /// </summary>
        [Required]
        public String shippingMethod { get; set; }

        /// <summary>
        /// Dictionary of order for Estimate Shipping Charges request
        /// </summary>
        public List<EstimateRequestOrderLine> order { get; set; }

        /// <summary>
        /// Postal code for Estimate Shipping Charges request
        /// </summary>
        [Required]
        public String postalCode { get; set; }

        /// <summary>
        /// Country code for Estimate Shipping Charges request
        /// </summary>
        [Required]
        public String countryCode { get; set; }

        /// <summary>
        /// Address 1 for Estimate Shipping Charges request
        /// </summary>
        [Required]
        public String address1 { get; set; }

        /// <summary>
        /// Address 2 for Estimate Shipping Charges request
        /// </summary>
        [Required]
        public String address2 { get; set; }
    }

    // TODO - check for detail parameter and send whole array back, else sum
    /// <summary>
    /// Represents estimate shipping charges response
    /// </summary>
    public class EstimateResponse
    {

        /// <summary>
        /// Initializes a new instance of the EstimatedResponse
        /// </summary>
        /// <param name="charges">Order shipping charges</param>
        /// <param name="status">Status of Order shipping charges</param>
        /// <param name="failProducts">Failed products of Order shipping charges</param>
        /// <param name="responseDetail">Dictionary object containing product and shipping charges</param>
        public EstimateResponse(float charges, string status, string failProducts, List<EstimateResponseOrderLine> responseDetail)
        {
            this.orderShippingCharges = new decimal(charges);
            this.status = status;
            this.failProducts = failProducts;
            this.detail = responseDetail;
        }

        /// <summary>
        /// Order shipping charges
        /// </summary>
        public decimal orderShippingCharges { get; set; }

        /// <summary>
        /// Status of Shipping Charges
        /// </summary>
        /// 
        public string status { get; set; }

        /// <summary>
        /// Failed Products
        /// </summary>
        public string failProducts { get; set; }

        /// <summary>
        /// Dictionary of product and shipping charges
        /// </summary>
        public List<EstimateResponseOrderLine> detail { get; set; }

    }

    /// <summary>
    /// Represents estimate shipping charges response
    /// </summary>
    public class EstimateSmallResponse
    {

        /// <summary>
        /// Initializes a new instance of the EstimatedResponse
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <param name="charges"></param>
        public EstimateSmallResponse(string status, string message, float charges)
        { 
            this.status = status;
            this.message = message;
            this.orderShippingCharges = new decimal(charges);
        }

        /// <summary>
        /// Status of Shipping Charges
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// Order shipping charges
        /// </summary>
        public decimal orderShippingCharges { get; set; }

    }

    #endregion

}
