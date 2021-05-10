using DHT.Nodes;
using DHT.NodesProtocol;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHT.Tests.NodesProtocol
{
    public class NodeServerClientFactoryTests
    {
        [Test]
        public void NodeServerClientFactory_CreateLocalClient()
        {
            // Arrange
            var factory = new NodeServerClientFactory();

            // Act
            var client = factory.CreateLocalClient();

            // Assert
            Assert.IsInstanceOf<LocalNodeServerClient>(client);
        }

        [Test]
        public void NodeServerClientFactory_CreateRemoteClient()
        {
            // Arrange
            var factory = new NodeServerClientFactory();
            var nodeInfo = new NodeInfo();

            // Act
            var client = factory.CreateRemoteClient(nodeInfo);

            // Assert
            Assert.IsNotInstanceOf<LocalNodeServerClient>(client);
        }
    }
}
