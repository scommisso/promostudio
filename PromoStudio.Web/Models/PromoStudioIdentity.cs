using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace PromoStudio.Web
{
    public class PromoStudioIdentity : IPromoStudioIdentity, IIdentity
    {
        public long CustomerId { get; set; }
        public long? OrganizationId { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public string OrganizationName { get; set; }
        public string AuthenticationType { get; set; }

        string IIdentity.Name
        {
            get { return EmailAddress; }
        }

        public bool IsAuthenticated
        {
            get
            {
                return !string.IsNullOrEmpty(AuthenticationType);
            }
        }


        public PromoStudioIdentity(AuthData authData)
        {
            CustomerId = authData.CustomerId;
            OrganizationId = authData.OrganizationId;
            FullName = authData.FullName;
            EmailAddress = authData.EmailAddress;
            OrganizationName = authData.OrganizationName;
            AuthenticationType = authData.AuthenticationType;

        }
    }
}
