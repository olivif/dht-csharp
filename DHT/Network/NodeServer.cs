namespace DHT.Network
{
    using System.Threading.Tasks;
    using ArgumentValidator;
    using Dhtproto;
    using Nodes;
    using Routing;
    using grpc = global::Grpc.Core;

    public class NodeServer : DhtProtoService.DhtProtoServiceBase
    {
        private readonly NodeInfo nodeInfo;

        private readonly IRoutingTable routingTable;

        public NodeServer(NodeInfo nodeInfo, IRoutingTable routingTable)
        {
            Throw.IfNull(nodeInfo, nameof(nodeInfo));
            Throw.IfNull(routingTable, nameof(routingTable));

            this.nodeInfo = nodeInfo;
            this.routingTable = routingTable;
        }

        public override Task<StringMessage> SayHello(StringMessage request, grpc::ServerCallContext context)
        {
            var stringMessage = new StringMessage()
            {
                Message = "Received " + request.Message
            };

            return Task.FromResult<StringMessage>(stringMessage);
        }

        public override Task<KeyValueMessage> GetValue(KeyMessage request, grpc.ServerCallContext context)
        {
            // See if we have the value locally
            // Otherwise we find the node which should have the value and ask it
            return base.GetValue(request, context);
        }

        public override Task<KeyValueMessage> StoreValue(KeyValueMessage request, grpc.ServerCallContext context)
        {
            return base.StoreValue(request, context);
        }
    }
}
