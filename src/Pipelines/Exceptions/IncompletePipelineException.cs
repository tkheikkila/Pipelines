using System;
using System.Runtime.Serialization;

namespace Pipelines.Exceptions
{
    [Serializable]
    public sealed class IncompletePipelineException : Exception
    {
        public IncompletePipelineException()
        {
        }

        private IncompletePipelineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public IncompletePipelineException(string message) : base(message)
        {
        }

        public IncompletePipelineException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
