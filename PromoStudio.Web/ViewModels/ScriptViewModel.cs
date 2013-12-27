using System.Web;
using System.Web.Routing;

namespace PromoStudio.Web.ViewModels
{
    public class ScriptViewModel : BuildViewModelBase
    {
        public ScriptViewModel(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
            CurrentStep = 3;
        }
    }
}