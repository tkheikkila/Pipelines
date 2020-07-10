using System;

namespace Pipelines.Internals
{
    /// <summary>
    ///     Pipeline function segment with 1..* service segments and 0..* function segments earlier in the pipeline.
    ///     Aggregates continuous delegates into a single delegate.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TSegmentRequest"></typeparam>
    /// <typeparam name="TNextRequest"></typeparam>
    /// <typeparam name="TNextResponse"></typeparam>
    /// <typeparam name="TSegmentResponse"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal class FunctionSegment<TRequest, TSegmentRequest, TNextRequest, TNextResponse, TSegmentResponse, TResponse> : PipelineSegment<TRequest, TNextRequest, TNextResponse, TResponse>
    {
        private readonly Segment<TSegmentRequest, TNextRequest, TNextResponse, TSegmentResponse> _middleware;
        private readonly PipelineSegment<TRequest, TSegmentRequest, TSegmentResponse, TResponse> _tail;

        public FunctionSegment(
            Segment<TSegmentRequest, TNextRequest, TNextResponse, TSegmentResponse> middleware,
            IServiceResolver resolver,
            PipelineSegment<TRequest, TSegmentRequest, TSegmentResponse, TResponse> tail
        ) : base(resolver)
        {
            _middleware = middleware;
            _tail = tail;
        }

        public override Process<TRequest, TResponse> BuildPipeline(
            ServiceFactory factory,
            Process<TNextRequest, TNextResponse> nextProcess
        )
            => _tail.BuildPipeline(factory, Segments.Connect(_middleware, nextProcess));

        public override IPipelineBuilder<TRequest, TNextRequest, TNextResponse, TResponse> Use(
            Segment<TNextRequest, TNextResponse> segment
        )
        {
            if (segment == null)
                throw new ArgumentNullException(nameof(segment));

            return new FunctionSegment<TRequest, TSegmentRequest, TNextRequest, TNextResponse, TSegmentResponse, TResponse>(
                Segments.Compose(_middleware, segment),
                Resolver.Next(),
                _tail
            );
        }

        public override IPipelineBuilder<TRequest, TExpectedRequest, TExpectedResponse, TResponse>
            Use<TExpectedRequest, TExpectedResponse>(
                Segment<TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse> segment
            )
        {
            if (segment == null)
                throw new ArgumentNullException(nameof(segment));

            return new FunctionSegment<TRequest, TSegmentRequest, TExpectedRequest, TExpectedResponse, TSegmentResponse, TResponse>(
                Segments.Compose(_middleware, segment),
                Resolver.Next(),
                _tail
            );
        }

        public override IPipelineFactory<TRequest, TResponse> OutputTo(
            Process<TNextRequest, TNextResponse> process
        )
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));

            return new FunctionProcessPipeline<TRequest, TSegmentRequest, TSegmentResponse, TResponse>(
                Segments.Connect(_middleware, process),
                _tail
            );
        }
    }
}
