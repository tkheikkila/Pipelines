namespace Pipelines
{
    public interface IResolveStrategy
    {
        Segment<TRequest, TNextRequest, TNextResponse, TResponse> ResolveSegment<TSegment, TRequest, TNextRequest, TNextResponse, TResponse>(ServiceFactory factory)
            where TSegment : ISegment<TRequest, TNextRequest, TNextResponse, TResponse>;

        Process<TRequest, TResponse> ResolveProcess<TProcess, TRequest, TResponse>(ServiceFactory factory)
            where TProcess : IProcess<TRequest, TResponse>;
    }

    public sealed class EagerResolveStrategy : IResolveStrategy
    {
        public Segment<TRequest, TNextRequest, TNextResponse, TResponse> ResolveSegment<TSegment, TRequest, TNextRequest, TNextResponse, TResponse>(ServiceFactory factory)
            where TSegment : ISegment<TRequest, TNextRequest, TNextResponse, TResponse>
        {
            var middleware = factory.GetRequiredService<TSegment>();
            return (input, pipe, cancellationToken) => middleware.ExecuteAsync(input, pipe, cancellationToken);
        }

        public Process<TRequest, TResponse> ResolveProcess<TProcess, TRequest, TResponse>(ServiceFactory factory)
            where TProcess : IProcess<TRequest, TResponse>
            => factory.GetRequiredService<TProcess>()
                .ExecuteAsync;
    }

    public sealed class LazyResolveStrategy : IResolveStrategy
    {
        public Segment<TRequest, TNextRequest, TNextResponse, TResponse> ResolveSegment<TSegment, TRequest, TNextRequest, TNextResponse, TResponse>(ServiceFactory factory)
            where TSegment : ISegment<TRequest, TNextRequest, TNextResponse, TResponse>
            => (input, request, cancellationToken) => factory.GetRequiredService<TSegment>()
                .ExecuteAsync(input, request, cancellationToken);

        public Process<TRequest, TResponse> ResolveProcess<TProcess, TRequest, TResponse>(ServiceFactory factory)
            where TProcess : IProcess<TRequest, TResponse>
            => (input, cancellationToken) => factory.GetRequiredService<TProcess>()
                .ExecuteAsync(input, cancellationToken);
    }
}
