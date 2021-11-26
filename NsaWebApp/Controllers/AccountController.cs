using Firebase.Auth;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NsaWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace NsaWebApp.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// firestore variables 
        /// ApiKey to establish connection
        /// Bucket which is the database container credentials
        /// </summary>
        /// <returns></returns>
        private static string apiKey = "AIzaSyDL-UIoPKUEs3JfXH1yViIuTjDeqh006k4";
        private static string Bucket = "nsaauth.appspot.com";

        IFirebaseConfig config = new FireSharp.Config.FirebaseConfig
        {
            AuthSecret = "POVN51CMexBrDIGd7S0dYkPQjZINwkbt5M7NoW9r",
            BasePath = "https://nsaauth-default-rtdb.firebaseio.com/"
        };

        IFirebaseClient client;

        // GET: Account
        public ActionResult signUp()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> signUp(RegisterModel model)
        {
            try
            {
                var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(apiKey));

                var a = await auth.CreateUserWithEmailAndPasswordAsync(model.userEmail, model.userPassword, model.fullName, true);
                ModelState.AddModelError(string.Empty, "Please Verify Your Email To Login");

                addUserToFirebase(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }

            return View();
        }

        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginModel model)
        {
            try
            {
                //verification of the user
                if (ModelState.IsValid)
                {
                    var auth = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(apiKey));
                    var result = await auth.SignInWithEmailAndPasswordAsync(model.userEmail, model.userPassword);

                    string token = result.FirebaseToken;
                    string userID = result.User.LocalId;

                    if (token != "")
                    {
                        addLoginToFirebase(model);
                        Session["UserID"] = userID.ToString();
                        //this.SignInUser(user.Email, token, false);
                        //RedirectToAction("About", "Home");
                        return RedirectToAction("Index","Home");
                    }
                    else
                    {
                        //settings
                        ModelState.AddModelError(string.Empty, "Invalid Username Or Password");
                    }
                }
            }
            catch (Exception ex)
            {
                //info
                Console.Write(ex);
            }

            //something failed, unknown error redisplay form
            return this.View(model);
        }

        private void SignInUser(string email, string token, bool isPersistent)
        {
            //list of claims
            var claims = new List<Claim>();

            try
            {
                //Setting it up
                claims.Add(new Claim(ClaimTypes.Email, email));
                claims.Add(new Claim(ClaimTypes.Authentication, token));

                var claimID = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var ctx = Request.GetOwinContext();
                var authenticationManager = ctx.Authentication;

                //sign user in
                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, claimID);
            }
            catch (Exception ex)
            {
                //info
                throw ex;
            }
        }

        private void ClaimIdentities(string username, bool isPersistent)
        {
            //claim list
            var claims = new List<Claim>();

            try
            {
                claims.Add(new Claim(ClaimTypes.Name, username));

                var claimID = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
            }
            catch (Exception ex)
            {
                //display error
                throw ex;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                //verify url
                if (Url.IsLocalUrl(returnUrl))
                {
                    return this.Redirect(returnUrl);
                }
            }
            catch (Exception ex)
            {
                //display error
                throw ex;
            }

            //redirect to action
            return this.RedirectToAction("Logout", "Account");
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Logout()
        {
            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        //for the login auditing
        private void addLoginToFirebase(LoginModel model)
        {
            client = new FireSharp.FirebaseClient(config);

            var login = model;

            PushResponse response = client.Push("LoginAudit/", login);
            login.loginDateTime = DateTime.Now.ToString();
            login.loginID = response.Result.name;
            SetResponse setResponse = client.Set("LoginAudit/" + login.loginID, login);
        }

        private void addUserToFirebase(RegisterModel model)
        {
            client = new FireSharp.FirebaseClient(config);

            var user = model;

            PushResponse response = client.Push("User/", user);
            user.userEmail = response.Result.name;
            SetResponse setResponse = client.Set("User/" + user.userEmail, user);
        }
    }
}