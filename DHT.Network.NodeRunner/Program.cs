namespace DHT.Network.NodeRunner
{
    using System;
    using System.Linq;
    using DHT.Nodes;
    using DHT.Routing;
    using Grpc.Core;
    using NodesProtocol;

    public class Program
    {
        static void Main(string[] args)
        {
            // Create routing table
            var routingTable = RoutingTableFactory.FromFile("routingTable.txt");

            // Get node to run
            var nodeId = UInt32.Parse(args[0]);
            var nodeInfo = routingTable.Nodes.FirstOrDefault(n => n.NodeId == nodeId);

            // Start that node 
            var server = StartNodeServer(nodeInfo, routingTable);

            // Wait for input before killing it
            Console.ReadKey();

            KillNodeServer(server);
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

            Console.WriteLine("NodeServer server killed");
            Console.WriteLine();
        }
    }
}
