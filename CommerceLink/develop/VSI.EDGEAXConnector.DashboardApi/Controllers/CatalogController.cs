using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using VSI.EDGEAXConnector.Configuration;
using VSI.EDGEAXConnector.DashboardApi.Common;
using VSI.EDGEAXConnector.Data;
using VSI.EDGEAXConnector.MongoData.Enums;
using VSI.EDGEAXConnector.MongoData.Helpers;

namespace VSI.EDGEAXConnector.DashboardApi.Controllers
{
    //[DashboardActionFilter]
    public class CatalogController : ApiBaseController
    {
        /// <summary>
        /// Constructor
        /// </summary>

        public CatalogController()
        {
            ControllerName = "CatalogController";
        }
        public string MongoDBName = string.Empty;
        public string MongoDBConn = string.Empty;

        [HttpGet]
        //[Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("GetProducts is deprecated, please use GetProducts with POST parameter instead.")]
        public async Task<IHttpActionResult> GetProducts(string fileName, int? offSet, int? pageSize, string filter = "")

        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(fileName, CatalogModel.Product, offSet, pageSize, filter));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        //[Authorize(Roles = "User, Admin, SuperAdmin")]
        public async Task<IHttpActionResult> GetProducts([FromBody] GetProductsRequest ProductsRequest)

        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(ProductsRequest.FileName, CatalogModel.Product, ProductsRequest.OffSet, ProductsRequest.PageSize, ProductsRequest.Filter));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        [Obsolete("GetUpdatedFileNames is deprecated, please use GetUpdatedFileNames with POST parameter instead.")]
        public async Task<IHttpActionResult> GetUpdatedFileNames(CatalogModel type)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(type, this.StoreName));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Obsolete("GetUpdatedFileNames is deprecated, please use GetUpdatedFileNames with POST parameter instead.")]
        public async Task<IHttpActionResult> GetUpdatedFileNames([FromBody] GetUpdatedFileNamesRequest UpdatedFileNamesRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(UpdatedFileNamesRequest.Type, this.StoreName));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpGet]
        //[Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("GetMasterProducts is deprecated, please use GetMasterProducts with POST parameter instead.")]
        public async Task<IHttpActionResult> GetMasterProducts(string fileName)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadMasterProducts(fileName));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }
        [HttpPost]
        //[Authorize(Roles = "User, Admin, SuperAdmin")]
        public async Task<IHttpActionResult> GetMasterProducts([FromBody] GetMasterProductsRequest MasterProductsRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadMasterProducts(MasterProductsRequest.FileName));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }

        [HttpGet]
        //[Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("GetProduct is deprecated, please use GetProduct with POST parameter instead.")]
        public IHttpActionResult GetProduct(string fileName, string prodId)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadProduct(fileName, prodId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }
        [HttpPost]
        //[Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetProduct([FromBody] GetProductRequest ProductRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadProduct(ProductRequest.FileName, ProductRequest.ProdId));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }
        [HttpGet]
        //[Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("Search is deprecated, please use Search with POST parameter instead.")]
        public IHttpActionResult Search(string fileName, string query, CatalogModel model, int? offSet, int? pageSize)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                if (model == CatalogModel.Product)
                {
                    return Ok(helper.SearchProduct(fileName, query, offSet, pageSize));
                }
                if (model == CatalogModel.Category)
                {
                    return Ok(helper.SearchCategory(fileName, query, offSet, pageSize));
                }
                if (model == CatalogModel.CategoryAssignment)
                {
                    return Ok(helper.SearchCatAssignment(fileName, query, offSet, pageSize));
                }
                return Ok();

            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }
        [HttpPost]
        //[Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult Search([FromBody] GetSearchRequest SearchRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                if (SearchRequest.Model == CatalogModel.Product)
                {
                    return Ok(helper.SearchProduct(SearchRequest.FileName, SearchRequest.Query, SearchRequest.Offset, SearchRequest.Pagesize));
                }
                if (SearchRequest.Model == CatalogModel.Category)
                {
                    return Ok(helper.SearchCategory(SearchRequest.FileName, SearchRequest.Query, SearchRequest.Offset, SearchRequest.Pagesize));
                }
                if (SearchRequest.Model == CatalogModel.CategoryAssignment)
                {
                    return Ok(helper.SearchCatAssignment(SearchRequest.FileName, SearchRequest.Query, SearchRequest.Offset, SearchRequest.Pagesize));
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
        [Obsolete("SearchResultCount is deprecated, please use SearchResultCount with POST parameter instead.")]
        public IHttpActionResult SearchResultCount(string fileName, string query, CatalogModel model)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                if (model == CatalogModel.Product)
                {
                    return Ok(helper.SearchProductCount(fileName, query));
                }
                if (model == CatalogModel.Category)
                {
                    return Ok(helper.SearchCategoryCount(fileName, query));
                }
                if (model == CatalogModel.CategoryAssignment)
                {
                    return Ok(helper.SearchCatAssignmentCount(fileName, query));
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
        public IHttpActionResult SearchResultCount([FromBody] GetSearchResultCountRequest SearchResultCountRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                if (SearchResultCountRequest.Model == CatalogModel.Product)
                {
                    return Ok(helper.SearchProductCount(SearchResultCountRequest.FileName, SearchResultCountRequest.Query));
                }
                if (SearchResultCountRequest.Model == CatalogModel.Category)
                {
                    return Ok(helper.SearchCategoryCount(SearchResultCountRequest.FileName, SearchResultCountRequest.Query));
                }
                if (SearchResultCountRequest.Model == CatalogModel.CategoryAssignment)
                {
                    return Ok(helper.SearchCatAssignmentCount(SearchResultCountRequest.FileName, SearchResultCountRequest.Query));
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
        [Obsolete("CatalogCount is deprecated, please use CatalogCount with POST parameter instead.")]
        public IHttpActionResult GetCatalogCount(string fileName, CatalogModel modal)
        {
            try
            {
                return Ok(GetCount(fileName, modal));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }
        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public IHttpActionResult GetCatalogCount([FromBody] GetCatalogCountRequest CatalogCountRequest)
        {
            try
            {
                return Ok(GetCount(CatalogCountRequest.FileName, CatalogCountRequest.Modal));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);

            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("Categories is deprecated, please use Categories with POST parameter instead.")]
        public async Task<IHttpActionResult> Categories(string fileName, int? offSet, int? pageSize)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(fileName, CatalogModel.Category, offSet, pageSize));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public async Task<IHttpActionResult> Categories([FromBody] GetCategoriesRequest CategoriesRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(CategoriesRequest.FileName, CatalogModel.Category, CategoriesRequest.Offset, CategoriesRequest.Pagesize));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("CategoriesAssignments is deprecated, please use CategoriesAssignments with POST parameter instead.")]
        public async Task<IHttpActionResult> CategoriesAssignments(string fileName, int? offSet, int? pageSize)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(fileName, CatalogModel.CategoryAssignment, offSet, pageSize));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public async Task<IHttpActionResult> CategoriesAssignments([FromBody] GetCategoriesAssignmentsRequest CategoriesAssignmentsRequest)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return Ok(helper.ReadNodes(CategoriesAssignmentsRequest.FileName, CatalogModel.CategoryAssignment, CategoriesAssignmentsRequest.Offset, CategoriesAssignmentsRequest.Pagesize));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        [Obsolete("CatalogStatisticsByName is deprecated, please use CatalogStatisticsByName with POST parameter instead.")]
        public async Task<IHttpActionResult> CatalogStatisticsByName(string fileName)
        {
            try
            {
                List<KeyValuePair<string, string>> listViewModelLine = new List<KeyValuePair<string, string>>();

                listViewModelLine.Add(new KeyValuePair<string, string>("Total Products", GetCount(fileName, CatalogModel.Product).ToString()));
                listViewModelLine.Add(new KeyValuePair<string, string>("Categories", GetCount(fileName, CatalogModel.Category).ToString()));
                listViewModelLine.Add(new KeyValuePair<string, string>("Assignments", GetCount(fileName, CatalogModel.CategoryAssignment).ToString()));

                return Ok(listViewModelLine);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Authorize(Roles = "User, Admin, SuperAdmin")]
        public async Task<IHttpActionResult> CatalogStatisticsByName([FromBody] GetCatalogStatisticsByNameRequest CatalogStatisticsByNameRequest)
        {
            try
            {
                List<KeyValuePair<string, string>> listViewModelLine = new List<KeyValuePair<string, string>>();

                listViewModelLine.Add(new KeyValuePair<string, string>("Total Products", GetCount(CatalogStatisticsByNameRequest.FileName, CatalogModel.Product).ToString()));
                listViewModelLine.Add(new KeyValuePair<string, string>("Categories", GetCount(CatalogStatisticsByNameRequest.FileName, CatalogModel.Category).ToString()));
                listViewModelLine.Add(new KeyValuePair<string, string>("Assignments", GetCount(CatalogStatisticsByNameRequest.FileName, CatalogModel.CategoryAssignment).ToString()));

                return Ok(listViewModelLine);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        private long GetCount(string fileName, CatalogModel model)
        {
            try
            {
                MongoHelper.SetMongoDBCreds(this.StoreKey, ref this.MongoDBConn, ref this.MongoDBName);
                CatalogMongoHelper helper = new CatalogMongoHelper(MongoDBConn, MongoDBName);
                return helper.ReadModelCount(fileName, model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region Catalog Controller, Response classes
        public class GetProductsRequest
        {
            public string FileName { get; set; }
            public int OffSet { get; set; }
            public int PageSize { get; set; }
            public string Filter { get; set; }

        }
        public class GetMasterProductsRequest
        {
            public string FileName { get; set; }

        }
        public class GetUpdatedFileNamesRequest
        {
            public CatalogModel Type { get; set; }

        }//
        public class GetProductRequest
        {
            public string FileName { get; set; }
            public string ProdId { get; set; }

        }
        public class GetCatalogCountRequest
        {
            public string FileName { get; set; }
            public CatalogModel Modal { get; set; }

        }
        public class GetSearchRequest
        {
            public string FileName { get; set; }
            public string Query { get; set; }
            public CatalogModel Model { get; set; }
            public int? Offset { get; set; }
            public int? Pagesize { get; set; }

        }
        public class GetSearchResultCountRequest
        {
            public string FileName { get; set; }
            public string Query { get; set; }
            public CatalogModel Model { get; set; }

        }
        public class GetCategoriesRequest
        {
            public string FileName { get; set; }
            public int? Offset { get; set; }
            public int? Pagesize { get; set; }

        }
        public class GetCategoriesAssignmentsRequest
        {
            public string FileName { get; set; }
            public int? Offset { get; set; }
            public int? Pagesize { get; set; }

        }
        public class GetCatalogStatisticsByNameRequest
        {
            public string FileName { get; set; }

        }
        #endregion
    }
}
