namespace DHT.Tests.Routing
{
    using System;
    using DHT.Routing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Sha256HashGeneratorTests
    {
        [TestMethod]
        public void Sha256HashGenerator_Hash_IsConsistent()
        {
            // Arrange
            var value = "value";
            var hashGenerator = new Sha256HashGenerator();

            // Act
            var valueHash1 = hashGenerator.Hash(value);
            var valueHash2 = hashGenerator.Hash(value);

            // Assert
            Assert.AreEqual(valueHash1, valueHash2);
        }

        [TestMethod]
        public void Sha256HashGenerator_Hash_IsDifferent()
        {
            // Arrange
            var value1 = "value1";
            var value2 = "value2";
            var hashGenerator = new Sha256HashGenerator();

            // Act
            var valueHash1 = hashGenerator.Hash(value1);
            var valueHash2 = hashGenerator.Hash(value2);

            // Assert
            Assert.AreNotEqual(valueHash1, valueHash2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Sha256HashGenerator_Hash_ThrowsForNullString()
        {
            // Arrange
            string value = null;
            var hashGenerator = new Sha256HashGenerator();

            // Act
            var valueHash = hashGenerator.Hash(value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Sha256HashGenerator_Hash_ThrowsForEmptyString()
        {
            // Arrange
            string value = string.Empty;
            var hashGenerator = new Sha256HashGenerator();

            // Act
            var valueHash = hashGenerator.Hash(value);
        }
    }
}
