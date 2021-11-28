using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NsaWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NsaWebApp.Controllers
{
    public class HomeController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "POVN51CMexBrDIGd7S0dYkPQjZINwkbt5M7NoW9r",
            BasePath = "https://nsaauth-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Requests");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<DonationRequestModel>();
            try
            {
                if (!data.Equals(null))
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<DonationRequestModel>(((JProperty)item).Value.ToString()));
                    }
                    return View(list);
                }
                else
                {

                    ModelState.AddModelError(string.Empty, "No Data Found");
                    return RedirectToAction("Create");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "No Data Found");
                Console.WriteLine(ex.Message);
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}