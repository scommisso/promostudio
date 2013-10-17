using System.Security.Principal;

namespace PromoStudio.Web
{
    public interface IPromoStudioIdentity : IIdentity
    {
        long CustomerId { get; }
        int? OrganizationId { get; }
        int? VerticalId { get; }
        string FullName { get; }
        string EmailAddress { get; }
        string OrganizationName { get; }
    }
}
