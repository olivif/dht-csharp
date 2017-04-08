namespace DHT
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a node in the DHT
    /// </summary>
    public class Node
    {
        private readonly NodeId nodeId;

        private IList<Node> nodes;

        public NodeId NodeId
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
            this.nodeId = new NodeId();
            this.nodes = new List<Node>();
        }
    }
}
