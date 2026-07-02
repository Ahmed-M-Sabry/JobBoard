using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Dtos.AuthenticationDtos.User.UserData.Profile
{
    public class EmployerProfileDto
    {
        public int Id { get; set; }
        public string userId { get; set; }
        public string CompanyName { get; set; } = default!;
        public string Location { get; set; } = default!;
        public string Description { get; set; } = default!;
    }
}
