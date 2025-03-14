using Grpc.Core;

namespace TheMover.API.Services;

/// <summary>Basic gRPC service for debugging purposes</summary>
public class GreeterService : Greeter.GreeterBase {
    /// <summary>Basic RPC for debug purposes</summary>
    /// <param name="request">The request data, that the caller provided</param>
    /// <param name="context">Context about the request</param>
    /// <returns>A response that contains the greeting</returns>
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context) => Task.FromResult(new HelloReply { Message = $"Hello {request.Name}" });
}
