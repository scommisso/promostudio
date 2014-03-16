using System;
using System.Security.Principal;

namespace PromoStudio.Web
{
    public class PromoStudioPrincipal : IPrincipal
    {
        private readonly IPromoStudioIdentity _identity;

        public PromoStudioPrincipal(AuthData authData)
        {
            _identity = new PromoStudioIdentity(authData);
        }

        public IIdentity Identity
        {
            get { return _identity; }
        }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }
    }
}