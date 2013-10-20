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
                "http://s3.amazonaws.com/PromoStudio-Beta-1/1-preview.m4v?AWSAccessKeyId=AKIAJGWX4PS3I77KQNYA&Expires=1413844161&Signature=JXw7JKZz9NQx5aKlQEpiYjUDTYg%3D",
                "My First Video");
        }
    }
}
