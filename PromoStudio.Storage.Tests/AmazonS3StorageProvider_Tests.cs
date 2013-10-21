using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PromoStudio.Storage.Tests
{
    [TestClass]
    public class AmazonS3StorageProvider_Tests
    {
        [TestMethod]
        public void TestUpload()
        {
            var provider = new AmazonS3StorageProvider();
            var playerId = provider.StoreFile("1", "test.mov", @"C:\PromoStudio\Output\1\3\5_test_RotatingPhotos_preview.mov");
            var url = provider.GetFile("1", "test.mov");
        }
    }
}
