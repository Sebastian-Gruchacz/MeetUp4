namespace CommonComponents.Tests
{
    using MeetUp.Enumerations;

    using NUnit.Framework;

    public class LangaugeHelpersTest
    {
        [TestCase("en-US", LanguageCode.English)]
        [TestCase("english", LanguageCode.English)]
        [TestCase("English", LanguageCode.English)]
        [TestCase("da-DK", LanguageCode.Danish)]
        public void proper_descriptions_of_language_code_should_render_proper_enum_values(string desc, LanguageCode expectedCode)
        {
            var code = LanguageExtensions.GetCode(desc);

            Assert.AreEqual(expectedCode, code);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("en-GB")]
        public void improper_descriptions_of_language_code_should_render_unknown_enum_values(string desc)
        {
            var code = LanguageExtensions.GetCode(desc);

            Assert.AreEqual(LanguageCode.Unknown, code);
        }
    }
}
