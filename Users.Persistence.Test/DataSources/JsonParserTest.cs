using Xunit;
using Users.Persistence.DataSources;
using Users.Application.Exceptions;
using System.Linq;
using Users.Domain.Enumerations;

namespace Users.Persistence.Test.DataSources
{
    public class JsonParserTest
    {
        private JsonParser _jsonParser;
        public JsonParserTest()
        {
            _jsonParser = new JsonParser();
        }

        [Fact]
        public void ParseUsers_EmptyJsonString_VerifyExceptionIsThrown()
        {
            //Arrange
            string jsonString = null;

            //Act, Assert
            Assert.Throws<InvalidJsonFormatException>(() => _jsonParser.ParseUsers(jsonString));          
        }

        [Fact]
        public void ParseUsers_NullJsonString_VerifyExceptionIsThrown()
        {
            //Arrange
            string jsonString = string.Empty;
            
            //Act, Assert
            Assert.Throws<InvalidJsonFormatException>(() => _jsonParser.ParseUsers(jsonString));            
        }

        [Fact]
        public void ParseUsers_InvalidJsonString_VerifyExceptionIsThrown()
        {
            //Arrange
            var jsonString = "[{ \"id\": 53, \"first\": Bill, \"last\": \"Bryson\", \"age\":23, \"gender\":\"M\" },\r\n{ \"id\": 62, \"first\": \"John\", \"last\": \"Travolta\", \"age\":54, \"gender\":\"M\" },\r\n{ \"id\": 41, \"first\": \"Frank\", \"last\": \"Zappa\", \"age\":23, gender:\"T\" }]";

            //Act, Assert
            Assert.Throws<InvalidJsonFormatException>(() => _jsonParser.ParseUsers(jsonString));
        }

        [Fact]
        public void ParseUsers_ValidJsonString_MissingFields_VerifyMissingFieldIsIgnored()
        {
            //Arrange
            var jsonString = "[{ \"id\": 53, \"first\": \"Bill\", \"age\":23, \"gender\":\"M\" },\r\n{ \"id\": 62, \"first\": \"John\", \"last\": \"Travolta\", \"age\":54, \"gender\":\"M\" },\r\n{ \"id\": 41, \"first\": \"Frank\", \"last\": \"Zappa\", \"age\":23, gender:\"T\" }]";

            //Act
            var users = _jsonParser.ParseUsers(jsonString);

            //Assert
            var usersList = users.ToList();
            Assert.Equal(3, usersList.Count);
            var user1 = usersList[0];
            Assert.Equal(53, user1.Id);
            Assert.Equal("Bill", user1.FirstName);
            Assert.Null(user1.LastName);
            Assert.Equal(23, user1.Age);
            Assert.Equal(Gender.Male, user1.Gender);
            var user2 = usersList[1];
            Assert.Equal(62, user2.Id);
            Assert.Equal("John", user2.FirstName);
            Assert.Equal("Travolta", user2.LastName);
            Assert.Equal(54, user2.Age);
            Assert.Equal(Gender.Male, user2.Gender);
            var user3 = usersList[2];
            Assert.Equal(41, user3.Id);
            Assert.Equal("Frank", user3.FirstName);
            Assert.Equal("Zappa", user3.LastName);
            Assert.Equal(23, user3.Age);
            Assert.Equal(Gender.Unknown, user3.Gender);
        }

