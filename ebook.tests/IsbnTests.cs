using System;
using ebook.core.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ebook.tests
{
    [TestClass]
    public class IsbnTests
    {
        [TestMethod]
        public void Normalise_Null_ReturnsNull()
        {
            AssertIsbn(null, null);
        }

        [TestMethod]
        public void Normalise_Empty_ReturnsEmpty()
        {
            AssertIsbn(string.Empty, string.Empty);
        }

        [TestMethod]
        public void Normalise_OK_ReturnsOK()
        {
            AssertIsbn("0123456789012", "0123456789012");
        }

        [TestMethod]
        public void Normalise_WithHyphen_ReturnsNoHyphen()
        {
            AssertIsbn("0-1234-567890-12", "0123456789012");
        }

        [TestMethod]
        public void Normalise_WithSpaces_ReturnsNoSpaces()
        {
            AssertIsbn("0 1234 567890 12", "0123456789012");
        }

        void AssertIsbn(string input, string expected)
        {
            string actual = Isbn.Normalise(input);
            Assert.AreEqual(expected, actual);
        }
    }
}
