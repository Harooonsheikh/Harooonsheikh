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
    public class ShippingController : Controller
    {
        private string REAL_TIME_SERVICE_URL = ConfigurationManager.AppSettings["RealTimeServiceURL"].ToString();

        public ActionResult Shipping()
        {
            return View();
        }

        public ActionResult ShippingActions(string command, string shippingmethod, string order, string postalcode, string countrycode, string address1, string address2)
        {
            string xAPIKey = System.Configuration.ConfigurationManager.AppSettings["X-API-KEY"];

            if (command == "Get Shipping Charges")
            {
                string baseUri = REAL_TIME_SERVICE_URL + "/api/v1/Shipping/EstimateShippingCharges";

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                        client.Headers.Add("Content-Type", "application/json");
                        client.Headers.Add("x-api-key", xAPIKey);

                        ShippingChargesRequest shippingChargesRequest = new ShippingChargesRequest();

                        shippingChargesRequest.shippingMethod = shippingmethod;
                        shippingChargesRequest.order = order;
                        shippingChargesRequest.postalCode = postalcode;
                        shippingChargesRequest.countryCode = countrycode;
                        shippingChargesRequest.address1 = address1;
                        shippingChargesRequest.address2 = address2;

                        string requestString = JsonConvert.SerializeObject(shippingChargesRequest);

                        string data = client.UploadString(baseUri, requestString);

                        var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
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

            return RedirectToAction("Shipping");
        }
    }

    class ShippingChargesRequest
    {
        public string shippingMethod { get; set; }
        public string order { get; set; }
        public string postalCode { get; set; }
        public string countryCode { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
    }
}
