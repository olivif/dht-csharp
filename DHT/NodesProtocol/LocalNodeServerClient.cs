namespace DHT.NodesProtocol
{
    using DHT.Exceptions;
    using Dhtproto;
    using Grpc.Core;
    using Nodes;
    using System;

    public class LocalNodeServerClient : DhtProtoService.DhtProtoServiceClient
    {
        private readonly INodeStore nodeStore;

        public LocalNodeServerClient(INodeStore nodeStore)
        {
            this.nodeStore = nodeStore ?? throw new ArgumentNullException();
        }

        public LocalNodeServerClient()
            : this(new NodeStore())
        {
        }

        public override KeyValueMessage GetValue(KeyMessage request, CallOptions options)
        {
            if (!this.nodeStore.ContainsKey(request.Key))
            {
                ThrowRpcException(StatusCode.NotFound, "Key not found");
            }

            var value = this.nodeStore.GetValue(request.Key);
            var response = new KeyValueMessage()
            {
                Key = request.Key,
                Value = value
            };

            return response;
        }

        public override KeyMessage RemoveValue(KeyMessage request, CallOptions options)
        {
            var removed = this.nodeStore.RemoveValue(request.Key);

            if (!removed)
            {
                ThrowRpcException(StatusCode.NotFound, "Key not found, can't remove.");
            }

            var response = new KeyMessage()
            {
                Key = request.Key
            };

            return response;
        }

        public override KeyValueMessage StoreValue(KeyValueMessage request, CallOptions options)
        {
            try
            {
                var added = this.nodeStore.AddValue(request.Key, request.Value);

                if (!added)
                {
                    ThrowRpcException(StatusCode.Internal, "Couldn't store value.");
                }
            }
            catch (DuplicateKeyException)
            {
                ThrowRpcException(StatusCode.AlreadyExists, "Duplicate key found.");
            }

            return request;
        }

        private void ThrowRpcException(StatusCode statusCode, string message)
        {
            var status = new Status(statusCode, message);
            throw new RpcException(status);
        }
    }
}
