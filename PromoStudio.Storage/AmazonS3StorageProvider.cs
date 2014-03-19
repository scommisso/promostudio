using System;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using LitS3;
using PromoStudio.Storage.Properties;

namespace PromoStudio.Storage
{
    public class AmazonS3StorageProvider : IStorageProvider
    {
        public async Task<string> StoreFile(string bucketName, string fileName, Stream dataStream)
        {
            if (dataStream == null || !dataStream.CanRead)
            {
                throw new ArgumentException("Unable to read from dataStream");
            }


            bucketName = Settings.Default.BucketNamePrefix + bucketName;

            string tempPath = Path.GetTempFileName();
            try
            {
                using (var fs = File.Open(tempPath, FileMode.Create))
                {
                    await dataStream.CopyToAsync(fs);
                }

                // TODO: Make S3 provider async when available
                S3Service storage = GetStorageService();
                EnsureBucketExists(storage, bucketName);
                storage.AddObject(tempPath, bucketName, fileName, MimeMapping.GetMimeMapping(fileName),
                    CannedAcl.PublicRead);
                return storage.GetUrl(bucketName, fileName);
            }
            finally
            {
                try
                {
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                }
                catch
                {
                }
            }

        }

        public string GetFileUrl(string bucketName, string fileName)
        {
            bucketName = Settings.Default.BucketNamePrefix + bucketName;
            S3Service storage = GetStorageService();
            return storage.GetUrl(bucketName, fileName);
        }

        public Stream GetFile(string bucketName, string fileName)
        {
            bucketName = Settings.Default.BucketNamePrefix + bucketName;

            S3Service storage = GetStorageService();
            return storage.GetObjectStream(bucketName, fileName);
        }

        private void EnsureBucketExists(S3Service s3Service, string bucketName)
        {
            BucketAccess access = s3Service.QueryBucket(bucketName);
            if (access == BucketAccess.Accessible)
            {
                return;
            }
            if (access == BucketAccess.NotAccessible)
                throw new ArgumentOutOfRangeException("bucketName",
                    "bucketName is already taken. Please provide a unique name");
            s3Service.CreateBucket(bucketName);
        }

        private S3Service GetStorageService()
        {
            var s3 = new S3Service
            {
                AccessKeyID = Settings.Default.AmazonS3AccessKey,
                SecretAccessKey = Settings.Default.AmazonS3SecretKey
            };
            return s3;
        }
    }
}