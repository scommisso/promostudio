using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LitS3;
using PromoStudio.Storage.Properties;

namespace PromoStudio.Storage
{
    public class AmazonS3StorageProvider : IStorageProvider
    {
        public string StoreFile(string bucketName, string fileName, string filePath)
        {
            if (!File.Exists(filePath)) {
                throw new ArgumentException(string.Format("filePath \"{0}\" does not exist.", filePath));
            }
            bucketName = Settings.Default.BucketNamePrefix + bucketName;

            var storage = GetStorageService();
            EnsureBucketExists(storage, bucketName);
            storage.AddObject(filePath, bucketName, fileName);

            return storage.GetAuthorizedUrl(bucketName, fileName, DateTime.Now.AddYears(1));
        }

        public System.IO.Stream GetFile(string bucketName, string fileName)
        {
            bucketName = Settings.Default.BucketNamePrefix + bucketName;

            var storage = GetStorageService();
            return storage.GetObjectStream(bucketName, fileName);
        }

        private void EnsureBucketExists(S3Service s3service, string bucketName)
        {
            var access = s3service.QueryBucket(bucketName);
            if (access == BucketAccess.Accessible) { return; }
            if (access == BucketAccess.NotAccessible)
                throw new ArgumentOutOfRangeException("bucketName is already taken. Please provide a unique name");
            s3service.CreateBucket(bucketName);
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
