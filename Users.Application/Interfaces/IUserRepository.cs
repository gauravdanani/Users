using System;
using System.Linq;
using System.Linq.Expressions;
using Users.Domain.Entities;

namespace Users.Application.Interfaces
{
    public interface IUserRepository
    {
        User GetUser(Expression<Func<User, bool>> filter);
        IQueryable<User> GetUsers(Expression<Func<User, bool>> filter);
        IQueryable<User> GetAllUsers();
    }
}
