using System.Collections.Generic;
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
    public class ExecutingPipelineWithFunctionProcess<TConfiguration, TRequest, TResponse, TStrategy>
        where TConfiguration : MiddlewareConfiguration<TRequest, TResponse>, new()
        where TStrategy : IResolveStrategy, new()
    {
        private static TConfiguration Config { get; } = new TConfiguration();

        public static IEnumerable<TRequest> ValidRequestItems
            => Config.ValidRequestItems;
        private Process<TRequest, TResponse> Pipeline { get; set; }

        [SetUp]
        public void Setup()
        {
            Pipeline = Config.Configure<TStrategy>()
                .OutputTo(command => new CommandProcessor().ExecuteAsync(command))
                .BuildPipeline(Resolver.Resolve);
        }

        [TestCaseSource(nameof(ValidRequestItems))]
        [Parallelizable]
        public void Must_return_task_with_correct_result_value(TRequest command)
        {
            object result = Pipeline(command);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf<Task<TResponse>>(result);
        }

        [TestCaseSource(nameof(ValidRequestItems))]
        [Parallelizable]
        public void Must_not_throw_on_pipeline_that_will_not_throw_exceptions(TRequest command)
            => Assert.DoesNotThrowAsync(async () => await Pipeline(command));

        [TestCaseSource(nameof(ValidRequestItems))]
        [Parallelizable]
        public async Task Must_not_return_null_results_when_results_should_not_be_null(TRequest command)
        {
            var result = await Pipeline(command);

            Assert.IsNotNull(result);
        }
    }
}
