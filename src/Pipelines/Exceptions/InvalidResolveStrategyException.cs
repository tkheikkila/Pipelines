using System;
using System.Runtime.Serialization;

namespace Pipelines.Exceptions
{
    [Serializable]
    public sealed class InvalidResolveStrategyException : Exception
    {
        public IResolveStrategy? ResolveStrategy { get; }

        public InvalidResolveStrategyException(IResolveStrategy strategy)
            => ResolveStrategy = strategy;

        private InvalidResolveStrategyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public InvalidResolveStrategyException()
        {
        }

        public InvalidResolveStrategyException(string message) : base(message)
        {
        }

        public InvalidResolveStrategyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
