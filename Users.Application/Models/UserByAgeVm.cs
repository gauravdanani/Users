namespace Users.Application.Models
{
    public class UserByAgeVm
    {
        public string FirstName { get; set; }
        public override string ToString()
        {
            return FirstName;
        }
    }
}
