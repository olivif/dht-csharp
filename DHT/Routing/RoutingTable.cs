namespace DHT.Routing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ArgumentValidator;
    using DHT.Nodes;

    /// <summary>
    /// Routing table implementation
    /// </summary>
    public class RoutingTable : IRoutingTable
    {
        private readonly IConsistentHashGenerator hashGenerator;

        public RoutingTable(IConsistentHashGenerator hashGenerator)
        {
            Throw.IfNull(hashGenerator, nameof(hashGenerator));

            this.hashGenerator = hashGenerator;
        }

        private IList<NodeInfo> SortedNodes
        {
            get
            {
                return this.Nodes.OrderBy(node => node.NodeId).ToList();
            }
        }

        public IList<NodeInfo> Nodes { get; set; }

        public NodeInfo FindNode(string key)
        {
            NodeInfo partitionNode;

            // Hash the key to get the "partition" key
            var partitionKey = this.hashGenerator.Hash(key);

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
                partitionNode = this.SortedNodes.Last();
            }

            return partitionNode;
        }
    }
}
