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
    public class VolunteerController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "POVN51CMexBrDIGd7S0dYkPQjZINwkbt5M7NoW9r",
            BasePath = "https://nsaauth-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        // GET: Volunteer
        public ActionResult Index()
        {
            client = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = client.Get("Volunteers");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<VolunteerModel>();
            try
            {
                if (!data.Equals(null))
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<VolunteerModel>(((JProperty)item).Value.ToString()));
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
        public ActionResult Create(VolunteerModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    addVolunteerToFirebase(model);
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

        private void addVolunteerToFirebase(VolunteerModel model)
        {
            client = new FireSharp.FirebaseClient(config);

            var volunteer = model;

            PushResponse response = client.Push("Volunteers/", volunteer);
            volunteer.VolunteerID = response.Result.name;
            SetResponse setResponse = client.Set("Volunteers/" + volunteer.VolunteerID, volunteer);
        }
    }
}