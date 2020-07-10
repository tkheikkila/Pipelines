using System.Threading;
using System.Threading.Tasks;

namespace Pipelines.UnitTests.Mocks
{
    public class StringProcessor : IProcess<string, string>
    {
        public async Task<string> ExecuteAsync(string request, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return RepeatableValueGenerationMethods.StringToStringOperation(request);
        }
    }
}
