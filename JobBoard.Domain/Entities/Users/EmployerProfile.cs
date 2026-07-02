using JobBoard.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Entities.Users
{
    public class EmployerProfile : BaseEntity
    {
        public string ApplicationUserId { get; set; } = default!;
        public string CompanyName { get; set; } = default!;
        public string Location { get; set; }
        public string Description { get; set; }
        public ICollection<JobPost> jobPosts { get; set; }

        // public string Industry { get; set; }  // Will add this later when I implement industry categories

        // I Will add more fields later like website, social media links, etc.
    }
}
