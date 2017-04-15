namespace DHT
{
    using System.Threading.Tasks;
    using Dhtproto;
    using grpc = global::Grpc.Core;

    public class NodeServer : DhtProtoService.DhtProtoServiceBase
    {
        private Node node;

        public NodeServer()
        {
            this.node = new Node();
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
