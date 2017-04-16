namespace DHT.Network
{
    using System.Collections.Generic;
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

        private readonly IDictionary<string, string> store;

        public NodeServer(NodeInfo nodeInfo, IRoutingTable routingTable)
        {
            Throw.IfNull(nodeInfo, nameof(nodeInfo));
            Throw.IfNull(routingTable, nameof(routingTable));

            this.nodeInfo = nodeInfo;
            this.routingTable = routingTable;
            this.store = new Dictionary<string, string>();
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
            var key = request.Key;
            var value = string.Empty;

            // See if we have the value locally
            if (this.store.TryGetValue(key, out value))
            {
                // Found value locally, we can return
                var response = new KeyValueMessage()
                {
                    Key = key,
                    Value = value
                };

                return Task.FromResult(response);
            }

            // Otherwise we find the node which should have the value and ask it


            return base.GetValue(request, context);
        }

        public override Task<KeyValueMessage> StoreValue(KeyValueMessage request, grpc.ServerCallContext context)
        {
            return base.StoreValue(request, context);
        }
    }
}
