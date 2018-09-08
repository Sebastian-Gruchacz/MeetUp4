namespace MeetUp.Enumerations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public static class LanguageExtensions
    {
        // For better implementations see: https://github.com/jskeet/unconstrained-melody/blob/master/UnconstrainedMelody/EnumInternals.cs

        private static readonly Dictionary<LanguageCode, string> Codes = new Dictionary<LanguageCode, string>();
        private static readonly Dictionary<string, LanguageCode> Descsriptions = new Dictionary<string, LanguageCode>();

        static LanguageExtensions()
        {
            var values = (LanguageCode[])Enum.GetValues(typeof(LanguageCode));
            foreach (var languageCode in values.Where(code => code != LanguageCode.Unknown))
            {
                var desc = GetDescription(languageCode);

                Codes.Add(languageCode, desc);
                Descsriptions.Add(desc, languageCode);
            }
        }

        public static string GetLanguageCodeString(this LanguageCode code)
        {
            if (code == LanguageCode.Unknown)
            {
                return null;
            }

            return Codes[code];
        }

        public static LanguageCode GetCode(string languageCodeString)
        {
            if (string.IsNullOrWhiteSpace(languageCodeString))
            {
                return LanguageCode.Unknown;
            }

            if (Enum.TryParse<LanguageCode>(languageCodeString, true, out var parsed))
            {
                return parsed; // is English, Danish...
            }

            if (Descsriptions.TryGetValue(languageCodeString, out parsed))
            {
                return parsed;
            }

            return LanguageCode.Unknown; // TODO: throw?
        }

        private static string GetDescription(LanguageCode value)
        {
            FieldInfo field = typeof(LanguageCode).GetField(value.ToString());
            return field.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .Select(x => x.Description)
                .FirstOrDefault();
        }
    }
}