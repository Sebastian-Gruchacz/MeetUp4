namespace FileStorageService
{
    using System.IO;

    public interface IFileStore
    {
        string Download(string itemFileUrl, string destination);

        StreamReader Download(string itemFileUrl);

        void Upload(string itemFileUrl, string sourcePath);

        void Upload(string itemFileUrl, StreamReader stream);
    }
}