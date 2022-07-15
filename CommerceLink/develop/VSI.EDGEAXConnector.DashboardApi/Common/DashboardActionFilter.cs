using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using VSI.EDGEAXConnector.DashboardApi.Infrastructure;

namespace VSI.EDGEAXConnector.DashboardApi.Common
{
    public class DashboardActionFilter : ActionFilterAttribute
    {
        private readonly string STOREIDHEADER_KEY = "storeKey";
        private readonly string LOCATIONHEADER_KEY = "location";

        public DashboardActionFilter()
        {
        }
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var header = actionContext.Request.Headers;
            var storeId = header.Contains(STOREIDHEADER_KEY);
            if (!storeId)
                throw new Exception("Store Key Invalid / Missing.");

            //var location = header.Contains(LOCATIONHEADER_KEY);
            //if (!location)
            //    throw new Exception("Location Not Available.");


            //actionContext.ControllerContext.RequestContext.RouteData.Values[STOREIDHEADER_KEY] = header.GetValues(STOREIDHEADER_KEY).First();
            //actionContext.ControllerContext.RequestContext.RouteData.Values[LOCATIONHEADER_KEY] = header.GetValues(LOCATIONHEADER_KEY).First();

            //var requestParams = actionContext.ActionArguments["store"]; //actionContext.ActionDescriptor.GetParameters();
            //if (requestParams != null)
            //{
            //    var storeName = requestParams.ToString();
            //    if (storeName == null)
            //    {
            //        throw new Exception("Store not defined.");
            //    }
            //    if (storeName != null)
            //    {
            //        RequestSharedParams.StoreName = "";
            //        using (var context = new ApplicationDbContext())
            //        {
            //            var store = context.Stores.Where(m => m.Name.Equals(storeName)).FirstOrDefault();
            //            if (store == null)
            //            {
            //                throw new Exception("Store Not Found.");
            //            }
            //            if (!store.IsActive)
            //            {
            //                throw new Exception("Store Currently Disabled.");
            //            }
            //            RequestSharedParams.StoreConnectionString = "metadata = res://*/IntegrationModel.csdl|res://*/IntegrationModel.ssdl|res://*/IntegrationModel.msl;provider=System.Data.SqlClient;provider connection string='Data Source=" + store.DBServer + "; Initial Catalog=" + store.DBName + ";integrated security=True;MultipleActiveResultSets=True;App=EntityFramework'";
            //            RequestSharedParams.MongoConnection = store.MongoConnection;
            //            RequestSharedParams.MongoName = store.MongoDbName;
            //            RequestSharedParams.ConnStrForExceptionLogging = "data source=" + store.DBServer + ";initial catalog=" + store.DBName + ";integrated security=true;";

            //        }
            //    }
            //}

            base.OnActionExecuting(actionContext);
        }
    }
}