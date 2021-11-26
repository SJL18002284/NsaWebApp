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
    public class DonationRequestController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "POVN51CMexBrDIGd7S0dYkPQjZINwkbt5M7NoW9r",
            BasePath = "https://nsaauth-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        // GET: DonationRequest
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

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DonationRequestModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    addRequestToFirebase(model);
                    ModelState.AddModelError(string.Empty, "Created Successfully");
                    //return RedirectToAction("Create");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Process unsuccessful");
                    return RedirectToAction("Create");

                }
                
                //
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                
            }
            ModelState.Clear();
            return View();
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Requests/" + id);
            DonationRequestModel data = JsonConvert.DeserializeObject<DonationRequestModel>(response.Body);
            return View(data);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Requests/" + id);
            DonationRequestModel data = JsonConvert.DeserializeObject<DonationRequestModel>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(DonationRequestModel model)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Requests/" + model.requestID, model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Requests/" + id);
            return RedirectToAction("Index");
        }

        private void addRequestToFirebase(DonationRequestModel model)
        {
            client = new FireSharp.FirebaseClient(config);

            var request = model;

            PushResponse response = client.Push("Requests/", request);
            request.requestID = response.Result.name;
            SetResponse setResponse = client.Set("Requests/" + request.requestID, request);
        }
    }
}