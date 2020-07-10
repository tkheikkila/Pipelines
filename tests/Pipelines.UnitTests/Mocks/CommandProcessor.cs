using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pipelines.UnitTests.Mocks
{
    public class CommandProcessor : IProcess<Command, Response>
    {
        public async Task<Response> ExecuteAsync(Command request, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            return new Response
            {
                Id = Guid.NewGuid(),
                Origin = request,
                Value = RepeatableValueGenerationMethods.StringToStringOperation(request.Value)
            };
        }
    }
}
