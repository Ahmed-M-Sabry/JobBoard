using JobBoard.Application.Dtos.AuthenticationDtos.UserData;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SeekerProfileController : ControllerBase
    {
        private readonly ISeekerProfileService _seekerProfileService;
        public SeekerProfileController(ISeekerProfileService seekerProfileService)
        {
            _seekerProfileService = seekerProfileService;
        }

        [HttpGet("Profile")]
        [Authorize(Roles = AppRoles.Seeker)]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                    return Unauthorized();

                var profile = await _seekerProfileService.GetByUserIdAsync(userId);

                return Ok(profile);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Become-Seeker")]
        public async Task<IActionResult> CreateSeekerProfile([FromBody] CreateSeekerProfileDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized();
                }

                await _seekerProfileService.CreateAsync(userId, dto);

                return Ok(new { message = "Seeker profile created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
