using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VSI.EDGEAXConnector.Common.Enums;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.MongoData.Enums;
using VSI.EDGEAXConnector.MongoData.Helpers;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    [RoutePrefix("api/SalesOrder")]
    public class SalesOrderController : ApiBaseController
    {
        public string MongoDBName = string.Empty;
        public string MongoDBConn = string.Empty;

        public SalesOrderController()
        {
            ControllerName = "CatalogController";
        }

        [HttpPost]
        [Route("Save")]
        public IHttpActionResult Save(OrderModel model)
        {
            MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
            SaleOrderMongoHelper helper = new SaleOrderMongoHelper(MongoDBConn, MongoDBName);

            var response = helper.SaveOrder(model);

            return Ok(true);
        }

        [HttpGet]
        [Route("GetById")]
        [Obsolete("GetById is deprecated, please use GetById with POST parameter instead.")]

        public IHttpActionResult GetById(string id)
        {
            MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
            SaleOrderMongoHelper helper = new SaleOrderMongoHelper(MongoDBConn, MongoDBName);

            return Ok(helper.GetById(id));
        }

        [HttpPost]
        [Route("GetById")]
        public IHttpActionResult GetById([FromBody] GetGetByIdRequest GetByIdRequest)
        {
            MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
            SaleOrderMongoHelper helper = new SaleOrderMongoHelper(MongoDBConn, MongoDBName);

            return Ok(helper.GetById(GetByIdRequest.Id));
        }

        [HttpGet]
        [Route("GetAll")]
        [Obsolete("GetIntegrationKeys is deprecated, please use GetIntegrationKeys with POST parameter instead.")]
        public IHttpActionResult GetAll(string userId, int type = 1, int pageSize = 10, int pageNumber = 0)
        {
            MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
            SaleOrderMongoHelper helper = new SaleOrderMongoHelper(MongoDBConn, MongoDBName);

            var orders = helper.GetAll(userId, type, pageSize, pageNumber).Select(o => new {
                o.Id,
                o.CustomerId,
                o.CustomerName,
                CreatedOn = o.CreatedOn.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture),
                Type = o.Type == 1 ? OrderType.SalesOrder : OrderType.Quote,
                o.UserId
            });

            return Ok(orders);
        }

        [HttpPost]
        [Route("GetAll")]
        public IHttpActionResult GetAll([FromBody] GetAllRequest AllRequest)
        {
            MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
            SaleOrderMongoHelper helper = new SaleOrderMongoHelper(MongoDBConn, MongoDBName);

            var orders = helper.GetAll(AllRequest.UserId, AllRequest.Type, AllRequest.PageSize, AllRequest.PageNumber).Select(o => new {
                o.Id,
                o.CustomerId,
                o.CustomerName,
                CreatedOn = o.CreatedOn.ToString("MM/dd/yyyy HH:mm", CultureInfo.InvariantCulture),
                Type = o.Type == 1 ? OrderType.SalesOrder : OrderType.Quote,
                o.UserId
            });

            return Ok(orders);
        }

        #region Sale Order Controller, Response classes

        public class GetGetByIdRequest
        {

            public string Id { get; set; }

        }
        public class GetAllRequest
        {

            public string UserId { get; set; }
            public int Type { get; set; }
            public int PageSize { get; set; }
            public int PageNumber { get; set; }


        }

        #endregion
    }

}
