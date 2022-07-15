using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    public class HomeController : ApiController
    {
        /// <summary>
        /// Default action to run on application startup
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Index()
        {
            return Ok("Success");
        }
    }
}
