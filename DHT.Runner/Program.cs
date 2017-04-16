namespace DHT.Runner
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using Dhtproto;
    using Grpc.Core;
    using Nodes;
    using Routing;

    class Program
    {
        private static Random random = new Random();

        private static IRoutingTable routingTable = RoutingTableFactory.FromFile("routingTable.txt");

        private static IList<Process> processes = new List<Process>();
       
        static void Main(string[] args)
        {
            var commands = new Dictionary<string, Action<string>>()
            {
                { "help", Help },
                { "start", Start },
                { "stop", Stop },
            };

            // Main networking management loop
            while (true)
            {
                var line = Console.ReadLine();

                if (commands.ContainsKey(line))
                {
                    var command = commands[line];
                    command(line);
                }
                else
                {
                    Console.WriteLine("Unknown command");
                }
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

            processes.Add(p);
        }

        private static void Help(string command)
        {
            Console.WriteLine("Help command run");
        }

        private static void Start(string command)
        {
            // Start node servers
            var servers = new List<Server>();

            foreach (var node in routingTable.Nodes)
            {
                StartNodeServerProcess(node);
            }
        }

        private static void Stop(string command)
        {
            // Start node servers
            var servers = new List<Server>();

            foreach (var process in processes)
            {
                process.Kill();
            }

            processes.Clear();
        }
    }
}
