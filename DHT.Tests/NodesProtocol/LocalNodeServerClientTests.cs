using DHT.Exceptions;
using DHT.Nodes;
using DHT.NodesProtocol;
using Dhtproto;
using Grpc.Core;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHT.Tests.NodesProtocol
{
    public class LocalNodeServerClientTests
    {
        private Mock<INodeStore> nodeStoreMock;

        private LocalNodeServerClient client;

        [SetUp]
        public void TestInit()
        {
            this.nodeStoreMock = new Mock<INodeStore>();
            this.client = new LocalNodeServerClient(this.nodeStoreMock.Object);
        }

        [Test]
        public void LocalNodeServerClient_GetValue_ThrowsOnKeyNotFound()
        {
            // Arrange
            var key = "a";
            var request = new KeyMessage()
            {
                Key = key
            };

            // Act
            Assert.Catch<RpcException>(() => LocalNodeServerClient_GetValue_ThrowsOnKeyNotFoundAct(request));
        }

        private void LocalNodeServerClient_GetValue_ThrowsOnKeyNotFoundAct(KeyMessage request)
        {
            this.client.GetValue(request);
        }

        [Test]
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

        [Test]
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
            Assert.Catch<RpcException>(() => LocalNodeServerClient_RemoveValue_ThrowsOnFailureAct(request));
        }

        private void LocalNodeServerClient_RemoveValue_ThrowsOnFailureAct(KeyMessage request)
        {
            var response = this.client.RemoveValue(request);
        }

        [Test]
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

        [Test]
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
            Assert.Catch<RpcException>(() => LocalNodeServerClient_StoreValue_ThrowsOnFailureAct(request));
        }

        private void LocalNodeServerClient_StoreValue_ThrowsOnFailureAct(KeyValueMessage request)
        {
            var response = this.client.StoreValue(request);
        }

        [Test]
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
            Assert.Catch<RpcException>(() => LocalNodeServerClient_StoreValue_ThrowsOnDuplicateKeyAct(request));
        }

        private void LocalNodeServerClient_StoreValue_ThrowsOnDuplicateKeyAct(KeyValueMessage request)
        {
            var response = this.client.StoreValue(request);
        }

        [Test]
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
