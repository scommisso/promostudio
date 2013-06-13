using System;
using System.Threading.Tasks;

namespace PromoStudio.RenderQueue
{
    public interface IQueueProcessor
    {
        Task Execute();
    }
}
