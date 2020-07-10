using System.Collections.Generic;
using Pipelines.UnitTests.Mocks;

namespace Pipelines.UnitTests
{
    public abstract class MiddlewareConfiguration<TRequest, TResponse>
    {
        public abstract IEnumerable<TRequest> ValidRequestItems { get; }

        public IPipelineBuilder<TRequest, Command, Response, TResponse> Configure<TStrategy>()
            where TStrategy : IResolveStrategy, new()
            => Configure(Pipeline.Of<TRequest, TResponse>().WithDefaultStrategy<TStrategy>());
        protected abstract IPipelineBuilder<TRequest, Command, Response, TResponse> Configure(
            IPipelineBuilder<TRequest, TRequest, TResponse, TResponse> builder
        );
    }
}
