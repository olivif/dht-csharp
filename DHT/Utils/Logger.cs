namespace DHT.Utils
{
    using System;
    using DHT.Nodes;

    public static class Logger
    {
        public static void Log(NodeInfo nodeInfo, string method, string message)
        {
            Console.WriteLine("[{0}][{1}][{2}:{3}] {4}", 
                DateTime.UtcNow.ToString(),
                nodeInfo.NodeId, 
                nodeInfo.HostName, 
                nodeInfo.Port, 
                message);
        }
    }
}
