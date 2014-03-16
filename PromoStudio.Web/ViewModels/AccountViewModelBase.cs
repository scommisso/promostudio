using System.Web;
using System.Web.Routing;
using PromoStudio.Common.Models;

namespace PromoStudio.Web.ViewModels
{
    public abstract class AccountViewModelBase : ViewModelBase
    {
        public Customer Customer { get; set; }

        #region ctor

        public AccountViewModelBase(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
        }

        #endregion
    }
}