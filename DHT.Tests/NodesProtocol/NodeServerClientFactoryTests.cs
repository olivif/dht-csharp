namespace DHT.Tests.NodesProtocol
{
    using DHT.Nodes;
    using DHT.NodesProtocol;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NodeServerClientFactoryTests
    {
        [TestMethod]
        public void NodeServerClientFactory_CreateLocalClient()
        {
            // Arrange
            var factory = new NodeServerClientFactory();

            // Act
            var client = factory.CreateLocalClient();

            // Assert
            Assert.IsInstanceOfType(client, typeof(LocalNodeServerClient));
        }

        [TestMethod]
        public void NodeServerClientFactory_CreateRemoteClient()
        {
            // Arrange
            var factory = new NodeServerClientFactory();
            var nodeInfo = new NodeInfo();

            // Act
            var client = factory.CreateRemoteClient(nodeInfo);

            // Assert
            Assert.IsNotInstanceOfType(client, typeof(LocalNodeServerClient));
        }
    }
}
