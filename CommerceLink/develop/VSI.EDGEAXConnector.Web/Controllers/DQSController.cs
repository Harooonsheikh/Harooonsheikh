using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Web.Http;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.Enums.Enums;
using VSI.EDGEAXConnector.ERPDataModels.Custom.Responses;
using VSI.EDGEAXConnector.Web.ActionFilters;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <summary>
    /// DQSController defines properties and methods for API controller for DQS.
    /// </summary>
    [RoutePrefix("api/v1")]
    [SanitizeInput]
    public class DQSController : ApiBaseController
    { 
        /// <summary>
        /// DQS Controller 
        /// </summary>
        public DQSController()
        {
            ControllerName = "DQSController";
        }

        #region API Methods
        /// <summary>
        /// CreateOrUpdateCart creates and updates cart with provided details.
        /// </summary>
        /// <param name="dqsRequest">cart request to be create/update</param>
        /// <returns>CartResponse</returns>
        [HttpPost]
        [Route("DQS/GetDQS")]
        public DQSResponse GetDQS([FromBody] DQSRequest dqsRequest)
        {
            CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10000, currentStore, MethodBase.GetCurrentMethod().Name);

            if (dqsRequest != null)
            {
                CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10001, currentStore, MethodBase.GetCurrentMethod().Name, " dqs: " + JsonConvert.SerializeObject(dqsRequest));
            }

            DQSResponse dqsResponse = new DQSResponse(false, "", null);

            try
            {
                dqsResponse = this.ValidateDQSRequest(dqsRequest);

                if (dqsResponse != null)
                {
                    return dqsResponse;
                }
                else
                {
                    var erpDQSController = erpAdapterFactory.CreateDQSController(currentStore.StoreKey);

                    ErpDQSResponse erpDQSResponse = erpDQSController.GetDQS(
                        String.IsNullOrEmpty(dqsRequest.UserName) ? configurationHelper.GetSetting(EXTERNALWEBAPI.DQS_User_Name) : dqsRequest.UserName,
                        String.IsNullOrEmpty(dqsRequest.Password) ? configurationHelper.GetSetting(EXTERNALWEBAPI.DQS_Password) : dqsRequest.Password,
                        String.IsNullOrEmpty(dqsRequest.WorkflowName) ? configurationHelper.GetSetting(EXTERNALWEBAPI.DQS_Workflow_Name) : dqsRequest.WorkflowName,
                        JsonConvert.SerializeObject(dqsRequest.Header),
                        String.IsNullOrEmpty(dqsRequest.Endpoint) ? configurationHelper.GetSetting(EXTERNALWEBAPI.DQS_Endpoint) : dqsRequest.Endpoint);

                    if (erpDQSResponse.Success)
                    {
                        CommerceLinkLogger.LogTrace(CommerceLinkLoggerMessages.VSICL10002, currentStore, MethodBase.GetCurrentMethod().Name, JsonConvert.SerializeObject(erpDQSResponse));
                        dqsResponse = new DQSResponse(erpDQSResponse.Success, erpDQSResponse.Message, erpDQSResponse.Result);
                    }
                    else
                    {
                        dqsResponse = new DQSResponse(erpDQSResponse.Success, erpDQSResponse.Message, null);
                    }
                }
            }
            catch (Exception ex)
            {
                string message = CommerceLinkLogger.LogException(currentStore, ex, MethodBase.GetCurrentMethod().Name, GetRequestGUID(Request));
                return new DQSResponse(false, message, null);
            }

            return dqsResponse;
        }

        #endregion

        #region DQS Request, Response classes

        /// <summary>
        /// Represents create/update dqs request
        /// </summary>
        public class DQSRequest
        {
            /// <summary>
            /// UserName for DQS Service
            /// </summary>
            [Required]
            public string UserName { get; set; }

            /// <summary>
            /// Password for DQS Service
            /// </summary>
            [Required]
            public string Password { get; set; }

            /// <summary>
            /// WorkFlowName for DQS Service
            /// </summary>
            [Required]
            public string WorkflowName { get; set; }

            /// <summary>
            /// Endpoint for DQS Service
            /// </summary>
            [Required]
            public string Endpoint { get; set; }

            /// <summary>
            /// Header object
            /// </summary>
            public Header Header { get; set; }
        }

        /// <summary>
        /// Represents DQS response
        /// </summary>
        public class DQSResponse
        {
            /// <summary>
            /// Initializes a new instance of the DQS response
            /// </summary>
            /// <param name="status">status</param>
            /// <param name="errorMessage">error Message</param>
            /// <param name="result">result</param>
            public DQSResponse(bool status, string errorMessage, string result)
            {
                this.Status = status;
                this.ErrorMessage = errorMessage;
                this.Result = result;
            }

            /// <summary>
            /// Status of cart
            /// </summary>
            public bool Status { get; set; }

            /// <summary>
            /// ErrorMessage of DQS Service
            /// </summary>
            public string ErrorMessage { get; set; }

            /// <summary>
            /// Result of DQS Service
            /// </summary>
            public string Result { get; set; }

        }

        public class Header
        {
            public string address1_city { get; set; }
            public string address1_country { get; set; }
            public string address1_line1 { get; set; }
            public string address1_postalcode { get; set; }
            public string entitytypename { get; set; }
            public string id { get; set; }
            public string name { get; set; }
            public string orb_sanctionlistchk { get; set; }
            public string orb_sanctionlistinformation { get; set; }
            public string orb_sanctionliststatuscode { get; set; }
            public string orb_checksum { get; set; }
            List<Position> positions { get; set; }
        }

        public class Position
        {
            public string orb_sanctionlistresultid { get; set; }
            public string entitytypename { get; set; }
            public string id { get; set; }
            public string modifiedby { get; set; }
            public string modifiedbyname { get; set; }
            public string modifiedon { get; set; }
            public string orb_checksum { get; set; }
            public string orb_manualstatuscode { get; set; }
            public string orb_manualstatusreason { get; set; }
            public string orb_resultdetails { get; set; }
            public string orb_sanctionlistid { get; set; }
            public string orb_sanctionlistresultcode { get; set; }
            public string orb_sanctionlistversion { get; set; }

        }

        #endregion


        #region Private Handler

        /// <summary>
        /// ValidateDQSRequest validates dqs object.
        /// </summary>
        /// <param name="dqsRequest"></param>
        /// <returns></returns>
        private DQSResponse ValidateDQSRequest(DQSRequest dqsRequest)
        {
            if (dqsRequest == null)
            {
                string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40001, currentStore, MethodBase.GetCurrentMethod().Name);
                return new DQSResponse(false, message, null);
            }
            else
            {
               if (dqsRequest.Header == null)
                {
                    string message = CommerceLinkLogger.LogFatal(CommerceLinkLoggerMessages.VSICL40002, currentStore, MethodBase.GetCurrentMethod().Name, "DqsRequest.Header");
                    return new DQSResponse(false, message, null);
                }
            }

            return null;
        }

        #endregion

    }
}
