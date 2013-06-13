using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PromoStudio.Common.Extensions;

namespace PromoStudio.Common.Tests
{
    [TestClass]
    public class StringExtensions_Tests
    {
        [TestMethod]
        public void StringExtensions_ToSafeFileName_CollapsesUnderscores()
        {
            // Arrange
            string fileName = "a___b___c.txt";

            // Act
            string result = StringExtensions.ToSafeFileName(fileName);

            // Assert
            Assert.AreEqual("a_b_c.txt", result);
        }

        [TestMethod]
        public void StringExtensions_ToSafeFileName_CollapsesUnderscoresAndHyphens()
        {
            // Arrange
            string fileName = "a_-_b_-_c.txt";

            // Act
            string result = StringExtensions.ToSafeFileName(fileName);

            // Assert
            Assert.AreEqual("a_b_c.txt", result);
        }

        [TestMethod]
        public void StringExtensions_ToSafeFileName_DoesNotConvertHyphensToUnderscore()
        {
            // Arrange
            string fileName = "a-b-c_-_d.txt";

            // Act
            string result = StringExtensions.ToSafeFileName(fileName);

            // Assert
            Assert.AreEqual("a-b-c_d.txt", result);
        }
    }
}
