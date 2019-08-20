using System.Collections.Generic;
using Users.Domain.Entities;

namespace Users.Application.Interfaces
{
    public interface IFileDataParser
    {
        IEnumerable<User> ParseUsers(string input);
    }
}
