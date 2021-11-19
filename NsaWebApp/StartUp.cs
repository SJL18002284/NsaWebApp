using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartupAttribute(typeof(NsaWebApp.App_Start.StartUp))]
namespace NsaWebApp.App_Start
{
    public partial class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            configureAuth(app);
        }
       
    }
}