using System;

namespace Pipelines.Internals
{
    /// <summary>
    ///     Pipeline with no pipeline segments, just the service processor
    /// </summary>
    /// <typeparam name="TProcess"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal sealed class SingleProcessServiceFactory<TProcess, TRequest, TResponse>
        : IPipelineFactory<TRequest, TResponse>
        where TProcess : IProcess<TRequest, TResponse>
    {
        private readonly IServiceResolver _resolver;

        public SingleProcessServiceFactory(IServiceResolver strategy)
            => _resolver = strategy;

        public Process<TRequest, TResponse> BuildPipeline(ServiceFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return _resolver.ResolvePipe<TProcess, TRequest, TResponse>(factory);
        }
    }
}
