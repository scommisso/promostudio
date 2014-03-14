using PromoStudio.Common.Enumerations;
using VimeoDotNet.Models;
using VimeoDotNet.Net;

namespace PromoStudio.Storage
{
    public interface IStreamingProvider
    {
        IUploadRequest StoreFile(string filePath, string videoName, string videoDescription);
        Video GetVideo(long videoId);
    }
}
