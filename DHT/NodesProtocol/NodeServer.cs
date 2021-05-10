namespace DHT.NodesProtocol
{
    using System;
    using System.Threading.Tasks;
    using Dhtproto;
    using Grpc.Core;
    using Nodes;
    using Routing;
    using static Dhtproto.DhtProtoService;
    using grpc = global::Grpc.Core;

    public class NodeServer : DhtProtoServiceBase
    {
        private readonly NodeInfo nodeInfo;

        private readonly IRoutingTable routingTable;

        private readonly INodeServerClientFactory clientFactory;

        private readonly DhtProtoServiceClient localClient;

        public NodeServer(NodeInfo nodeInfo, IRoutingTable routingTable, INodeServerClientFactory clientFactory)
        {
            this.nodeInfo = nodeInfo ?? throw new ArgumentNullException();
            this.routingTable = routingTable ?? throw new ArgumentNullException();
            this.clientFactory = clientFactory ?? throw new ArgumentNullException();
            this.localClient = clientFactory.CreateLocalClient();
        }

        public override Task<KeyValueMessage> GetValue(KeyMessage request, ServerCallContext context)
        {
            var client = this.GetClient(request.Key);
            var response = client.GetValue(request);

            return Task.FromResult(response);
        }

        public override Task<KeyMessage> RemoveValue(KeyMessage request, ServerCallContext context)
        {
            var client = this.GetClient(request.Key);
            var response = client.RemoveValue(request);

            return Task.FromResult(response);
        }

        public override Task<KeyValueMessage> StoreValue(KeyValueMessage request, grpc.ServerCallContext context)
        {
            var client = this.GetClient(request.Key);
            var response = client.StoreValue(request);

            return Task.FromResult(response);
        }

        private DhtProtoServiceClient GetClient(string key)
        {
            // Find the node which should have this key
            var remoteNode = this.routingTable.FindNode(key);

            // Return true if it's the local node
            var isLocal = remoteNode.NodeId == this.nodeInfo.NodeId;

            if (isLocal)
            {
                return this.localClient;
            }

            return this.clientFactory.CreateRemoteClient(remoteNode);
        }
    }
}
