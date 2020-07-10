namespace Pipelines.UnitTests.Mocks
{
    internal class TimesTwo : Operation
    {
        protected override int Apply(int input)
            => input * 2;
    }
}
