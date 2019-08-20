using System;
using System.Collections.Generic;
using System.Linq;
using Users.Application.Interfaces;
using Users.Persistence.Repositories;
using Moq;
using Xunit;
using Users.Domain.Entities;
using Users.Domain.Enumerations;

namespace Users.Persistence.Test.Repositories
{
    public class UserRepositoryTest
    {
        private readonly UserRepository _userRepository;
        private readonly Mock<IUsersSource> _mockUsersSource;
        public UserRepositoryTest()
        {
            _mockUsersSource = new Mock<IUsersSource>(MockBehavior.Strict);
            _userRepository = new UserRepository(_mockUsersSource.Object);
        }

        [Fact]
        public void GetUser_NullFilter_VerifyArgumentNullExceptionIsThrown()
        {
            //Arrange
            var testUserList = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Gaurav",
                    LastName = "Danani",
                    Age = 32,
                    Gender = Gender.Male
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jigar",
                    LastName = "Danani",
                    Age = 31,
                    Gender = Gender.Male
                }
            };
            _mockUsersSource.Setup(s => s.LoadUsers()).Returns(testUserList.AsQueryable());

            //Act, Assert
            _ = Assert.Throws<ArgumentNullException>(() => _userRepository.GetUser(null));
        }

        [Fact]
        public void GetUser_UserIdFilter_VerifyCorrectUserIsReturned()
        {
            //Arrange
            var testUserList = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Gaurav",
                    LastName = "Danani",
                    Age = 32,
                    Gender = Gender.Male
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jigar",
                    LastName = "Danani",
                    Age = 31,
                    Gender = Gender.Male
                }
            };
            _mockUsersSource.Setup(s => s.LoadUsers()).Returns(testUserList.AsQueryable());

            //Act
            var user = _userRepository.GetUser(u => u.Id == 2);

            //Assert
            Assert.Equal(testUserList[1], user);
        }

        [Fact]
        public void GetUsers_NullFilter_VerifyArgumentNullExceptionIsThrown()
        {
            //Arrange
            var testUserList = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Gaurav",
                    LastName = "Danani",
                    Age = 32,
                    Gender = Gender.Male
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jigar",
                    LastName = "Danani",
                    Age = 31,
                    Gender = Gender.Male
                }
            };
            _mockUsersSource.Setup(s => s.LoadUsers()).Returns(testUserList.AsQueryable());

            //Act, Assert
            _ = Assert.Throws<ArgumentNullException>(() => _userRepository.GetUsers(null));
        }

        [Fact]
        public void GetUsers_AgeFilter_VerifyFilteredQueryableIsReturned()
        {
            //Arrange
            var testUserList = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Gaurav",
                    LastName = "Danani",
                    Age = 32,
                    Gender = Gender.Male
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jigar",
                    LastName = "Danani",
                    Age = 31,
                    Gender = Gender.Male
                },
                new User
                {
                    Id = 3,
                    FirstName = "Amisha",
                    LastName = "Thakkar",
                    Age = 31,
                    Gender = Gender.Female
                }
            };
            _mockUsersSource.Setup(s => s.LoadUsers()).Returns(testUserList.AsQueryable());

            //Act
            var usersQueryable = _userRepository.GetUsers(u => u.Age == 31);

            //Assert
            var userList = usersQueryable.ToList();
            Assert.Equal(2, userList.Count);
            Assert.DoesNotContain(testUserList[0], userList);
        }

        [Fact]
        public void GetAllUsers_VerifyUnFilteredUsersQuerableIsReturned()
        {
            //Arrange
            var testUserList = new List<User>
            {
                new User
                {
                    Id = 1,
                    FirstName = "Gaurav",
                    LastName = "Danani",
                    Age = 32,
                    Gender = Gender.Male
                },
                new User
                {
                    Id = 2,
                    FirstName = "Jigar",
                    LastName = "Danani",
                    Age = 31,
                    Gender = Gender.Male
                },
                new User
                {
                    Id = 3,
                    FirstName = "Amisha",
                    LastName = "Thakkar",
                    Age = 31,
                    Gender = Gender.Female
                }
            };
            _mockUsersSource.Setup(s => s.LoadUsers()).Returns(testUserList.AsQueryable());

            //Act
            var usersQueryable = _userRepository.GetAllUsers();

            //Assert
            var userList = usersQueryable.ToList();
            Assert.Equal(3, userList.Count);
            Assert.Contains(testUserList[0], userList);
            Assert.Contains(testUserList[1], userList);
            Assert.Contains(testUserList[2], userList);
        }
    }
}
