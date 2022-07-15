using System;
using System.Web.Http;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    public class ProductController : ApiBaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_erpAdapterFactory"></param>
        public ProductController()
        {
            ControllerName = "ProductController";
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetCatalogProducts(string fileName, int startIndex, int endIndex)
        {
            try
            {
                //ProductStatisticsDAL productStatistic = new ProductStatisticsDAL();
                //List<Product> list = productStatistic.GetProductStatistics(fileName, startIndex, endIndex);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}