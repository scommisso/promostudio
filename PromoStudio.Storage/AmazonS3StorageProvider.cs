using System;
using System.IO;
using System.Web;
using LitS3;
using PromoStudio.Storage.Properties;

namespace PromoStudio.Storage
{
    public class AmazonS3StorageProvider : IStorageProvider
    {
        public string StoreFile(string bucketName, string fileName, string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ArgumentException(string.Format("filePath \"{0}\" does not exist.", filePath));
            }
            bucketName = Settings.Default.BucketNamePrefix + bucketName;

            S3Service storage = GetStorageService();
            EnsureBucketExists(storage, bucketName);
            storage.AddObject(filePath, bucketName, fileName, MimeMapping.GetMimeMapping(fileName), CannedAcl.PublicRead);
            return storage.GetUrl(bucketName, fileName);
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