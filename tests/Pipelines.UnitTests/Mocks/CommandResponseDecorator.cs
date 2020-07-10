using System;
using System.Threading;
using System.Threading.Tasks;

namespace Pipelines.UnitTests.Mocks
{
    public class CommandResponseDecorator : ISegment<Command, Response>
    {
        public async Task<Response> ExecuteAsync(
            Command request,
            Process<Command, Response> nextSegment,
            CancellationToken cancellationToken = default
        )
        {
            request.Id = Guid.NewGuid();
            var response = await nextSegment(request, cancellationToken);
            response.Id = Guid.NewGuid();
            response.Origin = request;
            return response;
        }
    }
}
