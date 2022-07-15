using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels;
using VSI.EDGEAXConnector.ERPDataModels.Custom;
using VSI.EDGEAXConnector.Logging;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <summary>
    /// UpdateSalesOrderController defines properties and methods for API controller for Update Sales order.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class UpdateSalesOrderController : ApiBaseController
    {
        /// <summary>
        /// Update Sales Order Controller 
        /// </summary>
        public UpdateSalesOrderController()
        {
            ControllerName = "UpdateSalesOrderController";
        }

        #region API Methods

        /// <summary>
        /// Cancel Contract/ContractLine by salesId or salesLineRecId  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("UpdateSalesOrder/CancelContract")]
        public CancelContractResponse CancelContract([FromBody] CancelContractRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new CancelContractResponse(false, message, null);
            }
            if (string.IsNullOrEmpty(request.SalesOrderId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "SalesOrderId");
                return new CancelContractResponse(false, message, null);
            }
                        
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " SalesOrderId: " + request.SalesOrderId + " and SalesLineRecId (optional) : " + request.SalesLineRecId);
            CancelContractResponse cancelContractResponse = new CancelContractResponse(false, "", "");

            try
            {
                try
                {                   
                    var erpUpdateSalesOrderController = erpAdapterFactory.CreateUpdateSalesOrderController(currentStore.StoreKey);
                    var erpResponse = erpUpdateSalesOrderController.CancelContract(request.SalesOrderId, request.SalesLineRecId);

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));

                    if (erpResponse.Success)
                    {                        
                        cancelContractResponse = new CancelContractResponse(true, erpResponse.Message, erpResponse.Result);
                    }
                    else
                    {
                        cancelContractResponse = new CancelContractResponse(false, erpResponse.Message, erpResponse.Result);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), GetRequestGUID(Request));
                    throw new Exception(message);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cancelContractResponse = new CancelContractResponse(false, message, null);
                return cancelContractResponse;
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                cancelContractResponse = new CancelContractResponse(false, message, null);
                return cancelContractResponse;
            }

            return cancelContractResponse;
        }

        /// <summary>
        /// Cancel Contract/ContractLine by salesId or salesLineRecId  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("UpdateSalesOrder/TerminateContract")]
        public TerminateContractResponse TerminateContract([FromBody] TerminateContractRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new TerminateContractResponse(false, message, null);
            }
            if (string.IsNullOrEmpty(request.SalesOrderId) && string.IsNullOrEmpty(request.ChannelReferenceId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "SalesOrderId/ChannelReferenceId");
                return new TerminateContractResponse(false, message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " SalesOrderId: " + request.SalesOrderId + ", ChannelReferenceId: " + request.ChannelReferenceId + " and SalesLineRecId (optional) : " + request.SalesLineRecId);
            TerminateContractResponse terminateContractResponse = new TerminateContractResponse(false, "", "");

            try
            {
                try
                {
                    var erpUpdateSalesOrderController = erpAdapterFactory.CreateUpdateSalesOrderController(currentStore.StoreKey);
                    var erpResponse = erpUpdateSalesOrderController.TerminateContract(request.SalesOrderId, request.ChannelReferenceId, request.SalesLineRecId);

                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));

                    if (erpResponse.Success)
                    {
                        terminateContractResponse = new TerminateContractResponse(true, erpResponse.Message, erpResponse.Result);
                    }
                    else
                    {
                        terminateContractResponse = new TerminateContractResponse(false, erpResponse.Message, erpResponse.Result);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), GetRequestGUID(Request));
                    throw new Exception(message);
                }
            }
            catch (ArgumentException ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                terminateContractResponse = new TerminateContractResponse(false, message, null);
                return terminateContractResponse;
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                terminateContractResponse = new TerminateContractResponse(false, message, null);
                return terminateContractResponse;
            }

            return terminateContractResponse;
        }

        /// <summary>
        /// Switch / Migrate / Sell-AddOn Contract
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("UpdateSalesOrder/UpdateContract")]
        public UpdateContractResponse UpdateContract([FromBody] UpdateContractRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            UpdateContractResponse updateContractResponse = new UpdateContractResponse(false, "", "");

            updateContractResponse = this.ValidateUpdateContractRequest(request);

            if (updateContractResponse != null)
            {
                return updateContractResponse;
            }
            else
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(request));
                
                try
                {
                    try
                    {
                        ErpTenderLine tenderLine = new ErpTenderLine();

                        if (request.Payment != null)
                        {
                            tenderLine.Amount = request.Payment.Amount;
                            tenderLine.CardTypeId = request.Payment.CardType;
                            tenderLine.MaskedCardNumber = request.Payment.CardNumber;
                            tenderLine.CardOrAccount = request.Payment.CardHolder;
                            tenderLine.ExpMonth = request.Payment.expirationMonth;
                            tenderLine.ExpYear = request.Payment.expirationYear;
                            tenderLine.PayerId = request.Payment.PayerId;
                            tenderLine.ParentTransactionId = request.Payment.ParentTransactionId;
                            tenderLine.Email = request.Payment.Email;
                            tenderLine.Note = request.Payment.Note;
                            tenderLine.Authorization = request.Payment.Authorization;
                            tenderLine.CardToken = request.Payment.CardToken;
                            tenderLine.TenderTypeId = request.Payment.ProcessorId;

                            tenderLine.CustomerId = request.Payment.EcomCustomerId;
                            tenderLine.BillingAddress = request.Payment.BillingAddress;
                        }
                        else
                        {
                            tenderLine = null;
                        }
                        var erpUpdateSalesOrderController = erpAdapterFactory.CreateUpdateSalesOrderController(currentStore.StoreKey);
                        var erpResponse = erpUpdateSalesOrderController.UpdateContract(request.CrossSellType, request.SalesOrderId, request.SalesLineRecId, request.NewSalesLine, tenderLine, request.AddOns, GetRequestGUID(Request));

                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));

                        if (erpResponse.Success)
                        {
                            updateContractResponse = new UpdateContractResponse(true, erpResponse.Message, erpResponse.Result);
                        }
                        else
                        {
                            updateContractResponse = new UpdateContractResponse(false, erpResponse.Message, erpResponse.Result);
                        }
                    }
                    catch (Exception ex)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, ex.Message.ToString(), GetRequestGUID(Request));
                        throw new Exception(message);
                    }
                }
                catch (ArgumentException ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                    updateContractResponse = new UpdateContractResponse(false, message, null);
                    return updateContractResponse;
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                    updateContractResponse = new UpdateContractResponse(false, message, null);
                    return updateContractResponse;
                }
            }

            return updateContractResponse;
        }

        #endregion

        #region Private

        /// <summary>
        /// ValidateUpdateContractRequest validate Update Contract Request Object.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private UpdateContractResponse ValidateUpdateContractRequest(UpdateContractRequest request)
        {
            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new UpdateContractResponse(false, message, null);
            }
            else
            {
                if (request.CrossSellType.ToString() != Enum.GetName(typeof(ErpTMVCrosssellType), 10) &&
                    request.CrossSellType.ToString() != Enum.GetName(typeof(ErpTMVCrosssellType), 20) &&
                    request.CrossSellType.ToString() != Enum.GetName(typeof(ErpTMVCrosssellType), 30) &&
                    request.CrossSellType.ToString() != Enum.GetName(typeof(ErpTMVCrosssellType), 40) &&
                    request.CrossSellType.ToString() != Enum.GetName(typeof(ErpTMVCrosssellType), 41))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "CrossSellType");
                    return new UpdateContractResponse(false, message, null);
                }
                if (string.IsNullOrEmpty(request.SalesOrderId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "SalesOrderId");
                    return new UpdateContractResponse(false, message, null);
                }
                if (string.IsNullOrEmpty(request.SalesLineRecId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "SalesLineRecId");
                    return new UpdateContractResponse(false, message, null);
                }
                if (request.CrossSellType.ToString() != Enum.GetName(typeof(ErpTMVCrosssellType), 40))
                {
                    if (request.NewSalesLine == null)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "NewSalesLine");
                        return new UpdateContractResponse(false, message, null);
                    }
                    if (request.NewSalesLine != null && string.IsNullOrEmpty(request.NewSalesLine.ItemId))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "newSalesLine.ItemId");
                        return new UpdateContractResponse(false, message, null);
                    }
                    if (request.NewSalesLine != null && string.IsNullOrEmpty(request.NewSalesLine.InventDimensionId))
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "newSalesLine.InventDimensionId");
                        return new UpdateContractResponse(false, message, null);
                    }
                    if (request.NewSalesLine != null && request.NewSalesLine.Quantity == 0)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "newSalesLine.Quantity");
                        return new UpdateContractResponse(false, message, null);
                    }
                    if (request.Payment == null)
                    {
                        string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "Payment");
                        return new UpdateContractResponse(false, message, null);
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Add Contract Relation by orignal sales order and new sales order info
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateSalesOrder/AddContractRelation")]
        [Authorize(Roles = "eCommerce")]
        public AddContractRelationResponse AddContractRelation([FromBody] AddContractRelationRequest request)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (request == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new AddContractRelationResponse(false, message, null);
            }
            if (request.ContractRelationType.ToString() != Enum.GetName(typeof(ErpTMVCrosssellType), 10) &&
                    request.ContractRelationType.ToString() != Enum.GetName(typeof(ErpTMVCrosssellType), 20) &&
                    request.ContractRelationType.ToString() != Enum.GetName(typeof(ErpTMVCrosssellType), 30) &&
                    request.ContractRelationType.ToString() != Enum.GetName(typeof(ErpTMVCrosssellType), 40) &&
                    request.ContractRelationType.ToString() != Enum.GetName(typeof(ErpTMVCrosssellType), 41))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40003, currentStore, MethodBase.GetCurrentMethod().Name, "CrossSellType");
                return new AddContractRelationResponse(false, message, null);
            }
            if (string.IsNullOrEmpty(request.orgSalesOrderId))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "orgSalesOrderId");
                return new AddContractRelationResponse(false, message, null);
            }
            if (string.IsNullOrEmpty(request.orgSalesLineRecIds))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "orgSalesLineRecIds");
                return new AddContractRelationResponse(false, message, null);
            }
            if (string.IsNullOrEmpty(request.newSalesLineRecIds))
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "newSalesLineRecIds (Provide \"0,0,0\" if have not values)");
                return new AddContractRelationResponse(false, message, null);
            }

            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " CrossSellType : " + request.ContractRelationType + ", orgSalesOrderId : " + request.orgSalesOrderId + " and orgSalesLineRecIds : " + request.orgSalesLineRecIds);
            AddContractRelationResponse addContractRelationResponse = new AddContractRelationResponse(false, "", "");

            try
            {
                try
                {
                    var erpUpdateSalesOrderController = erpAdapterFactory.CreateUpdateSalesOrderController(currentStore.StoreKey);
                    var erpResponse = erpUpdateSalesOrderController.AddContractRelation(request.ContractRelationType, request.orgSalesOrderId, request.orgSalesLineRecIds, request.newSalesOrderId, request.newSalesLineRecIds);
                    
                    CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpResponse));

                    if (erpResponse.Success)
                    {
                        addContractRelationResponse = new AddContractRelationResponse(true, erpResponse.Message, erpResponse.Result);
                    }
                    else
                    {
                        addContractRelationResponse = new AddContractRelationResponse(false, erpResponse.Message, erpResponse.Result);
                    }
                }
                catch (Exception ex)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, ex.Message.ToString());
                    ex.Data.Add("AddContractRelationResponse:", JsonConvert.SerializeObject(ex));
                    throw ex;
                }
            }
            catch (ArgumentException exp)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message.ToString());
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy, ControllerName);
                return addContractRelationResponse;
            }
            catch (Exception exp)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, exp.Message.ToString());
                CustomLogger.LogException(exp, currentStore.StoreId, currentStore.CreatedBy, ControllerName);
                return addContractRelationResponse;
            }

            return addContractRelationResponse;
        }

        #endregion

        #region UpdateSalesOrder Request, Response classes

        /// <summary>
        /// Cancel Contract Request
        /// </summary>
        public class CancelContractRequest
        {
            /// <summary>
            /// Sales Order Id property
            /// </summary>
            [Required]
            public string SalesOrderId { get; set; }
            /// <summary>
            /// SalesLine Id property
            /// </summary>
            [Required]
            public string SalesLineRecId { get; set; }
        }

        /// <summary>
        /// Cancel Contract Response
        /// </summary>
        public class CancelContractResponse
        {
            /// <summary>
            /// Initialize a new instance of the CancelContractResponse 
            /// </summary>
            /// <param name="status">status of cancel contract response</param>
            /// <param name="message">message of cancel contract response</param>
            /// <param name="result">result of cancel contract response</param>
            public CancelContractResponse(bool status, string message, string result)
            {
                this.status = status;
                this.result = result;
                this.message = message;
            }

            /// <summary>
            /// status
            /// </summary>
            public bool status { get; set; }


            /// <summary>
            /// message
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// result
            /// </summary>
            public string result { get; set; }

        }

        /// <summary>
        /// Terminate Contract Request
        /// </summary>
        public class TerminateContractRequest
        {
            /// <summary>
            /// Sales Order Id property
            /// </summary>
            [Required]
            public string SalesOrderId { get; set; }

            /// <summary>
            /// Channel Reference Id property
            /// </summary>
            [Required]
            public string ChannelReferenceId { get; set; }
            /// <summary>
            /// SalesLine Id property
            /// </summary>
            [Required]
            public string SalesLineRecId { get; set; }
        }

        /// <summary>
        /// Terminate Contract Response
        /// </summary>
        public class TerminateContractResponse
        {
            /// <summary>
            /// Initialize a new instance of the TerminateContractResponse 
            /// </summary>
            /// <param name="status">status of Terminate contract response</param>
            /// <param name="message">message of Terminate contract response</param>
            /// <param name="result">result of Terminate contract response</param>
            public TerminateContractResponse(bool status, string message, string result)
            {
                this.status = status;
                this.result = result;
                this.message = message;
            }

            /// <summary>
            /// status
            /// </summary>
            public bool status { get; set; }


            /// <summary>
            /// message
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// result
            /// </summary>
            public string result { get; set; }

        }

        /// <summary>
        /// Update Contract Request
        /// </summary>
        public class UpdateContractRequest
        {
            /// <summary>
            /// constructor
            /// </summary>
            public UpdateContractRequest()
            {
                NewSalesLine = new ErpAdditionalSalesLine();
                AddOns = new List<ErpAdditionalSalesLine>();
            }
            /// <summary>
            /// Sales Order CrossSellType property
            /// </summary>
            public ErpTMVCrosssellType CrossSellType { get; set; }
            /// <summary>
            /// Sales Order Id of existing sales order
            /// </summary>
            [Required]
            public string SalesOrderId { get; set; }
            /// <summary>
            /// SalesLine Id of existing sales order
            /// </summary>
            [Required]
            public string SalesLineRecId { get; set; }
            /// <summary>
            /// SalesLine Id to be Update Contat property
            /// </summary>
            public ErpAdditionalSalesLine NewSalesLine { get; set; }
            /// <summary>
            /// Payment Id to be Update Contat property
            /// </summary>
            public PaymentDetails Payment { get; set; }
            /// <summary>
            /// AddOns Id to be Update Contat property
            /// </summary>
            public List<ErpAdditionalSalesLine> AddOns { get; set; }
        }

        /// <summary>
        /// PaymentDetails
        /// </summary>
        public class PaymentDetails
        {
            /// <summary>
            /// Constructor of PaymentDetails
            /// </summary>
            public PaymentDetails()
            {
                BillingAddress = new ErpAddress();
            }

            /// <summary>
            /// CardType of PaymentDetails
            /// </summary>
            public string CardType { get; set; }
            /// <summary>
            /// CardNumber of PaymentDetails
            /// </summary>
            public string CardNumber { get; set; }
            /// <summary>
            /// CardHolder of PaymentDetails
            /// </summary>
            public string CardHolder { get; set; }
            /// <summary>
            /// expirationMonth of PaymentDetails
            /// </summary>
            public int? expirationMonth { get; set; }
            /// <summary>
            /// expirationYear of PaymentDetails
            /// </summary>
            public int? expirationYear { get; set; }
            /// <summary>
            /// PayerId of PaymentDetails
            /// </summary>
            public string PayerId { get; set; }
            /// <summary>
            /// ParentTransactionId of PaymentDetails
            /// </summary>
            public string ParentTransactionId { get; set; }
            /// <summary>
            /// Email of PaymentDetails
            /// </summary>
            public string Email { get; set; }
            /// <summary>
            /// Note of PaymentDetails
            /// </summary>
            public string Note { get; set; }
            /// <summary>
            /// Authorization of PaymentDetails
            /// </summary>
            public string Authorization { get; set; }
            /// <summary>
            /// CardToken of PaymentDetails
            /// </summary>
            public string CardToken { get; set; }
            /// <summary>
            /// Amount of PaymentDetails
            /// </summary>
            public decimal Amount { get; set; }
            /// <summary>
            /// Amount of PaymentDetails
            /// </summary>
            public string ProcessorId { get; set; }

            /// <summary>
            /// EcomCustomerId of PaymentDetails
            /// </summary>
            public string EcomCustomerId { get; set; }

            /// <summary>
            /// BillingAddress of PaymentDetails
            /// </summary>
            public ErpAddress BillingAddress { get; set; }
        }

        /// <summary>
        /// Update Contract Response
        /// </summary>
        public class UpdateContractResponse
        {
            /// <summary>
            /// Initialize a new instance of the SwitchContractResponse 
            /// </summary>
            /// <param name="status">status of Switch contract response</param>
            /// <param name="message">message of Switch contract response</param>
            /// <param name="result">result of Switch contract response</param>
            public UpdateContractResponse(bool status, string message, string result)
            {
                this.status = status;
                this.result = result;
                this.message = message;
            }

            /// <summary>
            /// status
            /// </summary>
            public bool status { get; set; }


            /// <summary>
            /// message
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// result
            /// </summary>
            public string result { get; set; }

        }

        /// <summary>
        /// Add Contract Relation Request
        /// </summary>
        public class AddContractRelationRequest
        {
            /// <summary>
            /// Sales Order CrossSellType property
            /// </summary>
            public ErpTMVContractRelationType ContractRelationType { get; set; }

            /// <summary>
            /// Original Sales Order Id property
            /// </summary>
            [Required]
            public string orgSalesOrderId { get; set; }
            /// <summary>
            /// Orignal SalesLine Ids property
            /// </summary>
            [Required]
            public string orgSalesLineRecIds { get; set; }

            /// <summary>
            /// New Sales Order Id property
            /// </summary>
            [Required]
            public string newSalesOrderId { get; set; }
            /// <summary>
            /// New SalesLine Ids property
            /// </summary>
            [Required]
            public string newSalesLineRecIds { get; set; }
        }

        /// <summary>
        /// Add Contract Relation Response
        /// </summary>
        public class AddContractRelationResponse
        {
            /// <summary>
            /// Initialize a new instance of the CancelContractResponse 
            /// </summary>
            /// <param name="status">status of cancel contract response</param>
            /// <param name="message">message of cancel contract response</param>
            /// <param name="result">result of cancel contract response</param>
            public AddContractRelationResponse(bool status, string message, string result)
            {
                this.status = status;
                this.result = result;
                this.message = message;
            }

            /// <summary>
            /// status
            /// </summary>
            public bool status { get; set; }


            /// <summary>
            /// message
            /// </summary>
            public string message { get; set; }

            /// <summary>
            /// result
            /// </summary>
            public string result { get; set; }

        }

        #endregion
    }
}