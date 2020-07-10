namespace Pipelines
{
    public interface IPipelineFactory<in TRequest, TResponse>
    {
        Process<TRequest, TResponse> BuildPipeline(ServiceFactory factory);
    }
}
