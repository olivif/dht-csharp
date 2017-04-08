using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DHT.Tests
{
    [TestClass]
    public class NodeIdTests
    {
        [TestMethod]
        public void NodeId_Constructor_GeneratesId()
        {
            // Arrange 
            var nodeId = new NodeId();

            // Act
            var nodeIdString = nodeId.ToString();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(nodeIdString));
        }

        [TestMethod]
        public void NodeId_Constructor_GeneratesRandomIds()
        {
            // Arrange 
            var nodeId1 = new NodeId();
            var nodeId2 = new NodeId();

            // Act
            var nodeIdString1 = nodeId1.ToString();
            var nodeIdString2 = nodeId2.ToString();

            // Assert
            Assert.AreNotEqual(nodeIdString1, nodeIdString2);
        }

        [TestMethod]
        public void NodeId_GetDistance_ZeroForEqualNodes()
        {
            // Arrange 
            var expectedDistance = 0;
            var nodeId = new NodeId();

            // Act
            var nodeDistance = nodeId.GetDistance(nodeId);

            // Assert
            Assert.AreEqual(expectedDistance, nodeDistance);
        }

        [TestMethod]
        public void NodeId_GetDistance_NonZeroForDifferentNodes()
        {
            // Arrange 
            var expectedDistance = 0;
            var nodeId1 = new NodeId();
            var nodeId2 = new NodeId();

            // Act
            var nodeDistance = nodeId1.GetDistance(nodeId2);

            // Assert
            Assert.AreNotEqual(expectedDistance, nodeDistance);
        }

        [TestMethod]
        public void NodeId_GetDistance_DistanceSymmetric()
        {
            // Arrange 
            var nodeId1 = new NodeId();
            var nodeId2 = new NodeId();

            // Act
            var nodeDistance1 = nodeId1.GetDistance(nodeId2);
            var nodeDistance2 = nodeId2.GetDistance(nodeId1);

            // Assert
            Assert.AreEqual(nodeDistance1, nodeDistance2);
        }

        [TestMethod]
        public void NodeId_ToString_NotEmpty()
        {
            // Arrange 
            var nodeId = new NodeId();

            // Act
            var nodeIdString = nodeId.ToString();

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(nodeIdString));
        }

        [TestMethod]
        public void NodeId_ToString_DifferentForDifferentNodes()
        {
            // Arrange 
            var nodeId1 = new NodeId();
            var nodeId2 = new NodeId();

            // Act
            var nodeIdString1 = nodeId1.ToString();
            var nodeIdString2 = nodeId2.ToString();

            // Assert
            Assert.AreNotEqual(nodeIdString1, nodeIdString2);
        }
    }
}
