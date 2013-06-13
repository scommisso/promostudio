using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoStudio.Common.Enumerations
{
    public enum CustomerStatus : sbyte
    {
        Active = 1,
        Deleted,
        Suspended
    }
}
