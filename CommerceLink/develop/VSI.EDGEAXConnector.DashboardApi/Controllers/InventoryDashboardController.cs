using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DAL;
using VSI.EDGEAXConnector.MongoData.Helpers;
using VSI.EDGEAXConnector.MongoData.Enums;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.Configuration;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    [DashboardActionFilter]
    [RoutePrefix("api/Inventory")]
    public class InventoryController : ApiBaseController
    {

        /// <summary>
        /// Constructor
        /// </summary>

        public InventoryController()
        {
            ControllerName = "InventoryDashboardController";
        }

        public string MongoDBName = string.Empty;
        public string MongoDBConn = string.Empty;

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("StatisticsByName")]
        [Obsolete("InventoryStatisticsByName is deprecated, please use InventoryStatisticsByName with POST parameter instead.")]
        public async Task<IHttpActionResult> InventoryStatisticsByName(string fileName)
        {
            try
            {
                List<KeyValuePair<string, string>> listViewModelLine = new List<KeyValuePair<string, string>>();

                listViewModelLine.Add(new KeyValuePair<string, string>("Total Products", FileRecordCount(fileName).ToString()));

                return Ok(listViewModelLine);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("StatisticsByName")]
        public async Task<IHttpActionResult> InventoryStatisticsByName([FromBody] GetInventoryStatisticsByNameRequest InventoryStatisticsByNameRequest)
        {
            {
                try
                {
                    List<KeyValuePair<string, string>> listViewModelLine = new List<KeyValuePair<string, string>>();

                    listViewModelLine.Add(new KeyValuePair<string, string>("Total Products", FileRecordCount(InventoryStatisticsByNameRequest.FileName).ToString()));

                    return Ok(listViewModelLine);
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Header")]
        [Obsolete("Header is deprecated, please use Header with POST parameter instead.")]
        public async Task<IHttpActionResult> Header(string fileName, int? offSet = null, int? pageSize = null)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(fileName, InventoryModel.Header, offSet, pageSize));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Header")]
        public async Task<IHttpActionResult> Header([FromBody] GetHeaderRequest HeaderRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(HeaderRequest.FileName, InventoryModel.Header, HeaderRequest.Offset, HeaderRequest.Pagesize));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("HeaderCount")]
        [Obsolete("HeaderCount is deprecated, please use HeaderCount with POST parameter instead.")]
        public IHttpActionResult HeaderCount(string fileName)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadModelCount(fileName, InventoryModel.Header));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }
        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("HeaderCount")]
        public IHttpActionResult HeaderCount([FromBody] GetHeaderCountRequest HeaderCountRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadModelCount(HeaderCountRequest.FileName, InventoryModel.Header));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("HeaderById")]
        [Obsolete("Header is deprecated, please use Header with POST parameter instead.")]
        public IHttpActionResult HeaderById(string fileName, string headerId)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadHeader(fileName, headerId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }
        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("HeaderById")]
        public IHttpActionResult HeaderById([FromBody] GetHeaderByIdRequest HeaderByIdRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadHeader(HeaderByIdRequest.FileName, HeaderByIdRequest.HeaderId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Search")]
        [Obsolete("SearchRecord is deprecated, please use SearchRecord with POST parameter instead.")]
        public IHttpActionResult SearchRecord(string fileName, string query, InventoryModel model, int? offSet = null, int? pageSize = null)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
                if (model == InventoryModel.Record)
                {
                    return Ok(helper.SearchRecord(fileName, query, offSet, pageSize));
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Search")]
        public IHttpActionResult SearchRecord([FromBody] GetSearchRecordRequest SearchRecordRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
                if (SearchRecordRequest.Model == InventoryModel.Record)
                {
                    return Ok(helper.SearchRecord(SearchRecordRequest.FileName, SearchRecordRequest.Query, SearchRecordRequest.Offset, SearchRecordRequest.Pagesize));
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("SearchCount")]
        [Obsolete("SearchRecordCount is deprecated, please use SearchRecordCount with POST parameter instead.")]
        public IHttpActionResult SearchRecordCount(string fileName, string query, InventoryModel model)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
                if (model == InventoryModel.Record)
                {
                    return Ok(helper.SearchRecordCount(fileName, query));
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }
        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("SearchCount")]
        public IHttpActionResult SearchRecordCount([FromBody] GetSearchRecordCountRequest SearchRecordCountRequest)
        {
            {
                try
                {
                    MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                    InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
                    if (SearchRecordCountRequest.Model == InventoryModel.Record)
                    {
                        return Ok(helper.SearchRecordCount(SearchRecordCountRequest.FileName, SearchRecordCountRequest.Query));
                    }
                    return Ok();

                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);

                }
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Get")]
        [Obsolete("Records is deprecated, please use Records with POST parameter instead.")]
        public IHttpActionResult Records(string fileName, int? offSet, int? pageSize)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(fileName, InventoryModel.Record, offSet, pageSize));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Get")]
        public IHttpActionResult Records([FromBody] GetRecordsRequest RecordsRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(RecordsRequest.FileName, InventoryModel.Record, RecordsRequest.Offset, RecordsRequest.Pagesize));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("GetCount")]
        [Obsolete("RecordCount is deprecated, please use RecordCount with POST parameter instead.")]
        public IHttpActionResult RecordCount(string fileName)
        {
            try
            {
                return Ok(FileRecordCount(fileName));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("GetCount")]
        public IHttpActionResult RecordCount([FromBody] GetRecordCountRequest RecordCountRequest)
        {
            try
            {
                return Ok(FileRecordCount(RecordCountRequest.FileName));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        private long FileRecordCount(string fileName)
        {
            MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
            InventoryMongoHelper helper = new InventoryMongoHelper(MongoDBConn, MongoDBName);
            return helper.ReadModelCount(fileName, InventoryModel.Record);
        }
        #region Inventory Dashboard Controller, Response classes

        public class GetInventoryStatisticsByNameRequest
        {

            public string FileName { get; set; }

        }
        public class GetHeaderRequest
        {

            public string FileName { get; set; }
            public int? Offset { get; set; }
            public int? Pagesize { get; set; }

        }
        public class GetHeaderCountRequest
        {

            public string FileName { get; set; }

        }
        public class GetHeaderByIdRequest
        {

            public string FileName { get; set; }
            public string HeaderId { get; set; }

        }
        public class GetSearchRecordRequest
        {

            public string FileName { get; set; }
            public InventoryModel Model { get; set; }
            public string Query { get; set; }
            public int? Offset { get; set; }
            public int? Pagesize { get; set; }

        }
        public class GetSearchRecordCountRequest
        {

            public string FileName { get; set; }
            public string Query { get; set; }
            public InventoryModel Model { get; set; }

        }
        public class GetRecordsRequest
        {
            public string FileName { get; set; }
            public int? Offset { get; set; }
            public int? Pagesize { get; set; }

        }
        public class GetRecordCountRequest
        {
            public string FileName { get; set; }

        }
        #endregion
    }
}