using JobBoard.Application.Dtos.AuthenticationDtos.Login;
using JobBoard.Application.Dtos.AuthenticationDtos.Resgister;
using JobBoard.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace JobBoard.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {

        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public IdentityController(IAuthService authService 
            ,ITokenService tokenService)
        {
            _authService = authService;
            _tokenService= tokenService;
        }

        [HttpGet("Get-Me")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(userId == null) 
                    return Unauthorized(new { Message = "User is not authenticated" });
                
                var data = await _authService.GetUserData(userId);

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequest)
        {
            try
            {
                var userId = await _authService.RegisterAsync(registerRequest);
                return Ok(userId);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequest)
        {
            try
            {
                var response = await _authService.LoginAsync(loginRequest);
                SetRefreshTokenCookie(response.RefreshToken);

                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized();

            var token = await _authService.RefreshTokensAsync(refreshToken);
            if (token == null)
            {
                return Unauthorized(new { Message = "Invalid or expired refresh token" });
            }
            return Ok(token);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                await _tokenService.LogoutAsync(refreshToken);
            }

            Response.Cookies.Delete("refreshToken");

            return NoContent();
        }
        private void SetRefreshTokenCookie(string refreshToken)
        {
            Response.Cookies.Delete("refreshToken");

            Response.Cookies.Append(
                "refreshToken" ,
                refreshToken ,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTime.UtcNow.AddDays(7)
                });
        }
    }
}
