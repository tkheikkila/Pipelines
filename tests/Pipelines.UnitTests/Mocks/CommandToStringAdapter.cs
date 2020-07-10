using System.Threading;
using System.Threading.Tasks;

namespace Pipelines.UnitTests.Mocks
{
    public class CommandToStringAdapter : ISegment<Command, string, string, Response>
    {
        public async Task<Response> ExecuteAsync(
            Command request,
            Process<string, string> nextSegment,
            CancellationToken cancellationToken = default
        )
            => new Response
            {
                Value = await nextSegment(request.Value, cancellationToken)
            };
    }
}
