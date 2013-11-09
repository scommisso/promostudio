using System.Threading.Tasks;

namespace PromoStudio.CloudStatusService
{
    public interface ICloudStatusProcessor
    {
        Task Execute();
    }
}
