using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace APIsWebInterfaces.Controllers
{
    public class WishListController : Controller
    {
        private string REAL_TIME_SERVICE_URL = ConfigurationManager.AppSettings["RealTimeServiceURL"].ToString();
        public ActionResult WishList()
        {
            return View();
        }
        public ActionResult WishListActions(string command, string itemId, string variantid)
        {
            return RedirectToAction("WishList");
        }
    }
}
