namespace DHT
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a node in the DHT
    /// </summary>
    public class Node
    {
        private readonly Int32 nodeId;

        private IList<Node> nodes;

        private Dictionary<string, string> store;

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
        /// Store a key and a value
        /// </summary>
        public void StoreValue(string key, string value)
        {
            this.store.Add(key, value);
        }
    }
}
