namespace DHT.Routing
{
    using System.Collections.Generic;
    using DHT.Nodes;

    /// <summary>
    /// Routing interface
    /// </summary>
    public interface IRoutingTable
    {
        /// <summary>
        /// List of node info in the network
        /// </summary>
        IList<NodeInfo> Nodes { get; set; }
    }
}
