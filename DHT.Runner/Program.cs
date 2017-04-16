namespace DHT.Runner
{
    using System;
    using System.Collections.Generic;
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
            // Generate nodes info
            var nodes = GetRandomNodes(10);

            // Create routing table
            var hashGenerator = new Sha256HashGenerator();
            var routingTable = new RoutingTable(hashGenerator, nodes);

            // Start node servers
            var servers = new List<Server>();

            foreach(var node in nodes)
            {
                var nodeServer = StartNodeServer(node, routingTable);
                servers.Add(nodeServer);
            }

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
                var randomNodeId = random.Next();
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
    }
}
