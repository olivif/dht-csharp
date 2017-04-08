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
    }
}
