using System.Collections.Generic;
using Newtonsoft.Json;
using PromoStudio.Common.Models;
using PromoStudio.Resources;
using System.Web;
using System.Web.Routing;

namespace PromoStudio.Web.ViewModels
{
    public class AccountDashboardViewModel : AccountViewModelBase
    {
        #region ctor

        public AccountDashboardViewModel(HttpContextBase context, RouteData routeData)
            : base(context, routeData)
        {
        }

        #endregion
    }
}