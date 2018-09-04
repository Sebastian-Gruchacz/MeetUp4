namespace OrderService
{
    public interface IFileStore
    {
        string Download(string itemFileUrl, string destination);
    }
}