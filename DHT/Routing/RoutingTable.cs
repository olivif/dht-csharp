namespace DHT.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DHT.Nodes;
    using Utils;

    /// <summary>
    /// Routing table implementation
    /// </summary>
    public class RoutingTable : IRoutingTable
    {
        private IList<NodeInfo> nodes;

        private readonly IConsistentHashGenerator hashGenerator;

        public RoutingTable(IConsistentHashGenerator hashGenerator)
            : this(hashGenerator, new List<NodeInfo>())
        {
        }

        public RoutingTable(IConsistentHashGenerator hashGenerator, IList<NodeInfo> nodes)
        {
            this.nodes = nodes ?? throw new ArgumentNullException();
            this.hashGenerator = hashGenerator ?? throw new ArgumentNullException();
        }

        private IList<NodeInfo> SortedNodes
        {
            get
            {
                return this.Nodes.OrderBy(node => node.NodeId).ToList();
            }
        }

        public IList<NodeInfo> Nodes
        {
            get
            {
                return this.nodes;
            }
            set
            {
                this.nodes = value;
            }
        }

        public NodeInfo FindNode(string key)
        {
            NodeInfo partitionNode;

            // Hash the key to get the "partition" key
            var partitionKey = this.hashGenerator.Hash(key);

            Logger.Log("RoutingTable - FindNode", "key = " + key);
            Logger.Log("RoutingTable - FindNode", "partitionKey = " + partitionKey);

            // Now find the last node which has an id smaller than the
            // partition key. We also make sure that the nodes are sorted,
            // this might be overkill but unless we have millions of nodes
            // it should be OK performance wise.
            partitionNode = this.SortedNodes.LastOrDefault(n => n.NodeId <= partitionKey);

            // If we haven't found any node, we'll give the load to the last node.
            // If the node ids aren't evenly distributed we could be in trouble
            // since the last node might get overloaded.
            if (partitionNode == null)
            {
                Logger.Log("RoutingTable - FindNode", "Didn't find node, reverting to last");
                partitionNode = this.SortedNodes.Last();
            }

            Logger.Log("RoutingTable - FindNode", "FoundNode = " + partitionNode.NodeId);

            return partitionNode;
        }
    }
}
