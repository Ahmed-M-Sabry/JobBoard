using JobBoard.Application.Dtos.AuthenticationDtos.UserData;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployerProfileController : ControllerBase
    {
        private readonly IEmployerProfileService _employerProfileService;

        public EmployerProfileController(IEmployerProfileService employerProfileService)
        {
            _employerProfileService = employerProfileService;
        }
        [HttpGet("Profile")]
        [Authorize(Roles = AppRoles.Employer)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                    return Unauthorized();

                var profile = await _employerProfileService.GetByUserIdAsync(userId);

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Become-Employer")]
        public async Task<IActionResult> CreateEmployerProfile([FromBody] CreateEmployeerProfileDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                    return Unauthorized();

                var result = await _employerProfileService.CreateAsync(userId, dto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}