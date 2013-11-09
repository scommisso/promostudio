using PromoStudio.Common.Enumerations;
using PromoStudio.Storage.Vidyard;

namespace PromoStudio.Storage
{
    public interface IStreamingProvider
    {
        Player StoreFile(string downloadUrl, string videoName, string videoDescription);
        CloudStorageStatus GetFileStatus(long videoId);
    }
}
