using System;
using System.Threading.Tasks;
using NUnit.Framework;
using Pipelines.UnitTests.Mocks;

namespace Pipelines.UnitTests
{

    [TestFixture(TypeArgs = new []{ typeof(NoMiddleware), typeof(Command), typeof(Response), typeof(EagerResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(NoMiddleware), typeof(Command), typeof(Response), typeof(LazyResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(OneFunctionMiddleware), typeof(Command), typeof(Response), typeof(EagerResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(OneFunctionMiddleware), typeof(Command), typeof(Response), typeof(LazyResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(OneServiceMiddleware), typeof(Command), typeof(Response), typeof(EagerResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(OneServiceMiddleware), typeof(Command), typeof(Response), typeof(LazyResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(ManyFunctionMiddleware), typeof(Command), typeof(Response), typeof(EagerResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(ManyFunctionMiddleware), typeof(Command), typeof(Response), typeof(LazyResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(ManyServiceMiddleware), typeof(Command), typeof(Response), typeof(EagerResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(ManyServiceMiddleware), typeof(Command), typeof(Response), typeof(LazyResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(MixedFunctionAndServiceMiddleware), typeof(Command), typeof(Response), typeof(EagerResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(MixedFunctionAndServiceMiddleware), typeof(Command), typeof(Response), typeof(LazyResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(OneTypeChangingFunctionMiddleware), typeof(string), typeof(string), typeof(EagerResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(OneTypeChangingFunctionMiddleware), typeof(string), typeof(string), typeof(LazyResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(OneTypeChangingServiceMiddleware), typeof(string), typeof(string), typeof(EagerResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(OneTypeChangingServiceMiddleware), typeof(string), typeof(string), typeof(LazyResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(ManyTypeChangingFunctionMiddleware), typeof(string), typeof(string), typeof(EagerResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(ManyTypeChangingFunctionMiddleware), typeof(string), typeof(string), typeof(LazyResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(ManyTypeChangingServiceMiddleware), typeof(string), typeof(string), typeof(EagerResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(ManyTypeChangingServiceMiddleware), typeof(string), typeof(string), typeof(LazyResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(ManyTypeChangingMixedFunctionAndServiceMiddleware), typeof(string), typeof(string), typeof(EagerResolveStrategy) })]
    [TestFixture(TypeArgs = new []{ typeof(ManyTypeChangingMixedFunctionAndServiceMiddleware), typeof(string), typeof(string), typeof(LazyResolveStrategy) })]
    public class ConfigurationWithStrategy<TConfiguration, TRequest, TResponse, TStrategy>
        where TConfiguration : MiddlewareConfiguration<TRequest, TResponse>, new()
        where TStrategy : IResolveStrategy, new()
    {
        private IPipelineBuilder<TRequest, Command, Response, TResponse> Builder { get; set; }

        [SetUp]
        public void Setup()
            => Builder = new TConfiguration().Configure<TStrategy>();

        [Test]
        public void Must_not_throw_on_valid_processor()
            => Assert.DoesNotThrow(() => Builder.OutputTo<CommandProcessor>());

        [Test]
        public void Must_not_throw_on_valid_synchronous_function_processor()
            => Assert.DoesNotThrow(
                () => Builder.OutputTo(
                    async command => new Response
                    {
                        Value = await Task.FromResult(command.Value?.Trim() ?? "")
                    }
                )
            );

        [Test]
        public void Must_not_throw_on_valid_asynchronous_function_processor()
            => Assert.DoesNotThrow(
                () => Builder.OutputTo(
                    async command => new Response
                    {
                        Value = await Task.FromResult(command.Value?.Trim() ?? "")
                    }
                )
            );

        [Test]
        public void Must_not_throw_on_valid_middleware_with_same_pipeline_type_argument_expectations()
            => Assert.DoesNotThrow(() => Builder.Use<CommandResponseDecorator>());

        [Test]
        public void Must_not_throw_on_valid_function_middleware_with_same_pipeline_type_argument_expectations()
            => Assert.DoesNotThrow(
                () => Builder.Use(
                    (arg, next, ct) =>
                    {
                        arg.Id = Guid.NewGuid();
                        return next(arg, ct);
                    }
                )
            );

        [Test]
        public void Must_not_throw_on_valid_middleware_with_different_pipeline_type_argument_expectations()
            => Assert.DoesNotThrow(() => Builder.Use<CommandToStringAdapter, string, string>());

        [Test]
        public void Must_not_throw_on_valid_function_middleware_with_different_pipeline_type_argument_expectations()
            => Assert.DoesNotThrow(
                () => Builder.Use<string, string>(
                    async (arg, next, ct) => new Response
                    {
                        Value = await next(arg.Value, ct)
                    }
                )
            );
    }
}
