namespace DHT.Network
{
    using System;
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

        public NodeServer(NodeInfo nodeInfo, IRoutingTable routingTable)
        {
            Throw.IfNull(nodeInfo, nameof(nodeInfo));
            Throw.IfNull(routingTable, nameof(routingTable));

            this.nodeInfo = nodeInfo;
            this.routingTable = routingTable;
            this.nodeStore = new NodeStore();
        }

        public override Task<KeyValueMessage> GetValue(KeyMessage request, grpc.ServerCallContext context)
        {
            Logger.Log(this.nodeInfo, "GetValue", "Start");

            // Find the node which should store this key, value
            KeyValueMessage response = null;
            var key = request.Key;
            var node = this.routingTable.FindNode(key);

            // If it's us, we should get it from the local store
            if (node.NodeId == this.nodeInfo.NodeId)
            {
                var value = string.Empty;

                Logger.Log(this.nodeInfo, "GetValue", "Retrieving locally");

                if (this.nodeStore.ContainsKey(key))
                {
                    value = this.nodeStore.GetValue(key);
                } 

                response = new KeyValueMessage()
                {
                    Key = key,
                    Value = value
                };
            }
            else
            {
                // If it's not us, we ask that node remotely
                Logger.Log(this.nodeInfo, "GetValue", "Retrieving remotely");
                response = this.GetValueRemote(node, key);
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
                this.nodeStore.AddValue(key, value);
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
            var target = string.Format("{0}:{1}", node.HostName, node.Port);
            var channel = new Channel(target, ChannelCredentials.Insecure);
            var client = new DhtProtoService.DhtProtoServiceClient(channel);

            var request = new KeyMessage()
            {
                Key = key
            };

            var clientResponse = client.GetValue(request);

            return clientResponse;
        }

        private KeyValueMessage StoreValueRemote(NodeInfo node, string key, string value)
        {
            var target = string.Format("{0}:{1}", node.HostName, node.Port);
            var channel = new Channel(target, ChannelCredentials.Insecure);
            var client = new DhtProtoService.DhtProtoServiceClient(channel);

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
            var target = string.Format("{0}:{1}", node.HostName, node.Port);
            var channel = new Channel(target, ChannelCredentials.Insecure);
            var client = new DhtProtoService.DhtProtoServiceClient(channel);

            var request = new KeyMessage()
            {
                Key = key
            };

            var clientResponse = client.RemoveValue(request);

            return clientResponse;
        }
    }
}
