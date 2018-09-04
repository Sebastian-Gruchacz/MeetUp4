namespace MeetUp.Enumerations
{
    using System;

    public static class StringEnumExtensions
    {
        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}