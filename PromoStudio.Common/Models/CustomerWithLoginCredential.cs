using System;
using PromoStudio.Common.Enumerations;

namespace PromoStudio.Common.Models
{
    public class CustomerWithLoginCredential
    {
        public long pk_CustomerId { get; set; }
        public int? fk_OrganizationId { get; set; }
        public sbyte fk_CustomerStatusId { get; set; }
        public int? fk_VerticalId { get; set; }
        public string FullName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public short pk_CustomerLoginCredentialId { get; set; }
        public short fk_CustomerLoginPlatformId { get; set; }
        public string EmailAddress { get; set; }
        public string LoginKey { get; set; }
        public sbyte PrimaryLogin { get; set; }
        public DateTime LastLogin { get; set; }

        public string OrganizationName { get; set; }
        public string OrganizationDisplayName { get; set; }

        public CustomerStatus Status
        {
            get { return (CustomerStatus) fk_CustomerStatusId; }
            set { fk_CustomerStatusId = (sbyte) value; }
        }

        public CustomerLoginPlatform Platform
        {
            get { return (CustomerLoginPlatform) fk_CustomerLoginPlatformId; }
            set { fk_CustomerLoginPlatformId = (sbyte) value; }
        }

        public Customer ToCustomer()
        {
            return new Customer
            {
                pk_CustomerId = pk_CustomerId,
                fk_OrganizationId = fk_OrganizationId,
                fk_CustomerStatusId = fk_CustomerStatusId,
                fk_VerticalId = fk_VerticalId,
                FullName = FullName,
                DateCreated = DateCreated,
                DateUpdated = DateUpdated
            };
        }

        public CustomerLoginCredential ToCustomerLoginCredential()
        {
            return new CustomerLoginCredential
            {
                pk_CustomerLoginCredentialId = pk_CustomerLoginCredentialId,
                fk_CustomerId = pk_CustomerId,
                fk_CustomerLoginPlatformId = fk_CustomerLoginPlatformId,
                EmailAddress = EmailAddress,
                LoginKey = LoginKey,
                PrimaryLogin = PrimaryLogin,
                LastLogin = LastLogin
            };
        }

        public Organization ToOrganization()
        {
            return new Organization
            {
                pk_OrganizationId = fk_OrganizationId.Value,
                fk_VerticalId = fk_VerticalId,
                Name = OrganizationName,
                DisplayName = OrganizationDisplayName
            };
        }
    }
}