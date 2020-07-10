using System;

namespace Pipelines.Internals
{
    /// <summary>
    ///     Pipeline configuration where the process is a service and the tail contains 1..* function or service segments
    /// </summary>
    /// <typeparam name="TProcess"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TProcessRequest"></typeparam>
    /// <typeparam name="TProcessResponse"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal sealed class ServiceProcessPipelineFactory<TProcess, TRequest, TProcessRequest, TProcessResponse, TResponse> : IPipelineFactory<TRequest, TResponse>
        where TProcess : IProcess<TProcessRequest, TProcessResponse>
    {
        private readonly IServiceResolver _resolver;
        private readonly PipelineSegment<TRequest, TProcessRequest, TProcessResponse, TResponse> _tail;

        public ServiceProcessPipelineFactory(IServiceResolver resolver, PipelineSegment<TRequest, TProcessRequest, TProcessResponse, TResponse> tail)
        {
            _resolver = resolver;
            _tail = tail;
        }

        public Process<TRequest, TResponse> BuildPipeline(ServiceFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return _tail.BuildPipeline(factory, _resolver.ResolvePipe<TProcess, TProcessRequest, TProcessResponse>(factory));
        }
    }
}
