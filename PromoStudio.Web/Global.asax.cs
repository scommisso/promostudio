using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using Newtonsoft.Json;

namespace PromoStudio.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            IocConfig.RegisterIoc();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            var cName = FormsAuthentication.FormsCookieName;
            var cookie = Context.Request.Cookies[cName];
            if (cookie == null) { return; }

            var decrypted = FormsAuthentication.Decrypt(cookie.Value);
            if (decrypted == null) { return; }

            var userData = JsonConvert.DeserializeObject<AuthData>(decrypted.UserData);
            HttpContext.Current.User = new PromoStudioPrincipal(userData);
        }
    }
}