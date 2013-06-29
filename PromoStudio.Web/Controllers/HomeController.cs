using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using log4net;
using PromoStudio.Data;

namespace PromoStudio.Web.Controllers
{
    public class HomeController : ControllerBase
    {
        public HomeController(IDataService dataService, ILog log)
            : base(dataService, log)
        {
        }

        //
        // GET: /Home/
        public ActionResult Index()
        {
            return PAjax();
        }

        //
        // GET: /Home/goa2cb
        [ActionName("goa2cb")]
        public ActionResult GoogleOAuth2Callback()
        {
            return View("OAuth2Callback");
        }

        //
        // GET: /Home/foa2cb
        [ActionName("foa2cb")]
        public ActionResult FacebookOAuth2Callback()
        {
            return View("OAuth2Callback");
        }

        //
        // GET: /Home/toa2cb
        [ActionName("toa2cb")]
        public ActionResult TwitterOAuth2Callback()
        {
            return View("OAuth2Callback");
        }
    }
}
