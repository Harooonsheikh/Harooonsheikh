using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.Data.DAL;
using VSI.EDGEAXConnector.MongoData.Enums;
using VSI.EDGEAXConnector.MongoData.Helpers;

namespace VSI.EDGEAXConnector.DashboardApi
{
    [DashboardActionFilter]
    [RoutePrefix("api/Price")]
    public class PriceController : ApiBaseController
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public PriceController()
        {
            ControllerName = "PriceController";
        }
        public string MongoDBName = string.Empty;
        public string MongoDBConn = string.Empty;

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("StatisticsByName")]
        public async Task<IHttpActionResult> PriceStatisticsByName(string fileName)
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


        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("Header")]
        [Obsolete("Header is deprecated, please use Header with POST parameter instead.")]
        public async Task<IHttpActionResult> Header(string fileName, int? offSet, int? pageSize)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(fileName, PriceModel.Header, offSet, pageSize));
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
                PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(HeaderRequest.FileName, PriceModel.Header, HeaderRequest.Offset, HeaderRequest.Pagesize));
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
                PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadModelCount(fileName, PriceModel.Header));
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
                PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadModelCount(HeaderCountRequest.FileName, PriceModel.Header));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("HeaderById")]
        [Obsolete("HeaderById is deprecated, please use HeaderById with POST parameter instead.")]

        public IHttpActionResult HeaderById(string fileName, string headerId)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
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
                PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadHeader(HeaderByIdRequest.FileName, HeaderByIdRequest.HeaderId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }


        [HttpGet]
        [Route("Search")]
        [Obsolete("SearchRecord is deprecated, please use SearchRecord with POST parameter instead.")]
        public IHttpActionResult SearchRecord(string fileName, string query, PriceModel model, int? offSet = null, int? pageSize = null)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
                if (model == PriceModel.PriceTable)
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
        [Route("Search")]
        public IHttpActionResult SearchRecord([FromBody] GetSearchRecordRequest SearchRecordRequest)
        {
            {
                try
                {
                    MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                    PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
                    if (SearchRecordRequest.Model == PriceModel.PriceTable)
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
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Route("SearchCount")]
        [Obsolete("SearchRecordCount is deprecated, please use SearchRecordCount with POST parameter instead.")]
        public IHttpActionResult SearchRecordCount(string fileName, string query, PriceModel model)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
                if (model == PriceModel.PriceTable)
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
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
                if (SearchRecordCountRequest.Model == PriceModel.PriceTable)
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

        [HttpGet]
        [Route("Get")]
        public IHttpActionResult Records(string fileName, int? offSet, int? pageSize)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(fileName, PriceModel.PriceTable, offSet, pageSize));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("Get")]
        [Obsolete("Records is deprecated, please use Records with POST parameter instead.")]
        public IHttpActionResult Records([FromBody] GetRecordsRequest RecordsRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(RecordsRequest.FileName, PriceModel.PriceTable, RecordsRequest.Offset, RecordsRequest.Pagesize));
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
        public IHttpActionResult RecordCount([FromBody] GetRecordCountRequest RecordCount)
        {
            try
            {
                return Ok(FileRecordCount(RecordCount.FileName));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        private long FileRecordCount(string fileName)
        {
            MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
            PriceMongoHelper helper = new PriceMongoHelper(MongoDBConn, MongoDBName);
            return helper.ReadModelCount(fileName, PriceModel.PriceTable);
        }

        #region Price Controller, Response classes

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
            public PriceModel Model { get; set; }
            public string Query { get; set; }
            public int? Pagesize { get; set; }
            public int? Offset { get; set; }

        }
        public class GetSearchRecordCountRequest
        {

            public string FileName { get; set; }
            public PriceModel Model { get; set; }
            public string Query { get; set; }

        }
        public class GetRecordsRequest
        {

            public string FileName { get; set; }
            public int? Pagesize { get; set; }
            public int? Offset { get; set; }

        }
        public class GetRecordCountRequest
        {
            public string FileName { get; set; }

        }
        #endregion
    }
}