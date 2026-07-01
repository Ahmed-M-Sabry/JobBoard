using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Domain.Common
{
    public static class AppRoles
    {
        public const string Admin = "Admin";
        public const string Employer = "Employer";
        public const string Seeker = "Seeker";

        public static readonly string[] All =
        {
            Admin,
            Employer,
            Seeker
        };
    }
}
