namespace DHT.NodesProtocol
{
    using DHT.Nodes;
    using Dhtproto;
    using Grpc.Core;

    class NodeServerClientFactory : INodeServerClientFactory
    {
        public DhtProtoService.DhtProtoServiceClient CreateClient(NodeInfo nodeInfo)
        {
            var target = string.Format("{0}:{1}", nodeInfo.HostName, nodeInfo.Port);
            var channel = new Channel(target, ChannelCredentials.Insecure);
            var client = new DhtProtoService.DhtProtoServiceClient(channel);

            return client;
        }
    }
}
