using System.Collections.Generic;
using System.Linq;
using Users.Application.Exceptions;
using Users.Application.Interfaces;
using Users.Application.Models;

namespace Users.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDetailVm GetUserDetailById(int id)
        {
            var user = _userRepository.GetUser(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException($"User Not Found, Id : {id}");
            }
            return new UserDetailVm { FirstName = user.FirstName, LastName = user.LastName };
        }

        public IEnumerable<UserByAgeVm> GetUsersByAge(int age)
        {
            var firstNameList = _userRepository.GetUsers(u => u.Age == age).Select(u => u.FirstName).ToList();

            if(!firstNameList.Any())
            {
                throw new NotFoundException($"No Users Found of Age : {age}");
            }

            foreach (var name in firstNameList)
            {
                yield return new UserByAgeVm { FirstName = name };
            }
        }

        public IEnumerable<GendersPerAgeVm> GetGendersPerAge()
        {
            foreach (var g in _userRepository.GetAllUsers().GroupBy(u => u.Age).OrderBy(g => g.Key))
            {
                var vm = new GendersPerAgeVm { Age = g.Key };
                foreach (var gender in g.GroupBy(u => u.Gender))
                {
                    vm.GendersDetail.Add(new GenderDetailVm { Gender = gender.Key.ToString(), Count = gender.Count() });
                }
                yield return vm;
            }
        }
    }
}
