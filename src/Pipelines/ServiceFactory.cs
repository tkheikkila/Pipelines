using System;
using Pipelines.Exceptions;

namespace Pipelines
{
    public delegate object? ServiceFactory(Type type);

    public static class ServiceFactoryExtensions
    {
        public static T GetService<T>(this ServiceFactory factory)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));
#nullable disable
            return factory(typeof(T)) switch
            {
                T service => service,
                _ => default
            };
#nullable restore
        }

        public static T GetRequiredService<T>(this ServiceFactory factory)
        {
            if (factory is null)
                throw new ArgumentNullException(nameof(factory));

            return factory.GetService<T>() ?? throw new UnresolvedDependencyException(typeof(T));
        }
    }
}
