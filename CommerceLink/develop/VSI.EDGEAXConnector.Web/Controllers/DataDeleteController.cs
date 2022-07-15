using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Enums;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web
{
    /// <summary>
    /// DataDeleteController defines properties and methods for API controller for payment link.
    /// </summary>
    [RoutePrefix("api/v1")]
    public class DataDeleteController : ApiBaseController
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_erpAdapterFactory"></param>
        /// <param name="_eComAdapterFactory"></param>
        public DataDeleteController()
        {
            ControllerName = "DataDeleteController";
        }

        #region API Methods

        /// <summary>
        /// Save Data Delete Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpPost]
        [Route("DataDelete/Save")]
        public DataDeleteSaveResponse Save([FromBody] DataDeleteSaveRequest request)
        {
            DataDeleteSaveResponse customerResponse = new DataDeleteSaveResponse(false, "");
            try
            {
                customerResponse = ValidateDataDeleteSave(request);
                if (customerResponse != null)
                {
                    return customerResponse;
                }

                DataDeleteDAL dal = new DataDeleteDAL(currentStore.StoreKey);
                var model = dal.GetById(request.RequestId);
                if (model != null)
                {
                    customerResponse = new DataDeleteSaveResponse(false, "Request already exists");
                    return customerResponse;
                }

                var entity = this.MapRequestToDataDelete(request);
                dal.Add(entity);
                customerResponse = new DataDeleteSaveResponse(true, "Delete data request added to queue.");
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                customerResponse = new DataDeleteSaveResponse(false, message);
                return customerResponse;
            }

            return customerResponse;
        }

        /// <summary>
        /// Get Data Delete Request by requestId
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        [RequestResponseLog]
        [HttpGet]
        [Route("DataDelete/Get")]
        public DataDeleteGetResponse Get([FromUri] string requestId)
        {
            DataDeleteGetResponse response = new DataDeleteGetResponse(false, "", null);
            try
            {
                if (string.IsNullOrWhiteSpace(requestId))
                {
                    response = new DataDeleteGetResponse(false, "Invalid request", null);
                    return response;
                }

                DataDeleteDAL dal = new DataDeleteDAL(currentStore.StoreKey);
                var model = dal.GetById(requestId);
                if (model == null)
                {
                    return new DataDeleteGetResponse(false, "Request not found", null);
                }

                response = new DataDeleteGetResponse(true, "Success", model);
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                response = new DataDeleteGetResponse(false, message, null);
                return response;
            }
            return response;
        }

        #endregion

        #region "Private Methods"

        private DataDeleteSaveResponse ValidateDataDeleteSave(DataDeleteSaveRequest request)
        {
            try
            {
                if (request == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                    return new DataDeleteSaveResponse(false, message);
                }
                else if (String.IsNullOrWhiteSpace(request.RequestId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "DataDeleteSaveRequest.RequestId");
                    return new DataDeleteSaveResponse(false, message);
                }
                else if (String.IsNullOrWhiteSpace(request.ContactPersonEmail))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "DataDeleteSaveRequest.ContactPersonEmail");
                    return new DataDeleteSaveResponse(false, message);
                }
                else if (String.IsNullOrWhiteSpace(request.ContactPersonId))
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "DataDeleteSaveRequest.ContactPersonId");
                    return new DataDeleteSaveResponse(false, message);
                }

            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40000, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(ex));
                return new DataDeleteSaveResponse(false, message);
            }

            return null;
        }

        private DataDelete MapRequestToDataDelete(DataDeleteSaveRequest request)
        {
            return new DataDelete()
            {
                RequestId = request.RequestId,
                CustomerAccountNumber = request.CustomerAccountNumber,
                ContactPersonEmail = request.ContactPersonEmail,
                ContactPersonId = request.ContactPersonId
            };
        }

        #endregion

        #region Request, Response classes

        /// <summary>
        /// DataDeleteSave request
        /// </summary>
        public class DataDeleteSaveRequest
        {
            /// <summary>
            /// Data request id
            /// </summary>
            [Required]
            public string RequestId { get; set; }

            /// <summary>
            /// Customer Account Number
            /// </summary>
            [Required]
            public string CustomerAccountNumber { get; set; }

            /// <summary>
            /// contact person email
            /// </summary>
            [Required]
            public string ContactPersonEmail { get; set; }

            /// <summary>
            /// contact person id
            /// </summary>
            [Required]
            public string ContactPersonId { get; set; }

            /// <summary>
            /// Request Status
            /// </summary>
            [Required]
            public string Status { get; set; }
        }

        /// <summary>
        /// DataDeleteSave response
        /// </summary>
        public class DataDeleteSaveResponse
        {
            /// <summary>
            /// Initialize a new instance of the DataDeleteSave response 
            /// </summary>
            /// <param name="status">status of the DataDeleteSave response</param>
            /// <param name="message">message for DataDeleteSave response</param>

            public DataDeleteSaveResponse(bool status, string message)
            {
                this.Status = status;
                this.Message = message;
            }

            /// <summary>
            /// status of the DataDeleteSave response
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// message of the DataDeleteSave response
            /// </summary>
            public string Message { get; set; }

        }

        /// <summary>
        /// DataDeleteGet response
        /// </summary>
        public class DataDeleteGetResponse
        {
            /// <summary>
            /// Initialize a new instance of the DataDeleteGet response 
            /// </summary>
            /// <param name="status">status of the DataDeleteGet response</param>
            /// <param name="message">message for DataDeleteGet response</param>
            /// <param name="model">Data Delete model for DataDeleteGet response</param>

            public DataDeleteGetResponse(bool status, string message, DataDelete model)
            {
                this.Status = status;
                this.Message = message;
                if (model != null)
                {
                    this.CustomerAccountNumber = model.CustomerAccountNumber;
                    this.ContactPersonId = model.ContactPersonId;
                    this.ContactPersonEmail = model.ContactPersonEmail;
                    this.RequestId = model.RequestId;
                    if (Enum.IsDefined(typeof(DataDeleteStatus), model.Status))
                    {
                        this.RequestStatus = ((DataDeleteStatus)model.Status).ToString();
                    }
                }
            }

            /// <summary>
            /// status of the DataDeleteGet response
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// message of the DataDeleteGet response
            /// </summary>
            public string Message { get; set; }

            /// <summary>
            /// Customer account number of the DataDeleteGet response
            /// </summary>
            public string CustomerAccountNumber { get; set; }
            /// <summary>
            /// Contact person Id of the DataDeleteGet response
            /// </summary>
            public string ContactPersonId { get; set; }
            /// <summary>
            /// Contact person Email of the DataDeleteGet response
            /// </summary>
            public string ContactPersonEmail { get; set; }
            /// <summary>
            /// Request Id of the DataDeleteGet response
            /// </summary>
            public string RequestId { get; set; }
            /// <summary>
            /// Request status of the DataDeleteGet response
            /// </summary>
            public string RequestStatus { get; set; }
        }

        #endregion
    }
}
