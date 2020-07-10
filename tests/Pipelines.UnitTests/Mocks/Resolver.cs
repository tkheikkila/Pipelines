using System;

namespace Pipelines.UnitTests.Mocks
{
    public static class Resolver
    {
        public static object Resolve(Type type)
            => Activator.CreateInstance(type);
    }
}
