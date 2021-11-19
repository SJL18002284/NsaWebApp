using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NsaWebApp.App_Start
{
    public partial class StartUp
    {
        //confire authentication
        public void configureAuth(IAppBuilder app)
        {
            //create new cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                //default cookie type
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                //login path, controller and action result
                LoginPath = new PathString("/Account/Login"),
                //logout path, controller and action result
                LogoutPath = new PathString("/Account/Logout"),
                //expiration time for cookie
                ExpireTimeSpan = TimeSpan.FromMinutes(30.0)
            });
        }
    }
}