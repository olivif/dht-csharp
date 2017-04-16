namespace DHT.Tests.Nodes
{
    using System.Data.Linq;
    using DHT.Nodes;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NodeStoreTests
    {
        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
        [ExpectedException(typeof(DuplicateKeyException))]
        public void NodeStore_AddValue_ThrowsIfAlreadyThere()
        {
            // Arrange 
            var store = new NodeStore();
            var key = "key";
            var value = "value";
            store.AddValue(key, value);

            // Act
            store.AddValue(key, value);
        }

        [TestMethod]
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

        [TestMethod]
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
