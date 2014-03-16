﻿using System;
using PromoStudio.Common.Enumerations;

namespace PromoStudio.Common.Models
{
    public class Customer
    {
        public long pk_CustomerId { get; set; }
        public int? fk_OrganizationId { get; set; }
        public sbyte fk_CustomerStatusId { get; set; }
        public int? fk_VerticalId { get; set; }
        public string FullName { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }

        public CustomerStatus Status
        {
            get { return (CustomerStatus) fk_CustomerStatusId; }
            set { fk_CustomerStatusId = (sbyte) value; }
        }

        public dynamic ToPoco()
        {
            return new
            {
                pk_CustomerId,
                fk_OrganizationId,
                fk_CustomerStatusId,
                fk_VerticalId,
                FullName,
                DateCreated,
                DateUpdated
            };
        }
    }
}