using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.CommonDataService;
using Microsoft.CommonDataService.CommonEntitySets;
using Microsoft.CommonDataService.Configuration;
using Microsoft.CommonDataService.ServiceClient.Security;
using Swashbuckle.Swagger.Annotations;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace VSI.EDGEAXConnector.Web.Controllers
{

    // [RequireHttps]
    // [System.Web.Http.RoutePrefix("api/v1")]
    public class CDSLeadController : ApiBaseController
    {

        //[SwaggerResponse(HttpStatusCode.BadRequest, Description = "Invalid parameters.")]
        //[SwaggerResponse(HttpStatusCode.Unauthorized, Description = "401: Unauthorized")]
        //[SwaggerResponse(HttpStatusCode.Forbidden, Description = "403: Forbidden")]
        //[SwaggerResponse(HttpStatusCode.MethodNotAllowed, Description = "405: Method Not Allowed")]
        //[SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Description = "415: Unsupported Media Type")]
        //[SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Exception occured at Dynamics AX Server.")]
        public async Task<OperationResult<IReadOnlyList<Lead>>> Get()
        {

            try
            {

                OperationResult<IReadOnlyList<Lead>> result = null;
                using (var client = ConnectionSettings.Instance.CreateClient().Result)
                {
                    result = await Task.Run(() => SelectLeadsAsync(client));
                }

                return result;
            }
            catch (Exception ex) 
            {
                throw ex;
            }

        }

        // GET: api/CDSLead/5
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lead"></param>
        // POST: api/CDSLead
        [System.Web.Http.HttpPost]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Invalid parameters.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "401: Unauthorized")]
        [SwaggerResponse(HttpStatusCode.Forbidden, Description = "403: Forbidden")]
        [SwaggerResponse(HttpStatusCode.MethodNotAllowed, Description = "405: Method Not Allowed")]
        [SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Description = "415: Unsupported Media Type")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Exception occured at Dynamics AX Server.")]
        public async Task Post([FromBody]LeadRequest lead)
        {
            using (var client = ConnectionSettings.Instance.CreateClient().Result)
            {
                await Task.Run(() => InsertLeadsAsync(client, lead));
            }

        }

        // PUT: api/CDSLead/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CDSLead/5
        public void Delete(int id)
        {
        }

        #region Private Methods 

        static async Task<OperationResult<IReadOnlyList<Lead>>> SelectLeadsAsync(Client client)
        {
            var queryBuilder = client.GetRelationalEntitySet<Lead>()
                .CreateQueryBuilder();

            var query = queryBuilder
                //  .Where(pc => pc.Name == "Electronics")
                .OrderByAscending(pc => new object[] { pc.CreatedDateTime })
                .Project(pc => pc.SelectField(f => f.LeadId)
                    .SelectField(f => f.FullName)
                    .SelectField(f => f.Description));

            // Execute the query:
            OperationResult<IReadOnlyList<Lead>> queryResult = null;
            var executor = client.CreateRelationalBatchExecuter(
                RelationalBatchExecutionMode.Transactional)
                .Query(query, out queryResult);

            await executor.ExecuteAsync();

            return queryResult;
        }

        static async Task InsertLeadsAsync(Client client, LeadRequest lead)
        {
            // Insert New leads
            var newLead = new Lead();
            //  newLead = LeadRequest;

            newLead.Birthdate = new UtcDateTime(DateTime.Now);
            // newLead.Description = LeadRequest.Description;
            //newLead.FullName = LeadRequest.FullName;
            newLead.DUNSNumber = "ORG001";
            newLead.IsSecurityPrincipal = true;
            newLead.IsPhoneContactAllowed = true;
            newLead.IsEmailContactAllowed = true;
            newLead.Source = Source.Default;
            newLead.PartyType = PartyType.Person;
            newLead.OrganizationName = "ORG001";

            //{
            //    Birthdate = new UtcDateTime(DateTime.Now),
            //    Description = lead.Description,
            //    FullName = lead.FullName,
            //    DUNSNumber = "ORG001",
            //    Status = lead.Status,
            //    IsSecurityPrincipal = true,
            //    IsPhoneContactAllowed = true,
            //    IsEmailContactAllowed = true,
            //    Source = Source.Default,
            //    PartyType = PartyType.Person,
            //    OrganizationName = "ORG001"
            //};

            //++ Getting Executor 
            var executor = client.CreateRelationalBatchExecuter(
                     RelationalBatchExecutionMode.Transactional);

            executor.Insert(newLead);
            await executor.ExecuteAsync();
        }

        #endregion

        /// <summary>
        /// LeadRequest object 
        /// </summary>
        public class LeadRequest
        {
            /// <summary>
            /// For Graph
            /// </summary>
            public string OfficeGraphIdentifier { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public UtcDateTime Birthdate { get; set; }
            public Gender Gender { get; set; }
            public IndustryCode IndustryCode { get; set; }
            public string TaxIdentificationIssuer { get; set; }
            public string TaxIdentificationNumber { get; set; }
            public string DUNSNumber { get; set; }
            public bool IsEmailContactAllowed { get; set; }
            public bool IsPhoneContactAllowed { get; set; }
            public string Generation { get; set; }
            public string Profession { get; set; }
            public string LeadId { get; set; }
            public string Description { get; set; }
            public LeadStatus Status { get; set; }
            public Address MailingPostalAddress { get; set; }
            public Address ShippingPostalAddress { get; set; }
            public Source Source { get; set; }
            public string WebsiteURL { get; set; }
            public StockExchange StockExchange { get; set; }
            public string StockTicker { get; set; }
            public PartyType PartyType { get; set; }
            public string OrganizationName { get; set; }
            public PersonName PersonName { get; set; }
            public string FullName { get; set; }
            public string EmailPrimary { get; set; }
            public string EmailAlternate { get; set; }
            public string PhonePrimary { get; set; }
            public string Phone01 { get; set; }
            public Address OtherPostalAddress { get; set; }
            public string Phone02 { get; set; }
            public string FacebookIdentity { get; set; }
            public string LinkedInIdentity { get; set; }
            public string TwitterIdentity { get; set; }
            public SocialNetwork SocialNetwork01 { get; set; }
            public SocialNetwork SocialNetwork02 { get; set; }
            public string SocialNetworkIdentity01 { get; set; }
            public string SocialNetworkIdentity02 { get; set; }
            public string SatoriId { get; set; }
            public string Phone03 { get; set; }

        }
    }
}
