﻿using System.Collections.Generic;
using Pipelines.UnitTests.Configurations;
using Pipelines.UnitTests.Mocks;

namespace Pipelines.UnitTests
{
    public class OneTypeChangingServiceMiddleware : MiddlewareConfiguration<string, string>
    {
        public override IEnumerable<string> ValidRequestItems { get; } = ValidItems.Strings;
        protected override IPipelineBuilder<string, Command, Response, string> Configure(IPipelineBuilder<string, string, string, string> builder)
            => builder.Use<StringToCommandAdapter, Command, Response>();
    }
}
