using DHT.Exceptions;
using DHT.Nodes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHT.Tests.Nodes
{
    public class NodeStoreTests
    {
        [Test]
        public void NodeStore_ContainsKey_False()
        {
            // Arrange
            var store = new NodeStore();
            var key = "key";

            // Act
            var containsKey = store.ContainsKey(key);

            // Assert
            Assert.IsFalse(containsKey);
        }

        [Test]
        public void NodeStore_ContainsKey_True()
        {
            // Arrange
            var store = new NodeStore();
            var key = "key";
            var value = "value";
            store.AddValue(key, value);

            // Act
            var containsKey = store.ContainsKey(key);

            // Assert
            Assert.IsTrue(containsKey);
        }

        [Test]
        public void NodeStore_GetValue_Null()
        {
            // Arrange
            var store = new NodeStore();
            var key = "key";

            // Act
            var storeValue = store.GetValue(key);

            // Assert
            Assert.IsNull(storeValue);
        }

        [Test]
        public void NodeStore_GetValue_NotNull()
        {
            // Arrange
            var store = new NodeStore();
            var key = "key";
            var value = "value";
            store.AddValue(key, value);

            // Act
            var storeValue = store.GetValue(key);

            // Assert
            Assert.AreEqual(value, storeValue);
        }

        [Test]
        public void NodeStore_AddValue_True()
        {
            // Arrange
            var store = new NodeStore();
            var key = "key";
            var value = "value";

            // Act
            var added = store.AddValue(key, value);

            // Assert
            Assert.IsTrue(added);
        }

        [Test]
        public void NodeStore_AddValue_ThrowsIfAlreadyThere()
        {
            // Arrange
            var store = new NodeStore();
            var key = "key";
            var value = "value";
            store.AddValue(key, value);

            // Act
            Assert.Catch<DuplicateKeyException>(() => NodeStore_AddValue_ThrowsIfAlreadyThereAct(store, key, value));
        }

        private void NodeStore_AddValue_ThrowsIfAlreadyThereAct(NodeStore store, string key, string value)
        {
            store.AddValue(key, value);
        }

        [Test]
        public void NodeStore_RemoveValue_False()
        {
            // Arrange
            var store = new NodeStore();
            var key = "key";

            // Act
            var removed = store.RemoveValue(key);

            // Assert
            Assert.IsFalse(removed);
        }

        [Test]
        public void NodeStore_RemoveValue_True()
        {
            // Arrange
            var store = new NodeStore();
            var key = "key";
            var value = "value";
            var added = store.AddValue(key, value);

            // Act
            var removed = store.RemoveValue(key);

            // Assert
            Assert.IsTrue(removed);
        }
    }
}
