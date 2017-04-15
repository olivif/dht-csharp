namespace DHT
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Represents a node in the DHT
    /// </summary>
    public class Node
    {
        private readonly Int32 nodeId;

        private IList<Node> nodes;

        private Dictionary<Int32, string> store;

        private IConsistentHashGenerator hashGenerator;

        public Int32 NodeId
        {
            get { return this.nodeId; }
        }

        public IList<Node> Nodes
        {
            get { return this.nodes; }
        }

        /// <summary>
        /// Creates a node with a random id
        /// </summary>
        public Node()
        {
            var randomSeed = Guid.NewGuid().GetHashCode();
            var random = new Random(randomSeed);
            this.nodeId = random.Next(Int32.MinValue, Int32.MaxValue);
            this.nodes = new List<Node>();
            this.store = new Dictionary<Int32, string>();
            this.hashGenerator = new Sha256HashGenerator();
        }

        /// <summary>
        /// Gets a value using the key
        /// </summary>
        public string GetValue(Int32 key)
        {
            string value;
            this.store.TryGetValue(key, out value);

            return value;
        }

        /// <summary>
        /// Store a value
        /// </summary>
        public void StoreValue(string value)
        {
            var key = this.hashGenerator.Hash(value);
            this.store.Add(key, value);
        }

        public Node FindNode(string value)
        {
            var key = this.hashGenerator.Hash(value);

            // The node which stores this value should be the one with the smallest
            // key below the key generated.
            var sortedNodes = this.nodes.OrderBy(node => node.NodeId).ToList();

            var nodeToStore = sortedNodes.FindLast(node => node.nodeId < key);

            return nodeToStore;
        }
    }
}
