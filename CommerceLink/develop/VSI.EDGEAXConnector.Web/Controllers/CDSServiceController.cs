using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.CommonDataService;
using Microsoft.CommonDataService.CommonEntitySets;
using Microsoft.CommonDataService.Configuration;
using Microsoft.CommonDataService.ServiceClient.Security;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [RoutePrefix("api/v1")]
    public class CDSServiceController : ApiBaseController
    {
        // GET: CDSService
        [Route("CDSService/GetAllProductCategory")]
        [SwaggerResponse(HttpStatusCode.BadRequest, Description = "Invalid parameters.")]
        [SwaggerResponse(HttpStatusCode.Unauthorized, Description = "401: Unauthorized")]
        [SwaggerResponse(HttpStatusCode.Forbidden, Description = "403: Forbidden")]
        [SwaggerResponse(HttpStatusCode.MethodNotAllowed, Description = "405: Method Not Allowed")]
        [SwaggerResponse(HttpStatusCode.UnsupportedMediaType, Description = "415: Unsupported Media Type")]
        [SwaggerResponse(HttpStatusCode.InternalServerError, Description = "Exception occured at Dynamics AX Server.")]
        public void GetAllProductCategory()
        {

            using (var client = ConnectionSettings.Instance.CreateClient().Result)
            {
                Task.Run(async () => await InsertAsync(client));
            }

            var abc = "i am out";
        }


        private static async Task InsertAsync(Client client)
        {
            // Insert Surface and Phone product lines
            var surfaceCategory = new ProductCategory()
            {
                Name = "Tablet",
                Description = "Tablet product line"
            };
            var phoneCategory = new ProductCategory()
            {
                Name = "Phone",
                Description = "Phone product line"
            };

            var executor = client.CreateRelationalBatchExecuter(
                    RelationalBatchExecutionMode.Transactional);

            executor
                .Insert(surfaceCategory)
                .Insert(phoneCategory);

            await executor.ExecuteAsync();
        }


        static async Task SimpleSelectAsync(Client client)
        {
            var queryBuilder = client.GetRelationalEntitySet<ProductCategory>()
                .CreateQueryBuilder();

            var query = queryBuilder
                .Where(pc => pc.Name == "Electronics")
                .OrderByAscending(pc => new object[] { pc.CategoryId })
                .Project(pc => pc.SelectField(f => f.CategoryId)
                    .SelectField(f => f.Name)
                    .SelectField(f => f.Description));

            // Execute the query:
            OperationResult<IReadOnlyList<ProductCategory>> queryResult = null;
            var executor = client.CreateRelationalBatchExecuter(
                RelationalBatchExecutionMode.Transactional)
                .Query(query, out queryResult);

            await executor.ExecuteAsync();

            string result = string.Empty;

            foreach (var pc in queryResult.Result)
            {
                result = pc.Name;
            }

        }
    }
}