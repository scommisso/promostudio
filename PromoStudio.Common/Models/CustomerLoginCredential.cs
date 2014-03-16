using System;
using PromoStudio.Common.Enumerations;

namespace PromoStudio.Common.Models
{
    public class CustomerLoginCredential
    {
        public long pk_CustomerLoginCredentialId { get; set; }
        public long fk_CustomerId { get; set; }
        public short fk_CustomerLoginPlatformId { get; set; }
        public string EmailAddress { get; set; }
        public string LoginKey { get; set; }
        public sbyte PrimaryLogin { get; set; }
        public DateTime LastLogin { get; set; }

        public CustomerLoginPlatform Platform
        {
            get { return (CustomerLoginPlatform) fk_CustomerLoginPlatformId; }
            set { fk_CustomerLoginPlatformId = (sbyte) value; }
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_CustomerLoginCredentialId,
                fk_CustomerId,
                fk_CustomerLoginPlatformId,
                EmailAddress,
                LoginKey,
                PrimaryLogin,
                LastLogin
            };
        }
    }
}