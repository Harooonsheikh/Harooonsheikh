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
    public class StoreController : Controller
    {
        private string REAL_TIME_SERVICE_URL = ConfigurationManager.AppSettings["RealTimeServiceURL"].ToString();
        public ActionResult Store()
        {
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"];

            if (TempData["balance"] != null)
                ViewBag.balance = TempData["balance"];

            if (TempData["Result"] != null)
                ViewBag.Result = TempData["Result"];
            return View();
        }


        public ActionResult StoreActions(string command, string itemId, string variantid)
        {
            string xAPIKey = System.Configuration.ConfigurationManager.AppSettings["X-API-KEY"];

            if (command == "Get Inventory Lookup")
            {
                string baseUri = REAL_TIME_SERVICE_URL + "api/v1/Store/GetStoreAvailability?itemId=" + itemId + "&variantId=" + variantid;

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                        client.Headers.Add("Content-Type", "application/json");
                        client.Headers.Add("x-api-key", xAPIKey);
                        byte[] data = client.DownloadData(baseUri);

                        var responseText = System.Text.Encoding.ASCII.GetString(data);

                        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseText);
                        var inventoryResult = result["status"];
                        var inventoryData = result["inventoryLookupData"];

                        TempData["Result"] = inventoryData;
                    }
                    catch (WebException ex)
                    {
                        // Http Error
                        if (ex.Status == WebExceptionStatus.ProtocolError)
                        {
                            HttpWebResponse wrsp = (HttpWebResponse)ex.Response;
                            var statusCode = (int)wrsp.StatusCode;
                            var msg = wrsp.StatusDescription;

                            throw new HttpException(statusCode, msg);
                        }
                        else
                        {
                            //exception
                            throw new HttpException(500, ex.Message);
                        }
                    }
                }
            }

            return RedirectToAction("Store");
        }
    }

    class InventoryStatus
    {
        public string AvaialableInventory { get; set; }
        public string InventoryLocation { get; set; }
        public string ItemId { get; set; }
        public string StoreName { get; set; }
    }
}