namespace TestingHelpers
{
    public static class TestExtensions
    {
        public static T[] BuildSingleItemArray<T>(this T obj)
        {
            return new[] { obj };
        }
    }
}
