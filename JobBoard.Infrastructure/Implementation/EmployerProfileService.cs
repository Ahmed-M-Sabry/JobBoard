using JobBoard.Application.Dtos.AuthenticationDtos.User.UserData.Profile;
using JobBoard.Application.Dtos.AuthenticationDtos.UserData;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Common;
using JobBoard.Domain.Entities.Users;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Infrastructure.Implementation
{
    public class EmployerProfileService : IEmployerProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public EmployerProfileService(
            ApplicationDbContext context,
            IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<EmployerProfileDto> GetByUserIdAsync(string userId)
        {
            var profile = await _context.EmployerProfiles
                //.AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApplicationUserId == userId);

            if (profile == null)
                throw new Exception("Employer profile not found.");

            return new EmployerProfileDto
            {
                Id= profile.Id,
                userId = profile.ApplicationUserId,
                CompanyName = profile.CompanyName,
                Location = profile.Location,
                Description = profile.Description
            };
        }
        public async Task CreateAsync(string userId, CreateEmployeerProfileDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var user = await _identityService.GetUserByIdAsync(userId);

                if (user == null)
                    throw new Exception("User not found");

                var seekerExists = await _context.SeekerProfiles
                    .AnyAsync(x => x.ApplicationUserId == userId);

                var employerExists = await _context.EmployerProfiles
                    .AnyAsync(x => x.ApplicationUserId == userId);

                if (seekerExists || employerExists)
                    throw new Exception("Profile already exists");

                var employerProfile = new EmployerProfile
                {
                    ApplicationUserId = userId,
                    CompanyName = dto.CompanyName,
                    Location = dto.Location,
                    Description = dto.Description
                };

                _context.EmployerProfiles.Add(employerProfile);

                await _context.SaveChangesAsync();

                await _identityService.AddToRoleAsync(userId, AppRoles.Employer);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception(ex.Message);
            }
        }
    }
}