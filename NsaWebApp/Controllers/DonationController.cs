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
    public class DonationController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "POVN51CMexBrDIGd7S0dYkPQjZINwkbt5M7NoW9r",
            BasePath = "https://nsaauth-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        // GET: Donation
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Donations");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<DonationModel>();
            try
            {
                if (!data.Equals(null))
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<DonationModel>(((JProperty)item).Value.ToString()));
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
        public ActionResult Create(string id, DonationModel donModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    addDonationToFirebase(id, donModel);
                    ModelState.AddModelError(string.Empty, "Created Successfully");
                    //return RedirectToAction("Create");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Process Failed");
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
            FirebaseResponse response = client.Get("Donations/" + id);
            DonationModel data = JsonConvert.DeserializeObject<DonationModel>(response.Body);
            return View(data);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Donations/" + id);
            DonationModel data = JsonConvert.DeserializeObject<DonationModel>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(DonationModel model)
        {
            client = new FireSharp.FirebaseClient(config);
            SetResponse response = client.Set("Donations/" + model.donationID, model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Delete("Donations/" + id);
            return RedirectToAction("Index");
        }

        private void addDonationToFirebase(string id, DonationModel model)
        {
            client = new FireSharp.FirebaseClient(config);

            var donation = model;

            PushResponse response = client.Push("Donations/", donation);
            donation.donationID = response.Result.name;
            donation.requestID = id;
            SetResponse setResponse = client.Set("Donations/" + donation.donationID, donation);
        }
    }
}