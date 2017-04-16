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
    using static Dhtproto.DhtProtoService;

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
            var client = GetRandomClient();
            var request = new KeyMessage()
            {
                Key = args[0]
            };

            try
            {
                var response = client.GetValue(request);
                Console.WriteLine("Success: Got key value = {0} {1}", response.Key, response.Value);
            }
            catch (RpcException e)
            {
                Console.WriteLine("Error: {0} {1} ", e.Status.StatusCode, e.Status.Detail);
            }
        }

        private static void StoreValue(string command, IList<string> args)
        {
            var client = GetRandomClient();
            var request = new KeyValueMessage()
            {
                Key = args[0],
                Value = args[1]
            };

            try
            {
                var response = client.StoreValue(request);
                Console.WriteLine("Success: Got key value = {0} {1}", response.Key, response.Value);
            }
            catch (RpcException e)
            {
                Console.WriteLine("Error: {0} {1} ", e.Status.StatusCode, e.Status.Detail);
            }
        }

        private static void RemoveValue(string command, IList<string> args)
        {
            var client = GetRandomClient();
            var request = new KeyMessage()
            {
                Key = args[0]
            };

            try
            {
                var response = client.RemoveValue(request);
                Console.WriteLine("Success: Got key value = {0}", response.Key);
            }
            catch (RpcException e)
            {
                Console.WriteLine("Error: {0} {1} ", e.Status.StatusCode, e.Status.Detail);
            }
        }   

        private static NodeInfo GetRandomNode()
        {
            var randomNodeIdx = random.Next(0, routingTable.Nodes.Count);
            var randomNode = routingTable.Nodes[randomNodeIdx];

            return randomNode;
        }

        private static DhtProtoServiceClient GetRandomClient()
        {
            var node = GetRandomNode();

            var target = string.Format("{0}:{1}", node.HostName, node.Port);
            var channel = new Channel(target, ChannelCredentials.Insecure);
            var client = new DhtProtoServiceClient(channel);

            return client;
        }
    }
}
