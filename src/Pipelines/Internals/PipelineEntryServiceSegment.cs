using System;

namespace Pipelines.Internals
{
    /// <summary>
    ///     First meaningful pipeline service segment.
    /// </summary>
    /// <typeparam name="TService"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TNextRequest"></typeparam>
    /// <typeparam name="TNextResponse"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    internal sealed class PipelineEntryServiceSegment<TService, TRequest, TNextRequest, TNextResponse, TResponse> : PipelineSegment<TRequest, TNextRequest, TNextResponse, TResponse>
        where TService : ISegment<TRequest, TNextRequest, TNextResponse, TResponse>
    {
        public PipelineEntryServiceSegment(IServiceResolver resolver) : base(resolver)
        {
        }

        public override Process<TRequest, TResponse> BuildPipeline(
            ServiceFactory factory,
            Process<TNextRequest, TNextResponse> nextProcess
        )
        {
            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            if (nextProcess == null)
                throw new ArgumentNullException(nameof(nextProcess));

            return Segments.Connect(
                Resolver.ResolveSegment<TService, TRequest, TNextRequest, TNextResponse, TResponse>(factory),
                nextProcess
            );
        }
    }
}
