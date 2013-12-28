using PromoStudio.Common.Models;
using System.Web;
using System.Web.Routing;

namespace PromoStudio.Web.ViewModels
{
    public class CustomizeViewModel : BuildViewModelBase
    {
        public Organization Organization { get; set; }

        public CustomizeViewModel(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
            CurrentStep = 2;
        }
    }
}