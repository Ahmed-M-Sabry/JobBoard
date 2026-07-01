using JobBoard.Domain.Entities;
using JobBoard.Domain.Entities.Users;
using JobBoard.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1 : 1 Seeker - ApplicationUser
            builder.Entity<SeekerProfile>()
                .HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<SeekerProfile>(s => s.ApplicationUserId);

            // 1 : 1 Employer - ApplicationUser
            builder.Entity<EmployerProfile>()
                .HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<EmployerProfile>(e => e.ApplicationUserId);
        }
        public DbSet<JobPost> JobPosts { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<SeekerProfile> SeekerProfiles { get; set; }
        public DbSet<EmployerProfile> EmployerProfiles { get; set; }
    }
}
