using System.Collections.Generic;
using Pipelines.UnitTests.Configurations;
using Pipelines.UnitTests.Mocks;

namespace Pipelines.UnitTests
{
    public class ManyServiceMiddleware : MiddlewareConfiguration<Command, Response>
    {
        public override IEnumerable<Command> ValidRequestItems { get; } = ValidItems.Commands;
        protected override IPipelineBuilder<Command, Command, Response, Response> Configure(IPipelineBuilder<Command, Command, Response, Response> builder)
            => builder.Use<CommandResponseDecorator>()
                .Use<CommandResponseDecorator>()
                .Use<CommandResponseDecorator>()
                .Use<CommandResponseDecorator>();
    }
}
