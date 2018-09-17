namespace MeetUp.Common
{
    using System;
    using System.Globalization;

    using MeetUp.Enumerations;

    public static class AmountFormat
    {
        public static string FormatAmount(object value, bool removeDecimals, LanguageCode cultureName)
        {
            var val = Convert.ToString(value);
            if (string.IsNullOrEmpty(val))
                return "";

            decimal.TryParse(val, out var amount);

            var culture = CultureInfo.CreateSpecificCulture(cultureName.GetLanguageCodeString());

            var format = string.Format(culture, "{0:N2}", amount);
            if (removeDecimals)
                format = string.Format(culture, "{0:n0}", amount);

            return format;
        }
    }
}