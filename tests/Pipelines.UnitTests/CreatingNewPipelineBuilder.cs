using NUnit.Framework;
using Pipelines.UnitTests.Mocks;

namespace Pipelines.UnitTests
{
    [TestFixture]
    public class CreatingNewPipelineBuilder
    {
        [Test]
        public void Must_not_throw()
            => Assert.DoesNotThrow(() => Pipeline.Of<Command, Response>());

        [Test]
        public void Must_not_return_null()
            => Assert.NotNull(Pipeline.Of<Command, Response>());
    }
}
