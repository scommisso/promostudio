using System;
using System.IO;

namespace PromoStudio.Storage
{
    public class FileSystemStorageProvider // : IStorageProvider
    {
        public string StoreFile(string bucketName, string fileName, string filePath)
        {
            // Just return the file path to the file
            return filePath;
        }

        public string GetFileUrl(string bucketName, string fileName)
        {
            throw new NotImplementedException();
        }

        public Stream GetFile(string bucketName, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}