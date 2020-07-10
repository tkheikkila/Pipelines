using System.Collections.Generic;
using Pipelines.UnitTests.Configurations;
using Pipelines.UnitTests.Mocks;

namespace Pipelines.UnitTests
{
    public class ManyTypeChangingMixedFunctionAndServiceMiddleware : MiddlewareConfiguration<string, string>
    {
        public override IEnumerable<string> ValidRequestItems { get; } = ValidItems.Strings;
        protected override IPipelineBuilder<string, Command, Response, string> Configure(
            IPipelineBuilder<string, string, string, string> builder
        )
            => builder.Use<StringToCommandAdapter, Command, Response>()
                .Use<string, string>(
                    async (request, next, ct) => new Response
                    {
                        Value = await next(request.Value, ct)
                    }
                )
                .Use<StringToCommandAdapter, Command, Response>();
    }
}
