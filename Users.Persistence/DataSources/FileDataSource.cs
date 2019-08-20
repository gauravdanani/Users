using Microsoft.Extensions.Options;
using System.Linq;
using Users.Application.Config;
using Users.Application.Interfaces;
using Users.Domain.Entities;

namespace Users.Persistence.DataSources
{
    public class FileDataSource : IUsersSource
    {
        private readonly IFileDataParser _fileDataParser;
        private readonly IFileHelper _fileHelper;
        private readonly FileSourceConfig _fileConfiguration;
        public FileDataSource(IFileDataParser fileDataParser, IFileHelper fileHelper, IOptions<FileSourceConfig> fileConfiguration)
        {
            _fileDataParser = fileDataParser;
            _fileHelper = fileHelper;
            _fileConfiguration = fileConfiguration.Value;
        }

        public IQueryable<User> LoadUsers()
        {
            var filePath = string.IsNullOrWhiteSpace(_fileConfiguration.DataSourceFilePath)
                ? _fileHelper.GetFullFilePath("example_data.json")
                : _fileConfiguration.DataSourceFilePath;

            var users = _fileHelper.ReadAllText(filePath);
            return _fileDataParser.ParseUsers(users).AsQueryable();
        }
    }
}