        [Fact]
        public void ParseUsers_ValidJsonString_DuplicateFields_VerifyLatestFieldIsConsidered()
        {
            //Arrange
            var jsonString = "[{ \"id\": 53, \"first\": \"Bill\", \"last\": \"Bryson\", \"last\": \"Bryson1\", \"age\":23, \"gender\":\"M\" },\r\n{ \"id\": 62, \"first\": \"John\", \"last\": \"Travolta\", \"age\":54, \"gender\":\"M\" },\r\n{ \"id\": 41, \"first\": \"Frank\", \"last\": \"Zappa\", \"age\":23, gender:\"T\" }]";

            //Act
            var users = _jsonParser.ParseUsers(jsonString);

            //Assert
            var usersList = users.ToList();
            Assert.Equal(3, usersList.Count);
            var user1 = usersList[0];
            Assert.Equal(53, user1.Id);
            Assert.Equal("Bill", user1.FirstName);
            Assert.Equal("Bryson1", user1.LastName);
            Assert.Equal(23, user1.Age);
            Assert.Equal(Gender.Male, user1.Gender);
            var user2 = usersList[1];
            Assert.Equal(62, user2.Id);
            Assert.Equal("John", user2.FirstName);
            Assert.Equal("Travolta", user2.LastName);
            Assert.Equal(54, user2.Age);
            Assert.Equal(Gender.Male, user2.Gender);
            var user3 = usersList[2];
            Assert.Equal(41, user3.Id);
            Assert.Equal("Frank", user3.FirstName);
            Assert.Equal("Zappa", user3.LastName);
            Assert.Equal(23, user3.Age);
            Assert.Equal(Gender.Unknown, user3.Gender);
        }

        [Fact]
        public void ParseUsers_ValidJsonString_ExtraFields_VerifyExtraFieldIsIgnored()
        {
            //Arrange
            var jsonString = "[{ \"id\": 53, \"first\": \"Bill\", \"last\": \"Bryson\", \"age\":23, \"gender\":\"M\" },\r\n{ \"id\": 62, \"first\": \"John\", \"last\": \"Travolta\", \"age\":54, \"gender\":\"M\" },\r\n{ \"id\": 41, \"first\": \"Frank\", \"last\": \"Zappa\", \"age\":23, gender:\"T\" }]";

            //Act
            var users = _jsonParser.ParseUsers(jsonString);

            //Assert
            var usersList = users.ToList();
            Assert.Equal(3, usersList.Count);
            var user1 = usersList[0];
            Assert.Equal(53, user1.Id);
            Assert.Equal("Bill", user1.FirstName);
            Assert.Equal("Bryson", user1.LastName);
            Assert.Equal(23, user1.Age);
            Assert.Equal(Gender.Male, user1.Gender);
            var user2 = usersList[1];
            Assert.Equal(62, user2.Id);
            Assert.Equal("John", user2.FirstName);
            Assert.Equal("Travolta", user2.LastName);
            Assert.Equal(54, user2.Age);
            Assert.Equal(Gender.Male, user2.Gender);
            var user3 = usersList[2];
            Assert.Equal(41, user3.Id);
            Assert.Equal("Frank", user3.FirstName);
            Assert.Equal("Zappa", user3.LastName);
            Assert.Equal(23, user3.Age);
            Assert.Equal(Gender.Unknown, user3.Gender);
        }

        [Fact]
        public void ParseUsers_ValidJsonString_AllFields_VerifyUserListIsReturned()
        {
            //Arrange
            var jsonString = "[{ \"id\": 53, \"first\": \"Bill\", \"last\": \"Bryson\", \"last1\": \"Bryson\", \"age\":23, \"gender\":\"M\" },\r\n{ \"id\": 62, \"first\": \"John\", \"last\": \"Travolta\", \"age\":54, \"gender\":\"M\" },\r\n{ \"id\": 41, \"first\": \"Frank\", \"last\": \"Zappa\", \"age\":23, gender:\"T\" }]";

            //Act
            var users = _jsonParser.ParseUsers(jsonString);

            //Assert
            var usersList = users.ToList();
            Assert.Equal(3, usersList.Count);
            var user1 = usersList[0];
            Assert.Equal(53, user1.Id);
            Assert.Equal("Bill", user1.FirstName);
            Assert.Equal("Bryson", user1.LastName);
            Assert.Equal(23, user1.Age);
            Assert.Equal(Gender.Male, user1.Gender);
            var user2 = usersList[1];
            Assert.Equal(62, user2.Id);
            Assert.Equal("John", user2.FirstName);
            Assert.Equal("Travolta", user2.LastName);
            Assert.Equal(54, user2.Age);
            Assert.Equal(Gender.Male, user2.Gender);
            var user3 = usersList[2];
            Assert.Equal(41, user3.Id);
            Assert.Equal("Frank", user3.FirstName);
            Assert.Equal("Zappa", user3.LastName);
            Assert.Equal(23, user3.Age);
            Assert.Equal(Gender.Unknown, user3.Gender);
        }
    }
}
