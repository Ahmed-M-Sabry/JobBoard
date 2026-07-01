using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Dtos.AuthenticationDtos.User.UserData.Profile
{
    public class SeekerProfileDto
    {
        public string JobTitle { get; set; } = default!;
        public string Location { get; set; } = default!;
        public string Bio { get; set; } = default!;
        public int YearsOfExperience { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public string? PortfolioUrl { get; set; }
        public string userId { get; set; }
    }
}
