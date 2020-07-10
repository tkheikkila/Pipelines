using System.Threading;
using System.Threading.Tasks;

namespace Pipelines
{
    public interface IProcess<in TRequest, TResponse>
    {
        Task<TResponse> ExecuteAsync(TRequest request, CancellationToken cancellationToken = default);
    }
}
