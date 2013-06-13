using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PromoStudio.Web.Controllers
{
    public class ScriptsController : Controller
    {
        [ActionName("googleOAuth.js")]
        public ActionResult GoogleOAuth2()
        {
            return this.JavaScriptFromView("GoogleOAuth");
        }

        [ActionName("facebookOAuth.js")]
        public ActionResult FacebookOAuth2()
        {
            return this.JavaScriptFromView("FacebookOAuth");
        }

        protected override void HandleUnknownAction(string actionName)
        {
            var res = this.JavaScriptFromView();
            res.ExecuteResult(ControllerContext);
        }
    }
}
