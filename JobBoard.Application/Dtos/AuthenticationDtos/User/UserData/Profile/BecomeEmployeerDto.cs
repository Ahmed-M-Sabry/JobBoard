using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Dtos.AuthenticationDtos.User.UserData.Profile
{
    public class BecomeEmployeerDto
    {
        public string ApplicationUserId { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }

    }
}
