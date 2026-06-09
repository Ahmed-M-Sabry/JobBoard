using JobBoard.Infrastructure.Authentication;
using JobBoard.Infrastructure.Data;
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
                    => options.UseSqlServer("Server=localhost\\SQLEXPRESS;DataBase=JobBoardApp;Trusted_Connection=True;TrustServerCertificate=True;"));
            
            service
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            return service;
        }
    }
}
