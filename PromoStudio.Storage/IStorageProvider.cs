using System.IO;

namespace PromoStudio.Storage
{
    public interface IStorageProvider
    {
        string StoreFile(string bucketName, string fileName, string filePath);
        string GetFileUrl(string bucketName, string fileName);
        Stream GetFile(string bucketName, string fileName);
    }
}
