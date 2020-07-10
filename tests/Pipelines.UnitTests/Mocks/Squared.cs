namespace Pipelines.UnitTests.Mocks
{
    internal class Squared : Operation
    {
        protected override int Apply(int input)
            => input * input;
    }
}
