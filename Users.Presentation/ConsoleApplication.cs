using Microsoft.Extensions.Logging;
using System;
using System.IO;
using Users.Application.Exceptions;
using Users.Application.Interfaces;

namespace Users.Presentation
{
    public interface IConsoleApplication
    {
        void Run();
    }
    public class ConsoleApplication : IConsoleApplication
    {
        private readonly IUserService _userService;
        private readonly ILogger<ConsoleApplication> _logger;
        public ConsoleApplication(IUserService userService, ILogger<ConsoleApplication> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public void Run()
        {
            try
            {              
                //User with id 42
                try
                {
                    var userDetail = _userService.GetUserDetailById(42);
                    Console.WriteLine($"User for Id=42 : {userDetail.FirstName} {userDetail.LastName} {Environment.NewLine}");
                }
                catch (NotFoundException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }

                //All users of age 23
                try
                {
                    var usersByAge = _userService.GetUsersByAge(23);
                    Console.WriteLine($"Users who are 23: {string.Join(',', usersByAge)} {Environment.NewLine}");
                }
                catch (NotFoundException ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }

                //Genders per age
                Console.WriteLine($"Number of Genders per Age:");
                foreach (var result in _userService.GetGendersPerAge())
                {                  
                    Console.WriteLine(result.ToString());
                }
            }
            catch (InvalidJsonFormatException ex)
            {
                _logger.LogError(ex, $"Invalid Json file, Failed to load Users data.");
            }
            catch(FileNotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }
    }
}
