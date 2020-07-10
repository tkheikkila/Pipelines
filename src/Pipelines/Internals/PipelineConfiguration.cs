using System;

namespace Pipelines.Internals
{
    /// <summary>
    ///     Initial phase of a pipeline configuration
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <typeparam name="TDefault"></typeparam>
    internal class PipelineConfiguration<TRequest, TResponse, TDefault> : IPipelineConfiguration<TRequest, TResponse>
        where TDefault : IResolveStrategy, new()
    {
        private readonly IServiceResolver _resolver;

        public PipelineConfiguration()
            => _resolver = new ServiceResolver<TDefault>();

        public IPipelineFactory<TRequest, TResponse> OutputTo<TProcess>()
            where TProcess : IProcess<TRequest, TResponse>
            => new SingleProcessServiceFactory<TProcess, TRequest, TResponse>(_resolver.Next());

        public IPipelineFactory<TRequest, TResponse> OutputTo<TProcess, TStrategy>()
            where TProcess : IProcess<TRequest, TResponse>
            where TStrategy : IResolveStrategy, new()
            => new SingleProcessServiceFactory<TProcess, TRequest, TResponse>(_resolver.Next<TStrategy>());

        public IPipelineFactory<TRequest, TResponse> OutputTo(Process<TRequest, TResponse> process)
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));

            return new PureFunctionPipelineFactory<TRequest, TResponse>(process);
        }

        public IPipelineBuilder<TRequest, TExpectedRequest, TExpectedResponse, TResponse> Use<TSegment, TExpectedRequest, TExpectedResponse>()
            where TSegment : ISegment<TRequest, TExpectedRequest, TExpectedResponse, TResponse>
            => new PipelineEntryServiceSegment<TSegment, TRequest, TExpectedRequest, TExpectedResponse, TResponse>(_resolver.Next());

        public IPipelineBuilder<TRequest, TRequest, TResponse, TResponse> Use<TSegment>()
            where TSegment : ISegment<TRequest, TRequest, TResponse, TResponse>
            => new PipelineEntryServiceSegment<TSegment, TRequest, TRequest, TResponse, TResponse>(_resolver.Next());

        public IPipelineBuilder<TRequest, TExpectedRequest, TExpectedResponse, TResponse> Use<TExpectedRequest, TExpectedResponse>(
                Segment<TRequest, TExpectedRequest, TExpectedResponse, TResponse> segment
            )
        {
            if (segment == null)
                throw new ArgumentNullException(nameof(segment));

            return new PipelineEntryFunctionSegment<TRequest, TExpectedRequest, TExpectedResponse, TResponse>(
                segment,
                _resolver.Next()
            );
        }


        public IPipelineBuilder<TRequest, TRequest, TResponse, TResponse> Use<TSegment, TStrategy>()
            where TSegment : ISegment<TRequest, TRequest, TResponse, TResponse>
            where TStrategy : IResolveStrategy, new()
            => new PipelineEntryServiceSegment<TSegment, TRequest, TRequest, TResponse, TResponse>(_resolver.Next<TStrategy>());

        public IPipelineBuilder<TRequest, TRequest, TResponse, TResponse> Use(Segment<TRequest, TResponse> segment)
        {
            if (segment == null)
                throw new ArgumentNullException(nameof(segment));

            return new PipelineEntryFunctionSegment<TRequest, TRequest, TResponse, TResponse>(
                segment.ExpandTypeArguments(),
                _resolver.Next()
            );
        }

        public IPipelineBuilder<TRequest, TExpectedRequest, TExpectedResponse, TResponse> Use<TSegment, TExpectedRequest, TExpectedResponse, TStrategy>()
            where TSegment : ISegment<TRequest, TExpectedRequest, TExpectedResponse, TResponse>
            where TStrategy : IResolveStrategy, new()
            => new PipelineEntryServiceSegment<TSegment,TRequest,TExpectedRequest,TExpectedResponse,TResponse>(_resolver.Next());

        public IPipelineConfiguration<TRequest, TResponse> WithDefaultStrategy<TStrategy>()
            where TStrategy : IResolveStrategy, new()
            => new PipelineConfiguration<TRequest, TResponse, TStrategy>();
    }
}
