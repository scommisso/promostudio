using PromoStudio.Common.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PromoStudio.Common.Models
{
    public class Customer
    {
        public long pk_CustomerId { get; set; }
        public int fk_OrganizationId { get; set; }
        public sbyte fk_CustomerStatusId { get; set; }
        public int fk_VerticalId { get; set; }
        public string FullName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public CustomerStatus Status
        {
            get { return (CustomerStatus)fk_CustomerStatusId; }
            set { fk_CustomerStatusId = (sbyte)value; }
        }

        public dynamic ToPoco()
        {
            return new
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
    }
}
