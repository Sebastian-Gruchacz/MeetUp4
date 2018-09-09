namespace StoreServiceTests
{
    using System;
    using System.IO;

    using FileStorageService;

    using NUnit.Framework;

    public class StoreServiceOperationsTests
    {
        [TestCase(null)]
        [TestCase(@"")]
        [TestCase(@"..\a.txt")]
        [TestCase(@"\a.txt")]
        [TestCase(@"abs\..\..\abd\a.txt")]
        [TestCase(@"C:\temp\a.txt")]
        public void download_should_reject_invalid_urls(string testUrl)
        {
            var service = new FileStore();

            Assert.Throws<ArgumentException>(() => service.Download(testUrl, Path.GetTempFileName()));
        }

        [TestCase(@"abs\b.txt")]
        [TestCase(@"b.txt")]
        [TestCase(@".")]
        public void download_should_reject_nonexistent_urls(string testUrl)
        {
            var service = new FileStore();

            Assert.Throws<FileNotFoundException>(() => service.Download(testUrl, Path.GetTempFileName()));
        }

        [TestCase(@"a.txt")]
        [TestCase(@"abs\a.txt")]
        public void upload_should_allow_correct_urls(string testUrl)
        {
            var service = new FileStore();

            service.Upload(testUrl, Path.GetTempFileName());
        }

        // TODO: any1 would like to test actual upload / download & with streams?
    }
}
