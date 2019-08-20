using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Moq;
using Users.Application.Exceptions;
using Users.Application.Interfaces;
using Users.Application.Services;
using Users.Domain.Entities;
using Xunit;
using FluentAssertions;
using Users.Domain.Enumerations;

namespace Users.Application.Test.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly UserService _userService;
        public UserServiceTest()
        {
            _mockUserRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            _userService = new UserService(_mockUserRepository.Object);
        }

        [Fact]
        public void GetUserDetailById_UserDoesNotExist_VerifyNotFoundExceptionIsThrown()
        {
            //Arrange
            var user = new User() { Id = 2 };
            _mockUserRepository.Setup(ur => ur.GetUser(It.Is<Expression<Func<User, bool>>>(e => !e.Compile()(user)))).Returns((User)null);

            //Act
            Assert.Throws<NotFoundException>(() => _userService.GetUserDetailById(1));
        }

        [Fact]
        public void GetUserDetailById_UserExist_VerifyUserDetailVmIsReturned()
        {
            //Arrange
            var user = new User() { Id = 2, FirstName = "Gaurav", LastName = "Danani" };
            _mockUserRepository.Setup(ur => ur.GetUser(It.Is<Expression<Func<User, bool>>>(e => e.Compile()(user)))).Returns(user);

            //Act
            var matchingUser = _userService.GetUserDetailById(2);

            //Assert
            Assert.Equal(matchingUser.FirstName, user.FirstName);
            Assert.Equal(matchingUser.LastName, user.LastName);
        }

        [Fact]
        public void GetUsersByAge_NoUsersFoundForTheGivenAge_VerifyNotFoundExceptionIsThrown()
        {
            //Arrange
            var user = new User() { Age = 20 };
            _mockUserRepository.Setup(ur => ur.GetUsers(It.Is<Expression<Func<User, bool>>>(e => !e.Compile()(user)))).Returns(new List<User>().AsQueryable());

            //Act
            Assert.Throws<NotFoundException>(() => _userService.GetUsersByAge(21).ToList());
        }

        [Fact]
        public void GetUsersByAge_UsersFoundForTheGivenAge_VerifyListOfUserByAgeVmIsReturned()
        {
            //Arrange
            var users = new List<User>
            {
                new User{Age = 20, FirstName = "Gaurav", Id =1}, new User {Age = 20, FirstName = "Jigar", Id = 3}
            };

            var user = new User() { Age = 20 };
            _mockUserRepository.Setup(ur => ur.GetUsers(It.Is<Expression<Func<User, bool>>>(e => e.Compile()(user)))).Returns(users.AsQueryable());

            //Act
            var userByAgeVms = _userService.GetUsersByAge(20).ToList();

            //Assert
            Assert.Equal(2, userByAgeVms.Count);
            users.Should().BeEquivalentTo(userByAgeVms);
        }

        [Fact]
        public void GetGendersPerAge_EmptyUserList_VerifyNoElementsReturned()
        {
            //Arrange
            var users = new List<User>();
            _mockUserRepository.Setup(ur => ur.GetAllUsers()).Returns(users.AsQueryable());

            //Act
            var gendersPerAgeVms = _userService.GetGendersPerAge().ToList();

            //Assert
            Assert.Empty(gendersPerAgeVms);
        }

        [Fact]
        public void GetGendersPerAge_UsersOfDifferentAgeExist_VerifyCorrectGenederDetailsPerAgeAreReturned()
        {
            //Arrange
            var users = new List<User>
            {
                new User{ Age = 21, FirstName = "Gaurav", Id =1, Gender = Gender.Male},
                new User{ Age = 21, FirstName = "Jigar", Id = 3, Gender = Gender.Female},
                new User{ Age = 20, FirstName = "Jigar", Id = 3, Gender = Gender.Male},
                new User{ Age = 22, FirstName = "Jigar", Id = 3, Gender = Gender.Unknown},
            };

            _mockUserRepository.Setup(ur => ur.GetAllUsers()).Returns(users.AsQueryable());

            //Act
            var gendersPerAgeVms = _userService.GetGendersPerAge().ToList();

            //Assert
            Assert.Equal(3, gendersPerAgeVms.Count);
            var first = gendersPerAgeVms[0];
            Assert.Equal(users[2].Age, first.Age);
            Assert.Equal(1, first.GendersDetail.Count);
            Assert.Equal(1, first.GendersDetail.Single(d => d.Gender == Gender.Male.ToString()).Count);

            var second = gendersPerAgeVms[1];
            Assert.Equal(users[0].Age, second.Age);
            Assert.Equal(2, second.GendersDetail.Count);
            Assert.Equal(1, second.GendersDetail.Single(d => d.Gender == Gender.Male.ToString()).Count);
            Assert.Equal(1, second.GendersDetail.Single(d => d.Gender == Gender.Female.ToString()).Count);

            var third = gendersPerAgeVms[2];
            Assert.Equal(users[3].Age, third.Age);
            Assert.Equal(1, third.GendersDetail.Count);
            Assert.Equal(1, third.GendersDetail.Single(d => d.Gender == Gender.Unknown.ToString()).Count);
        }
    }
}
