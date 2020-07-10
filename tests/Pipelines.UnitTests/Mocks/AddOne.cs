namespace Pipelines.UnitTests.Mocks
{
    internal class AddOne : Operation
    {
        protected override int Apply(int input)
            => input + 1;
    }
}
