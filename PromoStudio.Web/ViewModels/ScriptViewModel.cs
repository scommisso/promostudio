using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using PromoStudio.Common.Models;

namespace PromoStudio.Web.ViewModels
{
    public class ScriptViewModel : BuildViewModelBase
    {
        public ScriptViewModel(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
            CurrentStep = 3;
        }

        public List<AudioScriptTemplate> Scripts { get; set; }
    }
}