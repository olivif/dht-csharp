namespace DHT.Routing
{
    using System.Collections.Generic;
    using DHT.Nodes;

    /// <summary>
    /// Routing table implementation
    /// </summary>
    public class RoutingTable : IRoutingTable
    {
        public IList<NodeInfo> Nodes { get; set; }
    }
}
