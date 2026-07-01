using JobBoard.Application.Dtos.AuthenticationDtos.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Interfaces
{
    public interface IIdentityService
    {
        Task<string> RegisterAsync(string email, string password, string fullName);

        Task<AuthenticatedUserDto?> ValidateUserAsync(
            string email,
            string password);

        Task<IList<string>> GetRolesAsync(string userId);
    }
}
