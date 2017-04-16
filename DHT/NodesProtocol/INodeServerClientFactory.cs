namespace DHT.NodesProtocol
{
    using DHT.Nodes;
    using static Dhtproto.DhtProtoService;

    public interface INodeServerClientFactory
    {
        DhtProtoServiceClient CreateRemoteClient(NodeInfo nodeInfo);

        DhtProtoServiceClient CreateLocalClient();
    }
}
