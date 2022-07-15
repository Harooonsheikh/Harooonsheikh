using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace APIsWebInterfaces.Controllers
{
    public class InventoryController : Controller
    {
        private string REAL_TIME_SERVICE_URL = ConfigurationManager.AppSettings["RealTimeServiceURL"].ToString();
        //
        // GET: /Inventory/

        public ActionResult Inventory()
        {
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"];

            if (TempData["Result"] != null)
                ViewBag.Result = TempData["Result"];
            return View();
        }

        public ActionResult InventoryLookupByBarcode(string command, string barcodes)
        {
            if (command == "Get Inventory By Barcodes")
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                        byte[] data = client.DownloadData(REAL_TIME_SERVICE_URL + "api/v1/Inventory/GetATPInventory?barcodes=" + barcodes);

                        var responseText = System.Text.Encoding.ASCII.GetString(data);

                        var result = JsonConvert.DeserializeObject(responseText);

                        //string isSuccess = result["succeeded"];
                        //if (isSuccess.Equals("false"))
                        //{
                        //    TempData["Message"] = result["errorMessage"];
                        //}
                        //else
                        //{
                            TempData["Result"] = result;
                        //}
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
            return RedirectToAction("Inventory");
        }

        public ActionResult InventoryLookupByItem(string command, string itemId, string variantId)
        {
            if (command == "Get Inventory")
            {
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                        byte[] data = client.DownloadData(REAL_TIME_SERVICE_URL + "api/store/InventoryLookup?itemId=" + itemId + "&variantId=" + variantId);

                        var responseText = System.Text.Encoding.ASCII.GetString(data);

                        var result = JsonConvert.DeserializeObject(responseText);

                        //string isSuccess = result["succeeded"];
                        //if (isSuccess.Equals("false"))
                        //{
                        //    TempData["Message"] = result["errorMessage"];
                        //}
                        //else
                        //{
                        TempData["Result"] = result;
                        //}
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
            return RedirectToAction("Inventory");
        }

    }
}
