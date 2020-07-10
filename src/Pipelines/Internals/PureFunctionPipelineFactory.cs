using System;

namespace Pipelines.Internals
{
    /// <summary>
    ///     Pipeline configuration where the entire pipeline is a pure function
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal sealed class PureFunctionPipelineFactory<TRequest, TResponse> : IPipelineFactory<TRequest, TResponse>
    {
        private readonly Process<TRequest, TResponse> _pipe;

        public PureFunctionPipelineFactory(Process<TRequest, TResponse> pipe)
            => _pipe = pipe;

        public Process<TRequest, TResponse> BuildPipeline(ServiceFactory factory)
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            return _pipe;
        }
    }
}
