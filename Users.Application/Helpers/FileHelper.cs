using System.IO;
using System.Reflection;
using Users.Application.Interfaces;

namespace Users.Application.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string ReadAllText(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("File does not exist", path);

            return File.ReadAllText(path);
        }

        public string GetFullFilePath(string fileName)
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), fileName);
        }
    }
}
