namespace DHT.Runner
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Dhtproto;
    using Grpc.Core;
    using Network;
    using Nodes;
    using Routing;

    class Program
    {
        private static Random random = new Random();

        static void Main(string[] args)
        {
            // Create routing table
            var routingTable = RoutingTableFactory.FromFile("routingTable.txt");

            // Start node servers
            var servers = new List<Server>();

            foreach(var node in routingTable.Nodes)
            {
                StartNodeServerProcess(node);
            }
        }

        private static KeyValueMessage GetValueRemote(NodeInfo node, string key)
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

        private static void StartNodeServerProcess(NodeInfo nodeInfo)
        {
            Process p = new Process();
            p.StartInfo.FileName = "DHT.Network.NodeRunner.exe";
            p.StartInfo.Arguments = nodeInfo.NodeId.ToString();
            p.Start();
        }
    }
}
