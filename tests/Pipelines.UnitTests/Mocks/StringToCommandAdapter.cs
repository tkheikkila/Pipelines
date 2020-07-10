using System.Threading;
using System.Threading.Tasks;

namespace Pipelines.UnitTests.Mocks
{
    public class StringToCommandAdapter : ISegment<string, Command, Response, string>
    {
        public async Task<string> ExecuteAsync(string request, Process<Command, Response> nextSegment, CancellationToken cancellationToken = default)
        {
            var response = await nextSegment(
                new Command
                {
                    Value = request
                },
                cancellationToken
            );
            return response.Value;
        }
    }
}
