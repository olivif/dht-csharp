namespace DHT.Tests.NodesProtocol
{
    using System.Data.Linq;
    using DHT.Nodes;
    using DHT.NodesProtocol;
    using Dhtproto;
    using Grpc.Core;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class LocalNodeServerClientTests
    {
        private Mock<INodeStore> nodeStoreMock;

        private LocalNodeServerClient client;

        [TestInitialize]
        public void TestInit()
        {
            this.nodeStoreMock = new Mock<INodeStore>();
            this.client = new LocalNodeServerClient(this.nodeStoreMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(RpcException))]
        public void LocalNodeServerClient_GetValue_ThrowsOnKeyNotFound()
        {
            // Arrange
            var key = "a";
            var request = new KeyMessage()
            {
                Key = key
            };

            // Act
            this.client.GetValue(request);
        }

        [TestMethod]
        public void LocalNodeServerClient_GetValue_GetsValue()
        {
            // Arrange
            var key = "a";
            var value = "aValue";
            var request = new KeyMessage()
            {
                Key = key
            };
            this.nodeStoreMock
                .Setup(x => x.ContainsKey(key))
                .Returns(true);
            this.nodeStoreMock
                .Setup(x => x.GetValue(key))
                .Returns(value);

            // Act
            var response = this.client.GetValue(request);

            // Assert
            Assert.AreEqual(key, response.Key);
            Assert.AreEqual(value, response.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(RpcException))]
        public void LocalNodeServerClient_RemoveValue_ThrowsOnFailure()
        {
            // Arrange
            var key = "a";
            var request = new KeyMessage()
            {
                Key = key
            };
            this.nodeStoreMock
                .Setup(x => x.RemoveValue(key))
                .Returns(false);

            // Act
            var response = this.client.RemoveValue(request);
        }

        [TestMethod]
        public void LocalNodeServerClient_RemoveValue_RemovesValue()
        {
            // Arrange
            var key = "a";
            var request = new KeyMessage()
            {
                Key = key
            };
            this.nodeStoreMock
                .Setup(x => x.RemoveValue(key))
                .Returns(true);

            // Act
            var response = this.client.RemoveValue(request);

            // Assert
            Assert.AreEqual(key, response.Key);
        }

        [TestMethod]
        [ExpectedException(typeof(RpcException))]
        public void LocalNodeServerClient_StoreValue_ThrowsOnFailure()
        {
            // Arrange
            var key = "a";
            var value = "aValue";
            var request = new KeyValueMessage()
            {
                Key = key,
                Value = value
            };
            this.nodeStoreMock
                .Setup(x => x.AddValue(key, value))
                .Returns(false);

            // Act
            var response = this.client.StoreValue(request);
        }

        [TestMethod]
        [ExpectedException(typeof(RpcException))]
        public void LocalNodeServerClient_StoreValue_ThrowsOnDuplicateKey()
        {
            // Arrange
            var key = "a";
            var value = "aValue";
            var request = new KeyValueMessage()
            {
                Key = key,
                Value = value
            };
            this.nodeStoreMock
                .Setup(x => x.AddValue(key, value))
                .Throws(new DuplicateKeyException(key));

            // Act
            var response = this.client.StoreValue(request);
        }

        [TestMethod]
        public void LocalNodeServerClient_StoreValue_StoresValue()
        {
            // Arrange
            var key = "a";
            var value = "aValue";
            var request = new KeyValueMessage()
            {
                Key = key,
                Value = value
            };
            this.nodeStoreMock
                .Setup(x => x.AddValue(key, value))
                .Returns(true);

            // Act
            var response = this.client.StoreValue(request);

            // Assert
            Assert.AreEqual(key, response.Key);
            Assert.AreEqual(value, response.Value);
        }
    }
}
