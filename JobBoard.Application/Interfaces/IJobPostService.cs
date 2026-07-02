using JobBoard.Application.Common;
using JobBoard.Application.Dtos.JobPost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Interfaces
{
    public interface IJobPostService
    {
        Task<Result<JobPostDto>> CreateJobPostAsync(CreateJobPost request, int employerId);
        Task<Result<JobPostDto>> UpdateJobPostAsync(int employeer , int jobPostId, EditJobPost request);

        Task<Result<List<JobPostDto>>> GetAllJobPostsAsync();
        Task<Result<JobPostDto>> GetJobPostByIdAsync(int id);

        Task<Result<string>> SoftDeleteJobPostAsync(int employeerId, int jobPostId);
        Task<Result<string>> CloseJobPostAsync(int employeerId, int jobPostId);
    }
}
