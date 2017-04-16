namespace DHT.Runner
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
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
            var commands = new Dictionary<string, Action<string, IList<string>>>()
            {
                { "help", Help },
                { "start", Start },
                { "stop", Stop },
                { "getValue", GetValue },
                { "storeValue", StoreValue },
                { "removeValue", RemoveValue },
            };

            // Main networking management loop
            while (true)
            {
                var line = Console.ReadLine();
                var lineArgs = line.Split(' ').ToList();
                var command = lineArgs[0];

                lineArgs.RemoveAt(0);

                if (commands.ContainsKey(command))
                {
                    var commandFunc = commands[command];
                    commandFunc(command, lineArgs);
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

        private static void Help(string command, IList<string> args)
        {
            Console.WriteLine("Help command run");
        }

        private static void Start(string command, IList<string> args)
        {
            // Start node servers
            var servers = new List<Server>();

            foreach (var node in routingTable.Nodes)
            {
                StartNodeServerProcess(node);
            }
        }

        private static void Stop(string command, IList<string> args)
        {
            // Start node servers
            var servers = new List<Server>();

            foreach (var process in processes)
            {
                process.Kill();
            }

            processes.Clear();
        }

        private static void GetValue(string command, IList<string> args)
        {
            Console.WriteLine("GetValue");
        }

        private static void StoreValue(string command, IList<string> args)
        {
            Console.WriteLine("StoreValue");
        }

        private static void RemoveValue(string command, IList<string> args)
        {
            Console.WriteLine("RemoveValue");
        }
    }
}
