// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: dht.proto
#region Designer generated code

using System;
using System.Threading;
using System.Threading.Tasks;
using grpc = global::Grpc.Core;

namespace Dhtproto {
  /// <summary>
  /// Interface exported by the server.
  /// </summary>
  public static partial class DhtProtoService
  {
    static readonly string __ServiceName = "dhtproto.DhtProtoService";

    static readonly grpc::Marshaller<global::Dhtproto.StringMessage> __Marshaller_StringMessage = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Dhtproto.StringMessage.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Dhtproto.KeyMessage> __Marshaller_KeyMessage = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Dhtproto.KeyMessage.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Dhtproto.KeyValueMessage> __Marshaller_KeyValueMessage = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Dhtproto.KeyValueMessage.Parser.ParseFrom);

    static readonly grpc::Method<global::Dhtproto.StringMessage, global::Dhtproto.StringMessage> __Method_SayHello = new grpc::Method<global::Dhtproto.StringMessage, global::Dhtproto.StringMessage>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SayHello",
        __Marshaller_StringMessage,
        __Marshaller_StringMessage);

    static readonly grpc::Method<global::Dhtproto.KeyMessage, global::Dhtproto.KeyValueMessage> __Method_GetValue = new grpc::Method<global::Dhtproto.KeyMessage, global::Dhtproto.KeyValueMessage>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetValue",
        __Marshaller_KeyMessage,
        __Marshaller_KeyValueMessage);

    static readonly grpc::Method<global::Dhtproto.KeyValueMessage, global::Dhtproto.KeyValueMessage> __Method_StoreValue = new grpc::Method<global::Dhtproto.KeyValueMessage, global::Dhtproto.KeyValueMessage>(
        grpc::MethodType.Unary,
        __ServiceName,
        "StoreValue",
        __Marshaller_KeyValueMessage,
        __Marshaller_KeyValueMessage);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Dhtproto.DhtReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of DhtProtoService</summary>
    public abstract partial class DhtProtoServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Dhtproto.StringMessage> SayHello(global::Dhtproto.StringMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Dhtproto.KeyValueMessage> GetValue(global::Dhtproto.KeyMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Dhtproto.KeyValueMessage> StoreValue(global::Dhtproto.KeyValueMessage request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for DhtProtoService</summary>
    public partial class DhtProtoServiceClient : grpc::ClientBase<DhtProtoServiceClient>
    {
      /// <summary>Creates a new client for DhtProtoService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public DhtProtoServiceClient(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for DhtProtoService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public DhtProtoServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected DhtProtoServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected DhtProtoServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Dhtproto.StringMessage SayHello(global::Dhtproto.StringMessage request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return SayHello(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Dhtproto.StringMessage SayHello(global::Dhtproto.StringMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SayHello, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Dhtproto.StringMessage> SayHelloAsync(global::Dhtproto.StringMessage request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return SayHelloAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Dhtproto.StringMessage> SayHelloAsync(global::Dhtproto.StringMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SayHello, null, options, request);
      }
      public virtual global::Dhtproto.KeyValueMessage GetValue(global::Dhtproto.KeyMessage request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return GetValue(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Dhtproto.KeyValueMessage GetValue(global::Dhtproto.KeyMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetValue, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Dhtproto.KeyValueMessage> GetValueAsync(global::Dhtproto.KeyMessage request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return GetValueAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Dhtproto.KeyValueMessage> GetValueAsync(global::Dhtproto.KeyMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetValue, null, options, request);
      }
      public virtual global::Dhtproto.KeyValueMessage StoreValue(global::Dhtproto.KeyValueMessage request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return StoreValue(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Dhtproto.KeyValueMessage StoreValue(global::Dhtproto.KeyValueMessage request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_StoreValue, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Dhtproto.KeyValueMessage> StoreValueAsync(global::Dhtproto.KeyValueMessage request, grpc::Metadata headers = null, DateTime? deadline = null, CancellationToken cancellationToken = default(CancellationToken))
      {
        return StoreValueAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Dhtproto.KeyValueMessage> StoreValueAsync(global::Dhtproto.KeyValueMessage request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_StoreValue, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override DhtProtoServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new DhtProtoServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(DhtProtoServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_SayHello, serviceImpl.SayHello)
          .AddMethod(__Method_GetValue, serviceImpl.GetValue)
          .AddMethod(__Method_StoreValue, serviceImpl.StoreValue).Build();
    }

  }
}
#endregion
