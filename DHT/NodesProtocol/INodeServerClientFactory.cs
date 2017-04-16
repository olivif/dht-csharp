namespace DHT.NodesProtocol
{
    using DHT.Nodes;
    using static Dhtproto.DhtProtoService;

    public interface INodeServerClientFactory
    {
        DhtProtoServiceClient CreateClient(NodeInfo nodeInfo);
    }
}
