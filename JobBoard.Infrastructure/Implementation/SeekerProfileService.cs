using JobBoard.Application.Dtos.AuthenticationDtos.User.UserData.Profile;
using JobBoard.Application.Dtos.AuthenticationDtos.UserData;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Common;
using JobBoard.Domain.Entities.Users;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace JobBoard.Infrastructure.Implementation
{
    public class SeekerProfileService : ISeekerProfileService
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public SeekerProfileService(ApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<SeekerProfileDto> GetByUserIdAsync(string userId)
        {
            var profile = await _context.SeekerProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ApplicationUserId == userId);

            if (profile == null)
                throw new Exception("Seeker profile not found.");

            return new SeekerProfileDto
            {
                userId = profile.ApplicationUserId,
                JobTitle = profile.JobTitle,
                Location = profile.Location,
                Bio = profile.Bio,
                YearsOfExperience = profile.YearsOfExperience,
                LinkedInUrl = profile.LinkedInUrl,
                GitHubUrl = profile.GitHubUrl,
                PortfolioUrl = profile.PortfolioUrl
            };
        }

        public async Task CreateAsync(string userId, CreateSeekerProfileDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _identityService.GetUserByIdAsync(userId);
                if(user == null) 
                    throw new Exception("User not found");

                var Seekerexists = await _context.SeekerProfiles.AnyAsync(x => x.ApplicationUserId == userId);
                var Employeerexists = await _context.EmployerProfiles.AnyAsync(x => x.ApplicationUserId == userId);

                if (Seekerexists || Employeerexists)
                    throw new Exception("Profile already exists");

                var seekerProfile = new SeekerProfile
                {
                    ApplicationUserId = userId,
                    JobTitle = dto.JobTitle,
                    Location = dto.Country,
                    Bio = dto.Bio,
                    YearsOfExperience = dto.YearsOfExperience,
                    LinkedInUrl = dto.LinkedInUrl,
                    GitHubUrl = dto.GitHubUrl,
                    PortfolioUrl = dto.PortfolioUrl
                };
                _context.Add(seekerProfile);
                await _context.SaveChangesAsync();

                await _identityService.AddToRoleAsync(userId, AppRoles.Seeker);

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
