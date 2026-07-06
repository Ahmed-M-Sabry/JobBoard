using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Dtos.AuthenticationDtos.User.UserData.Profile
{
    public class BecomeSeekerDto
    {
        public string ApplicationUserId { get; set; } = string.Empty;
        public string? JobTitle { get; set; }
        public string? Location { get; set; }
        public string? Bio { get; set; }
        public int YearsOfExperience { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public string? PortfolioUrl { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
