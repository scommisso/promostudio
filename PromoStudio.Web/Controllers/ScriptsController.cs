using System.Web.Mvc;

namespace PromoStudio.Web.Controllers
{
    public class ScriptsController : Controller
    {
        [ActionName("strings.js")]
        [OutputCache(Duration = 604800)] // 1 week
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