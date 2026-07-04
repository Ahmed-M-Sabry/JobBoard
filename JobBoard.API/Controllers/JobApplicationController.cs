using JobBoard.Application.Dtos.JobApplication;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Common;
using JobBoard.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationController : ControllerBase
    {
        private readonly IJobApplicationService _jobApplicationService;
        private readonly ISeekerProfileService _seekerProfileService;
        private readonly IEmployerProfileService _employerProfileService;

        public JobApplicationController(
            IJobApplicationService jobApplicationService,
            ISeekerProfileService seekerProfileService,
            IEmployerProfileService employerProfileService)
        {
            _jobApplicationService = jobApplicationService;
            _seekerProfileService = seekerProfileService;
            _employerProfileService = employerProfileService;
        }

        [HttpPost]
        [Authorize(Roles = AppRoles.Seeker)]
        public async Task<IActionResult> Apply([FromBody] CreateJobApplication request)
        {
            var seekerId = await GetSeekerProfileId();

            if (seekerId == null)
                return Unauthorized("Seeker not found");

            var result = await _jobApplicationService.ApplyAsync(seekerId.Value, request);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpPut("{applicationId}")]
        [Authorize(Roles = AppRoles.Seeker)]
        public async Task<IActionResult> Update(int applicationId, [FromBody] EditJobApplication request)
        {
            var seekerId = await GetSeekerProfileId();

            if (seekerId == null)
                return Unauthorized("Seeker not found");

            var result = await _jobApplicationService.UpdateAsync(seekerId.Value, applicationId, request);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("my-applications")]
        [Authorize(Roles = AppRoles.Seeker)]
        public async Task<IActionResult> GetMyApplications()
        {
            var seekerId = await GetSeekerProfileId();

            if (seekerId == null)
                return Unauthorized("Seeker not found");

            var result = await _jobApplicationService.GetJobApplicationsForSeekerAsync(seekerId.Value);

            return Ok(result.Data);
        }

        [HttpGet("{applicationId}/seeker")]
        [Authorize(Roles = AppRoles.Seeker)]
        public async Task<IActionResult> GetByIdForSeeker(int applicationId)
        {
            var seekerId = await GetSeekerProfileId();

            if (seekerId == null)
                return Unauthorized("Seeker not found");

            var result = await _jobApplicationService
                .GetByIdForSeekerAsync(seekerId.Value, applicationId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("job/{jobPostId}/applications")]
        [Authorize(Roles = AppRoles.Employer)]
        public async Task<IActionResult> GetApplicationsForJob(int jobPostId)
        {
            var employerId = await GetEmployerProfileId();

            if (employerId == null)
                return Unauthorized("Employer not found");

            var result = await _jobApplicationService
                .GetJobApplicationsForEmployerAsync(employerId.Value, jobPostId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }

        [HttpGet("{applicationId}/employer")]
        [Authorize(Roles = AppRoles.Employer)]
        public async Task<IActionResult> GetByIdForEmployer(int applicationId)
        {
            var employerId = await GetEmployerProfileId();

            if (employerId == null)
                return Unauthorized("Employer not found");

            var result = await _jobApplicationService
                .GetByIdForEmployerAsync(employerId.Value, applicationId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
        private async Task<int?> GetSeekerProfileId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return null;

            var seekerProfile = await _seekerProfileService.GetByUserIdAsync(userId);

            return seekerProfile?.Id;
        }
        [HttpPatch("{applicationId}/accept")]
        [Authorize(Roles = AppRoles.Employer)]
        public async Task<IActionResult> Accept(int applicationId)
        {
            var employerId = await GetEmployerProfileId();

            if (employerId == null)
                return Unauthorized();

            var result = await _jobApplicationService
                .ChangeStatusAsync(employerId.Value, applicationId, ApplicationStatus.Accepted);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
        [HttpPatch("{applicationId}/reject")]
        [Authorize(Roles = AppRoles.Employer)]
        public async Task<IActionResult> Reject(int applicationId)
        {
            var employerId = await GetEmployerProfileId();

            if (employerId == null)
                return Unauthorized();

            var result = await _jobApplicationService
                .ChangeStatusAsync(employerId.Value, applicationId, ApplicationStatus.Rejected);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
        private async Task<int?> GetEmployerProfileId()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
                return null;

            var employerProfile = await _employerProfileService.GetByUserIdAsync(userId);

            return employerProfile?.Id;
        }
    }
}