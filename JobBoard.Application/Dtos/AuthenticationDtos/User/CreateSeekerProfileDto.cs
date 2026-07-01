using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Dtos.AuthenticationDtos.UserData
{
    public class CreateSeekerProfileDto
    {
        public string JobTitle { get; set; }
        public string Country { get; set; }
        public string Bio { get; set; }
        public int YearsOfExperience { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public string? PortfolioUrl { get; set; }
    }
}
