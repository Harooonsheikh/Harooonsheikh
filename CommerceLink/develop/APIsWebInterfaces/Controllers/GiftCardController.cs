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
    public class GiftCardController : Controller
    {
        private string REAL_TIME_SERVICE_URL = ConfigurationManager.AppSettings["RealTimeServiceURL"].ToString();
        public ActionResult GiftCard()
        {
            if (TempData["Message"] != null)
                ViewBag.Message = TempData["Message"];

            if (TempData["balance"] != null)
                ViewBag.balance = TempData["balance"];
            return View();
        }

        public ActionResult GiftCardActions(string command, string amount, string depositcurrencycode, string giftcardid, string giftcardidtoprocess)
        {
            string xAPIKey = System.Configuration.ConfigurationManager.AppSettings["X-API-KEY"];

            if (command == "Get Gift Card Balance")
            {
                string baseUri = REAL_TIME_SERVICE_URL + "api/v1/giftcard/GetGiftCardBalance?giftCardId=" + giftcardid;

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                        client.Headers.Add("Content-Type", "application/json");
                        client.Headers.Add("x-api-key", xAPIKey);
                        byte[] data = client.DownloadData(baseUri);

                        var responseText = System.Text.Encoding.ASCII.GetString(data);

                        var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                        string isSuccess = result["status"];
                        if (isSuccess.ToUpper().Equals("TRUE"))
                        {
                            TempData["balance"] = result["balance"];
                        }
                        else
                        {
                            TempData["Message"] = "Unable to find the Gift Card " + giftcardid;
                        }
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
            else if (command == "Lock Gift Card")
            {
                string baseUri = REAL_TIME_SERVICE_URL + "api/v1/giftcard/LockGiftCard?giftCardId=" + giftcardid;

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                        client.Headers.Add("x-api-key", xAPIKey);
                        byte[] data = client.DownloadData(baseUri);

                        var responseText = System.Text.Encoding.ASCII.GetString(data).ToString();

                        var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                        string isSuccess = result["status"];
                        if (isSuccess.ToUpper().Equals("TRUE"))
                        {
                            TempData["Message"] = "Gift Card has been locked";
                        }
                        else
                        {
                            TempData["Message"] = "Unable to Lock the Gift Card " + giftcardid;
                        }
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
            else if (command == "Unlock Gift Card")
            {
                string baseUri = REAL_TIME_SERVICE_URL + "api/v1/giftcard/UnlockGiftCard?giftCardId=" + giftcardid;

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                        client.Headers.Add("x-api-key", xAPIKey);
                        byte[] data = client.DownloadData(baseUri);

                        var responseText = System.Text.Encoding.ASCII.GetString(data).ToString();

                        var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                        string isSuccess = result["status"];
                        if (isSuccess.ToUpper().Equals("TRUE"))
                        {
                            TempData["Message"] = "Gift Card has been Unlocked";
                        }
                        else
                        {
                            TempData["Message"] = "Unable to Unlock the Gift Card " + giftcardid;

                        }
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
            else if (command == "Issue Gift Card")
            {
                string baseUri = REAL_TIME_SERVICE_URL + "api/v1/giftcard/IssueGiftCard";

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                        GiftCard giftCard = new Controllers.GiftCard();
                        giftCard.GiftCardId = giftcardidtoprocess;
                        giftCard.Amount = amount;
                        giftCard.CurrencyCode = depositcurrencycode;
                        giftCard.TransactionId = "CL-" + Guid.NewGuid().ToString();

                        string jsonData = JsonConvert.SerializeObject(giftCard);

                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                        client.Headers.Add("Content-Type", "application/json");
                        client.Headers.Add("x-api-key", xAPIKey);
                        string responseText = client.UploadString(baseUri, jsonData);

                        var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                        string isSuccess = result["status"];
                        if (isSuccess.ToUpper().Equals("TRUE"))
                        {
                            TempData["Message"] = "Gift Card has been Issued";
                        }
                        else
                        {
                            TempData["Message"] = "Unable to Issue the Gift Card " + giftcardidtoprocess;
                        }
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
            else if (command == "Pay Gift Card")
            {
                string baseUri = REAL_TIME_SERVICE_URL + "api/v1/giftcard/PayGiftCard";

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        System.Net.ServicePointManager.ServerCertificateValidationCallback = (senderX, certificate, chain, sslPolicyErrors) => { return true; };

                        if (String.IsNullOrEmpty(giftcardidtoprocess) || String.IsNullOrWhiteSpace(giftcardidtoprocess))
                        {
                            TempData["Message"] = "Gift Card not provided";
                            return RedirectToAction("GiftCard"); ;
                        }

                        GiftCard giftCard = new Controllers.GiftCard();
                        giftCard.GiftCardId = giftcardidtoprocess;
                        giftCard.Amount = amount;
                        giftCard.CurrencyCode = depositcurrencycode;
                        giftCard.TransactionId = "CL-" + Guid.NewGuid().ToString();

                        string jsonData = JsonConvert.SerializeObject(giftCard);

                        var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                        client.Headers.Add("Content-Type", "application/json");
                        client.Headers.Add("x-api-key", xAPIKey);
                        string responseText = client.UploadString(baseUri, jsonData);

                        var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);

                        string isSuccess = result["status"];
                        if (isSuccess.ToUpper().Equals("TRUE"))
                        {
                            TempData["Message"] = "Gift Card has been Paid";
                        }
                        else
                        {
                            TempData["Message"] = "Unable to Pay the Gift Card " + giftcardidtoprocess;
                        }
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

            return RedirectToAction("GiftCard");
        }
    }

    class GiftCard
    {
        public string GiftCardId {get; set;}
        public string Amount { get; set; }
        public string CurrencyCode { get; set; }
        public string TransactionId { get; set; }
    }

}
