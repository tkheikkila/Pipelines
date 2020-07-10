using System.Threading.Tasks;
using NUnit.Framework;
using Pipelines.UnitTests.Mocks;

namespace Pipelines.UnitTests
{

    [TestFixture]
    [Parallelizable]
    public class PipelineWithSpecificExecutionOrder
    {
        [Test]
        [Parallelizable]
        public async Task Will_be_execute_function_middleware_layers_in_correct_order(
            [Random(10, Distinct = true)] int input
        )
        {
            var calculator = Pipeline.Of<int, int>()
                .Use((x, next) => next(x + 1))
                .Use((x, next) => next(x * 2))
                .OutputTo(x => x * x)
                .BuildPipeline(Resolver.Resolve);

            var expectedResult = (input + 1) * 2;
            expectedResult *= expectedResult;

            var reverseResult = input * input * 2 + 1;

            var result = await calculator(input);

            Assert.AreNotEqual(reverseResult, result, "The pipeline may have been resolved in the reverse order.");
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [Parallelizable]
        public async Task Will_be_execute_middleware_layers_in_correct_order([Random(10, Distinct = true)] int input)
        {
            var calculator = Pipeline.Of<int, int>()
                .Use<AddOne>()
                .Use<TimesTwo>()
                .OutputTo<Squared>()
                .BuildPipeline(Resolver.Resolve);

            var expectedResult = (input + 1) * 2;
            expectedResult *= expectedResult;

            var reverseResult = input * input * 2 + 1;

            var result = await calculator(input);

            Assert.AreNotEqual(reverseResult, result, "The pipeline may have been resolved in the reverse order.");
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [Parallelizable]
        public async Task Will_be_execute_mixed_middleware_layers_in_correct_order(
            [Random(10, Distinct = true)] int input
        )
        {
            var calculator = Pipeline.Of<int, int>()
                .Use<AddOne>()
                .Use((x, next) => next(x * 2))
                .Use<Squared>()
                .OutputTo(x => x)
                .BuildPipeline(Resolver.Resolve);

            var expectedResult = (input + 1) * 2;
            expectedResult *= expectedResult;

            var reverseResult = input * input * 2 + 1;

            var result = await calculator(input);

            Assert.AreNotEqual(reverseResult, result, "The pipeline may have been resolved in the reverse order.");
            Assert.AreEqual(expectedResult, result);
        }
    }

    [TestFixture]
    [Parallelizable]
    public class PipelineWithSameMiddlewareMultipleTimes
    {
        [Test]
        [Parallelizable]
        public async Task Will_execute_each_function_middleware_layer_exactly_once(
            [Random(
                0,
                10,
                5,
                Distinct = true
            )]
            int segmentCount,
            [Random(
                -10000,
                10000,
                5,
                Distinct = true
            )]
            int input
        )
        {
            IPipelineBuilder<int, int, int, int> builder = Pipeline.Of<int, int>();

            for (var i = 0; i < segmentCount; i++)
            {
                builder = builder.Use((x, next) => next(x + 1));
            }

            var calculator = builder.OutputTo(x => x)
                .BuildPipeline(Resolver.Resolve);

            var expectedResult = input + segmentCount;
            var result = await calculator(input);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [Parallelizable]
        public async Task Will_execute_each_middleware_layer_exactly_once(
            [Random(
                0,
                10,
                5,
                Distinct = false
            )]
            int segmentCount,
            [Random(
                -10000,
                10000,
                5,
                Distinct = true
            )]
            int input
        )
        {
            IPipelineBuilder<int, int, int, int> builder = Pipeline.Of<int, int>();

            for (var i = 0; i < segmentCount; i++)
            {
                builder = builder.Use<AddOne>();
            }

            var calculator = builder.OutputTo(x => x)
                .BuildPipeline(Resolver.Resolve);

            var expectedResult = input + segmentCount;
            var result = await calculator(input);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        [Parallelizable]
        public async Task Will_execute_each_mixed_middleware_layer_exactly_once(
            [Random(
                0,
                10,
                5,
                Distinct = false
            )]
            int segmentCount,
            [Random(
                -10000,
                10000,
                5,
                Distinct = true
            )]
            int input
        )
        {
            IPipelineBuilder<int, int, int, int> builder = Pipeline.Of<int, int>();

            for (var i = 0; i < segmentCount; i++)
            {
                builder = i % 3 == 0
                    ? builder.Use((x, next) => next(x + 1))
                    : builder.Use<AddOne>();
            }

            var calculator = builder.OutputTo(x => x)
                .BuildPipeline(Resolver.Resolve);

            var expectedResult = input + segmentCount;
            var result = await calculator(input);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
