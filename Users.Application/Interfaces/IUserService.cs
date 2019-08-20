using System.Collections.Generic;
using Users.Application.Models;

namespace Users.Application.Interfaces
{
    public interface IUserService
    {
        UserDetailVm GetUserDetailById(int id);
        IEnumerable<UserByAgeVm> GetUsersByAge(int age);
        IEnumerable<GendersPerAgeVm> GetGendersPerAge();
    }
}
