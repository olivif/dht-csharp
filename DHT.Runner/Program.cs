namespace DHT.Runner
{
    using System;
    using Dhtproto;
    using Grpc.Core;
    using Network;

    class Program
    {
        static void Main(string[] args)
        {
            var port = int.Parse(args[0]);

            var server = new Server
            {
                Services = { Dhtproto.DhtProtoService.BindService(new NodeServer(new Nodes.NodeInfo(), new Routing.RoutingTable())) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine("NodeServer server listening on port " + port);
            Console.WriteLine();

            var channel = new Channel(string.Format("127.0.0.1:{0}", port), ChannelCredentials.Insecure);
            var client = new DhtProtoService.DhtProtoServiceClient(channel);

            var request = new StringMessage()
            {
                Message = "Hello!"
            };
            var response = client.SayHello(request);
            Console.WriteLine("Sent {0} Server replied with {1} ", request.Message, response.Message);

            Console.WriteLine("Press any key to stop the server...");
            Console.ReadLine();

            server.ShutdownAsync().Wait();
        }
    }
}
