using System.Data;

namespace PromoStudio.Data
{
    public interface IConnectionManager
    {
        IDbConnection GetConnection();
    }
}