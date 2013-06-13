using System;
using System.Data;

namespace PromoStudio.Interfaces
{
    public interface IConnectionManager
    {
        IDbConnection GetConnection();
    }
}
