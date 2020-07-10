namespace Pipelines.UnitTests.Mocks
{
    public static class RepeatableValueGenerationMethods
    {
        public static string StringToStringOperation(string value)
        {
            if (value is null)
                return "<null>";
            else if (string.IsNullOrEmpty(value))
                return "<empty>";
            else if (string.IsNullOrWhiteSpace(value))
                return "<whitespace>";
            else
                return value.Trim();
        }
    }
}
