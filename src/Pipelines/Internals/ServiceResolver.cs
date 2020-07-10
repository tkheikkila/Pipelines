
using System;

namespace Pipelines.Internals
{
    internal interface IServiceResolver
    {
        public IServiceResolver Next<TStrategy>()
            where TStrategy : IResolveStrategy, new();

        public IServiceResolver Next();

        Process<TRequest, TResponse> ResolvePipe<TProcess, TRequest, TResponse>(ServiceFactory factory)
            where TProcess : IProcess<TRequest, TResponse>;

        Segment<TRequest, TNextRequest, TNextResponse, TResponse> ResolveSegment<TSegment, TRequest, TNextRequest, TNextResponse, TResponse>(ServiceFactory factory)
            where TSegment : ISegment<TRequest, TNextRequest, TNextResponse, TResponse>;
    }

    /// <summary>
    ///     Wrapper for resolve strategies to allow for simpler defaults
    /// </summary>
    /// <typeparam name="TDefault"></typeparam>
    internal class ServiceResolver<TDefault> : IServiceResolver
        where TDefault : IResolveStrategy, new()
    {
        private readonly IResolveStrategy _strategy;

        public ServiceResolver() : this(new TDefault())
        {
        }

        public ServiceResolver(IResolveStrategy strategy)
            => _strategy = strategy;

        public IServiceResolver Next<TStrategy>()
            where TStrategy : IResolveStrategy, new()
            => new ServiceResolver<TDefault>(new TStrategy());

        public IServiceResolver Next()
            => new ServiceResolver<TDefault>();

        public Process<TRequest, TResponse> ResolvePipe<TProcess, TRequest, TResponse>(ServiceFactory factory)
            where TProcess : IProcess<TRequest, TResponse>
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return _strategy.ResolveProcess<TProcess, TRequest, TResponse>(factory);
        }

        public Segment<TRequest, TNextRequest, TNextResponse, TResponse> ResolveSegment<TSegment, TRequest, TNextRequest, TNextResponse, TResponse>(
            ServiceFactory factory
        )
            where TSegment : ISegment<TRequest, TNextRequest, TNextResponse, TResponse>
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return _strategy.ResolveSegment<TSegment, TRequest, TNextRequest, TNextResponse, TResponse>(factory);
        }
    }
}
