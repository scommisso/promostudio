using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PromoStudio.Storage.Tests
{
    [TestClass]
    public class VidyardStreamingProvider_Tests
    {
        [TestMethod]
        public void TestUpload()
        {
            var provider = new VidyardStreamingProvider();
            var playerId = provider.StoreFile(
                "http://s3.amazonaws.com/PromoStudio-Beta-1/test.mov",
                "Public Test",
                "Testing Public S3 URL");
        }
    }
}
