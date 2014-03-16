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

        [ActionName("strings.js")]
        public ActionResult Strings()
        {
            return this.JavaScriptFromView("Strings");
        }

        protected override void HandleUnknownAction(string actionName)
        {
            ActionResult res = this.JavaScriptFromView();
            res.ExecuteResult(ControllerContext);
        }
    }
}