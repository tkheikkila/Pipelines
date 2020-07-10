﻿using System.Collections.Generic;
using Pipelines.UnitTests.Configurations;
using Pipelines.UnitTests.Mocks;

namespace Pipelines.UnitTests
{
    public class ManyFunctionMiddleware : MiddlewareConfiguration<Command, Response>
    {
        public override IEnumerable<Command> ValidRequestItems { get; } = ValidItems.Commands;

        protected override IPipelineBuilder<Command, Command, Response, Response> Configure(
            IPipelineBuilder<Command, Command, Response, Response> builder
        )
            => builder.Use(
                (request, next, ct) =>
                {
                    request.Value = "1";
                    return next(request, ct);
                }
            ).Use(
                (request, next, ct) =>
                {
                    request.Value = "2";
                    return next(request, ct);
                }
            ).Use(
                (request, next, ct) =>
                {
                    request.Value = "3";
                    return next(request, ct);
                }
            );
    }
}
