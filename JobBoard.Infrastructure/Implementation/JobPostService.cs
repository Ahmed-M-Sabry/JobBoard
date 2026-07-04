using JobBoard.Application.Common;
using JobBoard.Application.Dtos.JobPost;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Enum;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Infrastructure.Implementation
{
    public class JobPostService : IJobPostService
    {
        private readonly ApplicationDbContext _context;
        public JobPostService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result<List<EmployerJobApplicationCountDto>>> GetJobsAndApplicationsCountForEmployerAsync(int employerId)
        {
            var result = await _context.JobPosts
                .Where(j => j.EmployerProfileId == employerId)
                .Select(j => new EmployerJobApplicationCountDto
                {
                    Id = j.Id,
                    JobPostName = j.Title,
                    ApplicationsCount = j.JobApplications.Count()
                })
                .ToListAsync();

            return Result<List<EmployerJobApplicationCountDto>>.Success(result);
        }
        public async Task<Result<List<JobPostDto>>> GetAllJobPostsAsync()
        {
            var jobs = await _context.JobPosts
                .Where(j => !j.IsDeleted && j.JobPostStatus == JobStatus.Open)
                .Select(j => new JobPostDto
                {
                    Id = j.Id,
                    Title = j.Title,
                    Description = j.Description,
                    CompanyLocation = j.Location,
                    Salary = j.Salary,
                    PostedDate = j.PostedDate,
                    EmployeerId = j.EmployerProfileId,
                    JobPostStatus = j.JobPostStatus
                })
                .ToListAsync();

            return Result<List<JobPostDto>>.Success(jobs);
        }
        public async Task<Result<JobPostDto>> GetJobPostByIdAsync(int id)
        {
            var job = await _context.JobPosts
                .FirstOrDefaultAsync(j => j.Id == id && !j.IsDeleted && j.JobPostStatus == JobStatus.Open);

            if (job == null)
                return Result<JobPostDto>.Failure("Job not found");

            var dto = new JobPostDto
            {
                Id = job.Id,
                Title = job.Title,
                Description = job.Description,
                CompanyLocation = job.Location,
                Salary = job.Salary,
                PostedDate = job.PostedDate,
                EmployeerId = job.EmployerProfileId,
                JobPostStatus = job.JobPostStatus
            };

            return Result<JobPostDto>.Success(dto);
        }
        public async Task<Result<string>> SoftDeleteJobPostAsync(int employeerId, int jobPostId)
        {
            var job = await _context.JobPosts
                .FirstOrDefaultAsync(j => j.Id == jobPostId && !j.IsDeleted);

            if (job == null)
                return Result<string>.Failure("Job not found");

            if (job.EmployerProfileId != employeerId)
                return Result<string>.Failure("Not allowed");

            if (job.IsDeleted)
                return Result<string>.Failure("Job already deleted");

            job.IsDeleted = true;
            job.DeletedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Result<string>.Success("Job deleted successfully");
        }
        public async Task<Result<string>> CloseJobPostAsync(int employeerId, int jobPostId)
        {
            var job = await _context.JobPosts
                .FirstOrDefaultAsync(j => j.Id == jobPostId && !j.IsDeleted);

            if (job == null)
                return Result<string>.Failure("Job not found");

            if (job.EmployerProfileId != employeerId)
                return Result<string>.Failure("Not allowed");

            if (job.JobPostStatus == JobStatus.Closed)
                return Result<string>.Failure("Job already closed");

            job.JobPostStatus = JobStatus.Closed;
            job.ClosedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return Result<string>.Success("Job closed successfully");
        }
        public async Task<Result<JobPostDto>> CreateJobPostAsync(CreateJobPost request, int employerId)
        {
            var jobPost = new JobPost
            {
                Title = request.Title,
                Description = request.Description,
                Salary = request.Salary,
                EmployerProfileId = employerId,
                Location = request.CompanyLocation,
                PostedDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                JobPostStatus = request.JobStatus.HasValue ? (JobStatus)request.JobStatus.Value : JobStatus.Open
            };

            await _context.JobPosts.AddAsync(jobPost);
            await _context.SaveChangesAsync();

            var dto = new JobPostDto
            {
                Id = jobPost.Id,
                Title = jobPost.Title,
                Description = jobPost.Description,
                Salary = jobPost.Salary,
                CompanyLocation = jobPost.Location,
                PostedDate = jobPost.PostedDate,
                EmployeerId = jobPost.EmployerProfileId,
                JobPostStatus = jobPost.JobPostStatus
            };

            return Result<JobPostDto>.Success(dto, "Job created successfully");
        }
        public async Task<Result<JobPostDto>> UpdateJobPostAsync(int employeer, int jobPostId, EditJobPost request)
        {
            var jobPost = await _context.JobPosts.FirstOrDefaultAsync(x=>x.Id == jobPostId && x.JobPostStatus == JobStatus.Open);

            if (jobPost == null)
                return Result<JobPostDto>.Failure("Job post not found");

            if (jobPost.EmployerProfileId != employeer)
                return Result<JobPostDto>.Failure("You are not allowed to edit this job post");

            jobPost.Title = request.Title;
            jobPost.Description = request.Description;
            jobPost.Location = request.CompanyLocation;
            jobPost.Salary = request.Salary;
            jobPost.JobPostStatus = request.JobPostStatus.HasValue ? (JobStatus)request.JobPostStatus : JobStatus.Open;

            await _context.SaveChangesAsync();

            var dto = new JobPostDto
            {
                Id = jobPost.Id,
                Title = jobPost.Title,
                Description = jobPost.Description,
                Salary = jobPost.Salary,
                CompanyLocation = jobPost.Location,
                PostedDate = jobPost.PostedDate,
                EmployeerId = jobPost.EmployerProfileId,
                JobPostStatus = jobPost.JobPostStatus
            };

            return Result<JobPostDto>.Success(dto, "Job updated successfully");
        }
    }
}
