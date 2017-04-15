namespace DHT.Runner
{
    using System;
    using Grpc.Core;

    class Program
    {
        static void Main(string[] args)
        {
            var port = int.Parse(args[0]);

            var server = new Server
            {
                Services = { Dhtproto.DhtProtoService.BindService(new NodeServer()) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };

            server.Start();

            Console.WriteLine("NodeServer server listening on port " + port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadLine();

            server.ShutdownAsync().Wait();
        }
    }
}
