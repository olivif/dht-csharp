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

        public Int32 NodeId
        {
            get { return this.nodeId; }
        }

        public IList<Node> Nodes
        {
            get { return this.nodes; }
            set { this.nodes = value; }
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
        }
    }
}
