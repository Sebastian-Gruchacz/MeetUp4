namespace FileStorageService
{
    using System;
    using System.IO;

    public class FileStore : IFileStore
    {
        private readonly string _storageLocation;

        public FileStore()
        {
            _storageLocation = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                @"MettUpFourApp", // Yep, hardcoded ;-)
                "FileStorage");
            if (!Directory.Exists(_storageLocation))
            {
                Directory.CreateDirectory(_storageLocation);
            }
        }

        public string Download(string itemFileUrl, string destination)
        {
            var src = ValidateUrl(itemFileUrl);
            if (!File.Exists(src))
            {
                throw new FileNotFoundException(itemFileUrl);
            }

            File.Copy(src, destination, overwrite: true);

            return destination;
        }

        public StreamReader Download(string itemFileUrl)
        {
            var url = ValidateUrl(itemFileUrl);
            if (!File.Exists(url))
            {
                throw new FileNotFoundException(itemFileUrl);
            }

            throw new NotImplementedException();
        }

        public void Upload(string itemFileUrl, string sourcePath)
        {
            var destination = ValidateUrl(itemFileUrl);
            if (!Directory.Exists(Path.GetDirectoryName(destination)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destination));
            }

            File.Copy(sourcePath, destination, overwrite: true);
        }

        public void Upload(string itemFileUrl, StreamReader stream)
        {
            var url = ValidateUrl(itemFileUrl);

            if (!Directory.Exists(Path.GetDirectoryName(url)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(url));
            }


        }

        private string ValidateUrl(string itemFileUrl)
        {
            if (string.IsNullOrWhiteSpace(itemFileUrl))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(itemFileUrl));
            }

            if (Path.IsPathRooted(itemFileUrl))
            {
                throw new ArgumentException("Must specify related, storage path.", nameof(itemFileUrl));
            }

            var target = Path.Combine(_storageLocation, itemFileUrl);
            var url = new FileInfo(target);

            if (!url.FullName.StartsWith(_storageLocation))
            {
                throw new ArgumentException("Navigation outside root is forbidden.", nameof(itemFileUrl));
            }

            return url.FullName;
        }
    }
}
