using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;

namespace PromoStudio.Web
{
    public interface IPromoStudioIdentity : IIdentity
    {
        long CustomerId { get; }
        long? OrganizationId { get; }
        string FullName { get; }
        string EmailAddress { get; }
        string OrganizationName { get; }
    }
}
