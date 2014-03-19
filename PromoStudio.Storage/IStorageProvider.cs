using System.IO;
using System.Threading.Tasks;

namespace PromoStudio.Storage
{
    public interface IStorageProvider
    {
        Task<string> StoreFile(string bucketName, string fileName, Stream dataStream);
        string GetFileUrl(string bucketName, string fileName);
        Stream GetFile(string bucketName, string fileName);
    }
}