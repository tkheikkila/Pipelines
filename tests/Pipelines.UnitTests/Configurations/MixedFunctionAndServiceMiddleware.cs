using System.Collections.Generic;
using Pipelines.UnitTests.Configurations;
using Pipelines.UnitTests.Mocks;

namespace Pipelines.UnitTests
{
    public class MixedFunctionAndServiceMiddleware : MiddlewareConfiguration<Command, Response>
    {
        public override IEnumerable<Command> ValidRequestItems { get; } = ValidItems.Commands;

        protected override IPipelineBuilder<Command, Command, Response, Response> Configure(
            IPipelineBuilder<Command, Command, Response, Response> builder
        )
            => builder.Use(
                    (request, next, ct) =>
                    {
                        request.Value = "";
                        return next(request, ct);
                    }
                )
                .Use<CommandResponseDecorator>()
                .Use(
                    (request, next, ct) =>
                    {
                        request.Value = "value";
                        return next(request, ct);
                    }
                )
                .Use<CommandResponseDecorator>();
    }
}
