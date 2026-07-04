using JobBoard.Application.Dtos.JobApplication;
using JobBoard.Application.Dtos.JobPost;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Common;
using JobBoard.Infrastructure.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobPostController : ControllerBase
    {
        private readonly IJobPostService _jobPostService;
        private readonly IEmployerProfileService _employerProfileService;
       
        public JobPostController(IJobPostService jobPostService, IEmployerProfileService employerProfileService)
        {
            _jobPostService = jobPostService;
            _employerProfileService = employerProfileService;
        }
        [HttpGet("job/applications")]
        [Authorize(Roles = AppRoles.Employer)]
        public async Task<IActionResult> GetJobsAndApplicationsCount()
        {
            var employerId = await GetEmployerProfileId();

            if (employerId == null)
                return Unauthorized("Employer not found");

            var result = await _jobPostService
                .GetJobsAndApplicationsCountForEmployerAsync(employerId.Value);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Data);
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromBody] JobPostQueryParameters query)
        {
            var result = await _jobPostService.GetAllJobPostsAsync(query);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _jobPostService.GetJobPostByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result.Message);

            return Ok(result);
        }
        [HttpPost]
        [Authorize(Roles = AppRoles.Employer)]
        public async Task<IActionResult> Create([FromBody] CreateJobPost request)
        {
            var employerId = await GetEmployerProfileId();
            if (employerId == null)
                return Unauthorized("Employer not found");

            var result = await _jobPostService.CreateJobPostAsync(request, employerId.Value);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPut("{jobPostId}")]
        [Authorize(Roles = AppRoles.Employer)]
        public async Task<IActionResult> Update(int jobPostId, [FromBody] EditJobPost request)
        {
            var employerId = await GetEmployerProfileId();
            if (employerId == null)
                return Unauthorized("Employer not found");

            var result = await _jobPostService.UpdateJobPostAsync(employerId.Value, jobPostId, request);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpDelete("{jobPostId}")]
        [Authorize(Roles = AppRoles.Employer)]
        public async Task<IActionResult> SoftDelete(int jobPostId)
        {
            var employerId = await GetEmployerProfileId();
            if (employerId == null)
                return Unauthorized("Employer not found");

            var result = await _jobPostService.SoftDeleteJobPostAsync(employerId.Value, jobPostId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPatch("{jobPostId}/close")]
        [Authorize(Roles = AppRoles.Employer)]
        public async Task<IActionResult> Close(int jobPostId)
        {
            var employerId = await GetEmployerProfileId();
            if (employerId == null)
                return Unauthorized("Employer not found");

            var result = await _jobPostService.CloseJobPostAsync(employerId.Value, jobPostId);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
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

