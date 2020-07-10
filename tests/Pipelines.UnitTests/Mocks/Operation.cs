using System.Threading;
using System.Threading.Tasks;

namespace Pipelines.UnitTests.Mocks
{
    internal abstract class Operation : ISegment<int, int>, IProcess<int, int>
    {
        public Task<int> ExecuteAsync(
            int request,
            Process<int, int> nextSegment,
            CancellationToken cancellationToken = default
        )
            => nextSegment(Apply(request), cancellationToken);

        public Task<int> ExecuteAsync(int request, CancellationToken cancellationToken = default)
            => Task.FromResult(Apply(request));

        protected abstract int Apply(int input);
    }
}
