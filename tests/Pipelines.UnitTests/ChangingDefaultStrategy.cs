using NUnit.Framework;
using Pipelines.UnitTests.Mocks;

namespace Pipelines.UnitTests
{
    [TestFixture]
    public class ChangingDefaultStrategy
    {
        [Test]
        public void Must_not_throw()
            => Assert.DoesNotThrow(
                () => Pipeline.Of<Command, Response>()
                    .WithDefaultStrategy<LazyResolveStrategy>()
            );

        [Test]
        public void Must_not_throw_when_repeatedly_changing()
            => Assert.DoesNotThrow(
                () => Pipeline.Of<Command, Response>()
                    .WithDefaultStrategy<LazyResolveStrategy>()
                    .WithDefaultStrategy<EagerResolveStrategy>()
                    .WithDefaultStrategy<LazyResolveStrategy>()
            );

        [Test]
        public void Must_not_return_null()
            => Assert.NotNull(
                Pipeline.Of<Command, Response>()
                    .WithDefaultStrategy<LazyResolveStrategy>()
            );

        [Test]
        public void Must_not_return_null_when_repeatedly_changing()
            => Assert.NotNull(
                Pipeline.Of<Command, Response>()
                    .WithDefaultStrategy<LazyResolveStrategy>()
                    .WithDefaultStrategy<EagerResolveStrategy>()
                    .WithDefaultStrategy<LazyResolveStrategy>()
            );
    }
}
