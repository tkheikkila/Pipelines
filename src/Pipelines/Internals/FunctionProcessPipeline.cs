namespace Pipelines.Internals
{
    /// <summary>
    ///     Pipeline configuration where the process itself is a pure function, but the tail contains 1..* service segments
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TProcessRequest"></typeparam>
    /// <typeparam name="TProcessResponse"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal sealed class FunctionProcessPipeline<TRequest, TProcessRequest, TProcessResponse, TResponse> : IPipelineFactory<TRequest, TResponse>
    {
        private readonly Process<TProcessRequest, TProcessResponse> _pipe;
        private readonly PipelineSegment<TRequest, TProcessRequest, TProcessResponse, TResponse> _tail;

        public FunctionProcessPipeline(Process<TProcessRequest, TProcessResponse> pipe, PipelineSegment<TRequest, TProcessRequest, TProcessResponse, TResponse> tail)
        {
            _pipe = pipe;
            _tail = tail;
        }

        public Process<TRequest, TResponse> BuildPipeline(ServiceFactory factory)
        {
            if (factory is null)
                throw new System.ArgumentNullException(nameof(factory));

            return _tail.BuildPipeline(factory, _pipe);
        }
    }
}
