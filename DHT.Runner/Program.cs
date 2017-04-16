namespace DHT.Runner
{
    using System;
    using System.Collections.Generic;
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
            var routingTable = ReadRoutingTable("routingTable.txt");

            // Start node servers
            var servers = new List<Server>();

            foreach(var node in routingTable.Nodes)
            {
                var nodeServer = StartNodeServer(node, routingTable);
                servers.Add(nodeServer);
            }

            // Make a get request to one of the servers
            var testNode = routingTable.Nodes.First();

            GetValueRemote(testNode, "A");

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadLine();

            foreach (var nodeServer in servers)
            {
                KillNodeServer(nodeServer);
            }

            Console.WriteLine("All done.");
        }

        private static IList<NodeInfo> GetRandomNodes(int numberOfNodes)
        {
            var nodes = new List<NodeInfo>();
            var port = 11000;

            for (int nodeIdx = 0; nodeIdx < numberOfNodes; nodeIdx++)
            {
                var randomNodeId = (UInt32)random.Next(0, Int32.MaxValue);
                var nodeInfo = new NodeInfo()
                {
                    NodeId = randomNodeId,
                    HostName = "localhost",
                    Port = port++
                };

                nodes.Add(nodeInfo);
            }

            return nodes;
        }

        private static Server StartNodeServer(NodeInfo node, IRoutingTable routingTable)
        {
            var nodeServer = new NodeServer(node, routingTable);

            var server = new Server
            {
                Services = { Dhtproto.DhtProtoService.BindService(nodeServer) },
                Ports = { new ServerPort(node.HostName, node.Port, ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine("NodeServer server listening on {0}:{1} ", node.HostName, node.Port);
            Console.WriteLine();

            return server;
        }

        private static void KillNodeServer(Server nodeServer)
        {
            nodeServer.ShutdownAsync().Wait();
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

        private static IRoutingTable ReadRoutingTable(string routingTablePath)
        {
            var lines = File.ReadAllLines(routingTablePath);
            var nodes = new List<NodeInfo>();

            foreach(var line in lines)
            {
                var tokens = line.Split(' ');
                var nodeId = UInt32.Parse(tokens[0]);
                var hostName = tokens[1];
                var port = int.Parse(tokens[2]);

                var nodeInfo = new NodeInfo()
                {
                    NodeId = nodeId,
                    HostName = hostName,
                    Port = port
                };

                nodes.Add(nodeInfo);
            }

            var hashGenerator = new Sha256HashGenerator();
            var routingTable = new RoutingTable(hashGenerator, nodes);

            return routingTable;
        }
    }
}
