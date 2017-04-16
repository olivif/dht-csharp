namespace DHT.NodesProtocol
{
    using System.Data.Linq;
    using System.Threading.Tasks;
    using ArgumentValidator;
    using Dhtproto;
    using Grpc.Core;
    using Nodes;
    using Routing;
    using Utils;
    using grpc = global::Grpc.Core;

    public class NodeServer : DhtProtoService.DhtProtoServiceBase
    {
        private readonly NodeInfo nodeInfo;

        private readonly NodeStore nodeStore;

        private readonly IRoutingTable routingTable;

        private readonly INodeServerClientFactory clientFactory;

        public NodeServer(NodeInfo nodeInfo, IRoutingTable routingTable, INodeServerClientFactory clientFactory)
        {
            Throw.IfNull(nodeInfo, nameof(nodeInfo));
            Throw.IfNull(routingTable, nameof(routingTable));
            Throw.IfNull(clientFactory, nameof(clientFactory));

            this.nodeInfo = nodeInfo;
            this.routingTable = routingTable;
            this.clientFactory = clientFactory;
            this.nodeStore = new NodeStore();
        }

        public override Task<KeyValueMessage> GetValue(KeyMessage request, grpc.ServerCallContext context)
        {
            KeyValueMessage response;
            NodeInfo remoteNode;

            if (this.IsLocalNode(request.Key, out remoteNode))
            {
                // Get it from the local store
                Logger.Log(this.nodeInfo, "GetValue", "Retrieving locally");

                if (!this.nodeStore.ContainsKey(request.Key))
                {
                    ThrowRpcException(StatusCode.NotFound, "Key not found");
                }
                var value = this.nodeStore.GetValue(request.Key);
                response = new KeyValueMessage()
                {
                    Key = request.Key,
                    Value = value
                };

            }
            else
            {
                // Get it remotely
                Logger.Log(this.nodeInfo, "GetValue", "Retrieving remotely");
                response = this.GetValueRemote(remoteNode, request.Key);
            }

            return Task.FromResult(response);
        }

        public override Task<KeyValueMessage> RemoveValue(KeyMessage request, ServerCallContext context)
        {
            Logger.Log(this.nodeInfo, "RemoveValue", "Start");

            // Find the node which should store this key, value
            KeyValueMessage response = null;
            var key = request.Key;
            var node = this.routingTable.FindNode(key);

            // If it's us, we should get it from the local store
            if (node.NodeId == this.nodeInfo.NodeId)
            {
                Logger.Log(this.nodeInfo, "RemoveValue", "Removing locally");
                var removed = this.nodeStore.RemoveValue(key);

                if (!removed)
                {
                    var status = new Status(StatusCode.NotFound, "Key not found, can't remove.");
                    throw new RpcException(status);
                }

                response = new KeyValueMessage()
                {
                    Key = key,
                    Value = removed ? "removed" : "not removed"
                };
            }
            else
            {
                // If it's not us, we ask that node to remove it remotely
                Logger.Log(this.nodeInfo, "RemoveValue", "Removing remotely");
                response = this.RemoveValueRemote(node, key);
            }

            return Task.FromResult(response);
        }

        public override Task<KeyValueMessage> StoreValue(KeyValueMessage request, grpc.ServerCallContext context)
        {
            Logger.Log(this.nodeInfo, "StoreValue", "Start");

            // Find the node which should store this key, value
            KeyValueMessage response = null;
            var key = request.Key;
            var value = request.Value;
            var node = this.routingTable.FindNode(key);

            // If it's us, we should store it in the local store
            if (node.NodeId == this.nodeInfo.NodeId)
            {
                Logger.Log(this.nodeInfo, "StoreValue", "Adding locally");

                try
                {
                    var added = this.nodeStore.AddValue(key, value);

                    if (!added)
                    {
                        var status = new Status(StatusCode.Internal, "Couldn't store value.");
                        throw new RpcException(status);
                    }
                }
                catch (DuplicateKeyException)
                {
                    var status = new Status(StatusCode.AlreadyExists, "Duplicate key found.");
                    throw new RpcException(status);
                }

                response = new KeyValueMessage()
                {
                    Key = key,
                    Value = value
                };
            }
            else
            {
                // If it's not us, we store in that node remotely
                Logger.Log(this.nodeInfo, "StoreValue", "Adding remotely");
                response = this.StoreValueRemote(node, key, value);
            }

            return Task.FromResult(response);
        }

        private KeyValueMessage GetValueRemote(NodeInfo node, string key)
        {
            var client = this.clientFactory.CreateRemoteClient(node);

            var request = new KeyMessage()
            {
                Key = key
            };

            var clientResponse = client.GetValue(request);

            return clientResponse;
        }

        private KeyValueMessage StoreValueRemote(NodeInfo node, string key, string value)
        {
            var client = this.clientFactory.CreateRemoteClient(node);

            var request = new KeyValueMessage()
            {
                Key = key,
                Value = value
            };

            var clientResponse = client.StoreValue(request);

            return clientResponse;
        }

        private KeyValueMessage RemoveValueRemote(NodeInfo node, string key)
        {
            var client = this.clientFactory.CreateRemoteClient(node);

            var request = new KeyMessage()
            {
                Key = key
            };

            var clientResponse = client.RemoveValue(request);

            return clientResponse;
        }

        private bool IsLocalNode(string key, out NodeInfo remoteNode)
        {
            // Find the node which should have this key
            remoteNode = this.routingTable.FindNode(key);

            // Return true if it's the local node
            var isLocal = remoteNode.NodeId == this.nodeInfo.NodeId;

            return isLocal;
        }

        private void ThrowRpcException(StatusCode statusCode, string message)
        {
            var status = new Status(statusCode, message);
            throw new RpcException(status);
        }
    }
}
