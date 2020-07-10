using System;

namespace Pipelines.UnitTests.Mocks
{
    public class Response
    {
        public Guid Id { get; set; }
        public Command Origin { get; set; }
        public string Value { get; set; }
    }
}
