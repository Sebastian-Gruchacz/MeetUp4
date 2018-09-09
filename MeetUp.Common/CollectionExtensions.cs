// ReSharper disable once CheckNamespace (Must be visible everywhere where Array is)
namespace System
{
    public  static class CollectionExtensions
    {
        public static T[] ToSingleItemArray<T>(this T item)
        {
            return new[] {item};
        }
    }
}