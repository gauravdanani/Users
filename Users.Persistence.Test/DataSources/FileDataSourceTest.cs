using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Users.Application.Config;
using Users.Application.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Enumerations;
using Users.Persistence.DataSources;
using Xunit;

namespace Users.Persistence.Test.DataSources
{
    public class FileDataSourceTest
    {
        private readonly Mock<IFileDataParser> _mockFileDataParser;
        private readonly Mock<IFileHelper> _mockFileHelper;
        private readonly FileSourceConfig _fileConfig;
        private FileDataSource _fileDataSource;
        public FileDataSourceTest()
        {
            _mockFileDataParser = new Mock<IFileDataParser>(MockBehavior.Strict);
            _mockFileHelper = new Mock<IFileHelper>(MockBehavior.Strict);
            _fileConfig = new FileSourceConfig();
            _fileDataSource = new FileDataSource(_mockFileDataParser.Object, _mockFileHelper.Object, Options.Create(_fileConfig));
        }

        [Fact]
        public void LoadUsers_FilePathNotProvided_VerifyUserListIsLoadedFromTestFile()
        {
            //Arrange
            _fileConfig.DataSourceFilePath = null;
            string filePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "example_data.json");
            _mockFileHelper.Setup(f => f.GetFullFilePath("example_data.json")).Returns(filePath);

            var testJson = "[{ \"id\": 53, \"first\": \"Bill\", \"last\": \"Bryson\", \"age\":23, \"gender\":\"M\" },\r\n{ \"id\": 62, \"first\": \"John\", \"last\": \"Travolta\", \"age\":54, \"gender\":\"M\" },\r\n{ \"id\": 41, \"first\": \"Frank\", \"last\": \"Zappa\", \"age\":23, gender:\"T\" },\r\n{ \"id\": 31, \"first\": \"Jill\", \"last\": \"Scott\", \"age\":66, gender:\"Y\" },\r\n{ \"id\": 31, \"first\": \"Anna\", \"last\": \"Meredith\", \"age\":66, \"gender\":\"Y\" },\r\n{ \"id\": 31, \"first\": \"Janet\", \"last\": \"Jackson\", \"age\":66, \"gender\":\"F\" }]";
            _mockFileHelper.Setup(f => f.ReadAllText(filePath)).Returns(testJson);
            var userList = new List<User>
            {
                new User { Id = 53, FirstName = "Bill", LastName = "Bryson", Age = 23, Gender = Gender.Male },
                new User { Id = 54, FirstName = "Bill1", LastName = "Bryson1", Age = 24, Gender = Gender.Male }
            };
            _mockFileDataParser.Setup(p => p.ParseUsers(testJson)).Returns(userList);

            //Act
            var result = _fileDataSource.LoadUsers();

            //Assert
            Assert.Equal(result, userList);
            Assert.Equal(result.Count(), userList.Count);
        }

        [Fact]
        public void LoadUsers_FilePathProvidedFromConfig_VerifyUserListIsLoadedFromCofigFilePath()
        {
            //Arrange
            _fileConfig.DataSourceFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "example_data.json");            

            var testJson = "[{ \"id\": 53, \"first\": \"Bill\", \"last\": \"Bryson\", \"age\":23, \"gender\":\"M\" },\r\n{ \"id\": 62, \"first\": \"John\", \"last\": \"Travolta\", \"age\":54, \"gender\":\"M\" },\r\n{ \"id\": 41, \"first\": \"Frank\", \"last\": \"Zappa\", \"age\":23, gender:\"T\" },\r\n{ \"id\": 31, \"first\": \"Jill\", \"last\": \"Scott\", \"age\":66, gender:\"Y\" },\r\n{ \"id\": 31, \"first\": \"Anna\", \"last\": \"Meredith\", \"age\":66, \"gender\":\"Y\" },\r\n{ \"id\": 31, \"first\": \"Janet\", \"last\": \"Jackson\", \"age\":66, \"gender\":\"F\" }]";
            _mockFileHelper.Setup(f => f.ReadAllText(_fileConfig.DataSourceFilePath)).Returns(testJson);
            var userList = new List<User>
            {
                new User { Id = 53, FirstName = "Bill", LastName = "Bryson", Age = 23, Gender = Gender.Male },
                new User { Id = 54, FirstName = "Bill1", LastName = "Bryson1", Age = 24, Gender = Gender.Male }
            };
            _mockFileDataParser.Setup(p => p.ParseUsers(testJson)).Returns(userList);

            //Act
            var result = _fileDataSource.LoadUsers();

            //Assert
            Assert.Equal(result, userList);
            Assert.Equal(result.Count(), userList.Count);
        }
    }
}
