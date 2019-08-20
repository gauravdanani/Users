using System;
using System.Linq;
using System.Linq.Expressions;
using Users.Application.Interfaces;
using Users.Domain.Entities;

namespace Users.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IUsersSource _usersSource;
        public UserRepository(IUsersSource usersSource)
        {
            _usersSource = usersSource;
        }

        private IQueryable<User> _users;
        private IQueryable<User> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = _usersSource.LoadUsers();
                }
                return _users;
            }
        }

        public User GetUser(Expression<Func<User, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));
           
            return Users.FirstOrDefault(filter);
        }

        public IQueryable<User> GetUsers(Expression<Func<User, bool>> filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            return Users.Where(filter);
        }

        public IQueryable<User> GetAllUsers()
        {
            return Users;
        }
    }
}
