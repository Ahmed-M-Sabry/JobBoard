using JobBoard.Application.Interfaces;
using JobBoard.Infrastructure.Authentication;
using JobBoard.Infrastructure.Data;
using JobBoard.Infrastructure.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace JobBoard.Infrastructure
{
    public static class InfrastructureLayerDependencies
    {
        public static IServiceCollection InfrastructureLayerServices(this IServiceCollection service)
        {
            service.AddDbContext<ApplicationDbContext>(options 
                    => options.UseSqlServer("Server=(localdb)\\ProjectModels;DataBase=JobBoardApp;Trusted_Connection=True;TrustServerCertificate=True;"));

            service.AddIdentity<ApplicationUser, IdentityRole>(options =>
                   {
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequiredLength = 3;
                   })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();

            service.AddScoped<IIdentityService, IdentityService>();
            service.AddScoped<IAuthService, AuthService>();
            service.AddScoped<ITokenService, TokenService>();
            service.AddScoped<ISeekerProfileService, SeekerProfileService>();
            service.AddScoped<IEmployerProfileService, EmployerProfileService>();
            service.AddScoped<IJobPostService, JobPostService>();

            return service;
        }
    }
}
