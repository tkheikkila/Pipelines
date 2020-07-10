using System.Collections.Generic;
using Pipelines.UnitTests.Mocks;

namespace Pipelines.UnitTests.Configurations
{
    public static class ValidItems
    {
        public static IEnumerable<Command> Commands { get; } = GetCommands();

        public static IEnumerable<string> Strings { get; } = new[]
        {
            null,
            " \r\n\t",
            "value",
            "  trimmed value   "
        };

        private static IEnumerable<Command> GetCommands()
        {
            yield return new Command
            {
                Value = null
            };
            yield return new Command
            {
                Value = ""
            };
            yield return new Command
            {
                Value = " \r\n\t"
            };
            yield return new Command
            {
                Value = "value"
            };
            yield return new Command
            {
                Value = "  trimmed value   "
            };
        }
    }
}
