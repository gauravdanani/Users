namespace Users.Application.Interfaces
{
    public interface IFileHelper
    {
        string ReadAllText(string path);
        string GetFullFilePath(string fileName);
    }
}
