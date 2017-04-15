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

        private Dictionary<string, string> store;

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
            this.store = new Dictionary<string, string>();
            this.hashGenerator = new Sha256HashGenerator();
        }

        /// <summary>
        /// Checks if the value associated with this key is stored locally
        /// </summary>
        public bool HasKey(string key)
        {
            return this.store.ContainsKey(key);
        }

        /// <summary>
        /// Gets a value using the key
        /// </summary>
        public string GetValue(string key)
        {
            string value;
            this.store.TryGetValue(key, out value);

            return value;
        }

        /// <summary>
        /// Store a value
        /// </summary>
        public void StoreValue(string key, string value)
        {
            this.store.Add(key, value);
        }

        /// <summary>
        /// Find the node which should store this key
        /// </summary>
        public Node FindNode(string key)
        {
            var hashKey = this.hashGenerator.Hash(key);

            // The node which stores this value should be the one with the smallest
            // key below the key generated.
            var sortedNodes = this.nodes.OrderBy(node => node.NodeId).ToList();

            var nodeToStore = sortedNodes.FindLast(node => node.nodeId < hashKey);

            return nodeToStore;
        }
    }
}
