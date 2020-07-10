using System;

namespace Pipelines.Internals
{
    /// <summary>
    /// Default rules for process decoration
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TNextRequest"></typeparam>
    /// <typeparam name="TNextResponse"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal abstract class PipelineSegment<TRequest, TNextRequest, TNextResponse, TResponse> : IPipelineBuilder<TRequest, TNextRequest, TNextResponse, TResponse>
    {
        protected IServiceResolver Resolver { get; }

        protected PipelineSegment(IServiceResolver strategy)
            => Resolver = strategy;

        public IPipelineFactory<TRequest, TResponse> OutputTo<TProcess>()
            where TProcess : IProcess<TNextRequest, TNextResponse>
            => new ServiceProcessPipelineFactory<TProcess, TRequest, TNextRequest, TNextResponse, TResponse>(Resolver.Next(), this);

        public IPipelineFactory<TRequest, TResponse> OutputTo<TProcess, TStrategy>()
            where TProcess : IProcess<TNextRequest, TNextResponse>
            where TStrategy : IResolveStrategy, new()
            => new ServiceProcessPipelineFactory<TProcess, TRequest, TNextRequest, TNextResponse, TResponse>(Resolver.Next<TStrategy>(), this);

        public virtual IPipelineFactory<TRequest, TResponse> OutputTo(Process<TNextRequest, TNextResponse> process)
        {
            if (process == null)
                throw new ArgumentNullException(nameof(process));

            return new FunctionProcessPipeline<TRequest, TNextRequest, TNextResponse, TResponse>(process, this);
        }

        public IPipelineBuilder<TRequest, TExpectedRequest, TExpectedResponse, TResponse> Use<TSegment, TExpectedRequest, TExpectedResponse>()
            where TSegment : ISegment<TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse>
            => new ServiceSegment<TSegment,TRequest,TNextRequest,TExpectedRequest,TExpectedResponse,TNextResponse,TResponse>(Resolver.Next(), this);

        public IPipelineBuilder<TRequest, TNextRequest, TNextResponse, TResponse> Use<TSegment>()
            where TSegment : ISegment<TNextRequest, TNextRequest, TNextResponse, TNextResponse>
            => new ServiceSegment<TSegment, TRequest, TNextRequest, TNextRequest, TNextResponse, TNextResponse, TResponse>(Resolver.Next(), this);

        public IPipelineBuilder<TRequest, TNextRequest, TNextResponse, TResponse> Use<TSegment, TStrategy>()
            where TSegment : ISegment<TNextRequest, TNextRequest, TNextResponse, TNextResponse>
            where TStrategy : IResolveStrategy, new()
            => new ServiceSegment<TSegment, TRequest, TNextRequest, TNextRequest, TNextResponse, TNextResponse, TResponse>(Resolver.Next<TStrategy>(), this);

        public virtual IPipelineBuilder<TRequest, TNextRequest, TNextResponse, TResponse> Use(Segment<TNextRequest, TNextResponse> segment)
        {
            if (segment == null)
                throw new ArgumentNullException(nameof(segment));

            return new FunctionSegment<TRequest, TNextRequest, TNextRequest, TNextResponse, TNextResponse, TResponse>(
                segment.ExpandTypeArguments(),
                Resolver.Next(),
                this
            );
        }

        public virtual IPipelineBuilder<TRequest, TExpectedRequest, TExpectedResponse, TResponse> Use<TExpectedRequest, TExpectedResponse>(Segment<TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse> segment)
        {
            if (segment == null)
                throw new ArgumentNullException(nameof(segment));

            return new FunctionSegment<TRequest, TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse, TResponse>(segment, Resolver.Next(), this);
        }

        public IPipelineBuilder<TRequest, TExpectedRequest, TExpectedResponse, TResponse> Use<TSegment, TExpectedRequest, TExpectedResponse, TStrategy>()
            where TSegment : ISegment<TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse>
            where TStrategy : IResolveStrategy, new()
            => new ServiceSegment<TSegment, TRequest, TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse, TResponse>(Resolver.Next<TStrategy>(), this);

        public abstract Process<TRequest, TResponse> BuildPipeline(
            ServiceFactory factory,
            Process<TNextRequest, TNextResponse> nextProcess
        );
    }
}
