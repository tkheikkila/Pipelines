using System.Threading;
using System.Threading.Tasks;

namespace Pipelines
{
    public interface ISegment<in TRequest, out TNextRequest, TNextResponse, TResponse>
    {
        Task<TResponse> ExecuteAsync(
            TRequest request,
            Process<TNextRequest, TNextResponse> nextSegment,
            CancellationToken cancellationToken = default
        );
    }

    public interface ISegment<TRequest, TResponse> : ISegment<TRequest, TRequest, TResponse, TResponse>
    {
    }
}
