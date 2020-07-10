using System;

namespace Pipelines.Internals
{
    /// <summary>
    ///     Pipeline service segment with 1..* function or service segments
    /// </summary>
    /// <typeparam name="TSegment"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TNextRequest"></typeparam>
    /// <typeparam name="TExpectedRequest"></typeparam>
    /// <typeparam name="TExpectedResponse"></typeparam>
    /// <typeparam name="TNextResponse"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal class ServiceSegment<TSegment, TRequest, TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse, TResponse>
        : PipelineSegment<TRequest, TExpectedRequest, TExpectedResponse, TResponse>
        where TSegment : ISegment<TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse>
    {
        private readonly PipelineSegment<TRequest, TNextRequest, TNextResponse, TResponse> _tail;

        public ServiceSegment(
            IServiceResolver strategy,
            PipelineSegment<TRequest, TNextRequest, TNextResponse, TResponse> tail
        ) : base(strategy)
            => _tail = tail;

        public override Process<TRequest, TResponse> BuildPipeline(
            ServiceFactory factory,
            Process<TExpectedRequest, TExpectedResponse> nextProcess
        )
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            if (nextProcess == null)
                throw new ArgumentNullException(nameof(nextProcess));
            
            return _tail.BuildPipeline(
                factory,
                Segments.Connect(
                    Resolver.ResolveSegment<TSegment, TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse>(
                        factory
                    ),
                    nextProcess
                )
            );
        }
    }
}
