using System.Linq;
using Users.Domain.Entities;

namespace Users.Application.Interfaces
{
    public interface IUsersSource
    {
        IQueryable<User> LoadUsers();
    }
}
