using System;
using System.IO;
using System.Threading.Tasks;
using PromoStudio.Storage.Properties;

namespace PromoStudio.Storage
{
    public class FileSystemStorageProvider // : IStorageProvider
    {
        public async Task<string> StoreFileAsync(string bucketName, string fileName, Stream dataStream)
        {
            var destPath = GetFilePath(bucketName, fileName);
            using (var destStream = File.Open(destPath, FileMode.Create))
            {
                await dataStream.CopyToAsync(destStream);
            }

            return GetFileUrl(bucketName, fileName);
        }

        public Stream GetFile(string bucketName, string fileName)
        {
            var path = GetFilePath(bucketName, fileName);
            return File.OpenRead(path);
        }

        public string GetFileUrl(string bucketName, string fileName)
        {
            return string.Format("/Resources/Download?crid={0}", fileName);
        }

        public void RemoveFile(string bucketName, string fileName)
        {
            throw new NotImplementedException();
        }

        private string GetFilePath(string bucketName, string fileName)
        {
            return Path.Combine(Settings.Default.LocalStoragePath, string.Format("{0}\\{1}", bucketName, fileName));
        }
    }
}