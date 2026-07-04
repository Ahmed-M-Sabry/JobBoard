using JobBoard.Application.Common;
using JobBoard.Application.Dtos.JobApplication;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enum;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace JobBoard.Infrastructure.Implementation
{
    public class JobApplicationService : IJobApplicationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        public JobApplicationService(ApplicationDbContext context,IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }
        public async Task<Result<JobApplicationDto>> ApplyAsync(int seekerId, CreateJobApplication request)
        {
            var jobPost = await _context.JobPosts
                .FirstOrDefaultAsync(x => x.Id == request.JobPostId && !x.IsDeleted && x.JobPostStatus == JobStatus.Open);

            if (jobPost == null)
                return Result<JobApplicationDto>.Failure("Job post not found");

            var exists = await _context.JobApplications
                .AnyAsync(x => x.SeekerProfileId == seekerId && x.JobPostId == request.JobPostId);

            if (exists)
                return Result<JobApplicationDto>.Failure("You already applied to this job");

            var application = new JobApplication
            {
                SeekerProfileId = seekerId,
                JobPostId = request.JobPostId,
                CVLink = request.CVLink,
                Status = ApplicationStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _context.JobApplications.AddAsync(application);
            await _context.SaveChangesAsync();

            var dto = new JobApplicationDto
            {
                SeekerId = application.SeekerProfileId,
                JobPostId = application.JobPostId,
                CVLink = application.CVLink ?? "",
                Status = ApplicationStatus.Pending
            };

            return Result<JobApplicationDto>.Success(dto, "Application submitted successfully");
        }
        public async Task<Result<JobApplicationDto>> UpdateAsync(int seekerId, int applicationId, EditJobApplication request)
        {
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(x => x.Id == applicationId && x.Status == (int)ApplicationStatus.Pending);

            if (application == null)
                return Result<JobApplicationDto>.Failure("Application not found");

            if (application.SeekerProfileId != seekerId)
                return Result<JobApplicationDto>.Failure("You are not allowed to edit this application");

            if (!string.IsNullOrEmpty(request.CVLink))
                application.CVLink = request.CVLink;

            await _context.SaveChangesAsync();

            var dto = new JobApplicationDto
            {
                Id = application.Id,
                SeekerId = application.SeekerProfileId,
                JobPostId = application.JobPostId,
                CVLink = application.CVLink ?? "",
                Status = ApplicationStatus.Pending
            };

            return Result<JobApplicationDto>.Success(dto, "Application updated successfully");
        }
        
        public async Task<Result<List<EmployerJobApplicationDto>>> GetJobApplicationsForEmployerAsync(int employerId, int jobPostId)
        {
            var job = await _context.JobPosts
                .FirstOrDefaultAsync(j => j.Id == jobPostId && j.EmployerProfileId == employerId);

            if (job == null)
                return Result<List<EmployerJobApplicationDto>>.Failure("Job post not found or not yours");

            //var applications = await _context.JobApplications
            //    .Where(a => a.JobPostId == jobPostId)
            //    .ToListAsync();

            var applications = await _context.JobApplications
                   .Include(a => a.SeekerProfile)
                   .Include(a => a.JobPost)
                   .Where(a => a.JobPostId == jobPostId)
                   .ToListAsync();

            var result = new List<EmployerJobApplicationDto>();

            foreach (var a in applications)
            {
                var user = await _identityService.GetUserByIdAsync(a.SeekerProfile.ApplicationUserId);

                result.Add(new EmployerJobApplicationDto
                {
                    ApplicationId = a.Id,
                    SeekerId = a.SeekerProfileId,

                    ApplicantName = user?.FullName ?? "Unknown",
                    ApplicantEmail = user?.Email ?? "Unknown",

                    JobTitle = a.JobPost.Title,
                    YearsOfExp = a.SeekerProfile.YearsOfExperience,

                    CVLink = a.CVLink ?? "",
                    Status = a.Status,
                    AppliedAt = a.CreatedAt,
                });
            }

            return Result<List<EmployerJobApplicationDto>>.Success(result);
        }
        public async Task<Result<EmployerJobApplicationDto>> GetByIdForEmployerAsync(int employerId, int applicationId)
        {
            var application = await _context.JobApplications
                .Include(a => a.JobPost)
                .Include(a=>a.SeekerProfile)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
                return Result<EmployerJobApplicationDto>.Failure("Application not found");

            if (application.JobPost.EmployerProfileId != employerId)
                return Result<EmployerJobApplicationDto>.Failure("You are not allowed to view this application");

            var user = await _identityService.GetUserByIdAsync(application.SeekerProfile.ApplicationUserId);
            if (user == null)
                return Result<EmployerJobApplicationDto>.Failure("Applicant user not found");

            var dto = new EmployerJobApplicationDto
            {
                ApplicationId = application.Id,
                SeekerId = application.SeekerProfileId,
                ApplicantName = user.FullName,
                ApplicantEmail = user.Email,
                JobTitle = application.JobPost.Title,
                YearsOfExp = application.SeekerProfile.YearsOfExperience,
                CVLink = application.CVLink ?? "",
                Status = application.Status,
                AppliedAt = application.CreatedAt
            };

            return Result<EmployerJobApplicationDto>.Success(dto);
        }

        public async Task<Result<List<JobApplicationDto>>> GetJobApplicationsForSeekerAsync(int seekerId)
        {
            var applications = await _context.JobApplications
                   .Where(a => a.SeekerProfileId == seekerId)
                   .ToListAsync();

            var result = applications.Select(a => new JobApplicationDto
            {
                Id = a.Id,
                SeekerId = a.SeekerProfileId,
                JobPostId = a.JobPostId,
                CVLink = a.CVLink ?? "",
                Status = a.Status
            }).ToList();

            return Result<List<JobApplicationDto>>.Success(result);
        }
        public async Task<Result<JobApplicationDto>> GetByIdForSeekerAsync(int seekerId, int applicationId)
        {
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
                return Result<JobApplicationDto>.Failure("Application not found");

            if (application.SeekerProfileId != seekerId)
                return Result<JobApplicationDto>.Failure("You are not allowed to view this application");

            var dto = new JobApplicationDto
            {
                Id=application.Id,
                SeekerId = application.SeekerProfileId,
                JobPostId = application.JobPostId,
                CVLink = application.CVLink ?? "",
                Status = application.Status
            };

            return Result<JobApplicationDto>.Success(dto);
        }

        public async Task<Result<EmployerJobApplicationDto>> ChangeStatusAsync(
            int employerId,
            int applicationId,
            ApplicationStatus status)
        {
            var application = await _context.JobApplications
                .Include(a => a.JobPost)
                .Include(a=>a.SeekerProfile)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
                return Result<EmployerJobApplicationDto>.Failure("Application not found");

            if (application.JobPost.EmployerProfileId != employerId)
                return Result<EmployerJobApplicationDto>.Failure("You are not allowed to modify this application");

            if (application.Status == status)
                return Result<EmployerJobApplicationDto>.Failure("Application already in this status");

            application.Status = status;

            await _context.SaveChangesAsync();

            var user = await _identityService.GetUserByIdAsync(application.SeekerProfile.ApplicationUserId);
            if (user == null)
                return Result<EmployerJobApplicationDto>.Failure("Applicant user not found");

            var dto = new EmployerJobApplicationDto
            {
                ApplicationId = application.Id,
                SeekerId = application.SeekerProfileId,
                ApplicantName = user.FullName,
                ApplicantEmail = user.Email,
                JobTitle = application.JobPost.Title,
                YearsOfExp = application.SeekerProfile.YearsOfExperience,
                CVLink = application.CVLink ?? "",
                Status = application.Status,
                AppliedAt = application.CreatedAt
            };

            return Result<EmployerJobApplicationDto>.Success(dto, "Status updated successfully");
        }
    }
}