using System;
using System.Threading.Tasks;

namespace Pipelines
{
    public interface IPipelineBuilder<in TRequest, TNextRequest, TNextResponse, TResponse>
    {
        IPipelineFactory<TRequest, TResponse> OutputTo<TProcess>()
            where TProcess : IProcess<TNextRequest, TNextResponse>;

        IPipelineFactory<TRequest, TResponse> OutputTo<TProcess, TStrategy>()
            where TProcess : IProcess<TNextRequest, TNextResponse>
            where TStrategy : IResolveStrategy, new();

        IPipelineFactory<TRequest, TResponse> OutputTo(Process<TNextRequest, TNextResponse> process);

        IPipelineBuilder<TRequest, TExpectedRequest, TExpectedResponse, TResponse> Use<TSegment, TExpectedRequest, TExpectedResponse, TStrategy>()
            where TSegment : ISegment<TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse>
            where TStrategy : IResolveStrategy, new();

        IPipelineBuilder<TRequest, TExpectedRequest, TExpectedResponse, TResponse> Use<TAdapter, TExpectedRequest, TExpectedResponse>()
            where TAdapter : ISegment<TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse>;

        IPipelineBuilder<TRequest, TNextRequest, TNextResponse, TResponse> Use<TDecorator>()
            where TDecorator : ISegment<TNextRequest, TNextRequest, TNextResponse, TNextResponse>;

        IPipelineBuilder<TRequest, TNextRequest, TNextResponse, TResponse> Use<TDecorator, TStrategy>()
            where TDecorator : ISegment<TNextRequest, TNextRequest, TNextResponse, TNextResponse>
            where TStrategy : IResolveStrategy, new();

        IPipelineBuilder<TRequest, TNextRequest, TNextResponse, TResponse> Use(
            Segment<TNextRequest, TNextResponse> segment
        );

        IPipelineBuilder<TRequest, TExpectedRequest, TExpectedResponse, TResponse> Use<TExpectedRequest, TExpectedResponse>(
            Segment<TNextRequest, TExpectedRequest, TExpectedResponse, TNextResponse> segment
        );
    }

    public interface IPipelineBuilder<TRequest, TResponse> : IPipelineBuilder<TRequest, TRequest, TResponse, TResponse>
    {
    }

    public static class PipelineDecorationBuilderExtensions
    {
        public static IPipelineFactory<TRequest, TResponse> OutputTo<TRequest, TNextRequest, TNextResponse, TResponse>(
            this IPipelineBuilder<TRequest, TNextRequest, TNextResponse, TResponse> builder,
            Func<TNextRequest, TNextResponse> process
        )
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (process is null)
                throw new ArgumentNullException(nameof(process));

            return builder.OutputTo((request, ct) => Task.FromResult(process(request)));
        }

        public static IPipelineFactory<TRequest, TResponse> OutputTo<TRequest, TProcessRequest, TProcessResponse, TResponse>(
            this IPipelineBuilder<TRequest, TProcessRequest, TProcessResponse, TResponse> builder,
            Func<TProcessRequest, Task<TProcessResponse>> process
        )
        {
            if (builder is null)
                throw new ArgumentNullException(nameof(builder));

            if (process is null)
                throw new ArgumentNullException(nameof(process));

            return builder.OutputTo((request, ct) => process(request));
        }

        public static IPipelineBuilder<TRequest, TNextRequest, TNextResponse, TResponse> Use<TRequest, TNextRequest, TNextResponse, TResponse>(
            this IPipelineBuilder<TRequest, TNextRequest, TNextResponse, TResponse> segment,
            Func<TNextRequest, Process<TNextRequest, TNextResponse>, Task<TNextResponse>> decorator
        )
        {
            if (segment is null)
                throw new ArgumentNullException(nameof(segment));

            if (decorator is null)
                throw new ArgumentNullException(nameof(decorator));

            return segment.Use((request, next, ct) => decorator(request, next));
        }
    }
}
