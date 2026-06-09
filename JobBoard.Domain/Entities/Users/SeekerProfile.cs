using JobBoard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities.Users
{
    public class SeekerProfile : BaseEntity
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string JobTitle { get; set; }          
        public string Location { get; set; }          
        public string Bio { get; set; }                  
        public int YearsOfExperience { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public string? PortfolioUrl { get; set; }
        public string ApplicationUserId { get; set; } = default!;

        // I Will add more fields later like skills, education, etc.
    }
}
