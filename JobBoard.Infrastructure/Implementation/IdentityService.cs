using JobBoard.Application.Dtos.AuthenticationDtos.Login;
using JobBoard.Application.Interfaces;
using JobBoard.Infrastructure.Authentication;
using JobBoard.Infrastructure.Data;
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
        private readonly ApplicationDbContext _context;
        public IdentityService(UserManager<ApplicationUser> userManager , ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task AddToRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            var result = await _userManager.AddToRoleAsync(user, role);
            
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to add role: {errors}");
            }
        }
        public async Task<IList<string>> GetRolesAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return new List<string>();

            var roles = await _userManager.GetRolesAsync(user);
            return roles;
        }

        public async Task<AuthenticatedUserDto?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            if(user == null) 
                return null;
            var Roles = await _userManager.GetRolesAsync(user);

            return new AuthenticatedUserDto
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                Roles = Roles.ToList()
            };
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

        public async Task<AuthenticatedUserDto> ValidateUserAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
                return null;
            var isValid = await _userManager.CheckPasswordAsync(user, password);
            if(isValid == false) 
                return null;

            var AuthenticatedUser = new AuthenticatedUserDto
            {
                UserId = user.Id,
                Email = user.Email,
                FullName = user.FullName
            };
            return AuthenticatedUser;
        }
    }
}
