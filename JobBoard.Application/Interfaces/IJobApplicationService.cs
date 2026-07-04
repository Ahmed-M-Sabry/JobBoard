using JobBoard.Application.Common;
using JobBoard.Application.Dtos.JobApplication;
using JobBoard.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Interfaces
{
    public interface IJobApplicationService
    {
        Task<Result<JobApplicationDto>> ApplyAsync(int seekerId, CreateJobApplication request);
        Task<Result<JobApplicationDto>> UpdateAsync(int seekerId, int applicationId, EditJobApplication request);

        Task<Result<List<EmployerJobApplicationDto>>> GetJobApplicationsForEmployerAsync(int employerId, int jobPostId);
        Task<Result<EmployerJobApplicationDto>> GetByIdForEmployerAsync(int employerId, int applicationId);

        Task<Result<List<JobApplicationDto>>> GetJobApplicationsForSeekerAsync(int seekerId);
        Task<Result<JobApplicationDto>> GetByIdForSeekerAsync(int seekerId, int applicationId);


        Task<Result<EmployerJobApplicationDto>> ChangeStatusAsync(
            int employerId,
            int applicationId,
            ApplicationStatus status);


    }
}
