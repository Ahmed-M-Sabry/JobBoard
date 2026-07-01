using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.AuthenticationDtos.UserData
{
    public class UserData
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public List<string>? Roles { get; set; }
    }
}
