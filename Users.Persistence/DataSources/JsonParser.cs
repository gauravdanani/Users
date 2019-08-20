using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Users.Application.Exceptions;
using Users.Application.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Enumerations;

namespace Users.Persistence.DataSources
{
    public class JsonParser : IFileDataParser
    {      
        private readonly IDictionary<string, Gender> _genderMap = new Dictionary<string, Gender>
        {
            { "M", Gender.Male },
            { "F", Gender.Female }
        };
        public IEnumerable<User> ParseUsers(string input)
        {
            JArray userArray = new JArray();
            try
            {
                userArray = JArray.Parse(input);
            }
            catch (Exception e)
            {
                throw new InvalidJsonFormatException(e.Message, e);
            }

            var users = userArray.Select(p =>
            {
                var user = new User
                {
                    Id = p["id"] != null ? (int)p["id"] : 0,
                    FirstName = p["first"] != null ? (string)p["first"] : null,
                    LastName = p["last"] != null ? (string)p["last"] : null,
                    Age = p["age"] != null ? (int)p["age"] : 0,
                };
                var gender = p["gender"] != null ? ((string)p["gender"])?.ToUpperInvariant() : null;               
                user.Gender = gender != null && _genderMap.ContainsKey(gender) ? _genderMap[gender] : Gender.Unknown;
                return user;
            }).ToList();
            return users;
        }
    }
}
