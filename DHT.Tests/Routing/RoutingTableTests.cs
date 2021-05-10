﻿using DHT.Nodes;
using DHT.Routing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHT.Tests.Routing
{
    public class RoutingTableTests
    {
        private static Random random = new Random();

        private IRoutingTable routingTableMockHash;

        private IRoutingTable routingTableRealHash;

        [SetUp]
        public void TestInit()
        {
            // Setup routingTableMockHash
            var hashGenerator = new Mock<IConsistentHashGenerator>();
            hashGenerator
                .Setup(x => x.Hash(It.IsAny<string>()))
                .Returns(() => { return (UInt32)random.Next(); });

            this.routingTableMockHash = new RoutingTable(hashGenerator.Object);

            // Setup routingTableRealHash
            var realHashGenerator = new Sha256HashGenerator();
            this.routingTableRealHash = new RoutingTable(realHashGenerator);
        }

        [Test]
        public void RoutingTable_FindNode_FakeHash()
        {
            // Act Assert
            this.AssertFindNode(this.routingTableMockHash, 10, 100);
        }

        [Test]
        public void RoutingTable_FindNode_RealHash()
        {
            // Act Assert
            this.AssertFindNode(this.routingTableRealHash, 10, 100);
        }

        private void AssertFindNode(IRoutingTable routingTable, int randomNodes, int randomKeys)
        {
            Debug.WriteLine("--------");
            Debug.WriteLine("Test starting with {0} nodes and {1} keys", randomNodes, randomKeys);
            Debug.WriteLine("--------");
            Debug.WriteLine(string.Empty);

            // Arrange
            var nodes = this.GetRandomNodes(randomNodes);
            var freq = new Dictionary<NodeInfo, int>();
            routingTable.Nodes = nodes;

            foreach (var node in nodes)
            {
                freq.Add(node, 0);
            }

            var keys = this.GetRandomKeys(randomKeys);

            foreach (var key in keys)
            {
                // Act
                var partitionNode = routingTable.FindNode(key);

                // Assert
                Assert.IsNotNull(partitionNode);

                freq[partitionNode]++;
            }

            // Print frequency
            foreach (var freqKV in freq.OrderBy(n => n.Key.NodeId))
            {
                Debug.WriteLine("Node {0} selected {1}", freqKV.Key.NodeId, freqKV.Value);
            }

            Debug.WriteLine(string.Empty);
            Debug.WriteLine(string.Empty);
        }

        private IList<string> GetRandomKeys(int numberOfKeys)
        {
            var keys = new List<string>();

            for (int keyIdx = 0; keyIdx < numberOfKeys; keyIdx++)
            {
                var keyLength = random.Next(1, 10);
                var key = this.GetRandomString(keyLength);

                keys.Add(key);
            }

            return keys;
        }

        private string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            var randomChars = Enumerable
                .Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)])
                .ToArray();

            return new string(randomChars);
        }

        private IList<NodeInfo> GetRandomNodes(int numberOfNodes)
        {
            var nodes = new List<NodeInfo>();

            for (int nodeIdx = 0; nodeIdx < numberOfNodes; nodeIdx++)
            {
                var randomNodeId = (UInt32)random.Next(0, Int32.MaxValue);
                var nodeInfo = new NodeInfo()
                {
                    NodeId = randomNodeId
                };

                nodes.Add(nodeInfo);
            }

            return nodes;
        }
    }
}
