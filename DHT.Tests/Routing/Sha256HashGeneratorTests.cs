using DHT.Routing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHT.Tests.Routing
{
    public class Sha256HashGeneratorTests
    {
        [Test]
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

        [Test]
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

        [Test]
        public void Sha256HashGenerator_Hash_ThrowsForNullString()
        {
            // Arrange
            string value = null;
            var hashGenerator = new Sha256HashGenerator();

            // Act
            Assert.Catch<ArgumentNullException>(() => Sha256HashGenerator_Hash_ThrowsForNullStringAct(hashGenerator, value));
        }

        private void Sha256HashGenerator_Hash_ThrowsForNullStringAct(Sha256HashGenerator hashGenerator, string value)
        {
            var valueHash = hashGenerator.Hash(value);
        }

        [Test]
        public void Sha256HashGenerator_Hash_ThrowsForEmptyString()
        {
            // Arrange
            string value = string.Empty;
            var hashGenerator = new Sha256HashGenerator();

            // Act
            Assert.Catch<ArgumentException>(() => Sha256HashGenerator_Hash_ThrowsForEmptyStringAct(hashGenerator, value));
        }


        private void Sha256HashGenerator_Hash_ThrowsForEmptyStringAct(Sha256HashGenerator hashGenerator, string value)
        {

            var valueHash = hashGenerator.Hash(value);
        }
    }
}
