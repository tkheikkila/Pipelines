namespace Pipelines
{
    public interface IPipelineConfiguration<TRequest, TResponse> : IPipelineBuilder<TRequest, TResponse>
    {
        IPipelineConfiguration<TRequest, TResponse> WithDefaultStrategy<T>()
            where T : IResolveStrategy, new();
    }
}
