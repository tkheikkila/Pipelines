using System;
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
    public class ResolveFunctionProcessorWithStrategy<TConfiguration, TRequest, TResponse, TStrategy>
        where TConfiguration : MiddlewareConfiguration<TRequest, TResponse>, new()
        where TStrategy : IResolveStrategy, new()
    {
        private IPipelineFactory<TRequest, TResponse> Pipeline { get; set; }

        [SetUp]
        public void Setup()
            => Pipeline = new TConfiguration().Configure<TStrategy>()
                .OutputTo(command => new CommandProcessor().ExecuteAsync(command));

        [Test]
        public void Must_resolve_with_a_valid_resolver_without_throwing()
            => Assert.DoesNotThrow(() => Pipeline.BuildPipeline(Resolver.Resolve));

        [Test]
        public void Must_not_resolve_into_a_null()
        {
            var pipeline = Pipeline.BuildPipeline(Resolver.Resolve);
            Assert.IsNotNull(pipeline);
        }

        [Test]
        public void Must_throw_ArgumentNullException_on_null_argument()
            => Assert.Throws<ArgumentNullException>(() => Pipeline.BuildPipeline(null));
    }
}
