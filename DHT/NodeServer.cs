namespace DHT
{
    using System.Threading.Tasks;
    using Dhtproto;
    using grpc = global::Grpc.Core;

    public class NodeServer : DhtProtoService.DhtProtoServiceBase
    {
        public override Task<StringMessage> SayHello(StringMessage request, grpc::ServerCallContext context)
        {
            var stringMessage = new StringMessage()
            {
                Message = "Received " + request.Message
            };

            return Task.FromResult<StringMessage>(stringMessage);
        }
    }
}
