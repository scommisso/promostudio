using System.Web;
using System.Web.Routing;
using PromoStudio.Common.Models;

namespace PromoStudio.Web.ViewModels
{
    public class CustomizeViewModel : BuildViewModelBase
    {
        public CustomizeViewModel(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
            CurrentStep = 2;
        }

        public Organization Organization { get; set; }
    }
}