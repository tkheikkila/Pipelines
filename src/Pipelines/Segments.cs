using System.Threading;
using System.Threading.Tasks;

namespace Pipelines
{
    public delegate Task<TResponse> Process<in TRequest, TResponse>(
        TRequest request,
        CancellationToken cancellation = default
    );

    public delegate Task<TResponse> Segment<TRequest, TResponse>(
        TRequest request,
        Process<TRequest, TResponse> nextSegment,
        CancellationToken cancellationToken = default
    );

    public delegate Task<TResponse> Segment<in TRequest, out TNextRequest, TNextResponse, TResponse>(
        TRequest request,
        Process<TNextRequest, TNextResponse> nextSegment,
        CancellationToken cancellationToken = default
    );

    public static class Segments
    {
        public static Segment<TRequest, TRequest, TResponse, TResponse> ExpandTypeArguments<TRequest, TResponse>(
            this Segment<TRequest, TResponse> nextSegment
        )
            => (request, futureSegment, cancellationToken) => nextSegment(request, futureSegment, cancellationToken);

        public static Segment<TRequest, TExpectedRequest, TExpectedResponse, TResponse> Compose<TRequest, TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse, TResponse>(
            Segment<TRequest, TNextRequest, TNextResponse, TResponse> middleware,
            Segment<TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse> nextSegment
        )
            => (request, futureSegment, cancellationToken) => middleware(request, Connect(nextSegment, futureSegment), cancellationToken);

        public static Segment<TRequest, TNextRequest, TNextResponse, TResponse> Compose<TRequest, TNextRequest, TNextResponse, TResponse>(
            Segment<TRequest, TNextRequest, TNextResponse, TResponse> middleware,
            Segment<TNextRequest, TNextResponse> nextSegment
        )
            => (request, futureSegment, cancellationToken) => middleware(request, Connect(nextSegment, futureSegment), cancellationToken);

        public static Process<TRequest, TResponse> Connect<TRequest, TResponse>(
            Segment<TRequest, TResponse> current,
            Process<TRequest, TResponse> process
        )
            => (request, cancellationToken) => current(request, process, cancellationToken);

        public static Process<TRequest, TResponse> Connect<TRequest, TNextRequest, TNextResponse, TResponse>(
            Segment<TRequest, TNextRequest, TNextResponse, TResponse> middleware,
            Process<TNextRequest, TNextResponse> process
        )
            => (request, cancellationToken) => middleware(request, process, cancellationToken);
    }
}
