using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PromoStudio.Storage.Tests
{
    [TestClass]
    public class VimeoStreamingProvider_Tests
    {
        [TestMethod]
        public void TestUpload()
        {
            var provider = new VimeoStreamingProvider();
            var playerId = provider.StoreFile(
                "http://s3.amazonaws.com/PromoStudio-Beta-1/test.mov",
                "Public Test",
                "Testing Public S3 URL");
        }

        [TestMethod]
        public void TestStatus()
        {
            var provider = new VimeoStreamingProvider();
            var status = provider.GetFileStatus(48144);
        }
    }
}
