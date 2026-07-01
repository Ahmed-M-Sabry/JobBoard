using JobBoard.Application.Interfaces;
using JobBoard.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserRolesController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public UserRolesController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("become-seeker")]
        public async Task<IActionResult> BecomeSeeker()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            await _identityService.AddToRoleAsync(userId, AppRoles.Seeker);
            return Ok(new { Message = "You are now a Seeker" });
        }

        [HttpPost("become-employer")]
        public async Task<IActionResult> BecomeEmployer()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            await _identityService.AddToRoleAsync(userId, AppRoles.Employer);

            return Ok(new { Message = "You are now an Employer" });
        }
    }
}

