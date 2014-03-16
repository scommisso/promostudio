using System.Web.Mvc;
using log4net;
using PromoStudio.Data;

namespace PromoStudio.Web.Controllers
{
    public abstract class ControllerBase : AsyncController
    {
        protected PromoStudioIdentity _currentUser;
        protected IDataService _dataService;
        protected ILog _log;

        protected ControllerBase(IDataService dataService, ILog log)
        {
            _dataService = dataService;
            _log = log;
        }

        protected PromoStudioIdentity CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    if (HttpContext.User == null || HttpContext.User.Identity == null ||
                        !HttpContext.User.Identity.IsAuthenticated)
                    {
                        return null;
                    }
                    var ident = Request.RequestContext.HttpContext.User.Identity as PromoStudioIdentity;
                    if (ident == null)
                    {
                        return null;
                    }
                    _currentUser = ident;
                }
                return _currentUser;
            }
        }

        protected ActionResult PAjax(
            string viewName = null,
            string masterName = null,
            object model = null)
        {
            bool isPAJAX = Request.IsPAjaxRequest();
            ViewData["IsPAJAX"] = isPAJAX;
            return isPAJAX
                ? PartialView(viewName, model)
                : View(viewName, model) as ActionResult;
        }
    }
}