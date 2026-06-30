using JobBoard.Application.Interfaces;
using JobBoard.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Implementation
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> RegisterAsync(string email, string password, string fullName)
        {
            var IsEmailExists = await _userManager.FindByEmailAsync(email);

            if (IsEmailExists != null)
                throw new Exception("Email already exists");
            
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            return user.Id;
        }
    }
}
