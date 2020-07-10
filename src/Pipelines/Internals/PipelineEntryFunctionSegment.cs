using System;

namespace Pipelines.Internals
{
    /// <summary>
    ///     First meaningful pipeline function segment.
    ///     Aggregates continuous delegates into a single delegate.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TNextRequest"></typeparam>
    /// <typeparam name="TNextResponse"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal sealed class PipelineEntryFunctionSegment<TRequest, TNextRequest, TNextResponse, TResponse> : PipelineSegment<TRequest, TNextRequest, TNextResponse, TResponse>
    {
        private readonly Segment<TRequest, TNextRequest, TNextResponse, TResponse> _middleware;

        public PipelineEntryFunctionSegment(
            Segment<TRequest, TNextRequest, TNextResponse, TResponse> middleware,
            IServiceResolver resolver
        ) : base(resolver)
            => _middleware = middleware;

        public override IPipelineFactory<TRequest, TResponse> OutputTo(Process<TNextRequest, TNextResponse> process)
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));

            return new PureFunctionPipelineFactory<TRequest, TResponse>(Segments.Connect(_middleware, process));
        }

        public override IPipelineBuilder<TRequest, TNextRequest, TNextResponse, TResponse> Use(Segment<TNextRequest, TNextResponse> segment)
        {
            if (segment == null)
                throw new ArgumentNullException(nameof(segment));

            return new PipelineEntryFunctionSegment<TRequest, TNextRequest, TNextResponse, TResponse>(
                Segments.Compose(_middleware, segment),
                Resolver
            );
        }

        public override IPipelineBuilder<TRequest, TExpectedRequest, TExpectedResponse, TResponse> Use<TExpectedRequest, TExpectedResponse>(
            Segment<TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse> segment
        )
        {
            if (segment == null)
                throw new ArgumentNullException(nameof(segment));

            return new PipelineEntryFunctionSegment<TRequest, TExpectedRequest, TExpectedResponse, TResponse>(
                Segments.Compose(_middleware, segment),
                Resolver
            );
        }

        public override Process<TRequest, TResponse> BuildPipeline(ServiceFactory factory, Process<TNextRequest, TNextResponse> nextProcess)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            if (nextProcess == null)
                throw new ArgumentNullException(nameof(nextProcess));

            return Segments.Connect(_middleware, nextProcess);
        }
    }
}
