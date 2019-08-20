using System.Collections.Generic;

namespace Users.Application.Models
{
    public class GendersPerAgeVm
    {
        public GendersPerAgeVm()
        {
            GendersDetail = new List<GenderDetailVm>();
        }
        public int Age { get; set; }
        public IList<GenderDetailVm> GendersDetail { get; }
        public override string ToString()
        {
            var result = $"Age : {Age}";
            foreach (var genderDetail in GendersDetail)
            {
                result += $" {genderDetail.Gender} : {genderDetail.Count}";
            }
            return result;
        }
    }

    public class GenderDetailVm
    {
        public string Gender { get; set; }
        public int Count { get; set; }
    }
}
