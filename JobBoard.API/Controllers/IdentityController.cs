using JobBoard.Application.Dtos.AuthenticationDtos.Login;
using JobBoard.Application.Dtos.AuthenticationDtos.Resgister;
using JobBoard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
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
                await .LogoutAsync(refreshToken);
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
