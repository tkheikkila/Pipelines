using Pipelines.Internals;

namespace Pipelines
{
    public static class Pipeline
    {
        public static IPipelineConfiguration<TRequest, TResponse> Of<TRequest, TResponse>()
            => new PipelineConfiguration<TRequest, TResponse, EagerResolveStrategy>();
    }
}
