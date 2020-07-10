using System.Collections.Generic;
using Pipelines.UnitTests.Configurations;
using Pipelines.UnitTests.Mocks;

namespace Pipelines.UnitTests
{
    public class OneTypeChangingFunctionMiddleware : MiddlewareConfiguration<string, string>
    {
        public override IEnumerable<string> ValidRequestItems { get; } = ValidItems.Strings;
        protected override IPipelineBuilder<string, Command, Response, string> Configure(
            IPipelineBuilder<string, string, string, string> builder
        )
            => builder.Use<Command, Response>(
                async (request, next, ct) =>
                {
                    var response = await next(
                        new Command
                        {
                            Value = request
                        },
                        ct
                    );
                    return response.Value;
                }
            );
    }
}
