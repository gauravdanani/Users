using System.IO;
using Users.Application.Helpers;
using Xunit;

namespace Users.Application.Test.Helpers
{
    public class FileHelperTest
    {
        private readonly FileHelper _fileHelper;
        public FileHelperTest()
        {
            _fileHelper = new FileHelper();
        }

        [Fact]
        public void ReadAllText_InvalidFilePath_VerifyFileNotFoundException()
        {
            Assert.Throws<FileNotFoundException>(() => _fileHelper.ReadAllText("InvalidFilePath.json"));
        }


        [Fact]
        public void ReadAllText_ValidFilePath_VerifyCorrectFileDataRead()
        {
            var fileData = _fileHelper.ReadAllText("test_data.json");
            Assert.Equal("[{ \"id\": 53, \"first\": \"Bill\", \"last\": \"Bryson\", \"age\":23, \"gender\":\"M\" },\r\n{ \"id\": 62, \"first\": \"John\", \"last\": \"Travolta\", \"age\":54, \"gender\":\"M\" },\r\n{ \"id\": 41, \"first\": \"Frank\", \"last\": \"Zappa\", \"age\":23, gender:\"T\" },\r\n{ \"id\": 31, \"first\": \"Jill\", \"last\": \"Scott\", \"age\":66, gender:\"Y\" },\r\n{ \"id\": 31, \"first\": \"Anna\", \"last\": \"Meredith\", \"age\":66, \"gender\":\"Y\" },\r\n{ \"id\": 31, \"first\": \"Janet\", \"last\": \"Jackson\", \"age\":66, \"gender\":\"F\" }]", fileData);
        }
    }
}
