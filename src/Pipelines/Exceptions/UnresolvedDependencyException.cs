using System;
using System.Runtime.Serialization;

namespace Pipelines.Exceptions
{
    [Serializable]
    public sealed class UnresolvedDependencyException : Exception
    {
        public Type? DependencyType { get; }

        public UnresolvedDependencyException(Type type)
            => DependencyType = type;

        private UnresolvedDependencyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public UnresolvedDependencyException()
        {
        }

        public UnresolvedDependencyException(string message) : base(message)
        {
        }

        public UnresolvedDependencyException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
