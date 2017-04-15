namespace DHT.Tests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NodeTests
    {
        [TestMethod]
        public void Node_Constructor_Default()
        {
            // Act
            var node = new Node();

            // Assert
            Assert.IsNotNull(node.NodeId);
            Assert.IsFalse(node.Nodes.Any());
        }

        [TestMethod]
        public void Node_NodeId_GeneratesRandomIds()
        {
            // Arrange Act
            var node1 = new Node();
            var node2 = new Node();

            // Assert
            Assert.AreNotEqual(node1.NodeId, node2.NodeId);
        }
    }
}
